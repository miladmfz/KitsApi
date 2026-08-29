using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrokerPathController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<BrokerPathController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public BrokerPathController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<BrokerPathController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }





        [HttpPost]
        [Route("GetBrokerPath")]
        public async Task<IActionResult> GetBrokerPath([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwBrokerPath ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "BrokerPaths", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetBrokerPath));
                return StatusCode(500, "Internal server error.");
            }

        }







    }
}
