using FastReport;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using webapikits.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Drawing;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KowsarWebController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        public KowsarWebController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }


        [HttpGet]
        [Route("GetApplicationForMenu")]
        public string GetApplicationForMenu()
        {

            string query = $"select KeyValue,Description,DataValue,KeyId from dbsetup where KeyValue in ('AppBroker_ActivationCode','AppOcr_ActivationCode','AppOrder_ActivationCode') and DataValue <> '0'";


            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "applications", "");


        }


        [HttpGet]
        [Route("GetObjectTypeFromDbSetup")]
        public string GetObjectTypeFromDbSetup(string ObjectType)
        {

            string query = "select * from dbo.fnObjectType('" + ObjectType + "') ";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "ObjectTypes", "");

        }
        





        [HttpGet]
        [Route("GetLastGoodData")]
        public string GetLastGoodData()
        {

            string query = $"  declare @ss int  select  @ss=max(GoodCode) from good exec spWeb_GetGoodById @ss,0";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetGoodBase")]
        public string GetGoodBase(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},0";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }

        [HttpGet]
        [Route("GetGoodExplain")]
        public string GetGoodExplain(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},1";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetGoodComplete")]
        public string GetGoodComplete(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},2";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }




        [HttpGet]
        [Route("GetGoodProperty")]
        public string GetGoodProperty(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},3";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }






        [HttpGet]
        [Route("GetGoodRelations")]
        public string GetGoodRelations(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},4";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }



        [HttpGet]
        [Route("GetGoodImages")]
        public string GetGoodImages(string GoodCode)
        {

            string query = $" select  KsrImageCode,ClassName,ObjectRef,IsDefaultImage,FileName ,IMG='' from KsrImage Where ClassName='TGood' And objectref={GoodCode}";

            DataTable dataTable = db.Web_ImageExecQuery( query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetGoodGroups")]
        public string GetGoodGroups(string GoodCode)
        {

            string query = $"select GoodGroupCode,GroupCode, Name, GoodRef from GoodGroup join Goodsgrp  on GoodGroupRef = GroupCode  where Goodref = {GoodCode}  ";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }



        [HttpGet]
        [Route("GetGoodStacks")]
        public string GetGoodStacks(string GoodCode)
        {

            string query = $"select GoodStackCode,GoodRef,StackRef,Amount,ReservedAmount,Name,ActiveStack  from vwGoodStack where goodref= {GoodCode}  ";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");
        }





        


        [HttpPost]
        [Route("GoodCrudService")]
        public string GoodCrudService([FromBody] JsonModelDto jsonModelDto)
        {

            string query = $"Exec spGood_AddNew '{jsonModelDto.JsonData}' ";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");
        }






        [HttpGet]
        [Route("GetGoodList")]
        public string GetGoodList()
        {

            string query = $"Select top 100 * from vwgood order by 1 desc";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");
        }



        [HttpGet]
        [Route("GetStacks")]
        public string GetStacks()
        {

            string query = $"Select StackCode,L1,L2,L3,L4,L5,Name from Stacks";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Stacks", "");
        }


        [HttpGet]
        [Route("GetGoodsGrp")]
        public string GetGoodsGrp()
        {

            string query = $"select GroupCode,L1,L2,L3,L4,L5,Name from GoodsGrp";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "GoodsGrps", "");
        }

        



        [HttpPost]
        [Route("GetProperty")]
        public string GetProperty([FromBody] PropertyDto propertyDto)
        {

            string query = $"select dbo.NodeValue(PropertySchema, 'DisplayName') DisplayName, PropertySchemaCode,PropertySchema,ClassName,ObjectType,PropertyName,PropertySequence,PropertyType,PropertyValueMap From PropertySchema p where  p.ObjectType ='{propertyDto.ObjectType}' order by PropertySequence";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Propertys", "");
        }





        [HttpPost]
        [Route("GetPropertyChoiess")]
        public string GetPropertyChoiess([FromBody] PropertyDto propertyDto)
        {

            string query = $"Select x.value('text()[1]', 'nvarchar(100)') choice, ds.* " +
                $"From(Select cast(PropertySchema as xml) xschema, PropertySchemaCode, ClassName,ObjectType,PropertyName,PropertySequence,PropertyType,PropertyValueMap ," +
                $" dbo.NodeValue(PropertySchema, 'DisplayName') dispname  From PropertySchema where PropertyType = 'Choice' And ClassName='{propertyDto.ClassName}') ds cross apply ds.xschema.nodes('/Fields/CHOICES/ *') AS R(x)";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "GetPropertyChoiess", "");
        }



        /// <returns></returns>

        [HttpPost]
        [Route("UploadImage")]
        public string UploadImage([FromBody] ksrImageModeldto data)
        {


            try
            {


                // Decode the base64 string to bytes
                byte[] decodedImage = Convert.FromBase64String(data.image);

                // Save the image bytes to a file

                string filePath = _configuration.GetConnectionString("web_imagePath") + $"{data.ObjectCode}.jpg"; // Provide the path where you want to save the image

                System.IO.File.WriteAllBytes(filePath, decodedImage);


                string query = $"Exec spImageImport  '{data.ClassName}',{data.ObjectCode},'{filePath}' ;select @@IDENTITY KsrImageCode";


                DataTable dataTable = db.Web_ImageExecQuery( query);

                return "\"Ok\"";
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";

            }
        }


        [HttpGet]
        [Route("GetImageFromKsr")]
        public string GetImageFromKsr(string Pixel,string KsrImageCode)
        {

            string query = $"SELECT IMG FROM KsrImage WHERE KsrImageCode = {KsrImageCode}";

            DataTable dataTable = db.Web_ImageExecQuery( query);
            return jsonClass.ConvertAndScaleImageToBase64(Convert.ToInt32(Pixel), dataTable);

        }

        [HttpGet]
        [Route("DeleteGoodGroupCode")]
        public string DeleteGoodGroupCode(string Where)
        {

            string query = $" delete from GoodGroup Where GoodGroupCode = {Where}  ";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }


        [HttpGet]
        [Route("DeleteKsrImageCode")]
        public string DeleteKsrImageCode(string Where)
        {


            string query = $" delete from KsrImage Where KsrImageCode = {Where}  ";

            DataTable dataTable = db.Web_ImageExecQuery( query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }


        [HttpGet]
        [Route("GetBarcodeList")]
        public string GetBarcodeList(string Where)
        {

            string query = $"Select BarCodeId,GoodRef,BarCode From Barcode where goodref={Where}";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Barcodes", "");

        }



        [HttpGet]
        [Route("GetSimilarGood")]
        public string GetSimilarGood(string Where)
        {
           

            string query = $"Select top 5 GoodCode,GoodType,GoodName,Type,UsedGood,MinSellPrice,MaxSellPrice,BarCodePrintState,SellPriceType,SellPrice1,SellPrice2,SellPrice3,SellPrice4,SellPrice5,SellPrice6 From Good where GoodName like '%{Where}%'";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }



        [HttpPost]
        [Route("IsbnToBarcode")]
        public string IsbnToBarcode([FromBody] IsbnToBarcodeDto isbnToBarcodeDto)
        {


            string query = $" spGood_IsbnToBarcode  '{isbnToBarcodeDto.Isbn}' , {isbnToBarcodeDto.GoodCode} ";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetGridSchema")]
        public string GetGridSchema(string Where)
        {


            string query = $"Select * From [dbo].[fnGetGridSchema]('{Where}')  where Visible = 1";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "GridSchemas", "");

        }


        [HttpPost]
        [Route("GetGoods")]
        public string GetGoods([FromBody] SearchTargetDto searchTargetDto)
        {

            string query = $" Exec spWeb_GetGood '{searchTargetDto.SearchTarget}'";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="factorwebDto"></param>
        /// <returns></returns>






        [HttpPost]
        [Route("GetKowsarCustomer")]
        public string GetKowsarCustomer([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Exec [dbo].[spWeb_GetKowsarCustomer] '{searchTargetDto.SearchTarget}'";
            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Customers", "");
        }




        [HttpPost]
        [Route("GetGoodListSupport")]
        public string GetGoodListSupport([FromBody] SearchTargetDto searchTargetDto)
        {



            string query = $"spWeb_GetGoodListSupport '{SanitizeInput(searchTargetDto.SearchTarget)}'";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");


        }





        [HttpPost]
        [Route("GetFactor")]
        public string GetFactor([FromBody] FactorwebDto factorwebDto)
        {

            string query = $" spWeb_GetFactor '{factorwebDto.StartDateTarget}','{factorwebDto.EndDateTarget}','{factorwebDto.SearchTarget}','{factorwebDto.BrokerRef}','{factorwebDto.isShopFactor}'";



            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");



        }







        [HttpPost]
        [Route("EditFactorProperty")]
        public string EditFactorProperty([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"spWeb_EditFactorProperty '{factorwebDto.starttime}','{factorwebDto.Endtime}','{factorwebDto.worktime}','{factorwebDto.Barbary}',{factorwebDto.ObjectRef} ";



            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");



        }



        [HttpPost]
        [Route("WebFactorInsert")]
        public string WebFactorInsert([FromBody] FactorwebDto factorwebDto)
        {


            string UserId = _configuration.GetConnectionString("Support_UserId");

            string query = $"spWeb_Factor_Insert  @ClassName ='{factorwebDto.ClassName}',@StackRef ={factorwebDto.StackRef},@UserId ={UserId},@Date ='{factorwebDto.FactorDate}',@Customer ={factorwebDto.CustomerCode},@Explain ='{factorwebDto.Explain}',@BrokerRef  = {factorwebDto.BrokerRef},@IsShopFactor  = {factorwebDto.isShopFactor}";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");


        }



        [HttpPost]
        [Route("WebFactorInsertRow")]
        public string WebFactorInsertRow([FromBody] FactorRow factorRow)
        {

            string UserId = _configuration.GetConnectionString("Support_UserId");

            string query = $"spWeb_Factor_InsertRow  @ClassName ='{factorRow.ClassName}', @FactorCode={factorRow.FactorRef}, @GoodRef ={factorRow.GoodRef},@Amount ={factorRow.Amount},@Price ={factorRow.Price},@UserId ={UserId},@MustHasAmount ={factorRow.MustHasAmount}, @MergeFlag ={factorRow.MergeFlag} ";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");
        }



        [HttpGet]
        [Route("DeleteWebFactorRows")]
        public string DeleteWebFactorRows(string FactorRowCode)
        {

            string query = $" delete from  FactorRows where FactorRowCode= {FactorRowCode}";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");


        }

        [HttpGet]
        [Route("DeleteWebFactor")]
        public string DeleteWebFactor(string FactorCode)
        {

            string query = $" delete from  Factor where FactorCode= {FactorCode}";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");


        }



        [HttpGet]
        [Route("GetTodeyFromServer")]
        public string GetTodeyFromServer()
        {

            string query = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);


        }

        [HttpPost]
        [Route("GetFactors")]
        public string GetFactors([FromBody] SearchTargetDto searchTargetDto)
        {

            string query = $" Exec spWeb_GetFactor";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

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







