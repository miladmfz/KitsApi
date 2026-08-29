using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
using static DbService;

namespace webapikits.Controllers.KowsarBaseWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternalFactorController : ControllerBase
    {

        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<InternalFactorController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();

        public InternalFactorController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<InternalFactorController> logger,
            IConfiguration configuration
            )
        {

            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }





        [HttpPost("GetCustomerById")]
        public async Task<IActionResult> GetCustomerById([FromBody] SearchTargetDto searchTargetDto)
        {

            string query = $"Exec [dbo].[spWeb_GetCustomerById] {searchTargetDto.ObjectRef}";


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
        [Route("EditFactorProperty")]
        public async Task<IActionResult> EditFactorProperty([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"spWeb_EditFactorProperty '{factorwebDto.starttime}','{factorwebDto.Endtime}','{factorwebDto.worktime}','{factorwebDto.Barbary}',{factorwebDto.ObjectRef} ";



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



        [HttpGet]
        [Route("GetWebFactorSupport")]
        public async Task<IActionResult> GetWebFactorSupport(string FactorCode)
        {

            string rowLevelSecurityCondition = await db.GetRowLevelSecurityStringAsync(HttpContext, "TFactor");


            string query = $" select FactorCode, FactorDate, CustName, CustomerCode, Explain, BrokerRef, BrokerName, starttime, Endtime, worktime, Barbary, owner, OwnerName from vwFactor where FactorCode = {FactorCode} {rowLevelSecurityCondition}";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetWebFactorSupport));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("GetWebFactorRowsSupport")]
        public async Task<IActionResult> GetWebFactorRowsSupport(string FactorCode)
        {

            string query = $" select FactorRowCode,GoodRef,GoodName from vwFactorRows where Factorref= {FactorCode}";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetWebFactorRowsSupport));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("DeleteWebFactorRowsSupport")]
        public async Task<IActionResult> DeleteWebFactorRowsSupport(string FactorRowCode)
        {

            string query = $" delete from  FactorRows where FactorRowCode= {FactorRowCode}";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteWebFactorRowsSupport));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("DeleteWebFactorSupport")]
        public async Task<IActionResult> DeleteWebFactorSupport(string FactorCode)
        {

            string query = $" delete from  Factor where FactorCode= {FactorCode}";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteWebFactorSupport));
                return StatusCode(500, "Internal server error.");
            }
        }








        [HttpPost]
        [Route("GetGoodListSupport")]
        public async Task<IActionResult> GetGoodListSupport([FromBody] SearchTargetDto searchTargetDto)
        {



            string query = $"spWeb_GetGoodListSupport '{SanitizeInput(searchTargetDto.SearchTarget)}'";



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
        [Route("WebSupportFactorInsert")]
        public async Task<IActionResult> WebSupportFactorInsert([FromBody] FactorwebDto factorwebDto)
        {

            string UserId = WebUtility.UrlDecode(HttpContext.Request.Headers["UI"].FirstOrDefault()) ?? string.Empty;

            string BrokerRef =  string.IsNullOrWhiteSpace(factorwebDto.BrokerRef?.ToString())    ? "NULL"    : factorwebDto.BrokerRef.ToString();


            string query = $"spWeb_Factor_Insert  @ClassName ='{factorwebDto.ClassName}',@StackRef ={factorwebDto.StackRef},@UserId ={UserId},@Date ='{factorwebDto.FactorDate}',@Customer ={factorwebDto.CustomerCode},@Explain ='{factorwebDto.Explain}',@BrokerRef  = {BrokerRef},@IsShopFactor  = {factorwebDto.IsShopFactor}";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WebSupportFactorInsert));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("WebSupportFactorInsertRow")]
        public async Task<IActionResult> WebSupportFactorInsertRow([FromBody] FactorRow factorRow)
        {

            string query = $"spWeb_Factor_InsertRow  @ClassName ='{factorRow.ClassName}', @FactorCode={factorRow.FactorRef}, @GoodRef ={factorRow.GoodRef},@Amount =1,@Price =0,@UserId =29,@MustHasAmount =0, @MergeFlag =1 ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WebSupportFactorInsertRow));
                return StatusCode(500, "Internal server error.");
            }
        }








        [HttpPost]
        [Route("Support_StartFactorTime")]
        public async Task<IActionResult> StartFactorTime([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"Update PropertyValue Set Nvarchar15 = '{factorwebDto.starttime}'  where ClassName = 'TFactor' And ObjectRef = {factorwebDto.ObjectRef} ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(StartFactorTime));
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("Support_EndFactorTime")]
        public async Task<IActionResult> EndFactorTime([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"Update PropertyValue Set Nvarchar9 = '{factorwebDto.Endtime}', int1 = {factorwebDto.worktime} where ClassName = 'TFactor' And ObjectRef = {factorwebDto.ObjectRef} ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(EndFactorTime));
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpPost]
        [Route("Support_ExplainFactor")]
        public async Task<IActionResult> Support_ExplainFactor([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"Update PropertyValue Set Nvarchar14 = '{SanitizeInput(factorwebDto.Barbary)}' where ClassName = 'TFactor' And ObjectRef = {factorwebDto.ObjectRef} ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Support_ExplainFactor));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpPost]
        [Route("GetSupportFactors")]
        public async Task<IActionResult> GetSupportFactors([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"Exec spWeb_GetSupportFactor '{factorwebDto.StartDateTarget}','{factorwebDto.EndDateTarget}','{factorwebDto.SearchTarget}','{factorwebDto.BrokerRef}','{factorwebDto.IsShopFactor}'";







            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetSupportFactors));
                return StatusCode(500, "Internal server error.");
            }
        }





        [HttpPost]
        [Route("WebFactorInsert")]
        public async Task<IActionResult> WebFactorInsert([FromBody] FactorwebDto factorwebDto)
        {


            string UserId = WebUtility.UrlDecode(HttpContext.Request.Headers["UI"].FirstOrDefault()) ?? string.Empty;



            string query = $"spWeb_Factor_Insert  @ClassName ='{factorwebDto.ClassName}',@StackRef ={factorwebDto.StackRef},@UserId ={UserId},@Date ='{factorwebDto.FactorDate}',@Customer ={factorwebDto.CustomerCode},@Explain ='{factorwebDto.Explain}',@IsShopFactor  = {factorwebDto.IsShopFactor}";


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

            string UserId = WebUtility.UrlDecode(HttpContext.Request.Headers["UI"].FirstOrDefault()) ?? string.Empty;

            string query = $"spWeb_Factor_InsertRow  @ClassName ='{factorRow.ClassName}', @FactorCode={factorRow.FactorRef}, @GoodRef ={factorRow.GoodRef},@Amount ={factorRow.Amount},@Price ={factorRow.Price},@UserId ={UserId},@MustHasAmount ={factorRow.MustHasAmount}, @MergeFlag ={factorRow.MergeFlag} ";




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



        [HttpGet]
        [Route("GetCustomerFactor")]
        public async Task<IActionResult> GetCustomerFactor(string Where)
        {


            string query = $"spWeb_GetCustomerFactor {Where}";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCustomerFactor));
                return StatusCode(500, "Internal server error.");
            }
        }





        [HttpPost]
        [Route("GetSupportPanel")]
        public async Task<IActionResult> GetSupportPanel([FromBody] SupportDto supportDto)

        {
            // 1 support panel
            // 2 EmptyEndTimeCount
            string UserId = WebUtility.UrlDecode(HttpContext.Request.Headers["UI"].FirstOrDefault()) ?? string.Empty;


            string query = $"   spWeb_GetSupportPanel @DateTarget = '{supportDto.DateTarget}', @UserId = {UserId}, @Flag = {supportDto.Flag}";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "SupportDatas", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetSupportPanel));
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










        [HttpPost]
        [Route("GetAllInternalFactor")]
        public async Task<IActionResult> GetAllInternalFactor([FromBody] FactorwebDto factorwebDto)
        {
            try
            {

                string rowLevelSecurityCondition = await db.GetRowLevelSecurityStringAsync(HttpContext, "TFactor");

                string query = $@" spWeb_GetInternalFactor '{factorwebDto.StartDateTarget}', '{factorwebDto.EndDateTarget}', '{factorwebDto.SearchTarget}', '{rowLevelSecurityCondition}', '{factorwebDto.IsShopFactor}'   ";

                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);

                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAllInternalFactor));

                return StatusCode(500, "Internal server error.");
            }
        }


















    }
}
