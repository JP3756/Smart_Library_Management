namespace SmartLibraryAPI.Models
{
    public enum FineStatus
    {
        Pending,
        Paid,
        Waived
    }

    /// <summary>
    /// Fine entity - represents penalties for overdue books
    /// </summary>
    public class Fine
    {
        private decimal _amount;

        public int Id { get; set; }
        
        // Foreign key
        public int LoanId { get; set; }
        public virtual Loan Loan { get; set; } = null!;

        public decimal Amount
        {
            get => _amount;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Fine amount cannot be negative");
                _amount = value;
            }
        }

        public FineStatus Status { get; set; }
        public DateTime DateAssessed { get; set; }
        public DateTime? DatePaid { get; set; }
        public string? Remarks { get; set; }

        // Business logic
        public void MarkAsPaid()
        {
            Status = FineStatus.Paid;
            DatePaid = DateTime.UtcNow;
        }

        public void Waive(string reason)
        {
            Status = FineStatus.Waived;
            Remarks = reason;
        }

        public Fine()
        {
            Status = FineStatus.Pending;
            DateAssessed = DateTime.UtcNow;
        }
    }
}
