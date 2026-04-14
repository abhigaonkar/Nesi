# Database Guidelines (EF Core 8 + MySQL)

## Entity Framework Core Principles

### Code-First Approach (Mandatory)
- Define entities in Domain layer
- Configure via Fluent API (not attributes)
- Migrations for all schema changes
- No direct SQL except for complex queries

### Entity Configuration
- Separate configuration classes
- IEntityTypeConfiguration<T>
- Explicit relationships

```csharp
public class TimesheetConfiguration : IEntityTypeConfiguration<Timesheet>
{
    public void Configure(EntityTypeBuilder<Timesheet> builder)
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
        
        builder.HasOne(t => t.User)
            .WithMany(u => u.Timesheets)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(t => new { t.UserId, t.Date })
            .IsUnique();
    }
}
```

## Naming Conventions

### Tables
- PascalCase, plural (Users, Timesheets, WorkOrders)
- No prefixes (tbl_)

### Columns
- PascalCase (UserId, CreatedAt, FirstName)
- Foreign keys: EntityName + Id (UserId, WorkOrderId)

### Indexes
- IX_TableName_ColumnNames (IX_Timesheets_UserId_Date)

### Constraints
- PK_TableName (PK_Timesheets)
- FK_Table1_Table2_Column (FK_Timesheets_Users_UserId)

## Schema Design

### Base Entity
```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
}
```

### Audit Fields (Mandatory)
- CreatedAt, UpdatedAt
- CreatedBy, UpdatedBy
- Soft delete with IsDeleted flag

### Data Types
- **IDs:** int (auto-increment)
- **Dates:** DateTime or DateOnly
- **Money:** decimal(19,4)
- **Hours:** decimal(5,2)
- **Short text:** nvarchar(length)
- **Long text:** nvarchar(max) or TEXT
- **Booleans:** bit (mapped to bool)

## Relationships

### One-to-Many
```csharp
// User has many Timesheets
builder.HasMany(u => u.Timesheets)
    .WithOne(t => t.User)
    .HasForeignKey(t => t.UserId)
    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
```

### Many-to-Many
```csharp
// Use explicit join entity for flexibility
public class UserRole
{
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int RoleId { get; set; }
    public Role Role { get; set; }
    
    public DateTime AssignedAt { get; set; }
}
```

## Migrations

### Creating Migrations
```bash
# From Infrastructure project directory
dotnet ef migrations add MigrationName --startup-project ../Nesi.Api

# Review migration before applying
# Check Up() and Down() methods
```

### Migration Guidelines
- Descriptive names (AddTimesheetTable, AddUserEmailIndex)
- Small, focused migrations
- Always test Down() method
- Never modify existing migrations after deploy

### Initial Data Seeding
```csharp
public class NesiDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Seed lookup tables
        modelBuilder.Entity<PayType>().HasData(
            new PayType { Id = 1, Code = "REG", Name = "Regular", Multiplier = 1.0m },
            new PayType { Id = 2, Code = "OT", Name = "Overtime", Multiplier = 1.5m }
        );
    }
}
```

## Queries

### Repository Pattern
```csharp
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly NesiDbContext _context;
    protected readonly DbSet<T> _dbSet;
    
    public Repository(NesiDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public virtual async Task<T> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, ct);
    }
    
    public virtual async Task<List<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbSet.ToListAsync(ct);
    }
}
```

### Query Performance
- Use AsNoTracking() for read-only queries
- Include() for eager loading
- Select projections for DTOs
- Pagination with Skip/Take

```csharp
// ✅ Good - projection
public async Task<List<TimesheetDto>> GetTimesheetsAsync(int userId)
{
    return await _context.Timesheets
        .Where(t => t.UserId == userId)
        .Select(t => new TimesheetDto
        {
            Id = t.Id,
            Date = t.Date,
            Hours = t.Hours
        })
        .AsNoTracking()
        .ToListAsync();
}

// ❌ Bad - loads entire entity
public async Task<List<Timesheet>> GetTimesheetsAsync(int userId)
{
    return await _context.Timesheets
        .Where(t => t.UserId == userId)
        .ToListAsync();
}
```

## Connection Strings

### Local Development
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=nesi_demo;User=root;Password=your_password;"
  }
}
```

### Best Practices
- Never commit passwords
- Use User Secrets for development
- Environment variables for production

## Indexes

### When to Add
- Foreign keys (automatic in MySQL)
- Frequently queried columns
- Unique constraints

```csharp
builder.HasIndex(t => t.UserId)
    .HasDatabaseName("IX_Timesheets_UserId");

builder.HasIndex(t => new { t.UserId, t.Date })
    .IsUnique()
    .HasDatabaseName("IX_Timesheets_UserId_Date");
```

## Transactions
```csharp
using var transaction = await _context.Database.BeginTransactionAsync();
try
{
    // Multiple operations
    await _context.SaveChangesAsync();
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```
