using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KhazaneWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhazanePaymentController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<KhazanePaymentController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public KhazanePaymentController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<KhazanePaymentController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetKhazanehPayment")]
        public async Task<IActionResult> GetKhazanehPayment([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwKhazanehPayment ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KhazanehPayments", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetKhazanehPayment));
                return StatusCode(500, "Internal server error.");
            }

        }






    }
}
