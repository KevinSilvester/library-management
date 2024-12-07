using library_management.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenHelper _jwtTokenHelper;

        public AuthController(JwtTokenHelper jwtTokenHelper)
        {
            _jwtTokenHelper = jwtTokenHelper;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Replace this with actual user validation logic (e.g., database check)
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = _jwtTokenHelper.GenerateToken(request.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
