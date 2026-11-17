using Parser.Models;

namespace Parser.Pipeline;

public interface IPipelineStage
{
    string Name { get; }
    Task ExecuteAsync(ResumeContext context, CancellationToken ct = default);
}
