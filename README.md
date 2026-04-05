# 📚 Smart Library Management System

A web application built with **ASP.NET Core MVC** for managing a  library — books, categories, and borrow operations.

---

## 🚀 Features

### 📖 Book Management
- Create, Edit, Delete, and View books
- Each book contains: Title, Author, Price, Stock Quantity, and Category
- Stock availability tracking

### 🗂️ Category Management
- Create, Edit, Delete categories
- Assign categories to books via dropdown

### 🔄 Borrow Management
- Create borrow transactions with multiple books
- Stock validation before confirmation
- Automatic stock decrease after borrow
- Full rollback on error (transaction-safe)

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 8) |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Frontend | Bootstrap 5 + Bootstrap Icons |
| Validation | Data Annotations |

---

## 📁 Project Structure

```
Smart_Library_Management_System/
├── Controllers/
│   ├── BooksController.cs
│   ├── CategoriesController.cs
│   └── BorrowsController.cs
├── Models/
│   ├── Book.cs
│   ├── Category.cs
│   ├── Borrow.cs
│   └── BorrowItem.cs
├── Views/
│   ├── Books/
│   ├── Categories/
│   ├── Borrows/
│   └── Shared/
│       └── _Layout.cshtml
├── Data/
│   └── LibraryDbContext.cs
└── Migrations/
```

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

### Installation

**1. Clone the repository**
```bash
git clone https://github.com/your-username/Smart_Library_Management_System.git
cd Smart_Library_Management_System
```

**2. Configure the database connection**

In `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=LibraryDB;Trusted_Connection=True;"
}
```

**3. Apply migrations and seed data**
```bash
dotnet ef database update
```

**4. Run the application**
```bash
dotnet run
```

Open your browser at `http://localhost:5245`

---

## 🗃️ Database Schema

```
Category
├── CategoryID (PK)
└── Name

Book
├── BookID (PK)
├── Title
├── Author
├── Price
├── StockQuantity
└── CategoryId (FK → Category)

Borrow
├── BorrowID (PK)
├── BorrowerName
├── BorrowDate
└── ReturnDate (nullable)

BorrowItem
├── Id (PK)
├── BorrowId (FK → Borrow)
├── BookID (FK → Book)
└── Quantity
```

---

## 🔒 Borrow Transaction Flow

```
BeginTransaction()
    │
    ├── For each book:
    │     ├── ✅ Validate stock
    │     ├── ✅ Update stock (decrease)
    │     └── ✅ Add BorrowItem
    │
    ├── SaveChangesAsync()
    ├── CommitAsync() ✅
    │
    └── On error → RollbackAsync() ❌
```

---

## 📌 Seed Data

The application seeds initial data on startup:

- **4 Categories**: Science, Literature, History, Technology
- **10+ Books** distributed across categories

---

## ✅ Technical Requirements Met

- [x] ASP.NET Core MVC architecture
- [x] Entity Framework Core with Migrations
- [x] `AsNoTracking()` for listing queries
- [x] `Include()` to avoid N+1 queries
- [x] Filtering before `ToList()`
- [x] Data Annotations: `[Required]`, `[Range]`, `[StringLength]`
- [x] Transaction with Rollback for borrow creation
- [x] Bootstrap 5 UI with navigation bar

---

## 👤 Author

Siham Bouzagra <Developed as part of a university mini-project>.

---

## 📄 License

This project is for educational purposes only.
