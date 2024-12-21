using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : Controller
    {

        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();


        public SearchController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }

        [HttpPost]
        [Route("GetGoodList")]
        public string GetGoodList([FromBody] SearchTargetDto searchTargetDto)
        {

            //string query = "Exec spApp_GetGoods2 @RowCount = $RowCount,@Where = N'$Where',@AppBasketInfoRef=$AppBasketInfoRef, @GroupCode = $GroupCode ,@AppType=3 , @OrderBy = ' order by PrivateCodeForSort ' ";
            string query = $"Exec spApp_GetGoods2 @RowCount = 100, @SearchTarget = N'{searchTargetDto.SearchTarget}', @AppType = 4";

            DataTable dataTable = db.Search_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("BrokerStack")]
        public string BrokerStack(string BrokerRef)
        {
            if (string.IsNullOrEmpty(BrokerRef))
            {
                BrokerRef = "0";
            }

            string query = $"exec spApp_GetBrokerStack {BrokerRef}";
            DataTable dataTable = db.Search_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "BrokerStack");

        }


        [HttpGet]
        [Route("GetColumnList")]
        public string GetColumnList(
            string Type,
            string AppType,
            string IncludeZero
            )
        {

            if (string.IsNullOrEmpty(Type))
            {
                Type = "0";
            }
            if (string.IsNullOrEmpty(AppType))
            {
                AppType = "0";
            }
            if (string.IsNullOrEmpty(IncludeZero))
            {
                IncludeZero = "0";
            }

            string query = $"Exec [spApp_GetColumn]  0  ,'', {Type},{AppType}, {IncludeZero}";


            DataTable dataTable = db.Search_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Columns", "");


        }


    }
}

