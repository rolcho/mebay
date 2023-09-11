using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MeBay.Models;
using MeBay.Interfaces;

namespace MeBay.Services;

public class JWTService : IJWTService
{
    private readonly IConfiguration _configuration;

    public JWTService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(int userId, string name, string userrole)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, userrole),
            new Claim("userId", userId.ToString()),
            new Claim("name", name)
        };

        var secretKey = _configuration["JwtSettings:Key"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public TokenDto DecodeToken(IEnumerable<Claim> claims)
    {
        var decodedToken = new TokenDto
        {
            UserId = int.Parse(claims.FirstOrDefault(c => c.Type == "userId")?.Value ?? "0"),
            Name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
            Role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
        };
        return decodedToken;
    }
}
