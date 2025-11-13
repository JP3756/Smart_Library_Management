using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Interfaces
{
    /// <summary>
    /// Strategy interface for borrowing rules (STRATEGY PATTERN)
    /// Demonstrates POLYMORPHISM through strategy pattern
    /// </summary>
    public interface IBorrowingStrategy
    {
        int GetMaxBorrowLimit();
        int GetMaxBorrowDays();
        decimal CalculateFine(int daysOverdue);
        bool CanBorrow(User user, int currentLoans);
    }
}
