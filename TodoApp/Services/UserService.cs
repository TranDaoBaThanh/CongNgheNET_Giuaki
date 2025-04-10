using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            try
            {
                // Hash the password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                
                // Add the user to the database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            try
            {
                // Find the user by username
                var user = await GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return null;
                }

                // Verify the password
                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    _logger.LogWarning("Failed login attempt for user: {Username}", username);
                    return null;
                }

                // Update last login time
                await UpdateLastLoginAsync(user.Id);

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during authentication: {Message}", ex.Message);
                return null;
            }
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.LastLoginAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating last login: {Message}", ex.Message);
            }
        }
    }
}
