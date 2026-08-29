using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KharidWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamaneGoodController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SamaneGoodController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public SamaneGoodController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<SamaneGoodController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetSamaneGood")]
        public async Task<IActionResult> GetSamaneGood([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwSamaneGood ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "SamaneGoods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetSamaneGood));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
