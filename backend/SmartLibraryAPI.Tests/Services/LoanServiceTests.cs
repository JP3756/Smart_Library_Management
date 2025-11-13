using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Services;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;
using SmartLibraryAPI.Data;

namespace SmartLibraryAPI.Tests.Services
{
    /// <summary>
    /// Unit tests for LoanService demonstrating:
    /// - REPOSITORY PATTERN usage
    /// - STRATEGY PATTERN integration
    /// - Dependency Injection
    /// - Mocking for unit testing
    /// </summary>
    public class LoanServiceTests
    {
        private LibraryDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new LibraryDbContext(options);
        }
        [Fact]
        public async Task BorrowBookAsync_ShouldCreateLoan_WhenValidRequest()
        {
            // Arrange - Demonstrate MOCKING and DEPENDENCY INJECTION
            var mockBookRepo = new Mock<IBookRepository>();
            var mockUserRepo = new Mock<IUserRepository>();

            var student = new Student
            {
                Id = 1,
                Name = "Test Student",
                Email = "test@student.edu",
                IsActive = true,
                Program = "CS",
                YearLevel = 1,
                Department = "CCS"
            };

            var book = new Book
            {
                Id = 1,
                ISBN = "978-0-13-468599-1",
                Title = "Test Book",
                Author = "Test Author",
                Publisher = "Test Publisher",
                PublicationYear = 2020,
                Category = "Fiction",
                TotalCopies = 5,
                AvailableCopies = 5
            };

            mockUserRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(student);
            mockUserRepo.Setup(r => r.GetUserWithLoansAsync(1)).ReturnsAsync(student);
            mockBookRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);
            mockBookRepo.Setup(r => r.UpdateAsync(It.IsAny<Book>())).ReturnsAsync(book);

            var context = GetInMemoryDbContext();
            var loanService = new LoanService(context, mockBookRepo.Object, mockUserRepo.Object);

            // Act
            var loan = await loanService.BorrowBookAsync(1, 1);

            // Assert - Verify STRATEGY PATTERN sets correct due date
            Assert.NotNull(loan);
            Assert.Equal(1, loan.UserId);
            Assert.Equal(1, loan.BookId);
            Assert.Equal(LoanStatus.Active, loan.Status);
            Assert.Equal(14, (loan.DueDate - loan.BorrowDate).Days); // Student: 14 days
        }

        [Fact]
        public async Task BorrowBookAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var mockBookRepo = new Mock<IBookRepository>();
            var mockUserRepo = new Mock<IUserRepository>();

            mockUserRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((User?)null);

            var context = GetInMemoryDbContext();
            var loanService = new LoanService(context, mockBookRepo.Object, mockUserRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                loanService.BorrowBookAsync(999, 1)
            );
        }

        [Fact]
        public async Task BorrowBookAsync_ShouldThrowException_WhenBookNotFound()
        {
            // Arrange
            var mockBookRepo = new Mock<IBookRepository>();
            var mockUserRepo = new Mock<IUserRepository>();

            var student = new Student
            {
                Id = 1,
                Name = "Test Student",
                Email = "test@student.edu",
                IsActive = true,
                Program = "CS",
                YearLevel = 1,
                Department = "CCS"
            };

            mockUserRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(student);
            mockUserRepo.Setup(r => r.GetUserWithLoansAsync(1)).ReturnsAsync(student);
            mockBookRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Book?)null);

            var context = GetInMemoryDbContext();
            var loanService = new LoanService(context, mockBookRepo.Object, mockUserRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                loanService.BorrowBookAsync(1, 999)
            );
        }

        [Fact]
        public async Task BorrowBookAsync_ShouldThrowException_WhenUserInactive()
        {
            // Arrange
            var mockBookRepo = new Mock<IBookRepository>();
            var mockUserRepo = new Mock<IUserRepository>();

            var student = new Student
            {
                Id = 1,
                Name = "Test Student",
                Email = "test@student.edu",
                IsActive = false, // Inactive user
                Program = "CS",
                YearLevel = 1,
                Department = "CCS"
            };

            mockUserRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(student);

            var context = GetInMemoryDbContext();
            var loanService = new LoanService(context, mockBookRepo.Object, mockUserRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                loanService.BorrowBookAsync(1, 1)
            );
        }

        [Fact]
        public async Task CanUserBorrowAsync_ShouldReturnTrue_WhenUnderLimit()
        {
            // Arrange
            var mockBookRepo = new Mock<IBookRepository>();
            var mockUserRepo = new Mock<IUserRepository>();

            var student = new Student
            {
                Id = 1,
                Name = "Test Student",
                Email = "test@student.edu",
                IsActive = true,
                Program = "CS",
                YearLevel = 1,
                Department = "CCS",
                Loans = new List<Loan>
                {
                    new Loan { Status = LoanStatus.Active },
                    new Loan { Status = LoanStatus.Active }
                }
            };

            mockUserRepo.Setup(r => r.GetUserWithLoansAsync(1)).ReturnsAsync(student);

            var context = GetInMemoryDbContext();
            var loanService = new LoanService(context, mockBookRepo.Object, mockUserRepo.Object);

            // Act
            var canBorrow = await loanService.CanUserBorrowAsync(1);

            // Assert - Student limit is 3, currently has 2
            Assert.True(canBorrow);
        }

        [Fact]
        public async Task CanUserBorrowAsync_ShouldReturnFalse_WhenAtLimit()
        {
            // Arrange
            var mockBookRepo = new Mock<IBookRepository>();
            var mockUserRepo = new Mock<IUserRepository>();

            var student = new Student
            {
                Id = 1,
                Name = "Test Student",
                Email = "test@student.edu",
                IsActive = true,
                Program = "CS",
                YearLevel = 1,
                Department = "CCS",
                Loans = new List<Loan>
                {
                    new Loan { Status = LoanStatus.Active },
                    new Loan { Status = LoanStatus.Active },
                    new Loan { Status = LoanStatus.Active }
                }
            };

            mockUserRepo.Setup(r => r.GetUserWithLoansAsync(1)).ReturnsAsync(student);

            var context = GetInMemoryDbContext();
            var loanService = new LoanService(context, mockBookRepo.Object, mockUserRepo.Object);

            // Act
            var canBorrow = await loanService.CanUserBorrowAsync(1);

            // Assert - Student limit is 3, currently has 3
            Assert.False(canBorrow);
        }
    }
}
