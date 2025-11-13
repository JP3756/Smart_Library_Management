using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Strategies
{
    /// <summary>
    /// Student borrowing strategy (STRATEGY PATTERN)
    /// Implements specific borrowing rules for students
    /// </summary>
    public class StudentBorrowingStrategy : IBorrowingStrategy
    {
        private const int MAX_BORROW_LIMIT = 3;
        private const int MAX_BORROW_DAYS = 14;
        private const decimal DAILY_FINE_RATE = 5.00m;

        public int GetMaxBorrowLimit() => MAX_BORROW_LIMIT;

        public int GetMaxBorrowDays() => MAX_BORROW_DAYS;

        public decimal CalculateFine(int daysOverdue)
        {
            if (daysOverdue <= 0) return 0m;
            
            // Progressive fine: higher rate after 7 days
            if (daysOverdue <= 7)
            {
                return daysOverdue * DAILY_FINE_RATE;
            }
            else
            {
                decimal firstWeekFine = 7 * DAILY_FINE_RATE;
                decimal remainingDays = daysOverdue - 7;
                decimal increasedRate = DAILY_FINE_RATE * 1.5m; // 50% increase
                return firstWeekFine + (remainingDays * increasedRate);
            }
        }

        public bool CanBorrow(User user, int currentLoans)
        {
            return user.IsActive && currentLoans < MAX_BORROW_LIMIT;
        }
    }
}
