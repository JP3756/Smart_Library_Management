using Xunit;
using SmartLibraryAPI.Factories;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Tests.Factories
{
    /// <summary>
    /// Unit tests for UserFactory demonstrating FACTORY PATTERN
    /// </summary>
    public class UserFactoryTests
    {
        [Fact]
        public void UserFactory_ShouldCreateStudent_WhenTypeIsStudent()
        {
            // Arrange - Demonstrate FACTORY PATTERN
            var factory = new UserFactory();

            // Act
            var user = factory.CreateUser("student", "Test Student", "test@student.edu");

            // Assert
            Assert.NotNull(user);
            Assert.IsType<Student>(user);
            Assert.Equal("Test Student", user.Name);
            Assert.Equal("test@student.edu", user.Email);
            Assert.Equal("Student", user.GetUserType());
        }

        [Fact]
        public void UserFactory_ShouldCreateFaculty_WhenTypeIsFaculty()
        {
            // Arrange - Demonstrate FACTORY PATTERN
            var factory = new UserFactory();

            // Act
            var user = factory.CreateUser("faculty", "Dr. Test Faculty", "test@faculty.edu");

            // Assert
            Assert.NotNull(user);
            Assert.IsType<Faculty>(user);
            Assert.Equal("Dr. Test Faculty", user.Name);
            Assert.Equal("test@faculty.edu", user.Email);
            Assert.Equal("Faculty", user.GetUserType());
        }

        [Fact]
        public void UserFactory_ShouldThrowException_WhenTypeIsInvalid()
        {
            // Arrange
            var factory = new UserFactory();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                factory.CreateUser("admin", "Test User", "test@test.edu")
            );

            Assert.Contains("Invalid user type", exception.Message);
        }

        [Fact]
        public void UserFactory_ShouldCreateUsersWithDifferentTypes()
        {
            // Arrange - Demonstrate FACTORY creates different object types
            var factory = new UserFactory();

            // Act
            var student = factory.CreateUser("student", "Student", "student@edu");
            var faculty = factory.CreateUser("faculty", "Faculty", "faculty@edu");

            // Assert - Demonstrate POLYMORPHISM
            Assert.IsType<Student>(student);
            Assert.IsType<Faculty>(faculty);
            Assert.NotEqual(student.GetMaxBorrowLimit(), faculty.GetMaxBorrowLimit());
            Assert.NotEqual(student.GetMaxBorrowDays(), faculty.GetMaxBorrowDays());
        }
    }
}
