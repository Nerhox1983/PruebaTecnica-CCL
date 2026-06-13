using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CCL_Inventario.Core.DTOs;
using CCL_Inventario.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CCL_Inventario.Core.Services;

public class AuthService(IConfiguration configuration) : IAuthService
{
    public async Task<AuthResponseDto?> LoginAsync(string username, string password)
    {
        await Task.Delay(10);

        var validUser = configuration["TestCredentials:Username"];
        var validPass = configuration["TestCredentials:Password"];

        if (username != validUser || password != validPass)
            return null;

        return new AuthResponseDto
        {
            Token = GenerateRealJwtToken(username),
            Type = "Bearer",
            ExpiresIn = 3600,
            Message = "Autenticación exitosa"
        };
    }

    private string GenerateRealJwtToken(string username)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "123456"),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}