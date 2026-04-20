# NESI Demo Application - Local Development

**Production-ready demonstration of NESI using modern future-state technologies**

## 🎯 Overview

This is a comprehensive demo application showcasing the NESI system rebuilt with modern technologies and best practices. The application demonstrates these key use cases:

1. **Authentication & Dashboard** - JWT-based authentication with personalized dashboard
2. **Customer Management** - Full CRUD operations with search, pagination, and soft delete
3. **Quote Management** - Multi-line quotes with approval workflow and conversion to work orders
4. **Work Order Management** - Track and manage field service work orders
5. **Employee Timesheet Management** - Complete CRUD operations with business rule enforcement

## 🏗️ Technology Stack

### Backend
- **Framework:** ASP.NET Core 9 Web API (.NET 9)
- **Language:** C# 12
- **Architecture:** Clean Architecture (Onion/Hexagonal)
- **Patterns:** CQRS (MediatR), Repository, Dependency Injection
- **ORM:** Entity Framework Core 8 (Code-First)
- **Database:** SQL Server (LocalDB/Express/Azure SQL)
- **Authentication:** Header-based (ready for JWT upgrade)
- **Validation:** FluentValidation
- **API Documentation:** Swagger/OpenAPI 3.0
- **Logging:** ASP.NET Core Logging
- **Testing:** Ready for xUnit, Moq, FluentAssertions

### Frontend
- **Framework:** Angular 19.2+ (standalone components)
- **Language:** TypeScript 5.4+
- **State Management:** Signals (Angular 19)
- **State Management:** NgRx or Angular Signals
- **Forms:** Reactive Forms
- **HTTP:** HttpClient with interceptors
- **Testing:** Jest, Angular Testing Library, Playwright
- **Security:** Using patched version (Angular 18.x has unpatched XSS/XSRF vulnerabilities)

### Development Tools
- **Code Quality:** ESLint, Prettier (FE) | StyleCop, EditorConfig (BE)
- **API Testing:** Postman collections
- **Version Control:** Git

## 📋 Prerequisites

Before you begin, ensure you have the following installed:

- **Node.js** 20.x or higher
- **.NET SDK** 8.0 or higher
- **SQL Server** - LocalDB (Windows), Express (Windows/Linux), or Docker
- **Git** (for version control)
- **Visual Studio Code** or **Visual Studio 2022** (recommended)

### Verify Installations

```bash
# Check Node.js version
node --version  # Should be v20.x or higher

# Check .NET version
dotnet --version  # Should be 8.0.x or higher

# Check SQL Server (Windows with LocalDB)
sqllocaldb info  # Should show mssqllocaldb

# Or check SQL Server (Linux/Docker)
docker ps  # If using Docker
```

## 🚀 Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/abhigaonkar/Nesi.git
cd Nesi/CoPilotGeneratedCode
```

### 2. Database Setup

The application uses **SQL Server**. For detailed setup instructions for all SQL Server options (LocalDB, Express, Docker, Azure SQL), see [Database Configuration Guide](docs/DATABASE_CONFIGURATION.md).

**Quick Setup (Windows LocalDB - Recommended):**

LocalDB is automatically installed with Visual Studio and .NET SDK.

```bash
cd backend/src/Nesi.Infrastructure

# Run migrations to create the database
dotnet ef database update --startup-project ../Nesi.Api
```

**Alternative: SQL Server Express or Docker** - See the [Database Configuration Guide](docs/DATABASE_CONFIGURATION.md) for detailed instructions.

### 3. Backend Setup

```bash
cd backend/src/Nesi.Api

# Restore packages
dotnet restore

# Run the application
dotnet run
```

The backend API will be available at: `http://localhost:5000`  
Swagger documentation: `http://localhost:5000/swagger`

**Note:** The default connection string in `appsettings.json` is already configured for LocalDB. For other SQL Server configurations, update the connection string as described in the [Database Configuration Guide](docs/DATABASE_CONFIGURATION.md).

### 4. Frontend Setup

```bash
# Open a new terminal
cd frontend

# Install dependencies
npm install

# Run the application
npm start
```

The frontend will be available at: `http://localhost:4200`

### 5. Using Helper Scripts

For convenience, use the provided helper scripts:

**Windows:**
```cmd
scripts\run-all.bat
```

**Linux/Mac:**
```bash
chmod +x scripts/run-all.sh
./scripts/run-all.sh
```

## 📂 Project Structure

```
CoPilotGeneratedCode/
├── Guidelines/              # 📘 CODING STANDARDS (READ FIRST!)
│   ├── README.md
│   ├── backend-guidelines.md
│   ├── frontend-guidelines.md
│   ├── database-guidelines.md
│   ├── testing-guidelines.md
│   └── naming-conventions.md
├── backend/                 # ASP.NET Core 8 Web API
│   ├── src/
│   │   ├── Nesi.Api/        # Controllers, Middleware
│   │   ├── Nesi.Application/# CQRS Handlers, DTOs, Validators
│   │   ├── Nesi.Domain/     # Entities, Business Rules
│   │   └── Nesi.Infrastructure/ # EF Core, Repositories
│   └── tests/               # xUnit Tests
├── frontend/                # Angular 18 Application
│   └── src/app/
│       ├── core/            # Services, Guards, Interceptors
│       ├── features/        # Feature Modules
│       │   ├── auth/
│       │   ├── dashboard/
│       │   └── timesheet/
│       └── shared/          # Reusable Components
├── database/                # SQL Scripts, Migrations
│   ├── migrations/
│   └── seed-data/
├── scripts/                 # Helper Scripts
│   ├── run-backend.sh/bat
│   ├── run-frontend.sh/bat
│   └── run-all.sh/bat
├── docs/                    # Documentation
│   ├── ARCHITECTURE.md
│   ├── API_REFERENCE.md
│   ├── USER_GUIDE.md
│   └── DEMO_SCRIPT.md
├── postman/                 # API Testing
│   ├── NESI-Demo.postman_collection.json
│   └── NESI-Demo.postman_environment.json
└── README.md               # This file
```

## 🎨 Key Features

### Authentication & Authorization
- JWT-based authentication
- Role-based access control (Admin, Manager, Employee)
- Secure password handling
- Token refresh mechanism

### Customer Management
- Create, edit, view, delete customers (soft delete)
- Search by name, customer number, email, phone
- Filter by active/inactive status
- Pagination for large datasets
- Add addresses, contacts, and activity notes
- Auto-generated customer numbers (CUST-YYYY-NNNNN)

### Quote Management
- Create quotes with multiple line items
- Quote types: Time & Material, Fixed Price, Cost Plus
- Automatic calculations (subtotal, tax, discount, total)
- Multi-step approval workflow:
  - Submit for approval
  - Manager approval
  - Customer approval
- Convert approved quotes to work orders
- Quote versioning and revision support

### Work Order Management
- Convert quotes to work orders
- Crew assignment and scheduling
- Status tracking
- Link to original quote
- Timesheet integration

### Dashboard
- Personalized user dashboard
- Pending timesheets summary
- Weekly hours total
- Quick action buttons

### Timesheet Management
- Create, edit, delete timesheet entries
- Date picker for entry date
- Dropdown selections for work order, pay type, job type
- Hours validation (0.25 - 24 hours)
- Submit for approval workflow
- Filter by date range and status
- Sortable, paginated data grid

### Business Rules
- Cannot exceed 24 hours per day per user
- Cannot enter future timesheets
- Cannot edit submitted/approved timesheets
- Overtime calculation after 40 hours/week
- Credit limit tracking for customers
- Quote approval required before work order conversion

## 👥 Demo User Accounts

The application comes with pre-seeded demo accounts:

| Username | Password | Role | Description |
|----------|----------|------|-------------|
| sarah.tech | Demo123! | Employee | Regular employee user |
| john.manager | Demo123! | Manager | Manager with approval rights |
| admin | Admin123! | Admin | System administrator |

## 🧪 Running Tests

### Backend Tests

```bash
cd backend
dotnet test

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Frontend Tests

```bash
cd frontend

# Unit tests
npm test

# With coverage
npm test -- --coverage

# E2E tests
npm run test:e2e
```

## 📖 Guidelines

**IMPORTANT:** All code in this project follows strict coding guidelines. Before making any changes:

1. Read the [Guidelines README](/CoPilotGeneratedCode/Guidelines/README.md)
2. Review relevant guideline documents:
   - [Backend Guidelines](/CoPilotGeneratedCode/Guidelines/backend-guidelines.md)
   - [Frontend Guidelines](/CoPilotGeneratedCode/Guidelines/frontend-guidelines.md)
   - [Database Guidelines](/CoPilotGeneratedCode/Guidelines/database-guidelines.md)
   - [Testing Guidelines](/CoPilotGeneratedCode/Guidelines/testing-guidelines.md)
   - [Naming Conventions](/CoPilotGeneratedCode/Guidelines/naming-conventions.md)

These guidelines ensure consistency, maintainability, and quality across the codebase.

## 🔧 Common Tasks

### Adding a New Migration

```bash
cd backend/src/Nesi.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../Nesi.Api
dotnet ef database update --startup-project ../Nesi.Api
```

### Resetting the Database

**LocalDB (Windows):**
```bash
cd backend/src/Nesi.Infrastructure
dotnet ef database drop --startup-project ../Nesi.Api --force
dotnet ef database update --startup-project ../Nesi.Api
```

**SQL Server Express/Docker:**
```sql
DROP DATABASE NesiDb;
```
Then run migrations again:
```bash
cd backend/src/Nesi.Infrastructure
dotnet ef database update --startup-project ../Nesi.Api
```

### Running Only Backend

```bash
cd backend/src/Nesi.Api
dotnet run
```

### Running Only Frontend

```bash
cd frontend
npm start
```

## 🐛 Troubleshooting

### Backend won't start
- **SQL Server not running:**
  - Windows LocalDB: `sqllocaldb start mssqllocaldb`
  - Docker: `docker start nesi-sqlserver`
  - Windows Service: Check services.msc
- Verify connection string in `appsettings.json`
- Check if port 5000 is available
- See [Database Configuration Guide](docs/DATABASE_CONFIGURATION.md) for detailed troubleshooting

### Frontend won't start
- Delete `node_modules` and run `npm install` again
- Clear npm cache: `npm cache clean --force`
- Check if port 4200 is available

### Database connection fails
- **LocalDB:** Run `sqllocaldb info` to verify instance exists
- **SQL Express:** Verify SQL Server service is running
- **Docker:** Run `docker ps` to check container status
- Check connection string format
- See detailed troubleshooting in [Database Configuration Guide](docs/DATABASE_CONFIGURATION.md)

### CORS errors in browser
- Ensure backend is running
- Check CORS configuration in `Program.cs`
- Verify frontend URL in CORS policy

## 📊 API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh` - Refresh token
- `POST /api/auth/logout` - User logout

### Customers
- `GET /api/customer` - Get all customers (paginated, filtered)
- `GET /api/customer/{id}` - Get customer by ID
- `GET /api/customer/search?searchTerm=...` - Search customers
- `POST /api/customer` - Create new customer
- `PUT /api/customer/{id}` - Update customer
- `DELETE /api/customer/{id}` - Soft delete customer
- `POST /api/customer/{id}/activate` - Reactivate customer
- `POST /api/customer/{id}/addresses` - Add address to customer
- `POST /api/customer/{id}/contacts` - Add contact to customer
- `POST /api/customer/{id}/notes` - Add note to customer

### Quotes
- `GET /api/quote` - Get all quotes
- `GET /api/quote/{id}` - Get quote by ID
- `POST /api/quote` - Create new quote
- `POST /api/quote/{id}/submit` - Submit quote for approval
- `POST /api/quote/{id}/approve` - Manager approve quote
- `POST /api/quote/{id}/reject` - Manager reject quote
- `POST /api/quote/{id}/customer-approve` - Customer approve quote
- `POST /api/quote/{id}/convert-to-workorder` - Convert to work order

### Work Orders
- `GET /api/workorder` - Get all work orders
- `GET /api/workorder/{id}` - Get work order by ID

### Timesheets
- `GET /api/timesheets` - Get all timesheets for current user
- `GET /api/timesheets/{id}` - Get timesheet by ID
- `POST /api/timesheets` - Create new timesheet
- `PUT /api/timesheets/{id}` - Update timesheet
- `DELETE /api/timesheets/{id}` - Delete timesheet
- `POST /api/timesheets/{id}/submit` - Submit timesheet for approval

### Lookups
- `GET /api/lookups/pay-types` - Get all pay types
- `GET /api/lookups/job-types` - Get all job types
- `GET /api/lookups/work-orders` - Get active work orders

For complete API documentation, visit: `http://localhost:5000/swagger`

## 🎬 Demo Script

For a complete demonstration of all features:

- **Quick Start (5 min):** See [QUICK_START.md](QUICK_START.md) for rapid setup and demo
- **Full Demo (20 min):** See [DEMO_GUIDE.md](DEMO_GUIDE.md) for comprehensive walkthrough
- **Implementation Details:** See [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md) for technical details

### Quick Demo Flow

1. **Customer Management** (2 min)
   - Create new customer → View details → Edit → Deactivate

2. **Quote Creation** (3 min)
   - Create quote for customer → Add line items → Submit → Approve

3. **Work Order** (1 min)
   - Convert approved quote to work order → View work order

4. **Time Tracking** (2 min)
   - Enter time against work order → Submit → Manager review

## 📚 Additional Documentation

- [Architecture Documentation](/CoPilotGeneratedCode/docs/ARCHITECTURE.md)
- [User Guide](/CoPilotGeneratedCode/docs/USER_GUIDE.md)
- [API Reference](/CoPilotGeneratedCode/docs/API_REFERENCE.md)

## 🤝 Contributing

When contributing to this project:

1. Follow all coding guidelines in the `Guidelines/` folder
2. Write tests for new features (70%+ coverage required)
3. Update documentation as needed
4. Run linters and formatters before committing
5. Create descriptive commit messages

## 📝 License

This is a demo application for the NESI project.

## 🔗 Links

- [Main Repository](https://github.com/abhigaonkar/Nesi)
- [Documentation Hub](/CopilotDocumentation/)
- [Guidelines](/CoPilotGeneratedCode/Guidelines/)

---

**Built with ❤️ using modern technologies and best practices**

**Last Updated:** April 2026  
**Version:** 1.0.0  
**Status:** Demo Ready 🚀
