using Microsoft.AspNetCore.Mvc;
using SmartLibraryAPI.DTOs;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Controllers
{
    /// <summary>
    /// Books API Controller - Handles all book-related operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        /// <summary>
        /// Get all books
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllAsync();
            var bookDtos = books.Select(b => MapToDto(b));
            return Ok(bookDtos);
        }

        /// <summary>
        /// Get book by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                return NotFound(new { message = $"Book with ID {id} not found" });

            return Ok(MapToDto(book));
        }

        /// <summary>
        /// Search books by title
        /// </summary>
        [HttpGet("search/title/{title}")]
        public async Task<ActionResult<IEnumerable<BookDto>>> SearchByTitle(string title)
        {
            var books = await _bookRepository.SearchByTitleAsync(title);
            var bookDtos = books.Select(b => MapToDto(b));
            return Ok(bookDtos);
        }

        /// <summary>
        /// Search books by author
        /// </summary>
        [HttpGet("search/author/{author}")]
        public async Task<ActionResult<IEnumerable<BookDto>>> SearchByAuthor(string author)
        {
            var books = await _bookRepository.SearchByAuthorAsync(author);
            var bookDtos = books.Select(b => MapToDto(b));
            return Ok(bookDtos);
        }

        /// <summary>
        /// Get books by category
        /// </summary>
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetByCategory(string category)
        {
            var books = await _bookRepository.GetByCategoryAsync(category);
            var bookDtos = books.Select(b => MapToDto(b));
            return Ok(bookDtos);
        }

        /// <summary>
        /// Get available books
        /// </summary>
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAvailableBooks()
        {
            var books = await _bookRepository.GetAvailableBooksAsync();
            var bookDtos = books.Select(b => MapToDto(b));
            return Ok(bookDtos);
        }

        /// <summary>
        /// Create new book
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
        {
            try
            {
                // Check if ISBN already exists
                var existingBook = await _bookRepository.GetByISBNAsync(createBookDto.ISBN);
                if (existingBook != null)
                    return Conflict(new { message = "Book with this ISBN already exists" });

                var book = new Book
                {
                    ISBN = createBookDto.ISBN,
                    Title = createBookDto.Title,
                    Author = createBookDto.Author,
                    Publisher = createBookDto.Publisher,
                    PublicationYear = createBookDto.PublicationYear,
                    Category = createBookDto.Category,
                    Description = createBookDto.Description,
                    TotalCopies = createBookDto.TotalCopies,
                    AvailableCopies = createBookDto.TotalCopies
                };

                var createdBook = await _bookRepository.AddAsync(book);
                return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, MapToDto(createdBook));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update book
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> UpdateBook(int id, UpdateBookDto updateBookDto)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);
                if (book == null)
                    return NotFound(new { message = $"Book with ID {id} not found" });

                // Update only provided fields
                if (!string.IsNullOrEmpty(updateBookDto.Title))
                    book.Title = updateBookDto.Title;
                if (!string.IsNullOrEmpty(updateBookDto.Author))
                    book.Author = updateBookDto.Author;
                if (!string.IsNullOrEmpty(updateBookDto.Publisher))
                    book.Publisher = updateBookDto.Publisher;
                if (updateBookDto.PublicationYear.HasValue)
                    book.PublicationYear = updateBookDto.PublicationYear.Value;
                if (!string.IsNullOrEmpty(updateBookDto.Category))
                    book.Category = updateBookDto.Category;
                if (updateBookDto.Description != null)
                    book.Description = updateBookDto.Description;
                if (updateBookDto.TotalCopies.HasValue)
                    book.TotalCopies = updateBookDto.TotalCopies.Value;
                if (updateBookDto.AvailableCopies.HasValue)
                    book.AvailableCopies = updateBookDto.AvailableCopies.Value;

                var updatedBook = await _bookRepository.UpdateAsync(book);
                return Ok(MapToDto(updatedBook));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete book
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var success = await _bookRepository.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = $"Book with ID {id} not found" });

            return NoContent();
        }

        // Helper method to map Book to BookDto
        private BookDto MapToDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                ISBN = book.ISBN,
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                PublicationYear = book.PublicationYear,
                Category = book.Category,
                Description = book.Description,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                Status = book.Status
            };
        }
    }
}
