using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
namespace webapikits.Controllers.TanzimatWebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutLetterController : ControllerBase
    {
        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<AutLetterController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public AutLetterController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<AutLetterController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("SendSmsAutLetter")]
        public async Task<string> SendSmsAutLetter([FromBody] PersonInfoDto personInfoDto)
        {

            string sms_api_key = _configuration.GetValue<string>("AppSettings:sms_api_key");

            HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("x-api-key", "me8CfaoTR0rLZEpRWQqdvtCnzcsRwpPtVz9mmwYdbWv5kBEjtSJKZG3wMYCvEndd");
            httpClient.DefaultRequestHeaders.Add("x-api-key", sms_api_key);

            var payload = @"{" + "\n" +
            @"  ""mobile"": """ + personInfoDto.NumberPhone + @"""," + "\n" +
            @"  ""templateId"": 959191," + "\n" +
            @"  ""parameters"": [" + "\n" +
            @"    {" + "\n" +
            @"      ""name"": ""CONTACTS""," + "\n" +
            @"      ""value"": """ + personInfoDto.CONTACTS + @"""" + "\n" +
            @"    }" + "\n" +
            @"  ]" + "\n" +
            @"}";
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://api.sms.ir/v1/send/verify", content);
            var result = await response.Content.ReadAsStringAsync();



            return result;
        }



        [HttpPost]
        [Route("GetAutLetterListForUser")]
        public async Task<IActionResult> GetAutLetterListForUser([FromBody] SearchTargetLetterDto searchTargetLetterDto)
        {
            string Where = "";

            if (!string.IsNullOrEmpty(searchTargetLetterDto.CentralRef))
            {
                string centralFilter =
                $@"
        (f.CreatorCentralRef = {searchTargetLetterDto.CentralRef}  OR f.OwnerCentralRef = {searchTargetLetterDto.CentralRef}   OR r.ExecutorCentralRef = {searchTargetLetterDto.CentralRef} )";

                if (!string.IsNullOrEmpty(Where))
                {
                    Where += $" AND {centralFilter}";
                }
                else
                {
                    Where = centralFilter;
                }
            }


            if (!string.IsNullOrEmpty(searchTargetLetterDto.StartTime))
            {
                string whereTime = $@" (  f.LetterDate BETWEEN ''{searchTargetLetterDto.StartTime}''  AND ''{searchTargetLetterDto.EndTime}'' )";

                if (!string.IsNullOrEmpty(Where))
                {
                    Where += $" AND {whereTime}";
                }
                else
                {
                    Where = whereTime;
                }
            }


            string query =
                $"Exec spWeb_AutLetterList_User '{Where}',{searchTargetLetterDto.OwnCentralRef},'{searchTargetLetterDto.SearchTarget}'";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);

                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAutLetterListForUser));

                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("GetAutLetterListForCustomer")]
        public async Task<IActionResult> GetAutLetterListForCustomer([FromBody] SearchTargetLetterDto searchTargetLetterDto)
        {
            string Where = "";

            if (!string.IsNullOrEmpty(searchTargetLetterDto.CentralRef))
            {
                string centralFilter =
                $@"
        (f.CreatorCentralRef = {searchTargetLetterDto.CentralRef}  OR f.OwnerCentralRef = {searchTargetLetterDto.CentralRef}  )";

                if (!string.IsNullOrEmpty(Where))
                {
                    Where += $" AND {centralFilter}";
                }
                else
                {
                    Where = centralFilter;
                }
            }


            if (!string.IsNullOrEmpty(searchTargetLetterDto.OwnerPersonInfoRef))
            {
                string privateFilter =
                $@"  (  f.IsPrivate = 0  OR  (  f.IsPrivate = 1 AND f.OwnerPersonInfoRef = {searchTargetLetterDto.OwnerPersonInfoRef} ) )";

                if (!string.IsNullOrEmpty(Where))
                {
                    Where += $" AND {privateFilter}";
                }
                else
                {
                    Where = privateFilter;
                }
            }


            if (!string.IsNullOrEmpty(searchTargetLetterDto.StartTime))
            {
                string whereTime = $@" (  f.LetterDate BETWEEN ''{searchTargetLetterDto.StartTime}''  AND ''{searchTargetLetterDto.EndTime}'' )";

                if (!string.IsNullOrEmpty(Where))
                {
                    Where += $" AND {whereTime}";
                }
                else
                {
                    Where = whereTime;
                }
            }


            string query =
                $"Exec spWeb_AutLetterList_PersonInfo '{Where}',{searchTargetLetterDto.OwnCentralRef},'{searchTargetLetterDto.SearchTarget}'";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);

                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAutLetterListForCustomer));

                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GetAutLetterListByCentral")]
        public async Task<IActionResult> GetAutLetterListByCentral([FromBody] SearchTargetLetterDto searchTargetLetterDto)
        {

            string Where = "";

            if (!string.IsNullOrEmpty(searchTargetLetterDto.SearchTarget))
            {
                Where = $"(LetterTitle like ''%{searchTargetLetterDto.SearchTarget}%'' or LetterDescription like ''%{searchTargetLetterDto.SearchTarget}%'' or ds.RowExecutorName like ''%{searchTargetLetterDto.SearchTarget}%'')";
            }

            if (!string.IsNullOrEmpty(searchTargetLetterDto.StartTime))
            {
                string Where_Time = $" (( HeaderLetterDate between ''{searchTargetLetterDto.StartTime}'' And ''{searchTargetLetterDto.EndTime}'') Or ( HeaderLetterDate between ''{searchTargetLetterDto.StartTime}'' And ''{searchTargetLetterDto.EndTime}'')) ";
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += $" And " + Where_Time;
                }
                else
                {
                    Where = Where_Time;
                }
            }


            string query = $"spWeb_AutLetterListByCentral '{Where}','{searchTargetLetterDto.CentralRef}' ,'{searchTargetLetterDto.Flag}'";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAutLetterListByCentral));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpPost("GetKowsarCustomer")]
        public async Task<IActionResult> GetKowsarCustomer([FromBody] SearchTargetDto searchTargetDto)
        {

            string query = $"Exec [dbo].[spWeb_GetCustomer] '{searchTargetDto.SearchTarget}',{searchTargetDto.Active} ,{searchTargetDto.BrokerRef}";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetKowsarCustomer));
                return StatusCode(500, "Internal server error.");
            }
        }









        [HttpPost]
        [Route("LetterInsert")]
        public async Task<IActionResult> LetterInsert([FromBody] LetterInsert letterInsert)
        {
            string CreatorCentral = _configuration.GetValue<string>("AppSettings:Support_CreatorCentral");
            string ownerPersonInfoRef =
    string.IsNullOrWhiteSpace(letterInsert.OwnerPersonInfoRef?.ToString())
        ? "NULL"
        : letterInsert.OwnerPersonInfoRef.ToString();

            string query = $"exec dbo.spAutLetter_Insert @LetterDate='{letterInsert.LetterDate}', @InOutFlag={letterInsert.InOutFlag},@Title ='{letterInsert.title}', " +
                $"@Description='{letterInsert.Description}',@State ='{letterInsert.LetterState}',@Priority ='{letterInsert.LetterPriority}', @ReceiveType =N'دستی', " +
                $"@CreatorCentral ={letterInsert.CreatorCentral}, @OwnerCentral ={letterInsert.OwnerCentral}, @OwnerPersonInfoRef ={ownerPersonInfoRef} , @IsPrivate ={letterInsert.IsPrivate} ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LetterInsert));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("GetLetterRowList")]
        public async Task<IActionResult> GetLetterRowList(string LetterRef)
        {

            string query = $"select  LetterRowCode,CreatorCentralRef,AutLetterRow_PropDescription1,g.Name RowCreatorName,c.Name RowExecutorName,LetterRef ,LetterDate RowLetterDate, LetterDescription LetterRowDescription, LetterState LetterRowState, ExecutorCentralRef RowExecutorCentralRef  from vwautletterrow join central c on c.CentralCode=ExecutorCentralRef  join central g on g.CentralCode=CreatorCentralRef  where LetterRef = {LetterRef} order by LetterRowCode desc ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLetterRowList));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpGet]
        [Route("AutLetterUpdate")]
        public async Task<IActionResult> AutLetterUpdate(string LetterCode, string State)
        {

            string query = $"Update AutLetter set letterstate = N'{State}' Where LetterCode = {LetterCode} ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(AutLetterUpdate));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("GetCentralUser")]
        public async Task<IActionResult> GetCentralUser()
        {

            string query = $"select CentralCode,CentralName from vwCentralUser ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);

                string json = jsonClass.JsonResult_Str(dataTable, "Centrals", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentralUser));
                return StatusCode(500, "Internal server error.");
            }


        }





        [HttpPost]
        [Route("AutLetterRowInsert")]
        public async Task<IActionResult> AutLetterRowInsert([FromBody] AutLetterRowInsert autLetterRowInsert)
        {

            string query = $"spAutLetterRow_Insert @LetterRef = {autLetterRowInsert.LetterRef}, @LetterDate = '{autLetterRowInsert.LetterDate}'" +
                $", @Description = '{autLetterRowInsert.Description}', @State = '{autLetterRowInsert.LetterState}', @Priority = '{autLetterRowInsert.LetterPriority}'" +
                $", @CreatorCentral = {autLetterRowInsert.CreatorCentral}, @ExecuterCentral = {autLetterRowInsert.ExecuterCentral}";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetterRows", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(AutLetterRowInsert));
                return StatusCode(500, "Internal server error.");
            }




        }






        [HttpGet]
        [Route("GetAutConversation")]
        public async Task<IActionResult> GetAutConversation(
   string LetterRef
    )
        {

            string query = $"Exec spWeb_GetAutConversation  {LetterRef}";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Conversations", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAutConversation));
                return StatusCode(500, "Internal server error.");
            }



        }






        [HttpGet]
        [Route("ConversationSeen")]
        public async Task<IActionResult> ConversationSeen(string CentralRef, string LetterRef)
        {

            string query = $"Exec spWeb_ConversationSeen  {CentralRef},{LetterRef}";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Conversations", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ConversationSeen));
                return StatusCode(500, "Internal server error.");
            }



        }





        [HttpGet]
        [Route("GetAutletterById")]
        public async Task<IActionResult> GetAutletterById(string LetterCode)
        {

            string query = $"Exec spWeb_GetAutLetterById {LetterCode}";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAutletterById));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpPost]
        [Route("SetAlarmOff")]
        public async Task<IActionResult> SetAlarmOff([FromBody] AlarmOffDto alarmOffDto)
        {

            string query = $"spWeb_SetAlarmOff {alarmOffDto.LetterRef},{alarmOffDto.CentralRef}";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SetAlarmOff));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpPost]
        [Route("Update_AutletterRow")]
        public async Task<IActionResult> Update_AutletterRow([FromBody] AutLetterRowInsert letterRowdto)
        {
            string query2 = "";
            string query3 = "";

            if (!string.IsNullOrEmpty(letterRowdto.AutLetterRow_PropDescription1))
            {
                query2 = $" spPropertyValue 'TAutLetterRow' , {letterRowdto.ObjectRef} ";
                DataTable dataTable2 = await db.Kowsar_ExecQuery(HttpContext, query2);


                query3 = $"Update PropertyValue Set Nvarchar1 = '{letterRowdto.AutLetterRow_PropDescription1}' Where ObjectRef = {letterRowdto.ObjectRef}  And ClassName ='TAutLetterRow'";
                DataTable dataTable3 = await db.Kowsar_ExecQuery(HttpContext, query3);
            }



            string query = $" Update AutLetterRow Set LetterState = '{letterRowdto.LetterRowState}' , LetterDescription = '{letterRowdto.LetterRowDescription}' , AlarmActive = 0 , ReformDate = GetDate() Where LetterRowCode = {letterRowdto.ObjectRef}";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Update_AutletterRow));
                return StatusCode(500, "Internal server error.");
            }

        }







        [HttpPost]
        [Route("Conversation_Insert")]
        public async Task<IActionResult> Conversation_Insert([FromBody] LetterDto letterdto)
        {

            string query = $"Exec spWeb_AutLetterConversation_Insert @LetterRef={letterdto.LetterRef}, @CentralRef={letterdto.CentralRef}, @ConversationText='{letterdto.ConversationText}', @ClassName='Text'";




            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Conversation_Insert));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpPost]
        [Route("Conversation_UploadFile")]
        public async Task<IActionResult> Conversation_UploadFile([FromBody] ksrImageModel data)
        {


            string query2 = $"Exec spWeb_AutLetterConversation_Insert @LetterRef={data.LetterRef}, @CentralRef={data.CentralRef}, @ConversationText='{data.Title}', @ClassName='File'";

            DataTable dataTable2 = await db.Kowsar_ExecQuery(HttpContext, query2);
            string Conversationref = dataTable2.Rows[0]["ConversationCode"] + "";
            byte[] decodedImage = Convert.FromBase64String(data.File);


            string data_base64 = data.File;
            byte[] data_Bytes = Convert.FromBase64String(data_base64);



            string dataName = $"{data.FileName}.{data.FileType}"; // Constructing the image name
            string dataPath = _configuration.GetValue<string>("AppSettings:Ocr_imagePath") + $"{dataName}"; // Provide the path where you want to save the image


            System.IO.File.WriteAllBytes(dataPath, data_Bytes);



            string connectionString = _configuration.GetConnectionString("Support_Connection"); // Provide your SQL Server connection string



            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {

                dbConnection.Open();
                string dbname = "";
                string query1 = "";
                if (data.ClassName == "Aut")
                {
                    query1 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate" +
                    $"" +
                    $"" +
                    $"" +
                    $", '/', '')   From FiscalPeriod p Join AutLetter aut on PeriodId=PeriodRef Where LetterCode= {data.LetterRef}  Select @db dbname";

                }
                else if (data.ClassName == "Factor")
                {

                    query1 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join Factor f on PeriodId=PeriodRef Where FactorCode= {data.ObjectRef}  Select @db dbname";

                }
                else
                {
                    query1 = $"Declare @dbname nvarchar(200)=db_name()+'Ocr' select  @dbname dbname";


                }


                DataTable dataTable1 = await db.Kowsar_ExecQuery(HttpContext, query1);
                dbname = dataTable1.Rows[0]["dbname"] + "";

                string sqlCommandText = @" INSERT INTO " + dbname + @".dbo.AttachedFiles (Title, ClassName, ObjectRef, FileName, SourceFile, Type, Owner, CreationDate, Reformer, ReformDate,FilePath)  VALUES (@Title, @ClassName, @ObjectRef, @FileName, @SourceFile, @Type, -1000, GETDATE(), -1000, GETDATE(),@FilePath)  ";


                using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, dbConnection))
                {


                    sqlCommand.Parameters.AddWithValue("@Title", data.Title);
                    sqlCommand.Parameters.AddWithValue("@FileName", dataName);
                    sqlCommand.Parameters.AddWithValue("@ObjectRef", Conversationref);
                    sqlCommand.Parameters.AddWithValue("@ClassName", data.ClassName);
                    sqlCommand.Parameters.AddWithValue("@Type", data.FileType);
                    sqlCommand.Parameters.AddWithValue("@FilePath", dataPath);
                    sqlCommand.Parameters.AddWithValue("@SourceFile", System.IO.File.ReadAllBytes(dataPath));


                    sqlCommand.ExecuteNonQuery();
                }


                System.IO.File.Delete(dataPath);

            }


            string query11 = "select dbo.fnDate_Today() TodeyFromServer ";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query11);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Conversation_UploadFile));
                return StatusCode(500, "Internal server error.");
            }





        }





        [HttpPost]
        [Route("GetConversationFileFromAttach")]
        public async Task<IActionResult> GetConversationFileFromAttach([FromBody] ConversationAttachDto dto)
        {
            try
            {
                // ۱. انتخاب دیتابیس بر اساس کلاس
                string query1;
                if (dto.ClassName == "Aut")
                {
                    query1 = $@" Declare @db nvarchar(100)=''   Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')    From FiscalPeriod p  Join AutLetter aut on PeriodId=PeriodRef  Where LetterCode= {dto.ObjectRef}   Select @db dbname";
                }
                else if (dto.ClassName == "Factor")
                {
                    query1 = $@" Declare @db nvarchar(100)=''   Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')    From FiscalPeriod p  Join Factor f on PeriodId=PeriodRef  Where FactorCode= {dto.ObjectRef}   Select @db dbname";
                }
                else
                {
                    query1 = $"Declare @dbname nvarchar(200)=db_name()+'Ocr' select @dbname dbname";
                }

                DataTable dataTable1 = await db.Kowsar_ExecQuery(HttpContext, query1);
                string dbname = dataTable1.Rows[0]["dbname"].ToString() ?? "";

                // ۲. گرفتن فایل
                string query = $@" SELECT * FROM {dbname}.dbo.AttachedFiles  WHERE Classname = '{dto.ClassName}'    AND ObjectRef = {dto.ConversationRef}  ORDER BY 1 DESC";

                DataTable dataTable = await db.Image_ExecQuery(HttpContext, query);

                string? fileName = dataTable.Rows[0]["FileName"]?.ToString() ?? "file.bin";
                string? extension = Path.GetExtension(fileName).ToLower();
                string? contentType = "application/octet-stream";


                switch (extension)
                {
                    case ".jpg":
                    case ".jpeg": contentType = "image/jpeg"; break;
                    case ".png": contentType = "image/png"; break;
                    case ".gif": contentType = "image/gif"; break;

                    case ".mp3": contentType = "audio/mpeg"; break;
                    case ".wav": contentType = "audio/wav"; break;

                    case ".mp4": contentType = "video/mp4"; break;

                    case ".pdf": contentType = "application/pdf"; break;

                    // Word
                    case ".doc": contentType = "application/msword"; break;
                    case ".docx": contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; break;

                    // Excel
                    case ".xls": contentType = "application/vnd.ms-excel"; break;
                    case ".xlsx": contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; break;

                    case ".json": contentType = "application/json"; break;
                    case ".zip": contentType = "application/zip"; break;
                    case ".txt": contentType = "text/plain"; break;
                    case ".csv": contentType = "text/csv"; break;
                }

                string base64 = Convert.ToBase64String((byte[])dataTable.Rows[0]["SourceFile"]);

                var result = new
                {
                    Text = base64,
                    ContentType = contentType,
                    FileName = fileName
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }







        [HttpGet]
        [Route("GetVoiceFileFromAttach")]
        public async Task<string> GetVoiceFileFromAttach(string pixelScale, string ClassName, string ObjectRef)

        {

            string query1 = "";
            string dbname = "";

            if (ClassName == "Aut")
            {
                query1 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join AutLetter aut on PeriodId=PeriodRef Where LetterCode= {ObjectRef}  Select @db dbname";

            }
            else if (ClassName == "Factor")
            {

                query1 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join Factor f on PeriodId=PeriodRef Where FactorCode= {ObjectRef}  Select @db dbname";

            }
            else
            {
                query1 = $"Declare @dbname nvarchar(200)=db_name()+'Ocr' select  @dbname dbname";


            }

            query1 = $"Declare @dbname nvarchar(200)=db_name()+'Ocr' select  @dbname dbname";

            DataTable dataTable1 = await db.Kowsar_ExecQuery(HttpContext, query1);
            dbname = dataTable1.Rows[0]["dbname"] + "";


            string query = $"SELECT * ,SourceFile FROM  " + dbname + $".dbo.AttachedFiles WHERE Classname = '{ClassName}' AND ObjectRef = {ObjectRef} order by 1 desc";
            DataTable dataTable = await db.Image_ExecQuery(HttpContext, query);
            return jsonClass.ConvertVoiceToBase64(dataTable);

        }





        [HttpGet]
        [Route("DeleteAutLetterRows")]
        public async Task<IActionResult> DeleteAutLetterRows(string LetterRowCode)
        {

            string query = $" Delete From  AutLetterRow where LetterRowCode= {LetterRowCode}";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteAutLetterRows));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpGet]
        [Route("DeleteAutLetter")]
        public async Task<IActionResult> DeleteAutLetter(string LetterCode)
        {

            string query = $" Delete From  AutLetter where LetterCode= {LetterCode}";




            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteAutLetter));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpPost("GetCentralByCode")]
        public async Task<IActionResult> GetCentralByCode([FromBody] SearchTargetDto searchTargetDto)
        {

            string query = $"Exec [dbo].[spWeb_GetCentralByCode] {searchTargetDto.ObjectRef}";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Centrals", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentralByCode));
                return StatusCode(500, "Internal server error.");
            }
        }





        [HttpPost]
        [Route("GetFactorByCustomerCode")]
        public async Task<IActionResult> GetFactorByCustomerCode([FromBody] SearchTargetDto searchTargetDto)
        {



            string query = $"Exec [dbo].[spWeb_GetFactorByCustomerCode] '{searchTargetDto.ClassName}',{searchTargetDto.ObjectRef}";

            DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
            string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
            return Content(json, "application/json");


        }





























































    }
}
