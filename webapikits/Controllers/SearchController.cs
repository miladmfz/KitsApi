using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : Controller
    {

        private readonly IDbService _dbService;
        private readonly IJsonFormatter _jsonFormatter;
        private readonly ILogger<SearchController> _logger;

        public SearchController(IDbService dbService, IJsonFormatter jsonFormatter, ILogger<SearchController> logger)
        {
            _dbService = dbService;
            _jsonFormatter = jsonFormatter;
            _logger = logger;
        }

        [HttpPost]
        [Route("GetGoodList")]
        public async Task<IActionResult> GetGoodList([FromBody] SearchTargetDto searchTargetDto)
        {
            if (string.IsNullOrWhiteSpace(searchTargetDto.SearchTarget))
                return BadRequest("SearchTarget cannot be empty.");

            string query = "Exec spApp_GetGoods2 @RowCount, @SearchTarget, @AppType";
            var parameters = new Dictionary<string, object>
            {
                {"@RowCount", 100},
                {"@SearchTarget", searchTargetDto.SearchTarget},
                {"@AppType", 4}
            };

            try
            {
                DataTable dataTable = await _dbService.ExecSearchQueryAsync(HttpContext, query, parameters);
                var json = _jsonFormatter.JsonResult_Str(dataTable, "Goods");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                LogException(ex);
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpGet]
        [Route("BrokerStack")]
        public async Task<IActionResult> BrokerStack(string BrokerRef)
        {
            if (string.IsNullOrEmpty(BrokerRef))
                BrokerRef = "0";

            string query = "exec spApp_GetBrokerStack @BrokerRef";
            var parameters = new Dictionary<string, object>
            {
                {"@BrokerRef", BrokerRef}
            };

            try
            {
                DataTable dataTable = await _dbService.ExecSearchQueryAsync(HttpContext, query, parameters);
                var json = _jsonFormatter.JsonResult_Str(dataTable, "Text", "BrokerStack");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                LogException(ex);
                return StatusCode(500, "Internal server error.");
            }
        }





        [HttpGet]
        [Route("GetColumnList")]
        public async Task<IActionResult> GetColumnList(string Type, string AppType, string IncludeZero)
        {
            Type = string.IsNullOrEmpty(Type) ? "0" : Type;
            AppType = string.IsNullOrEmpty(AppType) ? "0" : AppType;
            IncludeZero = string.IsNullOrEmpty(IncludeZero) ? "0" : IncludeZero;

            string query = "Exec [spApp_GetColumn] @Param0, @Param1, @Type, @AppType, @IncludeZero";
            var parameters = new Dictionary<string, object>
            {
                {"@Param0", 0},
                {"@Param1", ""},
                {"@Type", Type},
                {"@AppType", AppType},
                {"@IncludeZero", IncludeZero}
            };

            try
            {
                DataTable dataTable = await _dbService.ExecSearchQueryAsync(HttpContext, query, parameters);
                var json = _jsonFormatter.JsonResult_Str(dataTable, "Columns");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                LogException(ex);

                return StatusCode(500, "Internal server error.");
            }
        }

        private void LogException(Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string functionName = "")
        {
            _logger.LogError(ex, "Error occurred in {Function}", functionName);
        }

    }
}

