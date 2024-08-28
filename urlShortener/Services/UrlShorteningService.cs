using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using urlShortener.Services.Interfaces;

namespace urlShortener.Services
{
    public class UrlShorteningService : IUrlShorteningService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly ApplicationDbContext dbContext;
        public UrlShorteningService(IHttpContextAccessor httpContext, ApplicationDbContext dbContext)
        {
            this.httpContext = httpContext;
            this.dbContext = dbContext;
        }
        private const int ShortLinkLength = 5;
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        private Random random = new Random();
        public async Task<string> GenerateShortenedURL(string originalUrl)
        {
            char[] linkText = new char[ShortLinkLength];
            while(true)
            {
                for(int i = 0; i < ShortLinkLength; i++)
                {
                    var randomIndex = random.Next(Alphabet.Length - 1);

                    linkText[i] = Alphabet[randomIndex];
                }
                string text = new string(linkText);

                string shortenedURL = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}/urlshortener/{text}";
                if(!await URLExists(shortenedURL))
                {
                    return shortenedURL;
                }
            }
        }
        private async Task<bool> URLExists(string url)
        {
            return await dbContext.Urls.AnyAsync(r => r.ShortenedURL == url);
        }
    }
}