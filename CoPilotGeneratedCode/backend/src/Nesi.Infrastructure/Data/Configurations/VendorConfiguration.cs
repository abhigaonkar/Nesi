using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable("Vendors");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.VendorNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(v => v.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.ContactName)
            .HasMaxLength(100);

        builder.Property(v => v.Email)
            .HasMaxLength(100);

        builder.Property(v => v.Phone)
            .HasMaxLength(20);

        builder.Property(v => v.Fax)
            .HasMaxLength(20);

        builder.Property(v => v.Website)
            .HasMaxLength(200);

        builder.Property(v => v.Address)
            .HasMaxLength(500);

        builder.Property(v => v.City)
            .HasMaxLength(100);

        builder.Property(v => v.State)
            .HasMaxLength(50);

        builder.Property(v => v.ZipCode)
            .HasMaxLength(20);

        builder.Property(v => v.Country)
            .HasMaxLength(100);

        builder.Property(v => v.TaxId)
            .HasMaxLength(50);

        builder.Property(v => v.AccountNumber)
            .HasMaxLength(50);

        builder.Property(v => v.Status)
            .IsRequired();

        builder.Property(v => v.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(v => v.Rating)
            .HasPrecision(3, 2);

        builder.Property(v => v.CreditLimit)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(v => v.VendorNumber)
            .IsUnique()
            .HasDatabaseName("IX_Vendors_VendorNumber");

        builder.HasIndex(v => v.CompanyName)
            .HasDatabaseName("IX_Vendors_CompanyName");

        builder.HasIndex(v => v.Status)
            .HasDatabaseName("IX_Vendors_Status");
    }
}
