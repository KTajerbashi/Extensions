using System.Collections.Concurrent;
using UsersManagement.WebApi.Interfaces;
using UsersManagement.WebApi.Models.Entities;

namespace UsersManagement.WebApi.Services;

// Services/InMemoryRefreshTokenStore.cs
public class InMemoryRefreshTokenStore : IRefreshTokenStore
{
    private readonly ConcurrentDictionary<string, ApplicationRefreshToken> _tokens = new();

    public Task StoreToken(ApplicationRefreshToken token)
    {
        _tokens[token.Token] = token;
        return Task.CompletedTask;
    }

    public Task<ApplicationRefreshToken> GetToken(string token)
    {
        _tokens.TryGetValue(token, out var refreshToken);
        return Task.FromResult(refreshToken);
    }

    public Task RevokeToken(string token)
    {
        _tokens.TryRemove(token, out _);
        return Task.CompletedTask;
    }

    public Task RevokeAllTokens(string userId)
    {
        var tokensToRemove = _tokens.Where(t => t.Value.UserId == long.Parse(userId)).ToList();
        foreach (var token in tokensToRemove)
        {
            _tokens.TryRemove(token.Key, out _);
        }
        return Task.CompletedTask;
    }
}

