using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Infrastructure.Repositories;

public class EntityRepository : IEntityRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EntityRepository(ApplicationDbContext dbContext) => 
        _dbContext = dbContext;

    public IQueryable<TEntity> Query<TEntity, TKey>() 
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    {
        return _dbContext.Set<TEntity>().AsQueryable();
    }

    public async Task<TEntity?> GetAsync<TEntity, TKey>(TKey key, CancellationToken token = default) 
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    {
        return await _dbContext.Set<TEntity>()
            .FindAsync(new object?[] { key }, token)
            .ConfigureAwait(false);
    }

    public async Task<TEntity?> GetAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate, CancellationToken token) where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    {
        return await _dbContext.Set<TEntity>()
            .FirstOrDefaultAsync(predicate, token)
            .ConfigureAwait(false);
    }

    public async Task<bool> UniqueByAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) 
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    {
        return await _dbContext.Set<TEntity>()
            .AnyAsync(predicate, token)
            .ConfigureAwait(false);
    }

    public async Task<bool> ExistsAsync<TEntity, TKey>(TKey key, CancellationToken token)
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    {
        var entity = await _dbContext.Set<TEntity>()
            .FindAsync(new object?[] { key }, token)
            .ConfigureAwait(false);

        return entity is not null;
    }

    public async Task<long> CountAsync<TEntity, TKey>(CancellationToken token = default)
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    {
        return await _dbContext.Set<TEntity>()
            .LongCountAsync(token)
            .ConfigureAwait(false);
    }

    public async Task<long> CountAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate, CancellationToken token)
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    {
        return await _dbContext.Set<TEntity>()
            .LongCountAsync(predicate, token)
            .ConfigureAwait(false);
    }

    public Task<TKey> SaveAsync<TEntity, TKey>(TEntity entity, CancellationToken token) 
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    {
        _dbContext.Set<TEntity>()
            .Add(entity);

        return Task.FromResult(entity.Id);
    }

    public Task RemoveAsync<TEntity, TKey>(TEntity entity, CancellationToken token) 
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    {
        _dbContext.Set<TEntity>()
            .Remove(entity);

        return Task.CompletedTask;
    }

    public async Task RemoveAsync<TEntity, TKey>(TKey key, CancellationToken token) 
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    {
        var entity = await _dbContext.Set<TEntity>()
            .FindAsync(new object?[] { key }, token)
            .ConfigureAwait(false);

        if (entity is null)
            return;

        _dbContext.Set<TEntity>()
            .Remove(entity);
    }
}