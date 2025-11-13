namespace SmartLibraryAPI.Models
{
    /// <summary>
    /// Book entity - demonstrates ENCAPSULATION
    /// </summary>
    public class Book
    {
        private string _isbn;
        private string _title;
        private int _totalCopies;
        private int _availableCopies;

        public int Id { get; set; }

        // Validated ISBN (Encapsulation)
        public string ISBN
        {
            get => _isbn;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ISBN cannot be empty");
                _isbn = value;
            }
        }

        // Validated Title (Encapsulation)
        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Title cannot be empty");
                _title = value;
            }
        }

        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Encapsulated inventory management
        public int TotalCopies
        {
            get => _totalCopies;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Total copies cannot be negative");
                _totalCopies = value;
            }
        }

        public int AvailableCopies
        {
            get => _availableCopies;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Available copies cannot be negative");
                if (value > TotalCopies)
                    throw new ArgumentException("Available copies cannot exceed total copies");
                _availableCopies = value;
            }
        }

        // Computed property
        public bool IsAvailable => AvailableCopies > 0;
        public string Status => IsAvailable ? "Available" : "Out of Stock";

        // Navigation properties
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        // Business logic methods
        public bool CheckOut()
        {
            if (!IsAvailable) return false;
            
            AvailableCopies--;
            return true;
        }

        public void CheckIn()
        {
            if (AvailableCopies >= TotalCopies)
                throw new InvalidOperationException("Cannot return more copies than total");
            
            AvailableCopies++;
        }

        public Book()
        {
            _isbn = string.Empty;
            _title = string.Empty;
        }
    }
}
