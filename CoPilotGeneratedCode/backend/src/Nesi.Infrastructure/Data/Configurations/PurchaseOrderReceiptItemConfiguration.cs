using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class PurchaseOrderReceiptItemConfiguration : IEntityTypeConfiguration<PurchaseOrderReceiptItem>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderReceiptItem> builder)
    {
        builder.ToTable("PurchaseOrderReceiptItems");

        builder.HasKey(ri => ri.Id);

        builder.Property(ri => ri.QuantityReceived)
            .HasPrecision(18, 4);

        builder.Property(ri => ri.Condition)
            .HasMaxLength(100);

        builder.Property(ri => ri.Notes)
            .HasMaxLength(1000);

        builder.Property(ri => ri.DiscrepancyReason)
            .HasMaxLength(500);

        // Relationships
        builder.HasOne(ri => ri.Receipt)
            .WithMany(r => r.ReceiptItems)
            .HasForeignKey(ri => ri.PurchaseOrderReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ri => ri.LineItem)
            .WithMany()
            .HasForeignKey(ri => ri.PurchaseOrderLineItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(ri => ri.PurchaseOrderReceiptId)
            .HasDatabaseName("IX_POReceiptItems_ReceiptId");

        builder.HasIndex(ri => ri.PurchaseOrderLineItemId)
            .HasDatabaseName("IX_POReceiptItems_LineItemId");
    }
}
