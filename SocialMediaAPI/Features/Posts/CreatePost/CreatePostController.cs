using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.Data;
using SocialMediaAPI.Entities;
using static SocialMediaAPI.Features.Groups.CreateGroup.CreateGroupController;

namespace SocialMediaAPI.Features.Posts.CreatePost
{

    [Authorize]
    [Route("api/groups/")]
    [ApiController]
    public class CreatePostController : ControllerBase
    {
 
        public record CreatePostRequest(string Content, string Title);

        public AppDbContext _context;
        public UserManager<IdentityUser> _userManager;

        public CreatePostController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }


        /// <summary>
        /// Creates a new Post in a Group
        /// </summary>
        /// <param name="item"></param>
        /// <returns>message</returns>
        /// <remarks>
        /// Sample response:
        ///
        ///     Post /api/groups/{id}
        ///     {
        ///        "Title": "TechNews",
        ///        "Content": "News about Tech",
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Success</response>
        /// <response code="500">InternalServerError</response>
        [HttpPost("{id}")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest model, int id)
        {
            if (_context.Groups.Find(id) == null)
            {
                return NotFound("Gruppe nicht gefunden");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null)
            {
                return NotFound("Benutzer nicht gefunden");
            }

            Post newPost = new Post
            {
                Content = model.Content,
                GroupId = id,
                Title = model.Title,
                CreatedByUserId = user.Id
            };

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();

            return Created();
        }
    }
}
