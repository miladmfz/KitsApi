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
        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        public SupportAppController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }


        [HttpGet]
        [Route("GetTodeyFromServer")]
        public string GetTodeyFromServer()
        {

            string query = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable = db.SupportApp_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);


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
        public string GetAppActivation()
        {

            string query = $"select * from AppActivation";

            DataTable dataTable = db.SupportApp_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("GetAppActivationByCode")]
        public string GetAppActivationByCode(string ActivationCode)
        {

            string query = $"select * from AppActivation Where ActivationCode = '{ActivationCode}'";

            DataTable dataTable = db.SupportApp_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);





        }






        


        [HttpPost]
        [Route("CrudAppActivation")]
        public string CrudAppActivation([FromBody] AppActivationDto brokercustomerdto)
        {

            string query = $"exec [spApp_AppActivation_Crud]  '{brokercustomerdto.ActivationCode}','{brokercustomerdto.EnglishCompanyName}', '{brokercustomerdto.PersianCompanyName}', '{brokercustomerdto.ServerURL}', " +
                $" '{brokercustomerdto.SQLiteURL}', {brokercustomerdto.UsedDevice}, {brokercustomerdto.MaxDevice}, '{brokercustomerdto.SecendServerURL}' , '{brokercustomerdto.DbName}', '{brokercustomerdto.DbImageName}', " +
                $"{brokercustomerdto.AppType} , '{brokercustomerdto.ServerIp}', '{brokercustomerdto.ServerPort}', '{brokercustomerdto.ServerPathApi}'";


            DataTable dataTable = db.SupportApp_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }







        [HttpGet]
        [Route("GetActiveApplication")]
        public string GetActiveApplication(string Filter)
        {

            string query = $"Select Server_Name, STRING_AGG([Broker],',') within group (order by case when isnumeric([Broker])=1 then cast([Broker] as decimal) else 0 end, [Broker] ) as BrokerStr From (select Server_Name, Device_Id, [Broker] from app_info where DATEDIFF(m,Updatedate,GETDATE())<{Filter} group by Server_Name, Device_Id, [Broker]) ds group by Server_Name";


            DataTable dataTable = db.SupportApp_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetWebLog")]
        public string GetWebLog()
        {

            string query = $"select top 50 * from WebLog order by 1 desc";


            DataTable dataTable = db.Web_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }






        [HttpPost]
        [Route("GetAppLogReport")]
        public string GetAppLogReport([FromBody] LogReportDto logReportDto)
        {

            string query = $"exec spWeb_GetAppLogReport    @FromDate  = '{logReportDto.FromDate}',    @ToDate  = '{logReportDto.ToDate}',    @ServerName  = '{logReportDto.ServerName}',  @Flag  = {logReportDto.Flag}";


            DataTable dataTable = db.Web_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }




        [HttpPost]
        [Route("WebSiteInsert")]
        public string WebSiteInsert([FromBody] WebSiteActivationDto webSiteActivationDto)
        {

            string query = $"exec spWeb_InsertWebSiteActivation     @CustomerRef = {webSiteActivationDto.CustomerRef} , @CompanyName = '{webSiteActivationDto.CompanyName}' , @WebEmploy = '{webSiteActivationDto.WebEmploy}' , @Phone = '{webSiteActivationDto.Phone}' , @Explain = '{webSiteActivationDto.Explain}' , @Features = '{webSiteActivationDto.Features}' , @WebState = {webSiteActivationDto.WebState} , @Domain1 = '{webSiteActivationDto.Domain1}' , @Domain2 = '{webSiteActivationDto.Domain2}' , @Domain3 = '{webSiteActivationDto.Domain3}' , @Domain4 = '{webSiteActivationDto.Domain4}' , @KCServerVersion = '{webSiteActivationDto.KCServerVersion}' , @SiteType = '{webSiteActivationDto.SiteType}' , @PaymentGateway = '{webSiteActivationDto.PaymentGateway}' , @TorobApi = {webSiteActivationDto.TorobApi} , @EmallsApi = {webSiteActivationDto.EmallsApi} , @BasalamApi = {webSiteActivationDto.BasalamApi} , @SnapApi = {webSiteActivationDto.SnapApi} , @MobileTheme = {webSiteActivationDto.MobileTheme}  ";


            DataTable dataTable = db.Web_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }


        [HttpPost]
        [Route("WebSiteUpdate")]
        public string WebSiteUpdate([FromBody] WebSiteActivationDto webSiteActivationDto)
        {

            string query = $"exec spWeb_UpdateWebSiteActivation    @WebSiteActivationCode = {webSiteActivationDto.WebSiteActivationCode} , @CustomerRef = {webSiteActivationDto.CustomerRef} , @CompanyName = '{webSiteActivationDto.CompanyName}' , @WebEmploy = '{webSiteActivationDto.WebEmploy}' , @Phone = '{webSiteActivationDto.Phone}' , @Explain = '{webSiteActivationDto.Explain}'  , @Features = '{webSiteActivationDto.Features}' , @WebState = {webSiteActivationDto.WebState} , @Domain1 = '{webSiteActivationDto.Domain1}' , @Domain2 = '{webSiteActivationDto.Domain2}' , @Domain3 = '{webSiteActivationDto.Domain3}' , @Domain4 = '{webSiteActivationDto.Domain4}' , @KCServerVersion = '{webSiteActivationDto.KCServerVersion}' , @SiteType = '{webSiteActivationDto.SiteType}' , @PaymentGateway = '{webSiteActivationDto.PaymentGateway}' , @TorobApi = {webSiteActivationDto.TorobApi} , @EmallsApi = {webSiteActivationDto.EmallsApi} , @BasalamApi = {webSiteActivationDto.BasalamApi} , @SnapApi = {webSiteActivationDto.SnapApi} , @MobileTheme = {webSiteActivationDto.MobileTheme}  ";


            DataTable dataTable = db.Web_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }




        [HttpPost]
        [Route("GetWebSiteActivation")]
        public string GetWebSiteActivation([FromBody] WebSiteActivationDto webSiteActivationDto)
        {

            string query = $"exec spWeb_GetWebSiteActivation    @SearchTarget = '{webSiteActivationDto.SearchTarget}' ";


            DataTable dataTable = db.Web_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }




        [HttpGet]
        [Route("GetWebSiteActivationById")]
        public string GetWebSiteActivationById(string WebSiteActivationCode)
        {

            string query = $"exec spWeb_GetWebSiteActivationById    @WebSiteActivationCode = {WebSiteActivationCode} ";


            DataTable dataTable = db.Web_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }
















    }
}
