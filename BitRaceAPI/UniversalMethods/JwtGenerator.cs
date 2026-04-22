using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BitRaceAPI.Models;
using BitRaceAPI.Requests;
using Microsoft.IdentityModel.Tokens;

namespace BitRaceAPI.UniversalMethods;

public class JwtGenerator
{
    private readonly string _secretKey;

    public JwtGenerator(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:Key"] ??  throw new Exception("JWT НЕ НАЙДЕН!!");
    }

    public string GenerateToken(LoginPassword user)
    {
        var claims = new[]
        {
            new Claim("userId", user.UserId.ToString()),
            new Claim("roleId", user.RoleId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}