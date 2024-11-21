namespace Extensions.UsersManagement.Abstractions;

public class UserManagerFake : IUserManager<long>
{
    public UserMangement<long> CurrentUser => new UserMangement<long>()
    {
        Id = UserId,
        RoleId = RoleId,
        UserRoleId = UserRoleId,
        Username = Username,
        FirstName = FirstName,
        LastName = LastName,
        Email = Email,
        NationalCode = NationalCode,
        IsAdmin = true,
        IsLogin = true,
    };

    public long UserId => 1;

    public long RoleId => 1;

    public long UserRoleId => 1;

    public string DisplayName => "Fake DisplayName";

    public string FirstName => "Fake FirstName";

    public string LastName => "Fake DisplayName";

    public string Email => "Fake Email";

    public string NationalCode => "Fake NationalCode";

    public string Username => "Fake Username";

    public string Ip => "127.0.0.1";

    public List<KeyValue<string, long>> UserRoles => new List<KeyValue<string, long>>();

    public List<KeyValue<string, long>> Organizations => new List<KeyValue<string, long>>();

    public bool IsAdmin => true;

    public bool IsDefault => true;

    public List<KeyValue<string, string>> Claims() => default;

    public KeyValue<string, string> GetClaim(string key) => default;

    public string GetClaim<TClaim>(string key) => default;

    public TClaim GetClaimValue<TClaim>(string key) => default;

    public object GetUserAgent() => default;

}
