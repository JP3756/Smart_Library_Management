namespace SmartLibraryAPI.Models
{
    /// <summary>
    /// Student class - demonstrates INHERITANCE and POLYMORPHISM
    /// </summary>
    public class Student : User
    {
        private string _studentId = string.Empty;

        public string StudentId
        {
            get => _studentId;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Student ID cannot be empty");
                _studentId = value;
            }
        }

        public string Program { get; set; } = string.Empty;
        public int YearLevel { get; set; }
        public string Department { get; set; } = string.Empty;

        // Polymorphic implementation - Students have lower limits
        public override int GetMaxBorrowLimit() => 3;
        public override int GetMaxBorrowDays() => 14; // 2 weeks
        public override decimal GetDailyFineRate() => 5.00m; // 5 pesos per day
        public override string GetUserType() => "Student";

        public Student() : base()
        {
        }
    }
}
