using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Data;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Repositories
{
    /// <summary>
    /// Book repository implementation (REPOSITORY PATTERN)
    /// Demonstrates SOLID - Single Responsibility Principle
    /// </summary>
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Loans)
                .Include(b => b.Reservations)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Loans)
                .Include(b => b.Reservations)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(b => b.Id == id);
        }

        // Book-specific methods
        public async Task<IEnumerable<Book>> SearchByTitleAsync(string title)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(title))
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchByAuthorAsync(string author)
        {
            return await _context.Books
                .Where(b => b.Author.Contains(author))
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetByCategoryAsync(string category)
        {
            return await _context.Books
                .Where(b => b.Category == category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _context.Books
                .Where(b => b.AvailableCopies > 0)
                .ToListAsync();
        }

        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<bool> UpdateAvailabilityAsync(int bookId, int copies)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return false;

            book.AvailableCopies = copies;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
