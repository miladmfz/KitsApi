using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;

namespace webapikits.Controllers.KowsarBaseWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternalWorkItemController : ControllerBase
    {

        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<InternalWorkItemController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();

        public InternalWorkItemController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<InternalWorkItemController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }









        [HttpPost]
        [Route("WorkItem_Insert")]
        public async Task<IActionResult> WorkItem_Insert([FromBody] WorkItemDto workItemDto)
        {
            try
            {
                // رشته‌ها با تک کوتیشن ' و nullها با "" جایگزین شدند
                string query = $"Exec spWeb_WorkItem_Insert " +
                               $"'{workItemDto.Title ?? ""}', " +
                               $"'{workItemDto.Explain ?? ""}', " +
                               $"{workItemDto.Status}, " +
                               $"{workItemDto.Priority}, " +
                               $"'{workItemDto.OriginalDate ?? ""}', " +
                               $"'{workItemDto.TargetDate ?? ""}', " +
                               $"'{workItemDto.ModuleName ?? ""}', " +
                               $"'{workItemDto.ClassName ?? ""}', " +
                               $"{workItemDto.ObjectRef}, " +
                               $"{workItemDto.OwnerRef}, " +
                               $"{workItemDto.CreatorRef}";

                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WorkItems", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(WorkItem_Insert));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("WorkItem_Update")]
        public async Task<IActionResult> WorkItem_Update([FromBody] WorkItemDto workItemDto)
        {
            if (string.IsNullOrEmpty(workItemDto.WorkItemCode))
                return BadRequest("WorkItemCode is required");

            try
            {
                string query = $"Exec spWeb_WorkItem_Update " +
                               $"{workItemDto.WorkItemCode}, " +
                               $"'{workItemDto.Title ?? ""}', " +
                               $"'{workItemDto.Explain ?? ""}', " +
                               $"{workItemDto.Status}, " +
                               $"{workItemDto.Priority}, " +
                               $"'{workItemDto.TargetDate ?? ""}', " +
                               $"'{workItemDto.ModuleName ?? ""}', " +
                               $"'{workItemDto.ClassName ?? ""}', " +
                               $"{(string.IsNullOrEmpty(workItemDto.ObjectRef) ? "NULL" : workItemDto.ObjectRef)} ";

                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WorkItems", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(WorkItem_Update));
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("WorkItem_SetStatus")]
        public async Task<IActionResult> WorkItem_SetStatus([FromBody] WorkItemDto workItemDto)
        {
            if (string.IsNullOrEmpty(workItemDto.WorkItemCode) || string.IsNullOrEmpty(workItemDto.Status))
                return BadRequest("WorkItemCode and Status are required");

            try
            {
                string query = $"Exec spWeb_WorkItem_SetStatus " +
                               $"{workItemDto.WorkItemCode}, " +
                               $"{workItemDto.Status}, " +
                               $"'{workItemDto.ChangeStateDate ?? ""}'";

                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WorkItems", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(WorkItem_SetStatus));
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("WorkItem_Get")]
        public async Task<IActionResult> WorkItem_Get([FromBody] WorkItemDto workItemDto)
        {
            try
            {

                string query = $"Exec spWeb_WorkItem_Get {workItemDto.CentralRef},{workItemDto.Status}, '{workItemDto.SearchTarget ?? ""}'";

                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WorkItems", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(WorkItem_Get));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("WorkItem_Delete")]
        public async Task<IActionResult> WorkItem_Delete([FromBody] WorkItemDto workItemDto)
        {
            if (string.IsNullOrEmpty(workItemDto.WorkItemCode))
                return BadRequest("WorkItemCode is required");

            try
            {
                string query = $"Exec spWeb_WorkItem_Delete {workItemDto.WorkItemCode}";

                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WorkItems", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(WorkItem_Delete));
                return StatusCode(500, "Internal server error.");
            }
        }








































































































    }
}
