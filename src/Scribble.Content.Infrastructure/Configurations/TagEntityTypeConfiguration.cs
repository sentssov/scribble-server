using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Infrastructure.Configurations;

public class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(e => e.Id).HasName("pk_tags_id");

        builder.ToTable("tags");

        builder.HasIndex(e => e.Name, "tags_tag_name_key").IsUnique();

        builder.Property(e => e.Id)
            .HasConversion(
                value => value.Value,
                value => new TagId(value))
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("id");
        builder.Property(e => e.Name)
            .HasConversion(
                value => value.Value,
                value => TagName.Create(value, true).Value)
            .HasMaxLength(50)
            .HasColumnName("tag_name");
        builder.Property(e => e.UserId)
            .HasConversion(
                value => value.Value,
                value => new UserId(value))
            .HasColumnName("user_id");

        builder.HasOne(d => d.User).WithMany(p => p.Tags)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_tags_user_id");
    }
}