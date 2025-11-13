namespace SmartLibraryAPI.Models
{
    /// <summary>
    /// Base class for all users (Demonstrates INHERITANCE and ENCAPSULATION)
    /// Abstract class following OCP (Open/Closed Principle)
    /// </summary>
    public abstract class User
    {
        // Encapsulation: Private fields with public properties
        private string _email;
        private string _name;

        public int Id { get; set; }

        // Validated property (Encapsulation)
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty");
                _name = value;
            }
        }

        // Validated property (Encapsulation)
        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("Invalid email format");
                _email = value;
            }
        }

        public string? Phone { get; set; }
        public DateTime DateRegistered { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        // Polymorphic methods - to be overridden by derived classes
        public abstract int GetMaxBorrowLimit();
        public abstract int GetMaxBorrowDays();
        public abstract decimal GetDailyFineRate();
        public abstract string GetUserType();

        // Template method pattern
        public bool CanBorrowBook()
        {
            if (!IsActive) return false;
            
            var activeLoans = Loans.Count(l => l.Status == LoanStatus.Active);
            return activeLoans < GetMaxBorrowLimit();
        }

        protected User()
        {
            DateRegistered = DateTime.UtcNow;
        }
    }
}
