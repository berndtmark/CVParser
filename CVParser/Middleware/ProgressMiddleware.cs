using CVParser.Application.Interfaces.UnitOfWork;
using CVParser.Domain.Entities;
using CVParser.Pipeline;

namespace CVParser.Middleware;

/// <summary>
/// Persists stage progress via <see cref="IUnitOfWork"/>. Uses a fresh DI scope per write so
/// concurrent pipeline runs never share a DbContext.
/// Register inside <see cref="ErrorLoggingMiddleware"/> (after it in the Use chain) so stage
/// exceptions are recorded as failures before being swallowed.
/// </summary>
public sealed class ProgressMiddleware(IServiceScopeFactory scopes) : IStageMiddleware
{
    public async Task InvokeAsync(IPipelineStage stage, IPipelineContext context, StageDelegate next, CancellationToken ct)
    {
        var stageRunId = await StartAsync(stage, context, ct);
        try
        {
            await next(context, ct);
        }
        catch (Exception ex)
        {
            // Don't pass the caller's token: we still want to record failure/cancellation.
            await FinishAsync(stageRunId, ex.Message, CancellationToken.None);
            throw;
        }

        await FinishAsync(stageRunId, null, ct);
    }

    private async Task<Guid> StartAsync(IPipelineStage stage, IPipelineContext context, CancellationToken ct)
    {
        await using var scope = scopes.CreateAsyncScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        if (await uow.PipelineRunRepository.Get(context.RunId, ct) is null)
        {
            await uow.PipelineRunRepository.Add(PipelineRun.Start(context.Options.FileName, context.RunId));
        }

        var stageRun = StageRun.Start(context.RunId, stage.Name);
        await uow.StageRunRepository.Add(stageRun);
        await uow.SaveChangesAsync(ct);

        return stageRun.Id;
    }

    private async Task FinishAsync(Guid stageRunId, string? error, CancellationToken ct)
    {
        await using var scope = scopes.CreateAsyncScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var stageRun = await uow.StageRunRepository.Get(stageRunId, ct);
        if (stageRun is null)
            return;

        if (error is null)
            stageRun.Complete();
        else
            stageRun.Fail(error);

        await uow.SaveChangesAsync(ct);
    }
}
