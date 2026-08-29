using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;

namespace webapikits.Controllers.Wedding
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeddingController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<WeddingController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public WeddingController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<WeddingController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }



        /*
  این فایل را کامل داخل WeddingController.cs ادغام کن.
  جای پیشنهادی:
  - DTOها پایین فایل یا داخل فایل DTO جدا
  - APIها داخل کلاس WeddingController
  - Helperهای SqlN/SqlInt/SqlBit/SqlDateTime باید همان Helperهای فعلی Controller باشند.
*/

        // =========================
        // API: THEME
        // =========================
        [HttpPost]
        [Route("WeddingTheme_List")]
        public async Task<IActionResult> WeddingTheme_List([FromBody] WeddingThemeListDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingTheme_List
    @SearchText = {SqlN(dto.SearchText)},
    @OnlyActive = {SqlBit(dto.OnlyActive)}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingTheme_List));
        }

        [HttpPost]
        [Route("WeddingTheme_GetById")]
        public async Task<IActionResult> WeddingTheme_GetById([FromBody] WeddingThemeGetByIdDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingTheme_GetById
    @WeddingThemeId = {dto.WeddingThemeId}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingTheme_GetById));
        }

        [HttpPost]
        [Route("WeddingTheme_Crud")]
        public async Task<IActionResult> WeddingTheme_Crud([FromBody] WeddingThemeCrudDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingTheme_AddOrEdit
    @WeddingThemeId = {dto.WeddingThemeId},
    @ThemeCode = {SqlN(dto.ThemeCode)},
    @ThemeTitle = {SqlN(dto.ThemeTitle)},
    @PrimaryColor = {SqlN(dto.PrimaryColor)},
    @AccentColor = {SqlN(dto.AccentColor)},
    @TextColor = {SqlN(dto.TextColor)},
    @BackgroundColor = {SqlN(dto.BackgroundColor)},
    @FontFamily = {SqlN(dto.FontFamily)},
    @HeroImage1 = {SqlN(dto.HeroImage1)},
    @HeroImage2 = {SqlN(dto.HeroImage2)},
    @CoupleImage = {SqlN(dto.CoupleImage)},
    @FooterImage = {SqlN(dto.FooterImage)},
    @CustomCss = {SqlN(dto.CustomCss)},
    @IsDefault = {SqlBit(dto.IsDefault)},
    @IsActive = {SqlBit(dto.IsActive)},
    @Owner = {SqlInt(dto.Owner)},
    @Reformer = {SqlInt(dto.Reformer)}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingTheme_Crud));
        }

        [HttpPost]
        [Route("WeddingEvent_SetTheme")]
        public async Task<IActionResult> WeddingEvent_SetTheme([FromBody] WeddingEventSetThemeDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingEvent_SetTheme
    @WeddingEventId = {dto.WeddingEventId},
    @WeddingThemeId = {SqlInt(dto.WeddingThemeId)},
    @Reformer = {SqlInt(dto.Reformer)}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingEvent_SetTheme));
        }

        // =========================
        // API: HAMKAR / PEOPLE
        // =========================
        [HttpPost]
        [Route("WeddingPerson_List")]
        public async Task<IActionResult> WeddingPerson_List([FromBody] WeddingPersonListDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingPerson_List
    @WeddingEventId = {dto.WeddingEventId},
    @SearchText = {SqlN(dto.SearchText)},
    @PersonType = {SqlN(dto.PersonType)},
    @OnlyActive = {SqlBit(dto.OnlyActive)}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingPerson_List));
        }

        [HttpPost]
        [Route("WeddingPerson_GetById")]
        public async Task<IActionResult> WeddingPerson_GetById([FromBody] WeddingPersonGetByIdDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingPerson_GetById
    @WeddingPersonId = {dto.WeddingPersonId}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingPerson_GetById));
        }

        [HttpPost]
        [Route("WeddingPerson_Crud")]
        public async Task<IActionResult> WeddingPerson_Crud([FromBody] WeddingPersonCrudDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingPerson_AddOrEdit
    @WeddingPersonId = {dto.WeddingPersonId},
    @WeddingEventId = {dto.WeddingEventId},
    @PersonType = {SqlN(dto.PersonType)},
    @FullName = {SqlN(dto.FullName)},
    @DisplayName = {SqlN(dto.DisplayName)},
    @RoleTitle = {SqlN(dto.RoleTitle)},
    @Mobile = {SqlN(dto.Mobile)},
    @ImageUrl = {SqlN(dto.ImageUrl)},
    @InstagramUrl = {SqlN(dto.InstagramUrl)},
    @Description = {SqlN(dto.Description)},
    @SortOrder = {dto.SortOrder},
    @IsActive = {SqlBit(dto.IsActive)},
    @Owner = {SqlInt(dto.Owner)},
    @Reformer = {SqlInt(dto.Reformer)}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingPerson_Crud));
        }

        // =========================
        // API: EMARAT / VENUE
        // =========================
        [HttpPost]
        [Route("WeddingVenue_List")]
        public async Task<IActionResult> WeddingVenue_List([FromBody] WeddingVenueListDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingVenue_List
    @WeddingEventId = {dto.WeddingEventId},
    @SearchText = {SqlN(dto.SearchText)},
    @VenueType = {SqlN(dto.VenueType)},
    @OnlyActive = {SqlBit(dto.OnlyActive)}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingVenue_List));
        }

        [HttpPost]
        [Route("WeddingVenue_GetById")]
        public async Task<IActionResult> WeddingVenue_GetById([FromBody] WeddingVenueGetByIdDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingVenue_GetById
    @WeddingVenueId = {dto.WeddingVenueId}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingVenue_GetById));
        }

        [HttpPost]
        [Route("WeddingVenue_Crud")]
        public async Task<IActionResult> WeddingVenue_Crud([FromBody] WeddingVenueCrudDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingVenue_AddOrEdit
    @WeddingVenueId = {dto.WeddingVenueId},
    @WeddingEventId = {dto.WeddingEventId},
    @VenueType = {SqlN(dto.VenueType)},
    @VenueTitle = {SqlN(dto.VenueTitle)},
    @EventTimeTitle = {SqlN(dto.EventTimeTitle)},
    @EventDateJalali = {SqlN(dto.EventDateJalali)},
    @EventDateTime = {SqlDateTime(dto.EventDateTime)},
    @Address = {SqlN(dto.Address)},
    @MapUrl = {SqlN(dto.MapUrl)},
    @ImageUrl = {SqlN(dto.ImageUrl)},
    @Description = {SqlN(dto.Description)},
    @SortOrder = {dto.SortOrder},
    @IsMain = {SqlBit(dto.IsMain)},
    @IsActive = {SqlBit(dto.IsActive)},
    @Owner = {SqlInt(dto.Owner)},
    @Reformer = {SqlInt(dto.Reformer)}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingVenue_Crud));
        }

        // =========================
        // DTOs
        // =========================
        public class WeddingThemeListDto
        {
            public string? SearchText { get; set; }
            public bool OnlyActive { get; set; } = false;
        }

        public class WeddingThemeGetByIdDto
        {
            public int WeddingThemeId { get; set; }
        }

        public class WeddingThemeCrudDto
        {
            public int WeddingThemeId { get; set; }
            public string ThemeCode { get; set; } = "";
            public string ThemeTitle { get; set; } = "";
            public string? PrimaryColor { get; set; }
            public string? AccentColor { get; set; }
            public string? TextColor { get; set; }
            public string? BackgroundColor { get; set; }
            public string? FontFamily { get; set; }
            public string? HeroImage1 { get; set; }
            public string? HeroImage2 { get; set; }
            public string? CoupleImage { get; set; }
            public string? FooterImage { get; set; }
            public string? CustomCss { get; set; }
            public bool IsDefault { get; set; } = false;
            public bool IsActive { get; set; } = true;
            public int? Owner { get; set; }
            public int? Reformer { get; set; }
        }

        public class WeddingEventSetThemeDto
        {
            public int WeddingEventId { get; set; }
            public int? WeddingThemeId { get; set; }
            public int? Reformer { get; set; }
        }

        public class WeddingPersonListDto
        {
            public int WeddingEventId { get; set; }
            public string? SearchText { get; set; }
            public string? PersonType { get; set; }
            public bool OnlyActive { get; set; } = false;
        }

        public class WeddingPersonGetByIdDto
        {
            public int WeddingPersonId { get; set; }
        }

        public class WeddingPersonCrudDto
        {
            public int WeddingPersonId { get; set; }
            public int WeddingEventId { get; set; }
            public string PersonType { get; set; } = "PARTNER";
            public string FullName { get; set; } = "";
            public string? DisplayName { get; set; }
            public string? RoleTitle { get; set; }
            public string? Mobile { get; set; }
            public string? ImageUrl { get; set; }
            public string? InstagramUrl { get; set; }
            public string? Description { get; set; }
            public int SortOrder { get; set; } = 0;
            public bool IsActive { get; set; } = true;
            public int? Owner { get; set; }
            public int? Reformer { get; set; }
        }

        public class WeddingVenueListDto
        {
            public int WeddingEventId { get; set; }
            public string? SearchText { get; set; }
            public string? VenueType { get; set; }
            public bool OnlyActive { get; set; } = false;
        }

        public class WeddingVenueGetByIdDto
        {
            public int WeddingVenueId { get; set; }
        }

        public class WeddingVenueCrudDto
        {
            public int WeddingVenueId { get; set; }
            public int WeddingEventId { get; set; }
            public string VenueType { get; set; } = "AROOSI";
            public string VenueTitle { get; set; } = "";
            public string? EventTimeTitle { get; set; }
            public string? EventDateJalali { get; set; }
            public DateTime? EventDateTime { get; set; }
            public string? Address { get; set; }
            public string? MapUrl { get; set; }
            public string? ImageUrl { get; set; }
            public string? Description { get; set; }
            public int SortOrder { get; set; } = 0;
            public bool IsMain { get; set; } = false;
            public bool IsActive { get; set; } = true;
            public int? Owner { get; set; }
            public int? Reformer { get; set; }
        }


























        /// <summary>
        /// /////////////////////////////////////
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>


        [HttpPost]
        [Route("WeddingEvent_Crud")]
        public async Task<IActionResult> WeddingEvent_Crud([FromBody] WeddingEventCrudDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingEvent_AddOrEdit
    @WeddingEventId = {dto.WeddingEventId},
    @EventCode = {SqlN(dto.EventCode)},

    @GroomName = {SqlN(dto.GroomName)},
    @BrideName = {SqlN(dto.BrideName)},

    @EventTitle = {SqlN(dto.EventTitle)},
    @EventDateJalali = {SqlN(dto.EventDateJalali)},
    @EventDateTime = {SqlDateTime(dto.EventDateTime)},

    @MainVenueTitle = {SqlN(dto.MainVenueTitle)},
    @MainVenueAddress = {SqlN(dto.MainVenueAddress)},
    @MainVenueMapUrl = {SqlN(dto.MainVenueMapUrl)},

    @InviteDefaultText = {SqlN(dto.InviteDefaultText)},
    @MixedCeremonyText = {SqlN(dto.MixedCeremonyText)},
    @ChildPolicyText = {SqlN(dto.ChildPolicyText)},

    @IsActive = {SqlBit(dto.IsActive)},

    @Owner = {SqlInt(dto.Owner)},
    @Reformer = {SqlInt(dto.Reformer)}
";

            try
            {
                DataTable dataTable = await db.Wedding_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Weddings", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WeddingEvent_Crud));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("WeddingGuest_Crud")]
        public async Task<IActionResult> WeddingGuest_Crud([FromBody] WeddingGuestCrudDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingGuest_AddOrEdit
    @WeddingGuestId = {dto.WeddingGuestId},

    @WeddingEventId = {dto.WeddingEventId},
    @GuestCode = {SqlN(dto.GuestCode)},
    @GuestToken = {SqlN(dto.GuestToken)},

    @FullName = {SqlN(dto.FullName)},
    @DisplayName = {SqlN(dto.DisplayName)},

    @Mobile = {SqlN(dto.Mobile)},

    @GuestGroup = {SqlN(dto.GuestGroup)},
    @RelationTitle = {SqlN(dto.RelationTitle)},

    @CustomInviteText = {SqlN(dto.CustomInviteText)},

    @IsActive = {SqlBit(dto.IsActive)},

    @Owner = {SqlInt(dto.Owner)},
    @Reformer = {SqlInt(dto.Reformer)}
";

            try
            {
                DataTable dataTable = await db.Wedding_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Weddings", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WeddingGuest_Crud));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("WeddingGuest_Log")]
        public async Task<IActionResult> WeddingGuest_Log([FromBody] WeddingGuestLogDto dto)
        {
            string ipAddress = GetClientIp();
            string userAgent = GetUserAgent();
            string referrer = string.IsNullOrWhiteSpace(dto.ReferrerUrl)
                ? GetReferrer()
                : dto.ReferrerUrl;

            string query = $@"
EXEC dbo.spWeddingGuest_LogInsert
    @GuestCode = {SqlN(dto.GuestCode)},

    @LogType = {SqlN(dto.LogType)},
    @PageName = {SqlN(dto.PageName)},

    @IpAddress = {SqlN(ipAddress)},
    @UserAgent = {SqlN(userAgent)},
    @DeviceInfo = {SqlN(dto.DeviceInfo)},
    @ReferrerUrl = {SqlN(referrer)},

    @GuestToken = {SqlN(dto.GuestToken)}
";

            try
            {
                DataTable dataTable = await db.Wedding_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Weddings", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WeddingGuest_Log));
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("WeddingGuest_Response")]
        public async Task<IActionResult> WeddingGuest_Response([FromBody] WeddingGuestResponseDto dto)
        {
            string attendStatus = (dto.AttendStatus ?? "").Trim().ToUpper();

            if (attendStatus != "YES" && attendStatus != "NO")
            {
                return BadRequest(new
                {
                    ErrCode = 1,
                    ErrMessage = "وضعیت حضور نامعتبر است"
                });
            }

            string ipAddress = GetClientIp();
            string userAgent = GetUserAgent();

            string query = $@"
EXEC dbo.spWeddingGuest_ResponseSave
    @GuestCode = {SqlN(dto.GuestCode)},
    @AttendStatus = {SqlN(attendStatus)},

    @GuestMessage = {SqlN(dto.GuestMessage)},

    @IpAddress = {SqlN(ipAddress)},
    @UserAgent = {SqlN(userAgent)},

    @GuestToken = {SqlN(dto.GuestToken)}
";

            try
            {
                DataTable dataTable = await db.Wedding_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Weddings", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WeddingGuest_Response));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("WeddingGuest_List")]
        public async Task<IActionResult> WeddingGuest_List([FromBody] WeddingGuestListDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingGuest_ListByEvent
    @WeddingEventId = {dto.WeddingEventId},
    @SearchText = {SqlN(dto.SearchText)},
    @AttendStatus = {SqlN(dto.AttendStatus)},
    @SeenStatus = {SqlN(dto.SeenStatus)}
";

            try
            {
                DataTable dataTable = await db.Wedding_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Weddings", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WeddingGuest_List));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("WeddingGuest_GetById")]
        public async Task<IActionResult> WeddingGuest_GetById(string GuestCode)
        {


            string query = $"spWeddingGuest_GetByCode {GuestCode}";

            try
            {
                DataTable dataTable = await db.Wedding_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Weddings", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WeddingGuest_GetById));
                return StatusCode(500, "Internal server error.");
            }
        }












        [HttpPost]
        [Route("WeddingEvent_List")]
        public async Task<IActionResult> WeddingEvent_List()
        {
            string query = @"
EXEC dbo.spWeddingEvent_List
";

            return await ExecuteWeddingQuery(query, nameof(WeddingEvent_List));
        }


        [HttpPost]
        [Route("WeddingEvent_GetById")]
        public async Task<IActionResult> WeddingEvent_GetById([FromBody] WeddingEventGetByIdDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingEvent_GetById
    @WeddingEventId = {dto.WeddingEventId}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingEvent_GetById));
        }




        [HttpPost]
        [Route("WeddingGuest_GetByGuestId")]
        public async Task<IActionResult> WeddingGuest_GetByGuestId([FromBody] WeddingGuestGetByIdDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingGuest_GetById
    @WeddingGuestId = {dto.WeddingGuestId}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingGuest_GetByGuestId));
        }

        [HttpPost]
        [Route("WeddingGuest_Delete")]
        public async Task<IActionResult> WeddingGuest_Delete([FromBody] WeddingGuestDeleteDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingGuest_Delete
    @WeddingGuestId = {dto.WeddingGuestId},
    @WeddingEventId = {dto.WeddingEventId}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingGuest_Delete));
        }
        [HttpPost]
        [Route("WeddingGuest_LogList")]
        public async Task<IActionResult> WeddingGuest_LogList([FromBody] WeddingGuestLogListDto dto)
        {
            string query = $@"
EXEC dbo.spWeddingGuest_LogList
    @WeddingGuestId = {dto.WeddingGuestId}
";

            return await ExecuteWeddingQuery(query, nameof(WeddingGuest_LogList));
        }

        private async Task<IActionResult> ExecuteWeddingQuery(string query, string functionName)
        {
            try
            {
                DataTable dataTable = await db.Wedding_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Weddings", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}. Query: {Query}", functionName, query);

                return StatusCode(500, new
                {
                    ErrCode = 1,
                    ErrMessage = "Internal server error."
                });
            }
        }
        public class WeddingGuestDeleteDto
        {
            public int WeddingGuestId { get; set; }
            public int WeddingEventId { get; set; }
            public int? Reformer { get; set; }
        }
        private static string SqlN(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "NULL";

            return "N'" + value.Replace("'", "''") + "'";
        }

        private static string SqlStr(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "NULL";

            return "'" + value.Replace("'", "''") + "'";
        }

        private static string SqlInt(int? value)
        {
            return value.HasValue ? value.Value.ToString() : "NULL";
        }

        private static string SqlBit(bool value)
        {
            return value ? "1" : "0";
        }

        private static string SqlDateTime(DateTime? value)
        {
            if (!value.HasValue)
                return "NULL";

            return "'" + value.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        }

        private string GetClientIp()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
        }

        private string GetUserAgent()
        {
            return HttpContext.Request.Headers["User-Agent"].ToString();
        }

        private string GetReferrer()
        {
            return HttpContext.Request.Headers["Referer"].ToString();
        }

    }
}