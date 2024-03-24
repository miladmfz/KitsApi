using System.Data;
using Microsoft.AspNetCore.Mvc;
using webapikits.Model;
using Newtonsoft.Json;
using System.Text;
using SmsIrRestful;

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



        public class PrintRequest
        {
            public string Image { get; set; }
            public string Code { get; set; }
            public string PrinterName { get; set; }
            public int PrintCount { get; set; }
        }


        public class ApiCredentials
    {
        public string UserApiKey { get; set; }
        public string SecretKey { get; set; }
    }

    ApiCredentials credentials = new ApiCredentials
    {
        UserApiKey = "ce2ee6d2a00e4451f540e6d2",
        SecretKey = "Kowsar321@!"
    };

    


    [HttpPost]
    [Route("SendSms")]
    public async Task<string> SendSms(string RandomCode, string NumberPhone)
    {
            string query = " "+ RandomCode + " " + NumberPhone;
            Logger logger = new Logger();
            logger.LogFile(_configuration, "LOG", query); ;
            try
        {
            string url = "https://RestfulSms.com/api/Token";
            var credentials = new { UserApiKey = "ce2ee6d2a00e4451f540e6d2", SecretKey = "Kowsar321@!" };
            string json = JsonConvert.SerializeObject(credentials);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                        var resultJson = await response.Content.ReadAsStringAsync();
                        var resultObject = JsonConvert.DeserializeObject<TokenResultObject>(resultJson);

                        if (resultObject.IsSuccessful)
                        {
                            VerificationCode(RandomCode, NumberPhone, resultObject.TokenKey);
                            DataTable tb = new();
                            return jsonClass.JsonResult_Str(tb, "Text", "done");
                        }
                        else
                        {
                            Console.WriteLine("API Request was successful, but Token not available.");
                        }
                    }
                else
                {
                    Console.WriteLine("Request failed with status code: " + response.StatusCode);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        return ""; // You may want to return an error message or handle this differently.
    }




    [HttpPost]
    [Route("VerificationCode")]
    public async Task<string> VerificationCode(string RandomCode, string NumberPhone, string Token_Str)
    {
        try
        {
            string url = "https://RestfulSms.com/api/VerificationCode";

            // Define the request body
            var requestBody = new
            {
                Code = RandomCode,
                MobileNumber = NumberPhone
            };

            string json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Define the headers
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-sms-ir-secure-token", Token_Str);

                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var resultJson = await response.Content.ReadAsStringAsync();

                    return resultJson;
                }
                else
                {
                    Console.WriteLine("API Request failed with status code: " + response.StatusCode);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        return "Error"; // You may want to return an error message or handle this differently.
    }



    [HttpGet]
        [Route("kowsar_info")]
        public string kowsar_info(string Where)
        {

            string query = "select top 1 DataValue from dbsetup where KeyValue = '" + Where + "'";



           
            DataTable dataTable = db.Kits_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");


        }
        



        [HttpGet]
        [Route("KowsarQuery")]
        public string KowsarQuery(string str)
        {
            string query = str;
            DataTable dataTable = db.Kits_ExecQuery(Request.Path, query);
            return jsonClass.JsonResult_Str(dataTable, "Data", "");
        }

        


        [HttpGet]
        [Route("Activation")]
        public string Activation(string ActivationCode)
        {
            
            string query = "select * from AppBrokerCustomer Where ActivationCode = '" + ActivationCode + "'";

            DataTable dataTable = db.Kits_ExecQuery(Request.Path, query);
            return jsonClass.JsonResult_Str(dataTable, "Activations", "");

        }


        [HttpGet]
        [Route("GetDb")]
        public IActionResult GetDb(string Code)
        {

            
            string query = "select * from AppBrokerCustomer Where ActivationCode = '" + Code + "'";

            DataTable dataTable = db.Kits_ExecQuery(Request.Path, query);

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

            string query = " Insert into ErrorLogReport([ErrorLogText], [Broker], [DeviceId], [ServerName], [VersionName], [StrDate])values ('"+ ErrorLog + "','"+Broker + "','"+ DeviceId + "','"+ ServerName + "','"+ VersionName + "','"+ StrDate + "')";



            DataTable dataTable = db.Kits_ExecQuery(Request.Path, query);
            return jsonClass.JsonResult_Str(dataTable, "Text", "done");

        }



        
        
        [HttpGet]
        [Route("LogReport")]
        public string LogReport(
            string Device_Id,
            string Address_Ip,
            string Server_Name,
            string Factor_Code,
            string StrDate,
            string Broker,
            string Explain
            )
        {

            string query = "exec spApp_appinfo '"+ Device_Id + "','"+ Address_Ip + "','"+ Server_Name + "','"+ Factor_Code + "','"+ StrDate + "','"+ Broker + "','"+ Explain + "'";



            DataTable dataTable = db.Kits_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "done");


        }










    }
}
