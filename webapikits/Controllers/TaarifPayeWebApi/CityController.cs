using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.TaarifPayeWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<CityController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public CityController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<CityController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
}



        [HttpPost]
        [Route("GetCity")]
        public async Task<IActionResult> GetCity([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" spWeb_GetCity   N'%{searchTargetDto.SearchTarget}%'";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Citys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCity));
                return StatusCode(500, "Internal server error.");
            }

        }






        [HttpPost]
        [Route("CityCrudService")]
        public async Task<IActionResult> CityCrudService([FromBody] JsonModelDto jsonModelDto)

        {


            string query = $"Exec spCity_AddNew '{jsonModelDto.JsonData}' ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Citys", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CityCrudService));
                return StatusCode(500, "Internal server error.");
            }
        }










    }
}
