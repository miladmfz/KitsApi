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






        //public readonly IConfiguration _configuration;

        //Dictionary<string, string> jsonDict = new();

        //DataBaseClass db;
        //JsonClass jsonClass = new();



        //public MenuController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    db = new(_configuration);
        //}




        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public MenuController(
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
        [Route("WebOrderMizData")]
        public async Task<IActionResult> WebOrderMizData(string RstMizCode)
        {
            string query = $"exec spApp_OrderMizData {RstMizCode}";

            //DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            //return jsonClass.ConvertDataTableToJson(dataTable);
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WebOrderMizData));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("WebOrderInfoInsert")]
        public async Task<IActionResult> WebOrderInfoInsert(string Miz, string Date)
        {

            string query = $"exec spApp_OrderInfoInsert 0,{Miz},'','','',0,'','','{Date}',1,0 ";

            //DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            //return jsonClass.ConvertDataTableToJson(dataTable);
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WebOrderInfoInsert));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("GetMenuOnlinegroups")]
        public async Task<IActionResult> GetMenuOnlinegroups(
            string GroupName,
            string GroupCode
            )
        {

            string query = "Exec [dbo].[spApp_GetGoodGroups] @where='" + _configuration.GetConnectionString("Order_WhereMenuOnline") + " ', ";

            if (!string.IsNullOrEmpty(GroupName))
            {
                query += $" @GroupName = N'{GroupName}' ";
            }

            if (!string.IsNullOrEmpty(GroupCode))
            {
                query += $" @GroupCode = N'{GroupCode}' ";
            }


            //DataTable dataTable = db.Order_ExecQuery(HttpContext, sq);

            //return jsonClass.JsonResult_Str(dataTable, "Groups", "");
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Groups", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetMenuOnlinegroups));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("GetOrdergroupList")]
        public async Task<IActionResult> GetOrdergroupList(string GroupCode)
        {

            string query = "Exec [dbo].[spApp_GetGoodGroups]  @GroupName = N''  ";


            if (!string.IsNullOrEmpty(GroupCode))
            {
                query += $" , @GroupCode = {GroupCode} ";
            }


            //DataTable dataTable = db.Order_ExecQuery(HttpContext, sq);

            //return jsonClass.JsonResult_Str(dataTable, "Groups", "");
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Groups", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetOrdergroupList));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpPost]
        [Route("GetOrderGoodList")]
        public async Task<IActionResult> GetOrderGoodList([FromBody] OrderModel orderModel)
        {

            string query = $"Exec spApp_GetGoods2 @RowCount = {orderModel.RowCount}, @Where = N'{orderModel.Where}', @AppBasketInfoRef = {orderModel.AppBasketInfoRef}, @GroupCode = {orderModel.GroupCode}, @AppType = 3, @OrderBy = ' order by PrivateCodeForSort '";

            //DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetOrderGoodList));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetOrderSum")]
        public async Task<IActionResult> GetOrderSum(string AppBasketInfoRef)
        {

            string query = "Exec spApp_OrderGetSummmary " + AppBasketInfoRef;

            //DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetOrderSum));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpPost]
        [Route("OrderRowInsert")]
        public async Task<IActionResult> OrderRowInsert([FromBody] OrderModel orderModel)
        {

            string query = $"[dbo].[spApp_OrderRowInsert] {orderModel.GoodRef}," +
                        $" {orderModel.FacAmount}, {orderModel.Price}, {orderModel.bUnitRef}," +
                        $" {orderModel.bRatio}, '{orderModel.Explain}', {orderModel.UserId}," +
                        $" {orderModel.InfoRef}, {orderModel.RowCode}";

            //DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderRowInsert));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("OrderGet")]
        public async Task<IActionResult> OrderGet(string AppBasketInfoRef, string AppType)
        {

            string query = $"Exec [dbo].[spApp_OrderGet] {AppBasketInfoRef} , {AppType} ";

            //DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderGet));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("DeleteGoodFromBasket")]
        public async Task<IActionResult> DeleteGoodFromBasket(
            string RowCode,
            string AppBasketInfoRef
            )
        {

            string query = $"Delete From AppBasket Where AppBasketInfoRef = {AppBasketInfoRef} and AppBasketCode = {RowCode}";

            //DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResult_Str(dataTable, "Text", "Done");
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "Done");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteGoodFromBasket));
                return StatusCode(500, "Internal server error.");
            }

        }



    }
}
