using CVParser.Application.Pipeline.Interfaces;
using CVParser.Application.Pipeline.Models;

namespace CVParser.Application.Stages;

/// <summary>Reads plain text. Swap/extend for PDF/DOCX later.</summary>
public sealed class TextExtractionStage : IPipelineStage<ResumeContext>
{
    public string Name => "TextExtraction";

    public async Task ExecuteAsync(ResumeContext context, CancellationToken ct = default) =>
        context.RawText = "M John Doe";
}
