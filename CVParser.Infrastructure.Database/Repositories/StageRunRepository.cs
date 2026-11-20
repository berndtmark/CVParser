using CVParser.Application.Interfaces.Repositories;
using CVParser.Domain.Entities;
using CVParser.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace CVParser.Infrastructure.Database.Repositories;

public class StageRunRepository(DataBaseContext dbContext) : BaseRepository<StageRun>(dbContext), IStageRunRepository
{
    public async Task<StageRun?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.StageRun
            .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<StageRun> Add(StageRun stageRun)
    {
        return await AddEntity(stageRun);
    }
}
