using Microsoft.Extensions.DependencyInjection;
using Parser;
using Parser.Models;
using Parser.Middleware;
using Parser.Pipeline;
using Parser.Stages;

if (args.Length == 0)
{
    Console.Error.WriteLine("Usage: Parser <resume.txt>");
    return 1;
}

await using var provider = new ServiceCollection()
    .AddResumeParser()
    .BuildServiceProvider();

ResumePipeline pipeline = new ResumePipelineBuilder(provider)
    .Use<LoggingMiddleware>()
    .Add<TextExtractionStage>()
    .Add<SectionSplitStage>()
    .Add<ContactExtractionStage>();

// Optional 2nd arg: comma-separated stage names to run (default: all)
var options = new PipelineOptions
{
    Stages = args.Length > 1
        ? args[1].Split(',', StringSplitOptions.TrimEntries).Select(s => Enum.Parse<Stage>(s, ignoreCase: true)).ToArray()
        : null
};
var result = await pipeline.RunAsync(args[0], options);

Console.WriteLine($"Name:  {result.Contact?.Name}");
Console.WriteLine($"Email: {result.Contact?.Email}");
Console.WriteLine($"Phone: {result.Contact?.Phone}");
Console.WriteLine($"Sections: {string.Join(", ", result.Sections.Keys)}");
foreach (var w in result.Warnings) Console.WriteLine($"! {w}");
return 0;
