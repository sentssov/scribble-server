using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Infrastructure.Configurations;

public class LikeEntityTypeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasKey(e => e.Id).HasName("pk_like_id");

        builder.ToTable("likes");
        
        builder.Property(e => e.Id)
            .HasConversion(
                value => value.Value,
                value => new LikeId(value))
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("id");

        builder.Property(e => e.UserId)
            .HasConversion(
                value => value.Value,
                value => new UserId(value))
            .HasColumnName("user_id");
        
        builder.Property(e => e.PostId)
            .HasConversion(
                value => value.Value,
                value => new PostId(value))
            .HasColumnName("post_id");
    }
}