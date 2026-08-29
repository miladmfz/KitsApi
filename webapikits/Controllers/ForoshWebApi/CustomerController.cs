using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<CustomerController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public CustomerController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<CustomerController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
        }




        [HttpPost]
        [Route("GetCustomer")]
        public async Task<IActionResult> GetCustomer([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"  select * from  vwCustomer ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCustomer));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
