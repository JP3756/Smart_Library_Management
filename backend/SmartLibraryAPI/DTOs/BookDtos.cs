namespace SmartLibraryAPI.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateBookDto
    {
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalCopies { get; set; }
    }

    public class UpdateBookDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public int? PublicationYear { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public int? TotalCopies { get; set; }
        public int? AvailableCopies { get; set; }
    }

    public class UpdateAvailabilityDto
    {
        public int AvailableCopies { get; set; }
    }
}
