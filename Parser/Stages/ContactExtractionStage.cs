using System.Text.RegularExpressions;
using Parser.Models;
using Parser.Pipeline;

namespace Parser.Stages;

public sealed partial class ContactExtractionStage : IPipelineStage
{
    public string Name => "ContactExtraction";

    [GeneratedRegex(@"[\w.+-]+@[\w-]+\.[\w.-]+")]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"\+?\d[\d\s().-]{7,}\d")]
    private static partial Regex PhoneRegex();

    public Task ExecuteAsync(ResumeContext context, CancellationToken ct = default)
    {
        var header = context.Sections.GetValueOrDefault("Header") ?? context.RawText ?? "";
        var name = header.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                         .FirstOrDefault();
        var email = EmailRegex().Match(header);
        var phone = PhoneRegex().Match(header);

        context.Contact = new ContactInfo(
            name,
            email.Success ? email.Value : null,
            phone.Success ? phone.Value : null);
        return Task.CompletedTask;
    }
}
