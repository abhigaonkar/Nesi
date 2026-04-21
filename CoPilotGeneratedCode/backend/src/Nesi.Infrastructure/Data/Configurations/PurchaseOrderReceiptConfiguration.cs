using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class PurchaseOrderReceiptConfiguration : IEntityTypeConfiguration<PurchaseOrderReceipt>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderReceipt> builder)
    {
        builder.ToTable("PurchaseOrderReceipts");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.ReceiptNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.PackingSlipNumber)
            .HasMaxLength(50);

        builder.Property(r => r.Notes)
            .HasMaxLength(2000);

        builder.Property(r => r.Status)
            .IsRequired();

        // Relationships
        builder.HasOne(r => r.PurchaseOrder)
            .WithMany(po => po.Receipts)
            .HasForeignKey(r => r.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Receiver)
            .WithMany()
            .HasForeignKey(r => r.ReceivedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.ReceiptItems)
            .WithOne(ri => ri.Receipt)
            .HasForeignKey(ri => ri.PurchaseOrderReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(r => r.ReceiptNumber)
            .IsUnique()
            .HasDatabaseName("IX_POReceipts_ReceiptNumber");

        builder.HasIndex(r => r.PurchaseOrderId)
            .HasDatabaseName("IX_POReceipts_PurchaseOrderId");

        builder.HasIndex(r => r.ReceivedDate)
            .HasDatabaseName("IX_POReceipts_ReceivedDate");
    }
}
