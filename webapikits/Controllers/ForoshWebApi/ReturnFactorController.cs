using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnFactorController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<ReturnFactorController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public ReturnFactorController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<ReturnFactorController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetReturnFactor")]
        public async Task<IActionResult> GetReturnFactor([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwReturnFactor ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "ReturnFactors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetReturnFactor));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
