using ECommerceAPI.Data;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class TokenService: ITokenService
{
    private readonly IConfiguration config;
    private AppDbContext db;
    private ILogger<TokenService> logger;
    private ICacheService redis;

    public TokenService(IConfiguration config, AppDbContext db, ILogger<TokenService> logger, ICacheService redis)
    {
        this.config = config;
        this.db = db;
        this.logger = logger;
        this.redis = redis;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]));
        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: config["JwtSettings:Issuer"],
            audience: config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                double.Parse(config["JwtSettings:AccessTokenExpiryMinutes"])),
            signingCredentials: credentials
        );

        logger.LogInformation(
                "Access token generated for user {Username} " +
                "expires at {Expiry}",
                user.UserName,
                DateTime.Now.AddMinutes(
                double.Parse(config["JwtSettings:AccessTokenExpiryMinutes"])));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        byte[] randomBytes = new byte[64];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public void SaveRefreshToken(int userId, string refreshToken)
    {
        redis.Delete($"refreshtoken:{userId}");
        redis.Set($"refreshtoken:{userId}",refreshToken,TimeSpan.FromDays(double.Parse(config["JwtSettings:RefreshTokenExpiryDays"])));
    }

    public TokenResponse RefreshTokens(int userId)
    {
        if (!redis.Exists($"refreshtoken:{userId}"))
        {
            logger.LogWarning("Refresh Token Doesnt Exist for user");
            throw new UnauthorizedAccessException("Refresh Token Doesnt Exist");
        }
        User user = db.Users.FirstOrDefault(u => u.Id == userId);
        string newAccessToken = GenerateToken(user);
        string newRefreshToken = GenerateRefreshToken();
        SaveRefreshToken(userId, newRefreshToken);

        logger.LogInformation("Tokens refreshed successfully for userId {userId}",userId);

        return new TokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(
                double.Parse(config["JwtSettings:AccessTokenExpiryMinutes"])),
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(double.Parse(config["JwtSettings:RefreshTokenExpiryDays"])),
            Username = user.UserName,
            Role = user.Role.ToString()
        };
    }
}