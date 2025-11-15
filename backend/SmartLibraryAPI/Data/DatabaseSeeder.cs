using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Data;

public static class DatabaseSeeder
{
    public static void SeedDatabase(LibraryDbContext context)
    {
        // Only seed if database is empty
        if (context.Books.Any() && context.Books.Count() >= 10)
            return;
        
        // Clear existing data for fresh seed
        if (context.Books.Any())
        {
            Console.WriteLine("⚠️  Database already has data. Skipping seed...");
            return;
        }

        // Seed Books
        var books = new List<Book>
        {
            new Book { Title = "Clean Code", Author = "Robert C. Martin", ISBN = "978-0132350884", Category = "Programming", PublicationYear = 2008, TotalCopies = 5, AvailableCopies = 3, Publisher = "Prentice Hall", Description = "A Handbook of Agile Software Craftsmanship" },
            new Book { Title = "Design Patterns", Author = "Gang of Four", ISBN = "978-0201633610", Category = "Programming", PublicationYear = 1994, TotalCopies = 4, AvailableCopies = 2, Publisher = "Addison-Wesley", Description = "Elements of Reusable Object-Oriented Software" },
            new Book { Title = "The Pragmatic Programmer", Author = "Andrew Hunt", ISBN = "978-0135957059", Category = "Programming", PublicationYear = 2019, TotalCopies = 5, AvailableCopies = 5, Publisher = "Addison-Wesley" },
            new Book { Title = "Introduction to Algorithms", Author = "Thomas H. Cormen", ISBN = "978-0262033848", Category = "Computer Science", PublicationYear = 2009, TotalCopies = 6, AvailableCopies = 2, Publisher = "MIT Press", Description = "Comprehensive algorithms textbook" },
            new Book { Title = "Artificial Intelligence: A Modern Approach", Author = "Stuart Russell", ISBN = "978-0134610993", Category = "AI", PublicationYear = 2020, TotalCopies = 4, AvailableCopies = 2, Publisher = "Pearson" },
            new Book { Title = "Database System Concepts", Author = "Abraham Silberschatz", ISBN = "978-0078022159", Category = "Database", PublicationYear = 2019, TotalCopies = 5, AvailableCopies = 3, Publisher = "McGraw-Hill" },
            new Book { Title = "Operating System Concepts", Author = "Abraham Silberschatz", ISBN = "978-1118063330", Category = "Computer Science", PublicationYear = 2018, TotalCopies = 4, AvailableCopies = 2, Publisher = "Wiley" },
            new Book { Title = "Computer Networks", Author = "Andrew S. Tanenbaum", ISBN = "978-0132126953", Category = "Computer Science", PublicationYear = 2010, TotalCopies = 4, AvailableCopies = 3, Publisher = "Pearson" },
            new Book { Title = "The Art of Computer Programming", Author = "Donald Knuth", ISBN = "978-0201896831", Category = "Computer Science", PublicationYear = 1997, TotalCopies = 3, AvailableCopies = 1, Publisher = "Addison-Wesley", Description = "Fundamental algorithms" },
            new Book { Title = "Refactoring", Author = "Martin Fowler", ISBN = "978-0134757599", Category = "Programming", PublicationYear = 2018, TotalCopies = 5, AvailableCopies = 3, Publisher = "Addison-Wesley" },
            new Book { Title = "Head First Design Patterns", Author = "Eric Freeman", ISBN = "978-0596007126", Category = "Programming", PublicationYear = 2004, TotalCopies = 4, AvailableCopies = 2, Publisher = "O'Reilly" },
            new Book { Title = "Thinking in Java", Author = "Bruce Eckel", ISBN = "978-0131872486", Category = "Programming", PublicationYear = 2006, TotalCopies = 3, AvailableCopies = 1, Publisher = "Prentice Hall" },
        };

        context.Books.AddRange(books);
        context.SaveChanges();
        Console.WriteLine($"✅ Seeded {books.Count} books");

        // Seed Users (Students and Faculty)
        var users = new List<User>
        {
            new Student { Name = "Juan Dela Cruz", Email = "juan.delacruz@student.edu", StudentId = "STU2024001", Phone = "09171234567", IsActive = true },
            new Student { Name = "Maria Santos", Email = "maria.santos@student.edu", StudentId = "STU2024002", Phone = "09181234567", IsActive = true },
            new Student { Name = "Pedro Reyes", Email = "pedro.reyes@student.edu", StudentId = "STU2024003", Phone = "09191234567", IsActive = true },
            new Student { Name = "Ana Garcia", Email = "ana.garcia@student.edu", StudentId = "STU2024004", Phone = "09201234567", IsActive = true },
            new Student { Name = "Carlos Mendoza", Email = "carlos.mendoza@student.edu", StudentId = "STU2024005", Phone = "09211234567", IsActive = true },
            new Faculty { Name = "Dr. Roberto Cruz", Email = "roberto.cruz@faculty.edu", EmployeeId = "FAC2023001", Phone = "09221234567", IsActive = true, Department = "Computer Science" },
            new Faculty { Name = "Prof. Elena Rodriguez", Email = "elena.rodriguez@faculty.edu", EmployeeId = "FAC2023002", Phone = "09231234567", IsActive = true, Department = "Information Technology" },
            new Faculty { Name = "Dr. Miguel Santos", Email = "miguel.santos@faculty.edu", EmployeeId = "FAC2023003", Phone = "09241234567", IsActive = true, Department = "Software Engineering" },
        };

        context.Users.AddRange(users);
        context.SaveChanges();
        Console.WriteLine($"✅ Seeded {users.Count} users (5 students, 3 faculty)");

        // Get persisted books and users
        var persistedBooks = context.Books.ToList();
        var persistedStudents = context.Users.OfType<Student>().ToList();
        var persistedFaculty = context.Users.OfType<Faculty>().ToList();

        // Create Active Loans
        var activeLoans = new List<Loan>
        {
            // Student loans (14 days duration)
            new Loan
            {
                BookId = persistedBooks[0].Id, // Clean Code
                UserId = persistedStudents[0].Id,
                BorrowDate = DateTime.UtcNow.AddDays(-5),
                DueDate = DateTime.UtcNow.AddDays(9),
                Status = LoanStatus.Active
            },
            new Loan
            {
                BookId = persistedBooks[1].Id, // Design Patterns
                UserId = persistedStudents[1].Id,
                BorrowDate = DateTime.UtcNow.AddDays(-7),
                DueDate = DateTime.UtcNow.AddDays(7),
                Status = LoanStatus.Active
            },
            new Loan
            {
                BookId = persistedBooks[3].Id, // Algorithms
                UserId = persistedStudents[2].Id,
                BorrowDate = DateTime.UtcNow.AddDays(-10),
                DueDate = DateTime.UtcNow.AddDays(4),
                Status = LoanStatus.Active
            },
            // Faculty loans (30 days duration)
            new Loan
            {
                BookId = persistedBooks[4].Id, // AI Book
                UserId = persistedFaculty[0].Id,
                BorrowDate = DateTime.UtcNow.AddDays(-15),
                DueDate = DateTime.UtcNow.AddDays(15),
                Status = LoanStatus.Active
            },
            new Loan
            {
                BookId = persistedBooks[8].Id, // Art of Computer Programming
                UserId = persistedFaculty[1].Id,
                BorrowDate = DateTime.UtcNow.AddDays(-20),
                DueDate = DateTime.UtcNow.AddDays(10),
                Status = LoanStatus.Active
            },
        };

        // Create Overdue Loans
        var overdueLoans = new List<Loan>
        {
            // Student overdue (5 days overdue)
            new Loan
            {
                BookId = persistedBooks[6].Id, // Operating Systems
                UserId = persistedStudents[3].Id,
                BorrowDate = DateTime.UtcNow.AddDays(-20),
                DueDate = DateTime.UtcNow.AddDays(-5),
                Status = LoanStatus.Overdue
            },
            // Faculty overdue (10 days overdue, past grace period)
            new Loan
            {
                BookId = persistedBooks[10].Id, // Head First Design Patterns
                UserId = persistedFaculty[2].Id,
                BorrowDate = DateTime.UtcNow.AddDays(-50),
                DueDate = DateTime.UtcNow.AddDays(-10),
                Status = LoanStatus.Overdue
            },
            // Another student overdue
            new Loan
            {
                BookId = persistedBooks[11].Id, // Thinking in Java
                UserId = persistedStudents[4].Id,
                BorrowDate = DateTime.UtcNow.AddDays(-18),
                DueDate = DateTime.UtcNow.AddDays(-3),
                Status = LoanStatus.Overdue
            },
        };

        // Update book availability for active loans
        persistedBooks[0].CheckOut();
        persistedBooks[1].CheckOut();
        persistedBooks[3].CheckOut();
        persistedBooks[4].CheckOut();
        persistedBooks[8].CheckOut();
        
        // Update book availability for overdue loans
        persistedBooks[6].CheckOut();
        persistedBooks[10].CheckOut();
        persistedBooks[11].CheckOut();

        context.Loans.AddRange(activeLoans);
        context.Loans.AddRange(overdueLoans);
        context.SaveChanges();
        
        Console.WriteLine($"✅ Seeded {activeLoans.Count} active loans");
        Console.WriteLine($"✅ Seeded {overdueLoans.Count} overdue loans");

        // Create Fines for overdue loans
        var fines = new List<Fine>();
        
        foreach (var loan in overdueLoans)
        {
            var daysOverdue = (DateTime.UtcNow - loan.DueDate).Days;
            var user = context.Users.Find(loan.UserId);
            
            decimal fineAmount = 0;
            if (user is Student)
            {
                // Student: 5 pesos/day, progressive to 7.5 after 7 days
                if (daysOverdue <= 7)
                    fineAmount = daysOverdue * 5.00m;
                else
                    fineAmount = (7 * 5.00m) + ((daysOverdue - 7) * 7.50m);
            }
            else if (user is Faculty)
            {
                // Faculty: 3-day grace, then 10 pesos/day, progressive to 20 after 14 days
                var chargeableDays = Math.Max(0, daysOverdue - 3);
                if (chargeableDays <= 14)
                    fineAmount = chargeableDays * 10.00m;
                else
                    fineAmount = (14 * 10.00m) + ((chargeableDays - 14) * 20.00m);
            }

            if (fineAmount > 0)
            {
                var fine = new Fine
                {
                    LoanId = loan.Id,
                    Amount = fineAmount,
                    Status = FineStatus.Pending,
                    DateAssessed = DateTime.UtcNow
                };
                fines.Add(fine);
            }
        }

        if (fines.Any())
        {
            context.Fines.AddRange(fines);
            context.SaveChanges();
            Console.WriteLine($"✅ Seeded {fines.Count} fines (Total: ${fines.Sum(f => f.Amount):F2})");
        }

        Console.WriteLine("\n🎉 Database seeding completed successfully!");
        Console.WriteLine($"   - {books.Count} books");
        Console.WriteLine($"   - {users.Count} users");
        Console.WriteLine($"   - {activeLoans.Count + overdueLoans.Count} loans");
        Console.WriteLine($"   - {fines.Count} fines");
    }
}
