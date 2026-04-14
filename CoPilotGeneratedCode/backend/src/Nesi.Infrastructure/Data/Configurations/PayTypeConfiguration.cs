using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class PayTypeConfiguration : IEntityTypeConfiguration<PayType>
{
    public void Configure(EntityTypeBuilder<PayType> builder)
    {
        builder.ToTable("PayTypes");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Multiplier)
            .IsRequired()
            .HasColumnType("decimal(5,2)");

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Seed data
        builder.HasData(
            new { Id = 1, Code = "REG", Name = "Regular", Multiplier = 1.0m, IsActive = true, CreatedAt = new DateTime(2024, 1, 1), CreatedBy = "system", IsDeleted = false },
            new { Id = 2, Code = "OT", Name = "Overtime", Multiplier = 1.5m, IsActive = true, CreatedAt = new DateTime(2024, 1, 1), CreatedBy = "system", IsDeleted = false },
            new { Id = 3, Code = "DT", Name = "Double Time", Multiplier = 2.0m, IsActive = true, CreatedAt = new DateTime(2024, 1, 1), CreatedBy = "system", IsDeleted = false }
        );

        // Indexes
        builder.HasIndex(p => p.Code)
            .IsUnique()
            .HasDatabaseName("IX_PayTypes_Code");
    }
}
