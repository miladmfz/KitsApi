using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopFactorController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<ShopFactorController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public ShopFactorController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<ShopFactorController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetShopFactor")]
        public async Task<IActionResult> GetShopFactor([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwShopFactor ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "ShopFactors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetShopFactor));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
