using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KharidWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodExchangeController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<GoodExchangeController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public GoodExchangeController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<GoodExchangeController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetGoodExchange")]
        public async Task<IActionResult> GetGoodExchange([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwGoodExchange ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodExchanges", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodExchange));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
