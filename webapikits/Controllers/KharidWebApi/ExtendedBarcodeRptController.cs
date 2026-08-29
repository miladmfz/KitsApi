using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KharidWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtendedBarcodeRptController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<ExtendedBarcodeRptController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public ExtendedBarcodeRptController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<ExtendedBarcodeRptController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetGoodBarCode")]
        public async Task<IActionResult> GetGoodBarCode([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  GoodBarCode ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodBarCodes", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodBarCode));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
