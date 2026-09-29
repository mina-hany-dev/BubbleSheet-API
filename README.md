<div align="center">

# BubbleSheet API

**Backend API powering the BubbleSheet educational assessment platform**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge\&logo=dotnet\&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-ASP.NET_Core-239120?style=for-the-badge\&logo=csharp\&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-EF_Core-CC2927?style=for-the-badge\&logo=microsoftsqlserver\&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![JWT](https://img.shields.io/badge/Auth-JWT-000000?style=for-the-badge\&logo=jsonwebtokens\&logoColor=white)](https://jwt.io/)
[![Swagger](https://img.shields.io/badge/Docs-Swagger-85EA2D?style=for-the-badge\&logo=swagger\&logoColor=black)](https://swagger.io/)

*Helping teachers, schools, and educational centers move assessment workflows into the digital era.*

</div>

---

## Overview

**BubbleSheet** is an EdTech platform designed to digitize traditional assessment workflows and provide a centralized environment for managing educational assessments.

The platform provides tools for:

* Creating and managing exams and question banks
* Automatically evaluating student submissions
* Tracking student attempts and performance
* Managing educational lessons and PDF resources
* Managing student wallets and recharge codes
* Supporting administrative dashboards and platform operations
* Delivering in-app notifications to students

This repository contains the **backend API** responsible for the platform's core business logic, authentication, data access, assessment workflows, background processing, notification distribution, and external service integrations.

---

## Features

| Category                 | Features                                                                            |
| ------------------------ | ----------------------------------------------------------------------------------- |
| **Authentication**       | JWT authentication, role-based authorization, password reset                        |
| **Users**                | Student and admin management                                                        |
| **Exams**                | Exam creation and management, random exam generation                                |
| **Question Banks**       | Question bank management, questions and multiple-choice answers                     |
| **Assessment**           | Student attempts, automatic scoring, performance tracking                           |
| **Notifications**        | In-app notifications, per-student read status, asynchronous background distribution |
| **Notification Cleanup** | Automatic cleanup of notifications older than 30 days                               |
| **Reviews**              | Exam and question-bank reviews                                                      |
| **Content**              | Lesson management, PDF management                                                   |
| **Payments**             | Wallet system, recharge codes, transaction tracking                                 |
| **Platform**             | Advertisements, academic years, dashboard and statistics                            |
| **Email**                | Transactional email delivery through Brevo                                          |
| **Storage**              | External file storage through Bunny Storage                                         |
| **Documentation**        | Swagger / OpenAPI                                                                   |

---

# Architecture

BubbleSheet follows a **layered architecture inspired by Clean Architecture principles**, with responsibilities separated across three main projects:

```text
┌──────────────────────────────────────────────┐
│                  Presentation                │
│                                              │
│ Controllers / HTTP / Authentication         │
│ Services / Service Interfaces                │
│ Background Workers / Notification Queue      │
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│                Infrastructure                │
│                                              │
│ EF Core / SQL Server                         │
│ DbContext / Repositories / Unit of Work      │
│ External Services                            │
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│                    Domain                    │
│                                              │
│ Entities / Enums / Domain Interfaces         │
│ Core Business Contracts                      │
└──────────────────────────────────────────────┘
```

## Layers

| Layer              | Responsibility                                                                                                                   |
| ------------------ | -------------------------------------------------------------------------------------------------------------------------------- |
| **Presentation**   | HTTP endpoints, controllers, request handling, authentication, authorization, services, background processing, and API contracts |
| **Infrastructure** | EF Core, SQL Server, DbContext, repositories, Unit of Work, migrations, and external service integrations                        |
| **Domain**         | Core entities, enums, repository contracts, and domain-level abstractions                                                        |

## Dependency Direction

```text
Presentation
     │
     ├──────────────► Infrastructure
     │
     └──────────────► Domain

Infrastructure
     │
     └──────────────► Domain
```

The **Domain** layer remains independent from both Presentation and Infrastructure.

---

# Technology Stack

## Core

* **C#**
* **.NET 8**
* **ASP.NET Core Web API**

## Data Access

* **Entity Framework Core**
* **SQL Server**
* Repository Pattern
* Unit of Work Pattern
* EF Core Migrations

## Security

* JWT Bearer Authentication
* BCrypt password hashing
* Role-based authorization
* `Admin` and `Student` roles

## Background Processing

* ASP.NET Core `BackgroundService`
* `System.Threading.Channels`
* `PeriodicTimer`
* Asynchronous background notification processing
* Scheduled data cleanup

## External Services

* **Bunny Storage** — file and media storage
* **Brevo** — transactional email delivery

## API Documentation

* **Swagger / OpenAPI**

---

# Project Structure

```text
BubbleSheet/
│
├── BubleSheet/                         # Presentation / API Layer
│   ├── Controllers/                   # HTTP endpoints
│   │
│   ├── Services/
│   │   ├── Implementation/            # Service implementations
│   │   ├── Interfaces/                # Service contracts
│   │   └── Models/                    # Service-related models
│   │
│   ├── Worker/                        # Background workers
│   │   ├── NotificationWorker.cs      # Notification distribution
│   │   └── CleanupWorker.cs           # Scheduled data cleanup
│   │
│   ├── Program.cs
│   ├── appsettings.json
│   └── Properties/
│
├── Domain.bublesheet/                 # Domain Layer
│   ├── Entities/                      # Core domain entities
│   │   ├── Notification.cs
│   │   └── StudentNotification.cs
│   │
│   ├── Enums/                         # Domain enums
│   │   └── NotificationType.cs
│   │
│   └── Interfaces/                    # Repository/domain contracts
│
├── bubblesheet.Infrastracture/        # Infrastructure Layer
│   ├── Data/                          # EF Core DbContext
│   ├── Dtos/                          # Data Transfer Objects
│   ├── Migrations/                    # EF Core migrations
│   └── Repos/                         # Repository implementations
│
└── BubleSheet.sln
```

---

# Authentication & Authorization

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

# Notification System

BubbleSheet provides an **in-app notification system** for delivering platform events to students.

Notifications are separated from their student-specific state.

```text
Notification
     │
     │ 1
     ▼
StudentNotification
     │
     │ N
     ▼
Student
```

## Notification

The `Notification` entity represents the notification itself.

It contains:

* Notification type
* Optional reference ID for the related resource
* Creation timestamp

Supported notification types include:

```text
NewExam
NewQuestionBank
NewLesson
NewAd
```

The optional `ReferenceId` can point to the related resource, such as:

```text
ExamId
QuestionBankId
LessonId
AdId
```

## StudentNotification

`StudentNotification` represents the relationship between a notification and a student.

It allows the system to track:

* Which student received the notification
* Whether the student has read the notification
* The relationship between the student and notification

This design allows a single notification to be distributed to multiple students while maintaining an independent read status for each student.

For example:

```text
Notification #15
       │
       ├── Student A → IsRead = true
       ├── Student B → IsRead = false
       └── Student C → IsRead = true
```

The read state belongs to `StudentNotification`, not to `Notification`, because each student can have a different read state for the same notification.

---

# Background Notification Processing

When a notification needs to be delivered to **all students**, the API does not create every `StudentNotification` record during the original HTTP request.

Instead, notification distribution is handled asynchronously by a background worker.

```text
Client
   │
   │ Create Notification
   ▼
NotificationService
   │
   ├── Create Notification
   ├── Save Notification
   │
   └── Enqueue NotificationId
              │
              ▼
       Notification Queue
              │
              ▼
      NotificationWorker
              │
              ├── Dequeue NotificationId
              │
              ├── Get Students
              │
              ├── Create StudentNotification records
              │
              └── Save Changes
```

This keeps the original HTTP request independent from the potentially expensive distribution process.

---

## Notification Queue

The notification queue uses `System.Threading.Channels`.

The queue stores only the `NotificationId` because the current notification model distributes notifications to **all students**.

```text
NotificationService
        │
        │ EnqueueAsync(notificationId)
        ▼
      Channel
        │
        │ DequeueAsync()
        ▼
NotificationWorker
```

The queue provides an asynchronous boundary between the HTTP request and notification distribution.

The worker does **not continuously poll the queue**.

When the queue is empty, the worker asynchronously waits for an item:

```text
NotificationWorker
       │
       ▼
  DequeueAsync()
       │
       │ Waiting
       │
       ▼
Notification Enqueued
       │
       ▼
Worker Resumes
```

This allows the worker to remain idle while there is no notification work to process.

---

## Notification Worker

The `NotificationWorker` is implemented using ASP.NET Core's `BackgroundService`.

Its responsibilities are:

1. Wait for notification IDs in the queue.
2. Retrieve the students who should receive the notification.
3. Create `StudentNotification` records.
4. Add the records in bulk.
5. Commit the changes through the Unit of Work.

The worker runs independently from incoming HTTP requests.

### Processing Flow

```text
Create Notification
        │
        ▼
   Save Notification
        │
        ▼
 Enqueue NotificationId
        │
        ▼
 Notification Queue
        │
        ▼
 NotificationWorker
        │
        ▼
    Get Students
        │
        ▼
Create StudentNotification
        │
        ▼
      AddRange
        │
        ▼
UnitOfWork.SaveChanges()
```

If multiple notifications are queued, the worker processes them as queue items become available.

For very large student populations, notification distribution can be processed in batches to reduce memory usage and database pressure.

---

# Notification Cleanup

BubbleSheet also uses a separate background worker for **scheduled notification cleanup**.

The cleanup process is intentionally separated from `NotificationWorker` to maintain a clear separation of responsibilities.

```text
NotificationWorker
    │
    └── Notification Distribution


CleanupWorker
    │
    └── Scheduled Data Cleanup
```

## Cleanup Worker

The `CleanupWorker` runs independently using a `PeriodicTimer`.

Its responsibility is to periodically remove old notifications.

The cleanup process targets notifications that are older than **30 days**.

```text
CleanupWorker
      │
      ▼
PeriodicTimer
      │
      │ Scheduled interval
      ▼
Find Notifications
older than 30 days
      │
      ▼
Delete Notifications
      │
      ▼
Cascade Delete
      │
      ▼
StudentNotifications
```

## Cascade Delete

`StudentNotification` is dependent on `Notification`.

The relationship is configured with:

```csharp
.OnDelete(DeleteBehavior.Cascade)
```

Therefore, when an old `Notification` is deleted, its related `StudentNotification` records are automatically deleted by the database.

```text
Notification
     │
     ├── StudentNotification
     ├── StudentNotification
     └── StudentNotification

Delete Notification
        │
        ▼
Cascade Delete
        │
        ▼
Delete related StudentNotifications
```

This keeps notification cleanup centralized around the parent `Notification` entity instead of manually deleting child records first.

---

# Configuration

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

**Do not commit real secrets or credentials to the repository.**

---

# Getting Started

## Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* SQL Server
* Configured application settings
* Required Bunny Storage credentials
* Required Brevo credentials

## Restore Dependencies

```bash
dotnet restore
```

## Build

```bash
dotnet build
```

## Apply Database Migrations

```bash
dotnet ef database update \
  --project bubblesheet.Infrastracture \
  --startup-project BubleSheet
```

## Run the API

```bash
dotnet run --project BubleSheet
```

Once the application is running, open the configured Swagger endpoint to explore and test the API.

---

# Database Migrations

The project uses **Entity Framework Core Migrations** to manage database schema changes.

## Create a Migration

```bash
dotnet ef migrations add <MigrationName> \
  --project bubblesheet.Infrastracture \
  --startup-project BubleSheet
```

## Apply Migrations

```bash
dotnet ef database update \
  --project bubblesheet.Infrastracture \
  --startup-project BubleSheet
```

---

# Core Modules

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
<summary><strong>Notifications</strong></summary>

Provides in-app notifications for students, per-student read tracking, notification types, asynchronous background distribution through a notification queue, and scheduled cleanup of notifications older than 30 days.

</details>

<details>
<summary><strong>Dashboard & Statistics</strong></summary>

Provides administrative statistics and platform-level information.

</details>

---

# Engineering Practices

The project applies the following engineering practices:

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
* Background processing with `BackgroundService`
* In-process asynchronous queue using `System.Threading.Channels`
* Scheduled background processing with `PeriodicTimer`
* Separation of background worker responsibilities
* Cascade delete for dependent notification records
* Centralized configuration
* Bulk database operations where appropriate

---

# Repository & Usage Policy

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

# Author

<div align="center">

**Mina Hany**

*Backend .NET Developer — System Design & Software Architecture*

[![Portfolio](https://img.shields.io/badge/Portfolio-minahanydev.netlify.app-blue?style=flat-square)](https://minahanydev.netlify.app)

</div>

---

<div align="center">

> **BubbleSheet API** — Backend infrastructure for a modern digital assessment platform.

</div>
