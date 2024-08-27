using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using urlShortener.Models;

namespace urlShortener.Services
{
    public interface IAuthService
    {
        Task<(int, string)> Registration(RegistrationModel model, string role);
        Task<(int, string)> Login(LoginModel model);
    }
}