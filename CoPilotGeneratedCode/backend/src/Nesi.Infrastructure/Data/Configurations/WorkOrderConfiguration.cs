using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
{
    public void Configure(EntityTypeBuilder<WorkOrder> builder)
    {
        builder.ToTable("WorkOrders");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.WorkOrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(w => w.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(w => w.StartDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(w => w.EndDate)
            .HasColumnType("date");

        builder.Property(w => w.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Relationships
        builder.HasOne(w => w.Customer)
            .WithMany(c => c.WorkOrders)
            .HasForeignKey(w => w.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(w => w.WorkOrderNumber)
            .IsUnique()
            .HasDatabaseName("IX_WorkOrders_WorkOrderNumber");

        builder.HasIndex(w => w.CustomerId)
            .HasDatabaseName("IX_WorkOrders_CustomerId");
    }
}
