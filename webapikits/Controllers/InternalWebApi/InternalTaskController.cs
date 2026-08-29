using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;

namespace webapikits.Controllers.KowsarBaseWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternalTaskController : ControllerBase
    {

        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<InternalTaskController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();

        public InternalTaskController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<InternalTaskController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpGet]
        [Route("GetPatterns")]
        public async Task<IActionResult> GetPatterns()
        {

            string query = $"Select * from Pattern";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Patterns", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetPatterns));
                return StatusCode(500, "Internal server error.");
            }


        }





        [HttpGet]
        [Route("GetGoodFromPattern")]
        public async Task<IActionResult> GetGoodFromPattern(string PatternCode)
        {

            string query = $"Select * from PatternGood left Join Pattern on PatternCode =PatternRef left Join Good on GoodCode =GoodRef Where PatternRef= {PatternCode}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodFromPattern));
                return StatusCode(500, "Internal server error.");
            }


        }



        

        [HttpPost]
        [Route("Pattern_Crud")]
        public async Task<IActionResult> Pattern_Crud([FromBody] PatternDto patternDto)
        {



            string query = $"spweb_Pattern_Crud @PatternCode = {patternDto.PatternCode}, @Title = '{patternDto.Title}', @Explain = '{patternDto.Explain}'";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Patterns", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(Pattern_Crud));
                return StatusCode(500, "Internal server error.");
            }
        }





        [HttpPost]
        [Route("PatternGood_Add")]
        public async Task<IActionResult> PatternGood_Add([FromBody] PatternDto patternDto)
        {



            string query = $" spweb_PatternGood_AddNew @PatternRef = {patternDto.PatternRef}, @GoodRef = {patternDto.GoodRef}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Patterns", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(PatternGood_Add));
                return StatusCode(500, "Internal server error.");
            }
        }






        [HttpGet]
        [Route("PatternGood_Del")]
        public async Task<IActionResult> PatternGood_Del(string PatternGoodCode)
        {

            string query = $"Delete From PatternGood Where PatternGoodCode= {PatternGoodCode}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PatternGood_Del));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpGet]
        [Route("GetTaskFromGood")]
        public async Task<IActionResult> GetTaskFromGood(string GoodCode)
        {

            string query = $"spWeb_GetGoodTask {GoodCode}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetTaskFromGood));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpGet]
        [Route("GoodTask_Del")]
        public async Task<IActionResult> GoodTask_Del(string GoodTaskCode)
        {

            string query = $"Delete From GoodTask Where GoodTaskCode= {GoodTaskCode}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodTask_Del));
                return StatusCode(500, "Internal server error.");
            }


        }







        [HttpGet]
        [Route("GoodTaskRow_Factor_Add")]
        public async Task<IActionResult> GoodTaskRow_Factor_Add(string HeaderRef)
        {

            string query = $"spweb_GoodTaskRow_AddNew  @ClassName ='Factor' , @HeaderRef ={HeaderRef}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTaskRows", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodTaskRow_Factor_Add));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("GoodTaskRow_Factor_Del")]
        public async Task<IActionResult> GoodTaskRow_Factor_Del(string HeaderRef, string GoodRef)
        {

            string query = $"Delete from GoodTaskRow Where  ClassName ='Factor' And HeaderRef ={HeaderRef} And GoodRef = {GoodRef}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTaskRows", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodTaskRow_Factor_Del));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpGet]
        [Route("GoodTaskRow_Get_ByFactorRow")]
        public async Task<IActionResult> GoodTaskRow_Get_ByFactorRow(string FactorRowCode)
        {

            string query = $"spweb_GoodTaskRow_Get_ByFactorRow {FactorRowCode} ";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTaskRows", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodTaskRow_Get_ByFactorRow));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpGet]
        [Route("GoodTaskRow_Get_ByFactor")]
        public async Task<IActionResult> GoodTaskRow_Get_ByFactor(string FactorCode)
        {

            string query = $"spweb_GoodTaskRow_Get_ByFactor {FactorCode}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTaskRows", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodTaskRow_Get_ByFactor));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpGet]
        [Route("GoodTaskRow_Get_ByCustomerRow")]
        public async Task<IActionResult> GoodTaskRow_Get_ByCustomerRow(string CustomerGoodCode)
        {

            string query = $"spweb_GoodTaskRow_Get_ByCustomerRow {CustomerGoodCode} ";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTaskRows", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodTaskRow_Get_ByCustomerRow));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpGet]
        [Route("GoodTaskRow_Get_ByCustomer")]
        public async Task<IActionResult> GoodTaskRow_Get_ByCustomer(string CustomerCode)
        {

            string query = $"dbo.spweb_GoodTaskRow_Get_ByCustomer {CustomerCode}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTaskRows", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodTaskRow_Get_ByCustomer));
                return StatusCode(500, "Internal server error.");
            }


        }






        [HttpGet]
        [Route("GetCustomerGood")]
        public async Task<IActionResult> GetCustomerGood(string CustomerRef)
        {

            string query = $" select * from CustomerGood left join Good on  GoodRef = GoodCode where CustomerRef= {CustomerRef}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "CustomerGoods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCustomerGood));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpGet]
        [Route("CustomerGood_AddNew")]
        public async Task<IActionResult> CustomerGood_AddNew(string CustomerRef, string GoodRef)
        {

            string query = $" spweb_CustomerGood_AddNew @CustomerRef = {CustomerRef}, @GoodRef = {GoodRef}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "CustomerGoods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerGood_AddNew));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpGet]
        [Route("CustomerGood_Del")]
        public async Task<IActionResult> CustomerGood_Del(string CustomerGoodCode)
        {

            string query = $" delete from CustomerGood Where CustomerGoodCode ={CustomerGoodCode}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "CustomerGoods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerGood_Del));
                return StatusCode(500, "Internal server error.");
            }


        }






        [HttpGet]
        [Route("GoodTaskRow_Customer_Add")]
        public async Task<IActionResult> GoodTaskRow_Customer_Add(string HeaderRef)
        {

            string query = $"spweb_GoodTaskRow_AddNew  @ClassName ='Customer' , @HeaderRef ={HeaderRef}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTaskRows", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodTaskRow_Customer_Add));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("GoodTaskRow_Customer_Del")]
        public async Task<IActionResult> GoodTaskRow_Customer_Del(string HeaderRef, string GoodRef)
        {

            string query = $"Delete from GoodTaskRow Where  ClassName ='Customer' And HeaderRef ={HeaderRef} And GoodRef = {GoodRef}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTaskRows", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodTaskRow_Customer_Del));
                return StatusCode(500, "Internal server error.");
            }


        }





        [HttpPost]
        [Route("GoodTaskRow_Edit")]
        public async Task<IActionResult> GoodTaskRow_Edit([FromBody] GoodTaskRowDto goodTaskRowDto)
        {



            string query = $"spweb_GoodTaskRow_Edit   @GoodTaskRowCode ={goodTaskRowDto.GoodTaskRowCode},    @TaskDate ='{goodTaskRowDto.TaskDate}',    @StartTime ='{goodTaskRowDto.StartTime}',    @EndTime ='{goodTaskRowDto.EndTime}',   @State ={goodTaskRowDto.State},    @CompanyPerson ='{goodTaskRowDto.CompanyPerson}',    @Explain ='{goodTaskRowDto.Explain}',    @CentralRef ={goodTaskRowDto.CentralRef}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTaskRows", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GoodTaskRow_Edit));
                return StatusCode(500, "Internal server error.");
            }
        }





        [HttpPost]
        [Route("GoodTaskRow_EditInfo")]
        public async Task<IActionResult> GoodTaskRow_EditInfo([FromBody] GoodTaskRowDto goodTaskRowDto)
        {



            string query = $"spweb_GoodTaskRow_EditInfo   @GoodTaskRowCode ={goodTaskRowDto.GoodTaskRowCode} ,    @CompanyPerson ='{goodTaskRowDto.CompanyPerson}',    @Explain ='{goodTaskRowDto.Explain}'";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTaskRows", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GoodTaskRow_EditInfo));
                return StatusCode(500, "Internal server error.");
            }
        }








        [HttpPost]
        [Route("GoodTaskRow_ChangeState")]
        public async Task<IActionResult> GoodTaskRow_ChangeState([FromBody] GoodTaskRowDto goodTaskRowDto)
        {



            string query = $"spweb_GoodTaskRow_ChangeState   @GoodTaskRowCode ={goodTaskRowDto.GoodTaskRowCode},   @State ={goodTaskRowDto.State},    @TaskDate ='{goodTaskRowDto.TaskDate}',    @StartTime ='{goodTaskRowDto.StartTime}',    @EndTime ='{goodTaskRowDto.EndTime}',    @CentralRef ={goodTaskRowDto.CentralRef}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTaskRows", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GoodTaskRow_ChangeState));
                return StatusCode(500, "Internal server error.");
            }
        }





        [HttpPost]
        [Route("GetTasks")]
        public async Task<IActionResult> GetTasks([FromBody] KowsarTaskDto dto)
        {
            // تبدیل TaskRef و Flag به int یا NULL
            string taskRef = !string.IsNullOrEmpty(dto.TaskCode) ? dto.TaskCode : "NULL";
            string flag = !string.IsNullOrEmpty(dto.Flag) ? dto.Flag : "0";

            string query = $"Exec spWeb_GetKowsarTask @TaskRef = {taskRef}, @Flag = {flag}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GetTasks));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("InsertTask")]
        public async Task<IActionResult> InsertTask([FromBody] KowsarTaskDto dto)
        {
            try
            {
                string taskRef = !string.IsNullOrEmpty(dto.TaskRef) ? dto.TaskRef : "NULL";
                string title = string.IsNullOrEmpty(dto.Title) ? "" : dto.Title.Replace("'", "''");
                string explain = string.IsNullOrEmpty(dto.Explain) ? "" : dto.Explain.Replace("'", "''");

                string query = $"spWeb_KowsarTaskInsert @TaskRef = {taskRef}, @Title = N'{title}', @Explain = N'{explain}'";

                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(InsertTask));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("UpdateTask")]
        public async Task<IActionResult> UpdateTask([FromBody] KowsarTaskDto dto)
        {
            if (string.IsNullOrEmpty(dto.TaskCode) || !int.TryParse(dto.TaskCode, out int taskCode))
                return BadRequest("TaskCode is required and must be a valid number for update.");

            try
            {
                // جایگزینی ' برای جلوگیری از خطای SQL
                string title = string.IsNullOrEmpty(dto.Title) ? "" : dto.Title.Replace("'", "''");
                string explain = string.IsNullOrEmpty(dto.Explain) ? "" : dto.Explain.Replace("'", "''");

                string query = $"spWeb_KowsarTaskUpdate @TaskCode = {taskCode}, @Title = N'{title}', @Explain = N'{explain}'";

                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(UpdateTask));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("DeleteTask")]
        public async Task<IActionResult> DeleteTask([FromBody] KowsarTaskDto kowsarTaskDto)
        {
            if (string.IsNullOrEmpty(kowsarTaskDto.TaskCode) || !int.TryParse(kowsarTaskDto.TaskCode, out int taskCode))
                return BadRequest("TaskCode is required and must be a valid number for delete.");

            try
            {

                string query = $"spWeb_KowsarTaskDelete @TaskCode = {taskCode}";


                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(DeleteTask));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("DeleteTaskAll")]
        public async Task<IActionResult> DeleteTaskAll([FromBody] KowsarTaskDto kowsarTaskDto)
        {
            if (string.IsNullOrEmpty(kowsarTaskDto.TaskCode) || !int.TryParse(kowsarTaskDto.TaskCode, out int taskCode))
                return BadRequest("TaskCode is required and must be a valid number for delete.");

            try
            {

                string query = $"spWeb_KowsarTaskDeleteRecursive @TaskCode = {taskCode}";


                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(DeleteTaskAll));
                return StatusCode(500, "Internal server error.");
            }
        }

        public class KowsarTaskDependencyOrderDto
        {
            public string DependencyCode { get; set; } = "0";
            public string DependencyOrder { get; set; } = "0";
        }


















































        public class GoodTaskOrderDto
        {
            public string GoodTaskCode { get; set; } = "";
            public string GoodTaskOrder { get; set; } = "";
        }



        [HttpGet]
        [Route("GoodTask_Add")]
        public async Task<IActionResult> GoodTask_Add(string GoodRef, string TaskRef, string GoodTaskOrder = "0")
        {
            string query = $"spweb_GoodTask_AddNew @GoodRef ={GoodRef}, @TaskRef ={TaskRef}, @GoodTaskOrder ={GoodTaskOrder}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodTask_Add));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("GoodTask_UpdateOrder")]
        public async Task<IActionResult> GoodTask_UpdateOrder(string GoodTaskCode, string GoodTaskOrder)
        {
            string query = $"spWeb_GoodTask_UpdateOrder @GoodTaskCode ={GoodTaskCode}, @GoodTaskOrder ={GoodTaskOrder}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodTask_UpdateOrder));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("GoodTask_SaveOrder")]
        public async Task<IActionResult> GoodTask_SaveOrder([FromBody] List<GoodTaskOrderDto> rows)
        {
            try
            {
                string jsonInput = JsonSerializer.Serialize(rows ?? new List<GoodTaskOrderDto>()).Replace("'", "''");
                string query = $"spWeb_GoodTask_SaveOrder @Json = N'{jsonInput}'";

                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GoodTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodTask_SaveOrder));
                return StatusCode(500, "Internal server error.");
            }
        }


        // =====================================================================
        // این متدهای Dependency را داخل InternalTaskController جایگزین/اضافه کن
        // =====================================================================

        [HttpGet]
        [Route("KowsarTaskDependency_Get")]
        public async Task<IActionResult> KowsarTaskDependency_Get(string TaskCode)
        {
            string query = $"spWeb_KowsarTaskDependency_Get {TaskCode} ";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(KowsarTaskDependency_Get));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("KowsarTaskDependency_Delete")]
        public async Task<IActionResult> KowsarTaskDependency_Delete(string DependencyCode)
        {
            string query = $"spWeb_KowsarTaskDependency_Delete {DependencyCode} ";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(KowsarTaskDependency_Delete));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("KowsarTaskDependency_Save")]
        public async Task<IActionResult> KowsarTaskDependency_Save(string TaskRef, string DependencyTaskRef, string DependencyOrder = "0")
        {
            string query = $"spWeb_KowsarTaskDependency_Save @TaskRef ={TaskRef}, @DependencyTaskRef ={DependencyTaskRef}, @DependencyOrder ={DependencyOrder} ";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(KowsarTaskDependency_Save));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("KowsarTaskDependency_UpdateOrder")]
        public async Task<IActionResult> KowsarTaskDependency_UpdateOrder(string DependencyCode, string DependencyOrder)
        {
            string query = $"spWeb_KowsarTaskDependency_UpdateOrder @DependencyCode ={DependencyCode}, @DependencyOrder ={DependencyOrder}";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(KowsarTaskDependency_UpdateOrder));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("KowsarTaskDependency_SaveOrder")]
        public async Task<IActionResult> KowsarTaskDependency_SaveOrder([FromBody] List<KowsarTaskDependencyOrderDto> rows)
        {
            try
            {
                string jsonInput = JsonSerializer.Serialize(rows ?? new List<KowsarTaskDependencyOrderDto>()).Replace("'", "''");
                string query = $"spWeb_KowsarTaskDependency_SaveOrder @Json = N'{jsonInput}'";

                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(KowsarTaskDependency_SaveOrder));
                return StatusCode(500, "Internal server error.");
            }
        }




    }
}
