using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodValueInPathGroupController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<GoodValueInPathGroupController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public GoodValueInPathGroupController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<GoodValueInPathGroupController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetGoodValueInPathGroup")]
        public async Task<IActionResult> GetGoodValueInPathGroup([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwGoodValueInPathGroup ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodValueInPathGroups", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodValueInPathGroup));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
