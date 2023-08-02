using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Infrastructure.Configurations;

public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(e => e.Id).HasName("pk_comments_id");

        builder.ToTable("comments");

        builder.Property(e => e.Id)
            .HasConversion(
                value => value.Value,
                value => new CommentId(value))
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("id");
        builder.Property(e => e.UserId)
            .HasConversion(
                value => value.Value,
                value => new UserId(value))
            .HasColumnName("user_id");
        builder.Property(e => e.Text)
            .HasConversion(
                value => value.Value,
                value => CommentText.Create(value).Value)
            .HasColumnName("comment_text");
        builder.Property(e => e.CreatedOnUtc)
            .HasDefaultValueSql("LOCALTIMESTAMP(0)")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_on_utc");
        builder.Property(e => e.ModifiedOnUtc)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("modified_on_utc");
        builder.Property(e => e.PostId)
            .HasConversion(
                value => value.Value,
                value => new PostId(value))
            .HasColumnName("post_id");

        builder.HasOne(d => d.User).WithMany(p => p.Comments)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_comments_user_id");

        builder.HasOne(d => d.Post).WithMany(p => p.Comments)
            .HasForeignKey(d => d.PostId)
            .HasConstraintName("fk_comments_post_id");
    }
}