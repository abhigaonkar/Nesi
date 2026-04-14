# Integration Testing Guide

## Overview
This guide provides comprehensive instructions for testing the Nesi Timesheet Application end-to-end integration between the .NET 9 backend API and Angular 19 frontend.

## Prerequisites

### Required Software
- ✅ .NET 9 SDK installed
- ✅ Node.js 18+ and npm installed
- ✅ SQL Server (LocalDB or Express) running
- ✅ Modern web browser (Chrome, Edge, or Firefox)

### Verify Installations
```bash
dotnet --version  # Should be 9.0.x
node --version    # Should be 18.x or higher
npm --version     # Should be 9.x or higher
```

## Test Environment Setup

### 1. Database Setup

```bash
cd /home/runner/work/Nesi/Nesi/CoPilotGeneratedCode/backend/src/Nesi.Infrastructure

# Run migrations to create/update database
dotnet ef database update --startup-project ../Nesi.Api

# Verify migrations were successful
dotnet ef migrations list --startup-project ../Nesi.Api
```

**Expected Output:**
- Database created successfully
- All migrations applied
- Seed data loaded (default admin user)

### 2. Backend API Setup

```bash
cd /home/runner/work/Nesi/Nesi/CoPilotGeneratedCode/backend/src/Nesi.Api

# Build the API
dotnet build

# Run the API
dotnet run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shutdown.
```

**Verify API is running:**
- Open browser: `http://localhost:5000/swagger`
- You should see Swagger UI with all API endpoints

### 3. Frontend Setup

**Open a new terminal:**

```bash
cd /home/runner/work/Nesi/Nesi/CoPilotGeneratedCode/frontend

# Install dependencies (if not already done)
npm install

# Run the frontend
npm start
```

**Expected Output:**
```
✔ Browser application bundle generation complete.
** Angular Live Development Server is listening on localhost:4200
```

**Verify Frontend is running:**
- Open browser: `http://localhost:4200`
- You should see the login page

## Integration Test Suite

### Test 1: Authentication Flow ✅

#### 1.1 Login - Happy Path

**Test Steps:**
1. Navigate to `http://localhost:4200`
2. Verify login form is displayed
3. Enter credentials:
   - Username: `admin`
   - Password: `Admin@123`
4. Click "Login" button

**Expected Results:**
- ✅ Loading indicator appears briefly
- ✅ Redirect to dashboard (`/dashboard`)
- ✅ User information displayed in header
- ✅ No error messages

**API Calls to Monitor (Browser DevTools → Network):**
```
POST http://localhost:5000/api/auth/login
Request Body: { "username": "admin", "password": "Admin@123" }
Response: { "success": true, "data": { "id": 1, "username": "admin", "role": "Admin", ... } }
```

#### 1.2 Login - Invalid Credentials

**Test Steps:**
1. Navigate to `http://localhost:4200/login`
2. Enter invalid credentials:
   - Username: `admin`
   - Password: `wrongpassword`
3. Click "Login" button

**Expected Results:**
- ✅ Error message displayed: "Invalid username or password"
- ✅ Stay on login page
- ✅ No redirect

#### 1.3 Login - Validation

**Test Steps:**
1. Leave username field empty
2. Click "Login" button

**Expected Results:**
- ✅ Validation error shown
- ✅ Login button disabled or error message displayed

#### 1.4 Logout

**Test Steps:**
1. Login successfully
2. Click "Logout" button in header

**Expected Results:**
- ✅ Redirect to login page
- ✅ Session cleared
- ✅ Attempting to navigate to `/dashboard` redirects to `/login`

### Test 2: Dashboard Display ✅

#### 2.1 Dashboard Statistics

**Prerequisites:** Login as admin user

**Test Steps:**
1. Navigate to dashboard (should happen automatically after login)

**Expected Results:**
- ✅ Dashboard page loads
- ✅ Four statistics cards displayed:
  - Draft Timesheets count
  - Submitted Timesheets count
  - Approved Timesheets count
  - Total Hours
- ✅ Recent Timesheets table visible
- ✅ "View All Timesheets" button visible

**API Calls to Monitor:**
```
GET http://localhost:5000/api/timesheet?userId=1&pageNumber=1&pageSize=5
Response: { "success": true, "data": { "items": [...], "totalCount": X, ... } }
```

#### 2.2 Dashboard Navigation

**Test Steps:**
1. Click "View All Timesheets" button

**Expected Results:**
- ✅ Navigate to `/timesheets`
- ✅ Full timesheet list displayed

### Test 3: Timesheet CRUD Operations ✅

#### 3.1 View Timesheets List

**Prerequisites:** Login as any user

**Test Steps:**
1. Navigate to `/timesheets`

**Expected Results:**
- ✅ Timesheet table displayed with columns:
  - Date
  - Work Order
  - Hours
  - Pay Type
  - Status
  - Notes
  - Actions
- ✅ "Create Timesheet" button visible
- ✅ Total count displayed
- ✅ Pagination controls (if > 10 timesheets)

**API Calls to Monitor:**
```
GET http://localhost:5000/api/timesheet?userId=1&pageNumber=1&pageSize=10
GET http://localhost:5000/api/workorder (for work order dropdown)
```

#### 3.2 Create Timesheet - Happy Path

**Test Steps:**
1. Click "+ Create Timesheet" button
2. Modal form opens
3. Fill in the form:
   - Date: Today's date
   - Hours: 8
   - Pay Type: Regular
   - Work Order: Select any (or None)
   - Notes: "Test timesheet entry"
4. Click "Create" button

**Expected Results:**
- ✅ Success message: "Timesheet created successfully"
- ✅ Modal closes
- ✅ Timesheet list refreshes
- ✅ New timesheet appears in the list with "Draft" status

**API Calls to Monitor:**
```
POST http://localhost:5000/api/timesheet
Request Body: {
  "date": "2026-04-14T00:00:00.000Z",
  "hours": 8,
  "payTypeId": 1,
  "workOrderId": null,
  "notes": "Test timesheet entry"
}
Response: { "success": true, "data": <new_timesheet_id> }
```

#### 3.3 Create Timesheet - Validation

**Test Steps:**
1. Click "+ Create Timesheet"
2. Try to submit with:
   - Empty date
   - Hours = 0 or > 24
   - Empty pay type

**Expected Results:**
- ✅ Form validation prevents submission
- ✅ "Create" button disabled when form invalid
- ✅ Validation messages shown

#### 3.4 Edit Timesheet - Draft Only

**Test Steps:**
1. Find a timesheet with "Draft" status
2. Click edit (✏️) button
3. Modal opens with pre-filled data
4. Change hours to 4
5. Click "Update" button

**Expected Results:**
- ✅ Success message: "Timesheet updated successfully"
- ✅ Modal closes
- ✅ Timesheet list refreshes
- ✅ Hours updated to 4

**API Calls to Monitor:**
```
PUT http://localhost:5000/api/timesheet/{id}
Request Body: { "id": X, "date": ..., "hours": 4, ... }
Response: { "success": true }
```

#### 3.5 Edit Timesheet - Submitted/Approved (Should Fail)

**Test Steps:**
1. Find a timesheet with "Submitted" or "Approved" status
2. Try to click edit button

**Expected Results:**
- ✅ Edit button NOT visible for submitted/approved timesheets
- ✅ OR Error message if attempting to edit

#### 3.6 Submit Timesheet for Approval

**Test Steps:**
1. Find a timesheet with "Draft" status
2. Click submit (📤) button
3. Confirm in the dialog

**Expected Results:**
- ✅ Confirmation dialog appears
- ✅ Success message: "Timesheet submitted successfully"
- ✅ Timesheet status changes to "Submitted"
- ✅ Edit button disappears
- ✅ Submit button disappears

**API Calls to Monitor:**
```
POST http://localhost:5000/api/timesheet/{id}/submit
Response: { "success": true }
```

### Test 4: Workflow & Approvals ✅

#### 4.1 Approve Timesheet (Manager/Admin Only)

**Prerequisites:** Login as Manager or Admin

**Test Steps:**
1. Navigate to `/timesheets`
2. Find a timesheet with "Submitted" status
3. Click approve (✅) button
4. Confirm in the dialog

**Expected Results:**
- ✅ Approve button visible (only for Manager/Admin)
- ✅ Confirmation dialog appears
- ✅ Success message: "Timesheet approved successfully"
- ✅ Timesheet status changes to "Approved"
- ✅ Approve/Reject buttons disappear

**API Calls to Monitor:**
```
POST http://localhost:5000/api/timesheet/{id}/approve
Response: { "success": true }
```

#### 4.2 Reject Timesheet (Manager/Admin Only)

**Test Steps:**
1. Find a timesheet with "Submitted" status
2. Click reject (❌) button
3. Confirm in the dialog

**Expected Results:**
- ✅ Confirmation dialog appears
- ✅ Success message: "Timesheet rejected successfully"
- ✅ Timesheet status changes to "Rejected"

**API Calls to Monitor:**
```
POST http://localhost:5000/api/timesheet/{id}/reject
Response: { "success": true }
```

#### 4.3 Role-Based Button Visibility

**Test as Employee:**
- ✅ Can create timesheets
- ✅ Can edit DRAFT timesheets
- ✅ Can submit timesheets
- ✅ CANNOT see approve buttons
- ✅ CANNOT see reject buttons

**Test as Manager:**
- ✅ All Employee permissions +
- ✅ CAN see approve buttons (for submitted timesheets)
- ✅ CAN see reject buttons (for submitted timesheets)

**Test as Admin:**
- ✅ All Manager permissions +
- ✅ Full system access

### Test 5: Pagination & Filtering ✅

#### 5.1 Pagination

**Prerequisites:** Create > 10 timesheets (or ensure test data exists)

**Test Steps:**
1. Navigate to `/timesheets`
2. Verify pagination controls displayed
3. Click "Next" button
4. Click "Previous" button

**Expected Results:**
- ✅ Shows "Page X of Y"
- ✅ Next button disabled on last page
- ✅ Previous button disabled on first page
- ✅ Clicking Next loads next page of results
- ✅ Clicking Previous loads previous page

**API Calls to Monitor:**
```
GET http://localhost:5000/api/timesheet?userId=1&pageNumber=2&pageSize=10
```

### Test 6: Error Handling ✅

#### 6.1 API Unavailable

**Test Steps:**
1. Stop the backend API (Ctrl+C)
2. Try to login from frontend

**Expected Results:**
- ✅ Error message displayed
- ✅ User informed of connection issue
- ✅ No application crash

#### 6.2 Network Errors

**Test Steps:**
1. Open Browser DevTools → Network Tab
2. Throttle network to "Slow 3G"
3. Perform operations

**Expected Results:**
- ✅ Loading indicators show while waiting
- ✅ Operations complete (may take longer)
- ✅ Appropriate timeouts
- ✅ Error messages for timeouts

### Test 7: Browser Compatibility ✅

**Test in Multiple Browsers:**
- Chrome/Edge (Chromium)
- Firefox
- Safari (if available)

**Verify:**
- ✅ UI renders correctly
- ✅ All functionality works
- ✅ No console errors
- ✅ Responsive design works

### Test 8: Security ✅

#### 8.1 Route Protection

**Test Steps:**
1. Logout
2. Try to navigate directly to: `http://localhost:4200/dashboard`

**Expected Results:**
- ✅ Redirect to `/login`
- ✅ Return URL preserved: `/login?returnUrl=%2Fdashboard`
- ✅ After login, redirect to `/dashboard`

#### 8.2 Authorization

**Test as Employee:**
- ✅ Cannot approve/reject timesheets
- ✅ Approve/Reject buttons not visible

**Test as Manager:**
- ✅ Can approve/reject timesheets
- ✅ Buttons visible and functional

## Performance Testing

### Load Times
- ✅ Login page loads < 2 seconds
- ✅ Dashboard loads < 3 seconds
- ✅ Timesheet list loads < 3 seconds
- ✅ Create timesheet modal opens < 1 second

### Bundle Size
- ✅ Initial bundle < 500KB (currently ~368KB)
- ✅ No memory leaks (check DevTools → Memory)

## Automated Testing (Future)

### Backend Unit Tests
```bash
cd CoPilotGeneratedCode/backend
dotnet test
```

### Frontend Unit Tests
```bash
cd CoPilotGeneratedCode/frontend
npm test
```

### E2E Tests (Playwright/Cypress)
```bash
cd CoPilotGeneratedCode/frontend
npm run test:e2e
```

## Test Results Template

| Test Case | Status | Notes |
|-----------|--------|-------|
| 1.1 Login - Happy Path | ⬜ | |
| 1.2 Login - Invalid Credentials | ⬜ | |
| 1.3 Login - Validation | ⬜ | |
| 1.4 Logout | ⬜ | |
| 2.1 Dashboard Statistics | ⬜ | |
| 2.2 Dashboard Navigation | ⬜ | |
| 3.1 View Timesheets List | ⬜ | |
| 3.2 Create Timesheet - Happy Path | ⬜ | |
| 3.3 Create Timesheet - Validation | ⬜ | |
| 3.4 Edit Timesheet - Draft | ⬜ | |
| 3.5 Edit Timesheet - Submitted | ⬜ | |
| 3.6 Submit Timesheet | ⬜ | |
| 4.1 Approve Timesheet | ⬜ | |
| 4.2 Reject Timesheet | ⬜ | |
| 4.3 Role-Based Visibility | ⬜ | |
| 5.1 Pagination | ⬜ | |
| 6.1 API Unavailable | ⬜ | |
| 6.2 Network Errors | ⬜ | |
| 7 Browser Compatibility | ⬜ | |
| 8.1 Route Protection | ⬜ | |
| 8.2 Authorization | ⬜ | |

**Legend:**
- ⬜ Not Tested
- ✅ Passed
- ❌ Failed
- ⚠️ Partial/Needs Review

## Troubleshooting

### Backend Issues

**API won't start:**
- Check if SQL Server is running
- Verify connection string in `appsettings.json`
- Check if port 5000 is available
- Run `dotnet ef database update`

**Database connection errors:**
- Verify SQL Server is running
- Check connection string
- Ensure database exists
- Check firewall settings

### Frontend Issues

**Frontend won't start:**
- Delete `node_modules` and run `npm install`
- Clear npm cache: `npm cache clean --force`
- Check if port 4200 is available

**CORS errors:**
- Ensure backend is running
- Verify CORS configuration in `Program.cs`
- Check frontend URL matches CORS policy

**API calls fail:**
- Check API URL in `environment.ts`
- Verify backend is running on correct port
- Check browser console for errors

## Next Steps

After completing integration testing:
1. Document any bugs found
2. Fix critical issues
3. Run code quality checks (parallel_validation)
4. Update documentation
5. Prepare for deployment

## Conclusion

This guide provides comprehensive coverage of all integration test scenarios. Complete all tests before moving to production deployment.

**Testing Contact:** [Your Team/Name]
**Last Updated:** April 14, 2026
**Version:** 1.0.0
