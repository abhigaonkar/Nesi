# NESI Code Generation Guidelines

**IMPORTANT: All code generated for this project MUST follow these guidelines.**

## Purpose
These guidelines ensure consistency, maintainability, and quality across the entire codebase. They serve as rules for both human developers and AI code generators.

## Guidelines Files

1. **backend-guidelines.md** - .NET/C# coding standards
2. **frontend-guidelines.md** - Angular/TypeScript standards
3. **database-guidelines.md** - Database design and EF Core standards
4. **testing-guidelines.md** - Testing patterns and coverage requirements
5. **naming-conventions.md** - Naming standards across all layers

## How to Use

### For Developers
- Read all guidelines before writing code
- Use as a checklist during code reviews
- Refer back when uncertain about patterns

### For AI Code Generation
- Include relevant guideline sections in prompts
- Validate generated code against guidelines
- Iterate until code meets all standards

## Enforcement
- All PRs must pass guideline compliance checks
- Use linters and formatters configured per guidelines
- Code reviews verify guideline adherence

## Key Principles

### Consistency
- Same patterns across entire codebase
- Predictable structure and naming
- Uniform error handling and logging

### Quality
- Clean Architecture principles
- SOLID principles
- DRY (Don't Repeat Yourself)
- KISS (Keep It Simple, Stupid)

### Maintainability
- Self-documenting code
- Comprehensive tests
- Clear separation of concerns
- Minimal technical debt

### Security
- Input validation everywhere
- Parameterized queries (no SQL injection)
- JWT authentication
- Role-based authorization
- Secure password handling

## Technology Stack

### Backend
- **Framework:** ASP.NET Core 8
- **Language:** C# 12
- **Architecture:** Clean Architecture (Onion/Hexagonal)
- **Patterns:** CQRS, Repository, Dependency Injection
- **ORM:** Entity Framework Core 8
- **Database:** MySQL 8.0
- **Authentication:** JWT with ASP.NET Core Identity
- **Validation:** FluentValidation
- **Testing:** xUnit, Moq, FluentAssertions

### Frontend
- **Framework:** Angular 19.2.20+ (standalone components)
- **Language:** TypeScript 5.4+
- **UI Library:** Angular Material 19+
- **State Management:** NgRx or Angular Signals
- **Forms:** Reactive Forms
- **Testing:** Jest, Playwright
- **Security Note:** Angular 18.x has unpatched XSS/XSRF vulnerabilities. Angular 19.2.20+ is required.

### Development
- **Version Control:** Git with conventional commits
- **Code Quality:** ESLint, Prettier, StyleCop
- **Documentation:** Swagger/OpenAPI
- **Local Development:** No containerization (local MySQL)

## Quick Reference

### File Naming
- **Backend:** PascalCase.cs (UserService.cs)
- **Frontend:** kebab-case.type.ts (user.service.ts)
- **Database:** PascalCase tables (Users, Timesheets)

### Code Structure
- **Backend:** Feature folders (Features/Timesheets/Commands/)
- **Frontend:** Feature modules (features/timesheet/)
- **Tests:** Mirror source structure

### API Conventions
- **URLs:** /api/v1/resource-name
- **Methods:** GET (retrieve), POST (create), PUT (update), DELETE (remove)
- **Status Codes:** 200 OK, 201 Created, 400 Bad Request, 401 Unauthorized, 404 Not Found, 500 Server Error

## Getting Started

1. Read all guideline documents in order
2. Review example code snippets provided
3. Setup linters and formatters per guidelines
4. Start coding with guidelines as reference
5. Validate code against guidelines before committing

## Updates

These guidelines are living documents. When technology or best practices evolve, guidelines will be updated. Always refer to the latest version.

## Contact

For questions or clarifications about these guidelines, create an issue in the repository.

---

**Last Updated:** April 2026  
**Version:** 1.0  
**Applies To:** NESI Demo Application (Local Development)
