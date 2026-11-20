using CVParser.Application.Interfaces.Repositories;
using CVParser.Application.Interfaces.UnitOfWork;
using CVParser.Infrastructure.Database.Context;
using CVParser.Infrastructure.Database.Repositories;

namespace CVParser.Infrastructure.Database.UnitOfWork;

public sealed class UnitOfWork(DataBaseContext context) : IUnitOfWork
{
    private IPipelineRunRepository? _pipelineRunRepository;
    private IStageRunRepository? _stageRunRepository;

    public IPipelineRunRepository PipelineRunRepository
    {
        get
        {
            _pipelineRunRepository ??= new PipelineRunRepository(context);
            return _pipelineRunRepository;
        }
    }

    public IStageRunRepository StageRunRepository
    {
        get
        {
            _stageRunRepository ??= new StageRunRepository(context);
            return _stageRunRepository;
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
