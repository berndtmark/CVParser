using CVParser.Domain.Entities;

namespace CVParser.Application.Interfaces.Repositories;

public interface IPipelineRunRepository
{
    Task<PipelineRun?> Get(Guid id, CancellationToken cancellationToken = default);
    Task<PipelineRun> Add(PipelineRun pipelineRun);
}
