# Nesi API Documentation

## Base URL
`http://localhost:5000/api` (Development)

## Authentication
Currently using header-based authentication:
- Header: `X-User-Id: <userId>`
- TODO: Implement JWT Bearer tokens

## Endpoints

### Auth Controller

#### POST /api/auth/login
Login with username and password.

**Request Body:**
```json
{
  "username": "string",
  "password": "string"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "userId": 1,
    "username": "admin",
    "email": "admin@nesi.com",
    "firstName": "Admin",
    "lastName": "User",
    "role": 0,
    "token": "base64-encoded-token"
  }
}
```

**Response (401 Unauthorized):**
```json
{
  "success": false,
  "message": "Invalid username or password",
  "data": null,
  "errors": []
}
```

#### POST /api/auth/register
Register a new user.

**Request Body:**
```json
{
  "username": "string",
  "email": "string",
  "firstName": "string",
  "lastName": "string",
  "password": "string",
  "confirmPassword": "string",
  "role": 0
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "message": "User registered successfully",
  "data": 5
}
```

### Timesheet Controller

#### GET /api/timesheet
Get all timesheets with optional filtering and pagination.

**Query Parameters:**
- `userId` (optional): Filter by user ID
- `startDate` (optional): Filter by start date (ISO 8601)
- `endDate` (optional): Filter by end date (ISO 8601)
- `status` (optional): Filter by status (Draft, Submitted, Approved, Rejected)
- `pageNumber` (default: 1): Page number
- `pageSize` (default: 10): Items per page

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Success",
  "data": {
    "items": [
      {
        "id": 1,
        "userId": 1,
        "userName": "admin",
        "date": "2026-04-10T00:00:00",
        "hours": 8.0,
        "payTypeId": 1,
        "payTypeName": "Regular",
        "workOrderId": 1,
        "workOrderNumber": "WO-2026-001",
        "workOrderDescription": "Office Renovation",
        "jobTypeId": 1,
        "jobTypeName": "Carpentry",
        "notes": "Completed framing work",
        "status": 2,
        "createdAt": "2026-04-10T08:00:00",
        "updatedAt": null
      }
    ],
    "totalCount": 50,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 5,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

#### GET /api/timesheet/{id}
Get a specific timesheet by ID.

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Success",
  "data": {
    "id": 1,
    "userId": 1,
    "userName": "admin",
    "date": "2026-04-10T00:00:00",
    "hours": 8.0,
    "payTypeId": 1,
    "payTypeName": "Regular",
    "workOrderId": 1,
    "workOrderNumber": "WO-2026-001",
    "workOrderDescription": "Office Renovation",
    "jobTypeId": 1,
    "jobTypeName": "Carpentry",
    "notes": "Completed framing work",
    "status": 2,
    "createdAt": "2026-04-10T08:00:00",
    "updatedAt": null
  }
}
```

#### POST /api/timesheet
Create a new timesheet entry.

**Headers:**
- `X-User-Id: <userId>`

**Request Body:**
```json
{
  "date": "2026-04-14",
  "hours": 8.0,
  "payTypeId": 1,
  "workOrderId": 1,
  "jobTypeId": 1,
  "notes": "Completed work on project"
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "message": "Timesheet created successfully",
  "data": 51
}
```

#### PUT /api/timesheet/{id}
Update an existing timesheet entry.

**Headers:**
- `X-User-Id: <userId>`

**Request Body:**
```json
{
  "id": 51,
  "date": "2026-04-14",
  "hours": 7.5,
  "payTypeId": 1,
  "workOrderId": 1,
  "jobTypeId": 1,
  "notes": "Updated hours"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Timesheet updated successfully",
  "data": true
}
```

#### POST /api/timesheet/{id}/submit
Submit a timesheet for approval.

**Headers:**
- `X-User-Id: <userId>`

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Timesheet submitted successfully",
  "data": true
}
```

#### POST /api/timesheet/{id}/approve
Approve a timesheet (Manager/Admin only).

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Timesheet approved successfully",
  "data": true
}
```

#### POST /api/timesheet/{id}/reject
Reject a timesheet (Manager/Admin only).

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Timesheet rejected successfully",
  "data": true
}
```

### WorkOrder Controller

#### GET /api/workorder
Get all active work orders.

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Success",
  "data": [
    {
      "id": 1,
      "workOrderNumber": "WO-2026-001",
      "customerId": 1,
      "customerName": "Acme Construction",
      "description": "Office Renovation",
      "startDate": "2026-04-01T00:00:00",
      "endDate": "2026-06-30T00:00:00",
      "isActive": true,
      "createdAt": "2026-04-01T08:00:00"
    }
  ]
}
```

### User Controller

#### GET /api/user
Get all active users.

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Success",
  "data": [
    {
      "id": 1,
      "username": "admin",
      "email": "admin@nesi.com",
      "firstName": "Admin",
      "lastName": "User",
      "role": 0,
      "isActive": true,
      "createdAt": "2026-04-01T00:00:00"
    }
  ]
}
```

#### GET /api/user/{id}
Get a specific user by ID.

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Success",
  "data": {
    "id": 1,
    "username": "admin",
    "email": "admin@nesi.com",
    "firstName": "Admin",
    "lastName": "User",
    "role": 0,
    "isActive": true,
    "createdAt": "2026-04-01T00:00:00"
  }
}
```

## User Roles

```
0 = Admin
1 = Manager
2 = Employee
```

## Timesheet Status

```
0 = Draft
1 = Submitted
2 = Approved
3 = Rejected
```

## Error Responses

All endpoints may return error responses in this format:

**Response (400 Bad Request / 500 Internal Server Error):**
```json
{
  "success": false,
  "message": "Error message here",
  "data": null,
  "errors": ["Additional error details"]
}
```

## Testing with cURL

### Login
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin@123"}'
```

### Get Timesheets
```bash
curl -X GET "http://localhost:5000/api/timesheet?pageNumber=1&pageSize=10"
```

### Create Timesheet
```bash
curl -X POST http://localhost:5000/api/timesheet \
  -H "Content-Type: application/json" \
  -H "X-User-Id: 1" \
  -d '{
    "date": "2026-04-14",
    "hours": 8.0,
    "payTypeId": 1,
    "workOrderId": 1,
    "jobTypeId": 1,
    "notes": "Test timesheet"
  }'
```

## Swagger UI

When running in development mode, access interactive API documentation at:
`http://localhost:5000/swagger`
