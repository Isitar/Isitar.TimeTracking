namespace Isitar.TimeTracking.Api.Requests.V1.Auth
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
        public string JwtToken { get; set; }
    }
}