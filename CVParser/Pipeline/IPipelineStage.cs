using CVParser.Enums;
using CVParser.Models;

namespace CVParser.Pipeline;

public interface IPipelineStage
{
    Stage Name { get; }
    Task ExecuteAsync(ResumeContext context, CancellationToken ct = default);
}
