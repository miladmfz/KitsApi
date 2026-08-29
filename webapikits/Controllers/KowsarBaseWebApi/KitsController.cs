using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using webapikits.Controllers.InternalWebApi;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
using webapikits.Model;

namespace webapikits.Controllers.KowsarBaseWebApi
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
        private readonly ILogger<KitsController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public KitsController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<KitsController> logger,
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

            string sms_api_key = _configuration.GetValue<string>("AppSettings:sms_api_key");

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

             
            try
            {
                DataTable dataTable = await db.Kits_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str_AppActivation(dataTable, "Activations", "");
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




        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet]
        [Route("GetRssBySource")]
        public async Task<IActionResult> GetRssBySource(string name)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("name is required");

            var rssSources = _configuration
                .GetSection("AppSettings:RssSources")
                .Get<Dictionary<string, RssSource>>();

            if (rssSources == null || !rssSources.Any())
                return StatusCode(500, "RSS sources not configured");

            var source = rssSources
                .FirstOrDefault(x => x.Key.Equals(name, StringComparison.OrdinalIgnoreCase))
                .Value;

            if (source == null || string.IsNullOrWhiteSpace(source.Url))
                return NotFound("RSS source not found");

            try
            {
                using var handler = new HttpClientHandler();

                handler.UseCookies = false;

                using var client = new HttpClient(handler);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.ConnectionClose = true;

                client.DefaultRequestHeaders.TryAddWithoutValidation(
                    "User-Agent",
                    "Mozilla/5.0"
                );

                client.DefaultRequestHeaders.TryAddWithoutValidation(
                    "Accept",
                    "*/*"
                );

                client.DefaultRequestHeaders.TryAddWithoutValidation(
                    "Cache-Control",
                    "no-cache, no-store"
                );

                client.DefaultRequestHeaders.TryAddWithoutValidation(
                    "Pragma",
                    "no-cache"
                );

                client.Timeout = TimeSpan.FromSeconds(20);

                var requestUrl =
                    $"{source.Url}?t={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

                var xml = await client.GetStringAsync(requestUrl);

                var doc = XDocument.Parse(xml);

                XNamespace media = "http://search.yahoo.com/mrss/";
                XNamespace dc = "http://purl.org/dc/elements/1.1/";

                var items = doc.Descendants("item")
                    .Select(x =>
                    {
                        var title = x.Element("title")?.Value;
                        var link = x.Element("link")?.Value;
                        var description = x.Element("description")?.Value;

                        return new RssItemDto
                        {
                            Title = CleanText(title),
                            Link = link,
                            Description = CleanText(description),
                            PubDate = NormalizeDate(
                                x.Element("pubDate")?.Value
                            ),
                            Author =
                                x.Element(dc + "creator")?.Value ??
                                x.Element("author")?.Value,
                            Category = source.Category,
                            Image =
                                x.Element(media + "content")?.Attribute("url")?.Value ??
                                x.Element(media + "thumbnail")?.Attribute("url")?.Value,
                            Source = name
                        };
                    })
                    .Where(x => !string.IsNullOrWhiteSpace(x.Title))
                    .OrderByDescending(x => SafeDate(x.PubDate))
                    .Take(50)
                    .ToList();

                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRssBySource: {SourceName}", name);

                return StatusCode(500, new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }
        private static string CleanText(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var text = WebUtility.HtmlDecode(input);

            text = Regex.Replace(text, "<.*?>", " ");
            text = Regex.Replace(text, @"\s+", " ");

            return text.Trim();
        }


        private static string NormalizeDate(string? date)
        {
            if (DateTime.TryParse(date, out var parsedDate))
                return parsedDate.ToString("O");

            return DateTime.UtcNow.ToString("O");
        }

        private static DateTime SafeDate(string? date)
        {
            if (DateTime.TryParse(date, out var parsedDate))
                return parsedDate;

            return DateTime.MinValue;
        }




    }
}
