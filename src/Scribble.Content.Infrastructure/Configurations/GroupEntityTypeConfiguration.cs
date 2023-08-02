using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Infrastructure.Configurations;

public class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(e => e.Id).HasName("pk_groups_id");

        builder.ToTable("groups");
        
        builder.Property(e => e.Id)
            .HasConversion(
                value => value.Value,
                value => new GroupId(value))
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("id");
        builder.Property(e => e.UserId)
            .HasConversion(
                value => value.Value,
                value => new UserId(value))
            .HasColumnName("user_id");
        builder.Property(e => e.Name)
            .HasConversion(
                value => value.Value,
                value => GroupName.Create(value, true).Value)
            .HasMaxLength(75)
            .HasColumnName("group_name");
        builder.Property(e => e.ShortName)
            .HasConversion(
                value => value.Value,
                value => GroupShortName.Create(value, true).Value)
            .HasMaxLength(45)
            .HasColumnName("group_short_name");
        builder.Property(e => e.Description)
            .HasConversion(
                description => description.Value,
                value => GroupDescription.Create(value).Value)
            .HasMaxLength(400)
            .HasColumnName("group_description");
        builder.Property(e => e.CreatedOnUtc)
            .HasDefaultValueSql("LOCALTIMESTAMP(0)")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("created_on_utc");
        builder.Property(e => e.ModifiedOnUtc)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("modified_on_utc");

        builder.HasOne(d => d.User).WithMany(p => p.Groups)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_groups_user_id");

        builder.HasMany(d => d.Categories).WithMany(p => p.Groups)
            .UsingEntity<Dictionary<string, object>>(
                "GroupsCategory",
                r => r.HasOne<Category>().WithMany()
                    .HasForeignKey("CategoryId")
                    .HasConstraintName("fk_groups_categories_category_id"),
                l => l.HasOne<Group>().WithMany()
                    .HasForeignKey("GroupId")
                    .HasConstraintName("fk_groups_categories_group_id"),
                j =>
                {
                    j.HasKey("GroupId", "CategoryId").HasName("pk_groups_categories");
                    j.ToTable("groups_categories");
                    j.IndexerProperty<GroupId>("GroupId").HasColumnName("group_id");
                    j.IndexerProperty<CategoryId>("CategoryId").HasColumnName("category_id");
                });
    }
}