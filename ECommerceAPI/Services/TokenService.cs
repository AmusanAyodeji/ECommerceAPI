using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceAPI.Models;
using ECommerceAPI.Interfaces;

public class TokenService: ITokenService
{
    private readonly IConfiguration config;
    private ITokenService _tokenservice;

    public TokenService(IConfiguration config)
    {
        this.config = config;
    }

    public string GenerateToken(User user)
    {
        // Claims are pieces of information about the user
        // stored inside the token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        // get secret key
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]));

        // create credentials
        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        // create token
        var token = new JwtSecurityToken(
            issuer: config["JwtSettings:Issuer"],
            audience: config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                double.Parse(config["JwtSettings:ExpiryInMinutes"])),
            signingCredentials: credentials
        );

        // return token as string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}