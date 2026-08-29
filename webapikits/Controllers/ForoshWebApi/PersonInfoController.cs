using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonInfoController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<PersonInfoController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public PersonInfoController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<PersonInfoController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("SetPersonInfo_XUserNew")]
        public async Task<IActionResult> SetPersonInfo_XUserNew([FromBody] PersonInfoDto personInfoDto)
        {


            string query = $" spPersonInfo_XUser @PersonRef={personInfoDto.PersonInfoRef}, @XUserName = '{personInfoDto.XUserName}', @XUserPass = '{personInfoDto.XUserPass}'";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PersonInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SetPersonInfo_XUserName));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpPost]
        [Route("SetPersonInfo_XUserName")]
        public async Task<IActionResult> SetPersonInfo_XUserName([FromBody] PersonInfoDto personInfoDto)
        {


            string query = $" spPersonInfo_XUser @PersonRef={personInfoDto.PersonInfoRef}, @XUserName = '{personInfoDto.XUserName}'";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PersonInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SetPersonInfo_XUserName));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpPost]
        [Route("SetPersonInfo_XUserPass")]
        public async Task<IActionResult> SetPersonInfo_XUserPass([FromBody] PersonInfoDto personInfoDto)
        {


            string query = $" spPersonInfo_XUser @PersonRef={personInfoDto.PersonInfoRef}, @XUserPass = '{personInfoDto.XUserPass}'";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PersonInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SetPersonInfo_XUserPass));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("SetPersonInfo_XUserActive")]
        public async Task<IActionResult> SetPersonInfo_XUserActive(string PersonInfoCode, string Active)
        {

            string query = $" spPersonInfo_XUser @PersonRef={PersonInfoCode}, @Active ={Active} ";

            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PersonInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SetPersonInfo_XUserActive));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("SetPersonInfo_XUserAuthSms")]
        public async Task<IActionResult> SetPersonInfo_XUserAuthSms(string PersonInfoCode, string AuthSMS)
        {

            string query = $" spPersonInfo_XUser @PersonRef={PersonInfoCode}, @AuthSMS  = {AuthSMS} ";

            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PersonInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SetPersonInfo_XUserAuthSms));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("SetPersonInfo_XUserAdminUser")]
        public async Task<IActionResult> SetPersonInfo_XUserAdminUser(string PersonInfoCode, string IsAdminUser)
        {

            string query = $" spPersonInfo_XUser @PersonRef={PersonInfoCode}, @IsAdminUser  = {IsAdminUser} ";

            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PersonInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SetPersonInfo_XUserAdminUser));
                return StatusCode(500, "Internal server error.");
            }

        }


        









       [HttpPost]
        [Route("GetPersonInfo")]
        public async Task<IActionResult> GetPersonInfo([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select vp.*,XUserName,Active,AuthSms from  vwPersonInfo  vp Left join XUser xu on PersonInfoRef=PersonInfoCode ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PersonInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetPersonInfo));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpPost]
        [Route("GetPersonInfo_Customer")]
        public async Task<IActionResult> GetPersonInfo_Customer([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"select vp.*,XUserName,Active,AuthSms from  vwPersonInfo  vp left join XUser xu on PersonInfoRef=PersonInfoCode   Where  vp.CustomerRef = {searchTargetDto.ObjectRef} ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PersonInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetPersonInfo_Customer));
                return StatusCode(500, "Internal server error.");
            }

        }







        [HttpGet]
        [Route("GetPersonInfoById")]
        public async Task<IActionResult> GetPersonInfoById(string PersonInfoCode)
        {

            string query = $"Select vp.*,vj.JobPersonTitle,vc.CustName_Small from Vwpersoninfo vp left join vwCustomer vc on vc.CustomerCode = vp.CustomerRef left join vwJobPerson vj on vj.JobPersonCode = vp.JobPersonRef Where PersonInfoCode = {PersonInfoCode}  ";

            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PersonInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetPersonInfoById));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("ResetXUserPassword")]
        public async Task<IActionResult> ResetXUserPassword(string UserName)
        {

            string query = $"Exec spWeb_ResetXUserPassword  N'{UserName}'  ";

            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Users", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ResetXUserPassword));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpPost]
        [Route("PersonInfoCrudService")]
        public async Task<IActionResult> PersonInfoCrudService([FromBody] JsonModelDto jsonModelDto)

        {


            string query = $"Exec spPersonInfo_AddNew '{jsonModelDto.JsonData}' ";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PersonInfos", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PersonInfoCrudService));
                return StatusCode(500, "Internal server error.");
            }
        }




    }
}
