using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Data;
using SmartLibraryAPI.DTOs;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Controllers
{
    /// <summary>
    /// Reports API Controller - Generates statistics and reports
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public ReportsController(LibraryDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get library statistics
        /// </summary>
        [HttpGet("stats")]
        public async Task<ActionResult<LibraryStatsDto>> GetLibraryStats()
        {
            var stats = new LibraryStatsDto
            {
                TotalBooks = await _context.Books.CountAsync(),
                TotalUsers = await _context.Users.CountAsync(),
                ActiveLoans = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Active),
                OverdueLoans = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Overdue || 
                    (l.Status == LoanStatus.Active && l.DueDate < DateTime.UtcNow)),
                TotalStudents = await _context.Students.CountAsync(),
                TotalFaculty = await _context.Faculty.CountAsync(),
                TotalFinesCollected = await _context.Fines
                    .Where(f => f.Status == FineStatus.Paid)
                    .SumAsync(f => f.Amount),
                PendingFines = await _context.Fines
                    .Where(f => f.Status == FineStatus.Pending)
                    .SumAsync(f => f.Amount)
            };

            return Ok(stats);
        }

        /// <summary>
        /// Get borrowing report
        /// </summary>
        [HttpGet("borrowing")]
        public async Task<ActionResult<BorrowingReportDto>> GetBorrowingReport()
        {
            var totalBorrowings = await _context.Loans.CountAsync();
            var currentlyBorrowed = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Active);
            var returned = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Returned);
            var overdue = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Overdue || 
                (l.Status == LoanStatus.Active && l.DueDate < DateTime.UtcNow));

            // Top borrowers - Load data first, then call GetUserType() in memory
            var loansByUser = await _context.Loans
                .Include(l => l.User)
                .ToListAsync();
            
            var topBorrowers = loansByUser
                .GroupBy(l => new { l.UserId, l.User.Name })
                .Select(g => new TopBorrowerDto
                {
                    UserName = g.Key.Name,
                    UserType = g.First().User.GetUserType(),
                    BorrowCount = g.Count()
                })
                .OrderByDescending(x => x.BorrowCount)
                .Take(10)
                .ToList();

            // Popular books
            var popularBooks = await _context.Loans
                .Include(l => l.Book)
                .GroupBy(l => new { l.BookId, l.Book.Title, l.Book.Author })
                .Select(g => new PopularBookDto
                {
                    Title = g.Key.Title,
                    Author = g.Key.Author,
                    BorrowCount = g.Count()
                })
                .OrderByDescending(x => x.BorrowCount)
                .Take(10)
                .ToListAsync();

            var report = new BorrowingReportDto
            {
                TotalBorrowings = totalBorrowings,
                CurrentlyBorrowed = currentlyBorrowed,
                Returned = returned,
                Overdue = overdue,
                TopBorrowers = topBorrowers,
                PopularBooks = popularBooks
            };

            return Ok(report);
        }

        /// <summary>
        /// Get books by category statistics
        /// </summary>
        [HttpGet("books-by-category")]
        public async Task<ActionResult<object>> GetBooksByCategory()
        {
            var booksByCategory = await _context.Books
                .GroupBy(b => b.Category)
                .Select(g => new
                {
                    category = g.Key,
                    totalBooks = g.Count(),
                    availableBooks = g.Sum(b => b.AvailableCopies),
                    borrowedBooks = g.Sum(b => b.TotalCopies - b.AvailableCopies)
                })
                .ToListAsync();

            return Ok(booksByCategory);
        }

        /// <summary>
        /// Get user type statistics
        /// </summary>
        [HttpGet("user-statistics")]
        public async Task<ActionResult<object>> GetUserStatistics()
        {
            var students = await _context.Students.ToListAsync();
            var faculty = await _context.Faculty.ToListAsync();

            var studentStats = new
            {
                type = "Student",
                total = students.Count,
                active = students.Count(s => s.IsActive),
                currentlyBorrowing = students.Sum(s => s.Loans.Count(l => l.Status == LoanStatus.Active)),
                maxBorrowLimit = students.FirstOrDefault()?.GetMaxBorrowLimit() ?? 3
            };

            var facultyStats = new
            {
                type = "Faculty",
                total = faculty.Count,
                active = faculty.Count(f => f.IsActive),
                currentlyBorrowing = faculty.Sum(f => f.Loans.Count(l => l.Status == LoanStatus.Active)),
                maxBorrowLimit = faculty.FirstOrDefault()?.GetMaxBorrowLimit() ?? 10
            };

            return Ok(new { students = studentStats, faculty = facultyStats });
        }

        /// <summary>
        /// Get fine statistics
        /// </summary>
        [HttpGet("fines")]
        public async Task<ActionResult<object>> GetFineStatistics()
        {
            var fines = await _context.Fines.ToListAsync();

            var stats = new
            {
                totalFines = fines.Count,
                pendingFines = fines.Count(f => f.Status == FineStatus.Pending),
                paidFines = fines.Count(f => f.Status == FineStatus.Paid),
                waivedFines = fines.Count(f => f.Status == FineStatus.Waived),
                totalAmount = fines.Sum(f => f.Amount),
                collectedAmount = fines.Where(f => f.Status == FineStatus.Paid).Sum(f => f.Amount),
                pendingAmount = fines.Where(f => f.Status == FineStatus.Pending).Sum(f => f.Amount)
            };

            return Ok(stats);
        }

        /// <summary>
        /// Get monthly borrowing trend (last 12 months)
        /// </summary>
        [HttpGet("monthly-trend")]
        public async Task<ActionResult<object>> GetMonthlyTrend()
        {
            var oneYearAgo = DateTime.UtcNow.AddYears(-1);
            
            var monthlyData = await _context.Loans
                .Where(l => l.BorrowDate >= oneYearAgo)
                .GroupBy(l => new { l.BorrowDate.Year, l.BorrowDate.Month })
                .Select(g => new
                {
                    year = g.Key.Year,
                    month = g.Key.Month,
                    borrowings = g.Count(),
                    returns = g.Count(l => l.ReturnDate.HasValue)
                })
                .OrderBy(x => x.year)
                .ThenBy(x => x.month)
                .ToListAsync();

            return Ok(monthlyData);
        }
    }
}
