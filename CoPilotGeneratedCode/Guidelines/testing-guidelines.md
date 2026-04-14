# Testing Guidelines

## Test Coverage Requirements

- **Minimum:** 70% overall code coverage
- **Critical paths:** 90%+ (authentication, business rules)
- **Domain logic:** 100% (business rules, validators)

## Testing Pyramid

```
      /\
     /E2E\      10% - Full user flows
    /------\
   /  API   \   20% - Integration tests
  /----------\
 /   Unit     \ 70% - Unit tests
/--------------\
```

## Backend Testing (.NET)

### Unit Tests (xUnit)

**Structure:**
```csharp
public class CreateTimesheetHandlerTests
{
    // Naming: MethodName_Scenario_ExpectedResult
    [Fact]
    public async Task Handle_ValidRequest_CreatesTimesheet()
    {
        // Arrange
        var repository = new Mock<IRepository<Timesheet>>();
        var handler = new CreateTimesheetHandler(repository.Object);
        var command = new CreateTimesheetCommand { /* ... */ };
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        repository.Verify(r => r.AddAsync(It.IsAny<Timesheet>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(25)]
    public async Task Handle_InvalidHours_ThrowsException(decimal hours)
    {
        // Arrange, Act, Assert
        var act = () => handler.Handle(new CreateTimesheetCommand { Hours = hours });
        await act.Should().ThrowAsync<ValidationException>();
    }
}
```

**Mocking:**
- Use Moq for dependencies
- Mock interfaces, not classes
- Verify interactions

**Assertions:**
- Use FluentAssertions for readability
- Test one thing per test
- Descriptive test names

### Integration Tests

```csharp
public class TimesheetsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    public TimesheetsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetTimesheets_ReturnsOk()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
        
        // Act
        var response = await _client.GetAsync("/api/timesheets");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

## Frontend Testing (Angular)

### Component Tests (Jest)

```typescript
describe('TimesheetListComponent', () => {
  let component: TimesheetListComponent;
  let fixture: ComponentFixture<TimesheetListComponent>;
  let service: jest.Mocked<TimesheetService>;
  
  beforeEach(async () => {
    const serviceMock = {
      getTimesheets: jest.fn().mockReturnValue(of([]))
    };
    
    await TestBed.configureTestingModule({
      imports: [TimesheetListComponent],
      providers: [
        { provide: TimesheetService, useValue: serviceMock }
      ]
    }).compileComponents();
    
    service = TestBed.inject(TimesheetService) as jest.Mocked<TimesheetService>;
    fixture = TestBed.createComponent(TimesheetListComponent);
    component = fixture.componentInstance;
  });
  
  it('should create', () => {
    expect(component).toBeTruthy();
  });
  
  it('should load timesheets on init', () => {
    const mockData = [{ id: 1, date: new Date(), hours: 8 }];
    service.getTimesheets.mockReturnValue(of(mockData));
    
    component.ngOnInit();
    
    expect(service.getTimesheets).toHaveBeenCalled();
    expect(component.timesheets).toEqual(mockData);
  });
});
```

### E2E Tests (Playwright)

```typescript
import { test, expect } from '@playwright/test';

test.describe('Timesheet Flow', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/login');
    await page.fill('input[name="username"]', 'testuser');
    await page.fill('input[name="password"]', 'password');
    await page.click('button[type="submit"]');
    await expect(page).toHaveURL('/dashboard');
  });
  
  test('should create timesheet entry', async ({ page }) => {
    await page.click('text=Timesheets');
    await page.click('button:has-text("Add")');
    
    await page.fill('input[name="hours"]', '8');
    await page.selectOption('select[name="payType"]', '1');
    await page.click('button:has-text("Save")');
    
    await expect(page.locator('text=Saved successfully')).toBeVisible();
  });
});
```

## Test Data

### Test Builders
```csharp
public class TimesheetBuilder
{
    private int _userId = 1;
    private DateTime _date = DateTime.Today;
    private decimal _hours = 8;
    
    public TimesheetBuilder WithUserId(int userId)
    {
        _userId = userId;
        return this;
    }
    
    public TimesheetBuilder WithHours(decimal hours)
    {
        _hours = hours;
        return this;
    }
    
    public Timesheet Build()
    {
        return new Timesheet(_userId, _date, _hours);
    }
}

// Usage
var timesheet = new TimesheetBuilder()
    .WithUserId(5)
    .WithHours(10)
    .Build();
```

## CI/CD Testing

### Local Test Execution
```bash
# Backend
cd backend
dotnet test --collect:"XPlat Code Coverage"

# Frontend
cd frontend
npm test -- --coverage
npm run test:e2e
```

### Coverage Reports
- Generate HTML reports
- Fail build if < 70%
- Track coverage trends
