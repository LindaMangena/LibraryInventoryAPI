using Microsoft.AspNetCore.Mvc;
using LibraryInventoryAPI.Models.DTOs;
using LibraryInventoryAPI.Services;

namespace LibraryInventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public ActionResult<string> Login(LoginDto loginDto)
        {
            
            if (loginDto.Username != "admin" || loginDto.Password != "password")
            {
                return Unauthorized("Invalid credentials");
            }

            var token = _tokenService.CreateToken();
            return Ok(new { Token = token });
        }
    }
}
