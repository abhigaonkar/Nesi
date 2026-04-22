using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nesi.Domain.Entities;

namespace Nesi.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        // Basic Information
        builder.Property(c => c.CustomerNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.BusinessUnitId);

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(c => c.IsQualityChecked)
            .HasDefaultValue(false);

        builder.Property(c => c.IsPartner)
            .HasDefaultValue(false);

        builder.Property(c => c.IsKeyAccount)
            .HasDefaultValue(false);

        // Contact Information (Primary - for backward compatibility)
        builder.Property(c => c.ContactName)
            .HasMaxLength(100);

        builder.Property(c => c.Email)
            .HasMaxLength(100);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Address)
            .HasMaxLength(500);

        // Financial Information
        builder.Property(c => c.CreditType);

        builder.Property(c => c.CreditLimit)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);

        builder.Property(c => c.Discount)
            .HasColumnType("decimal(5,2)")
            .HasDefaultValue(0);

        builder.Property(c => c.BudgetThreshold)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);

        builder.Property(c => c.TermId);

        builder.Property(c => c.ApplyFinanceCharges)
            .HasDefaultValue(false);

        // Account Management
        builder.Property(c => c.AccountManagerId);
        builder.Property(c => c.InsideSalesRepId);
        builder.Property(c => c.OutsideSalesRepId);
        builder.Property(c => c.RegionalAccountManagerId);
        builder.Property(c => c.MajorAccountManagerId);
        builder.Property(c => c.DecisionMakerId);

        // Status and Hold Information
        builder.Property(c => c.HoldStatus)
            .HasMaxLength(20);

        builder.Property(c => c.HoldReason)
            .HasMaxLength(500);

        builder.Property(c => c.HoldByUserId);

        // Invoicing Preferences
        builder.Property(c => c.DefaultInvoiceType);

        builder.Property(c => c.StatementCode);

        builder.Property(c => c.ServiceChargeCode)
            .HasMaxLength(50);

        builder.Property(c => c.TaxPrompt)
            .HasMaxLength(50);

        builder.Property(c => c.PriceCode)
            .HasMaxLength(50);

        builder.Property(c => c.PORequired)
            .HasMaxLength(10);

        builder.Property(c => c.AutoInvoice)
            .HasDefaultValue(0);

        // Follow-up and Notes
        builder.Property(c => c.Notes)
            .HasMaxLength(2000);

        builder.Property(c => c.FollowUpNotes)
            .HasMaxLength(1000);

        builder.Property(c => c.NextFollowUpDate);

        builder.Property(c => c.FollowUpFrequencyDays)
            .HasDefaultValue(0);

        builder.Property(c => c.YearEnd);

        // Indexes
        builder.HasIndex(c => c.CustomerNumber)
            .IsUnique()
            .HasDatabaseName("IX_Customers_CustomerNumber");

        builder.HasIndex(c => c.Name)
            .HasDatabaseName("IX_Customers_Name");
    }
}
