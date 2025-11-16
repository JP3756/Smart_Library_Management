using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Strategies
{
    /// <summary>
    /// Faculty borrowing strategy (STRATEGY PATTERN)
    /// Implements specific borrowing rules for faculty members
    /// </summary>
    public class FacultyBorrowingStrategy : IBorrowingStrategy
    {
        private const int MAX_BORROW_LIMIT = 10;
        private const int MAX_BORROW_DAYS = 30;
        private const decimal DAILY_FINE_RATE = 10.00m;
        private const int GRACE_PERIOD_DAYS = 7;

        public int GetMaxBorrowLimit() => MAX_BORROW_LIMIT;

        public int GetMaxBorrowDays() => MAX_BORROW_DAYS;

        public decimal CalculateFine(int daysOverdue)
        {
            if (daysOverdue <= 0) return 0m;
            
            // Faculty get 7 days (1 week) grace period
            int chargeableDays = Math.Max(0, daysOverdue - GRACE_PERIOD_DAYS);
            
            if (chargeableDays == 0) return 0m;
            
            // Progressive fine: higher rate after 14 days
            if (chargeableDays <= 14)
            {
                return chargeableDays * DAILY_FINE_RATE;
            }
            else
            {
                decimal firstTwoWeeksFine = 14 * DAILY_FINE_RATE;
                decimal remainingDays = chargeableDays - 14;
                decimal increasedRate = DAILY_FINE_RATE * 2m; // 100% increase
                return firstTwoWeeksFine + (remainingDays * increasedRate);
            }
        }

        public bool CanBorrow(User user, int currentLoans)
        {
            return user.IsActive && currentLoans < MAX_BORROW_LIMIT;
        }
    }
}
