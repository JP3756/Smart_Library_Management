using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Interfaces
{
    /// <summary>
    /// Loan service interface (Business logic layer)
    /// Demonstrates SOLID - Dependency Inversion Principle
    /// </summary>
    public interface ILoanService
    {
        Task<Loan> BorrowBookAsync(int userId, int bookId);
        Task<Loan> ReturnBookAsync(int loanId);
        Task<IEnumerable<Loan>> GetUserLoansAsync(int userId);
        Task<IEnumerable<Loan>> GetActiveLoansAsync();
        Task<IEnumerable<Loan>> GetOverdueLoansAsync();
        Task<Fine?> CalculateFineAsync(int loanId);
        Task<bool> CanUserBorrowAsync(int userId);
    }
}
