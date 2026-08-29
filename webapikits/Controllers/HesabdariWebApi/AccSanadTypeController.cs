using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.HesabdariWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccSanadTypeController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<AccSanadTypeController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public AccSanadTypeController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<AccSanadTypeController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetAccSanadType")]
        public async Task<IActionResult> GetAccSanadType([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  AccSanadType ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AccSanadTypes", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAccSanadType));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
