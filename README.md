# NetCRM

NetCRM is a lightweight CRM (Customer Relationship Management) system built with **ASP.NET Core Razor Pages**, implementing JWT-based authentication and PostgreSQL for data storage.

---

## 🚀 Technologies Used

- ✅ ASP.NET Core (.NET 9)
- ✅ Razor Pages (UI)
- ✅ PostgreSQL (Code First with EF Core)
- ✅ JWT Authentication
- ✅ Entity Framework Core
- ✅ Bootstrap 5 (for responsive UI)
- ✅ Session management (for token handling)
- ✅ RESTful API (protected via JWT)

---

## ✨ Features

### 🔐 Authentication
- User login with JWT
- Role-based login response
- Token stored in session for Razor Pages

### 👥 Customer Management
- Create / Read / Update / Delete (CRUD)
- Customer filtering (by name, email, region, registration date range)
- API calls secured by JWT
- Friendly UI for managing customers

### 📄 Razor Pages UI
- Login / Dashboard / Customer List / Add / Edit / Delete pages
- Form validation
- UI messages in English
- Responsive design with Bootstrap

---

## 📦 How to Run

1. Clone the repo  
2. Make sure PostgreSQL is running  
3. Update your `appsettings.json` with your PostgreSQL connection string  
4. Run the migrations:


