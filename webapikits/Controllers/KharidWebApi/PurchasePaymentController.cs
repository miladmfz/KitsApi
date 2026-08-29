using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KharidWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasePaymentController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<PurchasePaymentController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public PurchasePaymentController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<PurchasePaymentController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetPurchasePayment")]
        public async Task<IActionResult> GetPurchasePayment([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwPurchasePayment ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PurchasePayments", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetPurchasePayment));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
