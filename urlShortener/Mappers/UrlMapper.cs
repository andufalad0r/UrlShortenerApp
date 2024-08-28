using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using urlShortener.Dtos;
using urlShortener.Models;

namespace urlShortener.Mappers
{
    public static class UrlMapper
    {
        public static UrlInfoViewModel ToUrlInfo(this Url url)
        {
            return new UrlInfoViewModel
            {
                OriginalURL = url.OriginalURL,
                ShortenedURL = url.ShortenedURL,
                CreatedAt = url.CreatedAt,
                CreatedBy = url.CreatedBy,
            };
        }
    }
}