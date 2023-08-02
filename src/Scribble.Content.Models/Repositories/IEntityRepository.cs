
using System.Linq.Expressions;
using Scribble.Content.Models.Primitives;

namespace Scribble.Content.Models.Repositories;

public interface IEntityRepository
{
    IQueryable<TEntity> Query<TEntity, TKey>()
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>;
    
    Task<TEntity?> GetAsync<TEntity, TKey>(TKey key, CancellationToken token = default)
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>;
    
    Task<TEntity?> GetAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate, CancellationToken token)
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>;

    Task<bool> UniqueByAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>;

    Task<bool> ExistsAsync<TEntity, TKey>(TKey key, CancellationToken token = default)
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>;
    
    Task<long> CountAsync<TEntity, TKey>(CancellationToken token = default)
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>;
    
    Task<long> CountAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>;

    Task<TKey> SaveAsync<TEntity, TKey>(TEntity entity, CancellationToken token = default)
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>;

    Task RemoveAsync<TEntity, TKey>(TEntity entity, CancellationToken token = default)
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>;
    
    Task RemoveAsync<TEntity, TKey>(TKey key, CancellationToken token = default)
        where TEntity : Entity<TKey> where TKey : IEquatable<TKey>;
}