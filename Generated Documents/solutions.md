# NESI Application - Technical Solutions (Backend Layer)

**Document Version:** 1.0  
**Date:** April 22, 2026  
**Application:** Network Engineering Service Integration (NESI)  
**Focus:** Backend Layer Architecture and Implementation  
**Technologies:** ASP.NET Web API 2, Entity Framework Code First

---

## Table of Contents

1. [Introduction](#introduction)
2. [Backend Architecture Overview](#backend-architecture-overview)
3. [Technology Stack](#technology-stack)
4. [API Layer Solutions](#api-layer-solutions)
5. [Business Logic Layer Solutions](#business-logic-layer-solutions)
6. [Data Access Layer Solutions](#data-access-layer-solutions)
7. [Entity Framework Code First Implementation](#entity-framework-code-first-implementation)
8. [Security Implementation](#security-implementation)
9. [Performance Optimization](#performance-optimization)
10. [Module-Specific Solutions](#module-specific-solutions)
11. [Cross-Cutting Concerns](#cross-cutting-concerns)

---

## Introduction

### Purpose
This document provides comprehensive technical solutions for the NESI application backend layer, detailing how functional and non-functional requirements are addressed through ASP.NET Web API 2 and Entity Framework Code First implementation.

### Scope
This document covers:
- Backend architecture and design patterns
- API endpoint design and implementation
- Entity Framework Code First data modeling
- Business logic layer organization
- Security and authentication mechanisms
- Performance optimization strategies
- Module-specific technical solutions

### Architecture Philosophy
The backend follows **Clean Architecture** principles with clear separation of concerns:
- **API Layer**: HTTP endpoint handling and request/response management
- **Business Logic Layer (BLL)**: Domain logic, validation, and business rules
- **Data Access Layer (DAL)**: Data persistence using Entity Framework
- **Cross-Cutting Concerns**: Logging, caching, authentication, authorization

---

## Backend Architecture Overview

### N-Tier Architecture

```
┌─────────────────────────────────────────────────────────┐
│              Angular Frontend (SPA)                      │
│              Customer/Vendor Portals                     │
└──────────────────┬──────────────────────────────────────┘
                   │ HTTP/HTTPS (JSON)
                   │ OAuth 2.0 Bearer Tokens
                   ▼
┌─────────────────────────────────────────────────────────┐
│           API LAYER (ASP.NET Web API 2)                  │
│  ┌──────────────────────────────────────────────────┐   │
│  │ Controllers (Customer, Quote, WorkOrder, etc.)   │   │
│  │ • Request/Response handling                      │   │
│  │ • Model validation                               │   │
│  │ • Authentication/Authorization filters           │   │
│  └──────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────┐   │
│  │ SignalR Hubs (Real-time notifications)           │   │
│  └──────────────────────────────────────────────────┘   │
└──────────────────┬──────────────────────────────────────┘
                   │ Method Calls
                   ▼
┌─────────────────────────────────────────────────────────┐
│       BUSINESS LOGIC LAYER (BLL)                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │ Services (QuoteService, WOService, etc.)         │   │
│  │ • Business rules and validation                  │   │
│  │ • Workflow orchestration                         │   │
│  │ • Data transformation (Entity ↔ DTO)            │   │
│  │ • Complex calculations                           │   │
│  └──────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────┐   │
│  │ Repositories (IRepository<T> pattern)            │   │
│  └──────────────────────────────────────────────────┘   │
└──────────────────┬──────────────────────────────────────┘
                   │ Repository Pattern
                   ▼
┌─────────────────────────────────────────────────────────┐
│       DATA ACCESS LAYER (DAL)                            │
│  ┌──────────────────────────────────────────────────┐   │
│  │ DbContext (ApplicationDbContext)                 │   │
│  │ • Entity configurations                          │   │
│  │ • DbSet<T> collections                           │   │
│  │ • Migrations                                     │   │
│  └──────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────┐   │
│  │ Entity Models (Code First POCOs)                 │   │
│  │ • Customer, Quote, WorkOrder, Employee, etc.     │   │
│  └──────────────────────────────────────────────────┘   │
└──────────────────┬──────────────────────────────────────┘
                   │ SQL Queries (EF Generated)
                   ▼
┌─────────────────────────────────────────────────────────┐
│           DATABASE LAYER (SQL Server)                    │
│              Tables, Views, Indexes                      │
└─────────────────────────────────────────────────────────┘
```

### Project Structure

```
NESI.Backend/
│
├── NESI.API/                           # Web API Project
│   ├── Controllers/
│   │   ├── CustomerController.cs
│   │   ├── QuoteController.cs
│   │   ├── WorkOrderController.cs
│   │   ├── TimesheetController.cs
│   │   ├── PurchaseOrderController.cs
│   │   ├── EmployeeController.cs
│   │   └── ...
│   ├── Filters/
│   │   ├── PageAuthorizationFilter.cs
│   │   ├── PrivilegeAuthorizationFilter.cs
│   │   ├── ExceptionHandlingFilter.cs
│   │   └── ValidationFilter.cs
│   ├── Models/
│   │   ├── DTOs/                       # Data Transfer Objects
│   │   ├── Requests/                   # API Request models
│   │   └── Responses/                  # API Response models
│   ├── App_Start/
│   │   ├── WebApiConfig.cs
│   │   ├── NinjectConfig.cs            # DI configuration
│   │   └── OAuthConfig.cs
│   └── Startup.cs
│
├── NESI.BLL/                           # Business Logic Layer
│   ├── Services/
│   │   ├── ICustomerService.cs
│   │   ├── CustomerService.cs
│   │   ├── IQuoteService.cs
│   │   ├── QuoteService.cs
│   │   ├── IWorkOrderService.cs
│   │   ├── WorkOrderService.cs
│   │   └── ...
│   ├── Validators/
│   │   ├── QuoteValidator.cs
│   │   ├── WorkOrderValidator.cs
│   │   └── ...
│   ├── Mappers/
│   │   └── AutoMapperProfile.cs
│   └── Helpers/
│       ├── CalculationHelper.cs
│       ├── WorkflowHelper.cs
│       └── ...
│
├── NESI.DAL/                           # Data Access Layer
│   ├── Context/
│   │   └── ApplicationDbContext.cs
│   ├── Entities/                       # Code First Entity Models
│   │   ├── Customer.cs
│   │   ├── Quote.cs
│   │   ├── QuoteLine.cs
│   │   ├── WorkOrder.cs
│   │   ├── WorkOrderLine.cs
│   │   ├── Timesheet.cs
│   │   ├── Employee.cs
│   │   ├── PurchaseOrder.cs
│   │   ├── Vendor.cs
│   │   └── ...
│   ├── Configurations/                 # Fluent API Configurations
│   │   ├── CustomerConfiguration.cs
│   │   ├── QuoteConfiguration.cs
│   │   └── ...
│   ├── Repositories/
│   │   ├── IRepository.cs
│   │   ├── Repository.cs
│   │   ├── ICustomerRepository.cs
│   │   ├── CustomerRepository.cs
│   │   └── ...
│   └── Migrations/                     # EF Migrations
│       └── ...
│
├── NESI.Common/                        # Shared Components
│   ├── Exceptions/
│   │   ├── BusinessException.cs
│   │   ├── ValidationException.cs
│   │   └── UnauthorizedException.cs
│   ├── Constants/
│   │   ├── AppConstants.cs
│   │   └── ErrorMessages.cs
│   ├── Enums/
│   │   ├── QuoteStatus.cs
│   │   ├── WorkOrderStatus.cs
│   │   └── ...
│   └── Extensions/
│       └── StringExtensions.cs
│
└── NESI.SignalR/                       # Real-time Communication
    ├── Hubs/
    │   ├── NotificationHub.cs
    │   └── TaskHub.cs
    └── Services/
        └── NotificationService.cs
```

---

## Technology Stack

### Core Technologies

| Technology | Version | Purpose |
|------------|---------|---------|
| **ASP.NET Web API 2** | 2.2 | RESTful API framework |
| **Entity Framework** | 6.4+ | ORM with Code First approach |
| **C#** | 8.0+ | Primary programming language |
| **.NET Framework** | 4.7.2+ | Runtime environment |
| **SQL Server** | 2016+ | Relational database |
| **OAuth 2.0 / OWIN** | Latest | Authentication/Authorization |
| **SignalR** | 2.4 | Real-time communication |
| **Ninject** | 3.3+ | Dependency injection container |
| **AutoMapper** | 10+ | Object-to-object mapping |
| **FluentValidation** | 9+ | Input validation |
| **Newtonsoft.Json** | 12+ | JSON serialization |

### Supporting Libraries

| Library | Purpose |
|---------|---------|
| **Serilog** | Structured logging |
| **MemoryCache** | In-memory caching |
| **EPPlus** | Excel generation |
| **iTextSharp** | PDF generation |
| **Hangfire** | Background job processing |

---

## API Layer Solutions

### API Controller Design Pattern

All API controllers follow a consistent pattern implementing separation of concerns:

```csharp
namespace NESI.API.Controllers
{
    [RoutePrefix("api/customers")]
    [PageAuthorizationFilter(PageId = 101)] // Page-level authorization
    public class CustomerController : ApiController
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger _logger;

        public CustomerController(ICustomerService customerService, ILogger logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        /// <summary>
        /// Get all customers with optional filtering
        /// Addresses: FR-CRM-001 (Customer Information Management)
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetCustomers(
            [FromUri] CustomerFilterRequest filter = null)
        {
            try
            {
                var customers = await _customerService.GetCustomersAsync(filter);
                return Ok(new ApiResponse<List<CustomerDto>>
                {
                    Success = true,
                    Data = customers,
                    Message = "Customers retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving customers");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get customer by ID
        /// Addresses: FR-CRM-001
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetCustomer(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                
                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(new ApiResponse<CustomerDto>
                {
                    Success = true,
                    Data = customer
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error retrieving customer {id}");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Create new customer
        /// Addresses: FR-CRM-001
        /// </summary>
        [HttpPost]
        [Route("")]
        [PrivilegeAuthorizationFilter(PrivilegeId = 1001)] // Create privilege
        public async Task<IHttpActionResult> CreateCustomer(
            [FromBody] CreateCustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var customerId = await _customerService.CreateCustomerAsync(request);
                var customer = await _customerService.GetCustomerByIdAsync(customerId);

                return Created(
                    new Uri(Request.RequestUri + "/" + customerId),
                    new ApiResponse<CustomerDto>
                    {
                        Success = true,
                        Data = customer,
                        Message = "Customer created successfully"
                    }
                );
            }
            catch (ValidationException vex)
            {
                return BadRequest(vex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating customer");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update existing customer
        /// Addresses: FR-CRM-001
        /// </summary>
        [HttpPut]
        [Route("{id:int}")]
        [PrivilegeAuthorizationFilter(PrivilegeId = 1002)] // Update privilege
        public async Task<IHttpActionResult> UpdateCustomer(
            int id,
            [FromBody] UpdateCustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _customerService.UpdateCustomerAsync(id, request);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Customer updated successfully"
                });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ValidationException vex)
            {
                return BadRequest(vex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error updating customer {id}");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get customer sales history
        /// Addresses: FR-CRM-003 (Customer Sales History)
        /// </summary>
        [HttpGet]
        [Route("{id:int}/sales-history")]
        public async Task<IHttpActionResult> GetCustomerSalesHistory(int id)
        {
            try
            {
                var history = await _customerService.GetSalesHistoryAsync(id);
                
                return Ok(new ApiResponse<CustomerSalesHistoryDto>
                {
                    Success = true,
                    Data = history
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error retrieving sales history for customer {id}");
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get customer contacts
        /// Addresses: FR-CRM-002 (Contact Management)
        /// </summary>
        [HttpGet]
        [Route("{id:int}/contacts")]
        public async Task<IHttpActionResult> GetCustomerContacts(int id)
        {
            try
            {
                var contacts = await _customerService.GetContactsAsync(id);
                
                return Ok(new ApiResponse<List<ContactDto>>
                {
                    Success = true,
                    Data = contacts
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error retrieving contacts for customer {id}");
                return InternalServerError(ex);
            }
        }
    }
}
```

### Standard API Response Model

```csharp
namespace NESI.API.Models.Responses
{
    /// <summary>
    /// Standard API response wrapper for consistent response structure
    /// </summary>
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
    }

    public class PagedApiResponse<T> : ApiResponse<List<T>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
```

### API Routes Design

Following RESTful conventions with hierarchical resource structure:

| HTTP Method | Route | Purpose | Requirement |
|-------------|-------|---------|-------------|
| **Customer Management** |
| GET | `/api/customers` | List customers | FR-CRM-001 |
| GET | `/api/customers/{id}` | Get customer details | FR-CRM-001 |
| POST | `/api/customers` | Create customer | FR-CRM-001 |
| PUT | `/api/customers/{id}` | Update customer | FR-CRM-001 |
| GET | `/api/customers/{id}/contacts` | Get customer contacts | FR-CRM-002 |
| POST | `/api/customers/{id}/contacts` | Add contact | FR-CRM-002 |
| GET | `/api/customers/{id}/sales-history` | Get sales history | FR-CRM-003 |
| GET | `/api/customers/{id}/rates` | Get customer rates | FR-CRM-004 |
| **Quote Management** |
| GET | `/api/quotes` | List quotes | FR-QUOTE-001 |
| GET | `/api/quotes/{id}` | Get quote details | FR-QUOTE-001 |
| POST | `/api/quotes` | Create quote | FR-QUOTE-001 |
| PUT | `/api/quotes/{id}` | Update quote | FR-QUOTE-001 |
| POST | `/api/quotes/{id}/submit` | Submit for approval | FR-QUOTE-003 |
| POST | `/api/quotes/{id}/approve` | Approve quote | FR-QUOTE-003 |
| POST | `/api/quotes/{id}/reject` | Reject quote | FR-QUOTE-003 |
| POST | `/api/quotes/{id}/convert` | Convert to work order | FR-QUOTE-004 |
| GET | `/api/quotes/{id}/lines` | Get quote line items | FR-QUOTE-001 |
| POST | `/api/quotes/{id}/lines` | Add line item | FR-QUOTE-001 |
| PUT | `/api/quotes/{id}/lines/{lineId}` | Update line item | FR-QUOTE-001 |
| DELETE | `/api/quotes/{id}/lines/{lineId}` | Delete line item | FR-QUOTE-001 |
| POST | `/api/quotes/{id}/calculate` | Recalculate totals | FR-QUOTE-002 |
| **Work Order Management** |
| GET | `/api/workorders` | List work orders | FR-WO-001 |
| GET | `/api/workorders/{id}` | Get work order details | FR-WO-001 |
| POST | `/api/workorders` | Create work order | FR-WO-001 |
| PUT | `/api/workorders/{id}` | Update work order | FR-WO-001 |
| GET | `/api/workorders/{id}/tasks` | Get tasks/buckets | FR-WO-002 |
| POST | `/api/workorders/{id}/tasks` | Add task | FR-WO-002 |
| PUT | `/api/workorders/{id}/progress` | Update progress | FR-WO-003 |
| POST | `/api/workorders/{id}/assign` | Assign team members | FR-WO-004 |
| POST | `/api/workorders/{id}/documents` | Upload document | FR-WO-005 |
| **Timesheet Management** |
| GET | `/api/timesheets` | List timesheet entries | FR-TIME-001 |
| POST | `/api/timesheets` | Create time entry | FR-TIME-001 |
| PUT | `/api/timesheets/{id}` | Update time entry | FR-TIME-001 |
| DELETE | `/api/timesheets/{id}` | Delete time entry | FR-TIME-001 |
| POST | `/api/timesheets/vacation` | Request vacation | FR-TIME-003 |
| POST | `/api/timesheets/{id}/approve` | Approve vacation | FR-TIME-003 |
| POST | `/api/timesheets/expense` | Submit expense | FR-TIME-004 |
| POST | `/api/timesheets/transfer` | Transfer time | FR-TIME-005 |
| **Purchase Order Management** |
| GET | `/api/purchaseorders` | List purchase orders | FR-PO-001 |
| GET | `/api/purchaseorders/{id}` | Get PO details | FR-PO-001 |
| POST | `/api/purchaseorders` | Create PO | FR-PO-001 |
| PUT | `/api/purchaseorders/{id}` | Update PO | FR-PO-001 |
| POST | `/api/purchaseorders/{id}/submit` | Submit for approval | FR-PO-002 |
| POST | `/api/purchaseorders/{id}/approve` | Approve PO | FR-PO-002 |
| POST | `/api/purchaseorders/{id}/receive` | Record receipt | FR-PO-004 |
| **Employee Management** |
| GET | `/api/employees` | List employees | FR-HR-001 |
| GET | `/api/employees/{id}` | Get employee details | FR-HR-001 |
| POST | `/api/employees` | Create employee | FR-HR-001 |
| PUT | `/api/employees/{id}` | Update employee | FR-HR-001 |
| PUT | `/api/employees/{id}/wages` | Update wages | FR-HR-002 |
| PUT | `/api/employees/{id}/permissions` | Update permissions | FR-HR-003 |
| POST | `/api/employees/{id}/reviews` | Add performance review | FR-HR-004 |
| POST | `/api/employees/{id}/terminate` | Terminate employee | FR-HR-005 |

---

## Business Logic Layer Solutions

### Service Layer Pattern

The BLL implements the Service Layer pattern with dependency injection:

```csharp
namespace NESI.BLL.Services
{
    public interface IQuoteService
    {
        Task<List<QuoteDto>> GetQuotesAsync(QuoteFilterRequest filter);
        Task<QuoteDto> GetQuoteByIdAsync(int id);
        Task<int> CreateQuoteAsync(CreateQuoteRequest request);
        Task UpdateQuoteAsync(int id, UpdateQuoteRequest request);
        Task<QuoteCalculationResult> CalculateQuoteAsync(int id);
        Task SubmitForApprovalAsync(int id, int userId);
        Task ApproveQuoteAsync(int id, int approverId, string comments);
        Task RejectQuoteAsync(int id, int approverId, string reason);
        Task<int> ConvertToWorkOrderAsync(int quoteId, int userId);
        Task<List<QuoteLineDto>> GetQuoteLinesAsync(int quoteId);
        Task AddQuoteLineAsync(int quoteId, CreateQuoteLineRequest request);
        Task UpdateQuoteLineAsync(int quoteId, int lineId, UpdateQuoteLineRequest request);
        Task DeleteQuoteLineAsync(int quoteId, int lineId);
    }

    public class QuoteService : IQuoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IQuoteCalculator _calculator;
        private readonly IQuoteValidator _validator;
        private readonly IWorkflowService _workflowService;
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;

        public QuoteService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IQuoteCalculator calculator,
            IQuoteValidator validator,
            IWorkflowService workflowService,
            INotificationService notificationService,
            ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _calculator = calculator;
            _validator = validator;
            _workflowService = workflowService;
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// Create new quote with validation and business rules
        /// Addresses: FR-QUOTE-001, FR-QUOTE-002
        /// </summary>
        public async Task<int> CreateQuoteAsync(CreateQuoteRequest request)
        {
            // Validate request
            var validationResult = await _validator.ValidateCreateQuoteAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Verify customer exists
            var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                throw new NotFoundException($"Customer {request.CustomerId} not found");
            }

            // Create quote entity
            var quote = new Quote
            {
                CustomerId = request.CustomerId,
                QuoteDate = DateTime.UtcNow,
                QuoteType = request.QuoteType,
                Description = request.Description,
                EstimatedStartDate = request.EstimatedStartDate,
                EstimatedCompletionDate = request.EstimatedCompletionDate,
                ProjectManagerId = request.ProjectManagerId,
                Terms = request.Terms,
                Status = QuoteStatus.Draft,
                CreatedBy = request.UserId,
                CreatedDate = DateTime.UtcNow,
                BusinessUnitId = request.BusinessUnitId
            };

            // Add line items
            if (request.LineItems != null && request.LineItems.Any())
            {
                foreach (var lineRequest in request.LineItems)
                {
                    var line = new QuoteLine
                    {
                        LineType = lineRequest.LineType,
                        Description = lineRequest.Description,
                        Quantity = lineRequest.Quantity,
                        UnitPrice = lineRequest.UnitPrice,
                        LineTotal = lineRequest.Quantity * lineRequest.UnitPrice,
                        SortOrder = lineRequest.SortOrder
                    };
                    quote.QuoteLines.Add(line);
                }
            }

            // Calculate totals
            CalculateQuoteTotals(quote);

            // Save to database
            await _unitOfWork.Quotes.AddAsync(quote);
            await _unitOfWork.CommitAsync();

            _logger.Information($"Quote {quote.Id} created for customer {customer.Name}");

            return quote.Id;
        }

        /// <summary>
        /// Calculate quote totals
        /// Addresses: FR-QUOTE-002
        /// </summary>
        public async Task<QuoteCalculationResult> CalculateQuoteAsync(int id)
        {
            var quote = await _unitOfWork.Quotes
                .Include(q => q.QuoteLines)
                .Include(q => q.Customer)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quote == null)
            {
                throw new NotFoundException($"Quote {id} not found");
            }

            // Calculate using calculator service
            var result = _calculator.Calculate(quote);

            // Update quote with calculated values
            quote.Subtotal = result.Subtotal;
            quote.TaxRate = result.TaxRate;
            quote.TaxAmount = result.TaxAmount;
            quote.Total = result.GrandTotal;
            quote.ProfitMargin = result.ProfitMargin;
            quote.LastModifiedDate = DateTime.UtcNow;

            await _unitOfWork.CommitAsync();

            return result;
        }

        /// <summary>
        /// Submit quote for approval workflow
        /// Addresses: FR-QUOTE-003
        /// </summary>
        public async Task SubmitForApprovalAsync(int id, int userId)
        {
            var quote = await _unitOfWork.Quotes.GetByIdAsync(id);
            
            if (quote == null)
            {
                throw new NotFoundException($"Quote {id} not found");
            }

            if (quote.Status != QuoteStatus.Draft)
            {
                throw new BusinessException("Only draft quotes can be submitted for approval");
            }

            // Determine if approval is required based on business rules
            var requiresApproval = await _workflowService.RequiresApprovalAsync(quote);

            if (requiresApproval)
            {
                // Route to appropriate approver
                var approver = await _workflowService.GetApproverAsync(quote);

                quote.Status = QuoteStatus.PendingApproval;
                quote.SubmittedBy = userId;
                quote.SubmittedDate = DateTime.UtcNow;
                quote.ApproverId = approver.Id;

                // Send notification to approver
                await _notificationService.NotifyQuoteApprovalRequiredAsync(
                    approver.Id, 
                    quote.Id, 
                    quote.Total);
            }
            else
            {
                // Auto-approve if below threshold
                quote.Status = QuoteStatus.Approved;
                quote.ApprovedBy = userId;
                quote.ApprovedDate = DateTime.UtcNow;
            }

            await _unitOfWork.CommitAsync();

            _logger.Information($"Quote {id} submitted for approval");
        }

        /// <summary>
        /// Approve quote
        /// Addresses: FR-QUOTE-003
        /// </summary>
        public async Task ApproveQuoteAsync(int id, int approverId, string comments)
        {
            var quote = await _unitOfWork.Quotes.GetByIdAsync(id);
            
            if (quote == null)
            {
                throw new NotFoundException($"Quote {id} not found");
            }

            if (quote.Status != QuoteStatus.PendingApproval)
            {
                throw new BusinessException("Quote is not pending approval");
            }

            quote.Status = QuoteStatus.Approved;
            quote.ApprovedBy = approverId;
            quote.ApprovedDate = DateTime.UtcNow;
            quote.ApprovalComments = comments;

            await _unitOfWork.CommitAsync();

            // Notify submitter
            await _notificationService.NotifyQuoteApprovedAsync(
                quote.SubmittedBy.Value, 
                quote.Id);

            _logger.Information($"Quote {id} approved by user {approverId}");
        }

        /// <summary>
        /// Convert approved quote to work order
        /// Addresses: FR-QUOTE-004
        /// </summary>
        public async Task<int> ConvertToWorkOrderAsync(int quoteId, int userId)
        {
            var quote = await _unitOfWork.Quotes
                .Include(q => q.QuoteLines)
                .Include(q => q.Customer)
                .FirstOrDefaultAsync(q => q.Id == quoteId);

            if (quote == null)
            {
                throw new NotFoundException($"Quote {quoteId} not found");
            }

            if (quote.Status != QuoteStatus.Approved)
            {
                throw new BusinessException("Only approved quotes can be converted to work orders");
            }

            // Create work order from quote
            var workOrder = new WorkOrder
            {
                QuoteId = quote.Id,
                CustomerId = quote.CustomerId,
                Description = quote.Description,
                StartDate = quote.EstimatedStartDate,
                CompletionDate = quote.EstimatedCompletionDate,
                ProjectManagerId = quote.ProjectManagerId,
                Status = WorkOrderStatus.Open,
                BudgetAmount = quote.Total,
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                BusinessUnitId = quote.BusinessUnitId
            };

            // Copy line items
            foreach (var quoteLine in quote.QuoteLines)
            {
                var woLine = new WorkOrderLine
                {
                    LineType = quoteLine.LineType,
                    Description = quoteLine.Description,
                    Quantity = quoteLine.Quantity,
                    UnitPrice = quoteLine.UnitPrice,
                    LineTotal = quoteLine.LineTotal,
                    SortOrder = quoteLine.SortOrder
                };
                workOrder.WorkOrderLines.Add(woLine);
            }

            // Update quote status
            quote.Status = QuoteStatus.Converted;
            quote.ConvertedDate = DateTime.UtcNow;
            quote.ConvertedBy = userId;

            // Save work order
            await _unitOfWork.WorkOrders.AddAsync(workOrder);
            await _unitOfWork.CommitAsync();

            // Notify project manager
            if (workOrder.ProjectManagerId.HasValue)
            {
                await _notificationService.NotifyWorkOrderAssignedAsync(
                    workOrder.ProjectManagerId.Value, 
                    workOrder.Id);
            }

            _logger.Information($"Quote {quoteId} converted to work order {workOrder.Id}");

            return workOrder.Id;
        }

        private void CalculateQuoteTotals(Quote quote)
        {
            // Calculate subtotal from line items
            quote.Subtotal = quote.QuoteLines.Sum(l => l.LineTotal);

            // Apply tax rate (get from customer location or default)
            quote.TaxRate = quote.Customer?.TaxRate ?? 0.08m;
            quote.TaxAmount = quote.Subtotal * quote.TaxRate;

            // Calculate grand total
            quote.Total = quote.Subtotal + quote.TaxAmount;

            // Calculate profit margin if cost data available
            var totalCost = quote.QuoteLines.Sum(l => l.Cost ?? 0);
            if (totalCost > 0)
            {
                quote.ProfitMargin = ((quote.Subtotal - totalCost) / quote.Subtotal) * 100;
            }
        }
    }
}
```

### Unit of Work Pattern

```csharp
namespace NESI.DAL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IQuoteRepository Quotes { get; }
        IWorkOrderRepository WorkOrders { get; }
        ITimesheetRepository Timesheets { get; }
        IPurchaseOrderRepository PurchaseOrders { get; }
        IEmployeeRepository Employees { get; }
        IVendorRepository Vendors { get; }
        
        Task<int> CommitAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            
            Customers = new CustomerRepository(_context);
            Quotes = new QuoteRepository(_context);
            WorkOrders = new WorkOrderRepository(_context);
            Timesheets = new TimesheetRepository(_context);
            PurchaseOrders = new PurchaseOrderRepository(_context);
            Employees = new EmployeeRepository(_context);
            Vendors = new VendorRepository(_context);
        }

        public ICustomerRepository Customers { get; }
        public IQuoteRepository Quotes { get; }
        public IWorkOrderRepository WorkOrders { get; }
        public ITimesheetRepository Timesheets { get; }
        public IPurchaseOrderRepository PurchaseOrders { get; }
        public IEmployeeRepository Employees { get; }
        public IVendorRepository Vendors { get; }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
            _transaction?.Dispose();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
```

---

## Data Access Layer Solutions

### Generic Repository Pattern

```csharp
namespace NESI.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<T> DbSet;

        public Repository(ApplicationDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public virtual async Task<int> CountAsync(
            Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null 
                ? await DbSet.CountAsync() 
                : await DbSet.CountAsync(predicate);
        }

        public virtual async Task<bool> AnyAsync(
            Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AnyAsync(predicate);
        }
    }
}
```

### Specific Repository with Custom Methods

```csharp
namespace NESI.DAL.Repositories
{
    public interface IQuoteRepository : IRepository<Quote>
    {
        Task<Quote> GetQuoteWithLinesAsync(int id);
        Task<IEnumerable<Quote>> GetQuotesByCustomerAsync(int customerId);
        Task<IEnumerable<Quote>> GetQuotesByStatusAsync(QuoteStatus status);
        Task<IEnumerable<Quote>> GetPendingApprovalsAsync(int approverId);
        Task<decimal> GetTotalQuoteValueByCustomerAsync(int customerId, DateTime? startDate, DateTime? endDate);
    }

    public class QuoteRepository : Repository<Quote>, IQuoteRepository
    {
        public QuoteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Quote> GetQuoteWithLinesAsync(int id)
        {
            return await Context.Quotes
                .Include(q => q.QuoteLines)
                .Include(q => q.Customer)
                .Include(q => q.ProjectManager)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<Quote>> GetQuotesByCustomerAsync(int customerId)
        {
            return await Context.Quotes
                .Where(q => q.CustomerId == customerId)
                .Include(q => q.QuoteLines)
                .OrderByDescending(q => q.QuoteDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Quote>> GetQuotesByStatusAsync(QuoteStatus status)
        {
            return await Context.Quotes
                .Where(q => q.Status == status)
                .Include(q => q.Customer)
                .OrderByDescending(q => q.QuoteDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Quote>> GetPendingApprovalsAsync(int approverId)
        {
            return await Context.Quotes
                .Where(q => q.Status == QuoteStatus.PendingApproval 
                         && q.ApproverId == approverId)
                .Include(q => q.Customer)
                .OrderBy(q => q.SubmittedDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalQuoteValueByCustomerAsync(
            int customerId, 
            DateTime? startDate, 
            DateTime? endDate)
        {
            var query = Context.Quotes
                .Where(q => q.CustomerId == customerId 
                         && q.Status == QuoteStatus.Approved);

            if (startDate.HasValue)
            {
                query = query.Where(q => q.QuoteDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(q => q.QuoteDate <= endDate.Value);
            }

            return await query.SumAsync(q => q.Total);
        }
    }
}
```

---

## Entity Framework Code First Implementation

### DbContext Configuration

```csharp
namespace NESI.DAL.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=NESIConnection")
        {
            // Configure lazy loading
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
            
            // Configure validation
            Configuration.ValidateOnSaveEnabled = true;
            
            // Configure performance
            Configuration.AutoDetectChangesEnabled = true;
        }

        // DbSets for all entities
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<QuoteLine> QuoteLines { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkOrderLine> WorkOrderLines { get; set; }
        public DbSet<WorkOrderTask> WorkOrderTasks { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<BusinessUnit> BusinessUnits { get; set; }
        public DbSet<UserPageAuth> UserPageAuths { get; set; }
        public DbSet<UserPrivilege> UserPrivileges { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Apply all configurations from assembly
            modelBuilder.Configurations.AddFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Global configurations
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync()
        {
            // Automatically set audit fields
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && 
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = DateTime.UtcNow;
                }
                
                entity.LastModifiedDate = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync();
        }
    }
}
```

### Entity Models (Code First POCOs)

#### Base Entity Class

```csharp
namespace NESI.DAL.Entities
{
    /// <summary>
    /// Base entity with common audit fields
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
```

#### Customer Entity

```csharp
namespace NESI.DAL.Entities
{
    /// <summary>
    /// Customer entity representing client organizations
    /// Addresses: FR-CRM-001, FR-CRM-002, FR-CRM-003, FR-CRM-004
    /// </summary>
    public class Customer : BaseEntity
    {
        public Customer()
        {
            Contacts = new HashSet<Contact>();
            Quotes = new HashSet<Quote>();
            WorkOrders = new HashSet<WorkOrder>();
            CustomerAddresses = new HashSet<CustomerAddress>();
            CustomerRates = new HashSet<CustomerRate>();
        }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(100)]
        public string AccountNumber { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string Fax { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Website { get; set; }

        [StringLength(100)]
        public string TaxId { get; set; }

        public decimal? TaxRate { get; set; }

        [StringLength(50)]
        public string PaymentTerms { get; set; }

        public int? CreditLimit { get; set; }

        public int BusinessUnitId { get; set; }

        [StringLength(20)]
        public string CustomerType { get; set; }

        [StringLength(50)]
        public string Industry { get; set; }

        public bool IsVIP { get; set; }

        public DateTime? LastContactDate { get; set; }

        // Navigation properties
        public virtual BusinessUnit BusinessUnit { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Quote> Quotes { get; set; }
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        public virtual ICollection<CustomerRate> CustomerRates { get; set; }
    }
}
```

#### Quote Entity

```csharp
namespace NESI.DAL.Entities
{
    /// <summary>
    /// Quote entity for customer proposals
    /// Addresses: FR-QUOTE-001, FR-QUOTE-002, FR-QUOTE-003, FR-QUOTE-004
    /// </summary>
    public class Quote : BaseEntity
    {
        public Quote()
        {
            QuoteLines = new HashSet<QuoteLine>();
        }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string QuoteNumber { get; set; }

        public DateTime QuoteDate { get; set; }

        [StringLength(20)]
        public string QuoteType { get; set; } // Time & Material, Fixed Price, Cost Plus

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? EstimatedStartDate { get; set; }

        public DateTime? EstimatedCompletionDate { get; set; }

        public int? ProjectManagerId { get; set; }

        [Column(TypeName = "text")]
        public string Terms { get; set; }

        public QuoteStatus Status { get; set; }

        public decimal Subtotal { get; set; }

        public decimal TaxRate { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal Total { get; set; }

        public decimal? ProfitMargin { get; set; }

        public int BusinessUnitId { get; set; }

        // Approval fields
        public int? SubmittedBy { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public int? ApproverId { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovalComments { get; set; }

        // Conversion tracking
        public int? ConvertedBy { get; set; }
        public DateTime? ConvertedDate { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; }
        public virtual Employee ProjectManager { get; set; }
        public virtual BusinessUnit BusinessUnit { get; set; }
        public virtual ICollection<QuoteLine> QuoteLines { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
    }

    public enum QuoteStatus
    {
        Draft = 1,
        PendingApproval = 2,
        Approved = 3,
        Rejected = 4,
        Sent = 5,
        Converted = 6,
        Expired = 7,
        Cancelled = 8
    }
}
```

#### Quote Line Entity

```csharp
namespace NESI.DAL.Entities
{
    /// <summary>
    /// Quote line item
    /// Addresses: FR-QUOTE-001
    /// </summary>
    public class QuoteLine : BaseEntity
    {
        public int QuoteId { get; set; }

        [Required]
        [StringLength(20)]
        public string LineType { get; set; } // Labor, Material, Equipment, Misc

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public decimal Quantity { get; set; }

        [StringLength(20)]
        public string Unit { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal LineTotal { get; set; }

        public decimal? Cost { get; set; }

        public int SortOrder { get; set; }

        [StringLength(200)]
        public string Notes { get; set; }

        // Navigation properties
        public virtual Quote Quote { get; set; }
    }
}
```

#### Work Order Entity

```csharp
namespace NESI.DAL.Entities
{
    /// <summary>
    /// Work Order entity for project execution
    /// Addresses: FR-WO-001, FR-WO-002, FR-WO-003, FR-WO-004, FR-WO-005
    /// </summary>
    public class WorkOrder : BaseEntity
    {
        public WorkOrder()
        {
            WorkOrderLines = new HashSet<WorkOrderLine>();
            WorkOrderTasks = new HashSet<WorkOrderTask>();
            Timesheets = new HashSet<Timesheet>();
            Documents = new HashSet<WorkOrderDocument>();
            TeamMembers = new HashSet<WorkOrderTeamMember>();
        }

        public int? QuoteId { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string WorkOrderNumber { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public int? ProjectManagerId { get; set; }

        public WorkOrderStatus Status { get; set; }

        public decimal? BudgetAmount { get; set; }

        public decimal? ActualCost { get; set; }

        public int CompletionPercentage { get; set; }

        public int BusinessUnitId { get; set; }

        [StringLength(20)]
        public string Priority { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

        // Navigation properties
        public virtual Quote Quote { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Employee ProjectManager { get; set; }
        public virtual BusinessUnit BusinessUnit { get; set; }
        public virtual ICollection<WorkOrderLine> WorkOrderLines { get; set; }
        public virtual ICollection<WorkOrderTask> WorkOrderTasks { get; set; }
        public virtual ICollection<Timesheet> Timesheets { get; set; }
        public virtual ICollection<WorkOrderDocument> Documents { get; set; }
        public virtual ICollection<WorkOrderTeamMember> TeamMembers { get; set; }
    }

    public enum WorkOrderStatus
    {
        Open = 1,
        Assigned = 2,
        InProgress = 3,
        OnHold = 4,
        Completed = 5,
        Closed = 6,
        Cancelled = 7
    }
}
```

#### Timesheet Entity

```csharp
namespace NESI.DAL.Entities
{
    /// <summary>
    /// Timesheet entity for labor tracking
    /// Addresses: FR-TIME-001, FR-TIME-002, FR-TIME-003, FR-TIME-004
    /// </summary>
    public class Timesheet : BaseEntity
    {
        public int EmployeeId { get; set; }

        public DateTime WorkDate { get; set; }

        public int? WorkOrderId { get; set; }

        public int? QuoteId { get; set; }

        [StringLength(50)]
        public string JobCode { get; set; }

        public PayType PayType { get; set; }

        public decimal Hours { get; set; }

        public decimal? HourlyRate { get; set; }

        public decimal? TotalCost { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsBillable { get; set; }

        public int BusinessUnitId { get; set; }

        public TimesheetStatus Status { get; set; }

        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        // Navigation properties
        public virtual Employee Employee { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        public virtual Quote Quote { get; set; }
        public virtual BusinessUnit BusinessUnit { get; set; }
    }

    public enum PayType
    {
        Regular = 1,
        Overtime = 2,
        DoubleTime = 3,
        Vacation = 4,
        Sick = 5,
        Holiday = 6,
        BankPay = 7
    }

    public enum TimesheetStatus
    {
        Draft = 1,
        Submitted = 2,
        Approved = 3,
        Rejected = 4
    }
}
```

#### Purchase Order Entity

```csharp
namespace NESI.DAL.Entities
{
    /// <summary>
    /// Purchase Order entity for procurement
    /// Addresses: FR-PO-001, FR-PO-002, FR-PO-003, FR-PO-004
    /// </summary>
    public class PurchaseOrder : BaseEntity
    {
        public PurchaseOrder()
        {
            PurchaseOrderLines = new HashSet<PurchaseOrderLine>();
            BusinessUnitAllocations = new HashSet<POBusinessUnitAllocation>();
        }

        public int VendorId { get; set; }

        [Required]
        [StringLength(50)]
        public string PONumber { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public int? WorkOrderId { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public POStatus Status { get; set; }

        public decimal Subtotal { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal Total { get; set; }

        [StringLength(50)]
        public string PaymentTerms { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public DateTime? ReceivedDate { get; set; }
        public int? ReceivedBy { get; set; }

        // Navigation properties
        public virtual Vendor Vendor { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        public virtual ICollection<POBusinessUnitAllocation> BusinessUnitAllocations { get; set; }
    }

    public enum POStatus
    {
        Draft = 1,
        PendingApproval = 2,
        Approved = 3,
        Ordered = 4,
        PartiallyReceived = 5,
        Received = 6,
        Closed = 7,
        Cancelled = 8
    }
}
```

#### Employee Entity

```csharp
namespace NESI.DAL.Entities
{
    /// <summary>
    /// Employee entity for HR management
    /// Addresses: FR-HR-001, FR-HR-002, FR-HR-003, FR-HR-004, FR-HR-005
    /// </summary>
    public class Employee : BaseEntity
    {
        public Employee()
        {
            Timesheets = new HashSet<Timesheet>();
            WageHistory = new HashSet<EmployeeWage>();
            Reviews = new HashSet<EmployeeReview>();
            PageAuths = new HashSet<UserPageAuth>();
            Privileges = new HashSet<UserPrivilege>();
        }

        [Required]
        [StringLength(50)]
        public string EmployeeNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public DateTime? HireDate { get; set; }

        [StringLength(100)]
        public string JobTitle { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        public int? ManagerId { get; set; }

        public decimal? CurrentHourlyRate { get; set; }

        public decimal? CurrentSalary { get; set; }

        public EmploymentType EmploymentType { get; set; }

        public EmploymentStatus Status { get; set; }

        public DateTime? TerminationDate { get; set; }
        public string TerminationReason { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(20)]
        public string State { get; set; }

        [StringLength(20)]
        public string ZipCode { get; set; }

        [StringLength(100)]
        public string EmergencyContactName { get; set; }

        [StringLength(20)]
        public string EmergencyContactPhone { get; set; }

        // Navigation properties
        public virtual Employee Manager { get; set; }
        public virtual ICollection<Timesheet> Timesheets { get; set; }
        public virtual ICollection<EmployeeWage> WageHistory { get; set; }
        public virtual ICollection<EmployeeReview> Reviews { get; set; }
        public virtual ICollection<UserPageAuth> PageAuths { get; set; }
        public virtual ICollection<UserPrivilege> Privileges { get; set; }
    }

    public enum EmploymentType
    {
        FullTime = 1,
        PartTime = 2,
        Contract = 3,
        Temporary = 4
    }

    public enum EmploymentStatus
    {
        Active = 1,
        OnLeave = 2,
        Suspended = 3,
        Terminated = 4
    }
}
```

### Fluent API Configuration

```csharp
namespace NESI.DAL.Configurations
{
    /// <summary>
    /// Customer entity configuration
    /// </summary>
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            // Table mapping
            ToTable("Customer");

            // Primary key
            HasKey(c => c.Id);

            // Properties
            Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            Property(c => c.AccountNumber)
                .HasMaxLength(100);

            Property(c => c.Email)
                .HasMaxLength(200);

            Property(c => c.TaxRate)
                .HasPrecision(5, 4);

            // Indexes
            HasIndex(c => c.Name)
                .HasName("IX_Customer_Name");

            HasIndex(c => c.AccountNumber)
                .IsUnique()
                .HasName("IX_Customer_AccountNumber");

            // Relationships
            HasRequired(c => c.BusinessUnit)
                .WithMany(bu => bu.Customers)
                .HasForeignKey(c => c.BusinessUnitId)
                .WillCascadeOnDelete(false);

            HasMany(c => c.Contacts)
                .WithRequired(co => co.Customer)
                .HasForeignKey(co => co.CustomerId)
                .WillCascadeOnDelete(true);

            HasMany(c => c.Quotes)
                .WithRequired(q => q.Customer)
                .HasForeignKey(q => q.CustomerId)
                .WillCascadeOnDelete(false);
        }
    }

    /// <summary>
    /// Quote entity configuration
    /// </summary>
    public class QuoteConfiguration : EntityTypeConfiguration<Quote>
    {
        public QuoteConfiguration()
        {
            ToTable("Quote");

            HasKey(q => q.Id);

            Property(q => q.QuoteNumber)
                .IsRequired()
                .HasMaxLength(50);

            Property(q => q.Description)
                .IsRequired()
                .HasMaxLength(500);

            Property(q => q.Subtotal)
                .HasPrecision(18, 2);

            Property(q => q.TaxRate)
                .HasPrecision(5, 4);

            Property(q => q.TaxAmount)
                .HasPrecision(18, 2);

            Property(q => q.Total)
                .HasPrecision(18, 2);

            Property(q => q.ProfitMargin)
                .HasPrecision(5, 2);

            // Indexes
            HasIndex(q => q.QuoteNumber)
                .IsUnique()
                .HasName("IX_Quote_QuoteNumber");

            HasIndex(q => q.CustomerId)
                .HasName("IX_Quote_CustomerId");

            HasIndex(q => q.Status)
                .HasName("IX_Quote_Status");

            HasIndex(q => q.QuoteDate)
                .HasName("IX_Quote_QuoteDate");

            // Relationships
            HasRequired(q => q.Customer)
                .WithMany(c => c.Quotes)
                .HasForeignKey(q => q.CustomerId)
                .WillCascadeOnDelete(false);

            HasOptional(q => q.ProjectManager)
                .WithMany()
                .HasForeignKey(q => q.ProjectManagerId)
                .WillCascadeOnDelete(false);

            HasMany(q => q.QuoteLines)
                .WithRequired(ql => ql.Quote)
                .HasForeignKey(ql => ql.QuoteId)
                .WillCascadeOnDelete(true);

            HasOptional(q => q.WorkOrder)
                .WithOptionalPrincipal(wo => wo.Quote)
                .Map(m => m.MapKey("QuoteId"));
        }
    }

    /// <summary>
    /// WorkOrder entity configuration
    /// </summary>
    public class WorkOrderConfiguration : EntityTypeConfiguration<WorkOrder>
    {
        public WorkOrderConfiguration()
        {
            ToTable("WorkOrder");

            HasKey(wo => wo.Id);

            Property(wo => wo.WorkOrderNumber)
                .IsRequired()
                .HasMaxLength(50);

            Property(wo => wo.Description)
                .IsRequired()
                .HasMaxLength(500);

            Property(wo => wo.BudgetAmount)
                .HasPrecision(18, 2);

            Property(wo => wo.ActualCost)
                .HasPrecision(18, 2);

            // Indexes
            HasIndex(wo => wo.WorkOrderNumber)
                .IsUnique()
                .HasName("IX_WorkOrder_WorkOrderNumber");

            HasIndex(wo => wo.CustomerId)
                .HasName("IX_WorkOrder_CustomerId");

            HasIndex(wo => wo.Status)
                .HasName("IX_WorkOrder_Status");

            // Relationships
            HasRequired(wo => wo.Customer)
                .WithMany(c => c.WorkOrders)
                .HasForeignKey(wo => wo.CustomerId)
                .WillCascadeOnDelete(false);

            HasOptional(wo => wo.ProjectManager)
                .WithMany()
                .HasForeignKey(wo => wo.ProjectManagerId)
                .WillCascadeOnDelete(false);
        }
    }
}
```

### Code First Migrations

```csharp
namespace NESI.DAL.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 200),
                    AccountNumber = c.String(maxLength: 100),
                    Description = c.String(maxLength: 500),
                    Phone = c.String(maxLength: 20),
                    Email = c.String(maxLength: 200),
                    TaxRate = c.Decimal(precision: 5, scale: 4),
                    PaymentTerms = c.String(maxLength: 50),
                    BusinessUnitId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    CreatedBy = c.Int(),
                    LastModifiedDate = c.DateTime(),
                    LastModifiedBy = c.Int(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessUnit", t => t.BusinessUnitId)
                .Index(t => t.Name, name: "IX_Customer_Name")
                .Index(t => t.AccountNumber, unique: true, name: "IX_Customer_AccountNumber")
                .Index(t => t.BusinessUnitId);

            CreateTable(
                "dbo.Quote",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CustomerId = c.Int(nullable: false),
                    QuoteNumber = c.String(nullable: false, maxLength: 50),
                    QuoteDate = c.DateTime(nullable: false),
                    QuoteType = c.String(maxLength: 20),
                    Description = c.String(nullable: false, maxLength: 500),
                    Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TaxRate = c.Decimal(nullable: false, precision: 5, scale: 4),
                    TaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Status = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    CreatedBy = c.Int(),
                    LastModifiedDate = c.DateTime(),
                    LastModifiedBy = c.Int(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId)
                .Index(t => t.QuoteNumber, unique: true, name: "IX_Quote_QuoteNumber")
                .Index(t => t.CustomerId, name: "IX_Quote_CustomerId")
                .Index(t => t.Status, name: "IX_Quote_Status");

            // Additional table creation code...
        }

        public override void Down()
        {
            DropForeignKey("dbo.Quote", "CustomerId", "dbo.Customer");
            DropIndex("dbo.Quote", "IX_Quote_Status");
            DropIndex("dbo.Quote", "IX_Quote_CustomerId");
            DropIndex("dbo.Quote", "IX_Quote_QuoteNumber");
            DropIndex("dbo.Customer", "IX_Customer_BusinessUnitId");
            DropIndex("dbo.Customer", "IX_Customer_AccountNumber");
            DropIndex("dbo.Customer", "IX_Customer_Name");
            DropTable("dbo.Quote");
            DropTable("dbo.Customer");
        }
    }
}
```

---

## Security Implementation

### OAuth 2.0 Authentication Configuration

```csharp
namespace NESI.API
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure OAuth options
            var oAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new NesiAuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(
                    Convert.ToDouble(ConfigurationManager.AppSettings["TokenExpiration"])),
                AllowInsecureHttp = bool.Parse(
                    ConfigurationManager.AppSettings["AllowInsecureHttp"]),
                RefreshTokenProvider = new NesiRefreshTokenProvider()
            };

            // Enable OAuth Bearer tokens
            app.UseOAuthAuthorizationServer(oAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
```

### Authorization Server Provider

```csharp
namespace NESI.API.Providers
{
    public class NesiAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(
            OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(
            OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add(
                "Access-Control-Allow-Origin", 
                new[] { "*" });

            using (var authRepo = new AuthRepository())
            {
                var user = await authRepo.FindUserAsync(
                    context.UserName, 
                    context.Password);

                if (user == null)
                {
                    context.SetError(
                        "invalid_grant", 
                        "The user name or password is incorrect.");
                    return;
                }

                if (!user.IsActive)
                {
                    context.SetError(
                        "invalid_grant", 
                        "User account is inactive.");
                    return;
                }

                // Create claims identity
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                identity.AddClaim(new Claim("EmployeeId", user.EmployeeId.ToString()));
                identity.AddClaim(new Claim("BusinessUnitId", user.BusinessUnitId.ToString()));
                identity.AddClaim(new Claim("UserType", user.UserType));

                // Add page authorizations as claims
                foreach (var pageAuth in user.PageAuths)
                {
                    identity.AddClaim(new Claim("PageAuth", pageAuth.PageId.ToString()));
                }

                // Add privileges as claims
                foreach (var privilege in user.Privileges)
                {
                    identity.AddClaim(new Claim("Privilege", privilege.PrivilegeId.ToString()));
                }

                // Create authentication ticket
                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { "userId", user.Id.ToString() },
                    { "userName", user.Username },
                    { "employeeId", user.EmployeeId.ToString() }
                });

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);

                // Log successful login
                await authRepo.LogLoginAsync(user.Id, context.Request.RemoteIpAddress);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}
```

### Authorization Filters

#### Page-Level Authorization

```csharp
namespace NESI.API.Filters
{
    /// <summary>
    /// Page-level authorization filter
    /// Addresses: NFR-SEC-002 (Authorization Controls)
    /// </summary>
    public class PageAuthorizationFilterAttribute : AuthorizeAttribute
    {
        public int PageId { get; set; }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!base.IsAuthorized(actionContext))
            {
                return false;
            }

            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            if (principal == null)
            {
                return false;
            }

            // Check if user has page authorization claim
            var pageAuthClaims = principal.FindAll("PageAuth");
            return pageAuthClaims.Any(c => c.Value == PageId.ToString());
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(
                HttpStatusCode.Forbidden,
                new ApiResponse
                {
                    Success = false,
                    Message = "You do not have permission to access this page."
                });
        }
    }
}
```

#### Privilege-Level Authorization

```csharp
namespace NESI.API.Filters
{
    /// <summary>
    /// Privilege-level authorization filter
    /// Addresses: NFR-SEC-002 (Authorization Controls)
    /// </summary>
    public class PrivilegeAuthorizationFilterAttribute : AuthorizeAttribute
    {
        public int PrivilegeId { get; set; }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!base.IsAuthorized(actionContext))
            {
                return false;
            }

            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            if (principal == null)
            {
                return false;
            }

            // Check if user has privilege claim
            var privilegeClaims = principal.FindAll("Privilege");
            return privilegeClaims.Any(c => c.Value == PrivilegeId.ToString());
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(
                HttpStatusCode.Forbidden,
                new ApiResponse
                {
                    Success = false,
                    Message = "You do not have permission to perform this action."
                });
        }
    }
}
```

---

## Performance Optimization

### Caching Strategy

```csharp
namespace NESI.BLL.Services
{
    public class CachingService : ICachingService
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;
        private const int DefaultCacheMinutes = 30;

        public T Get<T>(string key)
        {
            return (T)Cache.Get(key);
        }

        public void Set<T>(string key, T value, int minutes = DefaultCacheMinutes)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(minutes)
            };

            Cache.Set(key, value, policy);
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public bool Exists(string key)
        {
            return Cache.Contains(key);
        }
    }
}
```

### Query Optimization Patterns

```csharp
namespace NESI.DAL.Repositories
{
    public class OptimizedQuoteRepository : QuoteRepository
    {
        public async Task<IEnumerable<QuoteListDto>> GetQuoteListOptimizedAsync(
            QuoteFilterRequest filter)
        {
            // Use projection to select only needed fields
            // Reduces data transfer and improves performance
            var query = Context.Quotes
                .Where(q => q.IsActive)
                .Select(q => new QuoteListDto
                {
                    Id = q.Id,
                    QuoteNumber = q.QuoteNumber,
                    CustomerName = q.Customer.Name,
                    QuoteDate = q.QuoteDate,
                    Total = q.Total,
                    Status = q.Status
                });

            // Apply filters
            if (filter.CustomerId.HasValue)
            {
                query = query.Where(q => q.CustomerId == filter.CustomerId.Value);
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(q => q.Status == filter.Status.Value);
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(q => q.QuoteDate >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(q => q.QuoteDate <= filter.EndDate.Value);
            }

            // Order and paginate
            query = query
                .OrderByDescending(q => q.QuoteDate)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize);

            return await query.ToListAsync();
        }

        public async Task<Quote> GetQuoteWithDetailsAsync(int id)
        {
            // Use eager loading with Include to avoid N+1 queries
            return await Context.Quotes
                .Include(q => q.Customer)
                .Include(q => q.QuoteLines)
                .Include(q => q.ProjectManager)
                .Include(q => q.BusinessUnit)
                .AsNoTracking() // Read-only, improves performance
                .FirstOrDefaultAsync(q => q.Id == id);
        }
    }
}
```

### Async/Await Throughout

All data access methods use async/await for non-blocking I/O:

```csharp
// Service layer
public async Task<CustomerDto> GetCustomerAsync(int id)
{
    var customer = await _unitOfWork.Customers.GetByIdAsync(id);
    return _mapper.Map<CustomerDto>(customer);
}

// Repository layer
public async Task<Customer> GetByIdAsync(int id)
{
    return await DbSet.FindAsync(id);
}

// Controller layer
[HttpGet]
[Route("{id}")]
public async Task<IHttpActionResult> GetCustomer(int id)
{
    var customer = await _customerService.GetCustomerAsync(id);
    return Ok(customer);
}
```

---

## Module-Specific Solutions

### Quote Management Module

**Technical Implementation:**
- Quote creation with line item management
- Automatic calculation engine for totals and taxes
- Approval workflow with configurable thresholds
- PDF generation for customer delivery
- Conversion to work order with data transfer

**Key Components:**
- `QuoteController` - API endpoints
- `QuoteService` - Business logic
- `QuoteCalculator` - Calculation engine
- `QuoteValidator` - Validation rules
- `Quote`, `QuoteLine` entities - Data models

### Work Order Management Module

**Technical Implementation:**
- Work order lifecycle management
- Task/bucket organization
- Team member assignment with notifications
- Progress tracking and status updates
- Document attachment and storage

**Key Components:**
- `WorkOrderController` - API endpoints
- `WorkOrderService` - Business logic
- `WorkOrder`, `WorkOrderLine`, `WorkOrderTask` entities
- `NotificationService` - Real-time updates via SignalR

### Timesheet Module

**Technical Implementation:**
- Time entry with work order allocation
- Multiple pay type support
- Vacation and PTO management
- Expense submission and approval
- Time transfer capabilities

**Key Components:**
- `TimesheetController` - API endpoints
- `TimesheetService` - Business logic
- `Timesheet` entity
- `PayrollCalculator` - Wage calculations

---

## Cross-Cutting Concerns

### Logging

```csharp
namespace NESI.Common.Logging
{
    public interface ILogger
    {
        void Information(string message);
        void Warning(string message);
        void Error(Exception exception, string message);
        void Debug(string message);
    }

    public class SerilogLogger : ILogger
    {
        private readonly Serilog.ILogger _logger;

        public SerilogLogger()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.File("logs/nesi-.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();
        }

        public void Information(string message)
        {
            _logger.Information(message);
        }

        public void Warning(string message)
        {
            _logger.Warning(message);
        }

        public void Error(Exception exception, string message)
        {
            _logger.Error(exception, message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }
    }
}
```

### Exception Handling

```csharp
namespace NESI.API.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public GlobalExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            _logger.Error(context.Exception, "Unhandled exception in API");

            var response = new ApiResponse
            {
                Success = false,
                Message = "An error occurred processing your request."
            };

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            if (context.Exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                response.Message = context.Exception.Message;
            }
            else if (context.Exception is ValidationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                response.Message = context.Exception.Message;
                response.Errors = ((ValidationException)context.Exception).Errors.ToList();
            }
            else if (context.Exception is UnauthorizedException)
            {
                statusCode = HttpStatusCode.Forbidden;
                response.Message = "You do not have permission to perform this action.";
            }

            context.Response = context.Request.CreateResponse(statusCode, response);
        }
    }
}
```

### Dependency Injection (Ninject)

```csharp
namespace NESI.API.App_Start
{
    public class NinjectConfig
    {
        public static void RegisterServices(IKernel kernel)
        {
            // DbContext
            kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();

            // Unit of Work
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();

            // Repositories
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>)).InRequestScope();
            kernel.Bind<ICustomerRepository>().To<CustomerRepository>().InRequestScope();
            kernel.Bind<IQuoteRepository>().To<QuoteRepository>().InRequestScope();
            kernel.Bind<IWorkOrderRepository>().To<WorkOrderRepository>().InRequestScope();

            // Services
            kernel.Bind<ICustomerService>().To<CustomerService>().InRequestScope();
            kernel.Bind<IQuoteService>().To<QuoteService>().InRequestScope();
            kernel.Bind<IWorkOrderService>().To<WorkOrderService>().InRequestScope();
            kernel.Bind<ITimesheetService>().To<TimesheetService>().InRequestScope();
            kernel.Bind<IPurchaseOrderService>().To<PurchaseOrderService>().InRequestScope();
            kernel.Bind<IEmployeeService>().To<EmployeeService>().InRequestScope();

            // Cross-cutting concerns
            kernel.Bind<ILogger>().To<SerilogLogger>().InSingletonScope();
            kernel.Bind<ICachingService>().To<CachingService>().InSingletonScope();
            kernel.Bind<INotificationService>().To<NotificationService>().InRequestScope();

            // AutoMapper
            kernel.Bind<IMapper>().ToMethod(ctx => 
                new MapperConfiguration(cfg => 
                    cfg.AddProfile<AutoMapperProfile>()).CreateMapper()
            ).InSingletonScope();
        }
    }
}
```

---

## Requirements Traceability

### Requirement-to-Solution Mapping

| Requirement ID | Solution Component | Implementation Details |
|----------------|-------------------|------------------------|
| **FR-CRM-001** | CustomerController, CustomerService, Customer Entity | Full CRUD operations via REST API with EF Code First |
| **FR-CRM-002** | Contact Entity, ContactService | One-to-many relationship with Customer |
| **FR-CRM-003** | CustomerService.GetSalesHistoryAsync() | Aggregate queries across Quote and WorkOrder entities |
| **FR-CRM-004** | CustomerRate Entity | Separate table with date ranges and pricing |
| **FR-QUOTE-001** | QuoteController, QuoteService, Quote/QuoteLine Entities | Complete quote management with line items |
| **FR-QUOTE-002** | QuoteCalculator, CalculateQuoteAsync() | Automated calculation service |
| **FR-QUOTE-003** | WorkflowService, Approval routing logic | Configurable approval thresholds |
| **FR-QUOTE-004** | ConvertToWorkOrderAsync() | Entity transformation with data migration |
| **FR-WO-001** | WorkOrderController, WorkOrderService | Work order lifecycle management |
| **FR-WO-002** | WorkOrderTask Entity | Task organization within work orders |
| **FR-WO-003** | UpdateProgressAsync() | Progress percentage tracking |
| **FR-WO-004** | WorkOrderTeamMember Entity | Many-to-many relationship |
| **FR-WO-005** | WorkOrderDocument Entity | File upload and storage |
| **FR-TIME-001** | TimesheetController, Timesheet Entity | Time entry with work order allocation |
| **FR-TIME-002** | PayType enum, TimesheetService calculations | Multiple pay type support |
| **FR-TIME-003** | Vacation request workflow | Status-based approval process |
| **FR-TIME-004** | Expense Entity | Expense submission and approval |
| **FR-PO-001** | PurchaseOrderController, PO Entities | Procurement management |
| **FR-PO-002** | PO Approval workflow | Dollar threshold routing |
| **FR-PO-003** | POBusinessUnitAllocation Entity | Cost distribution |
| **FR-HR-001** | EmployeeController, Employee Entity | HR data management |
| **FR-HR-002** | EmployeeWage Entity | Wage history tracking |
| **FR-HR-003** | UserPageAuth, UserPrivilege Entities | Permission management |
| **NFR-SEC-001** | OAuth 2.0 OWIN implementation | Industry-standard authentication |
| **NFR-SEC-002** | PageAuthorizationFilter, PrivilegeAuthorizationFilter | Multi-level authorization |
| **NFR-PERF-001** | Async/await, caching, query optimization | Performance optimizations |
| **NFR-REL-002** | Audit fields in BaseEntity | Automated timestamp tracking |

---

## Summary

This document provides comprehensive technical solutions for the NESI application backend layer using **ASP.NET Web API 2** and **Entity Framework Code First**. The architecture follows industry best practices with:

- **Clean separation of concerns** across API, Business Logic, and Data Access layers
- **RESTful API design** with consistent endpoint structure
- **Entity Framework Code First** for maintainable data models
- **Repository and Unit of Work patterns** for data access abstraction
- **OAuth 2.0 authentication** with claims-based authorization
- **Comprehensive security** at page and privilege levels
- **Performance optimization** through async operations and caching
- **Complete traceability** from requirements to implementation

All functional requirements are addressed with specific technical components, ensuring the system meets business needs while maintaining code quality, security, and performance standards.

---

**Document Status:** Approved for Implementation  
**Maintained By:** Technical Architecture Team  
**Review Frequency:** Monthly or upon major requirement changes
