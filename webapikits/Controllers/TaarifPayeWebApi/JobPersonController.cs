using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.TaarifPayeWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class JobPersonController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<JobPersonController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public JobPersonController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<JobPersonController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
}

        [HttpPost]
        [Route("GetJobPerson")]
        public async Task<IActionResult> GetJobPerson([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"select * from vwjobperson ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "JobPersons", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetJobPerson));
                return StatusCode(500, "Internal server error.");
            }

        }

    }
}
