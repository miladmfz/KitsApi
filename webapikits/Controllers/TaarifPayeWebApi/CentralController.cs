using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.TaarifPayeWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentralController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<CentralController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public CentralController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<CentralController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }


        [HttpGet]
        [Route("GetAddressByCentral")]
        public async Task<IActionResult> GetAddressByCentral(string CentralRef)
        {

            string query = $" Select * from VwAddress where CentralRef={CentralRef}";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Address", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAddressByCentral));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpPost]
        [Route("AddressCrudService")]
        public async Task<IActionResult> AddressCrudService([FromBody] JsonModelDto jsonModelDto)

        {


            string query = $"Exec spAddress_AddNew '{jsonModelDto.JsonData}' ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Address", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(AddressCrudService));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("CentralCrudService")]
        public async Task<IActionResult> CentralCrudService([FromBody] JsonModelDto jsonModelDto)

        {


            string query = $"Exec spCentral_AddNew '{jsonModelDto.JsonData}' ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Central", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CentralCrudService));
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpPost]
        [Route("GetCentral")]
        public async Task<IActionResult> GetCentral([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Select * From Central where (Name like N'%{searchTargetDto.SearchTarget}%' Or FName like N'%{searchTargetDto.SearchTarget}%' Or Title like N'%{searchTargetDto.SearchTarget}%')  ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Centrals", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentral));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("GetCentralById")]
        public async Task<IActionResult> GetCentralById(string CentralCode)
        {

            string query = $" Select * From vwCentral Where CentralCOde ={CentralCode}";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Centrals", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentralById));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpPost]
        [Route("GetCentralGrp")]
        public async Task<IActionResult> GetCentralGrp([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" spWeb_GetCentralGrp   N'%{searchTargetDto.SearchTarget}%'";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "CentralGrps", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentralGrp));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetZone_Ostan")]
        public async Task<IActionResult> GetZone_Ostan()
        {

            string query = $"Select Ostan,OstanCode from Zone Group by Ostan,OstanCode order by 1   ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Zones", ""); ;
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetZone_Ostan));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetZone_Shahr")]
        public async Task<IActionResult> GetZone_Shahr(string SearchTarget)
        {

            string query = $"Select Shahr,ShahrCode from Zone Where OstanCode={SearchTarget} Order by 1 ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Zones", ""); ;
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetZone_Shahr));
                return StatusCode(500, "Internal server error.");
            }

        }














    }



}
