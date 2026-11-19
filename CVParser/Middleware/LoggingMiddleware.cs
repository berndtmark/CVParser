using System.Diagnostics;
using Microsoft.Extensions.Logging;
using CVParser.Pipeline;

namespace CVParser.Middleware;

public sealed class LoggingMiddleware(ILogger<LoggingMiddleware> log) : IStageMiddleware
{
    public async Task InvokeAsync(IPipelineStage stage, IPipelineContext context, StageDelegate next, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        log.LogInformation("{Stage} starting", stage.Name);
        try
        {
            await next(context, ct);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "{Stage} failed after {Ms}ms", stage.Name, sw.ElapsedMilliseconds);
            throw;
        }
        log.LogInformation("{Stage} done in {Ms}ms", stage.Name, sw.ElapsedMilliseconds);
    }
}
