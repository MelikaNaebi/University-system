using Microsoft.AspNetCore.Identity;
using University_system.Dtos;
using University_system.Interface_Service;
using University_system.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using University_system.Interface_Repository;

namespace University_system.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user == null)
            {
                return new AuthResultDto { IsSuccess = false, Message = "نام کاربری یا رمز عبور اشتباه است." };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return new AuthResultDto { IsSuccess = false, Message = "نام کاربری یا رمز عبور اشتباه است." };
            }

            var token = await GenerateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            string userRole = "Student";
            if (roles.Contains("DepartmentHead")) userRole = "DepartmentHead";
            else if (roles.Contains("Staff")) userRole = "Staff";
            else if (roles.Contains("Instructor")) userRole = "Instructor";

            return new AuthResultDto
            {
                IsSuccess = true,
                Token = token,
                Role = userRole,
                Message = "ورود با موفقیت انجام شد."
            };
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (roles.Contains("Student"))
            {
                var studentId = await _unitOfWork.Students.GetStudentIdByUserIdAsync(user.Id);
                if (studentId != 0)
                {
                    claims.Add(new Claim("StudentId", studentId.ToString()));
                }
            }
            else if (roles.Contains("Instructor"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "Instructor"));
            }
            else if (roles.Contains("DepartmentHead"))
            {
                claims.Add(new Claim("DepartmentHeadId", user.Id));
            }
            else if (roles.Contains("Staff"))
            {
                claims.Add(new Claim("StaffId", user.Id));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = creds,
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}