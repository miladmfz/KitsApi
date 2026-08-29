using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.KharidWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsGrpController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<GoodsGrpController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public GoodsGrpController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<GoodsGrpController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
        [Route("GetGoodsGrp")]
        public async Task<IActionResult> GetGoodsGrp([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" spWeb_GetGoodsGrp   N'%{searchTargetDto.SearchTarget}%'";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodsGrps", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodsGrp));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpPost]
        [Route("GoodsGrpCrudService")]
        public async Task<IActionResult> GoodsGrpCrudService([FromBody] JsonModelDto jsonModelDto)

        {
            var UserId = HttpContext.Request.Headers["UI"].FirstOrDefault();


            string query = $"Exec spGoodsGrp_AddNew '{jsonModelDto.JsonData}',{UserId} ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodsGrps", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodsGrpCrudService));
                return StatusCode(500, "Internal server error.");
            }
        }




    }
}
