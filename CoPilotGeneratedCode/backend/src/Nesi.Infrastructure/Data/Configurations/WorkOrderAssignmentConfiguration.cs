using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class WorkOrderAssignmentConfiguration : IEntityTypeConfiguration<WorkOrderAssignment>
{
    public void Configure(EntityTypeBuilder<WorkOrderAssignment> builder)
    {
        builder.ToTable("WorkOrderAssignments");

        builder.HasKey(woa => woa.Id);

        builder.Property(woa => woa.Role)
            .HasMaxLength(100);

        builder.Property(woa => woa.Notes)
            .HasMaxLength(500);

        builder.Property(woa => woa.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Relationships
        builder.HasOne(woa => woa.WorkOrder)
            .WithMany(w => w.Assignments)
            .HasForeignKey(woa => woa.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(woa => woa.Technician)
            .WithMany()
            .HasForeignKey(woa => woa.TechnicianId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(woa => woa.Assigner)
            .WithMany()
            .HasForeignKey(woa => woa.AssignedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(woa => woa.WorkOrderId)
            .HasDatabaseName("IX_WorkOrderAssignments_WorkOrderId");

        builder.HasIndex(woa => woa.TechnicianId)
            .HasDatabaseName("IX_WorkOrderAssignments_TechnicianId");

        builder.HasIndex(woa => new { woa.WorkOrderId, woa.TechnicianId, woa.IsActive })
            .HasDatabaseName("IX_WorkOrderAssignments_WorkOrderId_TechnicianId_IsActive");
    }
}
