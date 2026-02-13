using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Contracts.Models;

namespace InventoryService.Infrastructure.Database.Configurations;

public class ProcessedMessageConfiguration : IEntityTypeConfiguration<ProcessedMessage>
{
    public void Configure(EntityTypeBuilder<ProcessedMessage> entity)
    {
        entity.ToTable("processed_message");

        entity.HasKey(message => message.MessageId);

        entity.Property(message => message.MessageId)
            .HasColumnName("message_id");

        entity.Property(message => message.MessageType)
            .HasColumnName("message_type")
            .IsRequired();

        entity.Property(message => message.OccurredAt)
            .HasColumnName("occurred_at")
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        entity.Property(message => message.ProcessedAt)
            .HasColumnName("processed_at")
            .HasDefaultValueSql("NOW()")
            .IsRequired();
    }
}