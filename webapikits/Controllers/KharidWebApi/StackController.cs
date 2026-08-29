using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KharidWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class StackController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<StackController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public StackController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<StackController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("GetStacks")]
        public async Task<IActionResult> GetStacks([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" spWeb_GetStacks   N'%{searchTargetDto.SearchTarget}%'";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Stacks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetStacks));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpPost]
        [Route("StacksCrudService")]
        public async Task<IActionResult> StacksCrudService([FromBody] JsonModelDto jsonModelDto)

        {


            string query = $"Exec spStacks_AddNew '{jsonModelDto.JsonData}' ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Stacks", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(StacksCrudService));
                return StatusCode(500, "Internal server error.");
            }
        }





        









    }
}
