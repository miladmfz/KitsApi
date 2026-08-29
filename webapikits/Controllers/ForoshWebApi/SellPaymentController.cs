using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellPaymentController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SellPaymentController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public SellPaymentController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<SellPaymentController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetSellPayment")]
        public async Task<IActionResult> GetSellPayment([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwSellPayment ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "SellPayments", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetSellPayment));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
