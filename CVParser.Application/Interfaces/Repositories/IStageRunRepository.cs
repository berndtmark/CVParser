using CVParser.Domain.Entities;

namespace CVParser.Application.Interfaces.Repositories;

public interface IStageRunRepository
{
    /// <summary>Returns a tracked entity: change it, then call <c>IUnitOfWork.SaveChangesAsync</c>.</summary>
    Task<StageRun?> Get(Guid id, CancellationToken cancellationToken = default);
    Task<StageRun> Add(StageRun stageRun);
}
