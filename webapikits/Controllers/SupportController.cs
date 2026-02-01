using FastReport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using webapikits.Model;
using static System.Data.Entity.Infrastructure.Design.Executor;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace webapikits.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {


        JsonClass jsonClass = new JsonClass();

        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        public SupportController(
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
        [Route("SendSmsAutLetter")]
        public async Task<string> SendSmsAutLetter([FromBody] PersonInfoDto personInfoDto)
        {

            string sms_api_key = _configuration.GetConnectionString("sms_api_key");

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



        [HttpGet]
        [Route("GetTodeyFromServer")]
        public async Task<IActionResult> GetTodeyFromServer()
        {

            string query = "select dbo.fnDate_Today() TodeyFromServer ";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UpdatePersonInfo));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpPost]
        [Route("UpdatePersonInfo")]
        public async Task<IActionResult> UpdatePersonInfo([FromBody] PersonInfoDto personInfoDto)
        {


            string query = $"Exec [dbo].[spWeb_UpdatePersonInfo] {personInfoDto.PersonInfoCode} ,'{personInfoDto.PhFirstName}','{personInfoDto.PhLastName}','{personInfoDto.PhCompanyName}','{personInfoDto.PhAddress1}','{personInfoDto.PhTel1}','{personInfoDto.PhMobile1}','{personInfoDto.PhEmail}'";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UpdatePersonInfo));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("GetKowsarPersonInfo")]
        public async Task<IActionResult> GetKowsarPersonInfo(string PersonInfoCode)
        {

            string query = $"Exec [dbo].[spWeb_GetKowsarPersonInfo] {PersonInfoCode}";

            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetKowsarPersonInfo));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("IsUser")]
        public async Task<IActionResult> IsUser([FromBody] LoginUserDto loginUserDto)
        {


            string query = $"Exec [dbo].[spWeb_IsXUser] '{loginUserDto.UName}','{loginUserDto.UPass}'";

            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(IsUser));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpGet]
        [Route("GetObjectTypeFromDbSetup")]
        public async Task<IActionResult> GetObjectTypeFromDbSetup(string ObjectType)
        {

            string query = $"select * from dbo.fnObjectType('{ObjectType}') ";

            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "ObjectTypes", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetObjectTypeFromDbSetup));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> UploadImage([FromBody] ksrImageModeldto data)
        {

            byte[] decodedImage = Convert.FromBase64String(data.image);

            string data_base64 = data.image;
            byte[] data_Bytes = Convert.FromBase64String(data_base64);

            string dataName = $"{data.ObjectCode}.jpg"; // Constructing the image name
            string dataPath = _configuration.GetConnectionString("Ocr_imagePath") + $"{dataName}"; // Provide the path where you want to save the image

            System.IO.File.WriteAllBytes(dataPath, data_Bytes);

            string connectionString = _configuration.GetConnectionString("Support_ImageConnection"); // Provide your SQL Server connection string

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {

                dbConnection.Open();



                string sqlCommandText = @" INSERT INTO KsrImage (ClassName, ObjectRef, IMG, IsDefaultImage, FileName, Owner, CreationDate, Reformer, ReformDate, rowguid)  VALUES (@ClassName, @ObjectRef,@SourceFile, 0,  @FileName,  -1000, GETDATE(), -1000, GETDATE(), newId())  ";

                // Create a SqlCommand object
                using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, dbConnection))
                {
                    //Bind parameters
                    sqlCommand.Parameters.AddWithValue("@ClassName", data.ClassName);
                    sqlCommand.Parameters.AddWithValue("@ObjectRef", data.ObjectCode);
                    sqlCommand.Parameters.AddWithValue("@FileName", dataName);
                    sqlCommand.Parameters.AddWithValue("@SourceFile", System.IO.File.ReadAllBytes(dataPath));



                    // Execute the command
                    sqlCommand.ExecuteNonQuery();
                }


                System.IO.File.Delete(dataPath);

            }

            string query11 = "select dbo.fnDate_Today() TodeyFromServer ";

            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query11);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(AttachFile_Insert));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpGet]
        [Route("GetCentralById")]
        public async Task<IActionResult> GetCentralById(string CentralCode)
        {


            string query = $"select CentralCode,Title,Name,FName,Manager,Delegacy,CentralName from vwcentral where CentralCode={CentralCode}";

            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Centrals", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentralById));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpPost]
        [Route("GetKowsarCentral")]
        public async Task<IActionResult> GetKowsarCentral([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Exec [dbo].[spWeb_GetKowsarCentral] '{searchTargetDto.SearchTarget}'";

            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Centrals", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetKowsarCentral));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpPost("GetKowsarCustomer")]
        public async Task<IActionResult> GetKowsarCustomer([FromBody] SearchTargetDto searchTargetDto)
        {

            string query = $"Exec [dbo].[spWeb_GetCustomer] '{searchTargetDto.SearchTarget}' ,{searchTargetDto.BrokerRef}";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetKowsarCustomer));
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("GetCustomerById")]
        public async Task<IActionResult> GetCustomerById([FromBody] SearchTargetDto searchTargetDto)
        {

            string query = $"Exec [dbo].[spWeb_GetCustomerById] {searchTargetDto.ObjectRef}";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetKowsarCustomer));
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("GetCentralByCode")]
        public async Task<IActionResult> GetCentralByCode([FromBody] SearchTargetDto searchTargetDto)
        {

            string query = $"Exec [dbo].[spWeb_GetCentralByCode] {searchTargetDto.ObjectRef}";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Centrals", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetKowsarCustomer));
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("GetAutLetterList")]
        public async Task<IActionResult> GetAutLetterList([FromBody] SearchTargetLetterDto searchTargetLetterDto)
        {
            string Where = "";

            if (!string.IsNullOrEmpty(searchTargetLetterDto.CentralRef))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += $" And (CreatorCentralRef={searchTargetLetterDto.CentralRef} or OwnerCentralRef={searchTargetLetterDto.CentralRef} or RowExecutorCentralRef={searchTargetLetterDto.CentralRef})";
                }
                else
                {
                    Where = $"(CreatorCentralRef={searchTargetLetterDto.CentralRef} or OwnerCentralRef={searchTargetLetterDto.CentralRef} or RowExecutorCentralRef={searchTargetLetterDto.CentralRef})";
                }
            }


            if (!string.IsNullOrEmpty(searchTargetLetterDto.PersonInfoCode))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += $" And (OwnerPersonInfoRef={searchTargetLetterDto.PersonInfoCode})";
                }
                else
                {
                    Where = $"(OwnerPersonInfoRef={searchTargetLetterDto.PersonInfoCode})";
                }
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


            string query = $"Exec spWeb_AutLetterList '{Where}',{searchTargetLetterDto.OwnCentralRef},'{searchTargetLetterDto.SearchTarget}'";

            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");


                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAutLetterList));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpPost]
        [Route("GetAutLetterListByPerson")]
        public async Task<IActionResult> GetAutLetterListByPerson([FromBody] SearchTargetLetterDto searchTargetLetterDto)
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


            string query = $"spWeb_AutLetterListByPerson '{Where}','{searchTargetLetterDto.PersonInfoCode}' ,'{searchTargetLetterDto.Flag}'";

            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAutLetterListByPerson));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpPost]
        [Route("LetterInsert")]
        public async Task<IActionResult> LetterInsert([FromBody] LetterInsert letterInsert)
        {
            string CreatorCentral = _configuration.GetConnectionString("Support_CreatorCentral");


            string query = $"exec dbo.spAutLetter_Insert @LetterDate='{letterInsert.LetterDate}', @InOutFlag={letterInsert.InOutFlag},@Title ='{letterInsert.title}', " +
                $"@Description='{letterInsert.Description}',@State ='{letterInsert.LetterState}',@Priority ='{letterInsert.LetterPriority}', @ReceiveType =N'دستی', @CreatorCentral ={letterInsert.CreatorCentral}, @OwnerCentral ={letterInsert.OwnerCentral}, @OwnerPersonInfoRef ={letterInsert.OwnerPersonInfoRef} ";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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

            string query = $"select  LetterRowCode,CreatorCentralRef,AutLetterRow_PropDescription1,Name RowExecutorName,LetterRef ,LetterDate RowLetterDate,LetterDescription LetterRowDescription, LetterState LetterRowState, ExecutorCentralRef RowExecutorCentralRef from vwautletterrow join central on CentralCode=ExecutorCentralRef where LetterRef = {LetterRef} order by LetterRowCode desc";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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
        [Route("GetCentralUser")]
        public async Task<IActionResult> GetCentralUser()
        {

            string query = $"select CentralCode,CentralName from vwCentralUser ";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);

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
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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


        [HttpPost]
        [Route("SetAlarmOff")]
        public async Task<IActionResult> SetAlarmOff([FromBody] AlarmOffDto alarmOffDto)
        {

            string query = $"spWeb_SetAlarmOff {alarmOffDto.LetterRef},{alarmOffDto.CentralRef}";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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



        [HttpGet]
        [Route("GetWebLog")]
        public async Task<IActionResult> GetWebLog()
        {

            string query = $"ShowWebLog";


            try
            {
                DataTable dataTable = await db.SupportApp_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WebLogs", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetWebLog));
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
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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
        [Route("GetAutletterById")]
        public async Task<IActionResult> GetAutletterById(string LetterCode)
        {

            string query = $"select LetterCode,LetterTitle,LetterDate,LetterDescription,LetterState,LetterPriority,OwnerName,OwnerCentralRef,CreatorName,ExecutorName,RowsCount from vwautletter  where LetterCode={LetterCode}";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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
        [Route("Conversation_Insert")]
        public async Task<IActionResult> Conversation_Insert([FromBody] LetterDto letterdto)
        {

            string query = $"Exec spWeb_AutLetterConversation_Insert @LetterRef={letterdto.LetterRef}, @CentralRef={letterdto.CentralRef}, @ConversationText='{letterdto.ConversationText}', @ClassName='Text'";




            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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
        [Route("Update_AutletterRow")]
        public async Task<IActionResult> Update_AutletterRow([FromBody] AutLetterRowInsert letterRowdto)
        {
            string query2 = "";
            string query3 = "";

            if (!string.IsNullOrEmpty(letterRowdto.AutLetterRow_PropDescription1))
            {
                query2 = $" spPropertyValue 'TAutLetterRow' , {letterRowdto.ObjectRef} ";
                DataTable dataTable2 = await db.Support_ExecQuery(HttpContext, query2);


                query3 = $"Update PropertyValue Set Nvarchar1 = '{letterRowdto.AutLetterRow_PropDescription1}' Where ObjectRef = {letterRowdto.ObjectRef}  And ClassName ='TAutLetterRow'";
                DataTable dataTable3 = await db.Support_ExecQuery(HttpContext, query3);
            }



            string query = $" Update AutLetterRow Set LetterState = '{letterRowdto.LetterRowState}' , LetterDescription = '{letterRowdto.LetterRowDescription}' , AlarmActive = 0 , ReformDate = GetDate() Where LetterRowCode = {letterRowdto.ObjectRef}";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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


        [HttpGet]
        [Route("GetLetterFromPersoninfo")]
        public async Task<IActionResult> GetLetterFromPersoninfo(string PersonInfoCode)
        {

            string query = $"spWeb_AutLetterListByPerson {PersonInfoCode}";

            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLetterFromPersoninfo));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpPost]
        [Route("Conversation_UploadFile")]
        public async Task<IActionResult> Conversation_UploadFile([FromBody] ksrImageModel data)
        {


            string query2 = $"Exec spWeb_AutLetterConversation_Insert @LetterRef={data.LetterRef}, @CentralRef={data.CentralRef}, @ConversationText='{data.Title}', @ClassName='File'";

            DataTable dataTable2 = await db.Support_ExecQuery(HttpContext, query2);
            string Conversationref = dataTable2.Rows[0]["ConversationCode"] + "";
            byte[] decodedImage = Convert.FromBase64String(data.File);


            string data_base64 = data.File;
            byte[] data_Bytes = Convert.FromBase64String(data_base64);



            string dataName = $"{data.FileName}.{data.FileType}"; // Constructing the image name
            string dataPath = _configuration.GetConnectionString("Ocr_imagePath") + $"{dataName}"; // Provide the path where you want to save the image


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


                DataTable dataTable1 = await db.Support_ExecQuery(HttpContext, query1);
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
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query11);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(AttachFile_Insert));
                return StatusCode(500, "Internal server error.");
            }





        }


        [HttpGet]
        [Route("GetWebImagess")]
        public async Task<string> GetWebImagess(string pixelScale, string ClassName, string ObjectRef)

        {


            string query = $"SELECT *  FROM KsrImage WHERE Classname = '{ClassName}' AND ObjectRef = {ObjectRef} order by 1 desc";



            DataTable dataTable = await db.Support_ImageExecQuery(HttpContext, query);
            return jsonClass.ConvertAndScaleImageToBase64(Convert.ToInt32(pixelScale), dataTable);

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

                DataTable dataTable1 = await db.Support_ExecQuery(HttpContext, query1);
                string dbname = dataTable1.Rows[0]["dbname"].ToString() ?? "";

                // ۲. گرفتن فایل
                string query = $@" SELECT * FROM {dbname}.dbo.AttachedFiles  WHERE Classname = '{dto.ClassName}'    AND ObjectRef = {dto.ConversationRef}  ORDER BY 1 DESC";

                DataTable dataTable = await db.Support_ImageExecQuery(HttpContext, query);

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
        [Route("GetImageFileFromAttach")]
        public async Task<string> GetImageFileFromAttach(string pixelScale, string ClassName, string ObjectRef)

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

            DataTable dataTable1 = await db.Support_ExecQuery(HttpContext, query1);
            dbname = dataTable1.Rows[0]["dbname"] + "";


            string query = $"SELECT * ,SourceFile FROM  " + dbname + $".dbo.AttachedFiles WHERE Classname = '{ClassName}' AND ObjectRef = {ObjectRef} order by 1 desc";
            DataTable dataTable = await db.Support_ImageExecQuery(HttpContext, query);
            return jsonClass.ConvertAndScaleImageAttachedToBase64(Convert.ToInt32(pixelScale), dataTable);

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

            DataTable dataTable1 = await db.Support_ExecQuery(HttpContext, query1);
            dbname = dataTable1.Rows[0]["dbname"] + "";


            string query = $"SELECT * ,SourceFile FROM  " + dbname + $".dbo.AttachedFiles WHERE Classname = '{ClassName}' AND ObjectRef = {ObjectRef} order by 1 desc";
            DataTable dataTable = await db.Support_ImageExecQuery(HttpContext, query);
            return jsonClass.ConvertVoiceToBase64(dataTable);

        }




        [HttpPost]
        [Route("KowsarAttachFile")]
        public async Task<IActionResult> KowsarAttachFile([FromBody] SearchTargetDto searchTarget)
        {
            string query = $"spWeb_SearchAttachFile {searchTarget.SearchTarget}";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AttachedFiles", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(KowsarAttachFile));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpPost]
        [Route("KowsarAttachUrl")]
        public async Task<IActionResult> KowsarAttachUrl([FromBody] SearchTargetDto searchTarget)
        {
            string query = $"spWeb_SearchAttachFile '{searchTarget.SearchTarget}' ,'URL'";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                string json = jsonClass.JsonResult_Str(dataTable, "AttachedFiles", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(KowsarAttachUrl));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpPost]
        [Route("AttachFile_Insert")]
        public async Task<IActionResult> AttachFile_Insert([FromBody] AttachFile attachFile)
        {

            if (attachFile.Type == "URL")
            {


                string query = $"exec spWeb_AttachFile_Insert '{attachFile.Title}','{attachFile.FileName}','{attachFile.ClassName}','{attachFile.Type}','{attachFile.FilePath}',''";



                try
                {
                    DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                    //string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");
                    string json = jsonClass.JsonResult_Str(dataTable, "AttachedFiles", "");

                    return Content(json, "application/json");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in {Function}", nameof(AttachFile_Insert));
                    return StatusCode(500, "Internal server error.");
                }

            }
            else
            {

                string data_base64 = attachFile.Data;
                byte[] data_Bytes = Convert.FromBase64String(data_base64);



                string dataName = $"{attachFile.FileName}.{attachFile.FileType}"; // Constructing the image name
                string dataName_zip = $"{attachFile.FileName}.zip"; // Constructing the image name
                string dataPath = _configuration.GetConnectionString("Ocr_imagePath") + $"{dataName}"; // Provide the path where you want to save the image
                string data_zipPath = _configuration.GetConnectionString("Ocr_imagePath") + $"{dataName_zip}"; // Provide the path where you want to save the zip file


                System.IO.File.WriteAllBytes(dataPath, data_Bytes);

                using (FileStream zipStream = new FileStream(data_zipPath, FileMode.Create))
                {
                    using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                    {
                        archive.CreateEntryFromFile(dataPath, dataName);
                    }
                }


                string connectionString = _configuration.GetConnectionString("Support_Connection"); // Provide your SQL Server connection string



                using (SqlConnection dbConnection = new SqlConnection(connectionString))
                {

                    dbConnection.Open();
                    string dbname = "";
                    string query1 = "";
                    if (attachFile.ClassName == "AutLetter")
                    {
                        query1 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join AutLetter aut on PeriodId=PeriodRef Where LetterCode= {attachFile.ObjectRef}  Select @db dbname";

                    }
                    else if (attachFile.ClassName == "Factor")
                    {

                        query1 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join Factor f on PeriodId=PeriodRef Where FactorCode= {attachFile.ObjectRef}  Select @db dbname";

                    }
                    else
                    {
                        query1 = $"Declare @dbname nvarchar(200)=db_name()+'Ocr' select  @dbname dbname";


                    }

                    DataTable dataTable1 = await db.Support_ExecQuery(HttpContext, query1);
                    dbname = dataTable1.Rows[0]["dbname"] + "";

                    string sqlCommandText = @" INSERT INTO " + dbname + @".dbo.AttachedFiles
                                            (Title, ClassName, ObjectRef, FileName, SourceFile, Type, Owner, CreationDate, Reformer, ReformDate,FilePath)
                                             VALUES
                                             (@Title, @ClassName, @ObjectRef, @FileName, @SourceFile, @Type, -1000, GETDATE(), -1000, GETDATE(),@FilePath)  ";

                    // Create a SqlCommand object
                    using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, dbConnection))
                    {
                        // Bind parameters
                        sqlCommand.Parameters.AddWithValue("@Title", attachFile.Title);
                        sqlCommand.Parameters.AddWithValue("@FileName", attachFile.FileName);
                        sqlCommand.Parameters.AddWithValue("@ObjectRef", attachFile.ObjectRef);
                        sqlCommand.Parameters.AddWithValue("@ClassName", attachFile.ClassName);
                        sqlCommand.Parameters.AddWithValue("@Type", attachFile.Type);
                        sqlCommand.Parameters.AddWithValue("@FilePath", attachFile.FilePath);
                        sqlCommand.Parameters.AddWithValue("@SourceFile", System.IO.File.ReadAllBytes(data_zipPath));

                        // Execute the command
                        sqlCommand.ExecuteNonQuery();
                    }

                }

                System.IO.File.Delete(dataPath);
                System.IO.File.Delete(data_zipPath);

            }




            string query11 = "select dbo.fnDate_Today() TodeyFromServer ";

            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query11);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(AttachFile_Insert));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpPost]
        [Route("GetAttachFileList")]
        public async Task<IActionResult> GetAttachFileList([FromBody] AttachFile attachFile)
        {


            string dbname = "";
            string query1 = "";
            if (attachFile.ClassName == "AutLetter")
            {
                query1 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join AutLetter aut on PeriodId=PeriodRef Where LetterCode= {attachFile.ObjectRef}  Select @db dbname";

            }
            else if (attachFile.ClassName == "Factor")
            {

                query1 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join Factor f on PeriodId=PeriodRef Where FactorCode= {attachFile.ObjectRef}  Select @db dbname";

            }
            else
            {
                query1 = $"Declare @dbname nvarchar(200)=db_name()+'Ocr' select  @dbname dbname";


            }



            DataTable dataTable1 = await db.Support_ExecQuery(HttpContext, query1);
            dbname = dataTable1.Rows[0]["dbname"] + "";

            string query = $"select * from {dbname}..AttachedFiles where ClassName = '{attachFile.ClassName}' And ObjectRef = {attachFile.ObjectRef} ";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AttachedFiles", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAttachFileList));
                return StatusCode(500, "Internal server error.");
            }




        }

        [HttpGet]
        [Route("GetAttachFileNew")]
        public async Task<IActionResult> GetAttachFileNew(string AttachedFileCode, string ClassName, string ObjectRef)
        {
            try
            {
                // 1️⃣ پیدا کردن دیتابیس
                string queryDb = ClassName switch
                {
                    "AutLetter" => $"SELECT db_name() + 'Ocr' + REPLACE(FromDate, '/', '') dbname FROM FiscalPeriod p JOIN AutLetter aut ON PeriodId = PeriodRef WHERE LetterCode = {ObjectRef}",
                    "Factor" => $"SELECT db_name() + 'Ocr' + REPLACE(FromDate, '/', '') dbname FROM FiscalPeriod p JOIN Factor f ON PeriodId = PeriodRef WHERE FactorCode = {ObjectRef}",
                    _ => $"SELECT db_name() + 'Ocr' dbname"
                };

                DataTable dbResult = await db.Support_ExecQuery(HttpContext, queryDb);
                string dbname = dbResult.Rows[0]["dbname"].ToString();

                // 2️⃣ دریافت فایل
                string query = $"spWeb_GetAttachFile '{AttachedFileCode}', '{dbname}'";
                DataTable dt = await db.Support_ExecQuery(HttpContext, query);

                if (dt.Rows.Count == 0)
                    return NotFound(new { Success = false, Message = "فایل یافت نشد" });

                string base64 = dt.Rows[0]["SourceFile"].ToString() ?? "";
                string fileName = dt.Rows[0]["FileName"].ToString() ?? "file";
                string fileType = dt.Rows[0]["Type"].ToString().ToLower();

                return Ok(new
                {
                    Success = true,
                    FileName = fileName,
                    FileType = fileType,
                    Base64 = base64
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }


        [HttpGet]
        [Route("GetAttachFile")]
        public async Task<IActionResult> GetAttachFileAsync(string AttachedFileCode, string ClassName, string ObjectRef)
        {

            string dbname = "";
            string query11 = "";
            if (ClassName == "AutLetter")
            {
                query11 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join AutLetter aut on PeriodId=PeriodRef Where LetterCode= {ObjectRef}  Select @db dbname";

            }
            else if (ClassName == "Factor")
            {

                query11 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join Factor f on PeriodId=PeriodRef Where FactorCode= {ObjectRef}  Select @db dbname";

            }
            else
            {
                query11 = $"Declare @dbname nvarchar(200)=db_name()+'Ocr' select  @dbname dbname";


            }



            DataTable dataTable4 = await db.Support_ExecQuery(HttpContext, query11);
            dbname = dataTable4.Rows[0]["dbname"] + "";


            string query1 = $"spWeb_GetAttachFile '{AttachedFileCode}' , '{dbname}'";
            DataTable dataTable1 = await db.Support_ExecQuery(HttpContext, query1);
            string base64File = dataTable1.Rows[0]["SourceFile"] + "";
            byte[] fileBytes = Convert.FromBase64String(base64File);


            string FileName = dataTable1.Rows[0]["FileName"] + "";


            string dataName_zip = $"{FileName}.zip"; // Constructing the image name
            string data_zipPath = _configuration.GetConnectionString("Ocr_imagePath") + $"{dataName_zip}";
            string contentType = $"application/{dataTable1.Rows[0]["Type"]}";


            System.IO.File.WriteAllBytes(data_zipPath, fileBytes);
            return File(fileBytes, contentType, Path.GetFileName(data_zipPath));


        }

        [HttpGet]
        [Route("GetLeaveRequestPerson")]
        public async Task<IActionResult> GetLeaveRequestPerson(string TargetDate)
        {

            string query = $" Exec [dbo].[spWeb_GetLeaveRequest_Person_Bydate] '{TargetDate}'";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "LeaveRequests", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLeaveRequestPerson));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("DeleteAttachFile")]
        public async Task<IActionResult> DeleteAttachFile(string AttachedFileCode, string ClassName, string ObjectRef)
        {

            string dbname = "";
            string query11 = "";
            if (ClassName == "AutLetter")
            {
                query11 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join AutLetter aut on PeriodId=PeriodRef Where LetterCode= {ObjectRef}  Select @db dbname";

            }
            else if (ClassName == "Factor")
            {

                query11 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join Factor f on PeriodId=PeriodRef Where FactorCode= {ObjectRef}  Select @db dbname";

            }
            else
            {
                query11 = $"Declare @dbname nvarchar(200)=db_name()+'Ocr' select  @dbname dbname";


            }



            DataTable dataTable4 = await db.Support_ExecQuery(HttpContext, query11);
            dbname = dataTable4.Rows[0]["dbname"] + "";



            string query1 = $"Delete From {dbname}..AttachedFiles where ClassName = '{ClassName}' And AttachedFileCode = {AttachedFileCode} ";




            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query1);
                string json = jsonClass.JsonResult_Str(dataTable, "AttachFiles", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteAttachFile));
                return StatusCode(500, "Internal server error.");
            }






        }




        [HttpGet]
        [Route("GetNotification")]
        public async Task<IActionResult> GetNotification()
        {
            var PersonInfoCode = HttpContext.Request.Headers["PIC"].FirstOrDefault() ?? string.Empty;

            string query = $"spWeb_GetNotification {PersonInfoCode}";
            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetNotification));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpPost]
        [Route("EditFactorProperty")]
        public async Task<IActionResult> EditFactorProperty([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"spWeb_EditFactorProperty '{factorwebDto.starttime}','{factorwebDto.Endtime}','{factorwebDto.worktime}','{factorwebDto.Barbary}',{factorwebDto.ObjectRef} ";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(EditFactorProperty));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpPost]
        [Route("EditCustomerProperty")]
        public async Task<IActionResult> EditCustomerProperty([FromBody] CustomerWebDto customerWebDto)
        {

            string query = $"spWeb_EditCustomerProperty '{customerWebDto.AppNumber}','{customerWebDto.DatabaseNumber}','{customerWebDto.Delegacy}','{customerWebDto.Explain}',{customerWebDto.ObjectRef}";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(EditCustomerProperty));
                return StatusCode(500, "Internal server error.");
            }



        }


        [HttpPost]
        [Route("EditCustomerExplain")]
        public async Task<IActionResult> EditCustomerExplain([FromBody] CustomerWebDto customerWebDto)
        {

            string query = $"Update Customer set Explain ='{customerWebDto.Explain}' where CustomerCode={customerWebDto.ObjectRef}";




            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(EditCustomerExplain));
                return StatusCode(500, "Internal server error.");
            }
        }






        [HttpPost]
        [Route("GetWebFactor")]
        public async Task<IActionResult> GetWebFactor([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"spWeb_Get_Factor '{factorwebDto.ClassName}',{factorwebDto.ObjectRef},'{factorwebDto.StartDateTarget}','{factorwebDto.EndDateTarget}','{factorwebDto.SearchTarget}','{factorwebDto.BrokerRef}'";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(EditCustomerProperty));
                return StatusCode(500, "Internal server error.");
            }



        }






        [HttpPost]
        [Route("GetWebFactorRows")]
        public async Task<IActionResult> GetWebFactorRows([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"spWeb_Get_Factor_Rows '{factorwebDto.ClassName}',{factorwebDto.ObjectRef}";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(EditCustomerProperty));
                return StatusCode(500, "Internal server error.");
            }



        }


        [HttpGet]
        [Route("GetWebFactorSupport")]
        public async Task<IActionResult> GetWebFactorSupport(string FactorCode)
        {

            string query = $" select FactorCode, FactorDate, CustName, CustomerCode, Explain, BrokerRef, BrokerName ,starttime,Endtime,worktime,Barbary from vwFactor where FactorCode = {FactorCode}";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetWebFactorSupport));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("GetWebFactorRowsSupport")]
        public async Task<IActionResult> GetWebFactorRowsSupport(string FactorCode)
        {

            string query = $" select FactorRowCode,GoodName from vwFactorRows where Factorref= {FactorCode}";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetWebFactorRowsSupport));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("DeleteWebFactorRowsSupport")]
        public async Task<IActionResult> DeleteWebFactorRowsSupport(string FactorRowCode)
        {

            string query = $" delete from  FactorRows where FactorRowCode= {FactorRowCode}";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteWebFactorRowsSupport));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("DeleteWebFactorSupport")]
        public async Task<IActionResult> DeleteWebFactorSupport(string FactorCode)
        {

            string query = $" delete from  Factor where FactorCode= {FactorCode}";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteWebFactorSupport));
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpPost]
        [Route("GetGoodListSupport")]
        public async Task<IActionResult> GetGoodListSupport([FromBody] SearchTargetDto searchTargetDto)
        {



            string query = $"spWeb_GetGoodListSupport '{SanitizeInput(searchTargetDto.SearchTarget)}'";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodListSupport));
                return StatusCode(500, "Internal server error.");
            }
        }








        [HttpPost]
        [Route("WebSupportFactorInsert")]
        public async Task<IActionResult> WebSupportFactorInsert([FromBody] FactorwebDto factorwebDto)
        {

            string UserId = _configuration.GetConnectionString("Support_UserId");

            string query = $"spWeb_Factor_Insert  @ClassName ='{factorwebDto.ClassName}',@StackRef =1,@UserId ={UserId},@Date ='{factorwebDto.FactorDate}',@Customer ={factorwebDto.CustomerCode},@Explain ='{factorwebDto.Explain}',@BrokerRef  = {factorwebDto.BrokerRef},@IsShopFactor  = 0";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WebSupportFactorInsert));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("WebSupportFactorInsertRow")]
        public async Task<IActionResult> WebSupportFactorInsertRow([FromBody] FactorRow factorRow)
        {

            string query = $"spWeb_Factor_InsertRow  @ClassName ='{factorRow.ClassName}', @FactorCode={factorRow.FactorRef}, @GoodRef ={factorRow.GoodRef},@Amount =1,@Price =0,@UserId =29,@MustHasAmount =0, @MergeFlag =1 ";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WebSupportFactorInsertRow));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("Support_StartFactorTime")]
        public async Task<IActionResult> StartFactorTime([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"Update PropertyValue Set Nvarchar15 = '{factorwebDto.starttime}'  where ClassName = 'TFactor' And ObjectRef = {factorwebDto.ObjectRef} ";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(StartFactorTime));
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("Support_EndFactorTime")]
        public async Task<IActionResult> EndFactorTime([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"Update PropertyValue Set Nvarchar9 = '{factorwebDto.Endtime}', int1 = {factorwebDto.worktime} where ClassName = 'TFactor' And ObjectRef = {factorwebDto.ObjectRef} ";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(EndFactorTime));
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpPost]
        [Route("Support_ExplainFactor")]
        public async Task<IActionResult> Support_ExplainFactor([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"Update PropertyValue Set Nvarchar14 = '{SanitizeInput(factorwebDto.Barbary)}' where ClassName = 'TFactor' And ObjectRef = {factorwebDto.ObjectRef} ";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Support_ExplainFactor));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpPost]
        [Route("Support_Count")]
        public async Task<IActionResult> Support_Count([FromBody] FactorwebDto factorwebDto)
        {

            string query = $" Declare @S nvarchar(20)=dbo.fnDate_AddDays(dbo.fnDate_Today(),-365) select BrokerCode, BrokerName, sum(worktime)/60 worktime,cast(sum(SumAmount) as int) SumAmount,Count(*) FactorCount from vwFactor where FactorDate>@S group by BrokerName, BrokerCode ";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Support_Count));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpGet]
        [Route("GetGridSchema")]
        public async Task<IActionResult> GetGridSchema(string Where)
        {


            string query = $"Select * From [dbo].[fnGetGridSchema]('{Where}')  where Visible = 1";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GridSchemas", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGridSchema));
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("GetFactors")]
        public async Task<IActionResult> GetFactors([FromBody] SearchTargetDto searchTargetDto)
        {

            string query = $" Exec spWeb_GetFactor";




            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetFactors));
                return StatusCode(500, "Internal server error.");
            }
        }

        /*
        [HttpPost]
        [Route("GetFactor")]
        public async Task<IActionResult> GetFactor([FromBody] FactorwebDto factorwebDto)
        {

            string query = $" spWeb_GetFactor '{factorwebDto.StartDateTarget}','{factorwebDto.EndDateTarget}','{factorwebDto.SearchTarget}','{factorwebDto.BrokerRef}','{factorwebDto.isShopFactor}'";



            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");



        }

        */



        [HttpPost]
        [Route("GetSupportFactors")]
        public async Task<IActionResult> GetSupportFactors([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"Exec spWeb_GetSupportFactor '{factorwebDto.StartDateTarget}','{factorwebDto.EndDateTarget}','{factorwebDto.SearchTarget}','{factorwebDto.BrokerRef}','{factorwebDto.isShopFactor}'";







            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetSupportFactors));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("WebFactorInsert")]
        public async Task<IActionResult> WebFactorInsert([FromBody] FactorwebDto factorwebDto)
        {


            string UserId = _configuration.GetConnectionString("Support_UserId");

            string query = $"spWeb_Factor_Insert  @ClassName ='{factorwebDto.ClassName}',@StackRef ={factorwebDto.StackRef},@UserId ={UserId},@Date ='{factorwebDto.FactorDate}',@Customer ={factorwebDto.CustomerCode},@Explain ='{factorwebDto.Explain}',@BrokerRef  = {factorwebDto.BrokerRef},@IsShopFactor  = {factorwebDto.isShopFactor}";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WebFactorInsert));
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("WebFactorInsertRow")]
        public async Task<IActionResult> WebFactorInsertRow([FromBody] FactorRow factorRow)
        {

            string UserId = _configuration.GetConnectionString("Support_UserId");

            string query = $"spWeb_Factor_InsertRow  @ClassName ='{factorRow.ClassName}', @FactorCode={factorRow.FactorRef}, @GoodRef ={factorRow.GoodRef},@Amount ={factorRow.Amount},@Price ={factorRow.Price},@UserId ={UserId},@MustHasAmount ={factorRow.MustHasAmount}, @MergeFlag ={factorRow.MergeFlag} ";




            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(WebFactorInsertRow));
                return StatusCode(500, "Internal server error.");
            }
        }






        [HttpGet]
        [Route("GetCustomerFactor")]
        public async Task<IActionResult> GetCustomerFactor(string Where)
        {


            string query = $"spWeb_GetCustomerFactor {Where}";


            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCustomerFactor));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("GetSupportPanel")]
        public async Task<IActionResult> GetSupportPanel([FromBody] SupportDto supportDto)

        {
            // 1 support panel
            // 2 EmptyEndTimeCount


            string query = $"   spWeb_GetSupportPanel @DateTarget = '{supportDto.DateTarget}', @BrokerCode = {supportDto.BrokerCode}, @Flag = {supportDto.Flag}";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "SupportDatas", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetSupportPanel));
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpPost]
        [Route("ManualAttendance")]
        public async Task<IActionResult> ManualAttendance([FromBody] ManualAttendance manualAttendance)

        {
            // 0 ghayeb 
            // 1 hozor
            // 2 mashghol

            string userId = _configuration.GetConnectionString("Support_UserId");

            string query = $"spWeb_Attendance_Insert @CentralRef = {manualAttendance.CentralRef}, @Status = {manualAttendance.Status}";




            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Attendances", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ManualAttendance));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("AttendanceDashboard")]
        public async Task<IActionResult> AttendanceDashboard()
        {
            string query = "spWeb_Attendance_Dashboard";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Attendances", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(AttendanceDashboard));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("AttendanceHistory")]
        public async Task<IActionResult> AttendanceHistory(string CentralRef)
        {
            string query = $"spWeb_Attendance_History @CentralRef = {CentralRef}";




            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Attendances", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(AttendanceHistory));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("DeleteAutLetterRows")]
        public async Task<IActionResult> DeleteAutLetterRows(string LetterRowCode)
        {

            string query = $" Delete From  AutLetterRow where LetterRowCode= {LetterRowCode}";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AutLetters", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteAutLetter));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpGet]
        [Route("GetGoodBase")]
        public async Task<IActionResult> GetGoodBase(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},0";




            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodBase));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetLastGoodData")]
        public async Task<IActionResult> GetLastGoodData()
        {

            string query = $"  declare @ss int  select  @ss=max(GoodCode) from good exec spWeb_GetGoodById @ss,0";




            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLastGoodData));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpPost]
        [Route("GoodCrudService")]
        public async Task<IActionResult> GoodCrudService([FromBody] JsonModelDto jsonModelDto)

        {


            string query = $"Exec spGood_AddNew '{jsonModelDto.JsonData}' ";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodCrudService));
                return StatusCode(500, "Internal server error.");
            }
        }







        [HttpPost]
        [Route("GetAttendance_StatusDurations")]
        public async Task<IActionResult> GetAttendance_StatusDurations([FromBody] ManualAttendance manualAttendance)

        {
            // 0 ghayeb 
            // 1 hozor
            // 2 mashghol


            string query = $"spWeb_GetAttendance_StatusDurations @CentralRef={manualAttendance.CentralRef},@TargetDate='{manualAttendance.TargetDate}',@UseTodayInstead={manualAttendance.UseTodayInstead}";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Attendances", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAttendance_StatusDurations));
                return StatusCode(500, "Internal server error.");
            }
        }








        [HttpGet]
        [Route("GetAddress")]
        public async Task<IActionResult> GetAddress(string CentralCode)
        {
            string query = $"spWeb_GetAddress  {CentralCode}";




            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Address", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAddress));
                return StatusCode(500, "Internal server error.");
            }
        }





        [HttpPost]
        [Route("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress([FromBody] AddressDto addressDto)

        {
            // 0 ghayeb 
            // 1 hozor
            // 2 mashghol


            string query = $"spWeb_UpdateAddress Exec spWeb_UpdateAddress " +
                $"@CentralRef ={addressDto.CentralRef},@AddressTitle ='{addressDto.AddressTitle}',@Address ='{addressDto.Address}'," +
                $"@ZipCode ='{addressDto.ZipCode}',@PostCode ='{addressDto.PostCode}',@Phone ='{addressDto.Phone}',@Fax ='{addressDto.Fax}',@Mobile   ='{addressDto.Mobile}'," +
                $"@Email ='{addressDto.Email}',@MobileName ='{addressDto.MobileName}'";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Address", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UpdateAddress));
                return StatusCode(500, "Internal server error.");
            }
        }






        [HttpPost]
        [Route("GetTasks")]
        public async Task<IActionResult> GetTasks([FromBody] KowsarTaskDto dto)
        {
            // تبدیل TaskRef و Flag به int یا NULL
            string taskRef = !string.IsNullOrEmpty(dto.TaskRef) ? dto.TaskRef : "NULL";
            string flag = !string.IsNullOrEmpty(dto.Flag) ? dto.Flag : "0";

            string query = $"spWeb_GetKowsarTask @TaskRef = {taskRef}, @Flag = {flag}";

            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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

                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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

                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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


                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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


                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarTasks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(DeleteTaskAll));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("GetKowsarReport")]
        public async Task<IActionResult> GetKowsarReport([FromBody] KowsarReportDto kowsarReportDto)
        {
            
            try
            {
                 
                string query = $"Exec spWeb_GetKowsarReport @SearchTarget = '{kowsarReportDto.SearchTarget}',@CentralRef = {kowsarReportDto.CentralRef},@LetterRowCode = {kowsarReportDto.LetterRowCode},@Flag = {kowsarReportDto.Flag},@DateTarget = '{kowsarReportDto.DateTarget}'";


                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarReports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GetKowsarReport));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("GetCustomerReport")]
        public async Task<IActionResult> GetCustomerReport([FromBody] KowsarReportDto kowsarReportDto)
        {

            try
            {
                 
                string query = $"Exec spWeb_GetCustomerReport @StartDateTarget = '{kowsarReportDto.StartDateTarget}',@EndDateTarget = '{kowsarReportDto.EndDateTarget}',@SearchTarget = '{kowsarReportDto.SearchTarget}',@CustomerRef = {kowsarReportDto.CustomerRef},@Flag = {kowsarReportDto.Flag}";


                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarReports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GetCustomerReport));
                return StatusCode(500, "Internal server error.");
            }
        }




            [HttpPost]
            [Route("GetFactorByCustomerCode")]
            public async Task<IActionResult> GetFactorByCustomerCode([FromBody] SearchTargetDto searchTargetDto)
            {



                string query = $"Exec [dbo].[spWeb_GetFactorByCustomerCode] '{searchTargetDto.ClassName}',{searchTargetDto.ObjectRef}";

            DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
            string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
            return Content(json, "application/json");


        }


        [HttpPost]
        [Route("GetCity")]
        public async Task<IActionResult> GetCity([FromBody] SearchTargetDto searchTargetDto)
        {



            string query = $"Select CityCode,Name from City ";

            DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
            string json = jsonClass.JsonResult_Str(dataTable, "Citys", "");
            return Content(json, "application/json");


        }



        [HttpGet]
        [Route("GetCustomerByCode")]
        public async Task<IActionResult> GetCustomerByCode(string CustomerCode)
        {

            string query = $"select CustomerCode, FName, Name, CityCode, CityName, Address, Phone, Mobile, Email, Explain, ZipCode from vwcustomer Where CustomerCode= {CustomerCode} ";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCustomerByCode));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerCrud")]
        public async Task<IActionResult> CustomerCrud([FromBody] CustomerCrudDto customerCrudDto)
        {

            try
            {

                string UserId = _configuration.GetConnectionString("Support_UserId");
                string query = $"Exec spWeb_Customer_Crud  @FName ='{customerCrudDto.FName}',  @LName ='{customerCrudDto.LName}', @CityCode ={customerCrudDto.CityCode}, @Address ='{customerCrudDto.Address}', @Phone ='{customerCrudDto.Phone}', @Mobile ='{customerCrudDto.Mobile}', @CustomerCode  ={customerCrudDto.CustomerCode}, @UserId ={UserId}";


                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(CustomerCrud));
                return StatusCode(500, "Internal server error.");
            }
        }








        [HttpPost]
        [Route("InUp_MonthSummary")]
        public async Task<IActionResult> InUp_MonthSummary([FromBody] MonthSummaryDto monthSummaryDto)
        {

            try
            {
                 
                string query = $"Exec spweb_InUp_MonthSummary {monthSummaryDto.Sal},{monthSummaryDto.Mah},{monthSummaryDto.TotalDays},{monthSummaryDto.HolidayDays}, '{monthSummaryDto.Explain}'";


                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "MonthSummarys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(InUp_MonthSummary));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("InUp_Employee")]
        public async Task<IActionResult> InUp_Employee([FromBody] EmployeeDto employeDto)
        {

            try
            {
               
                string query = $"Exec spweb_InUp_Employee {employeDto.EmployeeCode},'{employeDto.FirstName}','{employeDto.LastName}','{employeDto.CodeMeli}','{employeDto.JobTitle}' ," +
                    $" {employeDto.Rozkarkard}, {employeDto.NerkhHoghogh}, {employeDto.NerkhSanavat}, {employeDto.NerkhMaskan}, {employeDto.NerkhKharobar}, {employeDto.NerkhEzafekar}," +
                    $" {employeDto.NerkhPadash}, {employeDto.TedadPadash}, {employeDto.NerkhExtra1}, {employeDto.TedadExtra1}, {employeDto.NerkhExtra2}, {employeDto.TedadExtra2}," +
                    $" {employeDto.BimePaye},{employeDto.BimeTakmili},{employeDto.Extra3}, {employeDto.Extra4}, {employeDto.SaatNaharNamaz}, '{employeDto.VaziyatTaahol}', {employeDto.TedadOlad}, {employeDto.HaghOlad}," +
                    $" {employeDto.HaghTaahol}, '{employeDto.Explain}' ";


                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Employees", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(InUp_Employee));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("AddSalaryForAllEmployees")]
        public async Task<IActionResult> AddSalaryForAllEmployees([FromBody] MonthSummaryDto monthSummaryDto)
        {

            try
            {
                string query = $"Exec spweb_AddSalarySummaryForAllEmployees {monthSummaryDto.MonthSummaryCode}";


                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "SalarySummarys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(AddSalaryForAllEmployees));
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpPost]
        [Route("UpdateWorkingEmployee")]
        public async Task<IActionResult> UpdateWorkingEmployee([FromBody] SalarySummaryDto salarySummaryDto)
        {

            try
            {

                string query = $"Exec spweb_UpdateWorkingAndOvertimeForEmployee {salarySummaryDto.SalarySummaryCode},N'{salarySummaryDto.WorkingHours}',{salarySummaryDto.LeaveDays}";

                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "SalarySummarys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(UpdateWorkingEmployee));
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpPost]
        [Route("GetSalarySummary")]
        public async Task<IActionResult> GetSalarySummary([FromBody] SalarySummaryDto salarySummaryDto)
        {

            try
            {
                string query = $"Exec spWeb_GetSalarySummary '{salarySummaryDto.SearchTarget}',{salarySummaryDto.Sal},{salarySummaryDto.Mah},{salarySummaryDto.EmployeCode}";


                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "SalarySummarys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GetSalarySummary));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("GetMonthSummary")]
        public async Task<IActionResult> GetMonthSummary([FromBody] MonthSummaryDto monthSummaryDto)
        {

            try
            {
                string query = $"Exec spWeb_GetMonthSummary {monthSummaryDto.Sal},{monthSummaryDto.Mah}";


                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "MonthSummarys", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GetMonthSummary));
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("GetEmployee")]
        public async Task<IActionResult> GetEmploye([FromBody] EmployeeDto employeDto)
        {

            try
            {
                 
                string query = $"Exec spWeb_GetEmployee '{employeDto.SearchTarget}'";


                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Employees", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GetEmploye));
                return StatusCode(500, "Internal server error.");
            }
        }
        




        [HttpGet]
        [Route("GetEmployeeByCode")]
        public async Task<IActionResult> GetEmployeeByCode(string EmployeeCode)
        {

            string query = $"Select * from Employee Where EmployeeCode= {EmployeeCode} ";
            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Employees", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetEmployeeByCode));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpGet]
        [Route("GetMonthSummaryByCode")]
        public async Task<IActionResult> GetMonthSummaryByCode(string MonthSummaryCode)
        {

            string query = $"Select * from MonthSummary Where MonthSummaryCode= {MonthSummaryCode} ";



            try
            {
                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "MonthSummarys", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetMonthSummaryByCode));
                return StatusCode(500, "Internal server error.");
            }
        }





        [HttpPost]
        [Route("WorkItem_Insert")]
        public async Task<IActionResult> WorkItem_Insert([FromBody] WorkItemDto workItemDto)
        {
            try
            {
                // رشته‌ها با تک کوتیشن ' و nullها با "" جایگزین شدند
                string query = $"Exec spWeb_WorkItem_Insert " +
                               $"'{(workItemDto.Title ?? "")}', " +
                               $"'{(workItemDto.Explain ?? "")}', " +
                               $"{(workItemDto.Status)}, " +
                               $"{(workItemDto.Priority)}, " +
                               $"'{(workItemDto.OriginalDate ?? "")}', " +
                               $"'{(workItemDto.TargetDate ?? "")}', " +
                               $"'{(workItemDto.ModuleName ?? "")}', " +
                               $"'{(workItemDto.ClassName ?? "")}', " +
                               $"{(workItemDto.ObjectRef)}, " +
                               $"{(workItemDto.OwnerRef)}, " +
                               $"{(workItemDto.CreatorRef)}";

                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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
                               $"'{(workItemDto.Title ?? "")}', " +
                               $"'{(workItemDto.Explain ?? "")}', " +
                               $"{workItemDto.Status}, " +
                               $"{workItemDto.Priority}, " +
                               $"'{(workItemDto.TargetDate ?? "")}', " +
                               $"'{(workItemDto.ModuleName ?? "")}', " +
                               $"'{(workItemDto.ClassName ?? "")}', " +
                               $"{(string.IsNullOrEmpty(workItemDto.ObjectRef) ? "NULL" : workItemDto.ObjectRef)} ";

                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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
                               $"'{(workItemDto.ChangeStateDate ?? "")}'";

                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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
  
                string query = $"Exec spWeb_WorkItem_Get {workItemDto.CentralRef},{workItemDto.Status}, '{(workItemDto.SearchTarget ?? "")}'";

        DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
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

                DataTable dataTable = await db.Support_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "WorkItems", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(WorkItem_Delete));
                return StatusCode(500, "Internal server error.");
            }
        }





        private string SanitizeInput(string input)
        {
            if (input == null)
                return string.Empty;

            input = input.Replace("'", "''");  // Escape single quotes for SQL
            input = input.Replace(";", "");    // Remove semicolons
            input = input.Replace("--", "");   // Remove SQL comments
            input = input.Replace("/*", "");   // Remove SQL block comments
            input = input.Replace("*/", "");   // Remove SQL block comments

            input = input.Replace("<", "&lt;"); // < becomes &lt;
            input = input.Replace(">", "&gt;"); // > becomes &gt;
            input = input.Replace("&", "&amp;"); // & becomes &amp;
            input = input.Replace("\"", "&quot;"); // " becomes &quot;
            input = input.Replace("'", "&#x27;"); // ' becomes &#x27;
            input = input.Replace("/", "&#x2F;"); // / becomes &#x2F;
            input = input.Replace("\\", "&#x5C;"); // \ becomes &#x5C;

            input = input.Trim();

            return input;
        }





    }
}
