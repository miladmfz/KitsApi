using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;

namespace webapikits.Controllers.KhazaneWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnChecksController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<ReturnChecksController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public ReturnChecksController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<ReturnChecksController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetReturnChecks")]
        public async Task<IActionResult> GetReturnChecks([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwReturnChecks ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "ReturnChecks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetReturnChecks));
                return StatusCode(500, "Internal server error.");
            }

        }






    }
}
