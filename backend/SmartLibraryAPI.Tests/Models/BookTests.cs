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
        public void Book_ISBNValidation_ShouldThrowExceptionForEmptyISBN()
        {
            // Arrange & Act & Assert - Demonstrate ENCAPSULATION with validation
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var book = new Book
                {
                    ISBN = "", // Empty ISBN
                    Title = "Test Book",
                    Author = "Test Author",
                    Publisher = "Test Publisher",
                    PublicationYear = 2020,
                    Category = "Fiction",
                    TotalCopies = 5
                };
            });

            Assert.Contains("ISBN cannot be empty", exception.Message);
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
        public void Book_CheckOut_ShouldNotDecreaseWhenNoCopiesAvailable()
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

            // Act
            book.CheckOut();

            // Assert - Should remain at 0 (doesn't throw, just doesn't decrease)
            Assert.Equal(0, book.AvailableCopies);
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
