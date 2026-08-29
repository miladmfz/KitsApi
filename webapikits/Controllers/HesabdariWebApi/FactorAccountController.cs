using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.HesabdariWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactorAccountController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<FactorAccountController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public FactorAccountController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<FactorAccountController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetFactorAccount")]
        public async Task<IActionResult> GetFactorAccount([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwFactorAccount ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "FactorAccounts", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetFactorAccount));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
