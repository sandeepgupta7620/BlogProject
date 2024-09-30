using Database.SearchEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmallProjectSandeepGupta.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public UsersController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("signup")]
    public async Task<ActionResult> Register([FromBody] User user)
    {
        if (_context.UsersSandeepGupta.Any(u => u.Username == user.Username || u.Email == user.Email))
            return BadRequest("Username or Email already exists");

        // Ensure PasswordHash is not empty and hash the password
        if (string.IsNullOrEmpty(user.PasswordHash))
            return BadRequest("Password is required");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        user.CreatedAt = DateTime.Now;
        _context.UsersSandeepGupta.Add(user);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] User login)

    {
        var UserID = 0;
        var Username = "";
        if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.PasswordHash))
            return BadRequest("Invalid login request");

        var user = await _context.UsersSandeepGupta.FirstOrDefaultAsync(u => u.Email == login.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(login.PasswordHash, user.PasswordHash))
        {
            
            return Unauthorized("Invalid username or password");
        }
        UserID = user.UserID;
        Username = user.Username;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserID.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Issuer"]
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { Token = tokenHandler.WriteToken(token) ,UserID,Username});
    }

    [HttpPost("GetUsers")]
    public User GetUsers([FromBody] FindUser formdata)
    {
        var user = _context.UsersSandeepGupta.Find(formdata.UserID);
        return user;
    }

}
