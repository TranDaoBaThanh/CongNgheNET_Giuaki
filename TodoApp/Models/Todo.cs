using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class Todo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [Range(1, 5, ErrorMessage = "Priority must be between 1 and 5")]
        public int Priority { get; set; } = 3;

        [Required]
        public int UserId { get; set; }

        [Display(Name = "Created")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated")]
        public DateTime? UpdatedAt { get; set; }

        // Helper properties for priority display
        public string PriorityClass
        {
            get
            {
                return Priority switch
                {
                    1 => "bg-info",
                    2 => "bg-success",
                    3 => "bg-primary",
                    4 => "bg-warning",
                    5 => "bg-danger",
                    _ => "bg-secondary"
                };
            }
        }

        public string PriorityText
        {
            get
            {
                return Priority switch
                {
                    1 => "Very Low",
                    2 => "Low",
                    3 => "Medium",
                    4 => "High",
                    5 => "Urgent",
                    _ => "Medium"
                };
            }
        }

        // Helper property for due date status
        public bool IsOverdue => !IsCompleted && DueDate.HasValue && DueDate.Value.Date < DateTime.Today;
    }
}
