using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SocialMediaAPI.Features.Auth.Register
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {
        public record RegisterRequest(string Name, string Email, string Password);

        private readonly UserManager<IdentityUser> _userManager;


        public RegisterController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="item"></param>
        /// <returns>message</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /register
        ///     {
        ///        "Name": "username",
        ///        "Email": "email",
        ///        "Password": "password"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Registered successful</response>
        /// <response code="400">BadRequest</response>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            //TODO: Check if double

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Registrierung erfolgreich" });
            }

            return BadRequest(result.Errors);
        }
    }
}
