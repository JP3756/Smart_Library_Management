using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Factories
{
    /// <summary>
    /// User factory implementation (FACTORY PATTERN)
    /// Creates different types of users based on user type
    /// </summary>
    public class UserFactory : IUserFactory
    {
        public User CreateUser(string userType, string name, string email)
        {
            return userType.ToLower() switch
            {
                "student" => new Student
                {
                    Name = name,
                    Email = email,
                    StudentId = GenerateStudentId(),
                    YearLevel = 1
                },
                "faculty" => new Faculty
                {
                    Name = name,
                    Email = email,
                    EmployeeId = GenerateEmployeeId(),
                    Position = "Instructor"
                },
                _ => throw new ArgumentException($"Invalid user type: {userType}")
            };
        }

        private string GenerateStudentId()
        {
            // Generate a student ID (e.g., 2025-12345)
            var year = DateTime.Now.Year;
            var random = new Random();
            var number = random.Next(10000, 99999);
            return $"{year}-{number}";
        }

        private string GenerateEmployeeId()
        {
            // Generate an employee ID (e.g., EMP-12345)
            var random = new Random();
            var number = random.Next(10000, 99999);
            return $"EMP-{number}";
        }
    }
}
