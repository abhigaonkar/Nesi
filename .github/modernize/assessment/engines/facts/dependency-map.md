# Dependency Map — Nesi Repository

Generated: 2026-06-23
Workspace: /workspaces/Nesi

## Summary
- Projects scanned: ~18 .NET project files (.csproj)
- package manifests scanned: 13 `packages.config`, 1 `package.json` (Angular frontend)
- Estimated unique external packages: ~90 (NuGet + npm)

This document summarizes major components, top-level dependencies, and an architecture-level dependency diagram suitable for inclusion in the assessment facts folder.

## Major Components
- Backend API: `Nesi.WebAPI`, `Nesi.Web` (ASP.NET / WebAPI)
- Business Logic: `NESI.BLL`, `NESI.Core`
- Common Libraries: `NESI.Common`, shared DTOs and helpers
- Data layer: Entity Framework (EF6) + MySQL connector packages
- Background Services: `HeartBeat` Windows Service
- Tools / Processors: `csvProcessor` and other small console apps
- Frontend: `angular/` (Angular 6 application, package.json present)
- Tests: `NESI.BLL.Tests`, `NESI.Common.Tests`, `NESI.WebAPI.Tests`

## Category-Level Dependencies (representative)
- Web / API: ASP.NET MVC 5.x, ASP.NET WebAPI 5.x, Microsoft.Owin, Katana
- Real-time / Messaging: SignalR 2.x
- Data / ORM: Entity Framework 6.x, MySQL.Data / MySqlConnector (older versions)
- DI / IoC: Ninject 3.x
- Authentication / Security: JWT/OAuth libraries (older nuget packages)
- Logging & Telemetry: Application Insights 2.x, Log4Net (or similar)
- PDF / Document Tools: iTextSharp, PDFsharp, third-party converters
- Frontend: Angular 6, RxJS 6, @ngrx (v5), PrimeNG (older)
- Testing: MSTest, xUnit 2.x, Moq 4.x

> Note: Many of the listed packages are legacy/EOL versions based on package manifests found in the repo. Upgrades will be required for long-term maintenance and security.

## High-level Dependency Diagram
The diagram below shows the main component boundaries and typical dependency flows (Frontend → API → BLL → Data). Use a Mermaid renderer to visualize.

```mermaid
flowchart LR
  subgraph Frontend
    A[Angular App]\n(angular/)
  end

  subgraph Server
    B[Nesi.Web / WebAPI]\n    C[NESI.BLL]\n    D[NESI.Common]
    E[NESI.Core]
    F[Background Services]\n  end

  subgraph Data
    G[EntityFramework 6]
    H[MySQL / SQL Server]
  end

  A -->|HTTP / REST| B
  B -->|Calls| C
  C -->|Utilizes| D
  C -->|Persists via| G
  G --> H
  F --> C
  B -->|SignalR| I[SignalR Hub]
  I --> A
```

## Per-repository / Project Dependency Notes
- `NESI.WebAPI` / `NESI.Web`: Classic ASP.NET / WebAPI stack relying on OWIN/Katana and older System.Web hosting model.
- `NESI.BLL`: Contains business-layer code with references to `NESI.Common` and database access helpers.
- `NESI.Common`: Shared DTOs, utilities, and template engines used across server projects.
- `HeartBeat` service: Windows Service project using `ServiceProcessInstaller` patterns; depends on core libraries and uses configuration-based scheduling/job execution.
- `csvProcessor`: Console app for batch CSV processing; references file-format libraries and internal BLL/common packages.
- `angular/`: Angular 6 application with `package.json`. Frontend packages are out-of-date relative to modern Angular versions.

## Critical Findings & Risks
- .NET Framework baseline: The codebase targets the .NET Framework family (legacy). Running on .NET Framework 4.5.2 (or similar) introduces security and support risks; Microsoft ended mainstream support for these older runtimes.
- Many NuGet packages are EOL: Observed versions indicate widespread out-of-support packages (EF6, SignalR 2.x, older security packages).
- Frontend tech mismatch: Angular 6 is several major versions behind current LTS (Angular 17+ as of 2026). Upgrading safely requires significant migration steps (RxJS, NgModule changes, zone.js, TypeScript upgrades).
- Transitive dependency exposure: Some packages pulled older transitive dependencies that may contain unpatched CVEs.
- Build / CI risk: No modern global `Directory.Packages.props` or centralized package management; upgrades will be manual per project unless consolidated.

## Recommended Remediation / Modernization Steps
1. Inventory: Produce an exact package inventory (NuGet & npm) with versions from each `packages.config` and `package.json` (automatable by a script).
2. Security triage: Run an automated CVE scan against identified packages and prioritize critical/high vulnerabilities.
3. Migration plan:
   - Short term: Patch immediate critical CVEs, move to supported runtime where possible, update high-risk packages.
   - Mid term: Migrate server code to .NET 6/8 (or .NET 7 LTS/8 LTS) — this will require code changes for System.Web removal, DI updates, and potential library replacements.
   - Frontend: Incrementally upgrade Angular (6 → 7 → 8 ... using the Angular update guide), or consider a full rewrite if technical debt is too high.
4. Consolidation: Consider central package management for NuGet and move to `PackageReference` to simplify per-project updates.
5. CI/CD: Add automated dependency scanning and upgrade bots (dependabot or similar) into CI pipeline.

## Quick Facts & Counts (approx)
- .csproj files scanned: 18
- `packages.config` files: 13
- npm `package.json`: 1
- Estimated unique external packages: ~90

## Next Actions for the Assessment Team
- If desired, I can produce a machine-readable CSV of packages and versions from all `packages.config` and `package.json` files (useful for vulnerability scanners).
- I can also export a longer Mermaid diagram with grouping by solution and per-project edges.

## Warnings
- This dependency map is generated by static analysis of repository manifests and project files. It may not capture runtime-loaded packages or private feeds.
- Confirm exact target .NET Framework versions per `.csproj` for precise upgrade paths.

---

Generated by the dependency-map assessment for inclusion in the assessment facts directory.
