using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KharidWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class GoodController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<GoodController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public GoodController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<GoodController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
}








        [HttpGet]
        [Route("GetLastGoodData")]
        public async Task<IActionResult> GetLastGoodData()
        {

            string query = $"  declare @ss int  select  @ss=max(GoodCode) from good exec spWeb_GetGoodById @ss,0";



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


        [HttpPost]
        [Route("GetStacks")]
        public async Task<IActionResult> GetStacks([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" spWeb_GetStacks   N'%{searchTargetDto.SearchTarget}%'";


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

        [HttpPost]
        [Route("GetGoodsGrp")]
        public async Task<IActionResult> GetGoodsGrp([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" spWeb_GetGoodsGrp   N'%{searchTargetDto.SearchTarget}%'";


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
        [Route("GetCity")]
        public async Task<IActionResult> GetCity([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" spWeb_GetCity   N'%{searchTargetDto.SearchTarget}%'";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Citys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCity));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpPost]
        [Route("GetUnits")]
        public async Task<IActionResult> GetUnits([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" spWeb_GetUnits   N'%{searchTargetDto.SearchTarget}%'";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Units", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetUnits));
                return StatusCode(500, "Internal server error.");
            }

        }







        [HttpPost]
        [Route("GetProperty")]
        public async Task<IActionResult> GetProperty([FromBody] PropertyDto propertyDto)
        {

            string query = $"select dbo.NodeValue(PropertySchema, 'DisplayName') DisplayName, PropertySchemaCode,PropertySchema,ClassName,ObjectType,PropertyName,PropertySequence" +
                $",PropertyType,PropertyValueMap From PropertySchema p where  p.ObjectType ='{propertyDto.ObjectType}' order by PropertySequence";



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
        [Route("UploadImageForGood")]
        public async Task<IActionResult> UploadImageForGood([FromBody] ksrImageModeldto data)
        {


            byte[] decodedImage = Convert.FromBase64String(data.image);


            string filePath = _configuration.GetValue<string>("AppSettings:web_imagePath") + $"{data.ObjectCode}.jpg";

            System.IO.File.WriteAllBytes(filePath, decodedImage);


            string query = $"Exec spImageImport  '{data.ClassName}',{data.ObjectCode},'{filePath}' ;select @@IDENTITY KsrImageCode";



            try
            {
                DataTable dataTable = await db.Image_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UploadImageForGood));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetImageFromKsr")]
        public async Task<string> GetImageFromKsr(string Pixel, string KsrImageCode)
        {

            string query = $"SELECT IMG FROM KsrImage WHERE KsrImageCode = {KsrImageCode}";

            DataTable dataTable = await db.Image_ExecQuery(HttpContext, query);
            return jsonClass.ConvertAndScaleImageToBase64(Convert.ToInt32(Pixel), dataTable);


        }

        [HttpGet]
        [Route("DeleteGoodGroupCode")]
        public async Task<IActionResult> DeleteGoodGroupCode(string Where)
        {

            string query = $" delete from GoodGroup Where GoodGroupCode = {Where}  ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodGroups", "");
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

            try
            {
                DataTable dataTable = await db.Image_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KsrImages", "");
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


            string query = $"Select top 5 GoodCode,GoodType,GoodName,Type,UsedGood,MinSellPrice,MaxSellPrice,BarCodePrintState,SellPriceType," +
                $"SellPrice1,SellPrice2,SellPrice3,SellPrice4,SellPrice5,SellPrice6 From Good where GoodName like '%{Where}%'";



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




    }
}
