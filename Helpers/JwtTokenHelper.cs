using DotNetEnv;
using library_management.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace library_management.Helpers;

public class JwtTokenHelper
{

    private readonly IConfiguration _configuration;

    public JwtTokenHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        Env.Load();
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

        var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "DefaultIssuer";
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "DefaultAudience";
        var jwtExpiresInMinutes = Environment.GetEnvironmentVariable("JWT_EXPIRES_IN_MINUTES") ?? "60";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        if (string.IsNullOrEmpty(jwtSecret))
        {
            throw new InvalidOperationException("JWT_SECRET environment variable is not set.");
        }

        var token = new JwtSecurityToken(
           issuer: jwtIssuer,
           audience: jwtAudience,
           claims: claims,
           expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtExpiresInMinutes)),
           signingCredentials: creds
       );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
