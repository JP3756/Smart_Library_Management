using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Data;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Repositories
{
    /// <summary>
    /// User repository implementation (REPOSITORY PATTERN)
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _context;

        public UserRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Loans)
                .Include(u => u.Reservations)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Loans)
                .Include(u => u.Reservations)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        // User-specific methods
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.Loans)
                .ToListAsync();
        }

        public async Task<IEnumerable<Faculty>> GetAllFacultyAsync()
        {
            return await _context.Faculty
                .Include(f => f.Loans)
                .ToListAsync();
        }

        public async Task<User?> GetUserWithLoansAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Loans)
                    .ThenInclude(l => l.Book)
                .Include(u => u.Loans)
                    .ThenInclude(l => l.Fine)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
