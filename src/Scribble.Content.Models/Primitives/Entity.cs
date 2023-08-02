namespace Scribble.Content.Models.Primitives;

public abstract class Entity<TKey> : IEquatable<Entity<TKey>>
 where TKey : IEquatable<TKey>
{
    protected Entity() { }
    protected Entity(TKey id)
    {
        Id = id;
    }

    public TKey Id { get; protected set; } = default!;

    public static bool operator ==(Entity<TKey>? first, Entity<TKey>? second)
    {
        return first is not null && second is not null && first.Id.Equals(second.Id);
    }

    public static bool operator !=(Entity<TKey>? first, Entity<TKey>? second)
    {
        return !(first == second);
    }
    
    public bool Equals(Entity<TKey>? other)
    {
        return other != null && other.Id.Equals(Id);
    }
    
    public override bool Equals(object? obj)
    {
        return obj is Entity<TKey> entity && entity.Id.Equals(Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }
}