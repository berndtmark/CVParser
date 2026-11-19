using CVParser.Enums;

namespace CVParser.Pipeline;

/// <summary>Non-generic view of a stage, so middleware doesn't need to know the context type.</summary>
public interface IPipelineStage
{
    Stage Name { get; }
}

public interface IPipelineStage<in TContext> : IPipelineStage where TContext : IPipelineContext
{
    Task ExecuteAsync(TContext context, CancellationToken ct = default);
}
