using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class JobTypeConfiguration : IEntityTypeConfiguration<JobType>
{
    public void Configure(EntityTypeBuilder<JobType> builder)
    {
        builder.ToTable("JobTypes");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(j => j.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(j => j.Description)
            .HasMaxLength(200);

        builder.Property(j => j.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Seed data
        builder.HasData(
            new { Id = 1, Code = "DEV", Name = "Development", Description = "Software development work", IsActive = true, CreatedAt = new DateTime(2024, 1, 1), CreatedBy = "system", IsDeleted = false },
            new { Id = 2, Code = "QA", Name = "Quality Assurance", Description = "Testing and quality assurance", IsActive = true, CreatedAt = new DateTime(2024, 1, 1), CreatedBy = "system", IsDeleted = false },
            new { Id = 3, Code = "DESIGN", Name = "Design", Description = "UI/UX design work", IsActive = true, CreatedAt = new DateTime(2024, 1, 1), CreatedBy = "system", IsDeleted = false },
            new { Id = 4, Code = "PM", Name = "Project Management", Description = "Project management activities", IsActive = true, CreatedAt = new DateTime(2024, 1, 1), CreatedBy = "system", IsDeleted = false }
        );

        // Indexes
        builder.HasIndex(j => j.Code)
            .IsUnique()
            .HasDatabaseName("IX_JobTypes_Code");
    }
}
