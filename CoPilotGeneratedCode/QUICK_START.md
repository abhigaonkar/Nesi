# Quick Start Guide - Nesi Application Demo

## 🚀 Quick Setup (5 Minutes)

### Step 1: Start the Backend API
```bash
cd CoPilotGeneratedCode/backend
dotnet run --project src/Nesi.Api/Nesi.Api.csproj
```
- Backend will start on `https://localhost:5001` or `http://localhost:5000`
- Wait for message: "Now listening on: http://localhost:5000"

### Step 2: Start the Frontend
Open a **new terminal**:
```bash
cd CoPilotGeneratedCode/frontend
npm install  # Only needed first time
npm start
```
- Frontend will start on `http://localhost:4200`
- Browser will open automatically

### Step 3: Login
- **URL**: http://localhost:4200
- **Demo Credentials** (check backend seed data):
  - Username: `admin@nesi.com`
  - Password: `Admin123!`

## 📋 5-Minute Demo Script

### Demo 1: Customer Management (2 mins)

1. **Navigate to Customers**
   - Click "👥 Customers" in sidebar
   
2. **Create New Customer**
   - Click "+ New Customer"
   - Enter:
     - Name: "Demo Company Inc."
     - Contact Name: "John Smith"
     - Email: "john@democompany.com"
     - Phone: "(555) 123-4567"
     - Credit Limit: 25000
     - Payment Terms: 30
   - Click "Create Customer"

3. **View Customer Details**
   - Observe auto-generated customer number
   - View all customer information

### Demo 2: Quote Creation (2 mins)

1. **Navigate to Quotes**
   - Click "💼 Quotes" in sidebar

2. **Create Quote**
   - Click "+ New Quote"
   - Select the customer you just created
   - Select Quote Type: "Time & Material"
   - Add description and scope
   - Add line items (labor and materials)
   - Click "Create Quote"

3. **Submit & Approve**
   - Click "Submit" to submit for approval
   - Click "Approve" (if manager role)
   - Click "Customer Approve"

### Demo 3: Work Order Creation (1 min)

1. **Convert Quote**
   - From approved quote, click "Convert to Work Order"
   - View newly created work order
   
2. **Navigate to Work Orders**
   - Click "🔧 Work Orders" in sidebar
   - Find the converted work order

## 🎯 Key Features to Show

### ✅ Customer Management
- Create, Read, Update, Delete customers
- Search and filter
- Pagination
- Soft delete (activate/deactivate)

### ✅ Quote Management  
- Create quotes with line items
- Multi-step approval workflow
- Auto-calculate totals
- Convert to work orders

### ✅ Work Order Management
- View work orders
- Track status
- Link to original quote

### ✅ Timesheet Management
- Enter time entries
- Submit for approval
- Manager review (role-based)

## 🔍 API Testing with Swagger

Visit: `http://localhost:5000/swagger`

Test endpoints directly:
- GET /api/customer
- POST /api/customer
- GET /api/quote
- POST /api/quote

## 🎨 UI Highlights

### Modern Design
- Clean, professional interface
- Responsive layout
- Intuitive navigation
- Status badges and icons

### User Experience
- Loading indicators
- Error messages
- Confirmation dialogs
- Form validation

### Navigation Flow
- Sidebar navigation
- Breadcrumbs
- Back buttons
- Logical flow between screens

## 📊 Data Flow Example

```
Customer → Quote → Work Order → Timesheet
   ↓         ↓         ↓            ↓
Create    Submit    Convert     Track Time
  ↓         ↓         ↓            ↓
View      Approve   Assign      Approve
  ↓         ↓         ↓            ↓
Edit      Customer  Schedule    Report
          Approve
```

## 🛠 Architecture Overview

### Backend (Clean Architecture)
```
Nesi.Api (Presentation)
    ↓
Nesi.Application (CQRS - Commands/Queries)
    ↓
Nesi.Domain (Entities, Interfaces)
    ↓
Nesi.Infrastructure (Data Access, Repositories)
```

### Frontend (Angular Components)
```
App Component
    ↓
Nav Component (Shared)
    ↓
Feature Components (Customer, Quote, WorkOrder)
    ↓
Services (API Calls)
    ↓
Models (TypeScript Interfaces)
```

## 🎯 Demo Talking Points

### Technical Excellence
1. **Clean Architecture**: Clear separation of concerns
2. **CQRS Pattern**: Commands and Queries separated
3. **Repository Pattern**: Data access abstraction
4. **Dependency Injection**: Loose coupling
5. **RESTful API**: Standard HTTP practices

### Business Value
1. **Complete Workflow**: Customer → Quote → Work Order
2. **Approval Process**: Multi-level authorization
3. **Time Tracking**: Integrated timesheet management
4. **Reporting Ready**: Structured data for analytics
5. **Scalable**: Built for growth

### User Experience
1. **Intuitive UI**: Easy to learn and use
2. **Responsive**: Works on all devices
3. **Fast**: Optimized performance
4. **Reliable**: Error handling and validation
5. **Modern**: Current technology stack

## 📝 Common Demo Questions & Answers

**Q: Can customers be deleted?**
A: Yes, using soft delete. Customers are deactivated, not removed from database.

**Q: How does the approval workflow work?**
A: Quote goes through: Draft → Submitted → Approved → Customer Approved

**Q: Can quotes be edited after submission?**
A: Currently no, but revision/versioning can be added.

**Q: How are quote numbers generated?**
A: Auto-generated: QUOTE-{year}-{sequence}

**Q: Is role-based access control implemented?**
A: Yes, admins, managers, and employees have different permissions.

## 🚨 Troubleshooting

### Backend Won't Start
- Check .NET 9 SDK is installed: `dotnet --version`
- Check port 5000/5001 is available
- Review connection string in appsettings.json

### Frontend Won't Start  
- Check Node.js is installed: `node --version`
- Run `npm install` in frontend directory
- Check port 4200 is available

### API Calls Failing
- Verify backend is running
- Check proxy.conf.json points to correct backend URL
- Check browser console for CORS errors

### No Data Showing
- Verify database is seeded
- Check API responses in browser Network tab
- Review backend logs

## 📚 Additional Resources

- **Full Demo Guide**: See `DEMO_GUIDE.md` for detailed walkthrough
- **API Documentation**: Access Swagger at `http://localhost:5000/swagger`
- **Source Code**: Review code in `CoPilotGeneratedCode/` directory

## ✨ Next Steps After Demo

1. **Add More Data**: Create additional customers and quotes
2. **Test Workflows**: Go through complete approval process
3. **Explore API**: Use Swagger to test endpoints
4. **Review Code**: Examine backend and frontend architecture
5. **Plan Enhancements**: Identify features to add

---

**Ready to Demo!** 🎉

Follow this guide for a smooth, professional demonstration of the Nesi application.
