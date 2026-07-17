using Microsoft.AspNetCore.Mvc;
using University_system.Dtos;
using University_system.Interface_Service;


namespace University_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
         {
            //// 🛑 کد موقت: هش دقیق سیستم شما را تولید می‌کند و به عنوان پیام خطا برمی‌گرداند
            //var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Microsoft.AspNetCore.Identity.IdentityUser>();
            //string exactHash = hasher.HashPassword(new Microsoft.AspNetCore.Identity.IdentityUser(), "Password123!");
            //return BadRequest(new { message = $"🎯 HASH: {exactHash}" });

            var result = await _authService.LoginAsync(loginDto);
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(result);

        }
    }
}
