# Project Blueprint: SkillSync (Skill-Swap Community Marketplace)

## 1. Project Overview
**SkillSync** is a web-based platform where users can trade services using "Time/Credits" instead of currency. A user can offer a skill (e.g., Web Design) and request a skill (e.g., Spanish Tutoring). The system manages matching, booking, and real-time communication.

## 2. Technology Stack
* **Frontend:** React 18+ (via Vite), TypeScript, Tailwind CSS, React Router DOM, Axios, React Query (for server state management), Zustand (for local state).
* **Backend:** .NET 8 (C#), ASP.NET Core Web API, Entity Framework Core (Code-First approach).
* **Database:** Microsoft SQL Server.
* **Real-time / Extras:** SignalR (for live chat), ASP.NET Core Identity + JWT (for Authentication/Authorization).

---

## 3. System Architecture
The application will follow a strict separation of concerns to ensure scalability and high grading for the academic project.

### Backend (N-Tier Architecture / Repository Pattern)
1.  **Presentation Layer (Controllers):** Handles HTTP requests, validates DTOs, and returns HTTP responses.
2.  **Business Logic Layer (Services):** Contains all the core logic (e.g., checking if a user has enough "Time Credits" before initiating a swap).
3.  **Data Access Layer (Repositories):** Abstracts Entity Framework queries. Only generic or specific repositories interact directly with the DB context.
4.  **Core/Domain Layer:** Contains Entity models, Enums, and custom Exceptions.

### Frontend (Component-Based Architecture)
1.  **Pages:** Top-level route components (e.g., `Dashboard.tsx`, `Profile.tsx`).
2.  **Components:** Reusable UI pieces (e.g., `SkillCard.tsx`, `ChatBox.tsx`).
3.  **Services/API:** Axios interceptors and functions mapping to backend endpoints.
4.  **Hooks:** Custom React Hooks to encapsulate logic (e.g., `useAuth()`, `useChat()`).

---

## 4. Database Schema Design (SQL)
*Using Entity Framework Code-First, these will be generated from C# models.*

| Table Name | Primary Key | Key Columns & Relationships |
| :--- | :--- | :--- |
| **Users** | `Id` (GUID) | `Email`, `PasswordHash`, `FullName`, `Bio`, `TimeBalance` (int - hours), `Rating` (float). |
| **Categories** | `Id` (Int) | `Name` (e.g., "Languages", "IT", "Music"), `Description`. |
| **Skills** | `Id` (Int) | `UserId` (FK), `CategoryId` (FK), `Title`, `Description`, `ProficiencyLevel`, `IsOffering` (boolean). |
| **SwapRequests** | `Id` (Int) | `RequesterId` (FK), `ReceiverId` (FK), `OfferedSkillId` (FK), `RequestedSkillId` (FK), `Status` (Enum: Pending, Accepted, Completed, Rejected), `ScheduledDate`. |
| **Messages** | `Id` (Int) | `SenderId` (FK), `ReceiverId` (FK), `Content` (string), `Timestamp`, `IsRead` (boolean). |
| **Reviews** | `Id` (Int) | `ReviewerId` (FK), `RevieweeId` (FK), `SwapRequestId` (FK), `Score` (1-5), `Comment`. |

---

## 5. User Flow & API Call Sequence

### A. Authentication Flow
1.  **User registers:** React posts `{ email, password, name }` to `/api/auth/register`.
2.  **Backend processes:** Hashes password, saves to DB, returns `200 OK`.
3.  **User logs in:** React posts `{ email, password }` to `/api/auth/login`.
4.  **Backend verifies:** Generates a JWT (JSON Web Token) valid for 24h.
5.  **React stores JWT:** Saved in `localStorage` or `HttpOnly Cookie`. Axios Interceptor attaches this token to the `Authorization: Bearer <token>` header for all future requests.

### B. The "Swap" Flow
1.  **Discovery:** User A navigates to `/explore`. React calls `GET /api/skills?isOffering=true`.
2.  **Request:** User A wants User B's "Guitar Lesson". User A clicks "Propose Swap".
3.  **API Call:** React calls `POST /api/swaps` with the respective Skill IDs.
4.  **Notification:** Backend saves the request as `Pending` and triggers a SignalR event `ReceiveNotification` to User B.
5.  **Acceptance:** User B accepts. `PUT /api/swaps/{id}/accept`. Backend updates status to `Accepted`.

### C. The Chat Flow (SignalR)
1.  Once a swap is `Accepted`, a chat channel opens.
2.  React connects to `https://localhost:5001/chathub`.
3.  When User A types a message, React calls `Invoke("SendMessage", receiverId, message)`.
4.  Backend saves the message to the SQL DB (for history) and broadcasts it to User B in real-time.

---

## 6. REST API Endpoint Design

### Auth
* `POST /api/auth/register`
* `POST /api/auth/login`

### Users
* `GET /api/users/{id}` - Get public profile.
* `PUT /api/users/profile` - Update bio/details.

### Skills
* `GET /api/skills` - List skills (with query params: `?category=IT&search=React`).
* `POST /api/skills` - Add a skill to the user's profile.
* `DELETE /api/skills/{id}`

### Swaps
* `GET /api/swaps` - Get user's active/past swaps.
* `POST /api/swaps` - Create a new request.
* `PUT /api/swaps/{id}/status` - Update status (Accept/Reject/Complete).

---

## 7. Setup & Execution Commands

### Phase 1: Backend (.NET)
```bash
# 1. Create the solution and projects
dotnet new sln -n SkillSync
dotnet new webapi -n SkillSync.API
dotnet new classlib -n SkillSync.Core
dotnet new classlib -n SkillSync.Infrastructure

# 2. Add projects to solution
dotnet sln add SkillSync.API/SkillSync.API.csproj
dotnet sln add SkillSync.Core/SkillSync.Core.csproj
dotnet sln add SkillSync.Infrastructure/SkillSync.Infrastructure.csproj

# 3. Add Project References
cd SkillSync.API
dotnet add reference ../SkillSync.Core/SkillSync.Core.csproj
dotnet add reference ../SkillSync.Infrastructure/SkillSync.Infrastructure.csproj
cd ../SkillSync.Infrastructure
dotnet add reference ../SkillSync.Core/SkillSync.Core.csproj
cd ..

# 4. Install necessary NuGet Packages (Run in Infrastructure & API appropriately)
dotnet add SkillSync.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer
dotnet add SkillSync.Infrastructure package Microsoft.EntityFrameworkCore.Tools
dotnet add SkillSync.API package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add SkillSync.API package Microsoft.AspNetCore.SignalR

# 5. Entity Framework Migrations (After configuring DbContext)
dotnet ef migrations add InitialCreate --project SkillSync.Infrastructure --startup-project SkillSync.API
dotnet ef database update --project SkillSync.Infrastructure --startup-project SkillSync.API

# 6. Run the API
cd SkillSync.API
dotnet watch run

# 1. Create the Vite React TypeScript project
npm create vite@latest skillsync-client -- --template react-ts
cd skillsync-client

# 2. Install dependencies
npm install
npm install react-router-dom axios zustand @tanstack/react-query @microsoft/signalr lucide-react

# 3. Install Tailwind CSS
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p

# 4. Start the frontend development server
npm run dev

. Instructions for the AI Code Generator
Keep it clean: Strictly enforce the Repository pattern on the backend. Do not inject DbContext directly into Controllers.

DTOs: Use Data Transfer Objects (DTOs) for all API requests and responses to prevent over-posting and circular reference issues.

Error Handling: Implement a Global Exception Handler middleware in .NET to catch errors and return consistent JSON structures (e.g., { error: true, message: "..." }).

CORS: Ensure CORS is configured in Program.cs to allow the React frontend (localhost:5173) to communicate with the API, especially configuring credentials for SignalR.