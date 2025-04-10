using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class TodoService : ITodoService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TodoService> _logger;

        public TodoService(ApplicationDbContext context, ILogger<TodoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Todo>> GetAllTodosAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<IEnumerable<Todo>> GetUserTodosAsync(int userId)
        {
            return await _context.Todos
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<Todo?> GetTodoByIdAsync(int id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task<bool> CreateTodoAsync(Todo todo)
        {
            try
            {
                _context.Todos.Add(todo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating todo: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateTodoAsync(Todo todo)
        {
            try
            {
                _context.Entry(todo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating todo: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteTodoAsync(int id)
        {
            try
            {
                var todo = await _context.Todos.FindAsync(id);
                if (todo == null)
                {
                    return false;
                }

                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting todo: {Message}", ex.Message);
                return false;
            }
        }
    }
}
