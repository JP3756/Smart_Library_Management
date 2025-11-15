using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Data;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;
using SmartLibraryAPI.Strategies;

namespace SmartLibraryAPI.Services
{
    /// <summary>
    /// Loan service implementation (Business Logic Layer)
    /// Demonstrates SOLID - Dependency Inversion and Single Responsibility
    /// Uses STRATEGY PATTERN for borrowing rules
    /// </summary>
    public class LoanService : ILoanService
    {
        private readonly LibraryDbContext _context;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public LoanService(
            LibraryDbContext context,
            IBookRepository bookRepository,
            IUserRepository userRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Loan>> GetAllLoansAsync()
        {
            return await _context.Loans
                .Include(l => l.User)
                .Include(l => l.Book)
                .ToListAsync();
        }

        public async Task<Loan?> GetLoanByIdAsync(int id)
        {
            return await _context.Loans
                .Include(l => l.User)
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Loan> BorrowBookAsync(int userId, int bookId)
        {
            // Get user and book
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                throw new ArgumentException("Book not found");

            // Check if book is available
            if (!book.IsAvailable)
                throw new InvalidOperationException("Book is not available");

            // Check if user can borrow
            if (!await CanUserBorrowAsync(userId))
                throw new InvalidOperationException("User has reached borrowing limit");

            // Get borrowing strategy based on user type
            IBorrowingStrategy strategy = GetBorrowingStrategy(user);

            // Create loan
            var loan = new Loan
            {
                UserId = userId,
                BookId = bookId,
                BorrowDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(strategy.GetMaxBorrowDays()),
                Status = LoanStatus.Active
            };

            // Update book availability
            book.CheckOut();

            // Save changes
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return loan;
        }

        public async Task<Loan> ReturnBookAsync(int loanId)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .Include(l => l.Fine)
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null)
                throw new ArgumentException("Loan not found");

            if (loan.Status == LoanStatus.Returned)
                throw new InvalidOperationException("Book already returned");

            // Set return date
            loan.ReturnDate = DateTime.UtcNow;
            loan.UpdateStatus();

            // Return book to inventory
            loan.Book.CheckIn();

            // Calculate fine if overdue
            if (loan.DaysOverdue > 0)
            {
                var fine = await CalculateFineAsync(loanId);
                // Fine is already created in CalculateFineAsync
            }

            await _context.SaveChangesAsync();

            return loan;
        }

        public async Task<IEnumerable<Loan>> GetUserLoansAsync(int userId)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Fine)
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.BorrowDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetActiveLoansAsync()
        {
            return await _context.Loans
                .Include(l => l.User)
                .Include(l => l.Book)
                .Where(l => l.Status == LoanStatus.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
        {
            var today = DateTime.UtcNow;
            return await _context.Loans
                .Include(l => l.User)
                .Include(l => l.Book)
                .Where(l => l.Status == LoanStatus.Active && l.DueDate < today)
                .ToListAsync();
        }

        public async Task<Fine?> CalculateFineAsync(int loanId)
        {
            var loan = await _context.Loans
                .Include(l => l.User)
                .Include(l => l.Fine)
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null)
                throw new ArgumentException("Loan not found");

            // If already has a fine, return it
            if (loan.Fine != null)
                return loan.Fine;

            // Check if overdue
            if (loan.DaysOverdue <= 0)
                return null;

            // Get strategy and calculate fine
            IBorrowingStrategy strategy = GetBorrowingStrategy(loan.User);
            decimal fineAmount = strategy.CalculateFine(loan.DaysOverdue);

            // Create fine
            var fine = new Fine
            {
                LoanId = loanId,
                Amount = fineAmount,
                Status = FineStatus.Pending,
                Remarks = $"Overdue by {loan.DaysOverdue} days"
            };

            _context.Fines.Add(fine);
            await _context.SaveChangesAsync();

            return fine;
        }

        public async Task<bool> CanUserBorrowAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            var activeLoansCount = await _context.Loans
                .CountAsync(l => l.UserId == userId && l.Status == LoanStatus.Active);

            IBorrowingStrategy strategy = GetBorrowingStrategy(user);
            return strategy.CanBorrow(user, activeLoansCount);
        }

        public async Task<bool> DeleteLoanAsync(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null) return false;

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return true;
        }

        // Helper method to get appropriate strategy (STRATEGY PATTERN)
        private IBorrowingStrategy GetBorrowingStrategy(User user)
        {
            return user switch
            {
                Student => new StudentBorrowingStrategy(),
                Faculty => new FacultyBorrowingStrategy(),
                _ => throw new ArgumentException("Unknown user type")
            };
        }
    }
}
