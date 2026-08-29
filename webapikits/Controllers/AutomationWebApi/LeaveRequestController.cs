using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;

namespace webapikits.Controllers.KowsarBaseWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {

        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<LeaveRequestController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();

        public LeaveRequestController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<LeaveRequestController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("LeaveRequestUserPolicy_Save")]
        public async Task<IActionResult> LeaveRequestUserPolicy_Save([FromBody] LeaveRequestUserPolicyDto policyDto)
        {
            string query = $@"
                    Exec [dbo].[spWeb_LeaveRequestUserPolicy_Save]
                        @PolicyCode = {SqlNumber(policyDto.PolicyCode, "0")},
                        @CentralRef = {SqlNumber(policyDto.CentralRef, "0")},

                        @EmploymentType = {SqlText(policyDto.EmploymentType)},

                        @WorkStartTime = {SqlText(policyDto.WorkStartTime)},
                        @WorkEndTime = {SqlText(policyDto.WorkEndTime)},

                        @BreakMinute = {SqlNumber(policyDto.BreakMinute, "60")},
                        @DailyWorkMinute = {SqlNumber(policyDto.DailyWorkMinute, "480")},

                        @AnnualLeaveLimitDay = {SqlNumber(policyDto.AnnualLeaveLimitDay, "26")},
                        @MonthlyHourlyLeaveLimitMinute = {SqlNumber(policyDto.MonthlyHourlyLeaveLimitMinute, "510")},
                        @PartTimeRatio = {SqlNumber(policyDto.PartTimeRatio, "1")},

                        @RequestSubmitStartTime = {SqlText(policyDto.RequestSubmitStartTime)},
                        @RequestSubmitEndTime = {SqlText(policyDto.RequestSubmitEndTime)},

                        @AllowDailyLeave = {SqlBitText(policyDto.AllowDailyLeave, "1")},
                        @AllowHourlyLeave = {SqlBitText(policyDto.AllowHourlyLeave, "1")},
                        @AllowSickLeave = {SqlBitText(policyDto.AllowSickLeave, "1")},

                        @IsActive = {SqlBitText(policyDto.IsActive, "1")},

                        @EffectiveFromJDate = {SqlText(policyDto.EffectiveFromJDate)},
                        @EffectiveToJDate = {SqlText(policyDto.EffectiveToJDate)},

                        @Explain = {SqlText(policyDto.Explain)}
                    ";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequestUserPolicies", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LeaveRequestUserPolicy_Save));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("LeaveRequestUserPolicy_Get")]
        public async Task<IActionResult> LeaveRequestUserPolicy_Get([FromBody] LeaveRequestUserPolicyDto policyDto)
        {
            string query = $@"
Exec [dbo].[spWeb_LeaveRequestUserPolicy_Get]
    @CentralRef = {SqlNumber(policyDto.CentralRef, "0")},
    @OnlyActive = {SqlNullableBit(policyDto.OnlyActive)}
";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequestUserPolicies", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LeaveRequestUserPolicy_Get));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("LeaveRequest_Insert")]
        public async Task<IActionResult> LeaveRequest_Insert([FromBody] LeaveRequestDto leaveRequestDto)
        {


            string query = $"Exec [dbo].[spWeb_LeaveRequest_Insert] {leaveRequestDto.UserRef},N'{leaveRequestDto.LeaveRequestType}',N'{leaveRequestDto.LeaveStartDate}',N'{leaveRequestDto.LeaveEndDate}',{leaveRequestDto.TotalDay}," +
                $"{leaveRequestDto.WorkDay},{leaveRequestDto.OffDay},N'{leaveRequestDto.LeaveStartTime}',N'{leaveRequestDto.LeaveEndTime}',N'{leaveRequestDto.LeaveRequestExplain}' ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LeaveRequest_Insert));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpPost]
        [Route("LeaveRequest_Update")]
        public async Task<IActionResult> LeaveRequest_Update([FromBody] LeaveRequestDto leaveRequestDto)
        {


            string query = $"Exec [dbo].[spWeb_LeaveRequest_Update] {leaveRequestDto.LeaveRequestCode},{leaveRequestDto.UserRef},N'{leaveRequestDto.LeaveRequestType}',N'{leaveRequestDto.LeaveStartDate}'," +
                $"{leaveRequestDto.TotalDay},{leaveRequestDto.WorkDay},{leaveRequestDto.OffDay},N'{leaveRequestDto.LeaveEndDate}',N'{leaveRequestDto.LeaveStartTime}',N'{leaveRequestDto.LeaveEndTime}'," +
                $"N'{leaveRequestDto.LeaveRequestExplain}',N'{leaveRequestDto.IgnoreLeaveBalance}',{leaveRequestDto.LimitDay},{leaveRequestDto.LimitMinute} ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LeaveRequest_Update));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpPost]
        [Route("LeaveRequest_WorkFlow")]
        public async Task<IActionResult> LeaveRequest_WorkFlow([FromBody] LeaveRequestDto leaveRequestDto)
        {


            string query = $"Exec [dbo].[spWeb_LeaveRequest_WorkFlow]  {leaveRequestDto.LeaveRequestCode},{leaveRequestDto.ManagerRef},{leaveRequestDto.WorkFlowStatus},'{leaveRequestDto.WorkFlowExplain}'";




            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LeaveRequest_WorkFlow));
                return StatusCode(500, "Internal server error.");
            }

        }






        [HttpPost]
        [Route("GetLeaveRequest")]
        public async Task<IActionResult> GetLeaveRequest([FromBody] LeaveRequestDto leaveRequestDto)
        {


            string query = $"Exec [dbo].[spWeb_GetLeaveRequest]  '{leaveRequestDto.StartDate}','{leaveRequestDto.EndDate}',{leaveRequestDto.UserRef},{leaveRequestDto.ManagerRef},{leaveRequestDto.WorkFlowStatus}";




            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLeaveRequest));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetLeaveRequestById")]
        public async Task<IActionResult> GetLeaveRequestById(string LeaveRequestCode)
        {

            string query = $" Exec [dbo].[spWeb_GetLeaveRequest_ById] {LeaveRequestCode}";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLeaveRequestById));
                return StatusCode(500, "Internal server error.");
            }


        }

        [HttpGet]
        [Route("GetLeaveRequestStatus")]
        public async Task<IActionResult> GetLeaveRequestStatus(string CentralCode)
        {

            string query = $" spWeb_GetLeaveRequestStatus {CentralCode}";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLeaveRequestStatus));
                return StatusCode(500, "Internal server error.");
            }


        }


         





        [HttpGet]
        [Route("GetLeaveRequestPerson")]
        public async Task<IActionResult> GetLeaveRequestPerson(string TargetDate)
        {

            string query = $" Exec [dbo].[spWeb_GetLeaveRequest_Person_Bydate] '{TargetDate}'";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLeaveRequestPerson));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpPost]
        [Route("DeleteLeaveRequest")]
        public async Task<IActionResult> DeleteLeaveRequest([FromBody] LeaveRequestDto leaveRequestDto)
        {


            string query = $"Delete From LeaveRequest WHERE LeaveRequestCode = {leaveRequestDto.LeaveRequestCode}";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteLeaveRequest));
                return StatusCode(500, "Internal server error.");
            }

        }





        private static string SqlText(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "NULL";

            return "N'" + value.Trim().Replace("'", "''") + "'";
        }

        private static string SqlNumber(string? value, string defaultValue = "0")
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return value.Trim();
        }

        private static string SqlBitText(string? value, string defaultValue = "0")
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            value = value.Trim().ToLower();

            if (value == "1" || value == "true")
                return "1";

            if (value == "0" || value == "false")
                return "0";

            return defaultValue;
        }

        private static string SqlNullableBit(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "NULL";

            value = value.Trim().ToLower();

            if (value == "1" || value == "true")
                return "1";

            if (value == "0" || value == "false")
                return "0";

            return "NULL";
        }




    }
}
