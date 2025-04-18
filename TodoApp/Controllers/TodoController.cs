using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoService _todoService;
        private readonly ILogger<TodoController> _logger;

        public TodoController(ITodoService todoService, ILogger<TodoController> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, string statusFilter)
        {
            // Get current user ID from claims
            var userId = GetCurrentUserId();

            // Get all todos for the current user
            var todos = await _todoService.GetUserTodosAsync(userId);

            // Apply search filter if search string is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                todos = todos.Where(t => t.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                         t.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(statusFilter))
            {
                bool isCompleted = statusFilter == "completed";
                todos = todos.Where(t => t.IsCompleted == isCompleted);
            }

            // Apply sort order
            ViewData["TitleSortParam"] = sortOrder == "title" ? "title_desc" : "title";
            ViewData["DateSortParam"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["PrioritySortParam"] = sortOrder == "priority" ? "priority_desc" : "priority";
            ViewData["StatusSortParam"] = sortOrder == "status" ? "status_desc" : "status";

            todos = sortOrder switch
            {
                "title" => todos.OrderBy(t => t.Title),
                "title_desc" => todos.OrderByDescending(t => t.Title),
                "date" => todos.OrderBy(t => t.DueDate),
                "date_desc" => todos.OrderByDescending(t => t.DueDate),
                "priority" => todos.OrderBy(t => t.Priority),
                "priority_desc" => todos.OrderByDescending(t => t.Priority),
                "status" => todos.OrderBy(t => t.IsCompleted),
                "status_desc" => todos.OrderByDescending(t => t.IsCompleted),
                _ => todos.OrderByDescending(t => t.CreatedAt),
            };

            // Prepare view model
            var viewModel = new TodoViewModel
            {
                Todos = todos.ToList(),
                SearchString = searchString,
                SortOrder = sortOrder,
                StatusFilter = statusFilter
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Todo todo)
        {
            if (ModelState.IsValid)
            {
                var userId = GetCurrentUserId();
                todo.UserId = userId;
                todo.CreatedAt = DateTime.UtcNow;

                await _todoService.CreateTodoAsync(todo);
                _logger.LogInformation("User {UserId} created new todo: {TodoTitle}", userId, todo.Title);

                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetCurrentUserId();
            var todo = await _todoService.GetTodoByIdAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            if (todo.UserId != userId)
            {
                return Forbid();
            }

            return View(todo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Todo todo)
        {
            if (id != todo.Id)
            {
                return NotFound();
            }

            var userId = GetCurrentUserId();

            // Check if todo exists and belongs to the current user
            var existingTodo = await _todoService.GetTodoByIdAsync(id);
            if (existingTodo == null)
            {
                return NotFound();
            }

            if (existingTodo.UserId != userId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                todo.UserId = userId;
                todo.UpdatedAt = DateTime.UtcNow;

                await _todoService.UpdateTodoAsync(todo);
                _logger.LogInformation("User {UserId} updated todo: {TodoTitle}", userId, todo.Title);

                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            var todo = await _todoService.GetTodoByIdAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            if (todo.UserId != userId)
            {
                return Forbid();
            }

            await _todoService.DeleteTodoAsync(id);
            _logger.LogInformation("User {UserId} deleted todo: {TodoTitle}", userId, todo.Title);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var userId = GetCurrentUserId();
            var todo = await _todoService.GetTodoByIdAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            if (todo.UserId != userId)
            {
                return Forbid();
            }

            // Toggle completion status
            todo.IsCompleted = !todo.IsCompleted;
            todo.UpdatedAt = DateTime.UtcNow;

            await _todoService.UpdateTodoAsync(todo);
            _logger.LogInformation("User {UserId} toggled completion status of todo: {TodoTitle}", userId, todo.Title);

            return RedirectToAction(nameof(Index));
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new ApplicationException("User ID claim not found or invalid");
        }
    }
}
