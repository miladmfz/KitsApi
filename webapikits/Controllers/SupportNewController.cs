using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using webapikits.Model;

namespace webapikits.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportNewController : ControllerBase
    {
        private readonly IDbService _dbService;
        private readonly IJsonFormatter _jsonFormatter;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        public SupportNewController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<SupportNewController> logger,
            IConfiguration configuration)
        {
            _dbService = dbService;
            _jsonFormatter = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }


        [HttpGet("Test")]
        public IActionResult Index()
        {
            return Ok("SupportNewController is working");
        }




        [HttpGet]
        [Route("GetTodeyFromServer")]
        public async Task<IActionResult> GetTodeyFromServer()
        {
            const string query = "SELECT dbo.fnDate_Today() AS TodeyFromServer";

            try
            {
                DataTable dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query);
                string json = _jsonFormatter.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetTodeyFromServer));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost]
        [Route("UpdatePersonInfo")]
        public async Task<IActionResult> UpdatePersonInfo([FromBody] PersonInfoDto personInfoDto)
        {
            string query = "Exec [dbo].[spWeb_UpdatePersonInfo] @PersonInfoCode, @PhFirstName, @PhLastName, @PhCompanyName, @PhAddress1, @PhTel1, @PhMobile1, @PhEmail";

            var parameters = new Dictionary<string, object>
{
    { "@PersonInfoCode", SanitizeInput(personInfoDto.PersonInfoCode) },
    { "@PhFirstName", SanitizeInput(personInfoDto.PhFirstName ?? string.Empty) },
    { "@PhLastName", SanitizeInput(personInfoDto.PhLastName ?? string.Empty) },
    { "@PhCompanyName", SanitizeInput(personInfoDto.PhCompanyName ?? string.Empty) },
    { "@PhAddress1", SanitizeInput(personInfoDto.PhAddress1 ?? string.Empty) },
    { "@PhTel1", SanitizeInput(personInfoDto.PhTel1 ?? string.Empty) },
    { "@PhMobile1", SanitizeInput(personInfoDto.PhMobile1 ?? string.Empty) },
    { "@PhEmail", SanitizeInput(personInfoDto.PhEmail ?? string.Empty) },
};


            try
            {
                DataTable dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dataTable, "users");
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
            if (string.IsNullOrEmpty(PersonInfoCode))
            {
                return BadRequest("PersonInfoCode is required.");
            }

            string query = "Exec [dbo].[spWeb_GetKowsarPersonInfo] @PersonInfoCode";

            var parameters = new Dictionary<string, object>
            {
                { "@PersonInfoCode", PersonInfoCode }
            };

            try
            {
                DataTable dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dataTable, "users");
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
            if (loginUserDto == null || string.IsNullOrEmpty(loginUserDto.UName) || string.IsNullOrEmpty(loginUserDto.UPass))
            {
                return BadRequest("Username and password are required.");
            }

            string query = "Exec [dbo].[spWeb_IsXUser] @UName, @UPass";

            var parameters = new Dictionary<string, object>
{
    {"@UName", SanitizeInput(loginUserDto.UName)},
    {"@UPass", SanitizeInput(loginUserDto.UPass)}
};



            try
            {
                DataTable dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dataTable, "users");
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
            if (string.IsNullOrEmpty(ObjectType))
            {
                return BadRequest("ObjectType parameter is required.");
            }

            string query = "SELECT * FROM dbo.fnObjectType(@ObjectType)";
            var parameters = new Dictionary<string, object>
    {
{"@ObjectType", SanitizeInput(ObjectType)}
    };

            try
            {
                DataTable dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dataTable, "ObjectTypes");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetObjectTypeFromDbSetup));
                return StatusCode(500, "Internal server error.");
            }
        }



        //[HttpPost("UploadImage")]
        //public async Task<IActionResult> UploadImage([FromBody] ksrImageModeldto data)
        //{
        //    if (data == null || string.IsNullOrEmpty(data.image) || string.IsNullOrEmpty(data.ClassName) || data.ObjectCode <= 0)
        //        return BadRequest("Invalid input data.");

        //    try
        //    {
        //        // Decode base64 image string
        //        byte[] decodedImage = Convert.FromBase64String(data.image);

        //        // Build file path securely (از Path.Combine استفاده کنید)
        //        var imageFolder = _configuration.GetConnectionString("web_imagePath");
        //        var filePath = Path.Combine(imageFolder, $"{data.ObjectCode}.jpg");

        //        await System.IO.File.WriteAllBytesAsync(filePath, decodedImage);

        //        // پارامترایز کردن کوئری
        //        string query = "Exec spImageImport @ClassName, @ObjectCode, @FilePath; select @@IDENTITY KsrImageCode";

        //        var parameters = new Dictionary<string, object>
        //{
        //    {"@ClassName", data.ClassName},
        //    {"@ObjectCode", data.ObjectCode},
        //    {"@FilePath", filePath}
        //};

        //        DataTable dataTable = await _dbService.Support_ImageExecQueryAsync(query, parameters);

        //        return Ok(new { status = "Ok" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred in UploadImage");
        //        return StatusCode(500, "Internal server error.");
        //    }
        //}



        [HttpGet("GetCentralById")]
        public async Task<IActionResult> GetCentralById(string CentralCode)
        {
            if (string.IsNullOrEmpty(CentralCode))
                return BadRequest("CentralCode is required.");

            string query = "select CentralCode,Title,Name,FName,Manager,Delegacy,CentralName from vwcentral where CentralCode = @CentralCode";
            var parameters = new Dictionary<string, object>
    {
        {"@CentralCode", CentralCode}
    };

            try
            {
                DataTable dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dataTable, "Centrals");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCentralById));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost("GetKowsarCentral")]
        public async Task<IActionResult> GetKowsarCentral([FromBody] SearchTargetDto searchTargetDto)
        {
            if (searchTargetDto == null || string.IsNullOrEmpty(searchTargetDto.SearchTarget))
                return BadRequest("SearchTarget is required.");

            string query = "Exec [dbo].[spWeb_GetKowsarCentral] @SearchTarget";
            var parameters = new Dictionary<string, object>
    {
       {"@SearchTarget", SanitizeInput(searchTargetDto.SearchTarget)}

    };

            try
            {
                DataTable dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dataTable, "Centrals");
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
            if (searchTargetDto == null || string.IsNullOrEmpty(searchTargetDto.SearchTarget))
                return BadRequest("SearchTarget is required.");

            string query = "Exec [dbo].[spWeb_GetKowsarCustomer] @SearchTarget";
            var parameters = new Dictionary<string, object>
    {
        {"@SearchTarget", SanitizeInput(searchTargetDto.SearchTarget)}

    };

            try
            {
                DataTable dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dataTable, "Customers");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetKowsarCustomer));
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("GetLetterList")]
        public async Task<IActionResult> GetLetterList([FromBody] SearchTargetLetterDto searchTargetLetterDto)
        {
            if (searchTargetLetterDto == null)
                return BadRequest("Invalid input");

            var whereClauses = new List<string>();
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(searchTargetLetterDto.SearchTarget))
            {
                whereClauses.Add("(LetterTitle LIKE @SearchTarget OR LetterDescription LIKE @SearchTarget OR ds.RowExecutorName LIKE @SearchTarget)");
                parameters.Add("@SearchTarget", $"%{searchTargetLetterDto.SearchTarget}%");
            }

            if (!string.IsNullOrEmpty(searchTargetLetterDto.CentralRef))
            {
                whereClauses.Add("(CreatorCentralRef = @CentralRef OR OwnerCentralRef = @CentralRef OR RowExecutorCentralRef = @CentralRef)");
                parameters.Add("@CentralRef", searchTargetLetterDto.CentralRef);
            }

            if (!string.IsNullOrEmpty(searchTargetLetterDto.CreationDate))
            {
                whereClauses.Add("LetterDate >= @CreationDate");
                parameters.Add("@CreationDate", searchTargetLetterDto.CreationDate);
            }

            string where = whereClauses.Count > 0 ? string.Join(" AND ", whereClauses) : "1=1";

            string query = $"EXEC spWeb_AutLetterList @Where, @OwnCentralRef";

            parameters.Add("@Where", SanitizeInput(where));
            parameters.Add("@OwnCentralRef", searchTargetLetterDto.OwnCentralRef);

            try
            {
                DataTable dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLetterList));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost("LetterInsert")]
        public async Task<IActionResult> LetterInsert([FromBody] LetterInsert letterInsert)
        {
            if (letterInsert == null)
                return BadRequest("Invalid input");

            string creatorCentral = _configuration.GetValue<string>("AppSettings:Support_CreatorCentral");
            string query = "exec dbo.spAutLetter_Insert " +
                           "@LetterDate, @InOutFlag, @Title, @Description, @State, @Priority, @ReceiveType, @CreatorCentral, @OwnerCentral";

            var parameters = new Dictionary<string, object>
{
    { "@LetterDate", letterInsert.LetterDate },  // اگر تاریخ هست نیازی به sanitize نداره
    { "@InOutFlag", letterInsert.InOutFlag },    // اگر عدد یا enum هست نیازی به sanitize نداره
    { "@Title", SanitizeInput(letterInsert.title) },
    { "@Description", SanitizeInput(letterInsert.Description) },
    { "@State", letterInsert.LetterState },      // عدد یا enum
    { "@Priority", letterInsert.LetterPriority },// عدد یا enum
    { "@ReceiveType", SanitizeInput("دستی") },   // اگر متن هست
    { "@CreatorCentral", letterInsert.CentralRef }, // عدد
    { "@OwnerCentral", creatorCentral }          // عدد
};


            try
            {
                DataTable dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LetterInsert));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpGet("GetLetterRowList")]
        public async Task<IActionResult> GetLetterRowList(string LetterRef)
        {
            if (string.IsNullOrEmpty(LetterRef))
                return BadRequest("LetterRef is required");

            string query = @"select LetterRowCode, CreatorCentralRef, AutLetterRow_PropDescription1, Name RowExecutorName,
                            LetterRef, LetterDate RowLetterDate, LetterDescription LetterRowDescription,
                            LetterState LetterRowState, ExecutorCentralRef RowExecutorCentralRef
                     from vwautletterrow 
                     join central on CentralCode = ExecutorCentralRef 
                     where LetterRef = @LetterRef 
                     order by LetterRowCode desc";

            var parameters = new Dictionary<string, object>
    {
        { "@LetterRef", LetterRef }
    };

            try
            {
                DataTable dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLetterRowList));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpGet("GetCentralUser")]
        public async Task<IActionResult> GetCentralUser()
        {
            const string query = "SELECT CentralCode, CentralName FROM vwCentralUser";

            try
            {
                var dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query);
                var json = _jsonFormatter.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCentralUser");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("AutLetterRowInsert")]
        public async Task<IActionResult> AutLetterRowInsert([FromBody] AutLetterRowInsert autLetterRowInsert)
        {
            string query = "spAutLetterRow_Insert @LetterRef, @LetterDate, @Description, @State, @Priority, @CreatorCentral, @ExecuterCentral";

            var parameters = new Dictionary<string, object>
{
    { "@LetterRef", SanitizeInput(autLetterRowInsert.LetterRef )},        
    { "@LetterDate", SanitizeInput(autLetterRowInsert.LetterDate )},      
    { "@Description", SanitizeInput(autLetterRowInsert.Description) }, 
    { "@State", SanitizeInput(autLetterRowInsert.LetterState) },          
    { "@Priority", SanitizeInput(autLetterRowInsert.LetterPriority )},   
    { "@CreatorCentral", SanitizeInput(autLetterRowInsert.CreatorCentral) },  
    { "@ExecuterCentral", SanitizeInput(autLetterRowInsert.ExecuterCentral) } 
};


            try
            {
                var dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                var json = _jsonFormatter.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AutLetterRowInsert");
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost("SetAlarmOff")]
        public async Task<IActionResult> SetAlarmOff([FromBody] AlarmOffDto alarmOffDto)
        {
            string query = "spWeb_SetAlarmOff @LetterRef, @CentralRef";

            var parameters = new Dictionary<string, object>
{
    {"@LetterRef", SanitizeInput(alarmOffDto.LetterRef)},
    {"@CentralRef", SanitizeInput(alarmOffDto.CentralRef)}
};


            try
            {
                var dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                var json = _jsonFormatter.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SetAlarmOff");
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet("GetAutConversation")]
        public async Task<IActionResult> GetAutConversation(string LetterRef)
        {
            string query = "Exec spWeb_GetAutConversation @LetterRef";

            var parameters = new Dictionary<string, object>
    {
        {"@LetterRef", SanitizeInput(LetterRef)}
    };

            try
            {
                var dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                var json = _jsonFormatter.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAutConversation");
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet("GetAutletterById")]
        public async Task<IActionResult> GetAutletterById(string LetterCode)
        {
            string query = "select LetterCode,LetterTitle,LetterDate,LetterDescription,LetterState,LetterPriority,OwnerName,CreatorName,ExecutorName,RowsCount from vwautletter where LetterCode = @LetterCode";

            var parameters = new Dictionary<string, object>
    {
        {"@LetterCode", SanitizeInput(LetterCode)}
    };

            try
            {
                var dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                var json = _jsonFormatter.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAutletterById");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("Conversation_Insert")]
        public async Task<IActionResult> Conversation_Insert([FromBody] LetterDto letterDto)
        {
            string query = "Exec spWeb_AutLetterConversation_Insert @LetterRef, @CentralRef, @ConversationText";

            var parameters = new Dictionary<string, object>
    {
        {"@LetterRef", SanitizeInput(letterDto.LetterRef)},
        {"@CentralRef", SanitizeInput(letterDto.CentralRef)},
        {"@ConversationText", SanitizeInput(letterDto.ConversationText)}
    };

            try
            {
                var dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                var json = _jsonFormatter.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Conversation_Insert");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("Update_AutletterRow")]
        public async Task<IActionResult> Update_AutletterRow([FromBody] AutLetterRowInsert letterRowdto)
        {
            try
            {
                if (!string.IsNullOrEmpty(letterRowdto.AutLetterRow_PropDescription1))
                {
                    // اجرای spPropertyValue
                    string query2 = "spPropertyValue @ClassName, @ObjectRef";
                    var param2 = new Dictionary<string, object>
            {
                { "@ClassName", "TAutLetterRow" },
                { "@ObjectRef", SanitizeInput(letterRowdto.ObjectRef) }
            };
                    await _dbService.ExecSupportQueryAsync(HttpContext, query2, param2);

                    // آپدیت PropertyValue
                    string query3 = "Update PropertyValue Set Nvarchar1 = @Description Where ObjectRef = @ObjectRef And ClassName = @ClassName";
                    var param3 = new Dictionary<string, object>
            {
                { "@Description", letterRowdto.AutLetterRow_PropDescription1 },
                { "@ObjectRef", letterRowdto.ObjectRef },
                { "@ClassName", "TAutLetterRow" }
            };
                    await _dbService.ExecSupportQueryAsync(HttpContext, query3, param3);
                }

                // آپدیت AutLetterRow
                string query = @"Update AutLetterRow 
                         Set LetterState = @LetterState, LetterDescription = @LetterDescription, AlarmActive = 0 
                         Where LetterRowCode = @ObjectRef";

                var param = new Dictionary<string, object>
        {
            { "@LetterState", SanitizeInput(letterRowdto.LetterRowState) },
            { "@LetterDescription", SanitizeInput(letterRowdto.LetterRowDescription) },
            { "@ObjectRef", SanitizeInput(letterRowdto.ObjectRef) }
        };

                var dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, param);
                var json = _jsonFormatter.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Update_AutletterRow");
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet("GetLetterFromPersoninfo")]
        public async Task<IActionResult> GetLetterFromPersoninfo(string PersonInfoCode)
        {
            try
            {
                string query = "spWeb_AutLetterListByPerson @PersonInfoCode";
                var param = new Dictionary<string, object> { { "@PersonInfoCode", PersonInfoCode } };

                var dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, param);
                var json = _jsonFormatter.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLetterFromPersoninfo");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("GetAutLetterListByPerson")]
        public async Task<IActionResult> GetAutLetterListByPerson([FromBody] SearchTargetLetterDto dto)
        {
            try
            {
                string where = "";

                if (!string.IsNullOrEmpty(dto.SearchTarget))
                {
                    // به جای string interpolation از پارامتر استفاده کنیم ولی چون این مقدار داخل sp هست و LIKE داره،
                    // به صورت امن باید مقدار رو به صورت پارامتر ارسال کنیم یا در Stored Procedure اصلاح بشه.
                    // اگر امکان تغییر SP نیست، دست کم Escape کنیم.

                    string escapedSearch = dto.SearchTarget.Replace("'", "''");
                    where = $"(LetterTitle LIKE '%{escapedSearch}%' OR LetterDescription LIKE '%{escapedSearch}%' OR ds.RowExecutorName LIKE '%{escapedSearch}%')";
                }

                if (!string.IsNullOrEmpty(dto.CreationDate))
                {
                    if (!string.IsNullOrEmpty(where))
                        where += $" AND LetterDate >= '{dto.CreationDate.Replace("'", "''")}'";
                    else
                        where = $"LetterDate >= '{dto.CreationDate.Replace("'", "''")}'";
                }

                string query = "spWeb_AutLetterListByPersontest @Where, @PersonInfoCode";
                var parameters = new Dictionary<string, object>
        {
            { "@Where",SanitizeInput( where) },
            { "@PersonInfoCode", dto.PersonInfoCode }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                var json = _jsonFormatter.JsonResultWithout_Str(dt);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAutLetterListByPerson");
                return StatusCode(500, "Internal server error");
            }
        }



        //[HttpPost("Conversation_UploadImage")]
        //public async Task<IActionResult> Conversation_UploadImage([FromBody] ksrImageModel data)
        //{
        //    try
        //    {
        //        // 1. Insert conversation with text 'Image'
        //        string insertConversationQuery = "Exec spWeb_AutLetterConversation_Insert @LetterRef, @CentralRef, @ConversationText";
        //        var convParams = new Dictionary<string, object>
        //{
        //    { "@LetterRef", data.LetterRef },
        //    { "@CentralRef", data.CentralRef },
        //    { "@ConversationText", "Image" }
        //};

        //        var dtConv = await _dbService.ExecSupportQueryAsync(HttpContext, insertConversationQuery, convParams);
        //        if (dtConv.Rows.Count == 0)
        //            return BadRequest("Failed to insert conversation");

        //        string conversationRef = dtConv.Rows[0]["ConversationCode"].ToString();

        //        // 2. Decode image and save file
        //        byte[] decodedImage = Convert.FromBase64String(data.image);
        //        string folderPath = _configuration.GetConnectionString("web_imagePath");
        //        if (string.IsNullOrEmpty(folderPath))
        //            return StatusCode(500, "Image storage path is not configured");

        //        string filePath = Path.Combine(folderPath, $"{conversationRef}.jpg");
        //        await System.IO.File.WriteAllBytesAsync(filePath, decodedImage);

        //        // 3. Insert image record in DB
        //        string insertImageQuery = "Exec spImageImport @ClassName, @ConversationRef, @FilePath; SELECT @@IDENTITY KsrImageCode";
        //        var imageParams = new Dictionary<string, object>
        //{
        //    { "@ClassName", data.ClassName },
        //    { "@ConversationRef", conversationRef },
        //    { "@FilePath", filePath }
        //};

        //        var dtImage = await _dbService.ExecImageExecQueryAsync(insertImageQuery, imageParams);

        //        var json = _jsonFormatter.JsonResultWithout_Str(dtImage);
        //        return Content(json, "application/json");
        //    }
        //    catch (FormatException)
        //    {
        //        return BadRequest("Invalid image format");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error in Conversation_UploadImage");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}



        //[HttpGet("GetWebImagess")]
        //public async Task<IActionResult> GetWebImagess(string pixelScale, string ClassName, string ObjectRef)
        //{
        //    try
        //    {
        //        // تبدیل پارامترها به مقدار مناسب با اعتبارسنجی اولیه
        //        if (!int.TryParse(pixelScale, out int scale))
        //            return BadRequest("Invalid pixelScale");

        //        if (!int.TryParse(ObjectRef, out int objectRef))
        //            return BadRequest("Invalid ObjectRef");

        //        string query = "SELECT * FROM KsrImage WHERE Classname = @ClassName AND ObjectRef = @ObjectRef ORDER BY 1 DESC";

        //        var parameters = new Dictionary<string, object>
        //{
        //    { "@ClassName", ClassName },
        //    { "@ObjectRef", objectRef }
        //};

        //        var dt = await _dbService.ExecImageExecQueryAsync(query, parameters);

        //        string base64Images = _jsonFormatter.ConvertAndScaleImageToBase64(scale, dt);

        //        return Content(base64Images, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error in GetWebImagess");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}


        [HttpPost("KowsarAttachFile")]
        public async Task<IActionResult> KowsarAttachFile([FromBody] SearchTargetDto searchTarget)
        {
            try
            {
                if (string.IsNullOrEmpty(searchTarget.SearchTarget))
                    return BadRequest("SearchTarget is required");

                string query = "spWeb_SearchAttachFile @SearchTarget";

                var parameters = new Dictionary<string, object>
        {
            { "@SearchTarget", SanitizeInput(searchTarget.SearchTarget) }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);

                string jsonResult = _jsonFormatter.JsonResultWithout_Str(dt);

                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in KowsarAttachFile");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("KowsarAttachUrl")]
        public async Task<IActionResult> KowsarAttachUrl([FromBody] SearchTargetDto searchTarget)
        {
            try
            {
                if (string.IsNullOrEmpty(searchTarget.SearchTarget))
                    return BadRequest("SearchTarget is required");

                string query = "spWeb_SearchAttachFile @SearchTarget, @Type";

                var parameters = new Dictionary<string, object>
        {
            { "@SearchTarget", SanitizeInput(searchTarget.SearchTarget) },
            { "@Type", "URL" }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);

                string jsonResult = _jsonFormatter.JsonResultWithout_Str(dt);

                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in KowsarAttachUrl");
                return StatusCode(500, "Internal server error");
            }
        }

        //[HttpPost("SetAttachFile")]
        //public async Task<IActionResult> SetAttachFile([FromBody] AttachFile attachFile)
        //{
        //    try
        //    {
        //        if (attachFile == null)
        //            return BadRequest("AttachFile is required");

        //        if (attachFile.Type == "URL")
        //        {
        //            // اجرای پروسیجر ذخیره URL بصورت امن
        //            string query = "exec spWeb_AttachFile @Title, @FileName, @ClassName, @Type, @FilePath, @EmptyParam";

        //            var parameters = new Dictionary<string, object>
        //    {
        //        { "@Title", attachFile.Title },
        //        { "@FileName", attachFile.FileName },
        //        { "@ClassName", attachFile.ClassName },
        //        { "@Type", attachFile.Type },
        //        { "@FilePath", attachFile.FilePath ?? string.Empty },
        //        { "@EmptyParam", string.Empty }
        //    };

        //            DataTable dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);

        //            string jsonResult = _jsonFormatter.JsonResultWithout_Str(dt);
        //            return Content(jsonResult, "application/json");
        //        }
        //        else
        //        {
        //            // ذخیره فایل باینری
        //            byte[] fileBytes = Convert.FromBase64String(attachFile.Data);
        //            string fileName = $"{attachFile.FileName}.{attachFile.FileType}";
        //            string zipFileName = $"{attachFile.FileName}.zip";

        //            string ocrImagePath = _configuration.GetConnectionString("Ocr_imagePath");
        //            string filePath = Path.Combine(ocrImagePath, fileName);
        //            string zipFilePath = Path.Combine(ocrImagePath, zipFileName);

        //            await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

        //            // ساخت فایل زیپ حاوی فایل ذخیره شده
        //            using (var zipStream = new FileStream(zipFilePath, FileMode.Create))
        //            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
        //            {
        //                archive.CreateEntryFromFile(filePath, fileName);
        //            }

        //            string connectionString = _configuration.GetConnectionString("Support_Connection");
        //            string dbName = "";

        //            using (var dbConnection = new SqlConnection(connectionString))
        //            {
        //                await dbConnection.OpenAsync();

        //                string queryDbName = attachFile.ClassName switch
        //                {
        //                    "AutLetter" => $"DECLARE @db nvarchar(100)=''; SELECT @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '') FROM FiscalPeriod p JOIN AutLetter aut ON PeriodId=PeriodRef WHERE LetterCode= {attachFile.ObjectRef}; SELECT @db dbname",
        //                    "Factor" => $"DECLARE @db nvarchar(100)=''; SELECT @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '') FROM FiscalPeriod p JOIN Factor f ON PeriodId=PeriodRef WHERE FactorCode= {attachFile.ObjectRef}; SELECT @db dbname",
        //                    _ => $"DECLARE @dbname nvarchar(200)=db_name()+'Ocr'; SELECT @dbname dbname"
        //                };

        //                using (var cmd = new SqlCommand(queryDbName, dbConnection))
        //                {
        //                    var reader = await cmd.ExecuteReaderAsync();
        //                    if (await reader.ReadAsync())
        //                    {
        //                        dbName = reader["dbname"].ToString();
        //                    }
        //                    await reader.CloseAsync();
        //                }

        //                string insertQuery = $@"
        //            INSERT INTO {dbName}.dbo.AttachedFiles
        //            (Title, ClassName, ObjectRef, FileName, SourceFile, Type, Owner, CreationDate, Reformer, ReformDate, FilePath)
        //            VALUES
        //            (@Title, @ClassName, @ObjectRef, @FileName, @SourceFile, @Type, -1000, GETDATE(), -1000, GETDATE(), @FilePath)";

        //                using (var insertCmd = new SqlCommand(insertQuery, dbConnection))
        //                {
        //                    insertCmd.Parameters.AddWithValue("@Title", attachFile.Title);
        //                    insertCmd.Parameters.AddWithValue("@ClassName", attachFile.ClassName);
        //                    insertCmd.Parameters.AddWithValue("@ObjectRef", attachFile.ObjectRef);
        //                    insertCmd.Parameters.AddWithValue("@FileName", attachFile.FileName);
        //                    insertCmd.Parameters.AddWithValue("@SourceFile", await System.IO.File.ReadAllBytesAsync(zipFilePath));
        //                    insertCmd.Parameters.AddWithValue("@Type", attachFile.Type);
        //                    insertCmd.Parameters.AddWithValue("@FilePath", attachFile.FilePath ?? string.Empty);

        //                    await insertCmd.ExecuteNonQueryAsync();
        //                }
        //            }

        //            // حذف فایل‌ها بعد از ذخیره
        //            System.IO.File.Delete(filePath);
        //            System.IO.File.Delete(zipFilePath);

        //            // بازگرداندن تاریخ امروز از سرور
        //            string queryToday = "select dbo.fnDate_Today() TodayFromServer";
        //            DataTable dtToday = await _dbService.ExecSupportQueryAsync(HttpContext, queryToday);

        //            string jsonToday = _jsonFormatter.JsonResult_Str(dtToday, "Text", "TodayFromServer");

        //            return Content(jsonToday, "application/json");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error in SetAttachFile");
        //        return StatusCode(500, "Internal server error: " + ex.Message);
        //    }
        //}








        [HttpPost("GetAttachFileList")]
        public async Task<IActionResult> GetAttachFileList([FromBody] AttachFile attachFile)
        {
            try
            {
                if (attachFile == null || string.IsNullOrEmpty(attachFile.ClassName))
                    return BadRequest("Invalid request data");

                string queryDb = attachFile.ClassName switch
                {
                    "AutLetter" => @$"
                DECLARE @db nvarchar(100) = '';
                SELECT @db = db_name() + 'Ocr' + REPLACE(FromDate, '/', '')
                FROM FiscalPeriod p
                JOIN AutLetter aut ON PeriodId = PeriodRef
                WHERE LetterCode = {attachFile.ObjectRef};
                SELECT @db AS dbname;",

                    "Factor" => @$"
                DECLARE @db nvarchar(100) = '';
                SELECT @db = db_name() + 'Ocr' + REPLACE(FromDate, '/', '')
                FROM FiscalPeriod p
                JOIN Factor f ON PeriodId = PeriodRef
                WHERE FactorCode = {attachFile.ObjectRef};
                SELECT @db AS dbname;",

                    _ => "DECLARE @dbname nvarchar(200) = db_name() + 'Ocr'; SELECT @dbname AS dbname;"
                };

                var dbNameTable = await _dbService.ExecSupportQueryAsync(HttpContext, queryDb);
                if (dbNameTable.Rows.Count == 0)
                    return NotFound("Database name could not be determined.");

                string dbName = dbNameTable.Rows[0]["dbname"].ToString();

                string query = $@"
            SELECT * FROM {dbName}.dbo.AttachedFiles
            WHERE ClassName = @ClassName AND ObjectRef = @ObjectRef";

                var parameters = new Dictionary<string, object>
        {
            { "@ClassName", attachFile.ClassName },
            { "@ObjectRef", attachFile.ObjectRef }
        };

                var resultTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);

                string json = _jsonFormatter.JsonResult_Str(resultTable, "AttachedFiles", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAttachFileList");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }






        [HttpGet("GetAttachFile")]
        public async Task<IActionResult> GetAttachFile(string attachedFileCode, string className, string objectRef)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(attachedFileCode) || string.IsNullOrWhiteSpace(className) || string.IsNullOrWhiteSpace(objectRef))
                    return BadRequest("Invalid input");

                string queryDb = className switch
                {
                    "AutLetter" => @$"
                DECLARE @db nvarchar(100) = '';
                SELECT @db = db_name() + 'Ocr' + REPLACE(FromDate, '/', '')
                FROM FiscalPeriod p
                JOIN AutLetter aut ON PeriodId = PeriodRef
                WHERE LetterCode = {objectRef};
                SELECT @db AS dbname;",

                    "Factor" => @$"
                DECLARE @db nvarchar(100) = '';
                SELECT @db = db_name() + 'Ocr' + REPLACE(FromDate, '/', '')
                FROM FiscalPeriod p
                JOIN Factor f ON PeriodId = PeriodRef
                WHERE FactorCode = {objectRef};
                SELECT @db AS dbname;",

                    _ => "DECLARE @dbname nvarchar(200) = db_name() + 'Ocr'; SELECT @dbname AS dbname;"
                };

                var dbNameTable = await _dbService.ExecSupportQueryAsync(HttpContext, queryDb);
                if (dbNameTable.Rows.Count == 0)
                    return NotFound("Database name not resolved.");

                string dbName = dbNameTable.Rows[0]["dbname"].ToString();

                string queryFile = $"spWeb_GetAttachFile @AttachedFileCode, @DbName";
                var parameters = new Dictionary<string, object>
        {
            { "@AttachedFileCode", attachedFileCode },
            { "@DbName", dbName }
        };

                var resultTable = await _dbService.ExecSupportQueryAsync(HttpContext, queryFile, parameters);
                if (resultTable.Rows.Count == 0)
                    return NotFound("File not found.");

                string base64File = resultTable.Rows[0]["SourceFile"].ToString();
                string fileName = resultTable.Rows[0]["FileName"].ToString();
                string fileType = resultTable.Rows[0]["Type"].ToString();

                byte[] fileBytes = Convert.FromBase64String(base64File);
                string contentType = $"application/{fileType}";

                return File(fileBytes, contentType, $"{fileName}.zip");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving attached file.");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet("GetNotification")]
        public async Task<IActionResult> GetNotification(string PersonInfoCode)
        {
            try
            {
                if (string.IsNullOrEmpty(PersonInfoCode))
                    return BadRequest("PersonInfoCode is required");

                string query = "spWeb_GetNotification @PersonInfoCode";
                var parameters = new Dictionary<string, object>
        {
            { "@PersonInfoCode", PersonInfoCode }
        };

                var dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string jsonResult = _jsonFormatter.JsonResult_Str(dataTable, "users", "");
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetNotification");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("EditFactorProperty")]
        public async Task<IActionResult> EditFactorProperty([FromBody] FactorwebDto factorwebDto)
        {
            try
            {
                if (factorwebDto == null)
                    return BadRequest("Invalid input");

                string query = "spWeb_EditFactorProperty @StartTime, @EndTime, @WorkTime, @Barbary, @ObjectRef";

                var parameters = new Dictionary<string, object>
        {
            { "@StartTime", factorwebDto.starttime },
            { "@EndTime", factorwebDto.Endtime },
            { "@WorkTime", factorwebDto.worktime },
            { "@Barbary", SanitizeInput(factorwebDto.Barbary )},
            { "@ObjectRef", factorwebDto.ObjectRef }
        };

                var dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string jsonResult = _jsonFormatter.JsonResult_Str(dataTable, "Factors", "");
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EditFactorProperty");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("EditCustomerProperty")]
        public async Task<IActionResult> EditCustomerProperty([FromBody] CustomerWebDto customerWebDto)
        {
            try
            {
                if (customerWebDto == null)
                    return BadRequest("Invalid input");

                string query = "spWeb_EditCustomerProperty @AppNumber, @DatabaseNumber, @Delegacy, @ObjectRef";

                var parameters = new Dictionary<string, object>
        {
            { "@AppNumber", SanitizeInput(customerWebDto.AppNumber) },
            { "@DatabaseNumber", SanitizeInput(customerWebDto.DatabaseNumber) },
            { "@Delegacy", SanitizeInput(customerWebDto.Delegacy) },
            { "@ObjectRef", SanitizeInput(customerWebDto.ObjectRef) }
        };

                var dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string jsonResult = _jsonFormatter.JsonResult_Str(dataTable, "Customers", "");

                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EditCustomerProperty");
                return StatusCode(500, "Internal server error");
            }
        }






        [HttpPost("EditCustomerExplain")]
        public async Task<IActionResult> EditCustomerExplain([FromBody] CustomerWebDto customerWebDto)
        {
            try
            {
                if (customerWebDto == null)
                    return BadRequest("Invalid input");

                string query = "UPDATE Customer SET Explain = @Explain WHERE CustomerCode = @CustomerCode";

                var parameters = new Dictionary<string, object>
        {
            { "@Explain",       SanitizeInput(customerWebDto.Explain)},
            { "@CustomerCode", customerWebDto.ObjectRef }
        };

                var dataTable = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string jsonResult = _jsonFormatter.JsonResult_Str(dataTable, "Customers", "");

                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EditCustomerExplain");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("GetWebFactorSupport")]
        public async Task<IActionResult> GetWebFactorSupport(string factorCode)
        {
            try
            {
                if (!int.TryParse(factorCode, out int code))
                    return BadRequest("Invalid FactorCode");

                string query = @"SELECT FactorCode, FactorDate, CustName, CustomerCode, Explain, BrokerRef, BrokerName, 
                         starttime, Endtime, worktime, Barbary 
                         FROM vwFactor 
                         WHERE FactorCode = @FactorCode";

                var parameters = new Dictionary<string, object>
        {
            { "@FactorCode", code }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetWebFactorSupport");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpGet("GetWebFactorRowsSupport")]
        public async Task<IActionResult> GetWebFactorRowsSupport(string factorCode)
        {
            try
            {
                if (!int.TryParse(factorCode, out int code))
                    return BadRequest("Invalid FactorCode");

                string query = "SELECT FactorRowCode, GoodName FROM vwFactorRows WHERE Factorref = @FactorCode";

                var parameters = new Dictionary<string, object>
        {
            { "@FactorCode", code }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetWebFactorRowsSupport");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpDelete("DeleteWebFactorRowsSupport")]
        public async Task<IActionResult> DeleteWebFactorRowsSupport(string factorRowCode)
        {
            try
            {
                if (!int.TryParse(factorRowCode, out int rowCode))
                    return BadRequest("Invalid FactorRowCode");

                string query = "DELETE FROM FactorRows WHERE FactorRowCode = @FactorRowCode";

                var parameters = new Dictionary<string, object>
        {
            { "@FactorRowCode", rowCode }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteWebFactorRowsSupport");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteWebFactorSupport")]
        public async Task<IActionResult> DeleteWebFactorSupport(string factorCode)
        {
            try
            {
                if (!int.TryParse(factorCode, out int code))
                    return BadRequest("Invalid FactorCode");

                string query = "DELETE FROM Factor WHERE FactorCode = @FactorCode";

                var parameters = new Dictionary<string, object>
        {
            { "@FactorCode", code }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteWebFactorSupport");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("GetGoodListSupport")]
        public async Task<IActionResult> GetGoodListSupport([FromBody] SearchTargetDto searchTargetDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTargetDto.SearchTarget))
                    return BadRequest("SearchTarget is required");

                string query = "spWeb_GetGoodListSupport @SearchTarget";

                var parameters = new Dictionary<string, object>
        {
            { "@SearchTarget", SanitizeInput(searchTargetDto.SearchTarget) }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetGoodListSupport");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("WebSupportFactorInsert")]
        public async Task<IActionResult> WebSupportFactorInsert([FromBody] FactorwebDto factorwebDto)
        {
            try
            {
                string userId = _configuration.GetValue<string>("AppSettings:Support_UserId");

                string query = @"spWeb_Factor_Insert  
                         @ClassName = 'Factor',
                         @StackRef = 1,
                         @UserId = @UserId,
                         @Date = @Date,
                         @Customer = @Customer,
                         @Explain = @Explain,
                         @BrokerRef = @BrokerRef,
                         @IsShopFactor = 0";

                var parameters = new Dictionary<string, object>
        {
            { "@UserId", userId },
            { "@Date", SanitizeInput(factorwebDto.FactorDate) },
            { "@Customer", SanitizeInput(factorwebDto.CustomerCode) },
            { "@Explain", SanitizeInput(factorwebDto.Explain)  },
            { "@BrokerRef", SanitizeInput(factorwebDto.BrokerRef)  }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in WebSupportFactorInsert");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("WebSupportFactorInsertRow")]
        public async Task<IActionResult> WebSupportFactorInsertRow([FromBody] FactorRow factorRow)
        {
            try
            {
                string query = @"spWeb_Factor_InsertRow  
                         @ClassName = 'Factor',
                         @FactorCode = @FactorCode,
                         @GoodRef = @GoodRef,
                         @Amount = 1,
                         @Price = 0,
                         @UserId = @UserId,
                         @MustHasAmount = 0,
                         @MergeFlag = 1";

                var parameters = new Dictionary<string, object>
        {
            { "@FactorCode", factorRow.FactorRef },
            { "@GoodRef", factorRow.GoodRef },
            { "@UserId", 29 } // بهتر است از کانفیگ یا توکن بگیری نه هاردکد
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in WebSupportFactorInsertRow");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpPost("Support_StartFactorTime")]
        public async Task<IActionResult> StartFactorTime([FromBody] FactorwebDto factorwebDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(factorwebDto.starttime))
                    return BadRequest("Invalid input");

                string query = @"UPDATE PropertyValue 
                         SET Nvarchar15 = @StartTime 
                         WHERE ClassName = 'TFactor' AND ObjectRef = @ObjectRef";

                var parameters = new Dictionary<string, object>
        {
            { "@StartTime",SanitizeInput( factorwebDto.starttime) },
            { "@ObjectRef", SanitizeInput(factorwebDto.ObjectRef) }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StartFactorTime");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Support_EndFactorTime")]
        public async Task<IActionResult> EndFactorTime([FromBody] FactorwebDto factorwebDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(factorwebDto.Endtime))
                    return BadRequest("Invalid input");

                string query = @"UPDATE PropertyValue 
                         SET Nvarchar9 = @EndTime, int1 = @WorkTime 
                         WHERE ClassName = 'TFactor' AND ObjectRef = @ObjectRef";

                var parameters = new Dictionary<string, object>
        {
            { "@EndTime", SanitizeInput(factorwebDto.Endtime) },
            { "@WorkTime", SanitizeInput(factorwebDto.worktime) },
            { "@ObjectRef",SanitizeInput( factorwebDto.ObjectRef) }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EndFactorTime");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpPost("Support_ExplainFactor")]
        public async Task<IActionResult> Support_ExplainFactor([FromBody] FactorwebDto factorwebDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(factorwebDto.Barbary))
                    return BadRequest("Invalid input");

                string query = @"UPDATE PropertyValue 
                         SET Nvarchar14 = @Barbary 
                         WHERE ClassName = 'TFactor' AND ObjectRef = @ObjectRef";

                var parameters = new Dictionary<string, object>
        {
            { "@Barbary", SanitizeInput(factorwebDto.Barbary) },
            { "@ObjectRef", SanitizeInput(factorwebDto.ObjectRef) }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Support_ExplainFactor");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Support_Count")]
        public async Task<IActionResult> Support_Count([FromBody] FactorwebDto factorwebDto)
        {
            try
            {
                string query = @"
            DECLARE @S nvarchar(20) = dbo.fnDate_AddDays(dbo.fnDate_Today(), -365);
            SELECT 
                BrokerCode, 
                BrokerName, 
                SUM(worktime) / 60 AS worktime,
                CAST(SUM(SumAmount) AS int) AS SumAmount,
                COUNT(*) AS FactorCount
            FROM vwFactor 
            WHERE FactorDate > @S
            GROUP BY BrokerName, BrokerCode";

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, null);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Support_Count");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("GetGridSchema")]
        public async Task<IActionResult> GetGridSchema(string where)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(where))
                    return BadRequest("Where parameter is required.");

                string query = "SELECT * FROM [dbo].[fnGetGridSchema](@Where) WHERE Visible = 1";

                var parameters = new Dictionary<string, object>
        {
            { "@Where", SanitizeInput(where) }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "GridSchemas", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetGridSchema");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("GetFactors")]
        public async Task<IActionResult> GetFactors([FromBody] SearchTargetDto searchTargetDto)
        {
            try
            {
                string query = "Exec spWeb_GetFactor";

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, null);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetFactors");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpPost("GetSupportFactors")]
        public async Task<IActionResult> GetSupportFactors([FromBody] FactorwebDto factorwebDto)
        {
            try
            {
                string query = "Exec spWeb_GetSupportFactor @StartDateTarget, @EndDateTarget, @SearchTarget, @BrokerRef, @IsShopFactor";

                var parameters = new Dictionary<string, object?>
        {
            { "@StartDateTarget", SanitizeInput(factorwebDto.StartDateTarget) },
            { "@EndDateTarget", SanitizeInput(factorwebDto.EndDateTarget) },
            { "@SearchTarget", SanitizeInput(factorwebDto.SearchTarget )},
            { "@BrokerRef", SanitizeInput(factorwebDto.BrokerRef) },
            { "@IsShopFactor", SanitizeInput(factorwebDto.isShopFactor) }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSupportFactors");
                return StatusCode(500, "Internal server error");
            }
        }






        [HttpPost("WebFactorInsert")]
        public async Task<IActionResult> WebFactorInsert([FromBody] FactorwebDto factorwebDto)
        {
            try
            {
                string userId = _configuration.GetValue<string>("AppSettings:Support_UserId");

                string query = "spWeb_Factor_Insert @ClassName, @StackRef, @UserId, @Date, @Customer, @Explain, @BrokerRef, @IsShopFactor";

                var parameters = new Dictionary<string, object?>
        {
            { "@ClassName", SanitizeInput(factorwebDto.ClassName) },
            { "@StackRef", SanitizeInput(factorwebDto.StackRef) },
            { "@UserId", SanitizeInput(userId) },
            { "@Date", SanitizeInput(factorwebDto.FactorDate) },
            { "@Customer", SanitizeInput(factorwebDto.CustomerCode) },
            { "@Explain", SanitizeInput(factorwebDto.Explain )},
            { "@BrokerRef", SanitizeInput(factorwebDto.BrokerRef )},
            { "@IsShopFactor", SanitizeInput(factorwebDto.isShopFactor) }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in WebFactorInsert");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("WebFactorInsertRow")]
        public async Task<IActionResult> WebFactorInsertRow([FromBody] FactorRow factorRow)
        {
            try
            {
                string userId = _configuration.GetValue<string>("AppSettings:Support_UserId");



                string query = "spWeb_Factor_InsertRow @ClassName, @FactorCode, @GoodRef, @Amount, @Price, @UserId, @MustHasAmount, @MergeFlag";

                var parameters = new Dictionary<string, object?>
        {
            { "@ClassName", SanitizeInput(factorRow.ClassName) },
            { "@FactorCode", SanitizeInput(factorRow.FactorRef )},
            { "@GoodRef", SanitizeInput(factorRow.GoodRef) },
            { "@Amount", SanitizeInput(factorRow.Amount) },
            { "@Price", SanitizeInput(factorRow.Price) },
            { "@UserId", SanitizeInput(userId) },
            { "@MustHasAmount", SanitizeInput(factorRow.MustHasAmount) },
            { "@MergeFlag", SanitizeInput(factorRow.MergeFlag) }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in WebFactorInsertRow");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("GetCustomerFactor")]
        public async Task<IActionResult> GetCustomerFactor([FromQuery] string where)
        {
            try
            {
                // اگر لازم هست sanitization یا validation انجام بدی، اینجا اضافه کن.

                string query = "spWeb_GetCustomerFactor @Where";

                var parameters = new Dictionary<string, object?>
        {
            { "@Where",SanitizeInput( where) }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCustomerFactor");
                return StatusCode(500, "Internal server error");
            }
        }





        [HttpPost("GetSupportData")]
        public async Task<IActionResult> GetSupportData([FromBody] SupportDto supportDto)
        {
            try
            {

                // 1 support panel
                // 2 EmptyEndTimeCount




                string query = "spWeb_SupportData @DateTarget, @BrokerCode, @Flag";

                var parameters = new Dictionary<string, object?>
        {
            { "@DateTarget", SanitizeInput(supportDto.DateTarget) },
            { "@BrokerCode", supportDto.BrokerCode },
            { "@Flag", supportDto.Flag }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string json = _jsonFormatter.JsonResult_Str(dt, "SupportDatas", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSupportData");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("ManualAttendance")]
        public async Task<IActionResult> ManualAttendance([FromBody] ManualAttendance manualAttendance)
        {
            try
            {

                // 0 ghayeb 
                // 1 hozor
                // 2 mashghol


                string userId = _configuration.GetValue<string>("AppSettings:Support_UserId");

                string query = "spWeb_Attendance_ManualInsert @CentralRef, @Status";

                var parameters = new Dictionary<string, object?>
        {
            { "@CentralRef", manualAttendance.CentralRef },
            { "@Status", manualAttendance.Status }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);

                string jsonResult = _jsonFormatter.JsonResult_Str(dt, "Attendances", "");
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ManualAttendance");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("AttendanceDashboard")]
        public async Task<IActionResult> AttendanceDashboard()
        {
            try
            {
                string query = "spWeb_Attendance_Dashboard";
                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query);
                string jsonResult = _jsonFormatter.JsonResult_Str(dt, "Attendances", "");
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AttendanceDashboard");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("AttendanceHistory")]
        public async Task<IActionResult> AttendanceHistory([FromQuery] int CentralRef)
        {
            try
            {
                string query = "spWeb_Attendance_History @CentralRef";
                var parameters = new Dictionary<string, object?>
        {
            { "@CentralRef", CentralRef }
        };
                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string jsonResult = _jsonFormatter.JsonResult_Str(dt, "Attendances", "");
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AttendanceHistory");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("DeleteAutLetterRows")]
        public async Task<IActionResult> DeleteAutLetterRows([FromQuery] int LetterRowCode)
        {
            try
            {
                string query = "Delete From AutLetterRow where LetterRowCode = @LetterRowCode";
                var parameters = new Dictionary<string, object?>
        {
            { "@LetterRowCode", LetterRowCode }
        };
                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string jsonResult = _jsonFormatter.JsonResult_Str(dt, "AutLetters", "");
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteAutLetterRows");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpGet("DeleteAutLetter")]
        public async Task<IActionResult> DeleteAutLetter([FromQuery] int LetterCode)
        {
            try
            {
                string query = "Delete From AutLetter where LetterCode = @LetterCode";
                var parameters = new Dictionary<string, object>
        {
            { "@LetterCode", LetterCode }
        };
                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string jsonResult = _jsonFormatter.JsonResult_Str(dt, "AutLetters", "");
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteAutLetter");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetGoodBase")]
        public async Task<IActionResult> GetGoodBase([FromQuery] int GoodCode)
        {
            try
            {
                string query = "spWeb_GetGoodById @GoodCode, 0";
                var parameters = new Dictionary<string, object>
        {
            { "@GoodCode", GoodCode }
        };
                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string jsonResult = _jsonFormatter.JsonResult_Str(dt, "Goods", "");
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetGoodBase");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetLastGoodData")]
        public async Task<IActionResult> GetLastGoodData()
        {
            try
            {
                string query = @"
            DECLARE @ss INT;
            SELECT @ss = MAX(GoodCode) FROM Good;
            EXEC spWeb_GetGoodById @ss, 0;
        ";

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query);
                string jsonResult = _jsonFormatter.JsonResult_Str(dt, "Goods", "");
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLastGoodData");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("GoodCrudService")]
        public async Task<IActionResult> GoodCrudService([FromBody] JsonModelDto jsonModelDto)
        {
            try
            {
                string query = "Exec spGood_AddNew @JsonData";
                var parameters = new Dictionary<string, object>
        {
            { "@JsonData", jsonModelDto.JsonData }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string jsonResult = _jsonFormatter.JsonResult_Str(dt, "Goods", "");
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GoodCrudService");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("GetSimilarGood")]
        public async Task<IActionResult> GetSimilarGood([FromQuery] string where)
        {
            try
            {
                string query = "Select top 5 GoodCode, GoodType, GoodName, Type, UsedGood, MinSellPrice, MaxSellPrice, BarCodePrintState, SellPriceType, SellPrice1, SellPrice2, SellPrice3, SellPrice4, SellPrice5, SellPrice6 " +
                               "From Good where GoodName like @SearchPattern";

                var parameters = new Dictionary<string, object>
        {
            { "@SearchPattern", "%" + SanitizeInput(where) + "%" }
        };

                var dt = await _dbService.ExecSupportQueryAsync(HttpContext, query, parameters);
                string jsonResult = _jsonFormatter.JsonResult_Str(dt, "Goods", "");
                return Content(jsonResult, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSimilarGood");
                return StatusCode(500, "Internal server error");
            }
        }




        private string SanitizeInput(string input)
        {
            if (input == null)
                return string.Empty;

            // Prevent SQL Injection by replacing dangerous characters
            input = input.Replace("'", "''");  // Escape single quotes for SQL
            input = input.Replace(";", "");    // Remove semicolons
            input = input.Replace("--", "");   // Remove SQL comments
            input = input.Replace("/*", "");   // Remove SQL block comments
            input = input.Replace("*/", "");   // Remove SQL block comments

            // Prevent XSS by replacing HTML-sensitive characters with their HTML-encoded equivalents
            input = input.Replace("<", "&lt;"); // < becomes &lt;
            input = input.Replace(">", "&gt;"); // > becomes &gt;
            input = input.Replace("&", "&amp;"); // & becomes &amp;
            input = input.Replace("\"", "&quot;"); // " becomes &quot;
            input = input.Replace("'", "&#x27;"); // ' becomes &#x27;
            input = input.Replace("/", "&#x2F;"); // / becomes &#x2F;
            input = input.Replace("\\", "&#x5C;"); // \ becomes &#x5C;

            // Remove leading/trailing whitespace
            input = input.Trim();

            return input;
        }




    }
}
