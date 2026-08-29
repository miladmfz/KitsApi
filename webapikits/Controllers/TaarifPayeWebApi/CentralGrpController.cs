using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.TaarifPayeWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class CentralGrpController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<CentralGrpController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public CentralGrpController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<CentralGrpController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
}





        [HttpPost]
        [Route("GetCentralGrp")]
        public async Task<IActionResult> GetCentralGrp([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" spWeb_GetCentralGrp   N'%{searchTargetDto.SearchTarget}%'";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "CentralGrps", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentralGrp));
                return StatusCode(500, "Internal server error.");
            }

        }








    }
}
