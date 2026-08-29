using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KharidWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodAdjustmentController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<GoodAdjustmentController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public GoodAdjustmentController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<GoodAdjustmentController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetGoodAdjustment")]
        public async Task<IActionResult> GetGoodAdjustment([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwGoodAdjustment ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodAdjustments", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodAdjustment));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
