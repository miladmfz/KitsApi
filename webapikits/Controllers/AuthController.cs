using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;
using webapikits.Model.Auth;
using webapikits.Service;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        private readonly JwtMaker _jwtMaker;

        JsonClass jsonClass = new JsonClass();

        public AuthController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<SupportNewController> logger,
            IConfiguration configuration,
            JwtMaker jwtMaker
        )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
            _jwtMaker = jwtMaker;
        }

        // ------------------------------
        //   قدیمی
        // ------------------------------
        [HttpGet("GetTodeyFromServer")]
        public async Task<IActionResult> GetTodeyFromServer()
        {
            string query = "select dbo.fnDate_Today() TodeyFromServer ";

            try
            {
                DataTable dataTable = await db.Auth_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTodeyFromServer");
                return StatusCode(500, "Internal server error.");
            }
        }

        // ------------------------------
        //   REGISTER
        // ------------------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            try
            {
                // ساخت hash و salt
                new PasswordHasher().CreatePasswordHash(req.Password, out byte[] hash, out byte[] salt);

                string query = $@"
                    EXEC spAuth_RegisterUser 
                        @Username = N'{req.Username}',
                        @PasswordHash = @PH,
                        @PasswordSalt = @PS,
                        @Email = N'{req.Email}',
                        @PhoneNumber = N'{req.PhoneNumber}',
                        @FullName = N'{req.FullName}'
                ";

                // ارسال پارامترهای varbinary به شکل درست
                var dt = await db.Auth_ExecQuery(HttpContext, query,
                    new Dictionary<string, object>
                    {
                        {"@PH" , hash },
                        {"@PS" , salt }
                    });

                string json = jsonClass.JsonResult_Str(dt, "Register", "Result");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register error");
                return StatusCode(500, "Internal error");
            }
        }

        // ------------------------------
        //   LOGIN + SESSION + JWT
        // ------------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            try
            {
                string query = $"EXEC spAuth_GetUserByUsername @Username = N'{req.Username}'";
                var dt = await db.Auth_ExecQuery(HttpContext, query);

                if (dt.Rows.Count == 0)
                    return Unauthorized("Username or password is incorrect");

                var row = dt.Rows[0];

                // رمز از دیتابیس: VARBINARY → byte[]
                byte[] hash = (byte[])row["PasswordHash"];
                byte[] salt = (byte[])row["PasswordSalt"];

                bool ok = new PasswordHasher().VerifyPassword(req.Password, hash, salt);

                if (!ok)
                    return Unauthorized("Username or password is incorrect");

                int userId = Convert.ToInt32(row["Id"]);
                string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                string device = req.DeviceId ?? "Unknown";

                // ساخت Refresh Token
                string refreshToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

                // انقضا — 1 روز
                DateTime expires = DateTime.UtcNow.AddDays(1);

                // ثبت Session
                string qSession = $@"
                    EXEC spUserSessions_Insert
                        @UserId = {userId},
                        @RefreshToken = N'{refreshToken}',
                        @DeviceId = N'{device}',
                        @IpAddress = N'{ip}',
                        @ExpiresAt = '{expires:yyyy-MM-dd HH:mm:ss}'
                ";

                var dtSession = await db.Auth_ExecQuery(HttpContext, qSession);
                int sessionId = Convert.ToInt32(dtSession.Rows[0]["SessionId"]);

                // ساخت JWT
                string token = _jwtMaker.CreateToken(userId.ToString(), req.Username);

                return Ok(new
                {
                    token,
                    refreshToken,
                    sessionId,
                    expiresAt = expires
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error");
                return StatusCode(500, "Error");
            }
        }

        // ------------------------------
        //   REFRESH TOKEN
        // ------------------------------
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
        {
            try
            {
                string qValid = $@"
                    EXEC spUserSessions_GetValid
                        @UserId = {req.UserId},
                        @RefreshToken = N'{req.RefreshToken}'
                ";

                var dt = await db.Auth_ExecQuery(HttpContext, qValid);
                if (dt.Rows.Count == 0)
                    return Unauthorized("Invalid refresh token");

                var row = dt.Rows[0];

                string username = row["Username"].ToString();
                int oldSessionId = Convert.ToInt32(row["Id"]);

                // revoke سشن قبلی
                string qRevoke = $"EXEC spUserSessions_Revoke @SessionId = {oldSessionId}";
                await db.Auth_ExecQuery(HttpContext, qRevoke);

                // ایجاد سشن جدید
                string newRefresh = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
                DateTime expires = DateTime.UtcNow.AddDays(1);

                string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                string device = req.DeviceId ?? "Unknown";

                string qInsert = $@"
                    EXEC spUserSessions_Insert
                        @UserId = {req.UserId},
                        @RefreshToken = N'{newRefresh}',
                        @DeviceId = N'{device}',
                        @IpAddress = N'{ip}',
                        @ExpiresAt = '{expires:yyyy-MM-dd HH:mm:ss}'
                ";

                var dtNew = await db.Auth_ExecQuery(HttpContext, qInsert);
                int sessionId = Convert.ToInt32(dtNew.Rows[0]["SessionId"]);

                string token = _jwtMaker.CreateToken(req.UserId.ToString(), username);

                return Ok(new
                {
                    token,
                    refreshToken = newRefresh,
                    sessionId,
                    expiresAt = expires
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh error");
                return StatusCode(500, "Error");
            }
        }

        // ------------------------------
        //   LOGOUT
        // ------------------------------
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest req)
        {
            try
            {
                string query = $"EXEC spUserSessions_Revoke @SessionId = {req.SessionId}";
                await db.Auth_ExecQuery(HttpContext, query);

                return Ok(new { Status = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout error");
                return StatusCode(500, "Error");
            }
        }
    }
}
