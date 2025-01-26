using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Data;
using SocialMediaAPI.Entities;

namespace SocialMediaAPI.Features.Groups.GetGroupPosts
{

    [Authorize]
    [Route("api/groups")]
    [ApiController]
    public class GetGroupPostsController : ControllerBase
    {
        public record GetGroupPostsRequest(int Id, string Title, string content, DateTime created, DateTime updated, string? createdBy);


        private readonly AppDbContext _context;

        public GetGroupPostsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all Posts of aspecific Group
        /// </summary>
        /// <param name="item"></param>
        /// <returns>List of Posts</returns>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /api/groups({id})
        ///     {
        ///        "Id": "username",
        ///        "Title": "description",
        ///        "Content" : "content",
        ///        "User" : "createdBy",
        ///        "CreatedAt" : "created",
        ///        "UpdatedAt" : "updated",
        ///  
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Created successful</response>
        /// <response code="400">BadRequest (Maybe Attributes are invalid)</response>
        /// <response code="404">User not Found</response>
        /// <response code="500">InternalServerError</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupPosts(int id)
        {
            //TODO: Kontolle ob User Mitglied ist
            //TODO: Kontrolle ob Gruppe öfentlich ist
            List<GetGroupPostsRequest> responsePosts = new List<GetGroupPostsRequest>();

            if (_context.Groups.Find(id) == null)
            {
                return NotFound("Gruppe nicht gefunden");
            }

            List<Post> posts = new List<Post>();
            try
            {
                posts = await _context.Posts.Where(p => p.GroupId == id).Include(p => p.CreatedByUser).ToListAsync();
                responsePosts = posts.Select(p => new GetGroupPostsRequest(p.Id, p.Title, p.Content, p.CreatedAt, p.UpdatedAt, p.CreatedByUser.UserName)).ToList();
            }
            catch (Exception e) 
            {
                return StatusCode(500, e.Message);
            }

            return Ok(responsePosts);
        }
    }
}
