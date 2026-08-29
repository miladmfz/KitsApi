using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KharidWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnPurchaseController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<ReturnPurchaseController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public ReturnPurchaseController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<ReturnPurchaseController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetReturnPurchase")]
        public async Task<IActionResult> GetReturnPurchase([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwReturnPurchase ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "ReturnPurchases", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetReturnPurchase));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
