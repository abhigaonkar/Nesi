using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class PurchaseOrderLineItemConfiguration : IEntityTypeConfiguration<PurchaseOrderLineItem>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderLineItem> builder)
    {
        builder.ToTable("PurchaseOrderLineItems");

        builder.HasKey(li => li.Id);

        builder.Property(li => li.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(li => li.PartNumber)
            .HasMaxLength(100);

        builder.Property(li => li.UnitOfMeasure)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(li => li.Quantity)
            .HasPrecision(18, 4);

        builder.Property(li => li.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(li => li.TotalPrice)
            .HasPrecision(18, 2);

        builder.Property(li => li.QuantityReceived)
            .HasPrecision(18, 4);

        builder.Property(li => li.Notes)
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(li => li.PurchaseOrder)
            .WithMany(po => po.LineItems)
            .HasForeignKey(li => li.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(li => li.PurchaseOrderId)
            .HasDatabaseName("IX_POLineItems_PurchaseOrderId");

        builder.HasIndex(li => li.LineNumber)
            .HasDatabaseName("IX_POLineItems_LineNumber");
    }
}
