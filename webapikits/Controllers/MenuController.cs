using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;
using static webapikits.Controllers.OrderController;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {






        public readonly IConfiguration _configuration;

        Dictionary<string, string> jsonDict = new();

        DataBaseClass db;
        JsonClass jsonClass = new();



        public MenuController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new(_configuration);
        }




        [HttpGet]
        [Route("WebOrderMizData")]
        public string WebOrderMizData(string RstMizCode)
        {
            string query = $"exec spApp_OrderMizData {RstMizCode}";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.ConvertDataTableToJson(dataTable);

        }




        [HttpGet]
        [Route("WebOrderInfoInsert")]
        public string WebOrderInfoInsert(string Miz, string Date)
        {

            string query = $"exec spApp_OrderInfoInsert 0,{Miz},'','','',0,'','','{Date}',1,0 ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.ConvertDataTableToJson(dataTable);
        }




        [HttpGet]
        [Route("GetMenuOnlinegroups")]
        public string GetMenuOnlinegroups(
            string GroupName,
            string GroupCode
            )
        {

            string sq = "Exec [dbo].[spApp_GetGoodGroups] @where='" + _configuration.GetConnectionString("Order_WhereMenuOnline") + " ', ";

            if (!string.IsNullOrEmpty(GroupName))
            {
                sq += $" @GroupName = N'{GroupName}' ";
            }

            if (!string.IsNullOrEmpty(GroupCode))
            {
                sq += $" @GroupCode = N'{GroupCode}' ";
            }


            DataTable dataTable = db.Order_ExecQuery(HttpContext, sq);

            return jsonClass.JsonResult_Str(dataTable, "Groups", "");

        }




        [HttpGet]
        [Route("GetOrdergroupList")]
        public string GetOrdergroupList(string GroupCode)
        {

            string sq = "Exec [dbo].[spApp_GetGoodGroups]  @GroupName = N''  ";


            if (!string.IsNullOrEmpty(GroupCode))
            {
                sq += $" , @GroupCode = {GroupCode} ";
            }


            DataTable dataTable = db.Order_ExecQuery(HttpContext, sq);

            return jsonClass.JsonResult_Str(dataTable, "Groups", "");

        }



        [HttpPost]
        [Route("GetOrderGoodList")]
        public string GetOrderGoodList([FromBody] OrderModel orderModel)
        {

            string query = $"Exec spApp_GetGoods2 @RowCount = {orderModel.RowCount}, @Where = N'{orderModel.Where}', @AppBasketInfoRef = {orderModel.AppBasketInfoRef}, @GroupCode = {orderModel.GroupCode}, @AppType = 3, @OrderBy = ' order by PrivateCodeForSort '";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetOrderSum")]
        public string GetOrderSum(string AppBasketInfoRef)
        {

            string query = "Exec spApp_OrderGetSummmary " + AppBasketInfoRef;

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpPost]
        [Route("OrderRowInsert")]
        public string OrderRowInsert([FromBody] OrderModel orderModel)
        {

            string query = $"[dbo].[spApp_OrderRowInsert] {orderModel.GoodRef}," +
                        $" {orderModel.FacAmount}, {orderModel.Price}, {orderModel.bUnitRef}," +
                        $" {orderModel.bRatio}, '{orderModel.Explain}', {orderModel.UserId}," +
                        $" {orderModel.InfoRef}, {orderModel.RowCode}";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }

        [HttpGet]
        [Route("OrderGet")]
        public string OrderGet(string AppBasketInfoRef, string AppType)
        {

            string query = $"Exec [dbo].[spApp_OrderGet] {AppBasketInfoRef} , {AppType} ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("DeleteGoodFromBasket")]
        public string DeleteGoodFromBasket(
            string RowCode,
            string AppBasketInfoRef
            )
        {

            string query = $"Delete From AppBasket Where AppBasketInfoRef = {AppBasketInfoRef} and AppBasketCode = {RowCode}";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");

        }

        

    }
}
