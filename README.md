<div align="center">

# 📋 BubbleSheet API

**Backend API powering the BubbleSheet educational assessment platform**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-ASP.NET_Core-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-EF_Core-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![JWT](https://img.shields.io/badge/Auth-JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)](https://jwt.io/)
[![Swagger](https://img.shields.io/badge/Docs-Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)](https://swagger.io/)

*Helping teachers, schools, and educational centers move assessment workflows into the digital era.*

</div>

---

## 📌 Overview

**BubbleSheet** is an EdTech platform that digitizes traditional assessment workflows. It provides a comprehensive suite of tools for:

- Creating and managing exams and question banks
- Automatically evaluating student answers
- Tracking student performance over time
- Managing educational content and PDFs
- Handling payments through a wallet and recharge-code system

This repository contains the **backend API** responsible for the core business logic, authentication, data management, assessment workflows, and external service integrations.

---

## ✨ Features

| Category | Features |
|---|---|
| 🔐 **Auth** | JWT authentication, role-based authorization, password reset |
| 👥 **Users** | Student management, admin management |
| 📝 **Exams** | Exam creation & management, random exam generation |
| 📚 **Question Banks** | Question bank management, multiple-choice answers |
| 📊 **Assessment** | Student attempts, automatic scoring, performance tracking |
| ⭐ **Reviews** | Exam and question-bank reviews |
| 📖 **Content** | Lesson management, PDF file management |
| 💳 **Payments** | Wallet system, recharge codes |
| 📢 **Platform** | Advertisements, academic year management, dashboard & statistics |
| 📧 **Notifications** | Email delivery via Brevo |
| ☁️ **Storage** | External file storage via Bunny Storage |
| 📘 **Docs** | Swagger / OpenAPI documentation |

---

## 🏗️ Architecture

The project follows a **Clean Architecture** inspired layered approach:

```
┌──────────────────────────────┐
│          API Layer           │
│   Controllers / HTTP / Auth  │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│        Service Layer         │
│       Business Logic         │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│     Infrastructure Layer     │
│  EF Core / Repos / External  │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│         Domain Layer         │
│  Entities / Interfaces /     │
│  Enums / Domain Contracts    │
└──────────────────────────────┘
```

### Layers

| Layer | Responsibility |
|---|---|
| **API** | HTTP endpoints, controllers, request handling, auth, Swagger |
| **Services** | Business logic, orchestration between layers |
| **Infrastructure** | EF Core, SQL Server, repositories, Unit of Work, external services |
| **Domain** | Core entities, interfaces, enums, domain contracts |

---

## 🛠️ Technology Stack

### Core
- **C# / .NET 8** — ASP.NET Core Web API

### Data Access
- **Entity Framework Core** with SQL Server
- Repository Pattern + Unit of Work Pattern
- EF Core Migrations

### Security
- JWT Bearer Authentication
- BCrypt password hashing
- Role-based authorization (`Admin`, `Student`)

### External Services
- **[Bunny Storage](https://bunny.net/)** — file & media storage
- **[Brevo](https://www.brevo.com/)** — transactional email delivery

### Documentation
- **Swagger / OpenAPI**

---

## 📁 Project Structure

```
BubbleSheet/
│
├── BubleSheet/                     # API Layer
│   ├── Controllers/                # HTTP endpoints
│   ├── Services/
│   │   ├── Implementation/         # Service implementations
│   │   ├── Interfaces/             # Service contracts
│   │   └── Models/                 # Request/response models
│   ├── Program.cs
│   └── appsettings.json
│
├── Domain.bublesheet/              # Domain Layer
│   ├── Entities/                   # Core domain entities
│   ├── Enums/
│   └── Interfaces/                 # Domain contracts
│
├── bubblesheet.Infrastracture/     # Infrastructure Layer
│   ├── Data/                       # DbContext
│   ├── Dtos/                       # Data transfer objects
│   ├── Migrations/                 # EF Core migrations
│   └── Repos/                      # Repository implementations
│
└── BubleSheet.sln
```

---

## 🔐 Authentication Flow

```
Client
  │
  │  POST /auth/login
  ▼
Authentication Endpoint
  │
  ├── Validate credentials (BCrypt)
  ├── Generate JWT token
  │
  ▼
Access Token returned to client
  │
  ▼
Bearer token used on protected endpoints
```

All protected endpoints require a valid `Authorization: Bearer <token>` header. Access is scoped by user role (`Admin` or `Student`).

---

## ⚙️ Configuration

The application requires the following configuration (do **not** commit real credentials):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<SQL Server connection string>"
  },
  "Jwt": {
    "Key": "<secret key>",
    "Issuer": "<issuer>",
    "Audience": "<audience>"
  },
  "BunnyStorage": {
    "ZoneName": "",
    "AccessKey": "",
    "Region": "",
    "BaseUrl": "",
    "UrlTokenAuthenticationKey": ""
  },
  "BrevoSettings": {
    "ApiKey": "",
    "SenderEmail": "",
    "SenderName": ""
  }
}
```

> ⚠️ **Never commit real secrets, API keys, or connection strings to source control.**

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server instance
- Configured `appsettings.json` (see Configuration section)
- External service credentials (Bunny Storage, Brevo)

### Run Locally

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Apply database migrations
dotnet ef database update --project bubblesheet.Infrastracture --startup-project BubleSheet

# Run the API
dotnet run --project BubleSheet
```

Once running, navigate to the Swagger UI (typically `https://localhost:<port>/swagger`) to explore and test the API.

### Database Migrations

```bash
# Add a new migration
dotnet ef migrations add <MigrationName> --project bubblesheet.Infrastracture --startup-project BubleSheet

# Apply migrations
dotnet ef database update --project bubblesheet.Infrastracture --startup-project BubleSheet
```

---

## 🧩 Core Modules

<details>
<summary><strong>Auth & Accounts</strong></summary>

Handles student and admin authentication, JWT generation, role-based authorization, and password reset flows.
</details>

<details>
<summary><strong>Exams</strong></summary>

Full exam lifecycle: creation, question management, configuration, student attempts, automatic evaluation, and score calculation.
</details>

<details>
<summary><strong>Question Banks</strong></summary>

Organize and reuse questions independently from individual exams. Supports question management, multiple-choice answers, attempt tracking, and scoring.
</details>

<details>
<summary><strong>Random Exams</strong></summary>

Dynamically generate exams based on configurable question-selection rules.
</details>

<details>
<summary><strong>Student Attempts & Scoring</strong></summary>

Records attempts, stores answers, evaluates submissions, calculates scores, and tracks performance history.
</details>

<details>
<summary><strong>Lessons & Educational Content</strong></summary>

Management of educational lessons and associated learning resources.
</details>

<details>
<summary><strong>PDF Management</strong></summary>

Upload, manage, and serve educational PDFs via Bunny Storage integration.
</details>

<details>
<summary><strong>Wallet & Recharge Codes</strong></summary>

A wallet-based system for students to use recharge codes and manage transactions.
</details>

<details>
<summary><strong>Dashboard & Statistics</strong></summary>

Platform-wide administrative statistics and analytics.
</details>

---

## 📐 Engineering Practices

- ✅ Clean / Layered Architecture
- ✅ Separation of Concerns
- ✅ Dependency Injection
- ✅ Repository Pattern + Unit of Work
- ✅ Service Layer abstraction
- ✅ DTO-based API contracts
- ✅ JWT Authentication + Role-based Authorization
- ✅ EF Core Migrations
- ✅ External service abstraction
- ✅ Async/await throughout
- ✅ Centralized configuration

---

## 📜 License

This repository is a **private/proprietary project showcase**. The source code is **not open source**.

**You may not:**
- Clone or redistribute this project
- Reuse the source code in another project
- Publish modified versions
- Use the source code commercially
- Claim the source code as your own

No open-source license is granted by this repository.

---

## 👨‍💻 Author

<div align="center">

**Mina Hany**

*Backend .NET Developer — System Design & Software Architecture*

[![Portfolio](https://img.shields.io/badge/Portfolio-minahanydev.netlify.app-blue?style=flat-square)](https://minahanydev.netlify.app)

</div>

---

<div align="center">

> **BubbleSheet API** — Backend infrastructure for a modern digital assessment platform.

</div>
