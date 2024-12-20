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
        [Route("GetAppBrokerCustomer")]
        public string GetAppBrokerCustomer()
        {

            string query = $"select * from AppBrokerCustomer";

            DataTable dataTable = db.SupportApp_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("GetAppBrokerCustomerByCode")]
        public string GetAppBrokerCustomerByCode(string ActivationCode)
        {

            string query = $"select * from AppBrokerCustomer Where ActivationCode = '{ActivationCode}'";

            DataTable dataTable = db.SupportApp_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);





        }






        


        [HttpPost]
        [Route("CrudAppBrokerCustomer")]
        public string CrudAppBrokerCustomer([FromBody] BrokerCustomerDto brokercustomerdto)
        {

            string query = $"exec [spApp_CrudAppBrokerCustomer]  '{brokercustomerdto.ActivationCode}','{brokercustomerdto.EnglishCompanyName}', '{brokercustomerdto.PersianCompanyName}', '{brokercustomerdto.ServerURL}', " +
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












    }
}
