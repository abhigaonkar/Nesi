# Quick Start Guide - Nesi Timesheet Application

## 🚀 Get Started in 5 Minutes

This guide will help you get the Nesi Timesheet Application running locally for development and testing.

## Prerequisites

Ensure you have these installed:
- ✅ **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- ✅ **Node.js 18+** - [Download](https://nodejs.org/)
- ✅ **SQL Server** - LocalDB, Express, or Full Edition
  - Windows: Included with Visual Studio
  - Linux/Mac: SQL Server Express or Docker

## Step 1: Clone Repository

```bash
git clone https://github.com/abhigaonkar/Nesi.git
cd Nesi/CoPilotGeneratedCode
```

## Step 2: Setup Database

### Option A: Using Default LocalDB (Windows)

```bash
cd backend/src/Nesi.Infrastructure
dotnet ef database update --startup-project ../Nesi.Api
```

### Option B: Using Custom SQL Server

1. Create a database named `NesiDb`
2. Update connection string in `backend/src/Nesi.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=NesiDb;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

3. Run migrations:
```bash
cd backend/src/Nesi.Infrastructure
dotnet ef database update --startup-project ../Nesi.Api
```

## Step 3: Start Backend API

```bash
cd backend/src/Nesi.Api
dotnet run
```

✅ **Backend running at:** `http://localhost:5000`  
✅ **Swagger UI at:** `http://localhost:5000/swagger`

**Keep this terminal open!**

## Step 4: Start Frontend (New Terminal)

```bash
cd frontend
npm install
npm start
```

✅ **Frontend running at:** `http://localhost:4200`

## Step 5: Login & Test

1. Open browser: `http://localhost:4200`
2. Login with default credentials:
   - **Username:** `admin`
   - **Password:** `Admin@123`
3. Explore the application!

## 🎯 What You Can Do

### As Admin User:
- ✅ View dashboard with statistics
- ✅ Create timesheets
- ✅ Edit draft timesheets
- ✅ Submit timesheets for approval
- ✅ **Approve/Reject** timesheets
- ✅ View all users' timesheets

### Key Features to Test:
1. **Dashboard** - See your timesheet statistics
2. **Create Timesheet** - Add new time entries
3. **Edit Timesheet** - Modify draft entries
4. **Submit for Approval** - Send to manager
5. **Approve/Reject** - Manager workflow

## 🛠️ Using Helper Scripts

### Windows:
```cmd
cd scripts
run-all.bat
```

### Linux/Mac:
```bash
cd scripts
chmod +x run-all.sh
./run-all.sh
```

## 📁 Project Structure

```
CoPilotGeneratedCode/
├── backend/            # .NET 9 API
│   └── src/
│       ├── Nesi.Api/           # Controllers, Startup
│       ├── Nesi.Application/   # CQRS Handlers
│       ├── Nesi.Domain/        # Entities, Interfaces
│       └── Nesi.Infrastructure/# Database, Repositories
├── frontend/           # Angular 19 App
│   └── src/app/
│       ├── components/         # UI Components
│       ├── services/           # Business Logic
│       └── models/             # TypeScript Interfaces
├── docs/               # Documentation
└── scripts/            # Helper Scripts
```

## 🐛 Troubleshooting

### Backend won't start

**Issue:** Port 5000 already in use  
**Solution:** 
```bash
# Find process using port 5000
netstat -ano | findstr :5000  # Windows
lsof -i :5000                 # Linux/Mac

# Kill the process or change port in launchSettings.json
```

**Issue:** Database connection error  
**Solution:**
- Verify SQL Server is running
- Check connection string
- Run migrations: `dotnet ef database update`

### Frontend won't start

**Issue:** Port 4200 already in use  
**Solution:**
```bash
# Kill process on port 4200
netstat -ano | findstr :4200  # Windows
lsof -i :4200                 # Linux/Mac
```

**Issue:** npm install fails  
**Solution:**
```bash
# Clear cache and reinstall
npm cache clean --force
rm -rf node_modules package-lock.json
npm install
```

### CORS Errors

**Issue:** API calls blocked by CORS  
**Solution:**
- Ensure backend is running on `http://localhost:5000`
- Ensure frontend is running on `http://localhost:4200`
- Check CORS configuration in `Program.cs` (already configured)

### Login Fails

**Issue:** Invalid credentials  
**Solution:**
- Ensure migrations ran successfully
- Check if seed data was loaded
- Try default credentials: `admin` / `Admin@123`

**Issue:** Cannot connect to API  
**Solution:**
- Verify backend is running (`http://localhost:5000/swagger`)
- Check environment.ts API URL: `http://localhost:5000`

## 📚 Next Steps

1. **Explore the Application:**
   - Create a few timesheets
   - Submit them for approval
   - Approve/reject as admin

2. **Read the Documentation:**
   - [Integration Testing Guide](./INTEGRATION_TESTING_GUIDE.md)
   - [Architecture Documentation](./ARCHITECTURE.md)
   - [API Reference](../backend/README.md)

3. **Run Tests:**
   ```bash
   # Backend tests (when available)
   cd backend
   dotnet test
   
   # Frontend tests
   cd frontend
   npm test
   ```

4. **Review the Code:**
   - Backend: Clean Architecture with CQRS
   - Frontend: Angular 19 with Signals
   - Database: EF Core with migrations

## 🔑 Default Test Users

| Username | Password | Role | Description |
|----------|----------|------|-------------|
| admin | Admin@123 | Admin | Full system access |
| manager | Manager@123 | Manager | Can approve timesheets |
| employee | Employee@123 | Employee | Can create timesheets |

*Note: Additional users can be created via registration*

## 📊 API Endpoints

View all endpoints in Swagger: `http://localhost:5000/swagger`

**Key Endpoints:**
- `POST /api/auth/login` - User login
- `GET /api/timesheet` - List timesheets
- `POST /api/timesheet` - Create timesheet
- `POST /api/timesheet/{id}/submit` - Submit for approval
- `POST /api/timesheet/{id}/approve` - Approve timesheet

## 🎨 UI Features

- ✅ **Modern Design** - Gradient colors, responsive layout
- ✅ **Role-Based UI** - Different views for Admin/Manager/Employee
- ✅ **Real-time Validation** - Form validation as you type
- ✅ **Loading States** - Visual feedback during operations
- ✅ **Error Handling** - Clear error messages
- ✅ **Modal Forms** - Clean create/edit experience
- ✅ **Pagination** - Handle large datasets

## 🔒 Security Features

- ✅ Header-based authentication (ready for JWT upgrade)
- ✅ Role-based access control
- ✅ Route guards (protected routes)
- ✅ Input validation (frontend & backend)
- ✅ SQL injection protection (EF Core)
- ✅ XSS protection (Angular sanitization)

## 💡 Development Tips

### Hot Reload

Both backend and frontend support hot reload:
- **Backend:** Code changes trigger automatic rebuild
- **Frontend:** Live reload in browser on file save

### Debugging

**Backend:**
- Use Visual Studio or VS Code with C# extension
- Set breakpoints in controllers/handlers
- Use Swagger to test API endpoints

**Frontend:**
- Use browser DevTools (F12)
- Angular DevTools extension
- Check Network tab for API calls
- Check Console for errors

### Database Management

```bash
# Add new migration
cd backend/src/Nesi.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../Nesi.Api

# Update database
dotnet ef database update --startup-project ../Nesi.Api

# Rollback migration
dotnet ef database update PreviousMigration --startup-project ../Nesi.Api
```

## 🚀 Production Build

### Backend
```bash
cd backend/src/Nesi.Api
dotnet publish -c Release -o ./publish
```

### Frontend
```bash
cd frontend
npm run build
# Output in dist/frontend
```

## 📞 Support

- **Documentation:** Check `docs/` folder
- **Issues:** [GitHub Issues](https://github.com/abhigaonkar/Nesi/issues)
- **Architecture:** See `docs/ARCHITECTURE.md`

## ✅ Success Criteria

You're ready to develop when:
- ✅ Backend runs without errors
- ✅ Frontend loads in browser
- ✅ Can login with admin credentials
- ✅ Dashboard shows statistics
- ✅ Can create and view timesheets
- ✅ Swagger UI accessible

**Happy Coding! 🎉**

---

**Version:** 1.0.0  
**Last Updated:** April 14, 2026  
**Tech Stack:** .NET 9 + Angular 19 + SQL Server
