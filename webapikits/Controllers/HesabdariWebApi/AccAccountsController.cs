using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.HesabdariWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccAccountsController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<AccAccountsController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public AccAccountsController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<AccAccountsController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetAccAccounts")]
        public async Task<IActionResult> GetAccAccounts([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwAccAccounts ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AccAccountss", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAccAccounts));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
