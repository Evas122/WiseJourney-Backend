﻿namespace WiseJourneyBackend.Application.Dtos.Auth;

public class LoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}