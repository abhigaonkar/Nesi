# Naming Conventions

## General Principles

1. **Consistency:** Same pattern across entire codebase
2. **Clarity:** Names should be self-documenting
3. **Brevity:** Concise but not cryptic
4. **No abbreviations:** Unless universally understood (Id, Url, Dto)

## Backend (C#)

### Classes & Interfaces
- **PascalCase**
- Nouns or noun phrases
- Interfaces start with 'I'

```csharp
✅ User, TimesheetEntry, IRepository, ITimesheetService
❌ user, timesheet_entry, RepositoryInterface
```

### Methods
- **PascalCase**
- Verbs or verb phrases
- Async methods suffix 'Async'

```csharp
✅ GetById, CreateTimesheet, SaveChangesAsync
❌ get_by_id, createtimesheet, SaveChanges (when async)
```

### Parameters & Local Variables
- **camelCase**
- Descriptive nouns

```csharp
✅ userId, timesheetDate, hoursWorked
❌ uid, ts_date, hrs
```

### Private Fields
- **_camelCase** (underscore prefix)

```csharp
private readonly IRepository<User> _userRepository;
private ILogger<TimesheetService> _logger;
```

### Constants
- **PascalCase** or **UPPER_CASE**

```csharp
public const int MaxHoursPerDay = 24;
public const string API_VERSION = "v1";
```

### Properties
- **PascalCase**

```csharp
public string FirstName { get; set; }
public DateTime CreatedAt { get; private set; }
```

## Frontend (TypeScript/Angular)

### Classes & Interfaces
- **PascalCase**
- Interfaces: descriptive, no 'I' prefix

```typescript
✅ User, TimesheetEntry, TimesheetService
❌ user, IUser, timesheet_entry
```

### Methods & Functions
- **camelCase**
- Verbs

```typescript
✅ getTimesheets(), createEntry(), onSubmit()
❌ GetTimesheets(), create_entry()
```

### Variables
- **camelCase**
- Observables suffix with '$'

```typescript
✅ userId, timesheetList, user$
❌ UserID, timesheet_list, userObservable
```

### Constants
- **UPPER_CASE** with underscores

```typescript
✅ const API_BASE_URL = 'http://localhost:5000';
✅ const MAX_HOURS_PER_DAY = 24;
❌ const apiBaseUrl = '...';
```

### Component Selectors
- **kebab-case** with app prefix

```typescript
✅ selector: 'app-timesheet-list'
❌ selector: 'TimesheetList' or 'timesheet_list'
```

## Database

### Tables
- **PascalCase, Plural**

```sql
✅ Users, Timesheets, WorkOrders
❌ user, tbl_timesheets, work_order
```

### Columns
- **PascalCase**

```sql
✅ UserId, FirstName, CreatedAt
❌ user_id, firstname, created_at
```

### Foreign Keys
- **EntityName + Id**

```sql
✅ UserId, WorkOrderId, PayTypeId
❌ User_ID, WO_Id
```

### Indexes
- **IX_TableName_ColumnName(s)**

```sql
✅ IX_Timesheets_UserId
✅ IX_Timesheets_UserId_Date
❌ idx_timesheet_user
```

### Constraints
- **Type_TableName_Details**

```sql
✅ PK_Timesheets
✅ FK_Timesheets_Users_UserId
✅ CK_Timesheets_Hours_Range
❌ pk_ts, fk_user
```

## Files & Folders

### Backend Files
- **PascalCase.cs**
- One class per file
- File name = class name

```
✅ TimesheetEntry.cs, CreateTimesheetCommand.cs
❌ timesheet-entry.cs, Commands.cs
```

### Frontend Files
- **kebab-case.type.ts**

```
✅ timesheet-list.component.ts
✅ auth.service.ts
✅ timesheet.model.ts
❌ TimesheetList.component.ts, auth-service.ts
```

### Folders
- **kebab-case** (frontend)
- **PascalCase** (backend for namespaces)

```
Frontend: auth/, timesheet-list/, shared/
Backend: Features/, Domain/, Infrastructure/
```

## REST API Endpoints

### URLs
- **kebab-case**
- Plural resource names
- Versioned

```
✅ /api/v1/timesheets
✅ /api/v1/work-orders/{id}
❌ /api/Timesheets, /api/timesheet
```

### HTTP Methods
- GET: Retrieve
- POST: Create
- PUT: Full update
- PATCH: Partial update
- DELETE: Remove

```
GET    /api/timesheets
POST   /api/timesheets
PUT    /api/timesheets/{id}
DELETE /api/timesheets/{id}
```

## DTOs & Models

### Request DTOs
- **Action + Entity + Request**

```csharp
CreateTimesheetRequest
UpdateUserRequest
LoginRequest
```

### Response DTOs
- **Entity + Dto/Response**

```csharp
TimesheetDto
UserResponse
LoginResponse
```

## Tests

### Test Methods
- **MethodName_Scenario_ExpectedResult**

```csharp
✅ Handle_ValidRequest_ReturnsSuccess
✅ Login_InvalidCredentials_ThrowsException
❌ Test1, TestLogin, HandleTest
```

### Test Classes
- **ClassUnderTest + Tests**

```csharp
✅ TimesheetServiceTests
✅ CreateTimesheetHandlerTests
❌ Tests, UnitTests, TimesheetTest
```

## Environment & Configuration

### Environment Variables
- **UPPER_CASE** with underscores

```
DATABASE_CONNECTION_STRING
JWT_SECRET_KEY
API_BASE_URL
```

### Configuration Keys
- **PascalCase** hierarchy with colons

```json
{
  "JwtSettings:Secret": "...",
  "ConnectionStrings:DefaultConnection": "..."
}
```

## Git & Versioning

### Branch Names
- **type/description-in-kebab-case**

```
feature/timesheet-module
bugfix/login-validation
hotfix/security-patch
```

### Commit Messages
- **Imperative mood, capitalize first word**

```
✅ Add timesheet validation
✅ Fix login error handling
✅ Update database schema
❌ added timesheet, fixing bugs
```

## Abbreviations (Allowed)

- Id (Identifier)
- Dto (Data Transfer Object)
- Url (Uniform Resource Locator)
- Api (Application Programming Interface)
- Jwt (JSON Web Token)
- Db (Database in context like DbContext)
- EF (Entity Framework)
- ORM (Object-Relational Mapping)

**All other abbreviations should be avoided.**

## Examples

### Complete Entity Example

```csharp
// File: Domain/Entities/TimesheetEntry.cs
namespace Nesi.Domain.Entities
{
    public class TimesheetEntry : BaseEntity
    {
        // Properties - PascalCase
        public int UserId { get; private set; }
        public DateTime Date { get; private set; }
        public decimal Hours { get; private set; }
        public string Notes { get; private set; }
        
        // Private constructor for EF Core
        private TimesheetEntry() { }
        
        // Public constructor - encapsulates business rules
        public TimesheetEntry(int userId, DateTime date, decimal hours, string notes = "")
        {
            UserId = userId;
            Date = date;
            Hours = hours;
            Notes = notes;
        }
        
        // Methods - PascalCase, verbs
        public void UpdateHours(decimal newHours)
        {
            if (newHours <= 0 || newHours > 24)
                throw new ArgumentException("Hours must be between 0 and 24");
            
            Hours = newHours;
        }
    }
}
```

### Complete Component Example

```typescript
// File: features/timesheet/timesheet-list.component.ts
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TimesheetService } from '../../core/services/timesheet.service';
import { Timesheet } from '../../models/timesheet.model';

@Component({
  selector: 'app-timesheet-list',
  templateUrl: './timesheet-list.component.html',
  styleUrls: ['./timesheet-list.component.scss']
})
export class TimesheetListComponent implements OnInit, OnDestroy {
  // Properties - camelCase
  timesheets: Timesheet[] = [];
  loading = false;
  
  // Observables - suffix with $
  timesheets$ = this.timesheetService.getTimesheets();
  
  // Private properties
  private destroy$ = new Subject<void>();
  
  // Constructor - dependency injection
  constructor(private timesheetService: TimesheetService) {}
  
  // Lifecycle hooks
  ngOnInit(): void {
    this.loadTimesheets();
  }
  
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  
  // Public methods - camelCase, verbs
  onCreateTimesheet(): void {
    // ...
  }
  
  onDeleteTimesheet(id: number): void {
    // ...
  }
  
  // Private methods
  private loadTimesheets(): void {
    this.loading = true;
    this.timesheetService.getTimesheets()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.timesheets = data;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading timesheets:', error);
          this.loading = false;
        }
      });
  }
}
```

---

**Remember:** Consistency is key. When in doubt, look at existing code that follows these conventions and match the pattern.
