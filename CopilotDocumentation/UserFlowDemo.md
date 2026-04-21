# NESI Application - Complete User Flow Demonstration Guide

## 📋 Table of Contents
1. [Introduction](#introduction)
2. [System Setup](#system-setup)
3. [Use Case 1: Customer & Quote Management](#uc1-customer-quote-management)
4. [Use Case 2: Work Order Execution](#uc2-work-order-execution)
5. [Use Case 3: Employee Time Tracking](#uc3-employee-time-tracking)
6. [Use Case 4: Purchase Order Processing](#uc4-purchase-order-processing)
7. [Use Case 5: Financial Reporting](#uc5-financial-reporting)
8. [Complete End-to-End Scenarios](#complete-scenarios)
9. [Troubleshooting](#troubleshooting)

---

## Introduction

This guide provides **step-by-step instructions** for demonstrating all use cases in the NESI (Network Electrical Services Inc.) application. Each section includes:
- Prerequisites
- Detailed step-by-step instructions
- Expected results
- Screenshots references
- Sample data
- Alternative flows

### Demo Environment

- **Backend API**: http://localhost:5000
- **Frontend App**: http://localhost:4200
- **Swagger Docs**: http://localhost:5000/swagger
- **Database**: SQL Server LocalDB

### Demo User Accounts

| Username | Password | Role | Use For |
|----------|----------|------|---------|
| admin@nesi.com | Admin123! | Admin | System administration, approvals |
| john.manager@nesi.com | Manager123! | Manager | Quote approvals, timesheet reviews |
| sarah.sales@nesi.com | Sales123! | Sales Rep | Customer & quote management |
| mike.tech@nesi.com | Tech123! | Field Tech | Time entry, work order execution |
| lisa.purchasing@nesi.com | Purchasing123! | Purchasing Agent | Purchase orders, vendor management |

---

## System Setup

### 1. Start the Backend API

```bash
cd /home/runner/work/Nesi/Nesi/CoPilotGeneratedCode/backend
dotnet run --project src/Nesi.Api/Nesi.Api.csproj
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### 2. Start the Frontend Application

```bash
cd /home/runner/work/Nesi/Nesi/CoPilotGeneratedCode/frontend
npm install  # First time only
npm start
```

**Expected Output:**
```
** Angular Live Development Server is listening on localhost:4200 **
✔ Compiled successfully.
```

### 3. Verify System is Running

1. Open browser to http://localhost:4200
2. You should see the NESI login page
3. Verify API is running at http://localhost:5000/swagger

---

## UC1: Customer & Quote Management

### 📝 **Use Case ID**: UC-001
### 🎯 **Goal**: Create and manage customer quotes through approval to work order conversion
### 👤 **Primary Actor**: Sales Representative
### ⏱️ **Duration**: 10-15 minutes

---

### Part 1: Customer Management

#### 1.1 Login as Sales Representative

**Steps:**
1. Navigate to http://localhost:4200
2. Enter credentials:
   - Username: `sarah.sales@nesi.com`
   - Password: `Sales123!`
3. Click **"Login"** button

**Expected Result:**
- Redirected to Dashboard
- Welcome message shows: "Welcome, Sarah"
- Navigation sidebar shows: Dashboard, Timesheets, Customers, Quotes, Work Orders

#### 1.2 View Customer List

**Steps:**
1. Click **"👥 Customers"** in the sidebar
2. Observe the customer list

**Expected Result:**
- Table displays existing customers with columns:
  - Customer # | Name | Contact | Email | Phone | Status | Actions
- Search box at top for filtering
- "Active Only" checkbox (checked by default)
- **"+ New Customer"** button in top-right
- Pagination controls at bottom (if >20 customers)

**Sample Data Visible:**
```
CUST-2026-00001 | ABC Construction Inc | John Smith | john@abc.com | (555) 123-4567 | Active
CUST-2026-00002 | XYZ Electrical Co | Jane Doe | jane@xyz.com | (555) 987-6543 | Active
```

#### 1.3 Create New Customer

**Steps:**
1. Click **"+ New Customer"** button
2. Fill in the form:
   - **Customer Name** (required): `Demo Facility Management`
   - **Contact Name**: `Robert Johnson`
   - **Email**: `robert@demofacility.com`
   - **Phone**: `(555) 234-5678`
   - **Address**: `123 Industrial Park Dr, Suite 200`
   - **Credit Limit**: `75000`
   - **Payment Terms (Days)**: `30`
   - **Business Unit ID**: `1`
   - **Account Manager ID**: `1`
   - **Notes**: `New customer - priority service required`
3. Click **"Create Customer"**

**Expected Result:**
- Success message appears: "Customer created successfully"
- Redirected to customer detail page
- Auto-generated customer number displayed: `CUST-2026-00003`
- All entered information displayed in detail view
- **Edit**, **Deactivate** buttons available

#### 1.4 View Customer Details

**Steps:**
1. From customer list, click 👁 (view icon) on any customer
2. Review the customer detail page

**Expected Result:**
- **Basic Information** section shows:
  - Customer Name, Business Unit, Contact Name, Email, Phone, Address
- **Financial Information** section shows:
  - Credit Limit, Payment Terms, Account Manager
- **System Information** section shows:
  - Created At, Last Updated dates
- Action buttons: **Back to Customers**, **✏️ Edit**, **🗑 Deactivate**

#### 1.5 Search and Filter Customers

**Steps:**
1. Return to customer list (click "Back to Customers")
2. In search box, type: `ABC`
3. Observe filtered results
4. Clear search box
5. Uncheck "Active Only" checkbox
6. Observe all customers including inactive

**Expected Result:**
- Search filters results in real-time
- Only matching customers displayed
- Filter by active status works correctly
- Counter shows filtered count

---

### Part 2: Quote Creation & Management

#### 2.1 Create Quote for Customer

**Steps:**
1. Navigate to **Quotes** from sidebar (or from customer detail, if available)
2. Click **"+ New Quote"** button
3. Fill in Quote Header:
   - **Customer**: Select `Demo Facility Management` from dropdown
   - **Quote Type**: Select `Time & Material`
   - **Description**: `Electrical panel upgrade and circuit installation`
   - **Scope**: `Replace existing 200A panel with 400A panel, install 6 new circuits for machinery, upgrade grounding system`
   - **Estimated Start Date**: Select date 2 weeks from today
   - **Estimated Completion Date**: Select date 4 weeks from today
   - **Project Manager**: Select from dropdown
   - **Terms and Conditions**: Use default or enter custom
   - **Tax Rate**: `8.5`

4. Add Line Items:

**Line Item 1 - Labor:**
   - **Item Type**: `Labor`
   - **Description**: `Electrician - Panel Installation`
   - **Job Type**: Select `Installation`
   - **Estimated Hours**: `24`
   - **Unit Price**: `85.00`
   - System calculates Total: `$2,040.00`

**Line Item 2 - Labor:**
   - **Item Type**: `Labor`
   - **Description**: `Master Electrician - Supervision`
   - **Job Type**: Select `Installation`
   - **Estimated Hours**: `8`
   - **Unit Price**: `125.00`
   - System calculates Total: `$1,000.00`

**Line Item 3 - Material:**
   - **Item Type**: `Material`
   - **Description**: `400A Main Panel with Breakers`
   - **Part Number**: `MP-400-HD`
   - **Quantity**: `1`
   - **Unit Price**: `2500.00`
   - System calculates Total: `$2,500.00`

**Line Item 4 - Material:**
   - **Item Type**: `Material`
   - **Description**: `Copper Wire 12 AWG (1000ft)`
   - **Part Number**: `CW-12-1000`
   - **Quantity**: `2`
   - **Unit Price**: `450.00`
   - System calculates Total: `$900.00`

**Line Item 5 - Equipment:**
   - **Item Type**: `Equipment`
   - **Description**: `Bucket Truck Rental - 3 days`
   - **Quantity**: `3`
   - **Unit Price**: `350.00`
   - System calculates Total: `$1,050.00`

**Line Item 6 - Miscellaneous:**
   - **Item Type**: `Miscellaneous`
   - **Description**: `Electrical Permit`
   - **Quantity**: `1`
   - **Unit Price**: `250.00`
   - System calculates Total: `$250.00`

5. Review Auto-Calculated Totals:
   - **Subtotal**: `$7,740.00`
   - **Tax (8.5%)**: `$657.90`
   - **Grand Total**: `$8,397.90`

6. Click **"Create Quote"**

**Expected Result:**
- Success message: "Quote created successfully"
- Redirected to quote detail page
- Auto-generated quote number: `QUOTE-2026-00001`
- Status badge shows: **"Draft"** (yellow/orange)
- All line items displayed in table
- Totals calculated correctly
- Actions available: **Submit**, **Edit**, **Delete**

#### 2.2 Submit Quote for Approval

**Steps:**
1. From quote detail page, click **"Submit"** button
2. Confirm submission in dialog: "Submit quote for manager approval?"
3. Click **"Yes, Submit"**

**Expected Result:**
- Success message: "Quote submitted for approval"
- Status badge changes to: **"Submitted"** (blue)
- Submit button disabled/hidden
- Edit and Delete buttons disabled/hidden
- Page shows: "Waiting for manager approval"

#### 2.3 Manager Approval (Switch User)

**Steps:**
1. Logout (click user menu → Logout)
2. Login as Manager:
   - Username: `john.manager@nesi.com`
   - Password: `Manager123!`
3. Navigate to **Quotes**
4. Click on the submitted quote (`QUOTE-2026-00001`)
5. Review quote details and line items
6. Click **"Approve"** button
7. Confirm approval: "Approve this quote?"
8. Click **"Yes, Approve"**

**Expected Result:**
- Success message: "Quote approved successfully"
- Status badge changes to: **"Approved"** (green)
- New action button appears: **"Customer Approve"**
- Quote ready for customer review

#### 2.4 Customer Approval

**Steps:**
1. Click **"Customer Approve"** button
2. Confirm: "Mark as customer approved?"
3. Click **"Yes, Customer Approved"**

**Expected Result:**
- Success message: "Quote marked as customer approved"
- Status badge changes to: **"Customer Approved"** (green)
- New action button appears: **"Convert to Work Order"**
- Quote ready for conversion

#### 2.5 Convert Quote to Work Order

**Steps:**
1. Click **"Convert to Work Order"** button
2. Confirm conversion: "Convert this quote to work order?"
3. Click **"Yes, Convert"**

**Expected Result:**
- Success message: "Work order created successfully: WO-2026-00001"
- Link to new work order displayed
- Quote status shows: **"Converted"** with work order reference
- Converted date/time recorded
- Quote marked as read-only
- Click link to view work order

---

## UC2: Work Order Execution

### 🔧 **Use Case ID**: UC-002
### 🎯 **Goal**: Execute and manage work order from creation to completion
### 👤 **Primary Actor**: Project Manager
### ⏱️ **Duration**: 15-20 minutes

---

### Part 1: Work Order Setup

#### 2.1 View Work Order Details

**Steps:**
1. After conversion (or navigate to **Work Orders** from sidebar)
2. Click on work order `WO-2026-00001`
3. Review work order details

**Expected Result:**
- **Work Order Header** displays:
  - Work Order #: `WO-2026-00001`
  - Customer: `Demo Facility Management`
  - Status: **"Pending"** (yellow)
  - Quote Reference: `QUOTE-2026-00001`
  - Scheduled Start: Date from quote
  - Scheduled End: Date from quote
  
- **Line Items** section shows:
  - All items from original quote
  - Labor hours, materials, equipment
  
- **Team Assignment** section shows:
  - Currently empty (no assigned technicians)
  
- **Documents** section shows:
  - Empty (ready for uploads)
  
- **Time Tracking** section shows:
  - No time entries yet
  
- Action buttons available:
  - **Assign Team**
  - **Add Document**
  - **Update Status**

#### 2.2 Assign Team Members

**Steps:**
1. Click **"Assign Team"** button
2. Add team members:
   
   **Assignment 1:**
   - **Employee**: Select `Mike Tech` from dropdown
   - **Role**: `Lead Electrician`
   - **Start Date**: Work order start date
   - **End Date**: Work order end date
   - Click **"Add Assignment"**
   
   **Assignment 2:**
   - **Employee**: Select `Tom Helper` from dropdown
   - **Role**: `Electrician`
   - **Start Date**: Work order start date
   - **End Date**: Work order end date
   - Click **"Add Assignment"**

3. Click **"Save Assignments"**

**Expected Result:**
- Success message: "Team assignments saved"
- **Team Assignment** section now shows:
  ```
  Mike Tech - Lead Electrician (Scheduled: [dates])
  Tom Helper - Electrician (Scheduled: [dates])
  ```
- Assigned employees receive notification (if SignalR implemented)
- Employees can now see work order in their dashboard

#### 2.3 Update Work Order Status to In Progress

**Steps:**
1. Click **"Update Status"** button
2. Select new status: **"In Progress"**
3. Enter notes: `Team started work on panel installation`
4. Click **"Update"**

**Expected Result:**
- Status badge changes to: **"In Progress"** (blue)
- Status history updated with timestamp
- Notes recorded in audit log

---

### Part 2: Work Execution

#### 2.4 Field Technician Views Work Order (Switch User)

**Steps:**
1. Logout and login as Field Technician:
   - Username: `mike.tech@nesi.com`
   - Password: `Tech123!`
2. Navigate to **Dashboard**
3. View **"My Work Orders"** widget
4. Click on `WO-2026-00001`

**Expected Result:**
- Dashboard shows assigned work orders
- Work order details visible to technician
- Technician can see:
  - Customer information
  - Work location
  - Scope of work
  - Materials list
  - Safety requirements
- Action available: **"Log Time"**

#### 2.5 Add Work Order Documents

**Steps:**
1. From work order detail, click **"Add Document"** button
2. Fill in document information:
   - **Document Type**: Select `Photo`
   - **Title**: `Panel Before Work`
   - **Description**: `Photo of existing 200A panel before removal`
   - **File**: Upload image file (if upload implemented)
3. Click **"Upload Document"**

4. Repeat for additional documents:
   - `Panel During Installation` (Photo)
   - `Final Panel Configuration` (Photo)
   - `Electrical Permit` (Permit)
   - `Safety Checklist` (Safety Document)

**Expected Result:**
- Documents appear in **Documents** section
- Each document shows:
  - Title, type, uploaded by, date
  - Download/view action
- Document count badge updates

---

### Part 3: Work Completion

#### 2.6 Complete Work and Close Work Order

**Steps:**
1. Login as Project Manager
2. Navigate to work order `WO-2026-00001`
3. Verify all requirements met:
   - ✅ All time logged
   - ✅ All materials used/documented
   - ✅ Required documents uploaded
   - ✅ Customer walkthrough completed
4. Click **"Update Status"** button
5. Select: **"Completed"**
6. Enter completion notes: `Work completed successfully. Customer satisfied with installation. All permits closed.`
7. Click **"Update"**

**Expected Result:**
- Status changes to: **"Completed"** (green)
- Work order marked as closed
- Invoice generation triggered (if implemented)
- Customer notification sent
- Work order appears in completed list
- Job cost calculation finalized

---

## UC3: Employee Time Tracking

### ⏱️ **Use Case ID**: UC-003
### 🎯 **Goal**: Track employee time for payroll and job costing
### 👤 **Primary Actor**: Field Technician / Employee
### ⏱️ **Duration**: 5-10 minutes

---

### Part 1: Time Entry

#### 3.1 Access Timesheet Module

**Steps:**
1. Login as Field Technician: `mike.tech@nesi.com`
2. Click **"⏱️ Timesheets"** in sidebar
3. View current week timesheet

**Expected Result:**
- Current week displayed (Monday-Sunday)
- Grid shows days of week across top
- Existing time entries displayed
- **"+ Add Time Entry"** button visible
- Summary shows:
  - Total hours for week
  - Regular hours
  - Overtime hours (if >40)
- Status shows: **"Draft"** or **"Submitted"**

#### 3.2 Add Time Entry for Work Order

**Steps:**
1. Click **"+ Add Time Entry"** button
2. Fill in time entry form:
   - **Date**: Select today's date
   - **Work Order**: Select `WO-2026-00001 - Demo Facility Management`
   - **Pay Type**: Select `Regular`
   - **Job Type**: Select `Installation`
   - **Hours**: `8.0`
   - **Notes**: `Installed main panel and roughed in circuits`
   - **Business Unit**: Auto-selected based on work order
3. Click **"Save"**

**Expected Result:**
- Time entry added to grid
- Entry shows in correct date column
- Hours total updated: `+8.0 hours`
- Work order dropdown filtered to assigned work orders
- Entry can be edited (pencil icon)
- Entry can be deleted (trash icon)

#### 3.3 Add Multiple Time Entries

**Steps:**
1. Click **"+ Add Time Entry"** again
2. Add entries for different days:

**Entry 2 - Next Day:**
   - Date: Tomorrow
   - Work Order: `WO-2026-00001`
   - Pay Type: `Regular`
   - Job Type: `Installation`
   - Hours: `8.0`
   - Notes: `Completed panel wiring and tested circuits`

**Entry 3 - Two Days Later:**
   - Date: Two days from now
   - Work Order: `WO-2026-00001`
   - Pay Type: `Regular`
   - Job Type: `Installation`
   - Hours: `7.5`
   - Notes: `Final connections and cleanup`

**Entry 4 - PTO Day:**
   - Date: Three days from now
   - Pay Type: `Vacation`
   - Hours: `8.0`
   - Notes: `Vacation day`
   - (No work order required for PTO)

**Expected Result:**
- Multiple entries displayed in grid
- Each entry in correct date column
- Total hours: `31.5 hours` (23.5 regular + 8.0 PTO)
- Entries grouped by date
- Summary updated with breakdown

#### 3.4 Edit Time Entry

**Steps:**
1. Click pencil icon on first time entry
2. Change hours from `8.0` to `8.5`
3. Update notes: `Installed main panel and roughed in circuits - stayed late for inspection`
4. Click **"Save"**

**Expected Result:**
- Entry updated with new hours
- Total hours recalculated: `32.0 hours`
- Last modified timestamp updated
- Edit audit trail recorded

#### 3.5 Validate Business Rules

**Steps:**
1. Try to add invalid entry:
   - Date: Future date (more than 7 days out)
   - Hours: `25.0` (exceeds daily limit)
2. Click **"Save"**

**Expected Result:**
- Validation errors displayed:
  - ❌ "Cannot enter time for future dates beyond 7 days"
  - ❌ "Hours cannot exceed 24 per day"
- Entry not saved
- Form stays open with error messages

---

### Part 2: Timesheet Submission & Approval

#### 3.6 Submit Timesheet for Approval

**Steps:**
1. Review all time entries for accuracy
2. Verify total hours: `32.0 hours`
3. Click **"Submit Timesheet"** button
4. Confirm submission: "Submit timesheet for approval?"
5. Click **"Yes, Submit"**

**Expected Result:**
- Success message: "Timesheet submitted for approval"
- Status changes to: **"Submitted"** (blue)
- All entries become read-only (cannot edit/delete)
- Submit button disabled
- Notification sent to supervisor

#### 3.7 Supervisor Reviews Timesheet (Switch User)

**Steps:**
1. Logout and login as Manager: `john.manager@nesi.com`
2. Click **"✅ Review Timesheets"** in sidebar
3. View pending timesheets list
4. Click on Mike Tech's timesheet for current week

**Expected Result:**
- List shows all submitted timesheets:
  ```
  Mike Tech | Week of [date] | 32.0 hours | Submitted | [View]
  ```
- Clicking **View** shows timesheet detail
- All time entries visible with:
  - Date, Work Order, Pay Type, Job Type, Hours, Notes
- Actions available: **Approve**, **Reject**

#### 3.8 Approve Timesheet

**Steps:**
1. Review each time entry
2. Verify hours against work order progress
3. Click **"Approve"** button
4. Confirm approval: "Approve timesheet?"
5. Click **"Yes, Approve"**

**Expected Result:**
- Success message: "Timesheet approved"
- Status changes to: **"Approved"** (green)
- Employee notified of approval
- Hours allocated to work orders for job costing
- Hours exported to payroll system (if integrated)
- Timesheet locked (no further changes)

#### 3.9 Reject Timesheet (Alternative Flow)

**Alternative Steps:**
1. If discrepancies found, click **"Reject"** button
2. Enter rejection reason: `Hours on 3/15 need correction - should be 7.5 not 8.5`
3. Click **"Submit Rejection"**

**Expected Result:**
- Timesheet status: **"Rejected"** (red)
- Employee receives notification with reason
- Timesheet unlocked for employee to edit
- Employee can make corrections and resubmit

---

## UC4: Purchase Order Processing

### 🛒 **Use Case ID**: UC-004
### 🎯 **Goal**: Procure materials while maintaining cost control
### 👤 **Primary Actor**: Purchasing Agent
### ⏱️ **Duration**: 10-15 minutes

---

### Part 1: Material Requisition

#### 4.1 Create Purchase Requisition

**Steps:**
1. Login as Purchasing Agent: `lisa.purchasing@nesi.com`
2. Navigate to **Purchase Orders** (or **Procurement**)
3. Click **"+ New Purchase Order"** button
4. Fill in PO header:
   - **Vendor**: Select `ABC Electrical Supply` from dropdown
   - **Work Order**: Select `WO-2026-00001` (to link costs)
   - **Business Unit**: Auto-filled from work order
   - **Ship To Address**: Select warehouse address
   - **Expected Delivery**: Select date 5 days from now
   - **Payment Terms**: `Net 30`
   - **Notes**: `Rush order for panel upgrade project`

**Expected Result:**
- PO form displayed with header section
- Vendor information auto-populated:
  - Vendor contact, phone, email
- Ship-to and bill-to addresses displayed
- Line items section ready for input

#### 4.2 Add Purchase Order Line Items

**Steps:**
1. Add line items for materials needed:

**Line Item 1:**
   - **Part Number**: `MP-400-HD` (can search or select)
   - **Description**: `400A Main Panel with Breakers`
   - **Quantity**: `1`
   - **Unit Price**: `$2,500.00`
   - **GL Account**: Auto-filled based on material category
   - System calculates: Line Total = `$2,500.00`
   - Click **"Add Item"**

**Line Item 2:**
   - **Part Number**: `CW-12-1000`
   - **Description**: `Copper Wire 12 AWG (1000ft roll)`
   - **Quantity**: `2`
   - **Unit Price**: `$450.00`
   - Line Total = `$900.00`
   - Click **"Add Item"**

**Line Item 3:**
   - **Part Number**: `CB-20A-10PK`
   - **Description**: `20A Circuit Breakers (10 pack)`
   - **Quantity**: `2`
   - **Unit Price**: `$85.00`
   - Line Total = `$170.00`
   - Click **"Add Item"**

2. Review totals:
   - **Subtotal**: `$3,570.00`
   - **Tax (8.5%)**: `$303.45`
   - **Shipping**: `$75.00`
   - **Grand Total**: `$3,948.45`

3. Click **"Create Purchase Order"**

**Expected Result:**
- Success message: "Purchase order created: PO-2026-00001"
- PO detail page displayed
- Status: **"Draft"** (yellow)
- Auto-generated PO number: `PO-2026-00001`
- All line items listed
- Actions available: **Submit for Approval**, **Edit**, **Delete**

#### 4.3 Submit PO for Approval (if over threshold)

**Steps:**
1. If PO total > approval threshold ($2,500):
2. Click **"Submit for Approval"** button
3. Confirm submission

**Expected Result:**
- Status changes to: **"Pending Approval"** (blue)
- Manager notified
- PO locked for editing

#### 4.4 Manager Approves PO

**Steps:**
1. Login as Manager
2. Navigate to **Purchase Orders**
3. Filter by status: **Pending Approval**
4. Click on `PO-2026-00001`
5. Review line items and total
6. Verify budget availability
7. Click **"Approve"** button

**Expected Result:**
- Status changes to: **"Approved"** (green)
- PO document generated (PDF)
- Ready to send to vendor

#### 4.5 Send PO to Vendor

**Steps:**
1. From PO detail page, click **"Send to Vendor"** button
2. Verify vendor email address
3. Preview PO document (PDF)
4. Click **"Send Email"**

**Expected Result:**
- Email sent to vendor with PO attachment
- Status updated to: **"Sent"** with sent date/time
- Vendor receives professional PO document
- PO tracking begins

---

### Part 2: Material Receipt

#### 4.6 Receive Materials

**Steps:**
1. When materials arrive, navigate to PO `PO-2026-00001`
2. Click **"Receive Materials"** button
3. For each line item, enter received quantity:
   
   **Item 1: Main Panel**
   - Ordered: `1`
   - Received: `1`
   - Condition: `Good`
   - Notes: ``
   
   **Item 2: Copper Wire**
   - Ordered: `2`
   - Received: `2`
   - Condition: `Good`
   - Notes: ``
   
   **Item 3: Circuit Breakers**
   - Ordered: `2`
   - Received: `2`
   - Condition: `Good`
   - Notes: ``

4. Click **"Complete Receipt"**

**Expected Result:**
- Receipt record created with date/time
- Inventory quantities updated (if inventory module)
- Status changes to: **"Received"**
- Materials linked to work order
- Receipt notification sent
- Ready for invoice matching

#### 4.7 Match Invoice (3-Way Match)

**Steps:**
1. When vendor invoice received, navigate to PO
2. Click **"Match Invoice"** button
3. Enter invoice details:
   - **Invoice Number**: `INV-12345`
   - **Invoice Date**: Today's date
   - **Invoice Amount**: `$3,948.45`
   - **Payment Due Date**: 30 days from invoice date
4. System performs 3-way match:
   - ✅ PO amount matches invoice
   - ✅ Receipt quantities match invoice
   - ✅ Prices match
5. Click **"Approve for Payment"**

**Expected Result:**
- Invoice matched successfully
- Status changes to: **"Approved for Payment"**
- Payment scheduled according to terms
- Accounting notified
- Costs allocated to work order

---

## UC5: Financial Reporting

### 📊 **Use Case ID**: UC-005
### 🎯 **Goal**: Generate accurate financial reports for decision-making
### 👤 **Primary Actor**: Manager / Accounting Staff
### ⏱️ **Duration**: 5-10 minutes

---

### Part 1: Job Cost Analysis

#### 5.1 Access Reports Module

**Steps:**
1. Login as Manager or Accounting user
2. Navigate to **Reports** or **Analytics** from sidebar
3. View reports dashboard

**Expected Result:**
- Reports dashboard displays:
  - Report categories (Financial, Operations, HR)
  - Quick links to common reports
  - Recent report history
  - Schedule report options

#### 5.2 Generate Job Cost Report

**Steps:**
1. Click **"Job Cost Report"**
2. Select parameters:
   - **Work Order**: Select `WO-2026-00001` or leave blank for all
   - **Date Range**: This month
   - **Business Unit**: All or specific
   - **Group By**: Work Order
   - **Include**: Select all (Labor, Materials, Equipment, Other)
3. Click **"Generate Report"**

**Expected Result:**
- Report displays with sections:

**Summary:**
```
Work Order: WO-2026-00001 - Demo Facility Management
Status: Completed
Quote Amount: $8,397.90
Actual Cost: $7,245.32
Variance: +$1,152.58 (13.7% under budget)
Profit Margin: 17.9%
```

**Cost Breakdown:**
| Category | Budgeted | Actual | Variance |
|----------|----------|--------|----------|
| Labor | $3,040.00 | $2,890.00 | +$150.00 |
| Materials | $3,850.00 | $3,948.45 | -$98.45 |
| Equipment | $1,050.00 | $1,050.00 | $0.00 |
| Permits/Misc | $250.00 | $250.00 | $0.00 |
| **Total** | **$8,190.00** | **$8,138.45** | **+$51.55** |

**Labor Detail:**
- Total Hours: 34.0
- Regular Hours: 34.0 @ $85/hr
- Overtime Hours: 0
- Employees: Mike Tech (24 hrs), Tom Helper (10 hrs)

**Material Detail:**
- Main Panel: $2,500.00
- Copper Wire: $900.00
- Circuit Breakers: $170.00
- Misc materials: $378.45

**Actions:**
- Export to Excel
- Export to PDF
- Save to favorites
- Schedule email

#### 5.3 Generate Income Statement

**Steps:**
1. From reports dashboard, click **"Income Statement"**
2. Select parameters:
   - **Period**: This Month
   - **Business Unit**: All
   - **Comparison**: Prior Month
3. Click **"Generate"**

**Expected Result:**
- Income statement displays:

```
NESI - Income Statement
Month Ending: [Current Month]

REVENUE
  Service Revenue                 $125,450.00
  Product Sales                    $42,300.00
  Other Income                      $1,250.00
                                  -----------
  Total Revenue                   $169,000.00

COST OF GOODS SOLD
  Direct Labor                     $68,500.00
  Materials                        $35,200.00
  Subcontractors                   $12,800.00
                                  -----------
  Total COGS                      $116,500.00

GROSS PROFIT                       $52,500.00
Gross Margin                          31.1%

OPERATING EXPENSES
  Salaries & Wages                 $22,000.00
  Rent & Utilities                  $4,500.00
  Insurance                         $3,200.00
  Vehicle Expenses                  $2,800.00
  Equipment Depreciation            $1,800.00
  Office Expenses                   $1,200.00
  Marketing                           $800.00
  Professional Fees                   $600.00
                                  -----------
  Total Operating Expenses         $36,900.00

NET INCOME                         $15,600.00
Net Margin                             9.2%
```

#### 5.4 Customer Rate Analysis

**Steps:**
1. Click **"Customer Rate Analysis"**
2. Select parameters:
   - **Date Range**: Last 6 months
   - **Customers**: Top 10 or specific
   - **Service Types**: All
3. Generate report

**Expected Result:**
- Comparative analysis shows:
  - Average hourly rate by customer
  - Service mix by customer
  - Profit margin by customer
  - Pricing opportunities identified
  - Customer ranking by profitability

#### 5.5 Outstanding Invoices (AR Aging)

**Steps:**
1. Click **"Accounts Receivable Aging"**
2. Select: **As of Today**
3. Generate report

**Expected Result:**
```
AR Aging Report - As of [Date]

Customer | Current | 1-30 Days | 31-60 Days | 61-90 Days | >90 Days | Total
---------|---------|-----------|------------|------------|----------|-------
ABC Construction | $5,200 | $0 | $0 | $0 | $0 | $5,200
Demo Facility | $8,398 | $0 | $0 | $0 | $0 | $8,398
XYZ Electrical | $0 | $3,450 | $0 | $0 | $0 | $3,450
...

Totals | $45,300 | $8,920 | $2,100 | $0 | $500 | $56,820

Summary:
- Total AR: $56,820
- Current: $45,300 (79.7%)
- Past Due: $11,520 (20.3%)
- Average Days Outstanding: 18.5
- Collections Needed: Follow up on $2,600 (>60 days)
```

---

## Complete Scenarios

### 🎭 **Scenario 1**: New Project - Start to Finish

**Duration**: 30-45 minutes  
**Objective**: Complete workflow from customer inquiry to job completion

#### Timeline:

**Week 1 - Sales & Quoting:**
1. **Day 1**: Create new customer (`CUST-2026-00005`)
2. **Day 1**: Create quote (`QUOTE-2026-00005`) with detailed line items
3. **Day 2**: Submit quote for internal approval
4. **Day 2**: Manager approves quote
5. **Day 3**: Customer approves quote
6. **Day 3**: Convert quote to work order (`WO-2026-00005`)

**Week 2 - Planning:**
7. **Day 1**: Project manager assigns team to work order
8. **Day 1**: Create purchase orders for materials
9. **Day 2**: Manager approves purchase orders
10. **Day 2**: Send POs to vendors
11. **Day 3-5**: Materials ordered and received

**Week 3-4 - Execution:**
12. **Day 1**: Update work order status to "In Progress"
13. **Daily**: Technicians log time against work order
14. **Daily**: Upload progress photos and documents
15. **Weekly**: Supervisor approves timesheets
16. **Last Day**: Project manager marks work order complete

**Week 5 - Closing:**
17. **Day 1**: Receive and match vendor invoices
18. **Day 2**: Generate job cost report
19. **Day 2**: Create customer invoice
20. **Day 3**: Send invoice to customer
21. **Day 15**: Receive payment
22. **Day 15**: Close work order

---

### 🎭 **Scenario 2**: Emergency Service Call

**Duration**: 10-15 minutes  
**Objective**: Demonstrate rapid response for urgent work

#### Steps:

1. **Customer calls with electrical emergency**
   - Dispatcher creates customer (if new) or finds existing
   - Creates direct work order (no quote needed for emergency)
   - Status: "Emergency"

2. **Immediate dispatch**
   - Assign available technician
   - Technician receives notification
   - Status: "In Progress"

3. **Field work**
   - Technician logs time real-time
   - Uploads photos of problem and repair
   - Documents materials used

4. **Completion**
   - Technician marks complete on mobile/site
   - Manager reviews and approves
   - Generate invoice with emergency surcharge
   - Send to customer

**Expected Completion**: Same day service

---

### 🎭 **Scenario 3**: Multi-Day Project with Change Orders

**Duration**: 20-30 minutes  
**Objective**: Demonstrate handling project changes

#### Flow:

1. **Initial Work Order**: Original scope for $10,000
2. **Day 3**: Customer requests additional work
3. **Create Change Order**:
   - Add to existing work order
   - New line items for additional scope
   - Updated total: $13,500
   - Requires new approval
4. **Approval Process**:
   - Submit change order
   - Manager approves
   - Customer approves additional cost
5. **Continue Work**: Team completes expanded scope
6. **Final Invoice**: Reflects all work including changes

---

## Troubleshooting

### Common Issues and Solutions

#### Issue 1: Cannot Login

**Symptoms:**
- "Invalid username or password" error
- Page redirects to login again

**Solutions:**
1. Verify credentials are typed correctly
2. Check Caps Lock is off
3. Ensure backend API is running (check http://localhost:5000/swagger)
4. Check browser console for errors (F12)
5. Clear browser cache and cookies
6. Verify user exists in database

**Test:**
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin@nesi.com","password":"Admin123!"}'
```

---

#### Issue 2: Quote Totals Not Calculating

**Symptoms:**
- Subtotal shows $0.00
- Tax not calculated
- Line item totals incorrect

**Solutions:**
1. Verify all required fields filled:
   - Quantity > 0
   - Unit Price > 0
2. Check tax rate is entered (e.g., 8.5)
3. Click out of field to trigger calculation
4. Refresh page if calculations seem stuck
5. Check browser console for JavaScript errors

---

#### Issue 3: Timesheet Submission Failed

**Symptoms:**
- "Submit" button does nothing
- Error message: "Validation failed"

**Solutions:**
1. Check all entries have:
   - Valid date (not future beyond 7 days)
   - Work order selected (except PTO)
   - Hours between 0.25 and 24
   - Pay type selected
2. Ensure total daily hours don't exceed 24
3. Verify work order is active/assigned
4. Check for duplicate entries (same day, same work order)

---

#### Issue 4: Work Order Status Won't Update

**Symptoms:**
- Status dropdown disabled
- Update button greyed out

**Solutions:**
1. Verify user has permission (Project Manager role)
2. Check work order isn't already completed/closed
3. Ensure required fields completed:
   - Team assigned
   - Scheduled dates set
4. Check for business rule violations:
   - Can't mark complete without time entries
   - Can't close with open POs
5. Refresh page and try again

---

#### Issue 5: Purchase Order Not Appearing in List

**Symptoms:**
- PO created but not in list
- Search returns no results

**Solutions:**
1. Check filter settings:
   - Status filter (Draft, Approved, etc.)
   - Date range filter
   - Business unit filter
2. Clear all filters and search again
3. Verify PO was successfully created (check confirmation message)
4. Check database directly if admin:
   ```sql
   SELECT TOP 10 * FROM PurchaseOrders ORDER BY CreatedAt DESC
   ```
5. Refresh browser page

---

### Performance Issues

#### Slow Page Load Times

**Solutions:**
1. Check network tab in browser dev tools (F12)
2. Verify backend API response times:
   - Should be < 500ms for list endpoints
   - Should be < 200ms for single entity endpoints
3. Check database query performance
4. Clear browser cache
5. Disable browser extensions
6. Check server resources (CPU, memory)

---

#### API Timeouts

**Solutions:**
1. Increase timeout in proxy.conf.json:
   ```json
   {
     "timeout": 120000
   }
   ```
2. Optimize database queries
3. Add pagination to large lists
4. Implement caching where appropriate
5. Check database indexes

---

### Data Issues

#### Missing Reference Data

**Symptoms:**
- Dropdowns empty
- "No options available"

**Solutions:**
1. Verify seed data loaded:
   ```bash
   dotnet ef database update
   ```
2. Check reference tables populated:
   - PayTypes
   - JobTypes
   - BusinessUnits
3. Run seed script if needed
4. Check API endpoint returns data:
   ```
   GET /api/lookups/pay-types
   GET /api/lookups/job-types
   ```

---

## Appendix

### Sample Test Data

#### Customers
```
Name: ABC Construction Inc
Contact: John Smith
Email: john@abc.com
Phone: (555) 123-4567
Credit Limit: $50,000
Payment Terms: Net 30

Name: XYZ Electrical Co
Contact: Jane Doe
Email: jane@xyz.com
Phone: (555) 987-6543
Credit Limit: $75,000
Payment Terms: Net 45
```

#### Job Types
- Installation
- Troubleshooting
- Maintenance
- Repair
- Inspection
- Travel
- Training

#### Pay Types
- Regular
- Overtime
- Double Time
- Vacation
- Sick Leave
- Holiday

### Quick Reference Commands

**Start Backend:**
```bash
cd /path/to/backend
dotnet run --project src/Nesi.Api/Nesi.Api.csproj
```

**Start Frontend:**
```bash
cd /path/to/frontend
npm start
```

**Database Reset:**
```bash
dotnet ef database drop --force
dotnet ef database update
```

**Run Tests:**
```bash
# Backend
dotnet test

# Frontend
npm test
```

---

## Glossary

- **UC**: Use Case
- **PO**: Purchase Order
- **WO**: Work Order
- **AR**: Accounts Receivable
- **AP**: Accounts Payable
- **PTO**: Paid Time Off
- **COGS**: Cost of Goods Sold
- **GL**: General Ledger

---

## Demo Checklist

Before each demonstration, verify:

- [ ] Backend API running (http://localhost:5000)
- [ ] Frontend app running (http://localhost:4200)
- [ ] Database has seed data
- [ ] Test user accounts active
- [ ] Sample customers exist
- [ ] Sample work orders exist
- [ ] Browser cache cleared
- [ ] Demo script reviewed
- [ ] Backup slides/notes ready
- [ ] Timer/stopwatch ready

---

## Support

For issues during demonstration:
- Check browser console (F12)
- Check backend logs
- Verify database connectivity
- Review API responses in Network tab
- Contact system administrator if persists

---

**Document Version**: 1.0  
**Last Updated**: 2026-04-21  
**Author**: NESI Development Team  
**Feedback**: Please report issues or suggestions for improvement
