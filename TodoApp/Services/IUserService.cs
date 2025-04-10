using TodoApp.Models;

namespace TodoApp.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> CreateUserAsync(User user, string password);
        Task<User?> AuthenticateAsync(string username, string password);
        Task UpdateLastLoginAsync(int userId);
    }
}
