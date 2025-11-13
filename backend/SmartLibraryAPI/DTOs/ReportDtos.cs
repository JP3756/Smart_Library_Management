namespace SmartLibraryAPI.DTOs
{
    public class LibraryStatsDto
    {
        public int TotalBooks { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveLoans { get; set; }
        public int OverdueLoans { get; set; }
        public int TotalStudents { get; set; }
        public int TotalFaculty { get; set; }
        public decimal TotalFinesCollected { get; set; }
        public decimal PendingFines { get; set; }
    }

    public class BorrowingReportDto
    {
        public int TotalBorrowings { get; set; }
        public int CurrentlyBorrowed { get; set; }
        public int Returned { get; set; }
        public int Overdue { get; set; }
        public List<TopBorrowerDto> TopBorrowers { get; set; } = new();
        public List<PopularBookDto> PopularBooks { get; set; } = new();
    }

    public class TopBorrowerDto
    {
        public string UserName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public int BorrowCount { get; set; }
    }

    public class PopularBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int BorrowCount { get; set; }
    }
}
