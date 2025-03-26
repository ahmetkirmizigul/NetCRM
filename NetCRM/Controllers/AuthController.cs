using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCRM.DataAccess;
using NetCRM.Services;
using NetCRM.Models.DTOs;
using NetCRM.Models;

namespace NetCRM.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(AppDbContext context, JwtService jwtService )
    {
        _context = context;
        _jwtService = jwtService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || user.PasswordHash != request.Password)
            return Unauthorized(new { message = "Geçersiz kullanıcı adı veya şifre." });

        var token = _jwtService.GenerateToken(user.Username, user.Role);

        return Ok(new
        {
            token,
            username = user.Username,
            role = user.Role
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (existingUser != null)
            return BadRequest(new { message = "Bu kullanıcı adı zaten kayıtlı." });

        var newUser = new User
        {
            Username = request.Username,
            PasswordHash = request.Password, // Hash yok.düz veri giriyoruz
            Role = request.Role,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Kullanıcı başarıyla oluşturuldu." });
    }


}
