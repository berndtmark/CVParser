namespace Parser.Models;

/// <summary>Shared state passed through every pipeline stage.</summary>
public sealed class ResumeContext
{
    public required string SourcePath { get; init; }
    public PipelineOptions Options { get; init; } = new();

    public string? RawText { get; set; }
    public Dictionary<string, string> Sections { get; } = new(StringComparer.OrdinalIgnoreCase);
    public ContactInfo? Contact { get; set; }

    public List<string> Warnings { get; } = [];
}
