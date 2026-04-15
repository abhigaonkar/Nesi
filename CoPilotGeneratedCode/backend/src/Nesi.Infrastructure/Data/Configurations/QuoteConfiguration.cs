using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        builder.ToTable("Quotes");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.QuoteNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(q => q.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(q => q.Scope)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(q => q.TermsAndConditions)
            .HasMaxLength(4000);

        builder.Property(q => q.Subtotal)
            .HasColumnType("decimal(18,2)");

        builder.Property(q => q.TaxRate)
            .HasColumnType("decimal(5,2)");

        builder.Property(q => q.TaxAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(q => q.DiscountPercent)
            .HasColumnType("decimal(5,2)");

        builder.Property(q => q.DiscountAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(q => q.Total)
            .HasColumnType("decimal(18,2)");

        builder.Property(q => q.RejectionReason)
            .HasMaxLength(500);

        builder.Property(q => q.CustomerApprovedBy)
            .HasMaxLength(200);

        // Relationships
        builder.HasOne(q => q.Customer)
            .WithMany(c => c.Quotes)
            .HasForeignKey(q => q.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(q => q.ProjectManager)
            .WithMany()
            .HasForeignKey(q => q.ProjectManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(q => q.Approver)
            .WithMany()
            .HasForeignKey(q => q.ApprovedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-one relationship with WorkOrder
        // Quote is the dependent side (has the foreign key WorkOrderId)
        builder.HasOne(q => q.WorkOrder)
            .WithOne(w => w.Quote)
            .HasForeignKey<Quote>(q => q.WorkOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(q => q.ParentQuote)
            .WithMany()
            .HasForeignKey(q => q.ParentQuoteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(q => q.LineItems)
            .WithOne(li => li.Quote)
            .HasForeignKey(li => li.QuoteId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(q => q.QuoteNumber)
            .IsUnique()
            .HasDatabaseName("IX_Quotes_QuoteNumber");

        builder.HasIndex(q => q.CustomerId)
            .HasDatabaseName("IX_Quotes_CustomerId");

        builder.HasIndex(q => q.Status)
            .HasDatabaseName("IX_Quotes_Status");

        builder.HasIndex(q => q.WorkOrderId)
            .HasDatabaseName("IX_Quotes_WorkOrderId");
    }
}
