# 🧠 Backend API - E-Commerce Platform (.NET + CQRS)

This is the backend API for the E-Commerce platform, built using **.NET** with **CQRS (Command Query Responsibility Segregation)** architecture. It powers both the Admin and Customer apps by handling business logic, secure data access, and real-time communication.

---

## 🏗️ Architecture: CQRS Pattern

The backend applies **CQRS** to cleanly separate read and write operations:

| Layer                | Description                                              |
|----------------------|----------------------------------------------------------|
| **Commands**         | Handle state-changing operations (create, update, delete)|
| **Queries**          | Handle read-only data fetching with optimal projection   |
| **MediatR**          | Used to dispatch command/query handlers cleanly          |
| **Entity Framework** | ORM for database access                                  |
| **SignalR / WebSocket** | Used for real-time chat system                        |
| **FluentValidation** | Input validation for commands                            |

---

## 📦 Main Features

### 🛒 Product & Bundle Management
- CRUD for individual products and grouped bundles
- Product categorization (enum-based: Food, Drink, Cosmetic, etc.)
- Stock tracking, discount relations, and product reviews

### 🚚 Delivery & Pickup Flow
- Manages delivery regions
- Handles customer selections (delivery or pickup)
- Stores can be filtered by customer’s area

### 💬 Real-time Chat API
- Implemented via **SignalR** (or WebSocket)
- Enables bi-directional communication between CS and Customer
- Supports order cancellation via chat requests

### 💳 Transactions API
- Handles order creation, status updates, and fulfillment tracking
- Delivery method stored per transaction
- Pickup confirmation and status update support

---

![image](https://github.com/user-attachments/assets/827813d2-c573-42f0-b698-d1f8206ce90d)
