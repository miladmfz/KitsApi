namespace webapikits.Model.Auth
{
    public class RefreshRequest
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = "";
        public string DeviceId { get; set; } = "";
    }

}
