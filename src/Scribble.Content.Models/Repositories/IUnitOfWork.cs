namespace Scribble.Content.Models.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken token);
}