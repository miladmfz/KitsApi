using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.HesabdariWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class VwCentralBrowsController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<VwCentralBrowsController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public VwCentralBrowsController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<VwCentralBrowsController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
        }




        [HttpPost]
        [Route("GetCentralBrowse")]
        public async Task<IActionResult> GetCentralBrowse([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwCentralBrowse ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "CentralBrowses", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentralBrowse));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
