using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;

namespace webapikits.Controllers.KharidWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<UnitController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public UnitController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<UnitController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }






        [HttpPost]
        [Route("GetUnits")]
        public async Task<IActionResult> GetUnits([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" spWeb_GetUnits   N'%{searchTargetDto.SearchTarget}%'";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Units", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetUnits));
                return StatusCode(500, "Internal server error.");
            }

        }




    }
}
