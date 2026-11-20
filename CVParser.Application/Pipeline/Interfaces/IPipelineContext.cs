namespace CVParser.Application.Pipeline.Interfaces;

/// <summary>
/// Minimum a context type must expose to run through a pipeline.
/// The pipeline creates the context itself and sets <see cref="Options"/>; stages fill in the rest.
/// </summary>
public interface IPipelineContext
{
    /// <summary>Unique id of this run; set by the pipeline.</summary>
    Guid RunId { get; init; }

    IPipelineOptions Options { get; init; }
}
