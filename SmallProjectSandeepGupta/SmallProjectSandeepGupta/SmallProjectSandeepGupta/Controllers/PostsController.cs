using Database.SearchEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SmallProjectSandeepGupta.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmallProjectSandeepGupta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            if (_context.PostsSandeepGupta == null)
            {
                return NotFound();
            }
            return await _context.PostsSandeepGupta.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetSinglePost(int id)
        {
            if (_context.PostsSandeepGupta == null)
            {
                return NotFound();
            }
            var singlePost = await _context.PostsSandeepGupta.FindAsync(id);

            if (singlePost == null)
            {
                return NotFound();
            }

            return singlePost;
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Post>> CreatePost([FromBody] Post post)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value ?? throw new InvalidOperationException("User ID not found in claims"));
                post.UserID = userId;
                post.CreatedAt = DateTime.Now;
                _context.PostsSandeepGupta.Add(post);
                await _context.SaveChangesAsync();
                await GetPosts();
                //return CreatedAtAction(nameof(GetPosts), new { id = post.PostID }, post);
                return Ok(await _context.PostsSandeepGupta.ToListAsync());
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var post = await _context.PostsSandeepGupta.FindAsync(id);
                if (post == null)
                    return NotFound();

               
                _context.PostsSandeepGupta.Remove(post);
                await _context.SaveChangesAsync();
                //return NoContent();
                return Ok(await _context.PostsSandeepGupta.ToListAsync());

            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] Post post)
        {
            try
            {
                if (id != post.PostID)
                    return BadRequest("Post ID mismatch");

                var existingPost = await _context.PostsSandeepGupta.FindAsync(id);
                if (existingPost == null)
                    return NotFound();

                var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value ?? throw new InvalidOperationException("User ID not found in claims"));
                if (existingPost.UserID != userId)
                    return Forbid();

                existingPost.Title = post.Title;
                existingPost.Description = post.Description;
                _context.PostsSandeepGupta.Update(existingPost);
                await _context.SaveChangesAsync();
                await GetPosts();
                return Ok(await _context.PostsSandeepGupta.ToListAsync());
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("feedByUserId")]
        [Authorize]
        //public List<Post> UserPost([FromBody] FindUser formdata)
        public async Task<ActionResult<IEnumerable<Post>>> UserPost([FromBody] FindUser formdata)
        {
            var posts = _context.PostsSandeepGupta.Where(p => p.UserID == formdata.UserID).ToList();
            return posts;
        }
    }
}
