using CVParser.Domain.Entities;
using CVParser.Infrastructure.Database.Context;

namespace CVParser.Infrastructure.Database.Repositories;

public abstract class BaseRepository<TEntity>(DataBaseContext dbContext) where TEntity : EntityBase
{
    protected DataBaseContext DbContext => dbContext;

    protected virtual async Task<TEntity> AddEntity(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(entity, cancellationToken);
        return entity;
    }
}
