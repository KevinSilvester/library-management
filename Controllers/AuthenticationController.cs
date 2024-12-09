using library_management.DTOs;
using library_management.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
namespace library_management.Helpers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtTokenHelper _jwtTokenHelper;

    public AuthController(IUserRepository userRepository, JwtTokenHelper jwtTokenHelper)
    {
        _userRepository = userRepository;
        _jwtTokenHelper = jwtTokenHelper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
        if (user == null) return Unauthorized("Invalid username or password");

        // Hash the input password and compare
        using var sha256 = SHA256.Create();
        var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)));


        if (user.PasswordHash != hashedPassword)
            return Unauthorized("Invalid username or password");

        var token = _jwtTokenHelper.GenerateToken(user);

        Response.Cookies.Append("authToken", token, new CookieOptions
        {
            HttpOnly = true, // Prevent client-side JavaScript access
            Secure = true,   // Use HTTPS
            SameSite = SameSiteMode.Strict, // Prevent cross-site cookie access
            Expires = DateTime.UtcNow.AddMinutes(60) // Set token expiration
        });

        return Ok(new { Message = "Login successful" });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // Remove the auth token cookie
        Response.Cookies.Delete("authToken");
        return Ok(new { Message = "Logout successful" });
    }
}
