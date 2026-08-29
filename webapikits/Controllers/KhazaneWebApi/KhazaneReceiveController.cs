using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KhazaneWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class KhazaneReceiveController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<KhazaneReceiveController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public KhazaneReceiveController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<KhazaneReceiveController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
}



        [HttpPost]
        [Route("GetKhazanehReceive")]
        public async Task<IActionResult> GetKhazanehReceive([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwKhazanehReceive ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KhazanehReceives", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetKhazanehReceive));
                return StatusCode(500, "Internal server error.");
            }

        }






    }
}
