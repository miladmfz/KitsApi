using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.TanzimatWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class FiscalPeriodController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<FiscalPeriodController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public FiscalPeriodController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<FiscalPeriodController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
}

        [HttpPost]
        [Route("GetFiscalPeriod")]
        public async Task<IActionResult> GetFiscalPeriod([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  FiscalPeriod ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "FiscalPeriods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetFiscalPeriod));
                return StatusCode(500, "Internal server error.");
            }

        }

    }
}
