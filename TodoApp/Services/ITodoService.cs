using TodoApp.Models;

namespace TodoApp.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetAllTodosAsync();
        Task<IEnumerable<Todo>> GetUserTodosAsync(int userId);
        Task<Todo?> GetTodoByIdAsync(int id);
        Task<bool> CreateTodoAsync(Todo todo);
        Task<bool> UpdateTodoAsync(Todo todo);
        Task<bool> DeleteTodoAsync(int id);
    }
}
