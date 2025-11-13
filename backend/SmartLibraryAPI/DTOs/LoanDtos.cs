namespace SmartLibraryAPI.DTOs
{
    public class LoanDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int DaysOverdue { get; set; }
        public decimal? FineAmount { get; set; }
    }

    public class CreateLoanDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }

    public class ReturnBookDto
    {
        public int LoanId { get; set; }
    }
}
