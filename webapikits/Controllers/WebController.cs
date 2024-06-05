using System.Data;
using Microsoft.AspNetCore.Mvc;
using webapikits.Model;


namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebController : ControllerBase
    {



        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        public WebController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }


        [HttpGet]
        [Route("GetTodeyFromServer")]
        public string GetTodeyFromServer()
        {

            string query = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);


        }

        [HttpGet]
        [Route("ExistUser")]
        public string ExistUser(string UName, string UPass)
        {

            string query = $"Exec spapp_IsXUser  '{UName}','{UPass}'";


            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }





        [HttpGet]
        [Route("ChangeXUserPassword")]
        public string ChangeXUserPassword(string UName, string UPass, string NewPass)
        {

            string query = $"Exec spApp_ChangeXUserPassword  '{UName}','{UPass}','{NewPass}'";


            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpGet]
        [Route("GetWebLog")]
        public string GetWebLog()
        {

            string query = $"select top 50 * from WebLog order by 1 desc";


            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }

        [HttpGet]
        [Route("InsertwebLog")]
        public string InsertwebLog(string ClassName, string TagName, string LogValue)
        {

            string query = $"exec sp_WebLogInsert @ClassName='{ClassName}',@TagName='{TagName}',@LogValue='{LogValue}'";


            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }


    }
}
