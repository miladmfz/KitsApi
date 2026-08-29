using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.HesabdariWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccTafzilyController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<AccTafzilyController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public AccTafzilyController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<AccTafzilyController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetAccTafzily")]
        public async Task<IActionResult> GetAccTafzily([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  AccTafzilyMandeh ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AccTafzilys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAccTafzily));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
