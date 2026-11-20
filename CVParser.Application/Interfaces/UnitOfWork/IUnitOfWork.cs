using CVParser.Application.Interfaces.Repositories;

namespace CVParser.Application.Interfaces.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IPipelineRunRepository PipelineRunRepository { get; }
    IStageRunRepository StageRunRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
