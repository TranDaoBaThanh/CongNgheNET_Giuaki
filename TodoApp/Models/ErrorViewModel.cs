namespace TodoApp.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string? ErrorMessage { get; set; }

        public string? ErrorDetail { get; set; }

        public int StatusCode { get; set; } = 500;
    }
}
