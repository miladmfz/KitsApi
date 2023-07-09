using System.Data;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapikits.Model;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {


        public readonly IConfiguration _configuration;
        DataBaseClass db = new DataBaseClass();
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;

        }


        [HttpGet]
        [Route("OrderMizList")]
        public string OrderMizList(string InfoState, string MizType)
        {

            string query = " exec spApp_OrderMizList  "+ InfoState + ",'"+ MizType + "' ";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }





        [HttpGet]
        [Route("OrderReserveList")]
        public string OrderReserveList(string MizRef)
        {

            string query = "exec spApp_OrderReserveList "+ MizRef;

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }
        



        [HttpGet]
        [Route("OrderInfoInsert")]
        public string OrderInfoInsert(
            string Broker,
            string Miz,
            string PersonName,
            string Mobile,
            string InfoExplain,
            string Prepayed,
            string ReserveStartTime,
            string ReserveEndTime,
            string Date,
            string State,
            string InfoCode
            )
        {

            string query = "exec spApp_OrderInfoInsert "+Broker+","+Miz+",'"+PersonName+"','"+Mobile + "','" + InfoExplain + "'," +Prepayed+",'"+ReserveStartTime+"','"+ReserveEndTime+"','"+Date+"',"+State+","+InfoCode;

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }



        [HttpGet]
        [Route("OrderRowInsert")]
        public string OrderRowInsert(
            string GoodRef,
            string FacAmount,
            string Price,
            string bUnitRef,
            string bRatio,
            string Explain,
            string UserId,
            string InfoRef,
            string RowCode
            )
        {

            string query = $"[dbo].[spApp_OrderRowInsert] {GoodRef}, {FacAmount}, {Price}, {bUnitRef}, {bRatio}, '{Explain}', {UserId}, {InfoRef}, {RowCode}";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }






        [HttpGet]
        [Route("GetGoodFromGroup")]
        public string GetGoodFromGroup(string GroupCode)
        {

            string query = "select GoodCode,GoodName,MaxSellPrice,'' ImageName from vwGood where  GoodCode in(Select GoodRef From GoodGroup p Join GoodsGrp s on p.GoodGroupRef = s.GroupCode Where s.GroupCode = "+ GroupCode + " or s.L1 = "+ GroupCode + " or s.L2 = "+ GroupCode + " or s.L3 = "+ GroupCode + " )";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }





        [HttpGet]
        [Route("GetObjectTypeFromDbSetup")]
        public string GetObjectTypeFromDbSetup(string ObjectType)
        {

            string query = "select * from dbo.fnObjectType('"+ ObjectType + "') ";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "ObjectTypes", "");

        }




        [HttpGet]
        [Route("GetTodeyFromServer")]
        public string GetTodeyFromServer()
        {

            string query = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");

        }



        [HttpGet]
        [Route("GetOrderGoodList")]
        public string GetOrderGoodList(
            string GroupCode,
            string RowCount,
            string Where,
            string AppBasketInfoRef
            )
        {

            //string query = "Exec spApp_GetGoods2 @RowCount = $RowCount,@Where = N'$Where',@AppBasketInfoRef=$AppBasketInfoRef, @GroupCode = $GroupCode ,@AppType=3 , @OrderBy = ' order by PrivateCodeForSort ' ";
            string query = $"Exec spApp_GetGoods2 @RowCount = {RowCount}, @Where = N'{Where}', @AppBasketInfoRef = {AppBasketInfoRef}, @GroupCode = {GroupCode}, @AppType = 3, @OrderBy = ' order by PrivateCodeForSort '";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }




        [HttpGet]
        [Route("DeleteGoodFromBasket")]
        public string DeleteGoodFromBasket(
            string RowCode,
            string AppBasketInfoRef
            )
        {

            //string query = "Exec spApp_GetGoods2 @RowCount = $RowCount,@Where = N'$Where',@AppBasketInfoRef=$AppBasketInfoRef, @GroupCode = $GroupCode ,@AppType=3 , @OrderBy = ' order by PrivateCodeForSort ' ";
            string query = $"Delete From AppBasket Where AppBasketInfoRef = {AppBasketInfoRef} and AppBasketCode = {RowCode}";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");

        }




        [HttpGet]
        [Route("GetSellBroker")]
        public string GetSellBroker()
        {

            string query = "select brokerCode,BrokerNameWithoutType from  vwSellBroker";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "SellBrokers", "");

        }




        [HttpGet]
        [Route("GetOrderSum")]
        public string GetOrderSum(string AppBasketInfoRef)
        {

            string query = "Exec spApp_OrderGetSummmary "+AppBasketInfoRef;

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }





        [HttpGet]
        [Route("OrderGet")]
        public string OrderGet(string AppBasketInfoRef,string AppType)
        {

            string query = $"Exec [dbo].[spApp_OrderGet] {AppBasketInfoRef} , {AppType} ";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }





        [HttpGet]
        [Route("OrderToFactor")]
        public string OrderToFactor(string AppBasketInfoRef, string UserId)
        {

            string query = $"Exec [dbo].[spApp_OrderToFactor] {AppBasketInfoRef} , {UserId} ";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }




        [HttpGet]
        [Route("OrderGetFactor")]
        public string OrderGetFactor(string AppBasketInfoRef)
        {

            string query = $"Exec [dbo].[spApp_OrderGetFactor] {AppBasketInfoRef}  ";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }





        [HttpGet]
        [Route("OrderGetFactorRow")]
        public string OrderGetFactorRow(
            string AppBasketInfoRef,
            string GoodGroups,
            string Where
            )
        {

            string query = $"Exec [dbo].[spApp_OrderGetFactorRow] {AppBasketInfoRef}, {GoodGroups}, '{Where}'";


            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }



        [HttpGet]
        [Route("OrderGetAppPrinter")]
        public string OrderGetAppPrinter()
        {

            string query = $"select * from AppPrinter ";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "AppPrinters", "");

        }




        [HttpGet]
        [Route("Order_CanPrint")]
        public string Order_CanPrint(
            string AppBasketInfoRef,
            string CanPrint
            )
        {

            string query = $"spApp_Order_CanPrint  {AppBasketInfoRef} ,{CanPrint}  ";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");

        }






        [HttpGet]
        [Route("OrderEditInfoExplain")]
        public string OrderEditInfoExplain(
            string AppBasketInfoCode,
            string Explain
            )
        {

            string query = $" spApp_OrderInfoUpdateExplain  '{Explain}', {AppBasketInfoCode}   ";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }




        [HttpGet]
        [Route("OrderDeleteAll")]
        public string OrderDeleteAll(string AppBasketInfoRef)
        {

            string query = $"Delete From AppBasket Where  PreFactorCode is null and  AppBasketInfoRef= {AppBasketInfoRef} ";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");

        }




        [HttpGet]
        [Route("OrderInfoReserveDelete")]
        public string OrderInfoReserveDelete(string AppBasketInfoRef)
        {

            string query = $" spApp_OrderInfoReserveDelete  {AppBasketInfoRef} ";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");

        }



















    }
}
