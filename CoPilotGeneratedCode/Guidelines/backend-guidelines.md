# Backend Coding Guidelines (.NET Core 8)

## Architecture Principles

### Clean Architecture (Mandatory)
```
Api Layer (Controllers, Middleware)
    ↓ depends on
Application Layer (Use Cases, DTOs, Validators)
    ↓ depends on
Domain Layer (Entities, Interfaces, Business Rules)
    ↑ implemented by
Infrastructure Layer (Data Access, External Services)
```

**Rules:**
- Domain layer has NO dependencies on other layers
- Application layer depends ONLY on Domain
- Infrastructure implements interfaces defined in Application/Domain
- API layer is thin - delegates to Application layer

### CQRS Pattern (Mandatory)
- **Commands:** Modify state, return void or ID
- **Queries:** Read-only, return DTOs
- Use MediatR for all business operations
- One handler per command/query

**Example:**
```csharp
// Command
public record CreateTimesheetCommand : IRequest<int>
{
    public int UserId { get; init; }
    public DateTime Date { get; init; }
    public decimal Hours { get; init; }
}

// Handler
public class CreateTimesheetHandler : IRequestHandler<CreateTimesheetCommand, int>
{
    private readonly IRepository<Timesheet> _repository;
    
    public async Task<int> Handle(CreateTimesheetCommand request, CancellationToken ct)
    {
        var timesheet = new Timesheet { /* map properties */ };
        await _repository.AddAsync(timesheet, ct);
        return timesheet.Id;
    }
}
```

## Code Style

### Naming Conventions
- **Classes:** PascalCase, noun (User, TimesheetEntry)
- **Interfaces:** I + PascalCase (IRepository, ITimesheetService)
- **Methods:** PascalCase, verb (GetById, CreateTimesheet)
- **Variables:** camelCase (userId, timesheetDate)
- **Private fields:** _camelCase (_repository, _logger)
- **Constants:** PascalCase or UPPER_CASE (MaxHoursPerDay, API_VERSION)

### File Organization
- One class per file
- File name matches class name
- Organize by feature, not type
  - ✅ Features/Timesheets/Commands/CreateTimesheet/
  - ❌ Commands/CreateTimesheetCommand.cs

### Dependency Injection (Mandatory)
- Constructor injection ONLY
- No service locator pattern
- Register in Program.cs or extension methods

```csharp
// ✅ Good
public class TimesheetService
{
    private readonly IRepository<Timesheet> _repository;
    private readonly ILogger<TimesheetService> _logger;
    
    public TimesheetService(IRepository<Timesheet> repository, ILogger<TimesheetService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}

// ❌ Bad
public class TimesheetService
{
    private IRepository<Timesheet> _repository = ServiceLocator.Get<IRepository<Timesheet>>();
}
```

## Domain Layer

### Entities
- Rich domain models (behavior + data)
- Encapsulate business rules
- No public setters for complex properties
- Use private constructors for invariants

```csharp
public class TimesheetEntry : BaseEntity
{
    public int UserId { get; private set; }
    public DateTime Date { get; private set; }
    public decimal Hours { get; private set; }
    public TimesheetStatus Status { get; private set; }
    
    private TimesheetEntry() { } // EF Core
    
    public TimesheetEntry(int userId, DateTime date, decimal hours)
    {
        if (hours <= 0 || hours > 24)
            throw new ArgumentException("Hours must be between 0 and 24");
            
        UserId = userId;
        Date = date.Date; // Normalize to date only
        Hours = hours;
        Status = TimesheetStatus.Draft;
    }
    
    public void Submit()
    {
        if (Status != TimesheetStatus.Draft)
            throw new InvalidOperationException("Only draft timesheets can be submitted");
            
        Status = TimesheetStatus.Submitted;
    }
}
```

### Business Rules
- Encapsulate in Domain Services
- Keep entities clean
- Use specification pattern for complex queries

## Application Layer

### DTOs
- Separate DTOs for requests and responses
- Never expose entities directly
- Use records for immutability

```csharp
public record TimesheetDto
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public decimal Hours { get; init; }
    public string Status { get; init; }
}
```

### Validation
- Use FluentValidation for all commands
- Validate at application boundary
- Business rules in domain, input validation in application

```csharp
public class CreateTimesheetValidator : AbstractValidator<CreateTimesheetCommand>
{
    public CreateTimesheetValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.Today);
            
        RuleFor(x => x.Hours)
            .GreaterThan(0)
            .LessThanOrEqualTo(24);
    }
}
```

## API Layer

### Controllers
- Thin controllers (delegate to MediatR)
- RESTful naming
- Proper HTTP status codes
- API versioning ready

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TimesheetsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    [HttpGet]
    [ProducesResponseType(typeof(List<TimesheetDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetTimesheetsQuery());
        return Ok(result);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(TimesheetDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTimesheetCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }
}
```

### Error Handling
- Global exception handler middleware
- Consistent error response format
- Log all errors

```csharp
public class ErrorResponse
{
    public string Message { get; set; }
    public string Detail { get; set; }
    public int StatusCode { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }
}
```

## Async/Await
- Use async all the way down
- Never use .Result or .Wait()
- Always pass CancellationToken
- Suffix async methods with Async

```csharp
// ✅ Good
public async Task<Timesheet> GetByIdAsync(int id, CancellationToken ct)
{
    return await _context.Timesheets.FindAsync(new object[] { id }, ct);
}

// ❌ Bad
public Timesheet GetById(int id)
{
    return _context.Timesheets.FindAsync(id).Result; // Deadlock risk!
}
```

## Logging
- Use structured logging (Serilog)
- Log levels: Debug, Information, Warning, Error, Critical
- Include context (user, correlation ID)
- Never log sensitive data (passwords, tokens)

```csharp
_logger.LogInformation(
    "Timesheet created: {TimesheetId} for User: {UserId} on {Date}",
    timesheet.Id, userId, date
);
```

## Configuration
- Use appsettings.json
- Environment-specific overrides
- Never hardcode connection strings
- Use strongly-typed options

```csharp
public class JwtSettings
{
    public string Secret { get; set; }
    public int ExpirationHours { get; set; }
}

// Registration
services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

// Usage
public class TokenService
{
    private readonly JwtSettings _settings;
    
    public TokenService(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }
}
```

## NuGet Packages (Approved)
- Microsoft.EntityFrameworkCore (8.0.x)
- MediatR (12.x)
- FluentValidation (11.x)
- AutoMapper (13.x)
- Serilog.AspNetCore (8.x)
- Swashbuckle.AspNetCore (6.x)
- xUnit (2.x)
- Moq (4.x)
- FluentAssertions (6.x)
