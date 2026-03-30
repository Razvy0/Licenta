# SkillSync — Getting Started

A skill-swap community marketplace built with .NET 9 + React 18.

---

## Prerequisites

| Tool | Version | Check |
|------|---------|-------|
| .NET SDK | 9.x | `dotnet --version` |
| Node.js | 18+ | `node --version` |
| npm | 9+ | `npm --version` |
| SQL Server | LocalDB or full | `sqllocaldb info` |

---

## 1. Backend (.NET API)

Open a terminal in the project root (`Licenta/`).

```bash
# Restore NuGet packages
dotnet restore

# Build the solution (verify no errors)
dotnet build

# Apply database migrations (first time only)
dotnet ef migrations add InitialCreate --project SkillSync.Infrastructure --startup-project SkillSync.API
dotnet ef database update --project SkillSync.Infrastructure --startup-project SkillSync.API

# Start the API with hot-reload
cd SkillSync.API
dotnet watch run
```

The API will be available at:
- **https://localhost:5001** (HTTPS)
- **http://localhost:5000** (HTTP)

> **Note:** If you get a certificate error, run `dotnet dev-certs https --trust` once.

---

## 2. Frontend (React)

Open a **second terminal** in the `skillsync-client/` folder.

```bash
cd skillsync-client

# Install dependencies (first time only)
npm install

# Start the dev server
npm run dev
```

The frontend will be available at:
- **http://localhost:5173**

---

## 3. Both Running Together

You need **two terminals** running simultaneously:

| Terminal | Directory | Command |
|----------|-----------|---------|
| 1 — Backend | `SkillSync.API/` | `dotnet watch run` |
| 2 — Frontend | `skillsync-client/` | `npm run dev` |

---

## 4. Environment Variables

Secrets are loaded from `SkillSync.API/.env` (git-ignored). Create it with:

> **Never commit `.env` to git.** It's already in `.gitignore`.

---

## 5. Database Configuration

The connection string is read from the `CONNECTION_STRING` env var in `.env`.
---

## 6. API Endpoints Overview

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/auth/register` | No | Create account |
| POST | `/api/auth/login` | No | Sign in (returns JWT) |
| GET | `/api/users/{id}` | No | Get user profile |
| PUT | `/api/users/profile` | Yes | Update own profile |
| GET | `/api/skills` | No | List skills (with filters) |
| POST | `/api/skills` | Yes | Add a skill |
| DELETE | `/api/skills/{id}` | Yes | Remove own skill |
| GET | `/api/swaps` | Yes | Get your swaps |
| POST | `/api/swaps` | Yes | Propose a swap |
| PUT | `/api/swaps/{id}/status` | Yes | Accept/Reject/Complete |
| GET | `/api/categories` | No | List categories |
| POST | `/api/categories` | Yes | Create category |
| GET | `/api/messages/{userId}` | Yes | Get conversation |
| POST | `/api/messages` | Yes | Send message |
| — | `/chathub` | Yes | SignalR real-time chat |

---

## 7. Project Structure

```
Licenta/
├── SkillSync.sln
├── SkillSync.Core/           # Entities, DTOs, Enums, Interfaces, Exceptions
├── SkillSync.Infrastructure/ # DbContext, EF Configurations, Repositories
├── SkillSync.API/            # Controllers, Services, Middleware, Hubs, Program.cs
└── skillsync-client/         # React + Vite + TypeScript + Tailwind
    ├── src/
    │   ├── components/       # Layout, SkillCard
    │   ├── hooks/            # useAuth, useSkills, useSwaps
    │   ├── pages/            # Login, Register, Dashboard, Explore, Profile, Swaps
    │   ├── services/         # Axios API clients
    │   └── stores/           # Zustand auth store
    └── package.json
```