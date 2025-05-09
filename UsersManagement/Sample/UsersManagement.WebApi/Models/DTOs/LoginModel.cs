﻿namespace UsersManagement.WebApi.Models.DTOs;

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}

public class RegisterModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public bool IsAdmin { get; set; }
}


public class RoleModel
{
    public string RoleName { get; set; }
}

public class AssignRoleModel
{
    public string Username { get; set; }
    public string RoleName { get; set; }
}