using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Interfaces
{
    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<IEnumerable<Faculty>> GetAllFacultyAsync();
        Task<User?> GetUserWithLoansAsync(int userId);
    }
}
