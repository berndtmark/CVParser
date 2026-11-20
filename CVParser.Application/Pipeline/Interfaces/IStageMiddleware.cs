namespace CVParser.Application.Pipeline.Interfaces;

public delegate Task StageDelegate(IPipelineContext context, CancellationToken ct);

/// <summary>Wraps the execution of each stage (logging, timing, retries...). Context-agnostic.</summary>
public interface IStageMiddleware
{
    Task InvokeAsync(IPipelineStage stage, IPipelineContext context, StageDelegate next, CancellationToken ct);
}
