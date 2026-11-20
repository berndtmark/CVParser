using CVParser.Pipeline;

namespace CVParser.Models;

/// <summary>Shared state passed through every pipeline stage.</summary>
public sealed class ResumeContext : IPipelineContext
{
    public Guid RunId { get; init; }
    public IPipelineOptions Options { get; init; } = null!; // always set by Pipeline<T>.RunAsync

    public string? RawText { get; set; }
    public Dictionary<string, string> Sections { get; } = new(StringComparer.OrdinalIgnoreCase);
    public ContactInfo? Contact { get; set; }

    public List<string> Warnings { get; } = [];
}
