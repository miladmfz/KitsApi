using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.ForoshWebApi
{
    [Route("api/[controller]")]
[ApiController]
public class CustomerGroupController : ControllerBase
    {
        private readonly IDbService db;
private readonly IJsonFormatter _jsonFormatter1;
private readonly ILogger<CustomerGroupController> _logger;
private readonly IConfiguration _configuration;
JsonClass jsonClass = new JsonClass();


public CustomerGroupController(
    IDbService dbService,
    IJsonFormatter jsonFormatter,
    ILogger<CustomerGroupController> logger,
    IConfiguration configuration
    )
{
    db = dbService;
    _jsonFormatter1 = jsonFormatter;
    _logger = logger;
    _configuration = configuration;
        }




        [HttpPost]
        [Route("GetCustomerGroup")]
        public async Task<IActionResult> GetCustomerGroup([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $" select * from  vwCustomerGroup ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "CustomerGroups", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCustomerGroup));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}
