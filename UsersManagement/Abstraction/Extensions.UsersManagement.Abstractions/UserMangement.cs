namespace Extensions.UsersManagement.Abstractions;

public class UserMangement<T>
{
    public T Id { get; set; }
    public T RoleId { get; set; }
    public T UserRoleId { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string NationalCode { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsLogin { get; set; }
}
