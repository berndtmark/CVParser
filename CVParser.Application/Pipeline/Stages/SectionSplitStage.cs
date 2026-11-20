using System.Text.RegularExpressions;
using CVParser.Application.Pipeline.Interfaces;
using CVParser.Application.Pipeline.Models;

namespace CVParser.Application.Stages;

/// <summary>Splits text into sections by common headings (Experience, Education, Skills...).</summary>
public sealed partial class SectionSplitStage : IPipelineStage<ResumeContext>
{
    public string Name => "SectionSplit";

    [GeneratedRegex(@"^\s*(summary|objective|experience|work experience|education|skills|projects|certifications)\s*:?\s*$",
        RegexOptions.IgnoreCase)]
    private static partial Regex HeadingRegex();

    public Task ExecuteAsync(ResumeContext context, CancellationToken ct = default)
    {
        if (context.RawText is null)
        {
            context.Warnings.Add($"{Name}: no text to split.");
            return Task.CompletedTask;
        }

        string current = "Header";
        var buffer = new List<string>();

        void Flush()
        {
            if (buffer.Count > 0)
                context.Sections[current] = string.Join(Environment.NewLine, buffer).Trim();
            buffer.Clear();
        }

        foreach (var line in context.RawText.Split('\n'))
        {
            var m = HeadingRegex().Match(line.TrimEnd('\r'));
            if (m.Success) { Flush(); current = m.Groups[1].Value; }
            else buffer.Add(line.TrimEnd('\r'));
        }
        Flush();
        return Task.CompletedTask;
    }
}
