namespace SmartLibraryAPI.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string UserType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int BooksIssued { get; set; }
        public int MaxBorrowLimit { get; set; }
    }

    public class CreateUserDto
    {
        public string UserType { get; set; } = string.Empty; // "student" or "faculty"
        public string Type { get; set; } = string.Empty; // Alternative name
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        
        // Student-specific
        public string? StudentId { get; set; }
        public string? Program { get; set; }
        public int? YearLevel { get; set; }
        
        // Faculty-specific
        public string? EmployeeId { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public string? Specialization { get; set; }
    }

    public class UpdateUserDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

    public class StudentDto : UserDto
    {
        public string StudentId { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public int YearLevel { get; set; }
        public string Department { get; set; } = string.Empty;
    }

    public class FacultyDto : UserDto
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
    }
}
