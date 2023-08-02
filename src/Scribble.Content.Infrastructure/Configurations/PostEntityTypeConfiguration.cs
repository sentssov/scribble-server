using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Infrastructure.Configurations;

public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(e => e.Id).HasName("pk_posts_id");

        builder.ToTable("posts");

        builder.Property(e => e.Id)
            .HasConversion(
                value => value.Value,
                value => new PostId(value))
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("id");
        builder.Property(e => e.GroupId).HasColumnName("group_id");
        builder.Property(e => e.CreatedOnUtc)
            .HasDefaultValueSql("LOCALTIMESTAMP(0)")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_on_utc");
        builder.Property(e => e.ModifiedOnUtc)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("modified_on_utc");
        builder.Property(e => e.Title)
            .HasConversion(
                value => value.Value,
                value => PostTitle.Create(value).Value)
            .HasMaxLength(200)
            .HasColumnName("post_title");
        builder.Property(e => e.Content)
            .HasConversion(
                value => value.Value,
                value => PostContent.Create(value).Value)
            .HasColumnName("post_content");
        builder.Property(e => e.Description)
            .HasConversion(
                value => value.Value,
                value => PostDescription.Create(value).Value)
            .HasMaxLength(500)
            .HasColumnName("post_description");

        builder.HasOne(d => d.Group).WithMany(p => p.Posts)
            .HasForeignKey(d => d.GroupId)
            .HasConstraintName("fk_posts_group_id");

        builder.HasMany(d => d.Tags).WithMany(p => p.Posts)
            .UsingEntity<Dictionary<string, object>>(
                "PostsTag",
                r => r.HasOne<Tag>().WithMany()
                    .HasForeignKey("TagId")
                    .HasConstraintName("fk_posts_tags_tag_id"),
                l => l.HasOne<Post>().WithMany()
                    .HasForeignKey("PostId")
                    .HasConstraintName("fk_posts_tags_post_id"),
                j =>
                {
                    j.HasKey("PostId", "TagId").HasName("pk_posts_tags");
                    j.ToTable("posts_tags");
                    j.IndexerProperty<PostId>("PostId").HasColumnName("post_id");
                    j.IndexerProperty<TagId>("TagId").HasColumnName("tag_id");
                });
    }
}