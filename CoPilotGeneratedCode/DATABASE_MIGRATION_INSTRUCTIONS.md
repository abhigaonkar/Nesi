# Database Migration Instructions

## Issue
The Customer entity was enhanced with many additional properties, but the database schema was not updated accordingly. This caused SQL errors when trying to query customers because the columns didn't exist in the database.

## Solution
A new migration `AddCustomerEnhancedColumns` has been created to add all the missing columns to the Customers table.

## How to Apply the Migration

### Prerequisites
- SQL Server or SQL Server Express installed and running
- Connection string configured in `appsettings.json`

### Steps

1. **Update the connection string** (if needed)
   
   Edit `CoPilotGeneratedCode/backend/src/Nesi.Api/appsettings.json`:
   
   For SQL Server Express:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=NesiDb;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
   }
   ```
   
   Or for Azure SQL:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=tcp:YOUR_SERVER.database.windows.net,1433;Initial Catalog=NesiDb;Persist Security Info=False;User ID=YOUR_USER;Password=YOUR_PASSWORD;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
   }
   ```

2. **Apply the migration**
   
   From the `CoPilotGeneratedCode/backend` directory, run:
   
   ```bash
   dotnet ef database update --project src/Nesi.Infrastructure --startup-project src/Nesi.Api
   ```
   
   Or simply run the application, and it will apply the migration automatically if configured to do so.

3. **Verify the migration**
   
   After applying the migration, you should be able to query customers without errors. The Customer table will now have all these additional columns:
   
   - CustomerNumber (with unique index)
   - BusinessUnitId
   - IsQualityChecked, IsPartner, IsKeyAccount
   - CreditType, CreditLimit, Discount, BudgetThreshold, TermId, ApplyFinanceCharges
   - AccountManagerId, InsideSalesRepId, OutsideSalesRepId, RegionalAccountManagerId, MajorAccountManagerId, DecisionMakerId
   - HoldStatus, HoldReason, HoldByUserId
   - DefaultInvoiceType, StatementCode, ServiceChargeCode, TaxPrompt, PriceCode, PORequired, AutoInvoice
   - Notes, FollowUpNotes, NextFollowUpDate, FollowUpFrequencyDays, YearEnd

## Migration Details

The migration includes:
- Adding all missing Customer columns with appropriate data types, lengths, and default values
- Auto-populating CustomerNumber for existing records (format: CUST00001, CUST00002, etc.)
- Creating a unique index on CustomerNumber
- Creating tables for CustomerAddress, CustomerContact, and CustomerNote entities
- Creating the Vendors table with all its columns

## Important Notes

- **CustomerNumber**: The migration automatically generates a CustomerNumber for any existing records using the pattern `CUST` + padded ID (e.g., CUST00001).
- **Default Values**: Boolean and numeric fields have sensible defaults (false for booleans, 0 for numerics).
- **Nullable Fields**: Most new fields are nullable to avoid issues with existing data.
