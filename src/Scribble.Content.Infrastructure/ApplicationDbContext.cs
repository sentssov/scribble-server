using Microsoft.EntityFrameworkCore;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Infrastructure;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<Group> Groups { get; set; } = null!;

    public virtual DbSet<Category> Categories { get; set; } = null!;

    public virtual DbSet<Comment> Comments { get; set; } = null!;

    public virtual DbSet<Post> Posts { get; set; } = null!;

    public virtual DbSet<Tag> Tags { get; set; } = null!;
    public virtual DbSet<Subscription> Followers { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}
