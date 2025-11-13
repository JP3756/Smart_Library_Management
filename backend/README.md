# Smart Library Management System - Backend

ASP.NET Core Web API demonstrating Object-Oriented Programming principles and design patterns.

## 🎯 OOP Principles Demonstrated

### 1. **Inheritance** ✅
- **Base Class**: `User` (abstract class)
- **Derived Classes**: `Student` and `Faculty`
- **Location**: `Models/User.cs`, `Models/Student.cs`, `Models/Faculty.cs`

```csharp
public abstract class User { }
public class Student : User { }
public class Faculty : User { }
```

### 2. **Polymorphism** ✅
- **Method Overriding**: Different borrowing rules per user type
- **Abstract Methods**: `GetMaxBorrowLimit()`, `GetMaxBorrowDays()`, `GetDailyFineRate()`
- **Runtime Polymorphism**: Strategy pattern for borrowing rules

```csharp
// Student: 3 books, 14 days, 5 pesos/day
public override int GetMaxBorrowLimit() => 3;

// Faculty: 10 books, 30 days, 10 pesos/day  
public override int GetMaxBorrowLimit() => 10;
```

### 3. **Encapsulation** ✅
- **Private Fields**: All model properties use private backing fields
- **Validated Properties**: Business logic in setters
- **Data Hiding**: Internal implementation details are hidden

```csharp
private string _email;
public string Email
{
    get => _email;
    set
    {
        if (!value.Contains("@"))
            throw new ArgumentException("Invalid email");
        _email = value;
    }
}
```

### 4. **Abstraction** ✅
- **Interfaces**: `IRepository<T>`, `ILoanService`, `IBorrowingStrategy`, `IUserFactory`
- **Abstract Classes**: `User`
- **Implementation Hiding**: Clients work with interfaces, not concrete classes

## 🏗️ Design Patterns Implemented

### 1. **Repository Pattern** ✅
- **Purpose**: Separates data access logic from business logic
- **Implementation**:
  - `IRepository<T>` - Generic repository interface
  - `IBookRepository` - Book-specific repository
  - `BookRepository` - Concrete implementation
- **SOLID Principle**: Single Responsibility Principle (SRP)

### 2. **Strategy Pattern** ✅
- **Purpose**: Different borrowing rules for different user types
- **Implementation**:
  - `IBorrowingStrategy` - Strategy interface
  - `StudentBorrowingStrategy` - Student rules
  - `FacultyBorrowingStrategy` - Faculty rules
- **SOLID Principle**: Open/Closed Principle (OCP)

### 3. **Factory Pattern** ✅
- **Purpose**: Create user objects without specifying exact class
- **Implementation**:
  - `IUserFactory` - Factory interface
  - `UserFactory` - Creates Student or Faculty based on type
- **Benefit**: Centralizes object creation logic

## 🔧 SOLID Principles

### S - Single Responsibility Principle ✅
- Each class has one reason to change
- `BookRepository` only handles data access
- `LoanService` only handles business logic
- **Example**: `BookRepository.cs` - only database operations for books

### O - Open/Closed Principle ✅
- Open for extension, closed for modification
- New user types can be added without changing existing code
- **Example**: Add new user type by extending `User` class

### L - Liskov Substitution Principle ✅
- Derived classes can substitute base class
- `Student` and `Faculty` can be used anywhere `User` is expected
- **Example**: `LoanService` works with `User`, accepts `Student` or `Faculty`

### I - Interface Segregation Principle ✅
- Clients don't depend on unused methods
- Specific interfaces: `IBookRepository`, `ILoanService`
- **Example**: `ILoanService` only has loan-related methods

### D - Dependency Inversion Principle ✅
- Depend on abstractions, not concrete classes
- Services use interfaces, not implementations
- **Example**: `LoanService` depends on `IBookRepository`, not `BookRepository`

## 📦 Project Structure

```
SmartLibraryAPI/
├── Models/               # Domain entities
│   ├── User.cs          # Abstract base class
│   ├── Student.cs       # Inherits User
│   ├── Faculty.cs       # Inherits User
│   ├── Book.cs
│   ├── Loan.cs
│   ├── Fine.cs
│   ├── Reservation.cs
│   └── Catalog.cs
│
├── Interfaces/          # Abstractions (DIP)
│   ├── IRepository.cs
│   ├── IBookRepository.cs
│   ├── IUserRepository.cs
│   ├── ILoanService.cs
│   ├── IBorrowingStrategy.cs
│   └── IUserFactory.cs
│
├── Repositories/        # Data access (Repository Pattern)
│   ├── Repository.cs
│   ├── BookRepository.cs
│   └── UserRepository.cs
│
├── Services/            # Business logic
│   └── LoanService.cs   # Uses Strategy Pattern
│
├── Strategies/          # Strategy Pattern
│   ├── StudentBorrowingStrategy.cs
│   └── FacultyBorrowingStrategy.cs
│
├── Factories/           # Factory Pattern
│   └── UserFactory.cs
│
├── DTOs/                # Data Transfer Objects
│   ├── BookDtos.cs
│   ├── UserDtos.cs
│   ├── LoanDtos.cs
│   └── ReportDtos.cs
│
├── Data/                # Database context
│   └── LibraryDbContext.cs
│
└── Controllers/         # API endpoints (to be created)
    ├── BooksController.cs
    ├── UsersController.cs
    ├── LoansController.cs
    └── ReportsController.cs
```

## �️ Tech Stack

- **C# with ASP.NET Core Web API** (.NET 9)
- **PostgreSQL** database
- **Entity Framework Core** (ORM)
- **Npgsql** (PostgreSQL provider)
- **Swagger/OpenAPI** (API documentation)
- **Dependency Injection** (built-in)

## �🚀 Features

### Core Functionality
- ✅ **Book Management**: Add, update, delete, search books
- ✅ **User Management**: Students and Faculty with different privileges
- ✅ **Borrowing System**: Check out and return books
- ✅ **Fine Calculation**: Automatic fine calculation based on user type
- ✅ **Reservations**: Book reservation system
- ✅ **Reports**: Statistics and borrowing reports

### Business Rules
- **Students**:
  - Max 3 books
  - 14 days borrowing period
  - 5 pesos/day fine (progressive after 7 days)
  
- **Faculty**:
  - Max 10 books
  - 30 days borrowing period
  - 3 days grace period
  - 10 pesos/day fine (progressive after 14 days)

## 🗄️ Database Setup

### 1. Install PostgreSQL
Download and install PostgreSQL from [postgresql.org](https://www.postgresql.org/download/)

### 2. Update Connection String
Edit `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=SmartLibraryDB;Username=postgres;Password=YOUR_PASSWORD"
}
```

### 3. Install EF Core Tools
```bash
dotnet tool install --global dotnet-ef
```

### 4. Create Database Migration
```bash
cd backend/SmartLibraryAPI
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 📝 Next Steps

1. **Create Controllers** - API endpoints for Books, Users, Loans, Reports
2. **Add Authentication** - JWT-based authentication
3. **Write Unit Tests** - Using xUnit
4. **Add Validation** - FluentValidation
5. **Logging** - Serilog integration
6. **Error Handling** - Global exception handler

## 🧪 Testing (xUnit)

To be implemented:
- Unit tests for business logic
- Integration tests for repositories
- Mock testing with Moq

## 📖 API Documentation

Once controllers are created, Swagger UI will be available at:
- `https://localhost:5001/swagger`

## 🎓 Learning Outcomes

This project demonstrates:
- ✅ Clean Architecture principles
- ✅ SOLID principles in practice
- ✅ Design patterns (Repository, Strategy, Factory)
- ✅ Entity Framework Core with MySQL
- ✅ Dependency Injection
- ✅ RESTful API design
