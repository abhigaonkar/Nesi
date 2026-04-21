using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrders");

        builder.HasKey(po => po.Id);

        builder.Property(po => po.PurchaseOrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(po => po.Description)
            .HasMaxLength(1000);

        builder.Property(po => po.Notes)
            .HasMaxLength(2000);

        builder.Property(po => po.Status)
            .IsRequired();

        builder.Property(po => po.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(po => po.SubTotal)
            .HasPrecision(18, 2);

        builder.Property(po => po.TaxAmount)
            .HasPrecision(18, 2);

        builder.Property(po => po.ShippingCost)
            .HasPrecision(18, 2);

        builder.Property(po => po.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(po => po.ShippingAddress)
            .HasMaxLength(500);

        builder.Property(po => po.ShippingCity)
            .HasMaxLength(100);

        builder.Property(po => po.ShippingState)
            .HasMaxLength(50);

        builder.Property(po => po.ShippingZipCode)
            .HasMaxLength(20);

        // Relationships
        builder.HasOne(po => po.Vendor)
            .WithMany(v => v.PurchaseOrders)
            .HasForeignKey(po => po.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(po => po.WorkOrder)
            .WithMany()
            .HasForeignKey(po => po.WorkOrderId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(po => po.Requester)
            .WithMany()
            .HasForeignKey(po => po.RequestedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(po => po.Approver)
            .WithMany()
            .HasForeignKey(po => po.ApprovedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(po => po.LineItems)
            .WithOne(li => li.PurchaseOrder)
            .HasForeignKey(li => li.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(po => po.Receipts)
            .WithOne(r => r.PurchaseOrder)
            .HasForeignKey(r => r.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(po => po.PurchaseOrderNumber)
            .IsUnique()
            .HasDatabaseName("IX_PurchaseOrders_PONumber");

        builder.HasIndex(po => po.VendorId)
            .HasDatabaseName("IX_PurchaseOrders_VendorId");

        builder.HasIndex(po => po.WorkOrderId)
            .HasDatabaseName("IX_PurchaseOrders_WorkOrderId");

        builder.HasIndex(po => po.Status)
            .HasDatabaseName("IX_PurchaseOrders_Status");

        builder.HasIndex(po => po.OrderDate)
            .HasDatabaseName("IX_PurchaseOrders_OrderDate");
    }
}
