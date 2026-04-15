using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Materials");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.PartNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(m => m.Quantity)
            .HasColumnType("decimal(10,2)");

        builder.Property(m => m.UnitCost)
            .HasColumnType("decimal(18,2)");

        builder.Property(m => m.TotalCost)
            .HasColumnType("decimal(18,2)");

        builder.Property(m => m.PurchaseOrderNumber)
            .HasMaxLength(50);

        builder.Property(m => m.Supplier)
            .HasMaxLength(200);

        builder.Property(m => m.Notes)
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(m => m.WorkOrder)
            .WithMany(w => w.Materials)
            .HasForeignKey(m => m.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(m => m.WorkOrderId)
            .HasDatabaseName("IX_Materials_WorkOrderId");

        builder.HasIndex(m => m.PartNumber)
            .HasDatabaseName("IX_Materials_PartNumber");
    }
}
