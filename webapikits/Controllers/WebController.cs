using System.Data;
using Microsoft.AspNetCore.Mvc;
using webapikits.Model;


namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebController : ControllerBase
    {



        //public readonly IConfiguration _configuration;
        //DataBaseClass db;
        //DataTable DataTable = new DataTable();
        //string Query = "";
        //Response response = new();
        //JsonClass jsonClass = new JsonClass();
        //Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        //public WebController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    db = new DataBaseClass(_configuration);

        //}



        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public WebController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<SupportNewController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }



        [HttpGet]
        [Route("GetTodeyFromServer")]
        public async Task<IActionResult> GetTodeyFromServer()
        {

            string query = "select dbo.fnDate_Today() TodeyFromServer ";

             

             
            try
            {
                DataTable dataTable = await db.Web_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetTodeyFromServer));
                return StatusCode(500, "Internal server error.");
            }


        }

        [HttpGet]
        [Route("ExistUser")]
        public async Task<IActionResult> ExistUser(string UName, string UPass)
        {

            string query = $"Exec spapp_IsXUser  '{UName}','{UPass}'";


             

             
            try
            {
                DataTable dataTable = await db.Web_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "XUsers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ExistUser));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpGet]
        [Route("ChangeXUserPassword")]
        public async Task<IActionResult> ChangeXUserPassword(string UName, string UPass, string NewPass)
        {

            string query = $"Exec spApp_ChangeXUserPassword  '{UName}','{UPass}','{NewPass}'";


             

             
            try
            {
                DataTable dataTable = await db.Web_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "XUsers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ChangeXUserPassword));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetWebLog")]
        public async Task<IActionResult> GetWebLog()
        {

            string query = $"select top 50 * from WebLog order by 1 desc";


             

             
            try
            {
                DataTable dataTable = await db.Web_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetWebLog));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("InsertwebLog")]
        public async Task<IActionResult> InsertwebLog(string ClassName, string TagName, string LogValue)
        {

            string query = $"exec sp_WebLogInsert @ClassName='{ClassName}',@TagName='{TagName}',@LogValue='{LogValue}'";


             

             
            try
            {
                DataTable dataTable = await db.Web_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WebLogs", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(InsertwebLog));
                return StatusCode(500, "Internal server error.");
            }

        }


    }
}
