using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KhazaneWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckPassesInfController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<CheckPassesInfController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public CheckPassesInfController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<CheckPassesInfController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetCheckPassedInf")]
        public async Task<IActionResult> GetCheckPassedInf([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwCheckPassedInf ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "CheckPassedInfs", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCheckPassedInf));
                return StatusCode(500, "Internal server error.");
            }

        }






    }
}
