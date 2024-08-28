using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using urlShortener.Dtos;
using urlShortener.Models;
using urlShortener.Services.Interfaces;

namespace urlShortener.Services
{
    public class UrlService : IUrlService
    {
        public readonly ApplicationDbContext dbContext;
        private readonly IUrlShorteningService urlShorteningService;
        public UrlService(ApplicationDbContext dbContext, IUrlShorteningService urlShorteningService)
        {
            this.dbContext = dbContext;
            this.urlShorteningService = urlShorteningService;
        }

        public async Task<List<Url>> GetAllAsync()
        {
            return await dbContext.Urls.ToListAsync();
        }
        public async Task<Url> CreateUrlAsync(UrlRequestDto url, string userId, string userName)
        {
            if(BadURL(url.Url))
            {
                return null;
            }
            var shortenedURL = await urlShorteningService.GenerateShortenedURL(url.Url);
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
        public async Task<Url> DeleteUrlAsync(int id, string userId)
        {
            var urlModel = await dbContext.Urls.FirstOrDefaultAsync(x => x.Id == id);
            if(urlModel == null)
            {
                return null;
            }
            if(urlModel.UserId != userId)
            {
                return null;
            }
            dbContext.Urls.Remove(urlModel);
            await dbContext.SaveChangesAsync();
            return urlModel;
        }
        public async Task<Url> DeleteUrlAdminAsync(int id)
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
            return !Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}