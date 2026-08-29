using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class PreFactorController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<PreFactorController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public PreFactorController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<PreFactorController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
}




        [HttpPost]
        [Route("UpdatePreFactorPFState")]
        public async Task<IActionResult> UpdatePreFactorPFState([FromBody] FactorwebDto factorwebDto)
        {

            string query = $" Update PreFactor Set PFState = {factorwebDto.PFState} Where PreFactorCode={factorwebDto.FactorCode} ;select 1 's'";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UpdatePreFactorPFState));
                return StatusCode(500, "Internal server error.");
            }



        }


        [HttpPost]
        [Route("WebFactorUpdateRow")]
        public async Task<IActionResult> WebFactorUpdateRow([FromBody] FactorRow factorRow)
        {

            string UserId = WebUtility.UrlDecode(HttpContext.Request.Headers["UI"].FirstOrDefault()) ?? string.Empty;

            string query = $"spWeb_Factor_UpdateRow @RowCode ={factorRow.RowCode},@Amount ={factorRow.Amount},@Price ={factorRow.Price}, @ClassName ='{factorRow.ClassName}',@UserId ={UserId}";


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
        [Route("GetWebFactor")]
        public async Task<IActionResult> GetWebFactor([FromBody] FactorwebDto factorwebDto)
        {


            string query = $"spWeb_Get_Factor '{factorwebDto.ClassName}',{factorwebDto.ObjectRef},'{factorwebDto.StartDateTarget}','{factorwebDto.EndDateTarget}','{factorwebDto.SearchTarget}','{factorwebDto.BrokerRef}'";



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
        [Route("GetKowsarCustomer")]
        public async Task<IActionResult> GetKowsarCustomer([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Exec [dbo].[spWeb_GetCustomer] '{searchTargetDto.SearchTarget}' ,{searchTargetDto.BrokerRef}";


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





        [HttpGet]
        [Route("DeleteWebPreFactorRows")]
        public async Task<IActionResult> DeleteWebPreFactorRows(string PreFactorRowCode)
        {

            string query = $" delete from  PreFactorRows where PreFactorRowCode= {PreFactorRowCode}";



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





        [HttpPost]
        [Route("GetCustomerById")]
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
        [Route("GetCentralByCode")]
        public async Task<IActionResult> GetCentralByCode([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Exec [dbo].[spWeb_GetCentralByCode] {searchTargetDto.ObjectRef}";



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




            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Centrals", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentralUser));
                return StatusCode(500, "Internal server error.");
            }

        }







        [HttpPost]
        [Route("GetAutLetterList")]
        public async Task<IActionResult> GetAutLetterList([FromBody] SearchTargetLetterDto searchTargetLetterDto)
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






            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAutLetterList));
                return StatusCode(500, "Internal server error.");
            }


        }










        [HttpPost]
        [Route("LetterInsert")]
        public async Task<IActionResult> LetterInsert([FromBody] LetterInsert letterInsert)
        {


            string CreatorCentral = _configuration.GetValue<string>("AppSettings:Support_CreatorCentral");


            string query = $"exec dbo.spAutLetter_Insert @LetterDate='{letterInsert.LetterDate}', @InOutFlag={letterInsert.InOutFlag},@Title ='{letterInsert.title}', " +
                $"@Description='{SanitizeInput(letterInsert.Description)}',@State ='{letterInsert.LetterState}',@Priority ='{letterInsert.LetterPriority}', @ReceiveType =N'دستی', " +
                $"@CreatorCentral ={letterInsert.CreatorCentral}, @OwnerCentral ={letterInsert.OwnerCentral} ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");

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


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
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



            string UserId = WebUtility.UrlDecode(HttpContext.Request.Headers["UI"].FirstOrDefault()) ?? string.Empty;

            string query = $"spWeb_Factor_Insert  @ClassName ='{factorwebDto.ClassName}',@StackRef ={factorwebDto.StackRef},@UserId ={UserId},@Date ='{factorwebDto.FactorDate}'," +
                $"@Customer ={factorwebDto.CustomerCode},@Explain ='{factorwebDto.Explain}',@IsShopFactor  = {factorwebDto.IsShopFactor}";


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

            string query = $"spWeb_Factor_InsertRow  @ClassName ='{factorRow.ClassName}', @FactorCode={factorRow.FactorRef}, @GoodRef ={factorRow.GoodRef},@Amount ={factorRow.Amount}," +
                $"@Price ={factorRow.Price},@UserId ={UserId},@MustHasAmount ={factorRow.MustHasAmount}, @MergeFlag ={factorRow.MergeFlag} ";





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
