using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KharidWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonJobController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<PersonJobController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public PersonJobController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<PersonJobController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetPersonJob")]
        public async Task<IActionResult> GetPersonJob([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwPersonJob ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PersonJobs", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetPersonJob));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
