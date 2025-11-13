using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Data;

public static class DatabaseSeeder
{
    public static void SeedDatabase(LibraryDbContext context)
    {
        // Only seed if database is empty
        if (context.Books.Any() || context.Users.Any())
            return;

        // Seed Books
        var books = new List<Book>
        {
            new Book { Title = "The Art of War", Author = "Sun Tzu", ISBN = "978-1599869773", Category = "Strategy", PublicationYear = 2005, TotalCopies = 3, AvailableCopies = 2 },
            new Book { Title = "48 Laws of Power", Author = "Robert Greene", ISBN = "978-0140280197", Category = "Self-Help", PublicationYear = 2000, TotalCopies = 2, AvailableCopies = 1 },
            new Book { Title = "Clean Code", Author = "Robert C. Martin", ISBN = "978-0132350884", Category = "Programming", PublicationYear = 2008, TotalCopies = 5, AvailableCopies = 3 },
            new Book { Title = "Design Patterns", Author = "Gang of Four", ISBN = "978-0201633610", Category = "Programming", PublicationYear = 1994, TotalCopies = 3, AvailableCopies = 2 },
            new Book { Title = "Introduction to Algorithms", Author = "Thomas H. Cormen", ISBN = "978-0262033848", Category = "Computer Science", PublicationYear = 2009, TotalCopies = 4, AvailableCopies = 0 },
            new Book { Title = "The Pragmatic Programmer", Author = "Andrew Hunt", ISBN = "978-0135957059", Category = "Programming", PublicationYear = 2019, TotalCopies = 5, AvailableCopies = 5 },
            new Book { Title = "Artificial Intelligence: A Modern Approach", Author = "Stuart Russell", ISBN = "978-0134610993", Category = "AI", PublicationYear = 2020, TotalCopies = 2, AvailableCopies = 1 },
            new Book { Title = "Database System Concepts", Author = "Abraham Silberschatz", ISBN = "978-0078022159", Category = "Database", PublicationYear = 2019, TotalCopies = 4, AvailableCopies = 3 },
            new Book { Title = "Operating System Concepts", Author = "Abraham Silberschatz", ISBN = "978-1118063330", Category = "Computer Science", PublicationYear = 2018, TotalCopies = 3, AvailableCopies = 2 },
            new Book { Title = "Computer Networks", Author = "Andrew S. Tanenbaum", ISBN = "978-0132126953", Category = "Computer Science", PublicationYear = 2010, TotalCopies = 4, AvailableCopies = 2 }
        };

        context.Books.AddRange(books);

        // Seed Users
        var users = new List<User>
        {
            new Student { Name = "JP Cabaluna", Email = "jp.cabaluna@university.edu", StudentId = "STU2024100" },
            new Student { Name = "Jake Sucgang", Email = "jake.sucgang@university.edu", StudentId = "STU2024101" },
            new Student { Name = "John Doe", Email = "john.doe@university.edu", StudentId = "STU2024001" },
            new Student { Name = "Mike Johnson", Email = "mike.j@university.edu", StudentId = "STU2024012" },
            new Student { Name = "Sarah Williams", Email = "sarah.w@university.edu", StudentId = "STU2023098" },
            new Faculty { Name = "Jane Smith", Email = "jane.smith@university.edu", EmployeeId = "FAC2023045" },
            new Faculty { Name = "Robert Brown", Email = "robert.brown@university.edu", EmployeeId = "FAC2022032" },
        };

        context.Users.AddRange(users);

        // Save to database
        context.SaveChanges();

        // Create some loans (need to work with persisted entities)
        var persistedBooks = context.Books.ToList();
        var persistedUsers = context.Users.ToList();

        var loans = new List<Loan>
        {
            new Loan
            {
                BookId = persistedBooks[0].Id, // The Art of War
                UserId = persistedUsers[0].Id, // First student
                BorrowDate = DateTime.Now.AddDays(-10),
                DueDate = DateTime.Now.AddDays(4),
                Status = LoanStatus.Active
            },
            new Loan
            {
                BookId = persistedBooks[1].Id, // 48 Laws of Power
                UserId = persistedUsers[1].Id, // Second student
                BorrowDate = DateTime.Now.AddDays(-12),
                DueDate = DateTime.Now.AddDays(2),
                Status = LoanStatus.Active
            },
            new Loan
            {
                BookId = persistedBooks[4].Id, // Introduction to Algorithms
                UserId = persistedUsers[5].Id, // First faculty
                BorrowDate = DateTime.Now.AddDays(-25),
                DueDate = DateTime.Now.AddDays(-15),
                Status = LoanStatus.Overdue
            }
        };

        // Update book availability
        persistedBooks[0].CheckOut();
        persistedBooks[1].CheckOut();
        persistedBooks[4].CheckOut();

        context.Loans.AddRange(loans);
        context.SaveChanges();

        Console.WriteLine("✅ Database seeded successfully!");
        Console.WriteLine($"   - {books.Count} books added");
        Console.WriteLine($"   - {users.Count} users added");
        Console.WriteLine($"   - {loans.Count} loans created");
    }
}
