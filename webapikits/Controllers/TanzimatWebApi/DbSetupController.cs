using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.TanzimatWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class DbSetupController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<DbSetupController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public DbSetupController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<DbSetupController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
}



        [HttpPost]
        [Route("GetDbSetup")]
        public async Task<IActionResult> GetDbSetup([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Select KeyId, KeyValue, DataValue, Description, SubSystem from DbSetup Where " +
                $" Description Like N'%{searchTargetDto.SearchTarget}%' Or KeyValue Like N'%{searchTargetDto.SearchTarget}%' Or  " +
                $" DataValue Like N'%{searchTargetDto.SearchTarget}%'  Or SubSystem Like N'%{searchTargetDto.SearchTarget}%'  ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "DbSetups", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetDbSetup));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpPost]
        [Route("UpdateDbSetup")]
        public async Task<IActionResult> UpdateDbSetup([FromBody] DbSetupDto dbSetupDto)
        {


            string query = $"Update DbSetup Set DataValue ='{dbSetupDto.DataValue}' Where KeyValue = '{dbSetupDto.KeyValue}' And KeyId ={dbSetupDto.KeyId} ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "DbSetups", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UpdateDbSetup));
                return StatusCode(500, "Internal server error.");
            }

        }
















    }
}
