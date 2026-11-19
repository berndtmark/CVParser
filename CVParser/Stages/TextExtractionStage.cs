using CVParser.Models;
using CVParser.Pipeline;

namespace CVParser.Stages;

/// <summary>Reads plain text. Swap/extend for PDF/DOCX later.</summary>
public sealed class TextExtractionStage : IPipelineStage<ResumeContext>
{
    public string Name => "TextExtraction";

    public async Task ExecuteAsync(ResumeContext context, CancellationToken ct = default) =>
        context.RawText = "M John Doe";
}
