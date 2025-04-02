namespace UsersManagement.WebApi.Models.DTOs;

// Models/RefreshToken.cs
public class RefreshToken
{
    public string Token { get; set; }
    public string UserId { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
}

