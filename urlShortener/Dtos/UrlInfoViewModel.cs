using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace urlShortener.Dtos
{
    public class UrlInfoViewModel
    {
        public string OriginalURL { get; set; } = string.Empty;
        public string ShortenedURL { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; }
    }
}