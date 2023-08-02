
namespace Scribble.Content.Models.Primitives;

public abstract class AggregateRoot<TKey> : Entity<TKey> 
    where TKey : IEquatable<TKey>
{
    protected AggregateRoot(TKey id) 
        : base(id)
    {
    }
}