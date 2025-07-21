# HCM – Human Capital Management Platform

[![.NET](https://img.shields.io/badge/.NET-9.0-blue?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-20-red?logo=angular)](https://angular.io/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-blue?logo=postgresql)](https://www.postgresql.org/)
[![Prometheus](https://img.shields.io/badge/Prometheus-Metrics-orange?logo=prometheus)](https://prometheus.io/)
[![Grafana](https://img.shields.io/badge/Grafana-Dashboards-orange?logo=grafana)](https://grafana.com/)

---

## 🚀 Overview

**HCM** is a modern, dockerized HR management system built with **ASP.NET Core** (FastEndpoints), **Angular**, **PostgreSQL**, and monitored via **Prometheus** and **Grafana**.  
It demonstrates backend and frontend architecture, vertical slice design, JWT authentication, custom metrics, and ready-to-use dashboards.

---

## 📦 Features

- User management (CRUD, roles: HrAdmin, Manager, Employee)
- Authentication with JWT
- Role-based authorization
- PostgreSQL as persistent storage
- Entity Framework Core with code-first migrations and seeding (20+ real sample users)
- Prometheus metrics (requests, custom login counter)
- Grafana dashboards (auto-provisioned)
- Docker Compose for full stack orchestration
- RESTful API (with OpenAPI/Swagger UI)
- Angular frontend

---

## 🏗️ Architecture

- **Backend**: ASP.NET Core 9 (FastEndpoints, Entity Framework Core)
- **Frontend**: Angular 20
- **Database**: PostgreSQL
- **Metrics/Monitoring**: Prometheus + Grafana (via `/metrics` endpoint)
- **Testing**: xUnit, Moq, InMemory EF for tests
- **Docker**: Multi-service orchestration

---

## 🛠️ Getting Started

### 1. Clone the repo

```bash
git clone https://github.com/YOUR_USERNAME/HCM.git
cd HCM
```
### 2. Build and run everything

```bash
docker-compose up --build
```

- The first run will apply EF Core migrations and seed the database with real users.
- Prometheus and Grafana will start automatically with provisioned dashboards.

### 3. Access the system

| Service      | URL                              | Default login            |
|--------------|----------------------------------|--------------------------|
| Backend API  | http://localhost:5221/swagger    | (see seeded users)  |
| Frontend     | http://localhost:4200            |                          |
| Grafana      | http://localhost:3000            | admin / admin            |
| Prometheus   | http://localhost:9090            | (no auth)                |
| Metrics      | http://localhost:5221/metrics    | (Prometheus scrapes this) |

## 🧪 Running Tests

Use your IDE’s test runner.

---

## ⚙️ Migrations & Seeding

- Migrations are applied **automatically** at container startup.
- The database is **seeded with 20 real users** across roles and departments.  
  (See `SeedDb.cs` for details.)

---

## Notes from me

This week turned out to be quite intense for me, and I was only able to dedicate a couple of days to working on the project. Unfortunately, there are still several minor bugs on the frontend that I didn’t have time to resolve. I also wasn’t able to fully implement role and department separation in the database.

I’m fully aware that relying on string representations for such data is not ideal, and I hope that my overall effort might still earn a few bonus points despite this. I sincerely hope that the architectural choices I made on the backend will be seen positively and won’t detract from the overall impression.

I also hope I haven’t overlooked anything important, and I’d be glad to attend the final interview and answer any questions you may have.