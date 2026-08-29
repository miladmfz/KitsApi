using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KhazaneWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckCanselController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<CheckCanselController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public CheckCanselController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<CheckCanselController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetCheckCancel")]
        public async Task<IActionResult> GetCheckCancel([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwCheckCancel ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "CheckCancels", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCheckCancel));
                return StatusCode(500, "Internal server error.");
            }

        }






    }
}
