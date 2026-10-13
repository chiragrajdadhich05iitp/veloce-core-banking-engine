<div align="center">

# ⚡ VeloceBank Core Ledger Engine
### Distributed, Low-Latency Transaction Processing Engine & Core Banking Microservice

[![Runtime](https://img.shields.io/badge/Runtime-.NET%208.0%20LTS-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Language](https://img.shields.io/badge/Language-C%23%2012-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Database](https://img.shields.io/badge/Persistence-MongoDB%20v2.24-47A248?style=for-the-badge&logo=mongodb&logoColor=white)](https://www.mongodb.com/)
[![Specification](https://img.shields.io/badge/API-OpenAPI%203.0%20%2F%20Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)](https://swagger.io/)
[![License](https://img.shields.io/badge/License-MIT-blue?style=for-the-badge)](LICENSE)

*An asynchronous, highly concurrent transactional banking engine designed for sub-millisecond state resolution, strict double-entry ledger invariants, and resilient fund settlement under distributed lock contention.*

</div>

---

## 🏛 Architecture & Systems Philosophy

**VeloceBank Core** is engineered as a decoupled, zero-lock-contention transactional ledger and account settlement engine. Built on top of the modern **ASP.NET Core (.NET 8.0)** asynchronous execution pipeline and backed by an optimized **MongoDB BSON persistence layer**, it guarantees auditable state transitions, non-blocking asynchronous I/O across boundary lines, and fault-tolerant account lifecycle orchestration.

### Architectural Schematic

                              [ INGRESS CLIENTS / GATEWAY ]
                                            │
                                            ▼  HTTPS / TLS 1.3
                 ┌───────────────────────────────────────────────────────┐
                 │           PRESENTATION & API GATEWAY LAYER            │
                 │  - REST Controllers          - OpenAPI 3.0 Specs      │
                 │  - Route Invariant Guards    - Global Error Handling  │
                 └──────────────────────────┬────────────────────────────┘
                                            │ DTOs / Service Contracts
                                            ▼
                 ┌───────────────────────────────────────────────────────┐
                 │            DOMAIN SERVICES & AUDIT ENGINE             │
                 │  ┌───────────────────────┐  ┌───────────────────────┐ │
                 │  │ AccountsService       │  │ TransactionsService   │ │
                 │  │ (Balance Resolution)  │  │ (Idempotent Ledger)   │ │
                 │  ├───────────────────────┤  ├───────────────────────┤ │
                 │  │ AuthService / Users   │  │ Goals & Tags Engine   │ │
                 │  │ (BCrypt Hash Guard)   │  │ (Category Aggregator) │ │
                 │  └───────────────────────┘  └───────────────────────┘ │
                 └──────────────────────────┬────────────────────────────┘
                                            │ Connection Pool / BSON Driver
                                            ▼
                 ┌───────────────────────────────────────────────────────┐
                 │          DISTRIBUTED PERSISTENCE SUBSYSTEM            │
                 │         MongoDB High-Throughput Document Store        │
                 │       Collections: Users | Accounts | Transactions    │
                 └───────────────────────────────────────────────────────┘

---

## 🔬 Core Engineering Principles

* **Atomic Transaction Invariants:** Guarantees deterministic fund routing and ledger synchronization across asymmetric debits and credits, strictly mitigating race conditions during concurrent mutations.
* **Idempotent Mutation Pipeline:** Prevents duplicate processing and ledger skew across network retry cascades and client-side replay vectors.
* **Lock-Free Asynchronous I/O:** Eliminates thread-pool starvation under burst workloads by leveraging non-blocking Task-based Asynchronous Patterns (TAP).
* **Hardened Security Perimeter:** Employs salted, computation-intensive BCrypt cryptographic derivations to neutralize brute-force exposure across credential storage vectors.
* **Inversion of Control (IoC):** Fine-grained singleton and scoped dependency injection lifetimes minimizing GC pressure, object allocation churn, and heap fragmentation.

---

## 📡 RESTful API Surface Specifications

| Subsystem | HTTP Verb | Route Path | Technical Functionality | State Impact |
| :--- | :--- | :--- | :--- | :--- |
| **Authentication** | `POST` | `/api/Auth/Login` | Salt-verified credential validation & session negotiation | Read-Only |
| **Accounts** | `GET` | `/api/Account` | Query aggregated account topologies and real-time balances | Read-Only |
| **Accounts** | `POST` | `/api/Account` | Provision new ledger accounts with base state validation | Document Insert |
| **Accounts** | `PUT` | `/api/Account/{id}` | Mutate account status and transactional limits | Atomic Update |
| **Accounts** | `DELETE`| `/api/Account/{id}` | Idempotently decommission ledger records | Soft Tombstone |
| **Transactions** | `POST` | `/api/Transactions` | Atomically commit balance debits, credits, and ledger transfers | Distributed Write |
| **Goals** | `POST` | `/api/Goal` | Initialize financial savings milestones bound to target accounts | Document Insert |

---

## 🛠 Technology Matrix

| Dimension | Specification | Engineering Purpose |
| :--- | :--- | :--- |
| **Target Runtime** | `.NET 8.0 (LTS)` | High-throughput, cross-platform JIT compilation with tiered PGO |
| **Language Standards** | `C# 12` | Pattern matching, records, primary constructors, and memory safety |
| **Framework** | `ASP.NET Core Web API` | Native low-overhead HTTP parsing and asynchronous middleware pipeline |
| **Persistence Engine**| `MongoDB.Driver v2.24.0`| High write-throughput NoSQL document serialization via native BSON |
| **Cryptographic Primitive** | `BCrypt.Net-Next v4.0.3` | Password hashing with adaptive work factors preventing rainbow table attacks |
| **Contract Tooling** | `Swashbuckle / OpenAPI 3.0` | Strongly typed contract generation and interactive API sandbox |

---

## ⚡ Deployment & Local Orchestration

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (x64)
* MongoDB Daemon running locally on port `27017` or cloud MongoDB Atlas URI

### 1. Repository Bootstrap
```bash
git clone [https://github.com/chiragrajdadhich05iitp/veloce-core-banking-engine.git](https://github.com/chiragrajdadhich05iitp/veloce-core-banking-engine.git)
cd veloce-core-banking-engine
2. Configuration Matrix
Set your target connection pool in CoreLedger.Api/appsettings.json:

JSON
{
  "ConnectionStrings": {
    "CoreLedger": "mongodb://localhost:27017"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
3. Restore & Launch
Bash
cd CoreLedger.Api
dotnet restore
dotnet build -c Release
dotnet run
4. Interactive API Sandboxing
Once the runtime logs Now listening on: http://localhost:11366, launch Swagger UI:

http://localhost:11366/swagger
🧪 Invariant Verification & Test Suite
The repository contains automated unit and integration tests verifying transactional boundaries and service contract invariants:

Bash
# Run unit test suite from root
dotnet test --verbosity normal
👤 Author & Architecture
Chirag Raj Dadhich

Specializing in High-Throughput Distributed Systems, Scalable Key-Value Stores, and Financial Ledger Engines.
