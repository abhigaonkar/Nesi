# NESI Demo Architecture

## Overview

This document describes the technical architecture of the NESI demo application, built using Clean Architecture principles with CQRS pattern.

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                      Frontend (Angular 19)                   │
│  ┌────────────┬──────────────┬────────────┬──────────────┐  │
│  │   Auth     │   Dashboard   │ Timesheet  │   Shared     │  │
│  │  Feature   │   Feature     │  Feature   │  Components  │  │
│  └────────────┴──────────────┴────────────┴──────────────┘  │
│         ↓              ↓             ↓             ↓         │
│  ┌───────────────────────────────────────────────────────┐  │
│  │         Core Services & HTTP Interceptors             │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                           ↓ HTTP/REST/JSON
┌─────────────────────────────────────────────────────────────┐
│                    Backend (ASP.NET Core 8)                  │
│  ┌───────────────────────────────────────────────────────┐  │
│  │          API Layer (Controllers, Middleware)          │  │
│  └───────────────────────────────────────────────────────┘  │
│                           ↓ MediatR
│  ┌───────────────────────────────────────────────────────┐  │
│  │    Application Layer (CQRS Handlers, DTOs, Validators│  │
│  └───────────────────────────────────────────────────────┘  │
│                           ↓
│  ┌───────────────────────────────────────────────────────┐  │
│  │      Domain Layer (Entities, Business Rules)          │  │
│  └───────────────────────────────────────────────────────┘  │
│                           ↑
│  ┌───────────────────────────────────────────────────────┐  │
│  │   Infrastructure Layer (EF Core, Repositories)        │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                           ↓ EF Core
┌─────────────────────────────────────────────────────────────┐
│                      MySQL Database                          │
└─────────────────────────────────────────────────────────────┘
```

## Clean Architecture Layers

### 1. Domain Layer (Core)
**Purpose:** Contains enterprise business rules and entities

**Components:**
- Entities (User, TimesheetEntry, WorkOrder, etc.)
- Value Objects
- Domain Events
- Business Rules
- Repository Interfaces

**Dependencies:** None (Pure .NET, no external dependencies)

**Example:**
```csharp
public class TimesheetEntry : BaseEntity
{
    public int UserId { get; private set; }
    public DateTime Date { get; private set; }
    public decimal Hours { get; private set; }
    
    public void Submit()
    {
        if (Status != TimesheetStatus.Draft)
            throw new InvalidOperationException("Only draft timesheets can be submitted");
        Status = TimesheetStatus.Submitted;
    }
}
```

### 2. Application Layer
**Purpose:** Contains application-specific business rules and use cases

**Components:**
- CQRS Commands & Queries
- Command/Query Handlers (MediatR)
- DTOs (Data Transfer Objects)
- Validators (FluentValidation)
- AutoMapper Profiles
- Application Interfaces

**Dependencies:** Domain Layer only

**Example:**
```csharp
public record CreateTimesheetCommand : IRequest<int>
{
    public int UserId { get; init; }
    public DateTime Date { get; init; }
    public decimal Hours { get; init; }
}

public class CreateTimesheetHandler : IRequestHandler<CreateTimesheetCommand, int>
{
    private readonly IRepository<TimesheetEntry> _repository;
    
    public async Task<int> Handle(CreateTimesheetCommand request, CancellationToken ct)
    {
        var timesheet = new TimesheetEntry(request.UserId, request.Date, request.Hours);
        await _repository.AddAsync(timesheet, ct);
        return timesheet.Id;
    }
}
```

### 3. Infrastructure Layer
**Purpose:** Implements interfaces from Application/Domain layers

**Components:**
- DbContext (EF Core)
- Repository Implementations
- External Service Implementations
- File System Access
- Email Services
- External APIs

**Dependencies:** Application & Domain layers

**Example:**
```csharp
public class NesiDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<TimesheetEntry> Timesheets { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
```

### 4. API Layer (Presentation)
**Purpose:** Handles HTTP requests and responses

**Components:**
- Controllers
- Middleware (Authentication, Error Handling, Logging)
- Filters
- Model Binders
- Startup/Program Configuration

**Dependencies:** Application & Infrastructure layers

**Example:**
```csharp
[ApiController]
[Route("api/[controller]")]
public class TimesheetsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTimesheetCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }
}
```

## Design Patterns

### 1. CQRS (Command Query Responsibility Segregation)
- **Commands:** Modify state (Create, Update, Delete)
- **Queries:** Read data (Get, List, Search)
- **Mediator:** MediatR handles routing

### 2. Repository Pattern
- Abstracts data access
- Enables testability
- Centralized query logic

### 3. Dependency Injection
- Constructor injection throughout
- Registered in Program.cs
- Enables loose coupling

### 4. Unit of Work
- Manages transactions
- Ensures consistency
- Rolled into DbContext

## Frontend Architecture

### Component Structure
```
features/
  timesheet/
    components/
      timesheet-list/
      timesheet-form/
    services/
      timesheet.service.ts
    models/
      timesheet.model.ts
```

### State Management
- Local component state for simple scenarios
- NgRx for complex global state
- Services for shared state

### HTTP Communication
- HttpClient with interceptors
- Auth token injection
- Global error handling
- Loading state management

## Security Architecture

### Authentication
- JWT tokens (access + refresh)
- Stored in localStorage
- Automatic token refresh
- Logout clears tokens

### Authorization
- Role-based (Admin, Manager, Employee)
- Controller-level: [Authorize(Roles = "Admin")]
- Client-side route guards

### API Security
- HTTPS only in production
- CORS configured for frontend URL
- Input validation (FluentValidation)
- SQL injection prevention (parameterized queries)

## Database Schema

### Core Tables
- Users
- Timesheets
- WorkOrders
- Customers
- PayTypes (lookup)
- JobTypes (lookup)

### Key Relationships
- User → Timesheets (one-to-many)
- WorkOrder → Timesheets (one-to-many)
- Customer → WorkOrders (one-to-many)

## Performance Considerations

### Backend
- AsNoTracking() for read-only queries
- Projection to DTOs (no over-fetching)
- Indexed foreign keys
- Async/await throughout

### Frontend
- Lazy loading modules
- OnPush change detection
- Virtual scrolling for large lists
- Image optimization

## Error Handling

### Backend
- Global exception middleware
- Structured error responses
- Serilog for logging
- Application Insights (optional)

### Frontend
- HTTP interceptor for global errors
- User-friendly error messages
- Retry logic for network errors
- Error boundary components

## Testing Strategy

### Backend (70%+ coverage)
- Unit tests (xUnit + Moq)
- Integration tests (WebApplicationFactory)
- Repository tests (In-memory DB)

### Frontend (70%+ coverage)
- Component tests (Jest)
- Service tests (Jest)
- E2E tests (Playwright)

## Deployment

### Local Development
- Backend: `dotnet run` (Port 5000)
- Frontend: `npm start` (Port 4200)
- Database: MySQL local instance

### Future: Cloud Deployment
- Azure App Service (Backend)
- Azure Static Web Apps (Frontend)
- Azure Database for MySQL (Database)
- Azure Key Vault (Secrets)

---

**Last Updated:** April 2026  
**Applies To:** NESI Demo v1.0
