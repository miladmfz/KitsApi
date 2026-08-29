using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;

namespace webapikits.Controllers.KowsarBaseWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkforceAbsenceController : ControllerBase
    {
        private readonly IDbService db;
        private readonly ILogger<WorkforceAbsenceController> _logger;
        private readonly IConfiguration _configuration;
        private readonly JsonClass jsonClass = new JsonClass();

        public WorkforceAbsenceController(
            IDbService dbService,
            ILogger<WorkforceAbsenceController> logger,
            IConfiguration configuration)
        {
            db = dbService;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Type_Get")]
        public async Task<IActionResult> Type_Get([FromBody] WorkforceAbsenceTypeDto dto)
        {
            string query = $@"
EXEC dbo.spWeb_WorkforceAbsenceType_Get
    @AbsenceTypeCode = {SqlInt(dto.AbsenceTypeCode)},
    @OnlyActive = {SqlNullableBit(dto.OnlyActive)}";

            return await ExecuteQuery(query, "WorkforceAbsenceTypes", nameof(Type_Get));
        }

        [HttpPost]
        [Route("Type_Save")]
        public async Task<IActionResult> Type_Save([FromBody] WorkforceAbsenceTypeDto dto)
        {
            string query = $@"
EXEC dbo.spWeb_WorkforceAbsenceType_Save
    @AbsenceTypeCode = {SqlInt(dto.AbsenceTypeCode)},
    @TypeKey = {SqlText(dto.TypeKey)},
    @TypeTitle = {SqlText(dto.TypeTitle)},
    @CalculationMode = {SqlText(dto.CalculationMode)},
    @BalanceMode = {SqlText(dto.BalanceMode)},
    @MinimumAdvanceWorkDay = {SqlInt(dto.MinimumAdvanceWorkDay)},
    @AllowFriday = {SqlBit(dto.AllowFriday)},
    @AllowHoliday = {SqlBit(dto.AllowHoliday)},
    @RequireAttachment = {SqlBit(dto.RequireAttachment)},
    @AttachmentRequiredAfterDay = {SqlDecimal(dto.AttachmentRequiredAfterDay)},
    @RequireDescription = {SqlBit(dto.RequireDescription, true)},
    @DeductFromBalance = {SqlBit(dto.DeductFromBalance, true)},
    @AllowFullTime = {SqlBit(dto.AllowFullTime, true)},
    @AllowPartTime = {SqlBit(dto.AllowPartTime, true)},
    @AllowShift = {SqlBit(dto.AllowShift, true)},
    @AllowCustom = {SqlBit(dto.AllowCustom, true)},
    @HelpText = {SqlText(dto.HelpText)},
    @DisplayOrder = {SqlInt(dto.DisplayOrder)},
    @IsActive = {SqlBit(dto.IsActive, true)}";

            return await ExecuteQuery(query, "WorkforceAbsenceTypes", nameof(Type_Save));
        }

        [HttpPost]
        [Route("Policy_Get")]
        public async Task<IActionResult> Policy_Get([FromBody] WorkforceAbsencePolicyDto dto)
        {
            string query = $@"
EXEC dbo.spWeb_WorkforceAbsenceUserPolicy_Get
    @CentralRef = {SqlInt(dto.CentralRef)},
    @OnlyActive = {SqlNullableBit(dto.OnlyActive)},
    @TargetJDate = {SqlText(dto.TargetJDate)}";

            return await ExecuteQuery(query, "WorkforceAbsencePolicies", nameof(Policy_Get));
        }

        [HttpPost]
        [Route("Policy_Save")]
        public async Task<IActionResult> Policy_Save([FromBody] WorkforceAbsencePolicyDto dto)
        {
            string query = $@"
EXEC dbo.spWeb_WorkforceAbsenceUserPolicy_Save
    @PolicyCode = {SqlInt(dto.PolicyCode)},
    @CentralRef = {SqlInt(dto.CentralRef)},
    @EmploymentType = {SqlText(dto.EmploymentType)},
    @WorkStartTime = {SqlText(dto.WorkStartTime)},
    @WorkEndTime = {SqlText(dto.WorkEndTime)},
    @BreakMinute = {SqlInt(dto.BreakMinute, 60)},
    @DailyWorkMinute = {SqlInt(dto.DailyWorkMinute, 480)},
    @AnnualDailyLimit = {SqlDecimal(dto.AnnualDailyLimit, 26)},
    @MonthlyHourlyLimitMinute = {SqlInt(dto.MonthlyHourlyLimitMinute, 510)},
    @PartTimeRatio = {SqlDecimal(dto.PartTimeRatio, 1)},
    @RequestSubmitStartTime = {SqlText(dto.RequestSubmitStartTime)},
    @RequestSubmitEndTime = {SqlText(dto.RequestSubmitEndTime)},
    @MinimumDailyAdvanceWorkDay = {SqlInt(dto.MinimumDailyAdvanceWorkDay, 2)},
    @MaximumHourlyMinutePerRequest = {SqlInt(dto.MaximumHourlyMinutePerRequest, 480)},
    @ConcurrentLeaveWarningCount = {SqlInt(dto.ConcurrentLeaveWarningCount, 2)},
    @SickAttachmentRequiredAfterDay = {SqlDecimal(dto.SickAttachmentRequiredAfterDay, 1)},
    @AllowDaily = {SqlBit(dto.AllowDaily, true)},
    @AllowHourly = {SqlBit(dto.AllowHourly, true)},
    @AllowSick = {SqlBit(dto.AllowSick, true)},
    @AllowEmergency = {SqlBit(dto.AllowEmergency, true)},
    @AllowHolidayRequest = {SqlBit(dto.AllowHolidayRequest)},
    @EffectiveFromJDate = {SqlText(dto.EffectiveFromJDate)},
    @EffectiveToJDate = {SqlText(dto.EffectiveToJDate)},
    @Explain = {SqlText(dto.Explain)},
    @IsActive = {SqlBit(dto.IsActive, true)}";

            return await ExecuteQuery(query, "WorkforceAbsencePolicies", nameof(Policy_Save));
        }

        [HttpPost]
        [Route("Request_Get")]
        public async Task<IActionResult> Request_Get([FromBody] WorkforceAbsenceSearchDto dto)
        {
            string query = $@"
EXEC dbo.spWeb_WorkforceAbsenceRequest_Get
    @StartJDate = {SqlText(dto.StartJDate)},
    @EndJDate = {SqlText(dto.EndJDate)},
    @CentralRef = {SqlInt(dto.CentralRef)},
    @WorkflowStatus = {SqlInt(dto.WorkflowStatus, -1)},
    @AbsenceTypeKey = {SqlText(dto.AbsenceTypeKey)}";

            return await ExecuteQuery(query, "WorkforceAbsenceRequests", nameof(Request_Get));
        }

        [HttpGet]
        [Route("Request_GetById")]
        public async Task<IActionResult> Request_GetById(string AbsenceRequestCode)
        {
            string query = $@"
EXEC dbo.spWeb_WorkforceAbsenceRequest_GetById
    @AbsenceRequestCode = {SqlInt(AbsenceRequestCode)}";

            return await ExecuteQuery(query, "WorkforceAbsenceRequests", nameof(Request_GetById));
        }

        [HttpPost]
        [Route("Request_Save")]
        public async Task<IActionResult> Request_Save([FromBody] WorkforceAbsenceRequestDto dto)
        {
            string query = $@"
EXEC dbo.spWeb_WorkforceAbsenceRequest_Save
    @AbsenceRequestCode = {SqlInt(dto.AbsenceRequestCode)},
    @CentralRef = {SqlInt(dto.CentralRef)},
    @AbsenceTypeKey = {SqlText(dto.AbsenceTypeKey)},
    @RequestMode = {SqlText(dto.RequestMode)},
    @RequestJDate = {SqlText(dto.RequestJDate)},
    @StartJDate = {SqlText(dto.StartJDate)},
    @EndJDate = {SqlText(dto.EndJDate)},
    @StartTime = {SqlText(dto.StartTime)},
    @EndTime = {SqlText(dto.EndTime)},
    @TotalCalendarDay = {SqlDecimal(dto.TotalCalendarDay)},
    @TotalWorkDay = {SqlDecimal(dto.TotalWorkDay)},
    @TotalOffDay = {SqlDecimal(dto.TotalOffDay)},
    @TotalMinute = {SqlInt(dto.TotalMinute)},
    @Description = {SqlText(dto.Description)},
    @CurrentUserRef = {SqlInt(dto.CurrentUserRef)},
    @IsManager = {SqlBit(dto.IsManager)},
    @ManagerOverrideReason = {SqlText(dto.ManagerOverrideReason)},
    @HasAttachment = {SqlBit(dto.HasAttachment)}";

            return await ExecuteQuery(query, "WorkforceAbsenceRequests", nameof(Request_Save));
        }

        [HttpPost]
        [Route("Request_Workflow")]
        public async Task<IActionResult> Request_Workflow([FromBody] WorkforceAbsenceRequestDto dto)
        {
            string query = $@"
EXEC dbo.spWeb_WorkforceAbsenceRequest_Workflow
    @AbsenceRequestCode = {SqlInt(dto.AbsenceRequestCode)},
    @NewStatus = {SqlInt(dto.WorkflowStatus)},
    @ManagerRef = {SqlInt(dto.ManagerRef)},
    @ManagerExplain = {SqlText(dto.ManagerExplain)},
    @HasAttachment = {SqlNullableBit(dto.HasAttachment)}";

            return await ExecuteQuery(query, "WorkforceAbsenceRequests", nameof(Request_Workflow));
        }

        [HttpPost]
        [Route("Request_Delete")]
        public async Task<IActionResult> Request_Delete([FromBody] WorkforceAbsenceRequestDto dto)
        {
            string query = $@"
EXEC dbo.spWeb_WorkforceAbsenceRequest_Delete
    @AbsenceRequestCode = {SqlInt(dto.AbsenceRequestCode)},
    @CurrentUserRef = {SqlInt(dto.CurrentUserRef)},
    @IsManager = {SqlBit(dto.IsManager)}";

            return await ExecuteQuery(query, "WorkforceAbsenceRequests", nameof(Request_Delete));
        }

        [HttpPost]
        [Route("Request_Status")]
        public async Task<IActionResult> Request_Status([FromBody] WorkforceAbsenceSearchDto dto)
        {
            string query = $@"
EXEC dbo.spWeb_WorkforceAbsenceRequest_Status
    @CentralRef = {SqlInt(dto.CentralRef)},
    @TargetJDate = {SqlText(dto.TargetJDate)}";

            return await ExecuteQuery(query, "WorkforceAbsenceStatus", nameof(Request_Status));
        }

        [HttpPost]
        [Route("Dashboard_Get")]
        public async Task<IActionResult> Dashboard_Get([FromBody] WorkforceAbsenceSearchDto dto)
        {
            string query = $@"
EXEC dbo.spWeb_WorkforceAbsence_Dashboard_Get
    @TargetJDate = {SqlText(dto.TargetJDate)},
    @CentralRef = {SqlInt(dto.CentralRef)}";

            return await ExecuteQuery(query, "WorkforceAbsenceDashboard", nameof(Dashboard_Get));
        }

        private async Task<IActionResult> ExecuteQuery(string query, string rootName, string functionName)
        {
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, rootName, "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", functionName);
                return StatusCode(500, "Internal server error.");
            }
        }

        private static string SqlText(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "NULL";

            return "N'" + value.Trim().Replace("'", "''") + "'";
        }

        private static string SqlInt(string? value, int defaultValue = 0)
        {
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed)
                ? parsed.ToString(CultureInfo.InvariantCulture)
                : defaultValue.ToString(CultureInfo.InvariantCulture);
        }

        private static string SqlDecimal(string? value, decimal defaultValue = 0)
        {
            return decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal parsed)
                ? parsed.ToString(CultureInfo.InvariantCulture)
                : defaultValue.ToString(CultureInfo.InvariantCulture);
        }

        private static string SqlBit(string? value, bool defaultValue = false)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue ? "1" : "0";

            string normalized = value.Trim().ToLowerInvariant();

            if (normalized is "1" or "true") return "1";
            if (normalized is "0" or "false") return "0";

            return defaultValue ? "1" : "0";
        }

        private static string SqlNullableBit(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "NULL";

            string normalized = value.Trim().ToLowerInvariant();

            if (normalized is "1" or "true") return "1";
            if (normalized is "0" or "false") return "0";

            return "NULL";
        }
    }
}
