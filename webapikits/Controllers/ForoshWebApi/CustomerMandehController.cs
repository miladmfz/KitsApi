using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class CustomerMandehController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<CustomerMandehController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public CustomerMandehController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<CustomerMandehController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
        }




        [HttpPost]
        [Route("GetCustomerMandeh")]
        public async Task<IActionResult> GetCustomerMandeh([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwCustomerMandeh ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "CustomerMandehs", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCustomerMandeh));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
