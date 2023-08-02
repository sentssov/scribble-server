using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Infrastructure.Configurations;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(e => e.Id).HasName("pk_categories_id");

        builder.ToTable("categories");

        builder.HasIndex(e => e.Name, "categories_category_name_key").IsUnique();

        builder.Property(e => e.Id)
            .HasConversion(
                value => value.Value,
                value => new CategoryId(value))
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("id");
        builder.Property(e => e.Name)
            .HasConversion(
                value => value.Value,
                value => CategoryName.Create(value, true).Value)
            .HasMaxLength(50)
            .HasColumnName("category_name");
        builder.Property(e => e.UserId)
            .HasConversion(
                value => value.Value,
                value => new UserId(value))
            .HasColumnName("user_id");
        
    }
}