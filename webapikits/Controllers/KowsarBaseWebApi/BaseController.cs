 using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Net;
using webapikits.Controllers.InternalWebApi;
using webapikits.Controllers.InternalWebApi;
using webapikits.Model;
using webapikits.Model;
using static webapikits.Model.LoginUserDto;

namespace webapikits.Controllers.KowsarBaseWebApi
{
    [Route("api/[controller]")]
    [ApiController]

public class BaseController : ControllerBase
    {

        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<BaseController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();

        public BaseController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<BaseController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
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



        [HttpPost("GetKowsarCustomer")]
        public async Task<IActionResult> GetKowsarCustomer([FromBody] SearchTargetDto searchTargetDto)
        {

            string query = $"Exec [dbo].[spWeb_GetCustomer] '{searchTargetDto.SearchTarget}',{searchTargetDto.Active} ";


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

        [HttpGet]
        [Route("GetWebImagess")]
        public async Task<string> GetWebImagess(string pixelScale, string ClassName, string ObjectRef)

        {


            string query = $"SELECT *  FROM KsrImage WHERE Classname = '{ClassName}' AND ObjectRef = {ObjectRef} order by 1 desc";



            DataTable dataTable = await db.Image_ExecQuery(HttpContext, query);
            return jsonClass.ConvertAndScaleImageToBase64(Convert.ToInt32(pixelScale), dataTable);

        }

        [HttpGet]
        [Route("GetLookup")]
        public async Task<IActionResult> GetLookup(string SearchTarget)
        {

            string query = $"Select * From Lookup Where Name ='{SearchTarget}'  ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Lookups", ""); ;
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetLookup));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetTodeyFromServer")]
        public async Task<IActionResult> GetTodeyFromServer()
        {

            string query = "select dbo.fnDate_Today() TodeyFromServer ";




            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetTodeyFromServer));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("GetAllGridSchema")]
        public async Task<IActionResult> GetAllGridSchema(string className)
        {
            var rawUserId = HttpContext.Request.Headers["UI"].FirstOrDefault();
            var userId = string.IsNullOrWhiteSpace(rawUserId) ? "1" : WebUtility.UrlDecode(rawUserId);

            string query = userId == "1"
                ? "SELECT * FROM [dbo].[fnGetGridSchema](@ClassName)"
                : "SELECT * FROM [dbo].[fnGetGridSchema_User](@ClassName, @UserId)";

            var parameters = new Dictionary<string, object>
    {
        { "@ClassName", className },
    };

            if (userId != "1")
                parameters.Add("@UserId", userId);

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query, parameters);

                // اگر userId != 1 و دیتاتابل خالی بود، دوباره با userId = 0 فراخوانی کن
                if (userId != "1" && (dataTable == null || dataTable.Rows.Count == 0))
                {
                    parameters["@UserId"] = "0";
                    dataTable = await db.Kowsar_ExecQuery(HttpContext, query, parameters);
                }

                string json = jsonClass.JsonResult_Str(dataTable, "GridSchemas", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAllGridSchema));
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("GetGridSchemaVisible")]
        public async Task<IActionResult> GetGridSchemaVisible(string className)
        {
            var rawUserId = HttpContext.Request.Headers["UI"].FirstOrDefault();
            var userId = string.IsNullOrWhiteSpace(rawUserId) ? "1" : WebUtility.UrlDecode(rawUserId);

            string query = userId == "1"
                ? "SELECT * FROM [dbo].[fnGetGridSchema](@ClassName) WHERE Visible = 1"
                : "SELECT * FROM [dbo].[fnGetGridSchema_User](@ClassName, @UserId) WHERE Visible = 1";

            var parameters = new Dictionary<string, object>
    {
        { "@ClassName", className },
    };

            if (userId != "1")
                parameters.Add("@UserId", userId);

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query, parameters);

                // اگر userId != 1 و دیتاتابل خالی بود، دوباره با userId = 0 فراخوانی کن
                if (userId != "1" && (dataTable == null || dataTable.Rows.Count == 0))
                {
                    parameters["@UserId"] = "0";
                    dataTable = await db.Kowsar_ExecQuery(HttpContext, query, parameters);
                }

                string json = jsonClass.JsonResult_Str(dataTable, "GridSchemas", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGridSchemaVisible));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpGet]
        [Route("GetObjectTypeFromDbSetup")]
        public async Task<IActionResult> GetObjectTypeFromDbSetup(string ObjectType)
        {

            string query = "select * from dbo.fnObjectType('" + ObjectType + "') ";




            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "ObjectTypes", ""); ;
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetObjectTypeFromDbSetup));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("GetApplicationForMenu")]
        public async Task<IActionResult> GetApplicationForMenu()
        {

            string query = $"select KeyValue,Description,DataValue,KeyId from dbsetup where KeyValue in ('AppBroker_ActivationCode','AppOcr_ActivationCode','AppOrder_ActivationCode') and DataValue <> '0'";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "applications", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetApplicationForMenu));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpPost]
        [Route("PropertyValueCrudService")]
        public async Task<IActionResult> PropertyValueCrudService([FromBody] JsonModelDto jsonModelDto)

        {


            string query = $"Exec    spPropertyValue_AddNew '{jsonModelDto.JsonData}' ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PropertyValues", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PropertyValueCrudService));
                return StatusCode(500, "Internal server error.");
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



            DataTable dataTable4 = await db.Kowsar_ExecQuery(HttpContext, query11);
            dbname = dataTable4.Rows[0]["dbname"] + "";



            string query1 = $"Delete From {dbname}..AttachedFiles where ClassName = '{ClassName}' And AttachedFileCode = {AttachedFileCode} ";




            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query1);
                string json = jsonClass.JsonResult_Str(dataTable, "AttachFiles", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteAttachFile));
                return StatusCode(500, "Internal server error.");
            }






        }





        [HttpPost]
        [Route("GetPropertyValue")]
        public async Task<IActionResult> GetPropertyValue([FromBody] PropertyDto propertyDto)
        {

            string query = $"select dbo.NodeValue(PropertySchema, 'DisplayName') DisplayName, PropertySchemaCode,PropertySchema,ClassName,ObjectType,PropertyName,PropertySequence" +
                $",PropertyType,PropertyValueMap From PropertySchema p where  p.ClassName ='{propertyDto.ClassName}'  ";
                
                if (propertyDto.ObjectType.Length > 0) {
                query = query + $" And p.ObjectType ='{propertyDto.ObjectType}' ";
                }

                query = query + $"order by PropertySequence";

            

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PropertyValues", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetPropertyValue));
                return StatusCode(500, "Internal server error.");
            }

        }

        


        [HttpPost]
        [Route("GetPropertyValueData")]
        public async Task<IActionResult> GetPropertyValueData([FromBody] PropertyDto propertyDto)
        {

            string query = $"      select " +
                $"PropertyValueCode,ClassName,ObjectRef," +
                $"    Nvarchar1, Nvarchar2,Nvarchar3, Nvarchar4, Nvarchar5," +
                $"    Nvarchar6, Nvarchar7, Nvarchar8, Nvarchar9,Nvarchar10," +
                $"    Nvarchar11, Nvarchar12, Nvarchar13, Nvarchar14,Nvarchar15," +
                $"    Nvarchar16, Nvarchar17, Nvarchar18,Nvarchar19, Nvarchar20," +
                $"    CAST( Int1 AS INT)Int1,   CAST( Int2 AS INT)Int2,    CAST( Int3 AS INT)Int3," +
                $"    CAST( Int4 AS INT)Int4,   CAST( Int5 AS INT)Int5,    CAST( Int6 AS INT)Int6," +
                $"    CAST( Int7 AS INT)Int7,    CAST( Int8 AS INT)Int8,    CAST( Int9 AS INT)Int9," +
                $"    CAST( Int10 AS INT) Int10,    Float1, Float2, Float3, Float4, Float5, Float6, Float7, Float8, Float9,Float10," +
                $"Text1, Text2, Text3, Text4, Text5 from  PropertyValue " +
                $" where ObjectRef ={propertyDto.ObjectRef} And ClassName='{propertyDto.ClassName}'";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PropertyValues", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetPropertyValueData));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpPost]
        [Route("PropertyCrudService")]
        public async Task<IActionResult> PropertyCrudService([FromBody] JsonModelDto jsonModelDto)

        {
            string SpName = "";
            string ResponseData_Field = "";

            if (jsonModelDto.TableName == "Good")
            {
                SpName = $" spGood_AddNew ";
                ResponseData_Field = "Goods";

            }
            else if (jsonModelDto.TableName == "Central")
            {

                SpName = $" spCentral_AddNew ";
                ResponseData_Field = "Centrals";

            }
            else if (jsonModelDto.TableName == "City")
            {

                SpName = $" spCity_AddNew ";
                ResponseData_Field = "Citys";

            }
            else
            {
                SpName = $"";


            }

            



            string query = $"Exec "+ SpName + $" '{jsonModelDto.JsonData}' ";



            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, ResponseData_Field, "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PropertyCrudService));
                return StatusCode(500, "Internal server error.");
            }
        }











        [HttpPost]
        [Route("ChangeXUserPassword")]
        public async Task<IActionResult> ChangeXUserPassword([FromBody] IsUserDto loginUserDto)
        {


            string query = $"Exec spApp_ChangeXUserPassword '{loginUserDto.UName}','{loginUserDto.UPass}','{loginUserDto.UNewPass}'";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ChangeXUserPassword));
                return StatusCode(500, "Internal server error.");
            }


        }







        [HttpPost]
        [Route("ResetXUserPassword")]
        public async Task<IActionResult> ResetXUserPassword([FromBody] IsUserDto loginUserDto)
        {


            string query = $"Exec spWeb_ResetXUserPassword '{loginUserDto.UName}' ";

            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ChangeXUserPassword));
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
                    DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
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
                string dataPath = _configuration.GetValue<string>("AppSettings:Ocr_imagePath") + $"{dataName}"; // Provide the path where you want to save the image
                string data_zipPath = _configuration.GetValue<string>("AppSettings:Ocr_imagePath") + $"{dataName_zip}"; // Provide the path where you want to save the zip file


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

                    DataTable dataTable1 = await db.Kowsar_ExecQuery(HttpContext, query1);
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
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query11);
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



            DataTable dataTable1 = await db.Kowsar_ExecQuery(HttpContext, query1);
            dbname = dataTable1.Rows[0]["dbname"] + "";

            string query = $"select * from {dbname}..AttachedFiles where ClassName = '{attachFile.ClassName}' And ObjectRef = {attachFile.ObjectRef} ";


            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
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

                DataTable dbResult = await db.Kowsar_ExecQuery(HttpContext, queryDb);
                string dbname = dbResult.Rows[0]["dbname"].ToString();

                // 2️⃣ دریافت فایل
                string query = $"spWeb_GetAttachFile '{AttachedFileCode}', '{dbname}'";
                DataTable dt = await db.Kowsar_ExecQuery(HttpContext, query);

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
                return BadRequest(new { Success = false, ex.Message });
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



        //[HttpGet]
        //[Route("GetNotification")]
        //public async Task<IActionResult> GetNotification()
        //{
        //    var PersonInfoCode = HttpContext.Request.Headers["PIC"].FirstOrDefault() ?? string.Empty;

        //    string query = $"spWeb_GetNotification {PersonInfoCode}";
        //    try
        //    {
        //        DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
        //        string json = jsonClass.JsonResult_Str(dataTable, "users", "");

        //        return Content(json, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred in {Function}", nameof(GetNotification));
        //        return StatusCode(500, "Internal server error.");
        //    }

        //}



        [HttpGet]
        [Route("GetCustomerNotification")]
        public async Task<IActionResult> GetCustomerNotification()
        {
            var PersonInfoCode = HttpContext.Request.Headers["PIC"].FirstOrDefault() ?? string.Empty;

            string query = $"spWeb_GetCustomerNotification {PersonInfoCode}";
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetCustomerNotification));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("GetKowsarNotification")]
        public async Task<IActionResult> GetKowsarNotification()
        {
            var CentralRef = HttpContext.Request.Headers["CR"].FirstOrDefault() ?? string.Empty;

            string query = $"spWeb_GetKowsarNotification {CentralRef}";
            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetKowsarNotification));
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

            string userId = WebUtility.UrlDecode(HttpContext.Request.Headers["UI"].FirstOrDefault()) ?? string.Empty;

            string query = $"spWeb_Attendance_Insert @CentralRef = {manualAttendance.CentralRef}, @Status = {manualAttendance.Status}";




            try
            {
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
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
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
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
                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Attendances", "");

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(AttendanceHistory));
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


                DataTable dataTable = await db.Kowsar_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "KowsarReports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Function}", nameof(GetKowsarReport));
                return StatusCode(500, "Internal server error.");
            }
        }




























































































































































































































































































































































































































































    }

}

