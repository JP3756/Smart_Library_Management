using Xunit;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Tests.Models
{
    /// <summary>
    /// Unit tests for Book model demonstrating Encapsulation
    /// </summary>
    public class BookTests
    {
        [Fact]
        public void Book_ISBNValidation_ShouldThrowExceptionForInvalidISBN()
        {
            // Arrange & Act & Assert - Demonstrate ENCAPSULATION with validation
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var book = new Book
                {
                    ISBN = "123", // Too short
                    Title = "Test Book",
                    Author = "Test Author",
                    Publisher = "Test Publisher",
                    PublicationYear = 2020,
                    Category = "Fiction",
                    TotalCopies = 5
                };
            });

            Assert.Contains("ISBN must be at least 10 characters", exception.Message);
        }

        [Fact]
        public void Book_TitleValidation_ShouldThrowExceptionForEmptyTitle()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var book = new Book
                {
                    ISBN = "978-0-13-468599-1",
                    Title = "", // Empty title
                    Author = "Test Author",
                    Publisher = "Test Publisher",
                    PublicationYear = 2020,
                    Category = "Fiction",
                    TotalCopies = 5
                };
            });

            Assert.Contains("cannot be empty", exception.Message);
        }

        [Fact]
        public void Book_CheckOut_ShouldDecreaseAvailableCopies()
        {
            // Arrange
            var book = new Book
            {
                ISBN = "978-0-13-468599-1",
                Title = "Test Book",
                Author = "Test Author",
                Publisher = "Test Publisher",
                PublicationYear = 2020,
                Category = "Fiction",
                TotalCopies = 5,
                AvailableCopies = 5
            };

            // Act
            book.CheckOut();

            // Assert
            Assert.Equal(4, book.AvailableCopies);
        }

        [Fact]
        public void Book_CheckOut_ShouldThrowExceptionWhenNoCopiesAvailable()
        {
            // Arrange
            var book = new Book
            {
                ISBN = "978-0-13-468599-1",
                Title = "Test Book",
                Author = "Test Author",
                Publisher = "Test Publisher",
                PublicationYear = 2020,
                Category = "Fiction",
                TotalCopies = 5,
                AvailableCopies = 0
            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => book.CheckOut());
            Assert.Contains("No copies available", exception.Message);
        }

        [Fact]
        public void Book_CheckIn_ShouldIncreaseAvailableCopies()
        {
            // Arrange
            var book = new Book
            {
                ISBN = "978-0-13-468599-1",
                Title = "Test Book",
                Author = "Test Author",
                Publisher = "Test Publisher",
                PublicationYear = 2020,
                Category = "Fiction",
                TotalCopies = 5,
                AvailableCopies = 3
            };

            // Act
            book.CheckIn();

            // Assert
            Assert.Equal(4, book.AvailableCopies);
        }

        [Fact]
        public void Book_Status_ShouldBeAvailableWhenCopiesExist()
        {
            // Arrange
            var book = new Book
            {
                ISBN = "978-0-13-468599-1",
                Title = "Test Book",
                Author = "Test Author",
                Publisher = "Test Publisher",
                PublicationYear = 2020,
                Category = "Fiction",
                TotalCopies = 5,
                AvailableCopies = 3
            };

            // Assert
            Assert.Equal("Available", book.Status);
        }

        [Fact]
        public void Book_Status_ShouldBeOutOfStockWhenNoCopies()
        {
            // Arrange
            var book = new Book
            {
                ISBN = "978-0-13-468599-1",
                Title = "Test Book",
                Author = "Test Author",
                Publisher = "Test Publisher",
                PublicationYear = 2020,
                Category = "Fiction",
                TotalCopies = 5,
                AvailableCopies = 0
            };

            // Assert
            Assert.Equal("Out of Stock", book.Status);
        }
    }
}
