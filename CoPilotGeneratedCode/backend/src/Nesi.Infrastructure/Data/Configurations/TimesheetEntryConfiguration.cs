using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;
using Nesi.Domain.Enums;

namespace Nesi.Infrastructure.Data.Configurations;

public class TimesheetEntryConfiguration : IEntityTypeConfiguration<TimesheetEntry>
{
    public void Configure(EntityTypeBuilder<TimesheetEntry> builder)
    {
        builder.ToTable("Timesheets");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Date)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(t => t.Hours)
            .IsRequired()
            .HasColumnType("decimal(5,2)");

        builder.Property(t => t.Notes)
            .HasMaxLength(500);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(TimesheetStatus.Draft);

        // Relationships
        builder.HasOne(t => t.User)
            .WithMany(u => u.Timesheets)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.WorkOrder)
            .WithMany(w => w.TimesheetEntries)
            .HasForeignKey(t => t.WorkOrderId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(t => t.PayType)
            .WithMany(p => p.TimesheetEntries)
            .HasForeignKey(t => t.PayTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.JobType)
            .WithMany(j => j.TimesheetEntries)
            .HasForeignKey(t => t.JobTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(t => new { t.UserId, t.Date })
            .HasDatabaseName("IX_Timesheets_UserId_Date");

        builder.HasIndex(t => t.Status)
            .HasDatabaseName("IX_Timesheets_Status");
    }
}
