using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class DecIncrementController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<DecIncrementController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public DecIncrementController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<DecIncrementController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
        }




        [HttpPost]
        [Route("GetDecIncrement")]
        public async Task<IActionResult> GetDecIncrement([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwDecIncrement ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "DecIncrements", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetDecIncrement));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
