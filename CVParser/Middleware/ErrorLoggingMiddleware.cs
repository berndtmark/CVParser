using Microsoft.Extensions.Logging;
using CVParser.Pipeline;

namespace CVParser.Middleware;

/// <summary>Logs any exception thrown by a stage and swallows it, so the pipeline carries on with the next stage.</summary>
public sealed class ErrorLoggingMiddleware(ILogger<ErrorLoggingMiddleware> log) : IStageMiddleware
{
    public async Task InvokeAsync(IPipelineStage stage, IPipelineContext context, StageDelegate next, CancellationToken ct)
    {
        try
        {
            await next(context, ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw; // shutdown, not a stage failure
        }
        catch (Exception ex)
        {
            log.LogError(ex, "{Stage} failed", stage.Name);
        }
    }
}
