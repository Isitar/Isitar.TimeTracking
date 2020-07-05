namespace Isitar.TimeTracking.Infrastructure.Identity
{
    using System;
    using NodaTime;

    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string JwtTokenId { get; set; }
        public Instant Expires { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }
    }
}