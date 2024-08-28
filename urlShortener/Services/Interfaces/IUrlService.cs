using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using urlShortener.Dtos;
using urlShortener.Models;

namespace urlShortener.Services
{
    public interface IUrlService
    {
        Task<List<Url>> GetAllAsync();
        Task<Url> CreateUrlAsync(UrlRequestDto url, string userName, string userId);
        Task<Url> DeleteUrlAsync(int id, string userId);
        Task<Url> DeleteUrlAdminAsync(int id);

    }
}