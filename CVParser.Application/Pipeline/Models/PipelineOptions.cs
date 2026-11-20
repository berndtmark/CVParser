using CVParser.Application.Pipeline.Interfaces;

namespace CVParser.Application.Pipeline.Models;

/// <summary>Per-run configuration passed into the pipeline (e.g. from the UI).</summary>
public sealed class PipelineOptions : IPipelineOptions
{
    /// <summary>The file this run is processing.</summary>
    public required string FileName { get; init; }

    /// <summary>Names of stages to run (case-insensitive). Null = run all.</summary>
    public IReadOnlyCollection<string>? Stages { get; init; }

    public bool IsEnabled(string stage) =>
        Stages is null || Stages.Contains(stage, StringComparer.OrdinalIgnoreCase);
}
