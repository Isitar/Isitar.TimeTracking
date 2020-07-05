namespace Isitar.TimeTracking.Api.Requests.V1.User
{
    public class CreateUserRequest
    {
        public string Acronym { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}