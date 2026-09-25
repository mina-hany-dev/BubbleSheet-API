<div align="center">

# 📋 BubbleSheet API

**Backend API powering the BubbleSheet educational assessment platform**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge\&logo=dotnet\&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-ASP.NET_Core-239120?style=for-the-badge\&logo=csharp\&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-EF_Core-CC2927?style=for-the-badge\&logo=microsoftsqlserver\&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![JWT](https://img.shields.io/badge/Auth-JWT-000000?style=for-the-badge\&logo=jsonwebtokens\&logoColor=white)](https://jwt.io/)
[![Swagger](https://img.shields.io/badge/Docs-Swagger-85EA2D?style=for-the-badge\&logo=swagger\&logoColor=black)](https://swagger.io/)

*Helping teachers, schools, and educational centers move assessment workflows into the digital era.*

</div>

---

## 📌 Overview

**BubbleSheet** is an EdTech platform designed to digitize traditional assessment workflows and provide a centralized environment for managing educational assessments.

The platform provides tools for:

* Creating and managing exams and question banks
* Automatically evaluating student submissions
* Tracking student attempts and performance
* Managing educational lessons and PDF resources
* Managing student wallets and recharge codes
* Supporting administrative dashboards and platform operations

This repository contains the **backend API** responsible for the platform's core business logic, authentication, data access, assessment workflows, and external service integrations.

---

## ✨ Features

| Category              | Features                                                        |
| --------------------- | --------------------------------------------------------------- |
| 🔐 **Authentication** | JWT authentication, role-based authorization, password reset    |
| 👥 **Users**          | Student and admin management                                    |
| 📝 **Exams**          | Exam creation and management, random exam generation            |
| 📚 **Question Banks** | Question bank management, questions and multiple-choice answers |
| 📊 **Assessment**     | Student attempts, automatic scoring, performance tracking       |
| ⭐ **Reviews**         | Exam and question-bank reviews                                  |
| 📖 **Content**        | Lesson management, PDF management                               |
| 💳 **Payments**       | Wallet system, recharge codes, transaction tracking             |
| 📢 **Platform**       | Advertisements, academic years, dashboard and statistics        |
| 📧 **Email**          | Transactional email delivery through Brevo                      |
| ☁️ **Storage**        | External file storage through Bunny Storage                     |
| 📘 **Documentation**  | Swagger / OpenAPI                                               |

---

## 🏗️ Architecture

The project follows a **layered architecture inspired by Clean Architecture principles**, with responsibilities separated across API, application services, infrastructure, and domain layers.

```text
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
│  EF Core / Repositories /    │
│      External Services       │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│         Domain Layer         │
│ Entities / Interfaces /      │
│ Enums / Domain Contracts     │
└──────────────────────────────┘
```

### Layers

| Layer              | Responsibility                                                                            |
| ------------------ | ----------------------------------------------------------------------------------------- |
| **API**            | HTTP endpoints, controllers, request handling, authentication, authorization, and Swagger |
| **Services**       | Application business logic and orchestration                                              |
| **Infrastructure** | EF Core, SQL Server, repositories, Unit of Work, migrations, and external services        |
| **Domain**         | Core entities, interfaces, enums, and domain contracts                                    |

---

## 🛠️ Technology Stack

### Core

* **C#**
* **.NET 8**
* **ASP.NET Core Web API**

### Data Access

* **Entity Framework Core**
* **SQL Server**
* Repository Pattern
* Unit of Work Pattern
* EF Core Migrations

### Security

* JWT Bearer Authentication
* BCrypt password hashing
* Role-based authorization
* `Admin` and `Student` roles

### External Services

* **Bunny Storage** — file and media storage
* **Brevo** — transactional email delivery

### API Documentation

* **Swagger / OpenAPI**

---

## 📁 Project Structure

```text
BubbleSheet/
│
├── BubleSheet/                     # API Layer
│   ├── Controllers/                # HTTP endpoints
│   ├── Services/
│   │   ├── Implementation/         # Service implementations
│   │   ├── Interfaces/             # Service contracts
│   │   └── Models/                 # Service-related models
│   ├── Program.cs
│   ├── appsettings.json
│   └── Properties/
│
├── Domain.bublesheet/              # Domain Layer
│   ├── Entities/                   # Core domain entities
│   ├── Enums/                      # Domain enums
│   └── Interfaces/                 # Domain contracts
│
├── bubblesheet.Infrastracture/     # Infrastructure Layer
│   ├── Data/                       # EF Core DbContext
│   ├── Dtos/                       # Data Transfer Objects
│   ├── Migrations/                 # EF Core migrations
│   └── Repos/                      # Repository implementations
│
└── BubleSheet.sln
```

---

## 🔐 Authentication & Authorization

The API uses **JWT Bearer Authentication** to secure protected endpoints.

The general authentication flow is:

```text
Client
  │
  │ Authentication Request
  ▼
Authentication Service
  │
  ├── Validate credentials
  ├── Verify password
  ├── Generate JWT
  │
  ▼
Access Token
  │
  ▼
Authenticated API Requests
  │
  └── Role-based authorization
```

Protected endpoints require a valid Bearer token:

```http
Authorization: Bearer <access-token>
```

Authorization is applied according to the authenticated user's role, including:

* `Admin`
* `Student`

---

## ⚙️ Configuration

The application requires environment-specific configuration for database access, authentication, storage, and email services.

Example configuration:

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
    "ZoneName": "<zone name>",
    "AccessKey": "<access key>",
    "Region": "<region>",
    "BaseUrl": "<base URL>",
    "UrlTokenAuthenticationKey": "<token key>"
  },
  "BrevoSettings": {
    "ApiKey": "<API key>",
    "SenderEmail": "<sender email>",
    "SenderName": "<sender name>"
  }
}
```

> ⚠️ **Never commit real credentials, API keys, connection strings, or other secrets to source control.**

---

## 🚀 Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* SQL Server
* Configured application settings
* Required Bunny Storage credentials
* Required Brevo credentials

### Restore Dependencies

```bash
dotnet restore
```

### Build

```bash
dotnet build
```

### Apply Database Migrations

```bash
dotnet ef database update \
  --project bubblesheet.Infrastracture \
  --startup-project BubleSheet
```

### Run the API

```bash
dotnet run --project BubleSheet
```

Once the application is running, open the configured Swagger endpoint to explore and test the API.

---

## 🗄️ Database Migrations

The project uses **Entity Framework Core Migrations** to manage database schema changes.

### Create a Migration

```bash
dotnet ef migrations add <MigrationName> \
  --project bubblesheet.Infrastracture \
  --startup-project BubleSheet
```

### Apply Migrations

```bash
dotnet ef database update \
  --project bubblesheet.Infrastracture \
  --startup-project BubleSheet
```

---

## 🧩 Core Modules

<details>
<summary><strong>Authentication & Accounts</strong></summary>

Handles student and admin authentication, JWT generation, role-based authorization, and password reset workflows.

</details>

<details>
<summary><strong>Exams</strong></summary>

Provides the complete exam workflow, including exam creation, question management, configuration, student attempts, automatic evaluation, and score calculation.

</details>

<details>
<summary><strong>Question Banks</strong></summary>

Provides reusable question collections independent from individual exams, including question management, multiple-choice answers, submissions, attempts, and scoring.

</details>

<details>
<summary><strong>Random Exams</strong></summary>

Supports dynamically generated exams based on configured question-selection rules.

</details>

<details>
<summary><strong>Student Attempts & Scoring</strong></summary>

Tracks student attempts, submitted answers, evaluation results, scores, and assessment history.

</details>

<details>
<summary><strong>Lessons & Educational Content</strong></summary>

Provides management functionality for educational lessons and associated learning resources.

</details>

<details>
<summary><strong>PDF Management</strong></summary>

Handles educational PDF resources and integrates with external storage infrastructure.

</details>

<details>
<summary><strong>Wallet & Recharge Codes</strong></summary>

Provides wallet operations, recharge codes, and transaction tracking for student accounts.

</details>

<details>
<summary><strong>Dashboard & Statistics</strong></summary>

Provides administrative statistics and platform-level information.

</details>

---

## 📐 Engineering Practices

* Separation of Concerns
* Layered Architecture inspired by Clean Architecture
* Dependency Injection
* Repository Pattern
* Unit of Work Pattern
* Service Layer abstraction
* DTO-based API contracts
* JWT Authentication
* Role-based Authorization
* EF Core Migrations
* External service abstraction
* Asynchronous database operations
* Centralized configuration

---

## 📜 Repository & Usage Policy

This repository is **publicly visible for portfolio and project demonstration purposes**.

The source code remains **proprietary** and is not released under an open-source license.

The code may not be:

* Cloned or redistributed
* Reused in other projects
* Published in modified or unmodified form
* Used commercially
* Presented as someone else's original work

No permission to copy, modify, distribute, or reuse the source code is granted by this repository.

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
