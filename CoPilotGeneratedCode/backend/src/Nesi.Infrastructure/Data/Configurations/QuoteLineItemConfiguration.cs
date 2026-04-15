using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class QuoteLineItemConfiguration : IEntityTypeConfiguration<QuoteLineItem>
{
    public void Configure(EntityTypeBuilder<QuoteLineItem> builder)
    {
        builder.ToTable("QuoteLineItems");

        builder.HasKey(qli => qli.Id);

        builder.Property(qli => qli.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(qli => qli.PartNumber)
            .HasMaxLength(100);

        builder.Property(qli => qli.EstimatedHours)
            .HasColumnType("decimal(10,2)");

        builder.Property(qli => qli.Quantity)
            .HasColumnType("decimal(10,2)");

        builder.Property(qli => qli.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(qli => qli.Total)
            .HasColumnType("decimal(18,2)");

        builder.Property(qli => qli.Notes)
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(qli => qli.Quote)
            .WithMany(q => q.LineItems)
            .HasForeignKey(qli => qli.QuoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(qli => qli.JobType)
            .WithMany()
            .HasForeignKey(qli => qli.JobTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(qli => qli.QuoteId)
            .HasDatabaseName("IX_QuoteLineItems_QuoteId");

        builder.HasIndex(qli => new { qli.QuoteId, qli.LineNumber })
            .IsUnique()
            .HasDatabaseName("IX_QuoteLineItems_QuoteId_LineNumber");
    }
}
