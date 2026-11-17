using Parser.Models;

namespace Parser.Pipeline;

public delegate Task StageDelegate(ResumeContext context, CancellationToken ct);

/// <summary>Wraps the execution of each stage (logging, timing, retries...).</summary>
public interface IStageMiddleware
{
    Task InvokeAsync(IPipelineStage stage, ResumeContext context, StageDelegate next, CancellationToken ct);
}
