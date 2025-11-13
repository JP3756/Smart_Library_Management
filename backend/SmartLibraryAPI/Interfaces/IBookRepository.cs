using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Interfaces
{
    /// <summary>
    /// Book-specific repository interface
    /// Extends generic repository with book-specific operations
    /// </summary>
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> SearchByTitleAsync(string title);
        Task<IEnumerable<Book>> SearchByAuthorAsync(string author);
        Task<IEnumerable<Book>> GetByCategoryAsync(string category);
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
        Task<Book?> GetByISBNAsync(string isbn);
        Task<bool> UpdateAvailabilityAsync(int bookId, int copies);
    }
}
