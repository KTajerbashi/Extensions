﻿namespace UsersManagement.WebApi.Models.DTOs;

public class AuthResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
}

