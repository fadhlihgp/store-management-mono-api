using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using store_management_mono_api.Entities;

namespace store_management_mono_api.Security;

public class JwtUtil : IJwtUtil
{
    private IConfiguration _configuration;

    public JwtUtil(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Account account)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _configuration["JwtSettings:Audience"],
            Expires = DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiresInMinutes"])),
            Issuer = _configuration["JwtSettings:Issuer"],
            IssuedAt = DateTime.Now,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new Claim("AccountId", account.Id),
                new Claim("UserName", account.Username),
                new Claim("Email", account.Email),
                new Claim("RoleId", account.RoleId),
                new Claim("Role", account.Role.Name),
                new Claim("StoreId", account.StoreId ?? "-")
            })
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }
}