namespace NZWalkRevise.Models.DTOs
{
    public class ResponseModelDto
    {
        public string? Data { get; set; }
        public Boolean IsSuccess { get; set; } = false;
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }
        public string? RedirectUrl { get; set; }
    }
}
