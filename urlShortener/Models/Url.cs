namespace urlShortener.Models
{
    public class Url
    {
        public int Id { get; set; }
        public string OriginalURL { get; set; } = string.Empty;
        public string ShortenedURL { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty; // username
        public string UserId { get; set; } = string.Empty; // userId
        public DateTime CreatedAt { get; set; }
    }
}