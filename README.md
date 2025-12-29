# 🛒 .NET 8 E-Commerce Microservices Reference Architecture

A **production-grade E-Commerce Microservices system** built using **.NET 8 and C# 12**, demonstrating **Domain-Driven Design (DDD)**, **CQRS**, **Vertical Slice Architecture**, **Clean Architecture**, and **event-driven microservices communication**.

This repository serves as a **reference implementation** for building scalable, maintainable, and modern microservices using the latest .NET ecosystem.

---

## 🚀 Key Highlights

- Built with **.NET 8 & C# 12**
- Microservices-based architecture
- **DDD, CQRS, Vertical Slice & Clean Architecture**
- **Event-driven communication** using RabbitMQ & MassTransit
- **High-performance gRPC** communication
- **YARP API Gateway**
- **Polyglot persistence** (PostgreSQL, SQL Server, SQLite, Redis)
- Fully **Dockerized** setup with Docker Compose
- Enterprise-ready patterns & best practices

---

## 🧱 System Architecture

The solution is composed of the following microservices:

| Microservice | Responsibility |
|--------------|----------------|
| Catalog | Product catalog management |
| Basket | Shopping cart & pricing |
| Discount | Discount calculation |
| Ordering | Order processing |
| Yarp API Gateway | Centralized routing & rate limiting |
| Shopping Web | Client-facing UI |

### Communication
- **Synchronous:** gRPC (Basket ↔ Discount)
- **Asynchronous:** RabbitMQ + MassTransit (Basket → Ordering)
- **Client Access:** YARP API Gateway

---

## 📦 Catalog Microservice

**Purpose:** Manage product catalog data.

**Features**
- ASP.NET Core **Minimal APIs**
- **Vertical Slice Architecture** (feature-based)
- **CQRS** using MediatR
- Validation pipeline with FluentValidation
- **Marten** as Transactional Document DB on PostgreSQL
- **Carter** for endpoint definition
- Cross-cutting concerns:
  - Logging
  - Global exception handling
  - Health checks
- Dockerized with PostgreSQL

**Tech Stack**
- ASP.NET Core 8
- MediatR
- Marten
- PostgreSQL
- Carter
- FluentValidation

---

## 🧺 Basket Microservice

**Purpose:** Manage shopping carts and pricing logic.

**Features**
- ASP.NET Core Web API
- RESTful CRUD endpoints
- **Redis** as distributed cache
- Implements:
  - Proxy Pattern
  - Decorator Pattern
  - Cache-Aside Pattern
- Consumes **Discount gRPC Service**
- Publishes `BasketCheckout` events via RabbitMQ
- Dockerized with Redis and PostgreSQL

**Tech Stack**
- ASP.NET Core 8
- Redis
- gRPC
- MassTransit
- RabbitMQ

---

## 💸 Discount Microservice

**Purpose:** Handle discount calculation logic.

**Features**
- High-performance **gRPC Service**
- Protobuf-based CRUD operations
- SQLite database using **Entity Framework Core**
- Code-first migrations
- N-Layer Architecture
- Fully containerized

**Tech Stack**
- ASP.NET Core gRPC
- EF Core
- SQLite
- Docker

---

## 📑 Ordering Microservice

**Purpose:** Process and manage orders.

**Features**
- ASP.NET Core **Minimal APIs**
- **DDD, CQRS, and Clean Architecture**
- Domain Events & Integration Events
- SQL Server with EF Core Code-First
- Consumes `BasketCheckout` events via RabbitMQ
- SOLID principles & Dependency Injection
- Fully containerized

**Tech Stack**
- ASP.NET Core 8
- EF Core
- SQL Server
- MassTransit
- RabbitMQ

---

## 🔁 Asynchronous Messaging (RabbitMQ)

**Purpose:** Enable decoupled, scalable communication.

**Features**
- Publish/Subscribe pattern
- Topic exchange model
- Shared `EventBus.Messages` library
- MassTransit abstraction over RabbitMQ
- Dockerized message broker

---

## 🌐 YARP API Gateway

**Purpose:** Unified entry point for client applications.

**Features**
- Reverse proxy routing
- Gateway Routing Pattern
- Rate limiting using `FixedWindowLimiter`
- Route, cluster, path & transform configuration
- Dockerized service

**Tech Stack**
- YARP Reverse Proxy
- ASP.NET Core 8

---

## 🖥️ Shopping Web Client

**Purpose:** Client-facing UI.

**Features**
- ASP.NET Core Razor Pages
- Bootstrap 4 UI
- API consumption via **Refit**
- HttpClientFactory integration
- View Components, Partial Views, Tag Helpers
- Fully containerized

---

## 🛠️ Patterns & Practices

### Architecture Styles
- Domain-Driven Design (DDD)
- Clean Architecture
- Vertical Slice Architecture

### Design Patterns
- SOLID Principles
- CQRS & Mediator
- Proxy & Decorator
- Publish-Subscribe
- API Gateway
- Cache-Aside
- Options Pattern

---

## 🗄️ Data & Infrastructure

| Technology | Usage |
|-----------|------|
| PostgreSQL | Catalog (Document DB via Marten) |
| SQL Server | Ordering |
| SQLite | Discount |
| Redis | Basket cache |
| RabbitMQ | Message broker |
| Docker | Containerization |
| Docker Compose | Local orchestration |

---

## ▶️ Running the Application

```bash
docker-compose up -d
