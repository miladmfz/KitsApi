using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.TaarifPayeWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class CurrencyExchangeController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<CurrencyExchangeController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public CurrencyExchangeController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<CurrencyExchangeController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
}



        [HttpPost]
        [Route("GetCurrencyExchange")]
        public async Task<IActionResult> GetCurrencyExchange([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwCurrencyExchange ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "CurrencyExchanges", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCurrencyExchange));
                return StatusCode(500, "Internal server error.");
            }

        }



    }
}
