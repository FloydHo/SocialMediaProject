using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.Data;
using SocialMediaAPI.Entities;

namespace SocialMediaAPI.Features.Groups.CreateGroup
{
    [Authorize]
    [Route("api/groups")]
    [ApiController]
    public class CreateGroupController : ControllerBase
    {
        public record CreateGroupRequest(string Name, string Description);

        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;

        public CreateGroupController(UserManager<IdentityUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Create a new group
        /// </summary>
        /// <param name="item"></param>
        /// <returns>message</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/groups
        ///     {
        ///        "Name": "username",
        ///        "Description": "description"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Created successful</response>
        /// <response code="400">BadRequest (Maybe Attributes are invalid)</response>
        /// <response code="404">User not Found</response>
        /// <response code="500">InternalServerError</response>
        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null)
            {
                return NotFound("Benutzer nicht gefunden");
            }

            var group = new Group
            {
                Name = model.Name,
                Description = model.Description,
                CreatedByUserId = user.Id
            };

            try
            {
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ein Fehler ist aufgetreten");
            }

            return StatusCode(201, model);
        }
    }
}
