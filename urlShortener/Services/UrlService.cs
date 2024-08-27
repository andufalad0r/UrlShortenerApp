using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using urlShortener.Dtos;
using urlShortener.Models;

namespace urlShortener.Services
{
    public class UrlService : IUrlService
    {
        public readonly ApplicationDbContext dbContext;
        private readonly HttpContext httpContext;
        public UrlService(ApplicationDbContext dbContext, HttpContext httpContext)
        {
            this.dbContext = dbContext;
            this.httpContext = httpContext;
        }

        public async Task<List<Url>> GetAllAsync()
        {
            return await dbContext.Urls.ToListAsync();
        }
        public async Task<Url> CreateUrlAsync(UrlRequestDto url, string userName, string userId)
        {
            if(BadURL(url.Url))
            {
                return null;
            }
            var shortenedURL = await GenerateShortenedURL(url.Url);
            var urlInfoModel = new Url {
                OriginalURL = url.Url,
                ShortenedURL = shortenedURL,
                UserId = userId,
                CreatedBy = userName,
                CreatedAt = DateTime.UtcNow,
            };
            await dbContext.AddAsync(urlInfoModel);
            await dbContext.SaveChangesAsync();
            return urlInfoModel;
        }

        public async Task<Url> DeleteUrlAsync(int id)
        {
            var urlModel = await dbContext.Urls.FirstOrDefaultAsync(x => x.Id == id);
            if(urlModel == null)
            {
                return null;
            }
            dbContext.Urls.Remove(urlModel);
            await dbContext.SaveChangesAsync();
            return urlModel;
        }
        private bool BadURL(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
        private const int ShortLinkLength = 5;
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        private Random random = new Random();
        private async Task<string> GenerateShortenedURL(string originalUrl)
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

                string shortenedURL = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/urlshortener/{text}";
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