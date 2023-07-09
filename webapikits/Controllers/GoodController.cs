using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using webapikits.Model;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodController : ControllerBase
    {

        public readonly IConfiguration _configuration;
      
        public GoodController(IConfiguration configuration)
        {
            _configuration = configuration;

        }


        DataBaseClass db = new DataBaseClass();
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();



        [HttpGet]
        [Route("GetTodeyFromServer")]
        public string GetTodeyFromServer()
        {

            string query = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");

        }



        [HttpGet]
        [Route("GetAllGood")]
        public String GetGoods()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("KowsardbCon").ToString());
            SqlDataAdapter ad = new SqlDataAdapter("select GoodCode,GoodName,GoodExplain1 from good ", con);
            DataTable dataTable = new();
            DataTable dt = dataTable;
            ad.Fill(dt);

            List<Good> goods = new();
            Response response = new();

            if (dt.Rows.Count > 0){

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Good g = new();
                    g.GoodCode = Convert.ToString(dt.Rows[i]["GoodCode"]);
                    g.GoodName = Convert.ToString(dt.Rows[i]["GoodName"]);
                    g.GoodExplain1 = Convert.ToString(dt.Rows[i]["GoodExplain1"]);
                    goods.Add(g);
                }
            }

            if (goods.Count > 0)
                return JsonConvert.SerializeObject(goods);
            else
            {

                response.StatusCode = "100";
                response.Errormessage = "No Good Data Found";

                return JsonConvert.SerializeObject(response);
            }



        }




    }
}
