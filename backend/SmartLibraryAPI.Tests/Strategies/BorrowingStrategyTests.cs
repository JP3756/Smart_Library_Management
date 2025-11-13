using Xunit;
using SmartLibraryAPI.Strategies;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Tests.Strategies
{
    /// <summary>
    /// Unit tests for Borrowing Strategies demonstrating STRATEGY PATTERN
    /// Different fine calculation strategies for Student vs Faculty
    /// </summary>
    public class BorrowingStrategyTests
    {
        [Fact]
        public void StudentBorrowingStrategy_ShouldCalculateFineCorrectly_NoDaysOverdue()
        {
            // Arrange
            var strategy = new StudentBorrowingStrategy();

            // Act
            var fine = strategy.CalculateFine(0); // No overdue days

            // Assert
            Assert.Equal(0m, fine);
        }

        [Fact]
        public void StudentBorrowingStrategy_ShouldCalculateFineCorrectly_FewDaysOverdue()
        {
            // Arrange
            var strategy = new StudentBorrowingStrategy();

            // Act
            var fine = strategy.CalculateFine(5); // 5 days overdue

            // Assert - 5 days × 5 pesos = 25 pesos
            Assert.Equal(25m, fine);
        }

        [Fact]
        public void StudentBorrowingStrategy_ShouldCalculateFineCorrectly_ProgressiveRate()
        {
            // Arrange
            var strategy = new StudentBorrowingStrategy();

            // Act - After 7 days, rate increases by 50%
            // First 7 days: 7 × 5 = 35 pesos
            // Next 3 days: 3 × 7.5 = 22.5 pesos
            // Total: 57.5 pesos
            var fine = strategy.CalculateFine(10); // 10 days overdue

            // Assert
            Assert.Equal(57.5m, fine);
        }

        [Fact]
        public void FacultyBorrowingStrategy_ShouldCalculateFineCorrectly_NoDaysOverdue()
        {
            // Arrange - Demonstrate POLYMORPHISM with different strategy
            var strategy = new FacultyBorrowingStrategy();

            // Act
            var fine = strategy.CalculateFine(0);

            // Assert
            Assert.Equal(0m, fine);
        }

        [Fact]
        public void FacultyBorrowingStrategy_ShouldCalculateFineCorrectly_FewDaysOverdue()
        {
            // Arrange
            var strategy = new FacultyBorrowingStrategy();

            // Act
            var fine = strategy.CalculateFine(5); // 5 days overdue

            // Assert - 5 days × 10 pesos = 50 pesos
            Assert.Equal(50m, fine);
        }

        [Fact]
        public void FacultyBorrowingStrategy_ShouldCalculateFineCorrectly_ProgressiveRate()
        {
            // Arrange
            var strategy = new FacultyBorrowingStrategy();

            // Act - After 10 days, rate increases by 50%
            // First 10 days: 10 × 10 = 100 pesos
            // Next 5 days: 5 × 15 = 75 pesos
            // Total: 175 pesos
            var fine = strategy.CalculateFine(15); // 15 days overdue

            // Assert
            Assert.Equal(175m, fine);
        }

        [Fact]
        public void BorrowingStrategies_ShouldDemonstratePolymorphism()
        {
            // Arrange - Demonstrate STRATEGY PATTERN and POLYMORPHISM
            var studentStrategy = new StudentBorrowingStrategy();
            var facultyStrategy = new FacultyBorrowingStrategy();

            // Act - Same number of overdue days, different calculations
            var studentFine = studentStrategy.CalculateFine(5);
            var facultyFine = facultyStrategy.CalculateFine(5);

            // Assert - Different strategies produce different results
            Assert.NotEqual(studentFine, facultyFine);
            Assert.Equal(25m, studentFine); // Student: 5 × 5
            Assert.Equal(50m, facultyFine); // Faculty: 5 × 10
        }
    }
}
