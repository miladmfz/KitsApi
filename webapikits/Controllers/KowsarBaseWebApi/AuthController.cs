using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
using webapikits.Model.Auth;
using webapikits.Service;
using System.Text;
using System.Globalization;

namespace webapikits.Controllers.KowsarBaseWebApi
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly JwtMaker _jwtMaker;

        JsonClass jsonClass = new JsonClass();

        public AuthController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<AuthController> logger,
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




        [HttpPost]
        [Route("SendSmsLogin")]
        public async Task<string> SendSmsLogin(string RandomCode, string NumberPhone)
        {

            string sms_api_key = _configuration.GetValue<string>("AppSettings:sms_api_key");

            HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("x-api-key", "me8CfaoTR0rLZEpRWQqdvtCnzcsRwpPtVz9mmwYdbWv5kBEjtSJKZG3wMYCvEndd");
            httpClient.DefaultRequestHeaders.Add("x-api-key", sms_api_key);

            var payload = @"{" + "\n" +
            @"  ""mobile"": """ + NumberPhone + @"""," + "\n" +
            @"  ""templateId"": 100000," + "\n" +
            @"  ""parameters"": [" + "\n" +
            @"    {" + "\n" +
            @"      ""name"": ""CODE""," + "\n" +
            @"      ""value"": """ + RandomCode + @"""" + "\n" +
            @"    }" + "\n" +
            @"  ]" + "\n" +
            @"}";
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://api.sms.ir/v1/send/verify", content);
            var result = await response.Content.ReadAsStringAsync();

            return result; // You may want to return an error message or handle this differently.
        }






        [HttpPost]
        [Route("IsUser")]
        public async Task<IActionResult> IsUser([FromBody] IsUserDto isUserDto)
        {
            if (isUserDto == null ||
                string.IsNullOrWhiteSpace(isUserDto.UName) ||
                string.IsNullOrWhiteSpace(isUserDto.UPass))
            {
                return BadRequest("نام کاربری و رمز عبور الزامی است.");
            }

            string query = "Exec [dbo].[spWeb_IsXUser] @UName, @UPass";

            try
            {
                var parameters = new Dictionary<string, object>
        {
            { "@UName", isUserDto.UName },
            { "@UPass", isUserDto.UPass }
        };

                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query, parameters);

                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];

                    int errCode = row.Table.Columns.Contains("ErrCode") && row["ErrCode"] != DBNull.Value
                        ? Convert.ToInt32(row["ErrCode"])
                        : -1;

                    int authSms = row.Table.Columns.Contains("AuthSms") && row["AuthSms"] != DBNull.Value
                        ? Convert.ToInt32(row["AuthSms"])
                        : 0;

                    string randomCode = row.Table.Columns.Contains("RandomeCode") && row["RandomeCode"] != DBNull.Value
                        ? Convert.ToString(row["RandomeCode"]) ?? ""
                        : "";

                    string numberPhone = row.Table.Columns.Contains("PhMobile1") && row["PhMobile1"] != DBNull.Value
                        ? Convert.ToString(row["PhMobile1"]) ?? ""
                        : "";

                    if (errCode == 0 && authSms == 1)
                    {
                        if (!string.IsNullOrWhiteSpace(randomCode) &&
                            !string.IsNullOrWhiteSpace(numberPhone))
                        {
                            string smsResult = await SendSmsLogin(randomCode, numberPhone);

                        }
                       
                    }

                    if (!string.IsNullOrWhiteSpace(randomCode))
                    {
                        string encodedCode = Convert.ToBase64String(
                            Encoding.UTF8.GetBytes(randomCode)
                        );

                        dataTable.Rows[0]["RandomeCode"] = encodedCode;
                    }
                }

                string json = jsonClass.JsonResult_Str(dataTable, "users", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(IsUser));
                return StatusCode(500, "Internal server error.");
            }
        }


        /// ////////////////////////////////////////////////////////////////////////


        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            string query = "SELECT * FROM dbo.Role WHERE Active = 1 ORDER BY RoleCode";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);

                string json = jsonClass.JsonResult_Str(dataTable, "Roles", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetRoles));
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpGet]
        [Route("GetRoleById")]
        public async Task<IActionResult> GetRoleById(string RoleCode)
        {
            string query = $"SELECT * FROM dbo.Role WHERE RoleCode = {RoleCode}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);

                string json = jsonClass.JsonResult_Str(dataTable, "Roles", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetRoleById));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpGet]
        [Route("GetPermissions")]
        public async Task<IActionResult> GetPermissions()
        {
            string query = "SELECT * FROM dbo.Permission WHERE Active = 1";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);

                string json = jsonClass.JsonResult_Str(dataTable, "Permissions", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetPermissions));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpGet]
        [Route("GetRolePermissions")]
        public async Task<IActionResult> GetRolePermissions(string RoleRef)
        {
            string query = $@"
        SELECT p.*
        FROM RolePermission rp
        JOIN Permission p ON p.PermissionCode = rp.PermissionRef
        WHERE rp.RoleRef = {RoleRef} AND rp.Active = 1";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);

                string json = jsonClass.JsonResult_Str(dataTable, "RolePermissions", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetRolePermissions));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("GetCentralRoles")]
        public async Task<IActionResult> GetCentralRoles(string CentralRef)
        {
            string query = $@"
        SELECT r.*
        FROM CentralRole cr
        JOIN Role r ON r.RoleCode = cr.RoleRef
        WHERE cr.CentralRef = {CentralRef} AND cr.Active = 1";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);

                string json = jsonClass.JsonResult_Str(dataTable, "CentralRoles", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentralRoles));
                return StatusCode(500, "Internal server error.");
            }
        }






        [HttpGet]
        [Route("GetCentralUsers")]
        public async Task<IActionResult> GetCentralUsers()
        {
            string query = @"
        SELECT
            u.UserId,
            u.CentralRef,
            u.UserName,
            u.UserNameInPrint,
            u.Active
        FROM dbo.Users u
        ORDER BY u.UserId
    ";

            try
            {
                DataTable usersTable = await db.Kowsar_ExecQuery(HttpContext, query);

                DataTable result = new DataTable();
                result.Columns.Add("UserId", typeof(int));
                result.Columns.Add("CentralRef", typeof(int));
                result.Columns.Add("CentralName", typeof(string));
                result.Columns.Add("UserName", typeof(string));
                result.Columns.Add("UserNameInPrint", typeof(string));
                result.Columns.Add("Active", typeof(string));

                foreach (DataRow row in usersTable.Rows)
                {
                    int userId = Convert.ToInt32(row["UserId"]);

                    string centralRefText = TimeDeCoderSafe(row["CentralRef"]?.ToString());
                    string userName = TimeDeCoderSafe(row["UserName"]?.ToString());
                    string active = TimeDeCoderSafe(row["Active"]?.ToString());

                    int centralRef = 0;
                    int.TryParse(centralRefText, out centralRef);

                    string centralName = "";

                    if (centralRef > 0)
                    {
                        DataTable centralTable = await db.Kowsar_ExecQuery(
                            HttpContext,
                            $"SELECT Name FROM dbo.Central WHERE CentralCode = {centralRef}"
                        );

                        if (centralTable.Rows.Count > 0)
                            centralName = centralTable.Rows[0]["Name"]?.ToString() ?? "";
                    }

                    result.Rows.Add(
                        userId,
                        centralRef,
                        centralName,
                        userName,
                        row["UserNameInPrint"]?.ToString() ?? "",
                        active
                    );
                }

                string json = jsonClass.JsonResult_Str(result, "CentralUsers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentralUsers));
                return StatusCode(500, "Internal server error.");
            }
        }





        /// //////////////////////////////////////////////////////////////////////////

        [HttpGet]
        [Route("CentralPermission")]
        public async Task<IActionResult> CentralPermission(string CentralRef)
        {

            string query = $"spWeb_CentralPermission_Get {CentralRef} ";




            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Permissions", ""); ;
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CentralPermission));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpPost]
        [Route("KowsarLogin")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            if (loginUserDto == null ||
                string.IsNullOrWhiteSpace(loginUserDto.UName) ||
                string.IsNullOrWhiteSpace(loginUserDto.UPass))
            {
                return BadRequest("اطلاعات ورود کامل نیست.");
            }

            string query = $@"
Select 
    LoginType = 'KOWSAR',

    UserId = IsNull(du.DepartmentUserCode, 0),
    OldUserId = u.UserId,

    u.CentralRef,
    u.UserName,
    u.PassWord,
    u.Active,

    DisplayName = IsNull(u.UserNameInPrint, ''),

    d.DepartmentCode,
    d.DepartmentName,

    UserMaxDiscount = IsNull(du.UserMaxDiscount, 0),

    UserIdRef = 
        Case 
            When IsNull(du.DepartmentUserCode, 0) > 0 
                Then (IsNull(du.DepartmentUserCode, 0) * 7) + 8
            Else 0
        End,

    DepartmentUserCode = IsNull(du.DepartmentUserCode, 0),

    ActiveDate = dbo.fnDate_Today(),

    XUserCode = '',
    CustomerCode = '',
    CustName_Small = '',
    PersonInfoRef = '',
    PhFullName = '',
    SessionId = newid(),

    Message = '',
    ErrCode = 0,
    ErrDesc = ''

From Users u
Left Join Department d 
    On d.DepartmentCode = {loginUserDto.DepartmentCode}
Left Join DepartmentUser du 
    On du.UserRef = u.UserId 
   And du.DepartmentRef = d.DepartmentCode 
   And du.IsKowsarUser = 1
";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);

                LoginResultDto loginResult = null;

                string inputUserName = NormalizeLoginText(loginUserDto.UName);
                string inputPassword = NormalizeLoginText(loginUserDto.UPass);

                foreach (DataRow row in dataTable.Rows)
                {
                    string dbUserNameRaw = row["UserName"]?.ToString() ?? "";
                    string dbPasswordRaw = row["PassWord"]?.ToString() ?? "";
                    string dbActiveRaw = row["Active"]?.ToString() ?? "";
                    string dbCentralRefRaw = row["CentralRef"]?.ToString() ?? "";

                    string dbUserName = TimeDeCoderSafe(dbUserNameRaw);
                    string dbPassword = TimeDeCoderSafe(dbPasswordRaw);
                    string dbActive = TimeDeCoderSafe(dbActiveRaw);
                    string centralRefDecoded = TimeDeCoderSafe(dbCentralRefRaw);

                    string dbUserNameNormalized = NormalizeLoginText(dbUserName);
                    string dbPasswordNormalized = NormalizeLoginText(dbPassword);

                    _logger.LogInformation(
                        "LOGIN_CHECK => InputUser:[{InputUser}] DbUser:[{DbUser}] InputPass:[{InputPass}] DbPass:[{DbPass}] Active:[{Active}]",
                        NormalizeLoginText(loginUserDto.UName),
                        NormalizeLoginText(dbUserName),
                        NormalizeLoginText(loginUserDto.UPass),
                        NormalizeLoginText(dbPassword),
                        dbActive
                    );

                    bool userMatched =
                        dbUserNameNormalized == inputUserName;

                    bool passwordMatched =
                        dbPasswordNormalized == inputPassword;

                    if (!userMatched || !passwordMatched)
                    {
                        continue;
                    }

                    int departmentUserCode = Convert.ToInt32(row["DepartmentUserCode"]);

                    bool isActive =
                        string.Equals(
                            NormalizeLoginText(dbActive),
                            "true",
                            StringComparison.OrdinalIgnoreCase
                        );

                    if (departmentUserCode <= 0)
                    {
                        return Content(
                            jsonClass.JsonResult_Str(CreateMessageTable("USER_HAS_NO_DEPARTMENT_ACCESS"), "users", ""),
                            "application/json"
                        );
                    }

                    if (!isActive)
                    {
                        return Content(
                            jsonClass.JsonResult_Str(CreateMessageTable("INACTIVE_USER"), "users", ""),
                            "application/json"
                        );
                    }

                    int centralRef = 0;

                    if (!int.TryParse(centralRefDecoded, out centralRef))
                    {
                        centralRef = 0;
                    }

                    string centralName = "";
                    string Manager = "";
                    string Delegacy = "";

                    if (centralRef > 0)
                    {
                        DataTable centralTable = await db.Kowsar_ExecQuery(
                            HttpContext,
                            $"Select Name ,Manager,Delegacy From Central Where CentralCode = {centralRef}"
                        );

                        if (centralTable.Rows.Count > 0)
                        {
                            centralName = centralTable.Rows[0]["Name"]?.ToString() ?? "";
                            Manager = centralTable.Rows[0]["Manager"]?.ToString() ?? "";
                            Delegacy = centralTable.Rows[0]["Delegacy"]?.ToString() ?? "";
                        }
                    }

                    loginResult = new LoginResultDto
                    {
                        UserId = departmentUserCode,
                        ActiveDate = row["ActiveDate"]?.ToString(),
                        SessionId = row["SessionId"]?.ToString(),
                        OldUserId = Convert.ToInt32(row["OldUserId"]),

                        CentralRef = centralRef,
                        CentralName = centralName,
                        Manager = Manager,
                        Delegacy = Delegacy,

                        UserName = dbUserName,
                        DisplayName = row["DisplayName"]?.ToString(),

                        DepartmentCode = Convert.ToInt32(row["DepartmentCode"]),
                        DepartmentName = row["DepartmentName"]?.ToString(),

                        UserMaxDiscount =
                            row["UserMaxDiscount"] == DBNull.Value
                                ? 0
                                : Convert.ToDecimal(row["UserMaxDiscount"]),

                        UserIdRef = departmentUserCode * 7 + 8
                    };

                    break;
                }

                if (loginResult == null)
                {
                    return Content(
                        jsonClass.JsonResult_Str(CreateMessageTable("INVALID_USER"), "users", ""),
                        "application/json"
                    );
                }

                DataTable resultTable = CreateLoginResultTable(loginResult);
                string json = jsonClass.JsonResult_Str(resultTable, "users", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Login));
                return StatusCode(500, "Internal server error.");
            }
        }
        private string NormalizeLoginText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            return value
                .Trim()
                .Replace('ي', 'ی')
                .Replace('ى', 'ی')
                .Replace('ك', 'ک')
                .Replace('ۀ', 'ه')
                .Replace('ة', 'ه')
                .Replace("‌", "")
                .Replace(" ", "");
        }

        private string TimeDeCoderSafe(string inStr)
        {
            if (string.IsNullOrWhiteSpace(inStr))
                return "";

            try
            {
                if (inStr.Length % 6 != 0)
                    return inStr;

                List<byte> bytes = new List<byte>();

                for (int i = 0; i < inStr.Length / 6; i++)
                {
                    int index = i * 6;

                    string p1 = inStr.Substring(index, 2);
                    string p2 = inStr.Substring(index + 2, 2);
                    string p3 = inStr.Substring(index + 4, 2);

                    if (!IsHex(p1) || !IsHex(p2) || !IsHex(p3))
                        return inStr;

                    int part1 = Convert.ToInt32(p1, 16);
                    int part2 = Convert.ToInt32(p2, 16);
                    int part3 = Convert.ToInt32(p3, 16);

                    int r = part2 / (i + 1);

                    if (r == 0)
                        return inStr;

                    int charCode =
                        (part1 * 255 + part3) / (r * (i + 1));

                    bytes.Add((byte)charCode);
                }

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                return Encoding
                    .GetEncoding(1256)
                    .GetString(bytes.ToArray());
            }
            catch
            {
                return inStr;
            }
        }
        private bool IsHex(string value)
        {
            return value.All(c =>
                (c >= '0' && c <= '9') ||
                (c >= 'A' && c <= 'F') ||
                (c >= 'a' && c <= 'f')
            );
        }

        private DataTable CreateLoginResultTable(LoginResultDto user)
        {
            DataTable table = new DataTable();

            table.Columns.Add("LoginType", typeof(string));
            table.Columns.Add("Success", typeof(bool));

            table.Columns.Add("UserId", typeof(int));
            table.Columns.Add("OldUserId", typeof(int));
            table.Columns.Add("CentralRef", typeof(int));
            table.Columns.Add("CentralName", typeof(string));
            table.Columns.Add("Manager", typeof(string));
            table.Columns.Add("Delegacy", typeof(string));
            table.Columns.Add("UserName", typeof(string));
            table.Columns.Add("DisplayName", typeof(string));

            table.Columns.Add("DepartmentCode", typeof(int));
            table.Columns.Add("DepartmentName", typeof(string));
            table.Columns.Add("UserMaxDiscount", typeof(decimal));
            table.Columns.Add("UserIdRef", typeof(int));

            table.Columns.Add("SessionId", typeof(string));
            table.Columns.Add("ActiveDate", typeof(string));
            table.Columns.Add("Active", typeof(string));

            table.Columns.Add("XUserCode", typeof(string));
            table.Columns.Add("CustomerCode", typeof(string));
            table.Columns.Add("CustName_Small", typeof(string));
            table.Columns.Add("Explain", typeof(string));
            table.Columns.Add("PersonInfoRef", typeof(string));
            table.Columns.Add("PhFullName", typeof(string));



            table.Columns.Add("ErrCode", typeof(int));
            table.Columns.Add("ErrDesc", typeof(string));
            table.Columns.Add("Message", typeof(string));

            table.Rows.Add(
                "KOWSAR",
                true,

                user.UserId,
                user.OldUserId,
                user.CentralRef,
                user.CentralName,
                user.Manager,
                user.Delegacy,
                user.UserName,
                user.DisplayName,

                user.DepartmentCode,
                user.DepartmentName,
                user.UserMaxDiscount,
                user.UserIdRef,
                user.SessionId,

                
                user.ActiveDate,     // ActiveDate
                "True", // Active
                "", // XUserCode
                "", // CustomerCode
                "", // CustName_Small
                "", // Explain
                "", // PersonInfoRef
                "", // PhFullName



                0,
                "",
                "LOGIN_SUCCESS"
            );

            return table;
        }
        private DataTable CreateMessageTable(string message)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Success", typeof(bool));
            table.Columns.Add("Message", typeof(string));

            table.Rows.Add(false, message);

            return table;
        }






        /*




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




        */








    }
}
