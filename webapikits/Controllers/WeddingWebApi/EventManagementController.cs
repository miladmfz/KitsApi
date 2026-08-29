using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System.Data;
using System.Globalization;
using webapikits.Model;

namespace webapikits.Controllers.Wedding
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventManagementController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<EventManagementController> _logger;
        private readonly IConfiguration _configuration;
        private readonly JsonClass jsonClass = new JsonClass();

        public EventManagementController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<EventManagementController>? logger,
            IConfiguration configuration
        )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger ?? NullLogger<EventManagementController>.Instance;
            _configuration = configuration;
        }

        /* ===============================================================
           Shared executor
        =============================================================== */

        private async Task<IActionResult> ExecuteEventQuery(string query, string functionName)
        {
            try
            {
                DataTable dataTable = await db.Event_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Weddings", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}. Query: {Query}", functionName, query);

                return StatusCode(500, new
                {
                    ErrCode = 1,
                    ErrMessage = "Internal server error.",
                    FunctionName = functionName,
                    ErrorDetail = ex.Message,
                    Query = query
                });
            }
        }

        private static string SqlN(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "NULL";

            return "N'" + value.Replace("'", "''") + "'";
        }

        private static string SqlInt(int? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : "NULL";
        }

        private static string SqlBit(bool value)
        {
            return value ? "1" : "0";
        }

        private static string SqlNullableBit(bool? value)
        {
            if (!value.HasValue)
                return "NULL";

            return value.Value ? "1" : "0";
        }

        private static string SqlDecimal(decimal? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : "NULL";
        }

        private static string SqlDateTime(DateTime? value)
        {
            if (!value.HasValue)
                return "NULL";

            return "'" + value.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'";
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

        /* ===============================================================
           Dashboard
        =============================================================== */

        [HttpPost]
        [Route("EventMng_Dashboard")]
        public async Task<IActionResult> EventMng_Dashboard()
        {
            return await ExecuteEventQuery("EXEC dbo.spEventMng_Dashboard", nameof(EventMng_Dashboard));
        }

        /* ===============================================================
           Master data
        =============================================================== */

        [HttpPost]
        [Route("EventMng_Master_List")]
        public async Task<IActionResult> EventMng_Master_List([FromBody] EventMngMasterListDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Master_List
    @MasterType = {SqlN(dto.MasterType)},
    @SearchText = {SqlN(dto.SearchText)},
    @OnlyActive = {SqlBit(dto.OnlyActive)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Master_List));
        }

        [HttpPost]
        [Route("EventMng_Master_GetById")]
        public async Task<IActionResult> EventMng_Master_GetById([FromBody] EventMngIdDto dto)
        {
            return await ExecuteEventQuery($"EXEC dbo.spEventMng_Master_GetById @MasterId = {dto.Id}", nameof(EventMng_Master_GetById));
        }

        [HttpPost]
        [Route("EventMng_Master_Save")]
        public async Task<IActionResult> EventMng_Master_Save([FromBody] EventMngMasterSaveDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Master_Save
    @MasterId = {dto.MasterId},
    @MasterType = {SqlN(dto.MasterType)},
    @Code = {SqlN(dto.Code)},
    @Title = {SqlN(dto.Title)},
    @Description = {SqlN(dto.Description)},
    @ColorCode = {SqlN(dto.ColorCode)},
    @SortOrder = {dto.SortOrder},
    @IsActive = {SqlBit(dto.IsActive)},
    @Owner = {SqlInt(dto.Owner)},
    @Reformer = {SqlInt(dto.Reformer)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Master_Save));
        }

        [HttpPost]
        [Route("EventMng_Master_Delete")]
        public async Task<IActionResult> EventMng_Master_Delete([FromBody] EventMngIdDto dto)
        {
            string query = $"EXEC dbo.spEventMng_Master_Delete @MasterId = {dto.Id}";
            return await ExecuteEventQuery(query, nameof(EventMng_Master_Delete));
        }

        /* ===============================================================
           Catalogs
        =============================================================== */

        [HttpPost]
        [Route("EventMng_Catalog_List")]
        public async Task<IActionResult> EventMng_Catalog_List([FromBody] EventMngCatalogListDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Catalog_List
    @CatalogType = {SqlN(dto.CatalogType)},
    @SearchText = {SqlN(dto.SearchText)},
    @OnlyActive = {SqlBit(dto.OnlyActive)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Catalog_List));
        }

        [HttpPost]
        [Route("EventMng_Catalog_GetById")]
        public async Task<IActionResult> EventMng_Catalog_GetById([FromBody] EventMngCatalogEntityDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Catalog_GetById
    @CatalogType = {SqlN(dto.CatalogType)},
    @EntityId = {dto.EntityId}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Catalog_GetById));
        }

        [HttpPost]
        [Route("EventMng_Catalog_Save")]
        public async Task<IActionResult> EventMng_Catalog_Save([FromBody] EventMngCatalogSaveDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Catalog_Save
    @CatalogType = {SqlN(dto.CatalogType)},
    @EntityId = {dto.EntityId},
    @ParentRef = {SqlInt(dto.ParentRef)},
    @Code = {SqlN(dto.Code)},
    @Title = {SqlN(dto.Title)},
    @UnitTitle = {SqlN(dto.UnitTitle)},
    @Mobile = {SqlN(dto.Mobile)},
    @Email = {SqlN(dto.Email)},
    @Address = {SqlN(dto.Address)},
    @MapUrl = {SqlN(dto.MapUrl)},
    @ImageUrl = {SqlN(dto.ImageUrl)},
    @BasePrice = {SqlDecimal(dto.BasePrice)},
    @Qty = {SqlDecimal(dto.Qty)},
    @Description = {SqlN(dto.Description)},
    @SortOrder = {dto.SortOrder},
    @IsActive = {SqlBit(dto.IsActive)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Catalog_Save));
        }

        [HttpPost]
        [Route("EventMng_Catalog_Delete")]
        public async Task<IActionResult> EventMng_Catalog_Delete([FromBody] EventMngCatalogEntityDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Catalog_Delete
    @CatalogType = {SqlN(dto.CatalogType)},
    @EntityId = {dto.EntityId}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Catalog_Delete));
        }

        /* ===============================================================
           Customers
        =============================================================== */

        [HttpPost]
        [Route("EventMng_Customer_List")]
        public async Task<IActionResult> EventMng_Customer_List([FromBody] EventMngCustomerListDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Customer_List
    @SearchText = {SqlN(dto.SearchText)},
    @IsActive = {SqlNullableBit(dto.IsActive)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Customer_List));
        }

        [HttpPost]
        [Route("EventMng_Customer_GetById")]
        public async Task<IActionResult> EventMng_Customer_GetById([FromBody] EventMngIdDto dto)
        {
            return await ExecuteEventQuery($"EXEC dbo.spEventMng_Customer_GetById @CustomerId = {dto.Id}", nameof(EventMng_Customer_GetById));
        }

        [HttpPost]
        [Route("EventMng_Customer_Save")]
        public async Task<IActionResult> EventMng_Customer_Save([FromBody] EventMngCustomerSaveDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Customer_Save
    @CustomerId = {dto.CustomerId},
    @CustomerCode = {SqlN(dto.CustomerCode)},
    @FullName = {SqlN(dto.FullName)},
    @Mobile = {SqlN(dto.Mobile)},
    @Email = {SqlN(dto.Email)},
    @CustomerTypeRef = {SqlInt(dto.CustomerTypeRef)},
    @LeadSourceRef = {SqlInt(dto.LeadSourceRef)},
    @CompanyName = {SqlN(dto.CompanyName)},
    @Address = {SqlN(dto.Address)},
    @Notes = {SqlN(dto.Notes)},
    @IsActive = {SqlBit(dto.IsActive)},
    @Owner = {SqlInt(dto.Owner)},
    @Reformer = {SqlInt(dto.Reformer)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Customer_Save));
        }

        [HttpPost]
        [Route("EventMng_Customer_Delete")]
        public async Task<IActionResult> EventMng_Customer_Delete([FromBody] EventMngIdDto dto)
        {
            string query = $"EXEC dbo.spEventMng_Customer_Delete @CustomerId = {dto.Id}";
            return await ExecuteEventQuery(query, nameof(EventMng_Customer_Delete));
        }

        /* ===============================================================
           Events
        =============================================================== */

        [HttpPost]
        [Route("EventMng_Event_List")]
        public async Task<IActionResult> EventMng_Event_List([FromBody] EventMngEventListDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Event_List
    @SearchText = {SqlN(dto.SearchText)},
    @EventStatusRef = {SqlInt(dto.EventStatusRef)},
    @EventTypeRef = {SqlInt(dto.EventTypeRef)},
    @CustomerRef = {SqlInt(dto.CustomerRef)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Event_List));
        }

        [HttpPost]
        [Route("EventMng_Event_GetById")]
        public async Task<IActionResult> EventMng_Event_GetById([FromBody] EventMngIdDto dto)
        {
            return await ExecuteEventQuery($"EXEC dbo.spEventMng_Event_GetById @EventId = {dto.Id}", nameof(EventMng_Event_GetById));
        }

        [HttpPost]
        [Route("EventMng_Event_Save")]
        public async Task<IActionResult> EventMng_Event_Save([FromBody] EventMngEventSaveDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Event_Save
    @EventId = {dto.EventId},
    @CustomerRef = {SqlInt(dto.CustomerRef)},
    @EventCode = {SqlN(dto.EventCode)},
    @EventTitle = {SqlN(dto.EventTitle)},
    @EventTypeRef = {SqlInt(dto.EventTypeRef)},
    @EventStatusRef = {SqlInt(dto.EventStatusRef)},
    @EventDateJalali = {SqlN(dto.EventDateJalali)},
    @EventStartDateTime = {SqlDateTime(dto.EventStartDateTime)},
    @EventEndDateTime = {SqlDateTime(dto.EventEndDateTime)},
    @MainVenueTitle = {SqlN(dto.MainVenueTitle)},
    @GuestEstimate = {SqlInt(dto.GuestEstimate)},
    @BudgetAmount = {SqlDecimal(dto.BudgetAmount)},
    @ServicePackageRef = {SqlInt(dto.ServicePackageRef)},
    @DisplayTemplateCode = {SqlN(dto.DisplayTemplateCode)},
    @PublicSlug = {SqlN(dto.PublicSlug)},
    @PublicIsActive = {SqlBit(dto.PublicIsActive)},
    @PublicIntroText = {SqlN(dto.PublicIntroText)},
    @PublicClosingText = {SqlN(dto.PublicClosingText)},
    @PublicCoverImageUrl = {SqlN(dto.PublicCoverImageUrl)},
    @PublicPrimaryColor = {SqlN(dto.PublicPrimaryColor)},
    @PublicSecondaryColor = {SqlN(dto.PublicSecondaryColor)},
    @DressCodeText = {SqlN(dto.DressCodeText)},
    @GiftText = {SqlN(dto.GiftText)},
    @Notes = {SqlN(dto.Notes)},
    @IsActive = {SqlBit(dto.IsActive)},
    @Owner = {SqlInt(dto.Owner)},
    @Reformer = {SqlInt(dto.Reformer)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Event_Save));
        }

        [HttpPost]
        [Route("EventMng_Event_Delete")]
        public async Task<IActionResult> EventMng_Event_Delete([FromBody] EventMngEventIdDto dto)
        {
            string query = $"EXEC dbo.spEventMng_Event_Delete @EventId = {dto.EventId}";
            return await ExecuteEventQuery(query, nameof(EventMng_Event_Delete));
        }

        /* ===============================================================
           Guests
        =============================================================== */

        [HttpPost]
        [Route("EventMng_Guest_List")]
        public async Task<IActionResult> EventMng_Guest_List([FromBody] EventMngGuestListDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Guest_List
    @EventId = {dto.EventId},
    @SearchText = {SqlN(dto.SearchText)},
    @AttendStatus = {SqlN(dto.AttendStatus)},
    @SeenStatus = {SqlN(dto.SeenStatus)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Guest_List));
        }

        [HttpPost]
        [Route("EventMng_Guest_GetById")]
        public async Task<IActionResult> EventMng_Guest_GetById([FromBody] EventMngIdDto dto)
        {
            return await ExecuteEventQuery($"EXEC dbo.spEventMng_Guest_GetById @GuestId = {dto.Id}", nameof(EventMng_Guest_GetById));
        }

        [HttpPost]
        [Route("EventMng_Guest_Save")]
        public async Task<IActionResult> EventMng_Guest_Save([FromBody] EventMngGuestSaveDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Guest_Save
    @GuestId = {dto.GuestId},
    @EventId = {dto.EventId},
    @UnitCode = {SqlN(dto.UnitCode)},
    @FullName = {SqlN(dto.FullName)},
    @DisplayName = {SqlN(dto.DisplayName)},
    @Mobile = {SqlN(dto.Mobile)},
    @GuestGroup = {SqlN(dto.GuestGroup)},
    @RelationTitle = {SqlN(dto.RelationTitle)},
    @CompanionCountAllowed = {dto.CompanionCountAllowed},
    @CustomInviteText = {SqlN(dto.CustomInviteText)},
    @IsInvited = {SqlBit(dto.IsInvited)},
    @IsActive = {SqlBit(dto.IsActive)},
    @Owner = {SqlInt(dto.Owner)},
    @Reformer = {SqlInt(dto.Reformer)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Guest_Save));
        }

        [HttpPost]
        [Route("EventMng_Guest_LogList")]
        public async Task<IActionResult> EventMng_Guest_LogList([FromBody] EventMngGuestLogListDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Guest_LogList
    @EventId = {dto.EventId},
    @GuestId = {SqlInt(dto.GuestId)},
    @UnitCode = {SqlN(dto.UnitCode)},
    @Top = {SqlInt(dto.Top)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Guest_LogList));
        }

        [HttpPost]
        [Route("EventMng_Guest_Delete")]
        public async Task<IActionResult> EventMng_Guest_Delete([FromBody] EventMngGuestDeleteDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Guest_Delete
    @EventId = {dto.EventId},
    @GuestId = {dto.GuestId}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Guest_Delete));
        }

        /* ===============================================================
           Workspace
        =============================================================== */

        [HttpPost]
        [Route("EventMng_EventWorkspace_Full")]
        public async Task<IActionResult> EventMng_EventWorkspace_Full([FromBody] EventMngEventIdDto dto)
        {
            return await ExecuteEventQuery($"EXEC dbo.spEventMng_EventWorkspace_Full @EventId = {dto.EventId}", nameof(EventMng_EventWorkspace_Full));
        }

        [HttpPost]
        [Route("EventMng_Event_ApplyServicePackage")]
        public async Task<IActionResult> EventMng_Event_ApplyServicePackage([FromBody] EventMngApplyPackageDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Event_ApplyServicePackage
    @EventId = {dto.EventId},
    @ServicePackageId = {dto.ServicePackageId},
    @ReplaceExisting = {SqlBit(dto.ReplaceExisting)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Event_ApplyServicePackage));
        }

        [HttpPost]
        [Route("EventMng_Event_PublicSettings_Save")]
        public async Task<IActionResult> EventMng_Event_PublicSettings_Save([FromBody] EventMngPublicSettingsDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Event_PublicSettings_Save
    @EventId = {dto.EventId},
    @DisplayTemplateCode = {SqlN(dto.DisplayTemplateCode)},
    @PublicSlug = {SqlN(dto.PublicSlug)},
    @PublicIsActive = {SqlBit(dto.PublicIsActive)},
    @PublicIntroText = {SqlN(dto.PublicIntroText)},
    @PublicClosingText = {SqlN(dto.PublicClosingText)},
    @PublicCoverImageUrl = {SqlN(dto.PublicCoverImageUrl)},
    @PublicPrimaryColor = {SqlN(dto.PublicPrimaryColor)},
    @PublicSecondaryColor = {SqlN(dto.PublicSecondaryColor)},
    @DressCodeText = {SqlN(dto.DressCodeText)},
    @GiftText = {SqlN(dto.GiftText)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_Event_PublicSettings_Save));
        }

        [HttpPost]
        [Route("EventMng_TemplateDisplay_List")]
        public async Task<IActionResult> EventMng_TemplateDisplay_List()
        {
            return await ExecuteEventQuery("EXEC dbo.spEventMng_TemplateDisplay_List", nameof(EventMng_TemplateDisplay_List));
        }

        [HttpPost]
        [Route("EventMng_EventTask_List")]
        public async Task<IActionResult> EventMng_EventTask_List([FromBody] EventMngEventIdDto dto)
        {
            return await ExecuteEventQuery($"EXEC dbo.spEventMng_EventTask_List @EventId = {dto.EventId}", nameof(EventMng_EventTask_List));
        }

        [HttpPost]
        [Route("EventMng_EventTask_Save")]
        public async Task<IActionResult> EventMng_EventTask_Save([FromBody] EventMngEventTaskSaveDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_EventTask_Save
    @EventTaskId = {dto.EventTaskId},
    @EventId = {dto.EventId},
    @Title = {SqlN(dto.Title)},
    @TaskDescription = {SqlN(dto.TaskDescription)},
    @Priority = {SqlN(dto.Priority)},
    @TaskStatus = {SqlN(dto.TaskStatus)},
    @AssignedStaffRef = {SqlInt(dto.AssignedStaffRef)},
    @DueDate = {SqlN(dto.DueDate)},
    @SortOrder = {dto.SortOrder}
";
            return await ExecuteEventQuery(query, nameof(EventMng_EventTask_Save));
        }

        [HttpPost]
        [Route("EventMng_EventTask_SetStatus")]
        public async Task<IActionResult> EventMng_EventTask_SetStatus([FromBody] EventMngTaskStatusDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_EventTask_SetStatus
    @EventTaskId = {dto.EventTaskId},
    @TaskStatus = {SqlN(dto.TaskStatus)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_EventTask_SetStatus));
        }

        [HttpPost]
        [Route("EventMng_EventTask_Delete")]
        public async Task<IActionResult> EventMng_EventTask_Delete([FromBody] EventMngDeleteDto dto)
        {
            return await ExecuteEventQuery($"EXEC dbo.spEventMng_EventTask_Delete @Id = {dto.Id}", nameof(EventMng_EventTask_Delete));
        }

        [HttpPost]
        [Route("EventMng_EventTeam_Save")]
        public async Task<IActionResult> EventMng_EventTeam_Save([FromBody] EventMngTeamSaveDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_EventTeam_Save
    @EventTeamId = {dto.EventTeamId},
    @EventId = {dto.EventId},
    @TeamTitle = {SqlN(dto.TeamTitle)},
    @RequiredCount = {dto.RequiredCount},
    @Description = {SqlN(dto.Description)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_EventTeam_Save));
        }

        [HttpPost]
        [Route("EventMng_EventTeam_Delete")]
        public async Task<IActionResult> EventMng_EventTeam_Delete([FromBody] EventMngDeleteDto dto)
        {
            return await ExecuteEventQuery($"EXEC dbo.spEventMng_EventTeam_Delete @Id = {dto.Id}", nameof(EventMng_EventTeam_Delete));
        }

        [HttpPost]
        [Route("EventMng_EventVenue_Save")]
        public async Task<IActionResult> EventMng_EventVenue_Save([FromBody] EventMngVenueSaveDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_EventVenue_Save
    @EventVenueId = {dto.EventVenueId},
    @EventId = {dto.EventId},
    @VenueTitle = {SqlN(dto.VenueTitle)},
    @ProgramTitle = {SqlN(dto.ProgramTitle)},
    @StartTime = {SqlN(dto.StartTime)},
    @EndTime = {SqlN(dto.EndTime)},
    @Address = {SqlN(dto.Address)},
    @MapUrl = {SqlN(dto.MapUrl)},
    @ImageUrl = {SqlN(dto.ImageUrl)},
    @Description = {SqlN(dto.Description)},
    @SortOrder = {dto.SortOrder}
";
            return await ExecuteEventQuery(query, nameof(EventMng_EventVenue_Save));
        }

        [HttpPost]
        [Route("EventMng_EventVenue_Delete")]
        public async Task<IActionResult> EventMng_EventVenue_Delete([FromBody] EventMngDeleteDto dto)
        {
            return await ExecuteEventQuery($"EXEC dbo.spEventMng_EventVenue_Delete @Id = {dto.Id}", nameof(EventMng_EventVenue_Delete));
        }

        [HttpPost]
        [Route("EventMng_EventServiceItem_Save")]
        public async Task<IActionResult> EventMng_EventServiceItem_Save([FromBody] EventMngServiceItemSaveDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_EventServiceItem_Save
    @EventServiceItemId = {dto.EventServiceItemId},
    @EventId = {dto.EventId},
    @Title = {SqlN(dto.Title)},
    @Qty = {SqlDecimal(dto.Qty)},
    @UnitPrice = {SqlDecimal(dto.UnitPrice)},
    @Description = {SqlN(dto.Description)},
    @SortOrder = {dto.SortOrder}
";
            return await ExecuteEventQuery(query, nameof(EventMng_EventServiceItem_Save));
        }

        [HttpPost]
        [Route("EventMng_EventServiceItem_Delete")]
        public async Task<IActionResult> EventMng_EventServiceItem_Delete([FromBody] EventMngDeleteDto dto)
        {
            return await ExecuteEventQuery($"EXEC dbo.spEventMng_EventServiceItem_Delete @Id = {dto.Id}", nameof(EventMng_EventServiceItem_Delete));
        }

        [HttpPost]
        [Route("EventMng_EventSchedule_Save")]
        public async Task<IActionResult> EventMng_EventSchedule_Save([FromBody] EventMngScheduleSaveDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_EventSchedule_Save
    @EventScheduleId = {dto.EventScheduleId},
    @EventId = {dto.EventId},
    @ScheduleTitle = {SqlN(dto.ScheduleTitle)},
    @ScheduleTimeTitle = {SqlN(dto.ScheduleTimeTitle)},
    @LocationTitle = {SqlN(dto.LocationTitle)},
    @Description = {SqlN(dto.Description)},
    @SortOrder = {dto.SortOrder}
";
            return await ExecuteEventQuery(query, nameof(EventMng_EventSchedule_Save));
        }

        [HttpPost]
        [Route("EventMng_EventSchedule_Delete")]
        public async Task<IActionResult> EventMng_EventSchedule_Delete([FromBody] EventMngDeleteDto dto)
        {
            return await ExecuteEventQuery($"EXEC dbo.spEventMng_EventSchedule_Delete @Id = {dto.Id}", nameof(EventMng_EventSchedule_Delete));
        }

        [HttpPost]
        [Route("EventMng_EventGallery_Save")]
        public async Task<IActionResult> EventMng_EventGallery_Save([FromBody] EventMngGallerySaveDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_EventGallery_Save
    @EventGalleryId = {dto.EventGalleryId},
    @EventId = {dto.EventId},
    @ImageUrl = {SqlN(dto.ImageUrl)},
    @ImageTitle = {SqlN(dto.ImageTitle)},
    @ImageAlt = {SqlN(dto.ImageAlt)},
    @SortOrder = {dto.SortOrder}
";
            return await ExecuteEventQuery(query, nameof(EventMng_EventGallery_Save));
        }

        [HttpPost]
        [Route("EventMng_EventGallery_Delete")]
        public async Task<IActionResult> EventMng_EventGallery_Delete([FromBody] EventMngDeleteDto dto)
        {
            return await ExecuteEventQuery($"EXEC dbo.spEventMng_EventGallery_Delete @Id = {dto.Id}", nameof(EventMng_EventGallery_Delete));
        }
        [HttpPost]
        [Route("EventMng_PublicInvite_LogVisit")]
        public async Task<IActionResult> EventMng_PublicInvite_LogVisit([FromBody] EventMngPublicInviteLogVisitDto dto)
        {
            string ipAddress = GetClientIp();
            string userAgent = GetUserAgent();

            string logType = string.IsNullOrWhiteSpace(dto.LogType)
                ? "PAGE_VIEW"
                : dto.LogType.Trim();

            string pageName = string.IsNullOrWhiteSpace(dto.PageName)
                ? "public-invite"
                : dto.PageName.Trim();

            string query = $@"
EXEC dbo.spEventMng_Guest_LogInsert
    @UnitCode = {SqlN(dto.UnitCode)},
    @LogType = {SqlN(logType)},
    @PageName = {SqlN(pageName)},
    @IpAddress = {SqlN(ipAddress)},
    @UserAgent = {SqlN(userAgent)},
    @DeviceInfo = {SqlN(dto.DeviceInfo)},
    @ReferrerUrl = {SqlN(dto.ReferrerUrl)}
";

            return await ExecuteEventQuery(query, nameof(EventMng_PublicInvite_LogVisit));
        }
        /* ===============================================================
           Public invite - UnitCode based
        =============================================================== */

        [HttpPost]
        [Route("EventMng_PublicInvite_GetByUnitCode")]
        public async Task<IActionResult> EventMng_PublicInvite_GetByUnitCode([FromBody] PublicInviteDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_PublicInvite_GetByUnitCode
    @UnitCode = {SqlN(dto.UnitCode)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_PublicInvite_GetByUnitCode));
        }

        [HttpPost]
        [Route("EventMng_PublicInvite_Log")]
        public async Task<IActionResult> EventMng_PublicInvite_Log([FromBody] PublicInviteLogDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_Guest_LogInsert
    @UnitCode = {SqlN(dto.UnitCode)},
    @LogType = {SqlN(string.IsNullOrWhiteSpace(dto.LogType) ? "PAGE_VIEW" : dto.LogType)},
    @PageName = {SqlN(dto.PageName)},
    @IpAddress = {SqlN(GetClientIp())},
    @UserAgent = {SqlN(GetUserAgent())},
    @DeviceInfo = {SqlN(dto.DeviceInfo)},
    @ReferrerUrl = {SqlN(string.IsNullOrWhiteSpace(dto.ReferrerUrl) ? GetReferrer() : dto.ReferrerUrl)}
";
            return await ExecuteEventQuery(query, nameof(EventMng_PublicInvite_Log));
        }

        [HttpPost]
        [Route("EventMng_PublicInvite_SaveResponse")]
        public async Task<IActionResult> EventMng_PublicInvite_SaveResponse([FromBody] PublicInviteResponseDto dto)
        {
            string query = $@"
EXEC dbo.spEventMng_PublicInvite_SaveResponse
    @UnitCode = {SqlN(dto.UnitCode)},
    @AttendStatus = {SqlN(dto.AttendStatus)},
    @CompanionCount = {dto.CompanionCount},
    @GuestMessage = {SqlN(dto.GuestMessage)},
    @IpAddress = {SqlN(GetClientIp())},
    @UserAgent = {SqlN(GetUserAgent())}
";
            return await ExecuteEventQuery(query, nameof(EventMng_PublicInvite_SaveResponse));
        }
    }

    /* ===============================================================
       DTOs
    =============================================================== */

    public class EventMngIdDto { public int Id { get; set; } }
    public class EventMngDeleteDto { public int Id { get; set; } }
    public class EventMngEventIdDto { public int EventId { get; set; } }

    public class EventMngCatalogEntityDto
    {
        public string CatalogType { get; set; } = "";
        public int EntityId { get; set; }
    }

    public class EventMngGuestDeleteDto
    {
        public int EventId { get; set; }
        public int GuestId { get; set; }
    }

    public class EventMngMasterListDto
    {
        public string MasterType { get; set; } = "";
        public string? SearchText { get; set; }
        public bool OnlyActive { get; set; } = true;
    }

    public class EventMngMasterSaveDto
    {
        public int MasterId { get; set; }
        public string MasterType { get; set; } = "";
        public string Code { get; set; } = "";
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string? ColorCode { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Owner { get; set; }
        public int? Reformer { get; set; }
    }

    public class EventMngCatalogListDto
    {
        public string CatalogType { get; set; } = "";
        public string? SearchText { get; set; }
        public bool OnlyActive { get; set; } = true;
    }

    public class EventMngCatalogSaveDto
    {
        public string CatalogType { get; set; } = "";
        public int EntityId { get; set; }
        public int? ParentRef { get; set; }
        public string? Code { get; set; }
        public string Title { get; set; } = "";
        public string? UnitTitle { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? MapUrl { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal? Qty { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class EventMngCustomerListDto
    {
        public string? SearchText { get; set; }
        public bool? IsActive { get; set; }
    }

    public class EventMngCustomerSaveDto
    {
        public int CustomerId { get; set; }
        public string? CustomerCode { get; set; }
        public string FullName { get; set; } = "";
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public int? CustomerTypeRef { get; set; }
        public int? LeadSourceRef { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Owner { get; set; }
        public int? Reformer { get; set; }
    }

    public class EventMngEventListDto
    {
        public string? SearchText { get; set; }
        public int? EventStatusRef { get; set; }
        public int? EventTypeRef { get; set; }
        public int? CustomerRef { get; set; }
    }

    public class EventMngEventSaveDto
    {
        public int EventId { get; set; }
        public int? CustomerRef { get; set; }
        public string? EventCode { get; set; }
        public string EventTitle { get; set; } = "";
        public int? EventTypeRef { get; set; }
        public int? EventStatusRef { get; set; }
        public string? EventDateJalali { get; set; }
        public DateTime? EventStartDateTime { get; set; }
        public DateTime? EventEndDateTime { get; set; }
        public string? MainVenueTitle { get; set; }
        public int? GuestEstimate { get; set; }
        public decimal? BudgetAmount { get; set; }
        public int? ServicePackageRef { get; set; }
        public string? DisplayTemplateCode { get; set; } = "romantic";
        public string? PublicSlug { get; set; }
        public bool PublicIsActive { get; set; } = true;
        public string? PublicIntroText { get; set; }
        public string? PublicClosingText { get; set; }
        public string? PublicCoverImageUrl { get; set; }
        public string? PublicPrimaryColor { get; set; }
        public string? PublicSecondaryColor { get; set; }
        public string? DressCodeText { get; set; }
        public string? GiftText { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Owner { get; set; }
        public int? Reformer { get; set; }
    }

    public class EventMngGuestListDto
    {
        public int EventId { get; set; }
        public string? SearchText { get; set; }
        public string? AttendStatus { get; set; }
        public string? SeenStatus { get; set; }
    }

    public class EventMngGuestSaveDto
    {
        public int GuestId { get; set; }
        public int EventId { get; set; }
        public string? UnitCode { get; set; }
        public string FullName { get; set; } = "";
        public string? DisplayName { get; set; }
        public string? Mobile { get; set; }
        public string? GuestGroup { get; set; }
        public string? RelationTitle { get; set; }
        public int CompanionCountAllowed { get; set; }
        public string? CustomInviteText { get; set; }
        public bool IsInvited { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public int? Owner { get; set; }
        public int? Reformer { get; set; }
    }

    public class EventMngGuestLogListDto
    {
        public int EventId { get; set; }
        public int? GuestId { get; set; }
        public string? UnitCode { get; set; }
        public int? Top { get; set; }
    }

    public class EventMngApplyPackageDto
    {
        public int EventId { get; set; }
        public int ServicePackageId { get; set; }
        public bool ReplaceExisting { get; set; }
    }

    public class EventMngPublicSettingsDto
    {
        public int EventId { get; set; }
        public string? DisplayTemplateCode { get; set; }
        public string? PublicSlug { get; set; }
        public bool PublicIsActive { get; set; } = true;
        public string? PublicIntroText { get; set; }
        public string? PublicClosingText { get; set; }
        public string? PublicCoverImageUrl { get; set; }
        public string? PublicPrimaryColor { get; set; }
        public string? PublicSecondaryColor { get; set; }
        public string? DressCodeText { get; set; }
        public string? GiftText { get; set; }
    }

    public class EventMngEventTaskSaveDto
    {
        public int EventTaskId { get; set; }
        public int EventId { get; set; }
        public string Title { get; set; } = "";
        public string? TaskDescription { get; set; }
        public string? Priority { get; set; } = "NORMAL";
        public string? TaskStatus { get; set; } = "PENDING";
        public int? AssignedStaffRef { get; set; }
        public string? DueDate { get; set; }
        public int SortOrder { get; set; }
    }

    public class EventMngTaskStatusDto
    {
        public int EventTaskId { get; set; }
        public string TaskStatus { get; set; } = "PENDING";
    }

    public class EventMngTeamSaveDto
    {
        public int EventTeamId { get; set; }
        public int EventId { get; set; }
        public string TeamTitle { get; set; } = "";
        public int RequiredCount { get; set; } = 1;
        public string? Description { get; set; }
    }

    public class EventMngVenueSaveDto
    {
        public int EventVenueId { get; set; }
        public int EventId { get; set; }
        public string VenueTitle { get; set; } = "";
        public string? ProgramTitle { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? Address { get; set; }
        public string? MapUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; }
    }

    public class EventMngServiceItemSaveDto
    {
        public int EventServiceItemId { get; set; }
        public int EventId { get; set; }
        public string Title { get; set; } = "";
        public decimal? Qty { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; }
    }

    public class EventMngScheduleSaveDto
    {
        public int EventScheduleId { get; set; }
        public int EventId { get; set; }
        public string ScheduleTitle { get; set; } = "";
        public string? ScheduleTimeTitle { get; set; }
        public string? LocationTitle { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; }
    }

    public class EventMngGallerySaveDto
    {
        public int EventGalleryId { get; set; }
        public int EventId { get; set; }
        public string ImageUrl { get; set; } = "";
        public string? ImageTitle { get; set; }
        public string? ImageAlt { get; set; }
        public int SortOrder { get; set; }
    }

    public class PublicInviteDto
    {
        public string UnitCode { get; set; } = "";
    }

    public class PublicInviteLogDto
    {
        public string UnitCode { get; set; } = "";
        public string? LogType { get; set; }
        public string? PageName { get; set; }
        public string? DeviceInfo { get; set; }
        public string? ReferrerUrl { get; set; }
    }

    public class PublicInviteResponseDto
    {
        public string UnitCode { get; set; } = "";
        public string AttendStatus { get; set; } = "";
        public int CompanionCount { get; set; }
        public string? GuestMessage { get; set; }
    }
    public class EventMngPublicInviteLogVisitDto
    {
        public string UnitCode { get; set; } = "";
        public string? LogType { get; set; }
        public string? PageName { get; set; }
        public string? DeviceInfo { get; set; }
        public string? ReferrerUrl { get; set; }
    }
}
