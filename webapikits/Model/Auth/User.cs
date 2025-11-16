namespace webapikits.Model.Auth
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public bool IsActive { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
