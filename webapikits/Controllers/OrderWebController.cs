
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using webapikits.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderWebController : ControllerBase
    {

        //public readonly IConfiguration _configuration;
        //DataBaseClass db;
        //DataTable DataTable = new DataTable();
        //string Query = "";
        //Response response = new();
        //JsonClass jsonClass = new JsonClass();

        //Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        //public OrderWebController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    db = new DataBaseClass(_configuration);

        //}


        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public OrderWebController(
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



        /// ///////////////////////////////////////////////////////////////






        [HttpGet]
        [Route("OrderMizList")]
        public async Task<IActionResult> OrderMizList(string InfoState, string MizType)
        {

            string query = $" exec spApp_OrderMizList  {InfoState}, N'{MizType}' ";

             
             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderMizList));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("GetAmountItem")]
        public async Task<IActionResult> GetAmountItem(string Date, string State)
        {

            string query = $" spWeb_GetBrokerPanel '{Date}' ,{State} ";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAmountItem));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("GetTodeyFromServer")]
        public async Task<IActionResult> GetTodeyFromServer(string day)
        {

            string query = $"select dbo.fnDate_AddDays(dbo.fnDate_Today(),{day}) TodeyFromServer  ";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetTodeyFromServer));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetOrderPanel")]
        public async Task<IActionResult> GetOrderPanel(string StartDate, string EndDate, string State)
        {

            string query = $" spweb_Getorderpanel '{StartDate}' ,'{EndDate}' ,{State} ";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetOrderPanel));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetCustomerMandeh")]
        public async Task<IActionResult> GetCustomerMandeh()
        {

            string query = $" spWeb_GetCustomerMandeh ";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCustomerMandeh));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("GetCustomerlastGood")]
        public async Task<IActionResult> GetCustomerlastGood(string CustomerCode)
        {

            string query = $" spWeb_GetCustomerlastGood {CustomerCode} ";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCustomerlastGood));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpGet]
        [Route("BasketColumnCard")]
        public async Task<IActionResult> BasketColumnCard(string Where, string AppType)
        {
            string query = "";



            if (Where == "ListVisible")
            {
                query = $" select AppBasketColumnCode,ColumnName,ColumnDesc,ColumnDefinition,ObjectType, " +
                        $" ListVisible,ColumnType,OrderIndex,Condition,AppType from AppBasketColumn " +
                        $" where apptype ={AppType} and ListVisible > 0 And ObjectType='' order by ListVisible  ";
            }
            else if (Where == "DetailVisible")
            {
                query = $" select AppBasketColumnCode,ColumnName,ColumnDesc,ColumnDefinition,ObjectType, " +
                        $" DetailVisible,ColumnType,OrderIndex,Condition,AppType from AppBasketColumn  " +
                        $" where apptype ={AppType} and DetailVisible > 0 And ObjectType='' order by DetailVisible  ";
            }
            else if (Where == "SearchVisible")
            {
                query = $" select AppBasketColumnCode,ColumnName,ColumnDesc,ColumnDefinition,ObjectType, " +
                        $"SearchVisible,ColumnType,OrderIndex,Condition,AppType from AppBasketColumn " +
                        $" where apptype ={AppType} and SearchVisible > 0 And ObjectType='' order by SearchVisible  ";
            }




             

             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BasketColumnCard));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("Web_GetDbsetupObject")]
        public async Task<IActionResult> Web_GetDbsetupObject(string Where)
        {
            string query = "";


             if (Where == "OrderKowsar")
            {
                query = " select KeyId,KeyValue,DataValue,Description,SubSystem from DbSetup where  KeyValue like '%apporder%' or KeyValue like'%rstfactor%' ";
            }
           


             

             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Web_GetDbsetupObject));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("CreateBasketColumn")]
        public async Task<IActionResult> CreateBasketColumn(string AppType)
        {
            string query = "";



             if (AppType == "3") // OrderKowsar
            {
                query = " select '' test";
            }




             

             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CreateBasketColumn));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("GetBasketColumnList")]
        public async Task<IActionResult> GetBasketColumnList(string AppType)
        {


            string query = $" select * from AppBasketColumn Where AppType ={AppType} ";

             

             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetBasketColumnList));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("GetGoodType")]
        public async Task<IActionResult> GetGoodType()
        {

            string query = "Exec [spApp_GetGoodType]";



             
             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodType));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetProperty")]
        public async Task<IActionResult> GetProperty(string Where)
        {


            string query = $" Select  PropertySchema,PropertyValueMap,PropertyName  from PropertySchema Where ClassName = 'TGOOD' And  ObjectType = '{Where}'";



             
             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetProperty));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("InsertSingleColumn")]
        public async Task<IActionResult> InsertSingleColumn(
            string ColumnName,
            string ColumnDesc,
            string ObjectType,
            string DetailVisible,
            string ListVisible,
            string SearchVisible,
            string ColumnType,
            string AppType
)
        {

            string query =

                $" Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)" +
                $" Select '{ColumnName}','{ColumnDesc}','','{ObjectType}','{DetailVisible}','{ListVisible}','-1','{SearchVisible}','{ColumnType}','0','','{AppType}' ";


             
             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(InsertSingleColumn));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpGet]
        [Route("UpdateDbSetup")]
        public async Task<IActionResult> UpdateDbSetup(
            string DataValue,
            string KeyId)

        {

            string query = $" update dbsetup set DataValue = '{DataValue}'  where keyid = {KeyId}";


             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UpdateDbSetup));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpGet]
        [Route("GetAppPrinter")]
        public async Task<IActionResult> GetAppprinter(string AppType)

        {

            string query = $"select * from AppPrinter Where AppType={AppType}";


             
             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAppprinter));
                return StatusCode(500, "Internal server error.");
            }

        }






        [HttpPost]
        [Route("UpdatePrinter")]
        public async Task<IActionResult> UpdatePrinter([FromBody] AppPrinterDto printerDto)
        {


            string query = "";

            if (printerDto.AppPrinterCode == "0")
            {
                query = $" Insert Into AppPrinter ( [PrinterName], [PrinterExplain], [GoodGroups], [WhereClause], [PrintCount], [PrinterActive], [FilePath], [AppType] ) values ('{printerDto.PrinterName}', '{printerDto.PrinterExplain}', '{printerDto.GoodGroups}', '{printerDto.WhereClause}', '{printerDto.PrintCount}', '{printerDto.PrinterActive}', '{printerDto.FilePath}', '{printerDto.AppType}') ";

            }
            else
            {
                query = $" Update AppPrinter set [PrinterName] = '{printerDto.PrinterName}', [PrinterExplain]= '{printerDto.PrinterExplain}', [GoodGroups]= '{printerDto.GoodGroups}', [WhereClause]= '{printerDto.WhereClause}', [PrintCount]= '{printerDto.PrintCount}', [PrinterActive]= '{printerDto.PrinterActive}', [FilePath]= '{printerDto.FilePath}', [AppType] = '{printerDto.AppType}' Where AppPrinterCode = {printerDto.AppPrinterCode}";
            }


             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UpdatePrinter));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpPost]
        [Route("GetOrderGoodList")]
        public async Task<IActionResult> GetOrderGoodList([FromBody] OrderGoodListSearchDto searchDto)
        {
            string searchtarget = searchDto.Where.Replace(" ", "%");

            string query = $"Exec spApp_GetGoods2 @RowCount = {searchDto.RowCount},@Where = N' GoodName like ''%{searchtarget}%''' ,@AppBasketInfoRef=1, @GroupCode = {searchDto.GroupCode} ,@AppType=3 ,@OrderBy =' Order By ActiveStack DESC , GoodCode  Desc'";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetOrderGoodList));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("ChangeGoodActive")]
        public async Task<IActionResult> ChangeGoodActive(string GoodCode, string ActiveFlag)
        {

            string query = $"spWeb_ChangeGoodActive {GoodCode},{ActiveFlag} ";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ChangeGoodActive));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpGet]
        [Route("GetGoodEdit")]
        public async Task<IActionResult> GetGoodEdit(string Where)
        {

            string query = $"Select GoodCode,GoodName, CAST(MaxSellprice AS INT) MaxSellprice,GoodExplain1,GoodExplain2,GoodExplain3,GoodExplain4,GoodExplain5,GoodExplain6 from Good Where GoodCode = {Where} ";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodEdit));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("GetActiveGood")]
        public async Task<IActionResult> GetActiveGood(string GoodCode)
        {

            string query = $"select ActiveStack,GoodRef from GoodStack where goodref = {GoodCode}  order by 1 desc ";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetActiveGood));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpPost]
        [Route("Web_InsertGood")]
        public async Task<IActionResult> Web_InsertGood([FromBody] GoodDto gooddto)
        {

            string query = $"Exec spWeb_InsertGood '{gooddto.GoodName}' , {gooddto.MaxSellPrice},'{gooddto.GoodExplain1}','{gooddto.GoodExplain2}','{gooddto.GoodExplain3}','{gooddto.GoodExplain4}','{gooddto.GoodExplain5}','{gooddto.GoodExplain6}' ";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Web_InsertGood));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpPost]
        [Route("Web_UpdateGoodDetail")]
        public async Task<IActionResult> Web_UpdateGoodDetail([FromBody] GoodDto gooddto)
        {

            string query = $"Exec spWeb_UpdateGoodDetail {gooddto.GoodCode},'{gooddto.GoodName}' , {gooddto.MaxSellPrice},'{gooddto.GoodExplain1}','{gooddto.GoodExplain2}','{gooddto.GoodExplain3}','{gooddto.GoodExplain4}','{gooddto.GoodExplain5}','{gooddto.GoodExplain6}' ";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Web_UpdateGoodDetail));
                return StatusCode(500, "Internal server error.");
            }

        }






        [HttpGet]
        [Route("GetGroupFromGood")]
        public async Task<IActionResult> GetGroupFromGood(string Where)
        {

            string query = $"select GoodGroupCode,GoodGroupRef, Name, GoodRef from GoodGroup join Goodsgrp  on GoodGroupRef = GroupCode where Goodref = {Where}  ";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGroupFromGood));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]


        [Route("GetGoodFromGroup")]
        public async Task<IActionResult> GetGoodFromGroup(string Where)
        {

            string query = $"select GoodGroupCode, GoodName, GoodCode from Good join GoodGroup on goodref = GoodCode  where GoodGroupRef = {Where}  ";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodFromGroup));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("DeleteGoodGroupCode")]
        public async Task<IActionResult> DeleteGoodGroupCode(string Where)
        {

            string query = $" delete from GoodGroup Where GoodGroupCode = {Where}  ";

             
             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteGoodGroupCode));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetWebImagess")]
        public async Task<string> GetWebImagess(string pixelScale, string ClassName, string ObjectRef)
        {
            string query = $"SELECT * FROM KsrImage WHERE Classname = '{ClassName}' AND ObjectRef = {ObjectRef} order by 1 desc";


            DataTable dataTable =await db.Image_ExecQuery(HttpContext, query);


            return jsonClass.ConvertAndScaleImageToBase64(Convert.ToInt32(pixelScale), dataTable);


        }

        [HttpPost]
        [Route("UploadImage")]
        public async Task<string> UploadImage([FromBody] ksrImageModeldto data)
        {


            try
            {


                // Decode the base64 string to bytes
                byte[] decodedImage = Convert.FromBase64String(data.image);

                // Save the image bytes to a file

                string filePath = _configuration.GetConnectionString("web_imagePath") + $"{data.ObjectCode}.jpg"; // Provide the path where you want to save the image

                System.IO.File.WriteAllBytes(filePath, decodedImage);


                string query = $"Exec spImageImport  '{data.ClassName}',{data.ObjectCode},'{filePath}' ;select @@IDENTITY KsrImageCode";


                DataTable dataTable =await db.Image_ExecQuery(HttpContext, query);

                return "\"Ok\"";
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";

            }
        }

    }
}







