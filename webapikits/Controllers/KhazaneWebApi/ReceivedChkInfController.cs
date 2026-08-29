using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KhazaneWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class ReceivedChkInfController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<ReceivedChkInfController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public ReceivedChkInfController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<ReceivedChkInfController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
}




        [HttpPost]
        [Route("GetReceivedChkInf")]
        public async Task<IActionResult> GetReceivedChkInf([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwReceivedChkInf ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "ReceivedChkInfs", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetReceivedChkInf));
                return StatusCode(500, "Internal server error.");
            }

        }






    }
}
