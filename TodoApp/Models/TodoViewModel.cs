namespace TodoApp.Models
{
    public class TodoViewModel
    {
        public List<Todo> Todos { get; set; } = new List<Todo>();
        public string? SearchString { get; set; }
        public string? SortOrder { get; set; }
        public string? StatusFilter { get; set; }
    }
}
