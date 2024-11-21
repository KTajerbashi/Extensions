namespace Extensions.UsersManagement.Abstractions;

public interface IUserManager<T>
{
    UserMangement<T> CurrentUser { get; }
    T UserId { get; }
    T RoleId { get; }
    T UserRoleId { get; }
    string DisplayName { get; }
    string FirstName { get; }
    string LastName { get; }
    string Email { get; }
    string NationalCode { get; }
    string Username { get; }
    string Ip { get; }
    bool IsAdmin { get; }
    bool IsDefault { get; }
    List<KeyValue<string, T>> UserRoles { get; }
    List<KeyValue<string, T>> Organizations { get; }
    List<KeyValue<string, string>> Claims();
    string GetClaim<TClaim>(string key);
    TClaim GetClaimValue<TClaim>(string key);
    KeyValue<string, string> GetClaim(string key);
    object GetUserAgent();

}
