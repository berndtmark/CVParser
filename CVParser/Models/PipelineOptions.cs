using CVParser.Enums;

namespace CVParser.Models;

/// <summary>Per-run configuration passed into the pipeline (e.g. from the UI).</summary>
public sealed class PipelineOptions
{
    /// <summary>Stages to run. Null = run all.</summary>
    public IReadOnlyCollection<Stage>? Stages { get; init; }

    public bool IsEnabled(Stage stage) => Stages is null || Stages.Contains(stage);
}
