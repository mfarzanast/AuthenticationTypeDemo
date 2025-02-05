using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationTypes.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class TokenAuthController : ControllerBase
    {
        private readonly  IConfiguration _configuration;
        public TokenAuthController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel user)
        {
            if (user.UserName == "admin" && user.Password == "Password")
            {
                var token = GenerateJwtToken(user.UserName);
                return Ok(new { token });
            }
            return Unauthorized();
            
        }

        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials:credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
