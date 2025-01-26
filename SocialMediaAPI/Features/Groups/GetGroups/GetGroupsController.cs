using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Data;
using SocialMediaAPI.Entities;

namespace SocialMediaAPI.Features.Groups.GetGroups
{

    [Authorize]
    [Route("api/groups")]
    [ApiController]
    public class GetGroupsController : ControllerBase
    {
        public record GetGroupsResponse(int Id, string Name, string Description);

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public GetGroupsController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Get all Groups
        /// </summary>
        /// <param name="item"></param>
        /// <returns>List of Groups</returns>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /api/groups
        ///     {
        ///        "Id": "id",
        ///        "Name": "Titel of Group",
        ///        "Description" : "content",
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Success</response>
        /// <response code="500">InternalServerError</response>
        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            //List<GetGroupsResponse> groups = new List<GetGroupsResponse>();
            //List<Group> allGroups = _context.Groups.ToList();

            List<GetGroupsResponse> groups = new List<GetGroupsResponse>();
            try
            {
                groups = await _context.Groups.Select(g => new GetGroupsResponse(g.Id, g.Name, g.Description)).ToListAsync();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }


            return Ok(groups);
        }
    }
}
