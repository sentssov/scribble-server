using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Infrastructure.Configurations;

public class SubscriptionEntityTypeConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(e => e.Id).HasName("pk_subscription_id");

        builder.ToTable("subscriptions");
        
        builder.Property(e => e.Id)
            .HasConversion(
                value => value.Value,
                value => new SubscriptionId(value))
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("id");

        builder.Property(e => e.UserId)
            .HasConversion(
                value => value.Value,
                value => new UserId(value))
            .HasColumnName("user_id");
        
        builder.Property(e => e.GroupId)
            .HasConversion(
                value => value.Value,
                value => new GroupId(value))
            .HasColumnName("group_id");
    }
}