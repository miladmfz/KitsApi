using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class CafeResturantFactorController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<CafeResturantFactorController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public CafeResturantFactorController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<CafeResturantFactorController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
        }




        [HttpPost]
        [Route("GetCafeResturantFactor")]
        public async Task<IActionResult> GetCafeResturantFactor([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"  select * from  vwFactor Where isShopFactor in (4,5,6) ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCafeResturantFactor));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
