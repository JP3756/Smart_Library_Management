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

            // Act - Faculty has 3-day grace period
            var fine = strategy.CalculateFine(5); // 5 days overdue, 2 chargeable (5-3)

            // Assert - 2 days × 10 pesos = 20 pesos
            Assert.Equal(20m, fine);
        }

        [Fact]
        public void FacultyBorrowingStrategy_ShouldCalculateFineCorrectly_ProgressiveRate()
        {
            // Arrange
            var strategy = new FacultyBorrowingStrategy();

            // Act - 3 day grace + 14 days normal rate + progressive
            // 20 days overdue - 3 grace = 17 chargeable days
            // First 14 days: 14 × 10 = 140 pesos
            // Next 3 days: 3 × 20 = 60 pesos (doubled rate)
            // Total: 200 pesos
            var fine = strategy.CalculateFine(20); // 20 days overdue

            // Assert
            Assert.Equal(200m, fine);
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
            Assert.Equal(25m, studentFine); // Student: 5 days × 5 pesos (no grace period)
            Assert.Equal(20m, facultyFine); // Faculty: (5-3 grace) = 2 days × 10 pesos
        }
    }
}
