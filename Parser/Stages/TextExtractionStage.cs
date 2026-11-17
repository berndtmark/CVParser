using Parser.Models;
using Parser.Pipeline;

namespace Parser.Stages;

/// <summary>Reads plain text. Swap/extend for PDF/DOCX later.</summary>
public sealed class TextExtractionStage : IPipelineStage
{
    public Stage Name => Stage.TextExtraction;

    public async Task ExecuteAsync(ResumeContext context, CancellationToken ct = default) =>
        context.RawText = await File.ReadAllTextAsync(context.SourcePath, ct);
}
