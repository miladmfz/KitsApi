namespace webapikits.Models.Auth
{
    public class UserSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = "";
        public string? DeviceId { get; set; }
        public string? IpAddress { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRevoked { get; set; }
    }

    public class CreateSessionRequest
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = "";
        public string? DeviceId { get; set; }
        public string? IpAddress { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class SessionValidationResult
    {
        public bool IsValid { get; set; }
        public UserSession? Session { get; set; }
    }
}
