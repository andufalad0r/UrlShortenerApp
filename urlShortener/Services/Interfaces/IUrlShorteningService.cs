using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace urlShortener.Services.Interfaces
{
    public interface IUrlShorteningService
    {
        Task<string> GenerateShortenedURL(string url);
    }
}