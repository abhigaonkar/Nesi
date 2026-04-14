# NESI Application Documentation

This directory contains comprehensive HTML documentation for the NESI application.

## Documentation Files

### Core Documentation
1. **application-description.html** - Comprehensive overview of the NESI application, its purpose, features, and business value
2. **technologies.html** - Detailed breakdown of all technologies, frameworks, and tools used in the application
3. **modules.html** - Description of all major modules and their functionality
4. **database.html** - Database schema documentation with tables, relationships, and data structures

### New Documentation (Added)
5. **security.html** - Security architecture including:
   - OAuth 2.0 Bearer Token authentication
   - Multi-level authorization (page, privilege, role)
   - User types (Employee, Customer, Vendor)
   - Password policies and session management
   - Audit logging

6. **technical-architecture.html** - N-Tier architecture documentation:
   - Layer descriptions (Frontend, API, BLL, Data, Database)
   - Key patterns (Repository, DI with Ninject, @ngrx, SignalR)
   - API structure and routing
   - Configuration management

7. **user-journey.html** - Typical user workflows:
   - Employee login to timesheet entry
   - Sales quote creation to WO conversion
   - Purchase order workflow
   - Customer portal access

8. **architecture-layers.html** - Detailed layer breakdown:
   - Angular frontend structure
   - ASP.NET Web API layer
   - Business Logic Layer
   - Data Access Layer with Entity Framework
   - Database layer

9. **use-cases.html** - Key business use cases:
   - Create and manage customer quotes
   - Execute work orders
   - Track employee time
   - Process purchase orders
   - Generate financial reports

10. **application-flow.html** - Data flow diagrams:
    - Quote to Work Order flow
    - Time entry to payroll flow
    - Purchase order flow
    - Authentication flow

## Viewing Documentation

Open any HTML file in a web browser. All files use consistent styling with:
- Gradient purple theme (#667eea to #764ba2)
- Responsive tables
- Back links to main documentation index
- Professional formatting with sections, code blocks, and diagrams

## Documentation Standards

All documentation files follow these standards:
- **Consistent Styling**: Same CSS theme across all files
- **Comprehensive Content**: Based on actual codebase analysis
- **Professional Format**: Tables, diagrams, code examples, and visual hierarchies
- **Accessibility**: Semantic HTML with proper heading structure
- **Responsive**: Mobile-friendly design

## Key Features

- 🎨 Consistent visual design with gradient purple theme
- 📊 Responsive tables with hover effects
- 💻 Code blocks with syntax highlighting
- 🔗 Navigation links between documents
- 📱 Mobile-responsive layout
- 🎯 Clear section hierarchies
- ✅ Professional documentation standards

## File Sizes

| File | Size | Description |
|------|------|-------------|
| security.html | 28K | Security and authentication |
| technical-architecture.html | 30K | N-tier architecture |
| user-journey.html | 28K | User workflows |
| architecture-layers.html | 33K | Detailed layer breakdown |
| use-cases.html | 29K | Business use cases |
| application-flow.html | 33K | Data flow diagrams |

Total: ~180KB of new comprehensive documentation

---

**Documentation Created:** April 2024  
**Based on:** NESI Application Codebase Analysis  
**Format:** HTML with embedded CSS  
**Theme:** Gradient Purple (#667eea → #764ba2)
