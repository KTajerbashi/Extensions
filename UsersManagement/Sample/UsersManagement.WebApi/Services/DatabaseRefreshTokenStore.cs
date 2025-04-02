using Microsoft.EntityFrameworkCore;
using UsersManagement.WebApi.DataContext;
using UsersManagement.WebApi.Interfaces;
using UsersManagement.WebApi.Models.Entities;

namespace UsersManagement.WebApi.Services;

// Entity Framework implementation example
public class DatabaseRefreshTokenStore : IRefreshTokenStore
{
    private readonly ApplicationDbContext _context;

    public DatabaseRefreshTokenStore(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task StoreToken(ApplicationRefreshToken token)
    {
        // Check if token already exists (for update scenario)
        var existingToken = await _context.ApplicationRefreshTokens
            .FirstOrDefaultAsync(t => t.Token == token.Token);

        if (existingToken != null)
        {
            _context.Entry(existingToken).CurrentValues.SetValues(token);
        }
        else
        {
            _context.ApplicationRefreshTokens.Add(token);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<ApplicationRefreshToken> GetToken(string token)
    {
        return await _context.ApplicationRefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task RevokeToken(string token)
    {
        var storedToken = await _context.ApplicationRefreshTokens
            .FirstOrDefaultAsync(t => t.Token == token);

        if (storedToken != null)
        {
            storedToken.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RevokeAllTokens(string userId)
    {
        var tokens = await _context.ApplicationRefreshTokens
            .Where(t => t.UserId == long.Parse(userId) && !t.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
        }

        await _context.SaveChangesAsync();
    }
}