using Xunit;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Tests.Models
{
    /// <summary>
    /// Unit tests for User model demonstrating OOP concepts:
    /// - Inheritance (Student and Faculty inherit from User)
    /// - Polymorphism (Different borrowing limits and fine rates)
    /// - Encapsulation (Private fields with validated public properties)
    /// </summary>
    public class UserTests
    {
        [Fact]
        public void Student_ShouldHaveCorrectBorrowingLimits()
        {
            // Arrange & Act
            var student = new Student
            {
                Name = "Test Student",
                Email = "test@student.edu",
                Program = "Computer Science",
                YearLevel = 3,
                Department = "CCS"
            };

            // Assert - Demonstrate POLYMORPHISM
            Assert.Equal(3, student.GetMaxBorrowLimit());
            Assert.Equal(14, student.GetMaxBorrowDays());
            Assert.Equal(5m, student.GetDailyFineRate());
            Assert.Equal("Student", student.GetUserType());
        }

        [Fact]
        public void Faculty_ShouldHaveCorrectBorrowingLimits()
        {
            // Arrange & Act
            var faculty = new Faculty
            {
                Name = "Dr. Test Faculty",
                Email = "test@faculty.edu",
                Department = "Computer Science",
                Position = "Professor",
                Specialization = "Software Engineering"
            };

            // Assert - Demonstrate POLYMORPHISM (Different values than Student)
            Assert.Equal(10, faculty.GetMaxBorrowLimit());
            Assert.Equal(30, faculty.GetMaxBorrowDays());
            Assert.Equal(10m, faculty.GetDailyFineRate());
            Assert.Equal("Faculty", faculty.GetUserType());
        }

        [Fact]
        public void User_EmailValidation_ShouldThrowExceptionForInvalidEmail()
        {
            // Arrange & Act & Assert - Demonstrate ENCAPSULATION with validation
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var student = new Student
                {
                    Name = "Test Student",
                    Email = "invalid-email", // Invalid email
                    Program = "CS",
                    YearLevel = 1,
                    Department = "CCS"
                };
            });

            Assert.Contains("must contain @", exception.Message);
        }

        [Fact]
        public void User_NameValidation_ShouldThrowExceptionForEmptyName()
        {
            // Arrange & Act & Assert - Demonstrate ENCAPSULATION with validation
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var student = new Student
                {
                    Name = "", // Empty name
                    Email = "test@student.edu",
                    Program = "CS",
                    YearLevel = 1,
                    Department = "CCS"
                };
            });

            Assert.Contains("cannot be empty", exception.Message);
        }

        [Fact]
        public void Student_YearLevel_ShouldBeValidRange()
        {
            // Arrange
            var student = new Student
            {
                Name = "Test Student",
                Email = "test@student.edu",
                Program = "CS",
                Department = "CCS"
            };

            // Act
            student.YearLevel = 3;

            // Assert
            Assert.Equal(3, student.YearLevel);
        }

        [Fact]
        public void User_ShouldBeActiveByDefault()
        {
            // Arrange & Act
            var student = new Student
            {
                Name = "Test Student",
                Email = "test@student.edu",
                Program = "CS",
                YearLevel = 1,
                Department = "CCS"
            };

            // Assert
            Assert.True(student.IsActive);
        }

        [Fact]
        public void User_CanBeDeactivated()
        {
            // Arrange
            var student = new Student
            {
                Name = "Test Student",
                Email = "test@student.edu",
                Program = "CS",
                YearLevel = 1,
                Department = "CCS"
            };

            // Act
            student.IsActive = false;

            // Assert
            Assert.False(student.IsActive);
        }
    }
}
