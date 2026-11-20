using CVParser.Application.Interfaces.UnitOfWork;
using CVParser.Application.Pipeline.Interfaces;
using CVParser.Domain.Entities;

namespace CVParser.Application.Middleware;

/// <summary>
/// Persists stage progress via <see cref="IUnitOfWork"/>.
/// Register inside <see cref="ErrorLoggingMiddleware"/> (after it in the Use chain) so stage
/// exceptions are recorded as failures before being swallowed.
/// </summary>
public sealed class ProgressMiddleware(IUnitOfWork uow) : IStageMiddleware
{
    public async Task InvokeAsync(IPipelineStage stage, IPipelineContext context, StageDelegate next, CancellationToken ct)
    {
        if (await uow.PipelineRunRepository.Get(context.RunId, ct) is null)
        {
            await uow.PipelineRunRepository.Add(PipelineRun.Start(context.Options.FileName, context.RunId));
        }

        var stageRun = StageRun.Start(context.RunId, stage.Name);
        await uow.StageRunRepository.Add(stageRun);
        await uow.SaveChangesAsync(ct);

        try
        {
            await next(context, ct);
        }
        catch (Exception ex)
        {
            // Don't pass the caller's token: we still want to record failure/cancellation.
            stageRun.Fail(ex.Message);
            await uow.SaveChangesAsync(CancellationToken.None);
            throw;
        }

        stageRun.Complete();
        await uow.SaveChangesAsync(ct);
    }
}
