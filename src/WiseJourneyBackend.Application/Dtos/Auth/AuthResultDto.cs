namespace WiseJourneyBackend.Application.Dtos.Auth;

public class AuthResultDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}