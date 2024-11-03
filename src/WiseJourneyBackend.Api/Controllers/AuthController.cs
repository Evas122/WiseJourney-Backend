using Microsoft.AspNetCore.Mvc;
using WiseJourneyBackend.Application.Dtos.Auth;
using WiseJourneyBackend.Application.Interfaces;

namespace WiseJourneyBackend.Api.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] LoginDto loginDto)
    {
        var authResult = await _authService.LoginAsync(loginDto);
        return Ok(authResult);
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] RegisterDto registerDto)
    {
        await _authService.RegisterAsync(registerDto);

        return Ok();
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
    {
        var result = await _authService.VerifyEmailAsync(token);
        if (!result)
        {
            return BadRequest(new { Message = "Invalid or expired token." });
        }
        return Ok(new {Message = "Email succesfully verified."});
    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] string email)
    {
        await _authService.SendPasswordResetAsync(email);

        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromQuery] string token, [FromBody] string newPassword)
    {
        var result = await _authService.ResetPasswordAsync(token, newPassword);
        if (!result)
        {
            return BadRequest(new { Message = "Invalid or expired token." });
        }
        return Ok(new { Message = "Password succesfully updated." });
    }
}