using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.HesabdariWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccSndExplainController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<AccSndExplainController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public AccSndExplainController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<AccSndExplainController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetAccSndExplain")]
        public async Task<IActionResult> GetAccSndExplain([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  AccSndExplain ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AccSndExplains", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAccSndExplain));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
