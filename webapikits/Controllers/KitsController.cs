using System.Data;
using Microsoft.AspNetCore.Mvc;
using webapikits.Model;
using System.Text;

namespace webapikits.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class KitsController : ControllerBase
    {
        //public readonly IConfiguration _configuration;
        //DataBaseClass db;
        //DataTable DataTable = new DataTable();
        //string Query = "";
        //Response response = new();
        //JsonClass jsonClass = new JsonClass();
        //Dictionary<string, string> jsonDict = new Dictionary<string, string>();


        //public KitsController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    db = new DataBaseClass(_configuration);

        //}



        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public KitsController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<SupportNewController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }




        [HttpPost]
    [Route("SendSms")]
    public async Task<string> SendSms(string RandomCode, string NumberPhone)
    {

            string sms_api_key = _configuration.GetConnectionString("sms_api_key");

            HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("x-api-key", "me8CfaoTR0rLZEpRWQqdvtCnzcsRwpPtVz9mmwYdbWv5kBEjtSJKZG3wMYCvEndd");
            httpClient.DefaultRequestHeaders.Add("x-api-key", sms_api_key);

            var payload = @"{" + "\n" +
            @"  ""mobile"": """+ NumberPhone + @"""," + "\n" +
            @"  ""templateId"": 100000," + "\n" +
            @"  ""parameters"": [" + "\n" +
            @"    {" + "\n" +
            @"      ""name"": ""CODE""," + "\n" +
            @"      ""value"": """+ RandomCode + @"""" + "\n" +
            @"    }" + "\n" +
            @"  ]" + "\n" +
            @"}";
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://api.sms.ir/v1/send/verify", content);
            var result = await response.Content.ReadAsStringAsync();

            return result; // You may want to return an error message or handle this differently.
        }


        [HttpGet]
        [Route("kowsar_info")]
        public async Task<IActionResult> kowsar_info(string Where)
        {

            string query = $"select top 1 DataValue from dbsetup where KeyValue = '{Where}'";

            //DataTable dataTable = db.Kits_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");



            try
            {
                DataTable dataTable = await db.Kits_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(kowsar_info));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("KowsarQuery")]
        public async Task<IActionResult> KowsarQuery(string str)
        {
            string query = str;
            //DataTable dataTable = db.Kits_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Data", "");
            try
            {
                DataTable dataTable = await db.Kits_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Data", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(KowsarQuery));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("Activation")]
        public async Task<IActionResult> Activation(string ActivationCode, string? Flag)
        {
            

            string query = $"spApp_GetActivation '{ActivationCode}' ";



            if (!string.IsNullOrEmpty(Flag))
            {
                query += $" , '{Flag}' ";
            }

            //DataTable dataTable = db.Kits_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Activations", "");
            try
            {
                DataTable dataTable = await db.Kits_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Activations", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Activation));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetDb")]
        public async Task<IActionResult> GetDbAsync(string Code)
        {

            
            string query = $"select * from AppActivation Where ActivationCode = '{Code}'";

            DataTable dataTable = await db.Kits_ExecQuery(HttpContext, query);

            string filePath = dataTable.Rows[0]["SQLiteURL"] + "";

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                Console.WriteLine("5");
                Console.WriteLine("File not found");
                return NotFound("File not found");
            }

            Console.WriteLine("6");
            // خواندن فایل به عنوان آرایه بایت
            byte[] fileBytes = System.IO.File.ReadAllBytes(@filePath);

            // تعیین نوع محتوای فایل (مثلاً برای PDF)
            string contentType = "application/x-sqlite3"; // یا هر نوع مورد نظر
            Console.WriteLine("7");
            // ارسال فایل به مشتری
            return File(fileBytes, contentType, Path.GetFileName(filePath));

        }

        [HttpGet]
        [Route("OcrKowsar")]
        public IActionResult OcrKowsar()
        {
            
            string filePath = _configuration.GetConnectionString("Ocr_path");

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return NotFound("File not found");
            }
                byte[] fileBytes = System.IO.File.ReadAllBytes(@filePath);
                        string contentType = "application/apk"; // یا هر نوع مورد نظر
            return File(fileBytes, contentType, Path.GetFileName(filePath));

        }
        [HttpGet]
        [Route("BrokerKowsar")]
        public IActionResult BrokerKowsar()
        {

            string filePath = _configuration.GetConnectionString("Broker_path");

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return NotFound("File not found");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(@filePath);
            string contentType = "application/apk"; // یا هر نوع مورد نظر
            return File(fileBytes, contentType, Path.GetFileName(filePath));

        }

        [HttpGet]
        [Route("OrderKowsar")]
        public IActionResult OrderKowsar()
        {

            string filePath = _configuration.GetConnectionString("Order_path");

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return NotFound("File not found");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(@filePath);
            string contentType = "application/apk"; // یا هر نوع مورد نظر
            return File(fileBytes, contentType, Path.GetFileName(filePath));

        }

        [HttpGet]
        [Route("KowsarCompany")]
        public IActionResult KowsarCompany()
        {
            
            string filePath = _configuration.GetConnectionString("KowsarCompany_path");

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return NotFound("File not found");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(@filePath);
            string contentType = "application/apk"; // یا هر نوع مورد نظر
            return File(fileBytes, contentType, Path.GetFileName(filePath));

        }


        [HttpGet]
        [Route("setup")]
        public IActionResult setup()
        {

            string filePath = _configuration.GetConnectionString("setup_path");

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return NotFound("File not found");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(@filePath);
            string contentType = "application/rar"; // یا هر نوع مورد نظر
            return File(fileBytes, contentType, Path.GetFileName(filePath));

        }





        [HttpGet]
        [Route("ErrorLog")]
        public async Task<IActionResult> ErrorLog(
            string ErrorLog,
            string Broker,
            string DeviceId,
            string ServerName,
            string VersionName,
            string StrDate

            )
        {

            string query = $" Insert into ErrorLogReport([ErrorLogText], [Broker], [DeviceId], [ServerName], [VersionName], [StrDate])values ('{ErrorLog}','{Broker}','{DeviceId}','{ServerName}','{VersionName}','{StrDate}')";



            //DataTable dataTable = db.Kits_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResult_Str(dataTable, "Text", "done");

            try
            {
                DataTable dataTable = await db.Kits_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "done");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ErrorLog));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpPost]
        [Route("LogReport")]
        public async Task<IActionResult> LogReport([FromBody] LogReportDto logReportDto)
        {

            string query = $"exec spApp_LogInsert '{logReportDto.Device_Id}','{logReportDto.Address_Ip}','{logReportDto.Server_Name}','{logReportDto.Factor_Code}','{logReportDto.StrDate}','{logReportDto.Broker}','{logReportDto.Explain}','{logReportDto.DeviceAgant}','{logReportDto.SdkVersion}','{logReportDto.DeviceIp}'";
            //DataTable dataTable = db.Kits_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResult_Str(dataTable, "Text", "done");
            try
            {
                DataTable dataTable = await db.Kits_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "done");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LogReport));
                return StatusCode(500, "Internal server error.");
            }



        }


    }
}
