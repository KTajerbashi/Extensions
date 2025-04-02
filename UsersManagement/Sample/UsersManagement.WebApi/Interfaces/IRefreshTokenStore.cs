using UsersManagement.WebApi.Models.Entities;

namespace UsersManagement.WebApi.Interfaces;

// Interfaces/IRefreshTokenStore.cs
public interface IRefreshTokenStore
{
    Task StoreToken(ApplicationRefreshToken token);
    Task<ApplicationRefreshToken> GetToken(string token);
    Task RevokeToken(string token);
    Task RevokeAllTokens(string userId);
}

