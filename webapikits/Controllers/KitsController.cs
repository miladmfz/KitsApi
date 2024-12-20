using System.Data;
using Microsoft.AspNetCore.Mvc;
using webapikits.Model;
using Newtonsoft.Json;
using System.Text;
using SmsIrRestful;
using IPE.SmsIrClient.Models.Requests;
using IPE.SmsIrClient;
using FastReport;

namespace webapikits.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class KitsController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();


        public KitsController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

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
        public string kowsar_info(string Where)
        {

            string query = $"select top 1 DataValue from dbsetup where KeyValue = '{Where}'";



           
            DataTable dataTable = db.Kits_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");


        }
        



        [HttpGet]
        [Route("KowsarQuery")]
        public string KowsarQuery(string str)
        {
            string query = str;
            DataTable dataTable = db.Kits_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Data", "");
        }

        


        [HttpGet]
        [Route("Activation")]
        public string Activation(string ActivationCode, string Flag)
        {
            
<<<<<<< HEAD
            string query = $"select * from AppBrokerCustomer Where ActivationCode = '{ActivationCode}'";
=======
            string query = $"spApp_GetActivation '{ActivationCode}' , '{Flag}'";
>>>>>>> 78da553c4d505feaec0ce50022ee7d86901c6722

            DataTable dataTable = db.Kits_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Activations", "");

        }


        [HttpGet]
        [Route("GetDb")]
        public IActionResult GetDb(string Code)
        {

            
            string query = $"select * from AppBrokerCustomer Where ActivationCode = '{Code}'";

            DataTable dataTable = db.Kits_ExecQuery(HttpContext, query);

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
        public string ErrorLog(
            string ErrorLog,
            string Broker,
            string DeviceId,
            string ServerName,
            string VersionName,
            string StrDate

            )
        {

            string query = $" Insert into ErrorLogReport([ErrorLogText], [Broker], [DeviceId], [ServerName], [VersionName], [StrDate])values ('{ErrorLog}','{Broker}','{DeviceId}','{ServerName}','{VersionName}','{StrDate}')";



            DataTable dataTable = db.Kits_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Text", "done");

        }


        [HttpPost]
        [Route("LogReport")]
        public string LogReport([FromBody] LogReportDto logReportDto)
        {

            string query = $"exec spApp_appinfo '{logReportDto.Device_Id}','{logReportDto.Address_Ip}','{logReportDto.Server_Name}','{logReportDto.Factor_Code}','{logReportDto.StrDate}','{logReportDto.Broker}','{logReportDto.Explain}','{logReportDto.DeviceAgant}','{logReportDto.SdkVersion}','{logReportDto.DeviceIp}'";
            DataTable dataTable = db.Kits_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "done");


        }










    }
}
