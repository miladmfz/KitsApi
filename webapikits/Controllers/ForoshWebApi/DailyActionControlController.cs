using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class DailyActionControlController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<DailyActionControlController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public DailyActionControlController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<DailyActionControlController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
        }




        [HttpPost]
        [Route("GetDailyActionControl")]
        public async Task<IActionResult> GetDailyActionControl([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwDailyActionControl ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "DailyActionControls", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetDailyActionControl));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
