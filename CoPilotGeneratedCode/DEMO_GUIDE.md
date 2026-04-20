# Nesi Application - Demo Guide

## Overview
This guide provides instructions for demonstrating the Nesi application's complete use case flows, including Customer Management, Quote Management, and Work Order Management.

## Prerequisites
1. Backend API running on `http://localhost:5000` (or configured API URL)
2. Frontend Angular application running on `http://localhost:4200`
3. Database seeded with sample data

## Starting the Application

### Backend
```bash
cd CoPilotGeneratedCode/backend
dotnet run --project src/Nesi.Api/Nesi.Api.csproj
```

### Frontend
```bash
cd CoPilotGeneratedCode/frontend
npm install
npm start
```

The application will be available at `http://localhost:4200`

## Demo Flow

### 1. Customer Management Flow

#### 1.1 View Customer List
1. Log in to the application
2. Navigate to **Customers** from the sidebar menu (👥 icon)
3. Observe the customer list with:
   - Customer number
   - Name and business unit
   - Contact information
   - Email and phone
   - Active/Inactive status
   - Action buttons

#### 1.2 Search and Filter Customers
1. Use the search box to search by:
   - Customer name
   - Customer number
   - Email
   - Phone number
2. Toggle "Active Only" checkbox to filter active/inactive customers
3. Use pagination controls to navigate through pages

#### 1.3 Create New Customer
1. Click "**+ New Customer**" button
2. Fill in the form:
   - **Required**: Customer Name
   - **Optional**: 
     - Contact Name
     - Email
     - Phone
     - Address
     - Credit Limit
     - Payment Terms (days)
     - Business Unit ID
     - Account Manager ID
     - Notes
3. Click "**Create Customer**"
4. System will redirect to customer detail page
5. Observe auto-generated customer number (e.g., `CUST-2026-00001`)

#### 1.4 View Customer Details
1. From customer list, click the view icon (👁) on any customer
2. Observe detailed information:
   - Basic Information section
   - Financial Information
   - Addresses (if any)
   - Contacts (if any)
   - Activity Notes (if any)
   - System Information (created/updated dates)

#### 1.5 Edit Customer
1. From customer detail page, click "**✏️ Edit**" button
2. Modify customer information
3. Click "**Update Customer**"
4. Verify changes in detail view

#### 1.6 Deactivate/Activate Customer
1. From customer list or detail page:
   - Click "**🗑 Deactivate**" for active customers
   - Click "**✓ Activate**" for inactive customers
2. Confirm the action
3. Observe status change

### 2. Quote Management Flow

#### 2.1 View Quote List
1. Navigate to **Quotes** from the sidebar menu (💼 icon)
2. Browse existing quotes with:
   - Quote number
   - Customer name
   - Quote type (Time & Material, Fixed Price, Cost Plus)
   - Status (Draft, Submitted, Approved, Rejected, Customer Approved)
   - Amount
   - Dates

#### 2.2 Create New Quote
1. Click "**+ New Quote**" button
2. Fill in quote details:
   - Select customer
   - Quote type
   - Description and scope
   - Estimated start and completion dates
   - Project manager
   - Terms and conditions
   - Tax rate
3. Add line items:
   - Item type (Labor, Material, Equipment, Miscellaneous)
   - Description
   - Quantity
   - Unit price
4. Click "**Create Quote**"
5. Review auto-generated quote number

#### 2.3 Quote Approval Workflow
1. **Submit for Approval**:
   - Open quote in draft status
   - Click "**Submit**" button
   - Quote status changes to "Submitted"

2. **Manager Approval** (requires manager role):
   - Navigate to submitted quote
   - Click "**Approve**" button
   - Quote status changes to "Approved"
   
   OR
   
   - Click "**Reject**" button
   - Provide rejection reason
   - Quote status changes to "Rejected"

3. **Customer Approval**:
   - Open approved quote
   - Click "**Customer Approve**" button
   - Quote status changes to "Customer Approved"

#### 2.4 Convert Quote to Work Order
1. Open a customer-approved quote
2. Click "**Convert to Work Order**" button
3. System creates new work order
4. Navigate to work orders to view the created work order

### 3. Work Order Management Flow

#### 3.1 View Work Orders
1. Navigate to **Work Orders** from sidebar menu (🔧 icon)
2. View list of work orders showing:
   - Work order number
   - Customer name
   - Status
   - Scheduled dates
   - Assigned crew

#### 3.2 View Work Order Details
1. Click on any work order
2. Review:
   - Work order information
   - Related quote details
   - Line items
   - Schedule information
   - Status history

### 4. Timesheet Management Flow

#### 4.1 Employee Timesheet Entry
1. Navigate to **Timesheets** (⏱️ icon)
2. View current week timesheet
3. Add time entries:
   - Select work order
   - Enter hours for each day
   - Add notes if needed
4. Submit timesheet for approval

#### 4.2 Manager Timesheet Review
1. Navigate to **Review Timesheets** (✅ icon) - Manager role required
2. View pending timesheets
3. Review time entries
4. Approve or reject timesheets

## Complete End-to-End Demo Scenario

### Scenario: "New Customer Project Workflow"

1. **Create Customer** (5 minutes)
   - Create new customer "Acme Corporation"
   - Add contact information and billing details
   - Set credit limit: $50,000
   - Payment terms: 30 days

2. **Create Quote** (7 minutes)
   - Create quote for Acme Corporation
   - Quote type: Time & Material
   - Add labor line items (Project Manager, Electricians)
   - Add material line items (Electrical supplies)
   - Set tax rate: 8.5%
   - Total estimate: $15,000

3. **Quote Approval Process** (3 minutes)
   - Submit quote for internal approval
   - Manager approves quote
   - Customer approves quote

4. **Convert to Work Order** (2 minutes)
   - Convert approved quote to work order
   - Assign crew members
   - Schedule work dates

5. **Track Time** (3 minutes)
   - Employees enter time against work order
   - Submit timesheets
   - Manager reviews and approves time

## Key Features to Highlight

### Customer Management
- ✅ Full CRUD operations
- ✅ Advanced search and filtering
- ✅ Pagination support
- ✅ Soft delete (activate/deactivate)
- ✅ Contact and address management
- ✅ Activity notes tracking

### Quote Management
- ✅ Multiple quote types
- ✅ Line item management
- ✅ Approval workflow
- ✅ Customer approval tracking
- ✅ Automatic calculations (subtotal, tax, total)
- ✅ Quote versioning support

### Work Order Management
- ✅ Quote to work order conversion
- ✅ Crew assignment
- ✅ Schedule management
- ✅ Status tracking

### System Features
- ✅ Role-based access control
- ✅ Responsive design
- ✅ Real-time updates
- ✅ Clean, modern UI
- ✅ RESTful API architecture
- ✅ Clean Architecture pattern (Backend)

## API Endpoints Reference

### Customers
- `GET /api/customer` - List customers
- `GET /api/customer/{id}` - Get customer by ID
- `GET /api/customer/search?searchTerm={term}` - Search customers
- `POST /api/customer` - Create customer
- `PUT /api/customer/{id}` - Update customer
- `DELETE /api/customer/{id}` - Deactivate customer
- `POST /api/customer/{id}/activate` - Activate customer
- `POST /api/customer/{id}/addresses` - Add address
- `POST /api/customer/{id}/contacts` - Add contact
- `POST /api/customer/{id}/notes` - Add note

### Quotes
- `GET /api/quote` - List quotes
- `GET /api/quote/{id}` - Get quote by ID
- `POST /api/quote` - Create quote
- `POST /api/quote/{id}/submit` - Submit for approval
- `POST /api/quote/{id}/approve` - Approve quote
- `POST /api/quote/{id}/reject` - Reject quote
- `POST /api/quote/{id}/customer-approve` - Customer approval
- `POST /api/quote/{id}/convert-to-workorder` - Convert to work order

### Work Orders
- `GET /api/workorder` - List work orders
- `GET /api/workorder/{id}` - Get work order by ID

## Troubleshooting

### Issue: Cannot log in
- **Solution**: Check that backend API is running and database is seeded with users

### Issue: Customers not loading
- **Solution**: Verify API URL in `environment.ts` file matches backend URL

### Issue: Navigation not working
- **Solution**: Clear browser cache and restart Angular dev server

## Technical Stack

### Backend
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- MediatR (CQRS pattern)
- AutoMapper
- FluentValidation
- SQL Server

### Frontend
- Angular 19
- TypeScript
- SCSS
- RxJS
- Standalone Components

## Architecture Highlights

- **Clean Architecture**: Separation of concerns with Domain, Application, Infrastructure, and API layers
- **CQRS Pattern**: Commands and Queries separated using MediatR
- **Repository Pattern**: Data access abstraction
- **Unit of Work**: Transaction management
- **Dependency Injection**: Loose coupling throughout
- **RESTful API**: Standard HTTP methods and status codes

## Next Steps

After the demo, consider:
1. Adding more sample data
2. Implementing reports and dashboards
3. Adding file upload for documents
4. Email notifications for approvals
5. Mobile responsiveness enhancements
6. Export to PDF functionality
