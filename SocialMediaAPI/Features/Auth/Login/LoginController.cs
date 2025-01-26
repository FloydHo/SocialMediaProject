using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMediaAPI.Features.Auth.Login
{


    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public record LoginRequest(string Email, string Password);


        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="item"></param>
        /// <returns>AuthToken</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /login
        ///     {
        ///        "Email": "email",
        ///        "password": "password"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Login successful</response>
        /// <response code="401">Unauthorized: Login failed</response>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            var result = await _signInManager.PasswordSignInAsync(
            user.UserName,
            model.Password,
            isPersistent: false,
            lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user!);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }



        /**************  Utils **************/


        private string GenerateJwtToken(IdentityUser user)
        {
            //Daten die mit dem Token verschlüsselt werden und später wieder ausgelesen werden können
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email), 
                //TODO: Add new Claim Role
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3), // Gültigkeit 3 Stunde
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
