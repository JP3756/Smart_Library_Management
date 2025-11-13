namespace SmartLibraryAPI.Models
{
    public enum LoanStatus
    {
        Active,
        Returned,
        Overdue
    }

    /// <summary>
    /// Loan entity - represents a book borrowing transaction
    /// </summary>
    public class Loan
    {
        public int Id { get; set; }
        
        // Foreign keys
        public int UserId { get; set; }
        public int BookId { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Book Book { get; set; } = null!;

        // Loan details
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public LoanStatus Status { get; set; }

        // Computed properties
        public bool IsOverdue => Status == LoanStatus.Active && DateTime.UtcNow > DueDate;
        public int DaysOverdue => IsOverdue ? (DateTime.UtcNow - DueDate).Days : 0;

        // Fine relationship
        public virtual Fine? Fine { get; set; }

        // Business logic
        public void UpdateStatus()
        {
            if (ReturnDate.HasValue)
            {
                Status = LoanStatus.Returned;
            }
            else if (DateTime.UtcNow > DueDate)
            {
                Status = LoanStatus.Overdue;
            }
            else
            {
                Status = LoanStatus.Active;
            }
        }

        public Loan()
        {
            BorrowDate = DateTime.UtcNow;
            Status = LoanStatus.Active;
        }
    }
}
