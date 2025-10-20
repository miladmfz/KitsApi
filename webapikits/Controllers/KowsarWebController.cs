using FastReport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using webapikits.Model;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KowsarWebController : ControllerBase
    {
        //public readonly IConfiguration _configuration;
        //DataBaseClass db;
        //DataTable DataTable = new DataTable();
        //string Query = "";
        //Response response = new();
        //JsonClass jsonClass = new JsonClass();
        //Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        //public KowsarWebController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    db = new DataBaseClass(_configuration);

        //}




        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public KowsarWebController(
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
        [Route("GetApplicationForMenu")]
        public async Task<IActionResult> GetApplicationForMenu()
        {

            string query = $"select KeyValue,Description,DataValue,KeyId from dbsetup where KeyValue in ('AppBroker_ActivationCode','AppOcr_ActivationCode','AppOrder_ActivationCode') and DataValue <> '0'";


            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "applications", "");

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "applications", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetApplicationForMenu));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetObjectTypeFromDbSetup")]
        public async Task<IActionResult> GetObjectTypeFromDbSetup(string ObjectType)
        {

            string query = "select * from dbo.fnObjectType('" + ObjectType + "') ";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResult_Str(dataTable, "ObjectTypes", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "ObjectTypes", ""); ;
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetObjectTypeFromDbSetup));
                return StatusCode(500, "Internal server error.");
            }

        }






        [HttpGet]
        [Route("GetLastGoodData")]
        public async Task<IActionResult> GetLastGoodData()
        {

            string query = $"  declare @ss int  select  @ss=max(GoodCode) from good exec spWeb_GetGoodById @ss,0";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLastGoodData));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetGoodBase")]
        public async Task<IActionResult> GetGoodBase(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},0";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodBase));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("GetGoodExplain")]
        public async Task<IActionResult> GetGoodExplain(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},1";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodExplain));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetGoodComplete")]
        public async Task<IActionResult> GetGoodComplete(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},2";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodComplete));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("GetGoodProperty")]
        public async Task<IActionResult> GetGoodProperty(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},3";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodProperty));
                return StatusCode(500, "Internal server error.");
            }

        }






        [HttpGet]
        [Route("GetGoodRelations")]
        public async Task<IActionResult> GetGoodRelations(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},4";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodRelations));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetGoodImages")]
        public async Task<IActionResult> GetGoodImages(string GoodCode)
        {

            string query = $" select  KsrImageCode,ClassName,ObjectRef,IsDefaultImage,FileName ,IMG='' from KsrImage Where ClassName='TGood' And objectref={GoodCode}";

            //DataTable dataTable = db.Web_ImageExecQuery( query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Image_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodImages));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetGoodGroups")]
        public async Task<IActionResult> GetGoodGroups(string GoodCode)
        {

            string query = $"select GoodGroupCode,GroupCode, Name, GoodRef from GoodGroup join Goodsgrp  on GoodGroupRef = GroupCode  where Goodref = {GoodCode}  ";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodGroups));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetGoodStacks")]
        public async Task<IActionResult> GetGoodStacks(string GoodCode)
        {

            string query = $"select GoodStackCode,GoodRef,StackRef,Amount,ReservedAmount,Name,ActiveStack  from vwGoodStack where goodref= {GoodCode}  ";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodStacks));
                return StatusCode(500, "Internal server error.");
            }

        }








        [HttpPost]
        [Route("GoodCrudService")]
        public async Task<IActionResult> GoodCrudService([FromBody] JsonModelDto jsonModelDto)
        {

            string query = $"Exec spGood_AddNew '{jsonModelDto.JsonData}' ";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodCrudService));
                return StatusCode(500, "Internal server error.");
            }

        }






        [HttpGet]
        [Route("GetGoodList")]
        public async Task<IActionResult> GetGoodList()
        {

            string query = $"Select top 100 * from vwgood order by 1 desc";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodList));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetStacks")]
        public async Task<IActionResult> GetStacks()
        {

            string query = $"Select StackCode,L1,L2,L3,L4,L5,Name from Stacks";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Stacks", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Stacks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetStacks));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetGoodsGrp")]
        public async Task<IActionResult> GetGoodsGrp()
        {

            string query = $"select GroupCode,L1,L2,L3,L4,L5,Name from GoodsGrp";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "GoodsGrps", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodsGrps", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodsGrp));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpPost]
        [Route("GetProperty")]
        public async Task<IActionResult> GetProperty([FromBody] PropertyDto propertyDto)
        {

            string query = $"select dbo.NodeValue(PropertySchema, 'DisplayName') DisplayName, PropertySchemaCode,PropertySchema,ClassName,ObjectType,PropertyName,PropertySequence,PropertyType,PropertyValueMap From PropertySchema p where  p.ObjectType ='{propertyDto.ObjectType}' order by PropertySequence";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Propertys", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Propertys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetProperty));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpPost]
        [Route("GetPropertyChoiess")]
        public async Task<IActionResult> GetPropertyChoiess([FromBody] PropertyDto propertyDto)
        {

            string query = $"Select x.value('text()[1]', 'nvarchar(100)') choice, ds.* " +
                $"From(Select cast(PropertySchema as xml) xschema, PropertySchemaCode, ClassName,ObjectType,PropertyName,PropertySequence,PropertyType,PropertyValueMap ," +
                $" dbo.NodeValue(PropertySchema, 'DisplayName') dispname  From PropertySchema where PropertyType = 'Choice' And ClassName='{propertyDto.ClassName}') ds cross apply ds.xschema.nodes('/Fields/CHOICES/ *') AS R(x)";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "GetPropertyChoiess", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GetPropertyChoiess", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetPropertyChoiess));
                return StatusCode(500, "Internal server error.");
            }

        }



        /// <returns></returns>

        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> UploadImage([FromBody] ksrImageModeldto data)
        {


                // Decode the base64 string to bytes
                byte[] decodedImage = Convert.FromBase64String(data.image);

                // Save the image bytes to a file

                string filePath = _configuration.GetConnectionString("web_imagePath") + $"{data.ObjectCode}.jpg"; // Provide the path where you want to save the image

                System.IO.File.WriteAllBytes(filePath, decodedImage);


                string query = $"Exec spImageImport  '{data.ClassName}',{data.ObjectCode},'{filePath}' ;select @@IDENTITY KsrImageCode";


                // DataTable dataTable = db.Image_ExecQuery( query);

            try
            {
                DataTable dataTable = await db.Image_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UploadImage));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetImageFromKsr")]
        public async Task<string> GetImageFromKsr(string Pixel,string KsrImageCode)
        {

            string query = $"SELECT IMG FROM KsrImage WHERE KsrImageCode = {KsrImageCode}";

            DataTable dataTable =await db.Image_ExecQuery(HttpContext, query);
            return jsonClass.ConvertAndScaleImageToBase64(Convert.ToInt32(Pixel), dataTable);


        }

        [HttpGet]
        [Route("DeleteGoodGroupCode")]
        public async Task<IActionResult> DeleteGoodGroupCode(string Where)
        {

            string query = $" delete from GoodGroup Where GoodGroupCode = {Where}  ";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
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
        [Route("DeleteKsrImageCode")]
        public async Task<IActionResult> DeleteKsrImageCode(string Where)
        {


            string query = $" delete from KsrImage Where KsrImageCode = {Where}  ";

            //DataTable dataTable = db.Web_ImageExecQuery( query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Image_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteKsrImageCode));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetBarcodeList")]
        public async Task<IActionResult> GetBarcodeList(string Where)
        {

            string query = $"Select BarCodeId,GoodRef,BarCode From Barcode where goodref={Where}";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Barcodes", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Barcodes", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetBarcodeList));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("GetSimilarGood")]
        public async Task<IActionResult> GetSimilarGood(string Where)
        {
           

            string query = $"Select top 5 GoodCode,GoodType,GoodName,Type,UsedGood,MinSellPrice,MaxSellPrice,BarCodePrintState,SellPriceType,SellPrice1,SellPrice2,SellPrice3,SellPrice4,SellPrice5,SellPrice6 From Good where GoodName like '%{Where}%'";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetSimilarGood));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpPost]
        [Route("IsbnToBarcode")]
        public async Task<IActionResult> IsbnToBarcode([FromBody] IsbnToBarcodeDto isbnToBarcodeDto)
        {


            string query = $" spGood_IsbnToBarcode  '{isbnToBarcodeDto.Isbn}' , {isbnToBarcodeDto.GoodCode} ";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(IsbnToBarcode));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpGet]
        [Route("GetGridSchema")]
        public async Task<IActionResult> GetGridSchema(string Where)
        {


            string query = $"Select * From [dbo].[fnGetGridSchema]('{Where}')  where Visible = 1";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "GridSchemas", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GridSchemas", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGridSchema));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpPost]
        [Route("GetWebFactor")]
        public async Task<IActionResult> GetWebFactor([FromBody] FactorwebDto factorwebDto)
        {


            string query = $"spWeb_Get_Factor '{factorwebDto.ClassName}',{factorwebDto.ObjectRef},'{factorwebDto.StartDateTarget}','{factorwebDto.EndDateTarget}','{factorwebDto.SearchTarget}'";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Factors", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetWebFactor));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpPost]
        [Route("GetWebFactorRows")]
        public async Task<IActionResult> GetWebFactorRows([FromBody] FactorwebDto factorwebDto)
        {


            string query = $"spWeb_Get_Factor_Rows '{factorwebDto.ClassName}',{factorwebDto.ObjectRef}";


            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Factors", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetWebFactorRows));
                return StatusCode(500, "Internal server error.");
            }


        }






        [HttpPost]
        [Route("GetFactors")]
        public async Task<IActionResult> GetFactors([FromBody] SearchTargetDto searchTargetDto)
        {

            string query = $" Exec spWeb_GetFactors";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Factors", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetFactors));
                return StatusCode(500, "Internal server error.");
            }


        }





        [HttpPost]
        [Route("GetGoods")]
        public async Task<IActionResult> GetGoods([FromBody] SearchTargetDto searchTargetDto)
        {

            string query = $" Exec spWeb_GetGood '{searchTargetDto.SearchTarget}'";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoods));
                return StatusCode(500, "Internal server error.");
            }


        }


        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="factorwebDto"></param>
        /// <returns></returns>






        [HttpPost]
        [Route("GetKowsarCustomer")]
        public async Task<IActionResult> GetKowsarCustomer([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Exec [dbo].[spWeb_GetCustomer] '{searchTargetDto.SearchTarget}'";
            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResult_Str(dataTable, "Customers", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetKowsarCustomer));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpPost]
        [Route("GetGoodListSupport")]
        public async Task<IActionResult> GetGoodListSupport([FromBody] SearchTargetDto searchTargetDto)
        {



            string query = $"spWeb_GetGoodListSupport '{SanitizeInput(searchTargetDto.SearchTarget)}'";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Goods", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodListSupport));
                return StatusCode(500, "Internal server error.");
            }


        }





        [HttpPost]
        [Route("GetFactor")]
        public async Task<IActionResult> GetFactor([FromBody] FactorwebDto factorwebDto)
        {

            string query = $" spWeb_GetFactor '{factorwebDto.StartDateTarget}','{factorwebDto.EndDateTarget}','{factorwebDto.SearchTarget}','{factorwebDto.BrokerRef}','{factorwebDto.isShopFactor}'";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Factors", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetFactor));
                return StatusCode(500, "Internal server error.");
            }


        }





        [HttpPost]
        [Route("EditFactorProperty")]
        public async Task<IActionResult> EditFactorProperty([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"spWeb_EditFactorProperty '{factorwebDto.starttime}','{factorwebDto.Endtime}','{factorwebDto.worktime}','{factorwebDto.Barbary}',{factorwebDto.ObjectRef} ";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Factors", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(EditFactorProperty));
                return StatusCode(500, "Internal server error.");
            }


        }







        [HttpGet]
        [Route("DeleteWebFactorRows")]
        public async Task<IActionResult> DeleteWebFactorRows(string FactorRowCode)
        {

            string query = $" delete from  FactorRows where FactorRowCode= {FactorRowCode}";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Factors", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteWebFactorRows));
                return StatusCode(500, "Internal server error.");
            }



        }

        [HttpGet]
        [Route("DeleteWebFactor")]
        public async Task<IActionResult> DeleteWebFactor(string FactorCode)
        {

            string query = $" delete from  Factor where FactorCode= {FactorCode}";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Factors", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteWebFactor));
                return StatusCode(500, "Internal server error.");
            }



        }

        [HttpGet]
        [Route("DeleteWebPreFactorRows")]
        public async Task<IActionResult> DeleteWebPreFactorRows(string PreFactorRowCode)
        {

            string query = $" delete from  PreFactorRows where PreFactorRowCode= {PreFactorRowCode}";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Factors", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteWebPreFactorRows));
                return StatusCode(500, "Internal server error.");
            }


        }

        [HttpGet]
        [Route("DeleteWebPreFactor")]
        public async Task<IActionResult> DeleteWebPreFactor(string PreFactorCode)
        {

            string query = $" delete from  PreFactor where PreFactorCode= {PreFactorCode}";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Factors", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteWebPreFactor));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("GetTodeyFromServer")]
        public async Task<IActionResult> GetTodeyFromServer()
        {

            string query = "select dbo.fnDate_Today() TodeyFromServer ";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetTodeyFromServer));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpPost]
        [Route("GetCustomerById")]
        public async Task<IActionResult> GetCustomerById([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Exec [dbo].[spWeb_GetCustomerById] {searchTargetDto.ObjectRef}";


            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Customers", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCustomerById));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpPost]
        [Route("GetCentralByCode")]
        public async Task<IActionResult> GetCentralByCode([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Exec [dbo].[spWeb_GetCentralByCode] {searchTargetDto.ObjectRef}";


            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Centrals", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Centrals", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentralByCode));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("GetCentralUser")]
        public async Task<IActionResult> GetCentralUser()
        {

            string query = $"select CentralCode,CentralName from vwCentralUser ";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentralUser));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpPost]
        [Route("GetLetterList")]
        public async Task<IActionResult> GetLetterList([FromBody] SearchTargetLetterDto searchTargetLetterDto)
        {
        
        
            string Where = "";
        
        
        
        if (!string.IsNullOrEmpty(searchTargetLetterDto.CentralRef))
        {
            if (!string.IsNullOrEmpty(Where))
            {
                Where += $" And (CreatorCentralRef={searchTargetLetterDto.CentralRef} or OwnerCentralRef={searchTargetLetterDto.CentralRef} or RowExecutorCentralRef={searchTargetLetterDto.CentralRef})";
            }
            else
            {
                Where = $"(CreatorCentralRef={searchTargetLetterDto.CentralRef} or OwnerCentralRef={searchTargetLetterDto.CentralRef} or RowExecutorCentralRef={searchTargetLetterDto.CentralRef})";
            }
        }
        
        
        if (!string.IsNullOrEmpty(searchTargetLetterDto.PersonInfoCode))
        {
            if (!string.IsNullOrEmpty(Where))
            {
                Where += $" And (OwnerPersonInfoRef={searchTargetLetterDto.PersonInfoCode})";
            }
            else
            {
                Where = $"(OwnerPersonInfoRef={searchTargetLetterDto.PersonInfoCode})";
            }
        }
        
        
        if (!string.IsNullOrEmpty(searchTargetLetterDto.StartTime))
        {
            if (!string.IsNullOrEmpty(Where))
            {
                Where += $" And (LetterDate between ''{searchTargetLetterDto.StartTime}'' And ''{searchTargetLetterDto.EndTime}'') ";
            }
            else
            {
                Where = $" ( LetterDate between ''{searchTargetLetterDto.StartTime}'' And ''{searchTargetLetterDto.EndTime}'') ";
            }
        }
        
        
        string query = $"Exec spWeb_AutLetterList '{Where}',{searchTargetLetterDto.OwnCentralRef},'{searchTargetLetterDto.SearchTarget}'";
        
        
        
        //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
        //return jsonClass.JsonResultWithout_Str(dataTable);

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLetterList));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpGet]
        [Route("DeleteAttachFile")]
        public async Task<IActionResult>  DeleteAttachFile(string AttachedFileCode, string ClassName, string ObjectRef)
        {

            string dbname = "";
            string query11 = "";
            if (ClassName == "AutLetter")
            {
                query11 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join AutLetter aut on PeriodId=PeriodRef Where LetterCode= {ObjectRef}  Select @db dbname";

            }
            else if (ClassName == "Factor")
            {

                query11 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join Factor f on PeriodId=PeriodRef Where FactorCode= {ObjectRef}  Select @db dbname";

            }
            else
            {
                query11 = $"Declare @dbname nvarchar(200)=db_name()+'Ocr' select  @dbname dbname";


            }



            DataTable dataTable =await db.Kowsar_ExecQuery(HttpContext, query11);

            dbname = dataTable.Rows[0]["dbname"] + "";



            string query1 = $"Delete From {dbname}..AttachedFiles where ClassName = '{ClassName}' And AttachedFileCode = {AttachedFileCode} ";



            //DataTable dataTable1 = db.Kowsar_ExecQuery(HttpContext, query1);
            //return jsonClass.JsonResultWithout_Str(dataTable1);


            try
            {
                DataTable dataTable1 = await db.Kowsar_ExecQuery(HttpContext, query1);
                string json = jsonClass.JsonResultWithout_Str(dataTable1);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteAttachFile));
                return StatusCode(500, "Internal server error.");
            }



        }









        [HttpPost]
            [Route("LetterInsert")]
            public async Task<IActionResult> LetterInsert([FromBody] LetterInsert letterInsert)
            {


                string CreatorCentral = _configuration.GetConnectionString("Support_CreatorCentral");


            string query = $"exec dbo.spAutLetter_Insert @LetterDate='{letterInsert.LetterDate}', @InOutFlag={letterInsert.InOutFlag},@Title ='{letterInsert.title}', " +
                $"@Description='{SanitizeInput(letterInsert.Description)}',@State ='{letterInsert.LetterState}',@Priority ='{letterInsert.LetterPriority}', @ReceiveType =N'دستی', @CreatorCentral ={letterInsert.CreatorCentral}, @OwnerCentral ={letterInsert.OwnerCentral} ";



            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LetterInsert));
                return StatusCode(500, "Internal server error.");
            }



        }











        [HttpPost]
            [Route("AutLetterRowInsert")]
            public async Task<IActionResult> AutLetterRowInsert([FromBody] AutLetterRowInsert autLetterRowInsert)
            {


                string query = $"spAutLetterRow_Insert @LetterRef = {autLetterRowInsert.LetterRef}, @LetterDate = '{autLetterRowInsert.LetterDate}'" +
                $", @Description = '{SanitizeInput(autLetterRowInsert.Description)}', @State = '{autLetterRowInsert.LetterState}', @Priority = '{autLetterRowInsert.LetterPriority}'" +
                $", @CreatorCentral = {autLetterRowInsert.CreatorCentral}, @ExecuterCentral = {autLetterRowInsert.ExecuterCentral}";



            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(AutLetterRowInsert));
                return StatusCode(500, "Internal server error.");
            }



        }



        [HttpPost]
            [Route("WebFactorInsert")]
            public async Task<IActionResult> WebFactorInsert([FromBody] FactorwebDto factorwebDto)
            {



                string UserId = _configuration.GetConnectionString("Support_UserId");

            string query = $"spWeb_Factor_Insert  @ClassName ='{factorwebDto.ClassName}',@StackRef ={factorwebDto.StackRef},@UserId ={UserId},@Date ='{factorwebDto.FactorDate}',@Customer ={factorwebDto.CustomerCode},@Explain ='{factorwebDto.Explain}',@BrokerRef  = {factorwebDto.BrokerRef},@IsShopFactor  = {factorwebDto.isShopFactor}";
            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Factors", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WebFactorInsert));
                return StatusCode(500, "Internal server error.");
            }

        }






        [HttpPost]
            [Route("WebFactorInsertRow")]
            public async Task<IActionResult> WebFactorInsertRow([FromBody] FactorRow factorRow)
            {


                string UserId = _configuration.GetConnectionString("Support_UserId");

            string query = $"spWeb_Factor_InsertRow  @ClassName ='{factorRow.ClassName}', @FactorCode={factorRow.FactorRef}, @GoodRef ={factorRow.GoodRef},@Amount ={factorRow.Amount},@Price ={factorRow.Price},@UserId ={UserId},@MustHasAmount ={factorRow.MustHasAmount}, @MergeFlag ={factorRow.MergeFlag} ";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);


            //return jsonClass.JsonResult_Str(dataTable, "Factors", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WebFactorInsertRow));
                return StatusCode(500, "Internal server error.");
            }

        }







        [HttpPost]
        [Route("GetFactorByCustomerCode")]
        public async Task<IActionResult> GetFactorByCustomerCode([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Exec [dbo].[spWeb_GetFactorByCustomerCode] '{searchTargetDto.ClassName}',{searchTargetDto.ObjectRef}";


            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Factors", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetFactorByCustomerCode));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpPost]
        [Route("LeaveRequest_Insert")]
        public async Task<IActionResult> LeaveRequest_Insert([FromBody] LeaveRequestDto leaveRequestDto)
        {


            string query = $"Exec [dbo].[spWeb_LeaveRequest_Insert] {leaveRequestDto.UserRef},'{leaveRequestDto.LeaveRequestType}','{leaveRequestDto.LeaveStartDate}',{leaveRequestDto.TotalDay},{leaveRequestDto.WorkDay},{leaveRequestDto.OffDay},'{leaveRequestDto.LeaveEndDate}','{leaveRequestDto.LeaveStartTime}','{leaveRequestDto.LeaveEndTime}','{leaveRequestDto.LeaveRequestExplain}' ";


            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LeaveRequest_Insert));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpPost]
        [Route("LeaveRequest_Update")]
        public async Task<IActionResult> LeaveRequest_Update([FromBody] LeaveRequestDto leaveRequestDto)
        {


            string query = $"Exec [dbo].[spWeb_LeaveRequest_Update] {leaveRequestDto.LeaveRequestCode},{leaveRequestDto.UserRef},'{leaveRequestDto.LeaveRequestType}','{leaveRequestDto.LeaveStartDate}',{leaveRequestDto.TotalDay},{leaveRequestDto.WorkDay},{leaveRequestDto.OffDay},'{leaveRequestDto.LeaveEndDate}','{leaveRequestDto.LeaveStartTime}','{leaveRequestDto.LeaveEndTime}','{leaveRequestDto.LeaveRequestExplain}' ";


            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LeaveRequest_Update));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpPost]
        [Route("LeaveRequest_WorkFlow")]
        public async Task<IActionResult> LeaveRequest_WorkFlow([FromBody] LeaveRequestDto leaveRequestDto)
        {


            string query = $"Exec [dbo].[spWeb_LeaveRequest_WorkFlow]  {leaveRequestDto.LeaveRequestCode},{leaveRequestDto.ManagerRef},{leaveRequestDto.WorkFlowStatus},'{leaveRequestDto.WorkFlowExplain}'";


            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LeaveRequest_WorkFlow));
                return StatusCode(500, "Internal server error.");
            }

        }






        [HttpPost]
        [Route("GetLeaveRequest")]
        public async Task<IActionResult> GetLeaveRequest([FromBody] LeaveRequestDto leaveRequestDto)
        {


            string query = $"Exec [dbo].[spWeb_GetLeaveRequest]  '{leaveRequestDto.StartDate}','{leaveRequestDto.EndDate}',{leaveRequestDto.UserRef},{leaveRequestDto.ManagerRef},{leaveRequestDto.WorkFlowStatus}";


            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLeaveRequest));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetLeaveRequestById")]
        public async Task<IActionResult> GetLeaveRequestById(string LeaveRequestCode)
        {

            string query = $" Exec [dbo].[spWeb_GetLeaveRequest_ById] {LeaveRequestCode}";

            //DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLeaveRequestById));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpPost]
        [Route("DeleteLeaveRequest")]
        public async Task<IActionResult> DeleteLeaveRequest([FromBody] LeaveRequestDto leaveRequestDto)
        {


            string query = $"Delete From LeaveRequest WHERE LeaveRequestCode = {leaveRequestDto.LeaveRequestCode}";

  
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteLeaveRequest));
                return StatusCode(500, "Internal server error.");
            }

        }









        private string SanitizeInput(string input)
        {
            if (input == null)
                return string.Empty;

            // Prevent SQL Injection by replacing dangerous characters
            input = input.Replace("'", "''");  // Escape single quotes for SQL
            input = input.Replace(";", "");    // Remove semicolons
            input = input.Replace("--", "");   // Remove SQL comments
            input = input.Replace("/*", "");   // Remove SQL block comments
            input = input.Replace("*/", "");   // Remove SQL block comments

            // Prevent XSS by replacing HTML-sensitive characters with their HTML-encoded equivalents
            input = input.Replace("<", "&lt;"); // < becomes &lt;
            input = input.Replace(">", "&gt;"); // > becomes &gt;
            input = input.Replace("&", "&amp;"); // & becomes &amp;
            input = input.Replace("\"", "&quot;"); // " becomes &quot;
            input = input.Replace("'", "&#x27;"); // ' becomes &#x27;
            input = input.Replace("/", "&#x2F;"); // / becomes &#x2F;
            input = input.Replace("\\", "&#x5C;"); // \ becomes &#x5C;

            // Remove leading/trailing whitespace
            input = input.Trim();

            return input;
        }












    }
}







