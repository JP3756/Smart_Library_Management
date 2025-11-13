using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Interfaces
{
    /// <summary>
    /// User factory interface (FACTORY PATTERN)
    /// </summary>
    public interface IUserFactory
    {
        User CreateUser(string userType, string name, string email);
    }
}
