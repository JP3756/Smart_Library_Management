namespace SmartLibraryAPI.Models
{
    /// <summary>
    /// Faculty class - demonstrates INHERITANCE and POLYMORPHISM
    /// </summary>
    public class Faculty : User
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

        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;

        // Polymorphic implementation - Faculty have higher limits
        public override int GetMaxBorrowLimit() => 10;
        public override int GetMaxBorrowDays() => 30; // 1 month
        public override decimal GetDailyFineRate() => 10.00m; // 10 pesos per day
        public override string GetUserType() => "Faculty";

        public Faculty() : base()
        {
        }
    }
}
