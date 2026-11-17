using Parser.Models;
using Parser.Stages;

namespace Parser.Pipeline;

public interface IPipelineStage
{
    Stage Name { get; }
    Task ExecuteAsync(ResumeContext context, CancellationToken ct = default);
}
