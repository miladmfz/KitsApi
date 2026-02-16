using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Sockets;
using webapikits.Model;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportAppController : Controller
    {
        //public readonly IConfiguration _configuration;
        //DataBaseClass db;
        //DataTable DataTable = new DataTable();
        //string Query = "";
        //Response response = new();
        //JsonClass jsonClass = new JsonClass();
        //Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        //public SupportAppController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    db = new DataBaseClass(_configuration);

        //}




        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public SupportAppController(
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
                DataTable dataTable = await db.SupportApp_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetTodeyFromServer));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpPost]
        [Route("CheckPort")]

        public IActionResult CheckPort([FromBody] PortCheckRequest request)
        {
            using (var client = new TcpClient())
            {
                try
                {
                    client.Connect(request.Ip, Convert.ToInt32(request.Port));
                    return Ok(new { Status = "open" });
                }
                catch
                {
                    return Ok(new { Status = "closed" });
                }
            }
        }


        [HttpGet]
        [Route("GetAppActivation")]
        public async Task<IActionResult> GetAppActivation()
        {

            string query = $"select * from AppActivation";

            
             

            try
            {
                DataTable dataTable = await db.SupportApp_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AppActivations", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAppActivation));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetAppActivationByCode")]
        public async Task<IActionResult> GetAppActivationByCode(string ActivationCode)
        {

            string query = $"select * from AppActivation Where ActivationCode = '{ActivationCode}'";

            
             

            try
            {
                DataTable dataTable = await db.SupportApp_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AppActivations", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAppActivationByCode));
                return StatusCode(500, "Internal server error.");
            }


        }









        [HttpPost]
        [Route("CrudAppActivation")]
        public async Task<IActionResult> CrudAppActivation([FromBody] AppActivationDto brokercustomerdto)
        {

            string query = $"exec [spApp_AppActivation_Crud]  '{brokercustomerdto.ActivationCode}','{brokercustomerdto.EnglishCompanyName}', '{brokercustomerdto.PersianCompanyName}', '{brokercustomerdto.ServerURL}', " +
                $" '{brokercustomerdto.SQLiteURL}', {brokercustomerdto.UsedDevice}, {brokercustomerdto.MaxDevice}, '{brokercustomerdto.SecendServerURL}' , '{brokercustomerdto.DbName}', '{brokercustomerdto.DbImageName}', " +
                $"{brokercustomerdto.AppType} , '{brokercustomerdto.ServerIp}', '{brokercustomerdto.ServerPort}', '{brokercustomerdto.ServerPathApi}', {brokercustomerdto.IsActive}";


            

            try
            {
                DataTable dataTable = await db.SupportApp_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AppActivations", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CrudAppActivation));
                return StatusCode(500, "Internal server error.");
            }

        }







        [HttpGet]
        [Route("GetActiveApplication")]
        public async Task<IActionResult> GetActiveApplication(string Filter)
        {

            string query = $"Select Server_Name, STRING_AGG([Broker],',') within group (order by case when isnumeric([Broker])=1 then cast([Broker] as decimal) else 0 end, [Broker] ) as BrokerStr From (select Server_Name, Device_Id, [Broker] from app_info where DATEDIFF(m,Updatedate,GETDATE())<{Filter} group by Server_Name, Device_Id, [Broker]) ds group by Server_Name";


            

             
            try
            {
                DataTable dataTable = await db.SupportApp_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "ActiveApplications", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetActiveApplication));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("GetWebLog")]
        public async Task<IActionResult> GetWebLog()
        {

            string query = $"ShowWebLog";


            

             
            try
            {
                DataTable dataTable = await db.SupportApp_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WebLogs", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetWebLog));
                return StatusCode(500, "Internal server error.");
            }

        }






        [HttpPost]
        [Route("GetAppLogReport")]
        public async Task<IActionResult> GetAppLogReport([FromBody] LogReportDto logReportDto)
        {

            string query = $"exec spWeb_GetAppLogReport    @FromDate  = '{logReportDto.FromDate}',    @ToDate  = '{logReportDto.ToDate}',    @ServerName  = '{logReportDto.ServerName}',  @Flag  = {logReportDto.Flag}";


            
             
            try
            {
                DataTable dataTable = await db.SupportApp_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AppLogs", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAppLogReport));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpPost]
        [Route("WebSiteInsert")]
        public async Task<IActionResult> WebSiteInsert([FromBody] WebSiteActivationDto webSiteActivationDto)
        {

            string query = $"exec spWeb_InsertWebSiteActivation     @CustomerRef = {webSiteActivationDto.CustomerRef} , @CompanyName = '{webSiteActivationDto.CompanyName}' , @WebEmploy = '{webSiteActivationDto.WebEmploy}' , @Phone = '{webSiteActivationDto.Phone}' , @Explain = '{webSiteActivationDto.Explain}' , @Features = '{webSiteActivationDto.Features}' , @WebState = {webSiteActivationDto.WebState} , @Domain1 = '{webSiteActivationDto.Domain1}' , @Domain2 = '{webSiteActivationDto.Domain2}' , @Domain3 = '{webSiteActivationDto.Domain3}' , @Domain4 = '{webSiteActivationDto.Domain4}' , @KCServerVersion = '{webSiteActivationDto.KCServerVersion}' , @SiteType = '{webSiteActivationDto.SiteType}' , @PaymentGateway = '{webSiteActivationDto.PaymentGateway}' , @TorobApi = {webSiteActivationDto.TorobApi} , @EmallsApi = {webSiteActivationDto.EmallsApi} , @BasalamApi = {webSiteActivationDto.BasalamApi} , @SnapApi = {webSiteActivationDto.SnapApi} , @MobileTheme = {webSiteActivationDto.MobileTheme}  ";


            
             

            try
            {
                DataTable dataTable = await db.SupportApp_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WebSites", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WebSiteInsert));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpPost]
        [Route("WebSiteUpdate")]
        public async Task<IActionResult> WebSiteUpdate([FromBody] WebSiteActivationDto webSiteActivationDto)
        {

            string query = $"exec spWeb_UpdateWebSiteActivation    @WebSiteActivationCode = {webSiteActivationDto.WebSiteActivationCode} , @CustomerRef = {webSiteActivationDto.CustomerRef} , @CompanyName = '{webSiteActivationDto.CompanyName}' , @WebEmploy = '{webSiteActivationDto.WebEmploy}' , @Phone = '{webSiteActivationDto.Phone}' , @Explain = '{webSiteActivationDto.Explain}'  , @Features = '{webSiteActivationDto.Features}' , @WebState = {webSiteActivationDto.WebState} , @Domain1 = '{webSiteActivationDto.Domain1}' , @Domain2 = '{webSiteActivationDto.Domain2}' , @Domain3 = '{webSiteActivationDto.Domain3}' , @Domain4 = '{webSiteActivationDto.Domain4}' , @KCServerVersion = '{webSiteActivationDto.KCServerVersion}' , @SiteType = '{webSiteActivationDto.SiteType}' , @PaymentGateway = '{webSiteActivationDto.PaymentGateway}' , @TorobApi = {webSiteActivationDto.TorobApi} , @EmallsApi = {webSiteActivationDto.EmallsApi} , @BasalamApi = {webSiteActivationDto.BasalamApi} , @SnapApi = {webSiteActivationDto.SnapApi} , @MobileTheme = {webSiteActivationDto.MobileTheme}  ";


            
             

            try
            {
                DataTable dataTable = await db.SupportApp_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WebSites", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WebSiteUpdate));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpPost]
        [Route("GetWebSiteActivation")]
        public async Task<IActionResult> GetWebSiteActivation([FromBody] WebSiteActivationDto webSiteActivationDto)
        {

            string query = $"exec spWeb_GetWebSiteActivation    @SearchTarget = '{webSiteActivationDto.SearchTarget}' ";


            
             

            try
            {
                DataTable dataTable = await db.SupportApp_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WebSites", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetWebSiteActivation));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("GetWebSiteActivationById")]
        public async Task<IActionResult> GetWebSiteActivationById(string WebSiteActivationCode)
        {

            string query = $"exec spWeb_GetWebSiteActivationById    @WebSiteActivationCode = {WebSiteActivationCode} ";


            

             
            try
            {
                DataTable dataTable = await db.SupportApp_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WebSites", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetWebSiteActivationById));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("DeleteWebSiteActivation")]
        public async Task<IActionResult> DeleteWebSiteActivation(string WebSiteActivationCode)
        {

            string query = $"Delete From WebSiteActivation Where WebSiteActivationCode={WebSiteActivationCode}";


            try
            {
                DataTable dataTable = await db.SupportApp_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WebSites", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteWebSiteActivation));
                return StatusCode(500, "Internal server error.");
            }

        }



    }
}
