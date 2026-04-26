# SkillSync Future Features Ideas

Here are 7 high-impact ideas to implement to make the SkillSync project stand out for the *licență*:

### 1. Robust "Time Bank" & Ledger System (Escrow Mechanics)
**The Idea:** The blueprint mentions a `TimeBalance`. Instead of just updating a simple integer in the database, implement a full transaction ledger. When a user requests a swap, their "time credits" should be locked (placed in escrow). Only when the swap is marked as `Completed` should the credits be transferred to the other user.
**Why it's great for a thesis:** This allows you to demonstrate advanced backend concepts like **Database Transactions** (ACID properties), concurrency handling (preventing double-spending), and financial-style ledgers.
**Deep Dive:**
*   **What it means:** "Escrow" means the platform temporarily holds a user's time credits securely while a swap is ongoing. A "Ledger" is an append-only table recording every single change in balance (like a bank statement), rather than just overwriting a single number.
*   **Current System:** According to your `SwapService.cs` and Entity models, `TimeBalance` is just an integer in the `Users` table. The swap logic currently just changes statuses (`Pending` -> `Accepted` -> `Completed`) without enforcing time balances.
*   **Limitations:** A user with 2 hours of balance could simultaneously propose 5 different swaps that each cost 1 hour. This is a classic "double-spend" problem. Furthermore, if you only update the balance upon swap completion, there is no history—if a balance seems wrong, there's no paper trail to audit why.
*   **Improvements:** 
    *   **Trust & Reliability:** Receivers know the requester actually has the time credits to pay them, because the credits are locked.
    *   **Transparency:** Users can view a "Transaction History" page showing exact deductions, refunds, and earnings.
    *   **Academic Value:** Demonstrates your ability to handle strict data integrity using Entity Framework Core's `IDbContextTransaction`.
*   **How to implement:**
    1.  **New Entity:** Create a `TimeTransaction` table (`Id`, `UserId`, `Amount`, `TransactionType` [EscrowHold, Earned, Spent, Refunded], `SwapRequestId`, `CreatedAt`).
    2.  **During Swap Creation:** In `CreateSwapAsync`, check if the requester has enough balance. If so, deduct the balance, create a `TimeTransaction` (EscrowHold), and save the `SwapRequest`. Wrap all these database calls in a single transaction (`await using var transaction = await _context.Database.BeginTransactionAsync();`) so if one fails, they all roll back.
    3.  **During Swap Cancellation/Rejection:** If a swap is rejected or cancelled, refund the requester's `TimeBalance` and log a `Refunded` transaction.
    4.  **During Swap Validation (Completion):** In `ValidateSwapAsync`, when the swap is marked `Completed`, add the balance to the receiver and create an `Earned` transaction for them.

### 2. Real-Time Chat System (SignalR)
**The Idea:** Once a swap is accepted, open a dedicated real-time chat room between the two users so they can discuss the details of their session.
**Why it's great for a thesis:** Your instructions already mention `ChatHub.cs` and SignalR. Implementing this proves you can handle asynchronous communication and WebSockets, which is a major step up from standard REST APIs. You can also add "read receipts" or typing indicators for extra polish.

### 3. Smart Matching / Mutual Suggestion Algorithm
**The Idea:** You have basic search, but you can build a "Recommended for You" engine. If User A offers "React" and wants "Spanish", and User B offers "Spanish" and wants "React", the system should automatically flag them as a **Perfect Match** and suggest the swap to both.
**Why it's great for a thesis:** Algorithms always score high in a thesis. You can write a specialized SQL query or a backend service that cross-references `IsOffering` and requested categories to generate these matches dynamically. 

### 4. Reputation, Reviews, and Dispute Resolution
**The Idea:** After a swap is completed, both users must leave a 1-5 star review and a comment. You can then calculate an aggregated `Rating` for the user. Furthermore, add a "Report/Dispute" button in case a user didn't show up.
**Why it's great for a thesis:** This builds trust in your platform. It shows you've thought about the real-world implications of your app. You could even create a small **Admin Dashboard** where an administrator can review these disputes and ban malicious users.

### 5. Interactive Dashboard Analytics & Charts
**The Idea:** Give users a visually appealing "Dashboard" page where they can see their statistics: a line chart of "Hours Earned vs Hours Spent" over the last 6 months, a pie chart of their most active categories, or a radar chart of their skill proficiencies.
**Why it's great for a thesis:** A thesis needs to look good during the presentation. Integrating a charting library (like `Recharts` or `Chart.js` in React) will make your application look incredibly professional and data-driven.

### 6. Scheduling and Calendar Integration
**The Idea:** Since swaps have a `ScheduledDate`, implement a calendar view within the app. Users can view a monthly or weekly calendar of their upcoming accepted swaps.
**Why it's great for a thesis:** Working with dates, timezones, and complex UI components (like `react-big-calendar` or `FullCalendar`) is a common real-world challenge. As a bonus feature, you could allow users to export the event as an `.ics` file to add to their Google/Apple Calendar.

### 7. Gamification & Badges
**The Idea:** Reward users for being active. Automatically grant them badges (e.g., "First Swap", "Teacher of the Month", "5-Star Streak") when they hit certain milestones.
**Why it's great for a thesis:** It allows you to implement an **Event-Driven** approach. When a swap completes or a review is posted, you can trigger an event that checks if the user qualifies for a new badge, decoupling the gamification logic from the core business logic.

### 8. Real-Time Typing Indicators (Chat Upgrade Phase 1)
**The Idea:** Enhance the existing chat by showing a "User is typing..." indicator when the other person is writing a message.
**Why it's great for a thesis:** This shows you understand transient real-time states that don't need to be saved to a database. It involves firing `UserTyping` and `UserStoppedTyping` events via SignalR and handling frontend input debouncing to prevent spamming the server.

### 9. Online Presence System (Chat Upgrade Phase 2)
**The Idea:** Implement a system to track if a user is currently online and looking at the app, displaying a green "Online" dot next to their name in the chat list.
**Why it's great for a thesis:** Demonstrates advanced SignalR connection lifecycle management by overriding `OnConnectedAsync` and `OnDisconnectedAsync`. It requires maintaining an in-memory store (like a `ConcurrentDictionary`) of active connections and broadcasting global `UserOnline` and `UserOffline` events.

### 10. Two-Way Real-Time Read Receipts (Chat Upgrade Phase 3)
**The Idea:** Upgrade the current one-way read system. When a user opens a chat and reads a message, instantly notify the *sender* in real-time so they see a "Seen" label or two blue ticks.
**Why it's great for a thesis:** Proves mastery over two-way reactive chat experiences. Instead of just a REST call to update the database, it involves broadcasting a `MessagesMarkedAsRead` event via WebSockets to seamlessly update the sender's UI without requiring a page refresh.
