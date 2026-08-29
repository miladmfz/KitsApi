using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnShopFactorController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<ReturnShopFactorController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public ReturnShopFactorController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<ReturnShopFactorController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetReturnShopFactor")]
        public async Task<IActionResult> GetReturnShopFactor([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwReturnShopFactor ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "ReturnShopFactors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetReturnShopFactor));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
