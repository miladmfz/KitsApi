namespace webapikits.Model.Auth
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public byte[] TokenHash { get; set; } = Array.Empty<byte>();
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public long? ReplacedById { get; set; }
    }
}
