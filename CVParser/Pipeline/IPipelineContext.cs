namespace CVParser.Pipeline;

/// <summary>
/// Minimum a context type must expose to run through a pipeline.
/// The pipeline creates the context itself and sets <see cref="Options"/>; stages fill in the rest.
/// </summary>
public interface IPipelineContext
{
    IPipelineOptions Options { get; init; }
}
