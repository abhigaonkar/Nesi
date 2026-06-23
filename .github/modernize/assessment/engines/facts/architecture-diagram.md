## Architecture Diagram

### Summary
This document provides a high-level architecture diagram and supporting notes for the Nesi repository. It highlights the major components (frontend, web, API, BLL, data layer, background services, SignalR, mobile) and shows their responsibilities, data flows, deployment topology, technology stack, key code references, and migration risks.

### Component Diagram

```mermaid
graph LR
  subgraph Clients
    Browser[Browser / Angular Frontend]
    Mobile[Mobile App]
  end

  Browser -->|HTTP/HTTPS| Web[Nesi.Web (IIS) / MVC]
  Browser -->|WS / SignalR| SignalR[SignalR Hub (NESI.SignalR)]
  Mobile -->|HTTP/HTTPS| API[Nesi.WebAPI]
  Mobile -->|WS / SignalR| SignalR

  Web -->|API calls / internal calls| API
  API -->|calls| BLL[NESI.BLL]
  BLL -->|data access| DataLayer[NESI.DataProcessor / Data Layer]
  DataLayer -->|reads/writes| SQL[(On‑Prem SQL Server)]

  SignalR -->|push| Browser
  SignalR -->|push| Mobile

  subgraph Background
    CSV[csvProcessor (batch processor)]
    HeartBeat[Nesi.HeartBeat (Windows Service)]
  end

  CSV -->|writes| DataLayer
  HeartBeat -->|health checks / scheduled jobs| BLL
  CSV -->|reads/writes| SQL

  External[External Integrations (SMTP, File Shares, 3rd‑party APIs)]
  BLL --> External
  API --> External

  Common[NESI.Common]
  BLL -.-> Common
  Web -.-> Common
  API -.-> Common

```

### Component Responsibilities

- **Angular Frontend (CodeBase/angular/)**
  - Render UI and client routing; single page app built with Angular/TypeScript.
  - Call Web/API endpoints, handle authentication and client-side validation.
  - Subscribe to SignalR for real‑time updates.

- **Mobile App (`Nesi.Mobile`)**
  - Native or hybrid mobile client consuming WebAPI and SignalR for realtime features.
  - Handles offline sync / limited local caching (if implemented).

- **Web (Nesi.Web)**
  - Hosts server-rendered pages and serves the Angular app in IIS.
  - Handles session/auth cookie handling, initial MVC endpoints and static assets.

- **Web API (Nesi.WebAPI)**
  - Exposes RESTful endpoints for SPA and mobile clients.
  - AuthN/AuthZ boundary for API consumers and orchestrates calls into BLL.

- **Business Logic Layer (NESI.BLL)**
  - Implements core domain logic, validation, orchestration and transaction boundaries.
  - Calls into repository/data layer and integrates with external services.

- **Common Libraries (NESI.Common)**
  - Shared models, helpers, DTOs, and utilities consumed across projects.

- **Data Layer / Processors (NESI.DataProcessor, NESI.Data, csvProcessor)**
  - Data access (repositories / ADO.NET / EF) and batch processors for ETL (csvProcessor).
  - Map domain entities to persistent storage.

- **Background Services (Nesi.HeartBeat, csvProcessor)**
  - Windows services and scheduled jobs for recurring work (heartbeat, scheduled ETL).

- **SignalR (NESI.SignalR)**
  - Real‑time messaging hub to push notifications and updates to connected clients.

### Main Data Flows

1. User request → Web → API → BLL → Data
   - Browser sends request to `Nesi.Web` (static SPA shell) or directly to `Nesi.WebAPI`.
   - `Nesi.WebAPI` authenticates and forwards to `NESI.BLL` which enforces business rules and persists through the Data Layer to SQL.

2. Scheduled job / Batch ETL → csvProcessor → Data
   - `csvProcessor` reads CSV files (or other sources), transforms and writes results via the Data Layer into the SQL database.

3. Realtime updates → BLL/SignalR → Clients
   - Domain events or BLL-triggered notifications are sent to `NESI.SignalR` which pushes updates to Angular clients and Mobile apps.

### Deployment Topology

```mermaid
graph LR
  subgraph OnPrem[On‑Premises]
    IIS[IIS Web Server(s)]
    WinSvc[Windows Service Host]
    DB[SQL Server]
  end

  subgraph Cloud[Optional / Hybrid]
    AppSvc[Azure App Service / Web App]
    AzureSQL[Azure SQL (optional)]
    Blob[Azure Blob Storage]
  end

  Browser --> IIS
  Mobile --> AppSvc
  IIS --> DB
  WinSvc --> DB
  AppSvc --> AzureSQL
  CSV --> DB

  note right of IIS: Hosts `Nesi.Web`, `Nesi.WebAPI`, `NESI.SignalR` (or
  run SignalR on a dedicated App Service/WebRole)

```

Notes:
- Primary deployment appears to be Windows/IIS and on‑prem SQL Server (Windows Services for background jobs). Cloud options include migrating Web/API/SignalR to Azure App Service and databases to Azure SQL.

### Technology Stack (by component)

- Angular Frontend: Angular, TypeScript, Node/npm, Webpack (CodeBase/angular)
- Web / WebAPI: ASP.NET (likely .NET Framework / MVC / WebAPI), C#, IIS
- Business Logic: C#, NESI.BLL project, shared `NESI.Common` library
- Data Layer / Persistence: ADO.NET / Entity Framework (inferred), SQL Server
- Background Services: Windows Service (`Nesi.HeartBeat`), console/batch `csvProcessor`
- Realtime: SignalR (`NESI.SignalR`)
- Tests: NUnit / MSTest / xUnit (projects with `Tests` suffix present)

### Key file / project references

- [CodeBase/Nesi.Web/](CodeBase/Nesi.Web/)
- [CodeBase/Nesi.WebAPI/](CodeBase/Nesi.WebAPI/)
- [CodeBase/NESI.BLL/](CodeBase/NESI.BLL/)
- [CodeBase/NESI.Common/](CodeBase/NESI.Common/)
- [CodeBase/csvProcessor/Program.cs](CodeBase/csvProcessor/Program.cs)
- [CodeBase/HeartBeat/](CodeBase/HeartBeat/)
- [CodeBase/NESI.SignalR/](CodeBase/NESI.SignalR/)
- [CodeBase/Nesi.Mobile/](CodeBase/Nesi.Mobile/)
- [CodeBase/angular/](CodeBase/angular/)
- [CodeBase/NESI.BLL.Tests/](CodeBase/NESI.BLL.Tests/)

### Risks / Migration Notes

- Tight coupling to Windows/IIS and on‑prem SQL: migrating to Linux‑based hosts or containers will require replacing Windows Services and reworking deployment scripts.
- Unclear data access tech: if ADO.NET or legacy SQL scripts are used heavily, moving to EF/Core or Azure SQL may require schema and data‑access refactors.
- Realtime scaling: `NESI.SignalR` may need an external backplane (Azure SignalR Service or Redis) for scale in cloud deployments.

---

Generated for repository root `/workspaces/Nesi`.
