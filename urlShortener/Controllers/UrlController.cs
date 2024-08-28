using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using urlShortener.Dtos;
using urlShortener.Services;

namespace urlShortener.Controllers
{
    [ApiController]
    [Route("urlshortener/urls")]
    
    public class UrlController : ControllerBase
    {
        private readonly IUrlService urlService;
        public UrlController(IUrlService urlService)
        {
            this.urlService = urlService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var urls = await urlService.GetAllAsync();
            return Ok(urls);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var urlInfo = await urlService.GetByIdAsync(id);
            return Ok(urlInfo);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UrlRequestDto urlRequestDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState); 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var urlModel = await urlService.CreateUrlAsync(urlRequestDto, userId, userName);
            if(urlModel == null)
            {
                return BadRequest("Bad url.");
            }
            return Ok(urlModel);
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState); 

            if(User.IsInRole("Admin"))
            {
                await urlService.DeleteUrlAdminAsync(id);
                return NoContent();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var urlModel = await urlService.DeleteUrlAsync(id, userId);
            if(urlModel == null)
            {
                return BadRequest("Bad url.");
            }
            return NoContent();
        }
    }
}