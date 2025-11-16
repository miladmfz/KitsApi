namespace webapikits.Model.Auth
{
    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public string DeviceId { get; set; } = "";
    }

}
