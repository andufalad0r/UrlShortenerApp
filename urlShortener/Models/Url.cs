namespace urlShortener.Models
{
    public class Urls
    {
        public int Id { get; set; }
        public string OriginalURL { get; set; } = null!;
        public string ShortenedURL { get; set; } = null!;
    }
}