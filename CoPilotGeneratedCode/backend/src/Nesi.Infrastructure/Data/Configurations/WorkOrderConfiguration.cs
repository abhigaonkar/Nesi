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

        builder.Property(w => w.Milestones)
            .HasMaxLength(2000);

        builder.Property(w => w.CompletionNotes)
            .HasMaxLength(1000);

        builder.Property(w => w.InvoiceAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(w => w.ScheduledStartDate)
            .HasColumnType("date");

        builder.Property(w => w.ScheduledEndDate)
            .HasColumnType("date");

        // Relationships
        builder.HasOne(w => w.Customer)
            .WithMany(c => c.WorkOrders)
            .HasForeignKey(w => w.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.ProjectManager)
            .WithMany()
            .HasForeignKey(w => w.ProjectManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-one relationship with Quote is configured on the Quote side

        builder.HasMany(w => w.TimesheetEntries)
            .WithOne(t => t.WorkOrder)
            .HasForeignKey(t => t.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(w => w.Materials)
            .WithOne(m => m.WorkOrder)
            .HasForeignKey(m => m.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(w => w.Assignments)
            .WithOne(a => a.WorkOrder)
            .HasForeignKey(a => a.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(w => w.Documents)
            .WithOne(d => d.WorkOrder)
            .HasForeignKey(d => d.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(w => w.WorkOrderNumber)
            .IsUnique()
            .HasDatabaseName("IX_WorkOrders_WorkOrderNumber");

        builder.HasIndex(w => w.CustomerId)
            .HasDatabaseName("IX_WorkOrders_CustomerId");

        builder.HasIndex(w => w.QuoteId)
            .HasDatabaseName("IX_WorkOrders_QuoteId");

        builder.HasIndex(w => w.Status)
            .HasDatabaseName("IX_WorkOrders_Status");
    }
}
