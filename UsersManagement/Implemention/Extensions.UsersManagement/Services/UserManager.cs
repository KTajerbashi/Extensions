using Extensions.UsersManagement.Abstractions;
using Extensions.UsersManagement.Extensions.Identity;
using Extensions.UsersManagement.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Extensions.UsersManagement.Services;

public class UserManager : IUserManager<long>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManagementOptions _configuration;

    public UserManager(
        IHttpContextAccessor httpContextAccessor,
        IOptions<UserManagementOptions> configuration)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
    }

    public UserMangement<long> CurrentUser => throw new NotImplementedException();

    public long UserId
    {
        get
        {
            var item = _httpContextAccessor?
                .HttpContext?
                .User?
                .GetClaimValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            var userIdClaim = GetClaimValue<string>(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userIdClaim) ? 0 : Convert.ToInt64(userIdClaim);
        }
    }

    public long RoleId => Convert.ToInt64(GetClaimValue<string>("RoleId") ?? "0");

    public long UserRoleId => Convert.ToInt64(GetClaimValue<string>("UserRoleId") ?? "0");

    public string DisplayName => $"{FirstName} {LastName}";

    public string FirstName => GetClaimValue<string>("FirstName") ?? string.Empty;

    public string LastName => GetClaimValue<string>("LastName") ?? string.Empty;

    public string Email => GetClaimValue<string>(ClaimTypes.Email) ?? string.Empty;

    public string NationalCode => GetClaimValue<string>("NationalCode") ?? string.Empty;

    public string Username => GetClaimValue<string>(ClaimTypes.Name) ?? _configuration.DefaultUsername;

    public string Ip => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? _configuration.DefaultUserIp;

    public bool IsAdmin => _httpContextAccessor.HttpContext?.User?.IsInRole("Admin") ?? false;

    public bool IsDefault => Username == _configuration.DefaultUsername;

    public List<KeyValue<string, long>> UserRoles => throw new NotImplementedException();

    public List<KeyValue<string, long>> Organizations => throw new NotImplementedException();

    public List<KeyValue<string, string>> Claims()
    {
        var claims = _httpContextAccessor.HttpContext?.User?.Claims;
        return claims == null
            ? new List<KeyValue<string, string>>()
            : claims.Select(c => new KeyValue<string, string>
            {
                Key = c.Type,
                Value = c.Value
            }).ToList();
    }

    public string GetClaim<TClaim>(string key)
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(key);
        return claim?.Value ?? string.Empty;
    }

    public KeyValue<string, string> GetClaim(string key)
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(key);
        return claim == null
            ? null
            : new KeyValue<string, string> { Key = claim.Type, Value = claim.Value };
    }

    public TClaim GetClaimValue<TClaim>(string key)
    {
        var claimValue = _httpContextAccessor.HttpContext?.User?.GetClaim(key);
        if (claimValue is null)
            return default;
        return (TClaim)Convert.ChangeType(claimValue, typeof(TClaim));
    }

    public object GetUserAgent()
        => _httpContextAccessor
        .HttpContext?
        .Request?
        .Headers["User-Agent"].ToString()
            ??
        _configuration.DefaultUserAgent;
}
