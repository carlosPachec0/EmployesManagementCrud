# Memory

## Project Overview
See
# Imported from README.md
# EmployeesManagement

A test project exploring how to implement cookie-based authentication in ASP.NET Core MVC. Includes a simple employee management feature to exercise the auth flow with API versioning and MVC/API separation.

## Tech Stack

- **.NET 10.0** — ASP.NET Core MVC
- **Entity Framework Core 10.0** — ORM with SQL Server
- **Asp.Versioning.Mvc** — API versioning (`/api/v1/...`)
- **Bootstrap 5** — UI framework

## Architecture

The app follows a dual-controller pattern:

- **MVC Controllers** (`Controllers/`) — serve views only, no data logic
- **API Controllers** (`Controllers/Api/V1/`) — handle all data operations (read/write) via JSON

All data flows through the versioned API layer. MVC controllers are thin view-servers.

### Key Patterns

- **Soft deletes**: Global query filters on `IsActive` for `User` and `Employee`
- **Audit fields**: `CreatedAt`/`ModifiedAt` set automatically via `SaveChangesAsync` override
- **Anti-forgery**: Header-based CSRF protection (`X-CSRF-TOKEN`) for all state-changing API calls
- **Cookie auth**: `SameSite=Lax`, 60-minute expiry, API routes return `401` instead of redirecting

## Project Structure

```
Controllers/
  ├── Api/V1/
  │   └── EmployeesController.cs    # API: GET, POST employees
  ├── AccountController.cs          # Login/Logout
  ├── EmployeesController.cs        # MVC: serves Create view
  └── HomeController.cs             # MVC: serves Home/Index view
Data/
  ├── ApplicationDbContext.cs        # EF Core context, audit override, soft-delete filters
  └── DbSeeder.cs                   # Seeds admin user
Models/
  ├── Entities/
  │   ├── AuditableEntity.cs        # Base: Id, CreatedAt, ModifiedAt, IsActive
  │   ├── Employee.cs
  │   ├── User.cs
  │   └── Enums.cs                  # UserRole, EmployeeType
  ├── Requests/
  │   ├── CreateEmployeeRequest.cs
  │   ├── LoginRequest.cs
  │   └── PagedRequest.cs
  └── Responses/
      ├── EmployeeResponse.cs
      └── PagedResponse.cs
Services/
  ├── AuthService.cs / IAuthService.cs
  └── EmployeeService.cs / IEmployeeService.cs
Views/
  ├── Account/Login.cshtml
  ├── Employees/Create.cshtml
  ├── Home/Index.cshtml
  └── Shared/_Layout.cshtml
wwwroot/
  ├── css/site.css
  └── js/
      ├── employees-table.js         # Fetches paginated employees for Home table
      └── employee-create.js         # POST new employee via API
```

## Running the Project

```bash
dotnet run --launch-profile https
```

Browse to `https://localhost:7164`.

### Database

SQL Server connection string is in `appsettings.json`. Migrations run automatically on startup.

### Default Admin

Seeded on first run (configurable in `appsettings.json`):

- **Email**: `admin@example.com`
- **Password**: `ChangeMe123!`

 for project overview and @package.json for available npm/pnpm commands for this project.

## Code Style Guidelines
- Use descriptive variable names
- Follow existing patterns in the codebase
- Extract complex conditions into meaningful boolean variables

## Architecture Notes
Add important architectural decisions and patterns here.

## Common Workflows
Document frequently used workflows and commands here.
