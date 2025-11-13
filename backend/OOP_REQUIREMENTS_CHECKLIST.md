# OOP Requirements Checklist ✅

## Classes Required ✅

| Class | Location | Purpose |
|-------|----------|---------|
| Book | `Models/Book.cs` | Represents library books |
| User | `Models/User.cs` | Abstract base class for all users |
| Student | `Models/Student.cs` | Student user type |
| Faculty | `Models/Faculty.cs` | Faculty user type |
| Loan | `Models/Loan.cs` | Book borrowing records |
| Fine | `Models/Fine.cs` | Overdue fines |
| Reservation | `Models/Reservation.cs` | Book reservations |
| Catalog | `Models/Catalog.cs` | Book categorization |

## Inheritance ✅

**Base Class**: `User` (abstract)
```csharp
public abstract class User
{
    public abstract int GetMaxBorrowLimit();
    public abstract int GetMaxBorrowDays();
    public abstract decimal GetDailyFineRate();
}
```

**Derived Classes**:
- `Student : User`
- `Faculty : User`

**Files**:
- `Models/User.cs` (lines 1-60)
- `Models/Student.cs` (lines 1-35)
- `Models/Faculty.cs` (lines 1-35)

## Polymorphism ✅

### Method Overriding
Different borrowing rules implemented polymorphically:

**Student Implementation**:
```csharp
public override int GetMaxBorrowLimit() => 3;
public override int GetMaxBorrowDays() => 14;
public override decimal GetDailyFineRate() => 5.00m;
```

**Faculty Implementation**:
```csharp
public override int GetMaxBorrowLimit() => 10;
public override int GetMaxBorrowDays() => 30;
public override decimal GetDailyFineRate() => 10.00m;
```

### Strategy Pattern (Another form of Polymorphism)
- `IBorrowingStrategy` interface
- `StudentBorrowingStrategy` implementation
- `FacultyBorrowingStrategy` implementation

**Files**:
- `Interfaces/IBorrowingStrategy.cs`
- `Strategies/StudentBorrowingStrategy.cs`
- `Strategies/FacultyBorrowingStrategy.cs`

## Encapsulation ✅

### Private Fields with Validated Properties

**Example 1 - Book Class**:
```csharp
private string _isbn;
public string ISBN
{
    get => _isbn;
    set
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ISBN cannot be empty");
        _isbn = value;
    }
}
```

**Example 2 - User Class**:
```csharp
private string _email;
public string Email
{
    get => _email;
    set
    {
        if (!value.Contains("@"))
            throw new ArgumentException("Invalid email format");
        _email = value;
    }
}
```

**Example 3 - Book Inventory**:
```csharp
private int _availableCopies;
public int AvailableCopies
{
    get => _availableCopies;
    set
    {
        if (value < 0)
            throw new ArgumentException("Available copies cannot be negative");
        if (value > TotalCopies)
            throw new ArgumentException("Available copies cannot exceed total");
        _availableCopies = value;
    }
}
```

**Files**: All model classes in `Models/` folder

## Interfaces ✅

| Interface | Purpose | Location |
|-----------|---------|----------|
| `IRepository<T>` | Generic data access | `Interfaces/IRepository.cs` |
| `IBookRepository` | Book-specific data access | `Interfaces/IBookRepository.cs` |
| `IUserRepository` | User-specific data access | `Interfaces/IUserRepository.cs` |
| `ILoanService` | Business logic for loans | `Interfaces/ILoanService.cs` |
| `IBorrowingStrategy` | Borrowing rules strategy | `Interfaces/IBorrowingStrategy.cs` |
| `IUserFactory` | User creation factory | `Interfaces/IUserFactory.cs` |

## Design Patterns ✅

### 1. Repository Pattern
**Purpose**: Separate data access from business logic

**Interface**: `IRepository<T>`
**Implementation**: `Repository<T>`
**Specific Repos**: `BookRepository`, `UserRepository`

**Files**:
- `Interfaces/IRepository.cs`
- `Repositories/Repository.cs`
- `Repositories/BookRepository.cs`
- `Repositories/UserRepository.cs`

### 2. Strategy Pattern
**Purpose**: Different borrowing rules for different user types

**Interface**: `IBorrowingStrategy`
**Strategies**:
- `StudentBorrowingStrategy`
- `FacultyBorrowingStrategy`

**Usage**: `LoanService` uses strategy to calculate fines and check limits

**Files**:
- `Interfaces/IBorrowingStrategy.cs`
- `Strategies/StudentBorrowingStrategy.cs`
- `Strategies/FacultyBorrowingStrategy.cs`
- `Services/LoanService.cs` (line 133-140)

### 3. Factory Pattern
**Purpose**: Create user objects without specifying exact class

**Interface**: `IUserFactory`
**Implementation**: `UserFactory`

**Code**:
```csharp
public User CreateUser(string userType, string name, string email)
{
    return userType.ToLower() switch
    {
        "student" => new Student { Name = name, Email = email },
        "faculty" => new Faculty { Name = name, Email = email },
        _ => throw new ArgumentException("Invalid user type")
    };
}
```

**Files**:
- `Interfaces/IUserFactory.cs`
- `Factories/UserFactory.cs`

## SOLID Principles ✅

### S - Single Responsibility Principle
Each class has one job:
- `BookRepository` → Only handles book data access
- `LoanService` → Only handles loan business logic
- `Student` → Only represents student data and behavior

### O - Open/Closed Principle
- `User` class is open for extension (can add more user types)
- Closed for modification (don't need to change User class to add new types)
- **Example**: Add `Librarian : User` without changing existing code

### L - Liskov Substitution Principle
- `Student` and `Faculty` can replace `User` anywhere
- **Example**: `LoanService.BorrowBookAsync()` accepts `User`, works with both Student and Faculty

### I - Interface Segregation Principle
- Focused interfaces: `IBookRepository`, `ILoanService`
- Clients only depend on methods they need
- **Example**: `ILoanService` only has loan methods, not book CRUD operations

### D - Dependency Inversion Principle
- High-level modules depend on abstractions
- **Example**: `LoanService` depends on `IBookRepository`, not `BookRepository`
- Configured in `Program.cs` using Dependency Injection

## Features Implemented ✅

- ✅ Manage books (CRUD operations)
- ✅ Manage users (Students and Faculty)
- ✅ Handle borrowing and returns
- ✅ Calculate fines (different rates for Student vs Faculty)
- ✅ Different borrowing limits (3 for Students, 10 for Faculty)
- ✅ Different borrowing periods (14 days for Students, 30 for Faculty)
- ✅ Generate reports (structure ready)

## Database Setup

**Technology**: Entity Framework Core with MySQL

**Context**: `LibraryDbContext.cs`

**Tables Created**:
- Users (with Student and Faculty using TPH inheritance)
- Books
- Loans
- Fines
- Reservations
- Catalogs

## Next Steps

1. **Create Controllers** - Wire up the API endpoints
2. **Run Migrations** - Create the database
3. **Write Tests** - xUnit test project
4. **Add More Features** - Authentication, advanced reports

## Quick Reference: Where to Find Code

| Concept | File | Lines |
|---------|------|-------|
| Inheritance | `Models/User.cs` | 14-60 |
| Polymorphism | `Models/Student.cs`, `Models/Faculty.cs` | 31-34 |
| Encapsulation | `Models/Book.cs` | 12-86 |
| Repository Pattern | `Repositories/BookRepository.cs` | All |
| Strategy Pattern | `Strategies/StudentBorrowingStrategy.cs` | All |
| Factory Pattern | `Factories/UserFactory.cs` | All |
| DI Configuration | `Program.cs` | 17-30 |
| Database Context | `Data/LibraryDbContext.cs` | All |
