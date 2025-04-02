using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsersManagement.WebApi.Interfaces;
using UsersManagement.WebApi.Models.DTOs;
using UsersManagement.WebApi.Models.Entities;

namespace UsersManagement.WebApi.Services;


public class TokenService<TUser, TId> where TUser : IdentityUser<TId> where TId : IEquatable<TId>
{
    private readonly IConfiguration _config;
    private readonly IRefreshTokenStore _refreshTokenStore;
    private readonly UserManager<TUser> _userManager;

    public TokenService(
        IConfiguration config,
        IRefreshTokenStore refreshTokenStore,
        UserManager<TUser> userManager)
    {
        _config = config;
        _refreshTokenStore = refreshTokenStore;
        _userManager = userManager;
    }

    public async Task<AuthResponse> GenerateTokensAsync(TUser user)
    {
        var accessToken = await GenerateAccessTokenAsync(user);
        var refreshToken = await GenerateRefreshTokenAsync(user.Id.ToString());

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = 900 // 15 minutes in seconds
        };
    }

    private async Task<string> GenerateAccessTokenAsync(TUser user)
    {
        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                // Add additional claims as needed
            };

        // Add user roles to claims
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Append(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<ApplicationRefreshToken> GenerateRefreshTokenAsync(string userId)
    {
        var refreshToken = new ApplicationRefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = long.Parse(userId),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow
        };

        await _refreshTokenStore.StoreToken(refreshToken);
        return refreshToken;
    }

    public async Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(accessToken);
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        var storedRefreshToken = await _refreshTokenStore.GetToken(refreshToken);
        if (storedRefreshToken == null || storedRefreshToken.UserId != long.Parse(userId) || storedRefreshToken.IsExpired)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new SecurityTokenException("User not found");
        }

        // Revoke the old refresh token
        await _refreshTokenStore.RevokeToken(refreshToken);

        // Generate new tokens
        return await GenerateTokensAsync(user);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}

