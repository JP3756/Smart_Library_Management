namespace SmartLibraryAPI.Models
{
    /// <summary>
    /// Librarian class - demonstrates INHERITANCE and POLYMORPHISM
    /// Librarians have full system access and highest privileges
    /// </summary>
    public class Librarian : User
    {
        private string _employeeId = string.Empty;

        public string EmployeeId
        {
            get => _employeeId;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Employee ID cannot be empty");
                _employeeId = value;
            }
        }

        public string Department { get; set; } = "Library Administration";
        public string Position { get; set; } = "Librarian";

        // Polymorphic implementation - Librarians have unlimited access
        public override int GetMaxBorrowLimit() => 50; // Can borrow many books for library management
        public override int GetMaxBorrowDays() => 90; // 3 months
        public override decimal GetDailyFineRate() => 0.00m; // No fines for librarians
        public override string GetUserType() => "Librarian";

        public Librarian() : base()
        {
        }
    }
}
