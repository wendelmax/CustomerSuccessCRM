namespace CustomerSuccessCRM.Web.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public string? Message { get; set; }
        public string? Details { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public bool ShowDetails => !string.IsNullOrEmpty(Details);

        public ErrorViewModel()
        {
            Message = "Ocorreu um erro inesperado. Por favor, tente novamente.";
        }

        public ErrorViewModel(string message)
        {
            Message = message;
        }

        public ErrorViewModel(string message, string details)
        {
            Message = message;
            Details = details;
        }
    }
}
