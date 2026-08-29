using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.TanzimatWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class SalaryController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<SalaryController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public SalaryController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<SalaryController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
}





        [HttpPost]
        [Route("GetEmployee")]
        public async Task<IActionResult> GetEmploye([FromBody] EmployeeDto employeDto)
        {

            try
            {

                string query = $"Exec spWeb_GetEmployee '{employeDto.SearchTarget}'";


                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Employees", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GetEmploye));
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpPost]
        [Route("GetMonthSummary")]
        public async Task<IActionResult> GetMonthSummary([FromBody] MonthSummaryDto monthSummaryDto)
        {

            try
            {
                string query = $"Exec spWeb_GetMonthSummary {monthSummaryDto.Sal},{monthSummaryDto.Mah}";


                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "MonthSummarys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GetMonthSummary));
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpPost]
        [Route("GetSalarySummary")]
        public async Task<IActionResult> GetSalarySummary([FromBody] SalarySummaryDto salarySummaryDto)
        {

            try
            {
                string query = $"Exec spWeb_GetSalarySummary '{salarySummaryDto.SearchTarget}',{salarySummaryDto.Sal},{salarySummaryDto.Mah},{salarySummaryDto.EmployeCode}";


                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "SalarySummarys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GetSalarySummary));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("UpdateWorkingEmployee")]
        public async Task<IActionResult> UpdateWorkingEmployee([FromBody] SalarySummaryDto salarySummaryDto)
        {

            try
            {

                string query = $"Exec spweb_UpdateWorkingAndOvertimeForEmployee {salarySummaryDto.SalarySummaryCode},N'{salarySummaryDto.WorkingHours}',{salarySummaryDto.LeaveDays}";

                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "SalarySummarys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(UpdateWorkingEmployee));
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpPost]
        [Route("AddSalaryForAllEmployees")]
        public async Task<IActionResult> AddSalaryForAllEmployees([FromBody] MonthSummaryDto monthSummaryDto)
        {

            try
            {
                string query = $"Exec spweb_AddSalarySummaryForAllEmployees {monthSummaryDto.MonthSummaryCode}";


                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "SalarySummarys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(AddSalaryForAllEmployees));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("InUp_Employee")]
        public async Task<IActionResult> InUp_Employee([FromBody] EmployeeDto employeDto)
        {

            try
            {

                string query = $"Exec spweb_InUp_Employee {employeDto.EmployeeCode},'{employeDto.FirstName}','{employeDto.LastName}','{employeDto.CodeMeli}','{employeDto.JobTitle}' ," +
                    $" {employeDto.Rozkarkard}, {employeDto.NerkhHoghogh}, {employeDto.NerkhSanavat}, {employeDto.NerkhMaskan}, {employeDto.NerkhKharobar}, {employeDto.NerkhEzafekar}," +
                    $" {employeDto.NerkhPadash}, {employeDto.TedadPadash}, {employeDto.NerkhExtra1}, {employeDto.TedadExtra1}, {employeDto.NerkhExtra2}, {employeDto.TedadExtra2}," +
                    $" {employeDto.BimePaye},{employeDto.BimeTakmili},{employeDto.Extra3}, {employeDto.Extra4}, {employeDto.SaatNaharNamaz}, '{employeDto.VaziyatTaahol}', {employeDto.TedadOlad}, {employeDto.HaghOlad}," +
                    $" {employeDto.HaghTaahol}, '{employeDto.Explain}' ";


                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Employees", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(InUp_Employee));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("InUp_MonthSummary")]
        public async Task<IActionResult> InUp_MonthSummary([FromBody] MonthSummaryDto monthSummaryDto)
        {

            try
            {

                string query = $"Exec spweb_InUp_MonthSummary {monthSummaryDto.Sal},{monthSummaryDto.Mah},{monthSummaryDto.TotalDays},{monthSummaryDto.HolidayDays}, '{monthSummaryDto.Explain}'";


                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "MonthSummarys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(InUp_MonthSummary));
                return StatusCode(500, "Internal server error.");
            }
        }





        [HttpGet]
        [Route("GetEmployeeByCode")]
        public async Task<IActionResult> GetEmployeeByCode(string EmployeeCode)
        {

            string query = $"Select * from Employee Where EmployeeCode= {EmployeeCode} ";
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Employees", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetEmployeeByCode));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpGet]
        [Route("GetMonthSummaryByCode")]
        public async Task<IActionResult> GetMonthSummaryByCode(string MonthSummaryCode)
        {

            string query = $"Select * from MonthSummary Where MonthSummaryCode= {MonthSummaryCode} ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "MonthSummarys", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetMonthSummaryByCode));
                return StatusCode(500, "Internal server error.");
            }
        }





























































































































































































































    }
}
