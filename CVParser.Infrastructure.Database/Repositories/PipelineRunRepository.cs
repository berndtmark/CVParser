using CVParser.Application.Interfaces.Repositories;
using CVParser.Domain.Entities;
using CVParser.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace CVParser.Infrastructure.Database.Repositories;

public class PipelineRunRepository(DataBaseContext dbContext) : BaseRepository<PipelineRun>(dbContext), IPipelineRunRepository
{
    public async Task<PipelineRun?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.PipelineRun
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<PipelineRun> Add(PipelineRun pipelineRun)
    {
        return await AddEntity(pipelineRun);
    }
}
