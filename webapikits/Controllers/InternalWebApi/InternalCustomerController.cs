using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;

namespace webapikits.Controllers.KowsarBaseWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternalCustomerController : ControllerBase
    {

        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<InternalCustomerController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();

        public InternalCustomerController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<InternalCustomerController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }






        [HttpPost]
        [Route("EditCustomerProperty")]
        public async Task<IActionResult> EditCustomerProperty([FromBody] CustomerWebDto customerWebDto)
        {

            string query = $"spWeb_EditCustomerProperty '{customerWebDto.AppNumber}','{customerWebDto.DatabaseNumber}','{customerWebDto.Delegacy}','{customerWebDto.Explain}',{customerWebDto.ObjectRef}";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(EditCustomerProperty));
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
        [Route("GetCustomerReport")]
        public async Task<IActionResult> GetCustomerReport([FromBody] KowsarReportDto kowsarReportDto)
        {

            try
            {

                string query = $"Exec spWeb_GetCustomerReport @StartDateTarget = '{kowsarReportDto.StartDateTarget}',@EndDateTarget = '{kowsarReportDto.EndDateTarget}',@SearchTarget = '{kowsarReportDto.SearchTarget}',@CustomerRef = {kowsarReportDto.CustomerRef},@Flag = {kowsarReportDto.Flag}";


                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarReports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GetCustomerReport));
                return StatusCode(500, "Internal server error.");
            }
        }









        [HttpPost]
        [Route("GetCity")]
        public async Task<IActionResult> GetCity([FromBody] SearchTargetDto searchTargetDto)
        {



            string query = $"Select CityCode,Name from City ";

            DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
            string json = jsonClass.JsonResult_Str(dataTable, "Citys", "");
            return Content(json, "application/json");


        }



        [HttpGet]
        [Route("GetCustomerByCode")]
        public async Task<IActionResult> GetCustomerByCode(string CustomerCode)
        {

            string query = $"select CustomerCode, FName, Name, CityCode, CityName, Address, Phone, Mobile, Email, Explain, ZipCode from vwcustomer Where CustomerCode= {CustomerCode} ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCustomerByCode));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpGet]
        [Route("GetCustomerByCodeFromSantral")]
        public async Task<IActionResult> GetCustomerByCodeFromSantral(string CentralRef)
        {

            string query = $"select CustName_Small,Explain,AppNumber,DatabaseNumber,lockNumber from vwCustomer where centralref= {CentralRef} ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCustomerByCodeFromSantral));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("CustomerCrud")]
        public async Task<IActionResult> CustomerCrud([FromBody] CustomerCrudDto customerCrudDto)
        {

            try
            {

                string UserId = WebUtility.UrlDecode(HttpContext.Request.Headers["UI"].FirstOrDefault()) ?? string.Empty;
                string query = $"Exec spWeb_Customer_Crud  @FName ='{customerCrudDto.FName}',  @LName ='{customerCrudDto.LName}', @CityCode ={customerCrudDto.CityCode}, @Address ='{customerCrudDto.Address}', @Phone ='{customerCrudDto.Phone}', @Mobile ='{customerCrudDto.Mobile}', @CustomerCode  ={customerCrudDto.CustomerCode}, @UserId ={UserId}";


                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(CustomerCrud));
                return StatusCode(500, "Internal server error.");
            }
        }












































































    }
}
