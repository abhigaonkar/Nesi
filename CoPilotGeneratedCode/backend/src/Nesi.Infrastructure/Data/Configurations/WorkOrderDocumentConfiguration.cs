using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class WorkOrderDocumentConfiguration : IEntityTypeConfiguration<WorkOrderDocument>
{
    public void Configure(EntityTypeBuilder<WorkOrderDocument> builder)
    {
        builder.ToTable("WorkOrderDocuments");

        builder.HasKey(wod => wod.Id);

        builder.Property(wod => wod.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(wod => wod.FileUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(wod => wod.FileType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(wod => wod.DocumentType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(wod => wod.Description)
            .HasMaxLength(500);

        // Relationships
        builder.HasOne(wod => wod.WorkOrder)
            .WithMany(w => w.Documents)
            .HasForeignKey(wod => wod.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wod => wod.Uploader)
            .WithMany()
            .HasForeignKey(wod => wod.UploadedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(wod => wod.WorkOrderId)
            .HasDatabaseName("IX_WorkOrderDocuments_WorkOrderId");

        builder.HasIndex(wod => wod.DocumentType)
            .HasDatabaseName("IX_WorkOrderDocuments_DocumentType");
    }
}
