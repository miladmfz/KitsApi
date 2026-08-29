using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KharidWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodSerialFrameController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<GoodSerialFrameController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public GoodSerialFrameController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<GoodSerialFrameController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetGoodSerial")]
        public async Task<IActionResult> GetGoodSerial([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwGoodSerial ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodSerials", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodSerial));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
