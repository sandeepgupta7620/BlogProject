using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmallProjectSandeepGupta.Models;
using System.Security.Claims;

namespace SmallProjectSandeepGupta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LikesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> LikePost([FromBody] Like like)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value);
            if (await _context.LikesSandeepGupta.AnyAsync(l => l.PostID == like.PostID && l.UserID == userId))
                return BadRequest("User has already liked this post");

            like.UserID = userId;
            like.LikedAt = DateTime.Now;
            _context.LikesSandeepGupta.Add(like);

            var post = await _context.PostsSandeepGupta.FindAsync(like.PostID);
            if (post == null)
                return NotFound();

            post.LikesCount++;
            _context.PostsSandeepGupta.Update(post);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }

}
