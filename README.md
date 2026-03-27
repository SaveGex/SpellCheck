# SpellCheck

ASP.NET Core 10 REST API for vocabulary learning. Built with Clean + Layered Architecture across 4 projects, with a full Docker Compose stack and GitHub Actions CI pipeline.

## Features

- **JWT authentication** with refresh token rotation and per-client audience validation
- **Role-based access control** with custom ownership filters (`Admin`, `Manager`, `User`)
- **Keyset pagination** for efficient cursor-based data traversal
- **EF Core soft delete interceptor** - deleted entities are never physically removed
- **Full Docker Compose stack** with MSSQL 2025, custom init scripts, and healthcheck-gated startup
- **GitHub Actions CI pipeline** with separated jobs: `build` -> [`tests`, `docker-compose`] -> `deploy` to `GHCR`

## Architecture

```
SpellCheck/
├── DbManagerApi/       # ASP.NET Core Web API - controllers, filters, DI composition root
├── Application/        # Use cases, service interfaces, DTOs, AutoMapper profiles
├── Infrastructure/     # EF Core, repositories, JWT/token services, file logger
├── DomainData/         # Domain models, repository interfaces, pagination records
└── Tests/              # xUnit unit tests (Moq, FluentAssertions)
```

## Tech Stack

| Area | Technologies |
|------|-------------|
| Runtime | .NET 10, ASP.NET Core 10 |
| Database | MSSQL 2025, EF Core 10 |
| Auth | JWT Bearer, custom refresh token rotation |
| Containerization | Docker, Docker Compose |
| CI/CD | GitHub Actions -> GHCR |
| Testing | xUnit, Moq, FluentAssertions |
| Other | AutoMapper, Scalar (OpenAPI UI), keyset pagination |

## Getting Started

**Prerequisites:** Docker, Docker Compose (Starting containers about 210 seconds)

### BASH
```bash
git clone https://github.com/SaveGex/SpellCheck
cd SpellCheck
cp .env.example .env   # fill in secrets
docker compose up --build
```
### CMD
```cmd
git clone https://github.com/SaveGex/SpellCheck
cd SpellCheck
copy .env.example .env   # fill in secrets
docker compose up --build
```
API will be available at `http://localhost:8080` 

OpenAPI UI (Scalar): `http://localhost:8080/scalar`

## API Overview

| Resource | Endpoints |
|----------|-----------|
| Auth | `POST /api/auth/register`, `POST /api/auth/login`, `POST /api/auth/refresh-token` |
| Modules | `GET/POST /api/modules`, `GET/PUT/DELETE /api/modules/{id}` |
| Words | `GET/POST /api/words`, `GET/PUT/DELETE /api/words/{id}`, `GET /api/module/{id}/words` |
| Users | `GET/PUT/DELETE /api/users/{id}`, role management |
| Friends | `GET/POST /api/friends`, `GET/PUT/DELETE /api/friends/{id}` |
| Roles | `GET/POST/PUT/DELETE /api/roles` (Admin only) |

Pagination on list endpoints uses keyset strategy. Pass `after` cursor and optional `propName`, `limit`, `reverse` query params.

## CI Pipeline

```
push -> build -> [tests, docker-compose integration] -> deploy to GHCR
                        /|\ all jobs must pass before deploy
                         |
```

Tests run against a live SQL Server container spun up in the pipeline. The deploy job pushes a Docker image to GitHub Container Registry on every successful master build.

## Environment Variables

See `.env.example` for all required variables. Key ones:

```
MSSQL_SA_PASSWORD      # SA password for SQL Server
MSSQL_DATABASE         # Database name
MSSQL_USER / MSSQL_USER_PASSWORD  # App user credentials
JWT__SecretKey         # HS256 signing key
APIAdministrator__*    # Seeds an admin user and test client on startup
and so on...
```