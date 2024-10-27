namespace PET.Models
{
    public class ResponseMessages
    {
        public string? Id { get; set; }
        public string? JWT { get; set; }
        public bool EmailError { get; set; }
        public bool NumberError { get; set; }
        public bool Success { get; set; }
        public string? CodeFroeEmail { get; set; }
    }
}
