using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Infrastructure.Configurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id).HasName("pk_users_id");

        builder.ToTable("users");

        builder.Property(e => e.Id)
            .HasConversion(
                value => value.Value,
                value => new UserId(value))
            .ValueGeneratedNever()
            .HasColumnName("id");
        
        builder.HasMany(d => d.Groups).WithOne(p => p.User)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_users_group_id");
        
        builder.HasMany(d => d.Comments).WithOne(p => p.User)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_users_comment_id");
        
        builder.HasMany(d => d.Categories).WithOne(p => p.User)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_users_category_id");
        
        builder.HasMany(d => d.Likes).WithOne(p => p.User)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_users_like_id");
        
        builder.HasMany(d => d.Tags).WithOne(p => p.User)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_users_tag_id");
    }
}