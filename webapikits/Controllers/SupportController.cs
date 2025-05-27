using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;
using System.IO.Compression;
using System.Data.SqlClient;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Net.Sockets;
using System;

namespace webapikits.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {

        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _requestCode;

        public SupportController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);
            _httpContextAccessor = httpContextAccessor;


        }






        [HttpGet]
        [Route("GetTodeyFromServer")]
        public string GetTodeyFromServer()
        {




            string query = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);


        }








        [HttpPost]
        [Route("UpdatePersonInfo")]
        public string UpdatePersonInfo([FromBody] PersonInfoDto personInfoDto)
        {


            string query = $"Exec [dbo].[spWeb_UpdatePersonInfo] {personInfoDto.PersonInfoCode} ,'{personInfoDto.PhFirstName}','{personInfoDto.PhLastName}','{personInfoDto.PhCompanyName}','{personInfoDto.PhAddress1}','{personInfoDto.PhTel1}','{personInfoDto.PhMobile1}','{personInfoDto.PhEmail}'";
            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "users", "");
        }



        [HttpGet]
        [Route("GetKowsarPersonInfo")]
        public string GetKowsarPersonInfo(String PersonInfoCode)
        {


            string query = $"Exec [dbo].[spWeb_GetKowsarPersonInfo] {PersonInfoCode}";
            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "users", "");
        }


        [HttpPost]
        [Route("IsUser")]
        public string IsUser([FromBody] LoginUserDto loginUserDto)
        {


            string query = $"Exec [dbo].[spWeb_IsXUser] '{loginUserDto.UName}','{loginUserDto.UPass}'";
            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "users", "");
        }




        [HttpGet]
        [Route("GetObjectTypeFromDbSetup")]
        public string GetObjectTypeFromDbSetup(string ObjectType)
        {

            string query = $"select * from dbo.fnObjectType('{ObjectType}') ";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "ObjectTypes", "");

        }





        /// <returns></returns>

        [HttpPost]
        [Route("UploadImage")]
        public string UploadImage([FromBody] ksrImageModeldto data)
        {


            try
            {


                // Decode the base64 string to bytes
                byte[] decodedImage = Convert.FromBase64String(data.image);

                // Save the image bytes to a file

                string filePath = _configuration.GetConnectionString("web_imagePath") + $"{data.ObjectCode}.jpg"; // Provide the path where you want to save the image

                System.IO.File.WriteAllBytes(filePath, decodedImage);


                string query = $"Exec spImageImport  '{data.ClassName}',{data.ObjectCode},'{filePath}' ;select @@IDENTITY KsrImageCode";


                DataTable dataTable = db.Support_ImageExecQuery(query);

                return "\"Ok\"";
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";

            }
        }




        [HttpGet]
        [Route("GetCentralById")]
        public string GetCentralById(string CentralCode)
        {


            string query = $"select CentralCode,Title,Name,FName,Manager,Delegacy,CentralName from vwcentral where CentralCode={CentralCode}";
            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Centrals", "");
        }


        [HttpPost]
        [Route("GetKowsarCentral")]
        public string GetKowsarCentral([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Exec [dbo].[spWeb_GetKowsarCentral] '{searchTargetDto.SearchTarget}'";
            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Centrals", "");
        }







        [HttpPost]
        [Route("GetKowsarCustomer")]
        public string GetKowsarCustomer([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Exec [dbo].[spWeb_GetKowsarCustomer] '{searchTargetDto.SearchTarget}'";
            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Customers", "");
        }





        [HttpPost]
        [Route("GetLetterList")]
        public string GetLetterList([FromBody] SearchTargetLetterDto searchTargetLetterDto)
        {


            string Where = "";

            if (!string.IsNullOrEmpty(searchTargetLetterDto.SearchTarget))
            {
                Where = $"(LetterTitle like ''%{searchTargetLetterDto.SearchTarget}%'' or LetterDescription like ''%{searchTargetLetterDto.SearchTarget}%'' or ds.RowExecutorName like ''%{searchTargetLetterDto.SearchTarget}%'')";
            }

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

            if (!string.IsNullOrEmpty(searchTargetLetterDto.CreationDate))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += $" And LetterDate>=''{searchTargetLetterDto.CreationDate}''";
                }
                else
                {
                    Where = $"LetterDate>=''{searchTargetLetterDto.CreationDate}''";
                }
            }


            string query = $"Exec spWeb_AutLetterList '{Where}',{searchTargetLetterDto.OwnCentralRef}";


            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);


        }





        [HttpPost]
        [Route("LetterInsert")]
        public string LetterInsert([FromBody] LetterInsert letterInsert)
        {
            string CreatorCentral = _configuration.GetConnectionString("Support_CreatorCentral");


            string query = $"exec dbo.spAutLetter_Insert @LetterDate='{letterInsert.LetterDate}', @InOutFlag={letterInsert.InOutFlag},@Title ='{letterInsert.title}', @Description='{letterInsert.Description}',@State ='{letterInsert.LetterState}',@Priority ='{letterInsert.LetterPriority}', @ReceiveType =N'دستی', @CreatorCentral ={letterInsert.CentralRef}, @OwnerCentral ={CreatorCentral} ";


            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpGet]
        [Route("GetLetterRowList")]
        public string GetLetterRowList(string LetterRef)
        {

            string query = $"select  LetterRowCode,CreatorCentralRef,AutLetterRow_PropDescription1,Name RowExecutorName,LetterRef ,LetterDate RowLetterDate,LetterDescription LetterRowDescription, LetterState LetterRowState, ExecutorCentralRef RowExecutorCentralRef from vwautletterrow join central on CentralCode=ExecutorCentralRef where LetterRef = {LetterRef} order by LetterRowCode desc";


            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetCentralUser")]
        public string GetCentralUser()
        {

            string query = $"select CentralCode,CentralName from vwCentralUser ";


            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpPost]
        [Route("AutLetterRowInsert")]
        public string AutLetterRowInsert([FromBody] AutLetterRowInsert autLetterRowInsert)
        {

            string query = $"spAutLetterRow_Insert @LetterRef = {autLetterRowInsert.LetterRef}, @LetterDate = '{autLetterRowInsert.LetterDate}'" +
                $", @Description = '{autLetterRowInsert.Description}', @State = '{autLetterRowInsert.LetterState}', @Priority = '{autLetterRowInsert.LetterPriority}'" +
                $", @CreatorCentral = {autLetterRowInsert.CreatorCentral}, @ExecuterCentral = {autLetterRowInsert.ExecuterCentral}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }








        [HttpPost]
        [Route("SetAlarmOff")]
        public string SetAlarmOff([FromBody] AlarmOffDto alarmOffDto)
        {

            string query = $"spWeb_SetAlarmOff {alarmOffDto.LetterRef},{alarmOffDto.CentralRef}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetAutConversation")]
        public string GetAutConversation(
           string LetterRef
            )
        {

            string query = $"Exec spWeb_GetAutConversation  {LetterRef}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpGet]
        [Route("GetAutletterById")]
        public string GetAutletterById(string LetterCode)
        {

            string query = $"select LetterCode,LetterTitle,LetterDate,LetterDescription,LetterState,LetterPriority,OwnerName,CreatorName,ExecutorName,RowsCount from vwautletter  where LetterCode={LetterCode}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }






        [HttpPost]
        [Route("Conversation_Insert")]
        public string Conversation_Insert([FromBody] LetterDto letterdto)
        {

            string query = $"Exec spWeb_AutLetterConversation_Insert @LetterRef={letterdto.LetterRef}, @CentralRef={letterdto.CentralRef}, @ConversationText='{letterdto.ConversationText}'";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }







        [HttpPost]
        [Route("Update_AutletterRow")]
        public string Update_AutletterRow([FromBody] AutLetterRowInsert letterRowdto)
        {
            string query2 = "";
            string query3 = "";

            if (!string.IsNullOrEmpty(letterRowdto.AutLetterRow_PropDescription1))
            {
                query2 = $" spPropertyValue 'TAutLetterRow' , {letterRowdto.ObjectRef} ";
                DataTable dataTable2 = db.Support_ExecQuery(HttpContext, query2);


                query3 = $"Update PropertyValue Set Nvarchar1 = '{letterRowdto.AutLetterRow_PropDescription1}' Where ObjectRef = {letterRowdto.ObjectRef}  And ClassName ='TAutLetterRow'";
                DataTable dataTable3 = db.Support_ExecQuery(HttpContext, query3);
            }

           

            string query = $" Update AutLetterRow Set LetterState = '{letterRowdto.LetterRowState}' , LetterDescription = '{letterRowdto.LetterRowDescription}' , AlarmActive = 0 Where LetterRowCode = {letterRowdto.ObjectRef}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }







        [HttpGet]
        [Route("GetLetterFromPersoninfo")]
        public string GetLetterFromPersoninfo(string PersonInfoCode)
        {

            string query = $"spWeb_AutLetterListByPerson {PersonInfoCode}";


            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }
        


        [HttpPost]
        [Route("GetAutLetterListByPerson")]
        public string GetAutLetterListByPerson([FromBody] SearchTargetLetterDto searchTargetLetterDto)
        {


            string Where = "";

            if (!string.IsNullOrEmpty(searchTargetLetterDto.SearchTarget))
            {
                Where = $"(LetterTitle like ''%{searchTargetLetterDto.SearchTarget}%'' or LetterDescription like ''%{searchTargetLetterDto.SearchTarget}%'' or ds.RowExecutorName like ''%{searchTargetLetterDto.SearchTarget}%'')";
            }


            if (!string.IsNullOrEmpty(searchTargetLetterDto.CreationDate))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += $" And LetterDate>=''{searchTargetLetterDto.CreationDate}''";
                }
                else
                {
                    Where = $"LetterDate>=''{searchTargetLetterDto.CreationDate}''";
                }
            }



            string query = $"spWeb_AutLetterListByPersontest '{Where}','{searchTargetLetterDto.PersonInfoCode}'";


            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }
















        [HttpPost]
        [Route("Conversation_UploadImage")]
        public string Conversation_UploadImage([FromBody] ksrImageModel data)
        {
            try
            {
                string query1 = $"Exec spWeb_AutLetterConversation_Insert @LetterRef={data.LetterRef}, @CentralRef={data.CentralRef}, @ConversationText='Image'";

                DataTable dataTable1 = db.Support_ExecQuery(HttpContext, query1);
                string Conversationref = dataTable1.Rows[0]["ConversationCode"] + "";
                byte[] decodedImage = Convert.FromBase64String(data.image);












                string filePath = _configuration.GetConnectionString("web_imagePath") + $"{Conversationref}.jpg"; // Provide the path where you want to save the image
                System.IO.File.WriteAllBytes(filePath, decodedImage);
                string query = $"Exec spImageImport  '{data.ClassName}',{Conversationref},'{filePath}' ;select @@IDENTITY KsrImageCode";
                DataTable dataTable = db.Support_ImageExecQuery(query);
                return jsonClass.JsonResultWithout_Str(dataTable);
            }
            catch (Exception ex)
            { return $"{ex.Message}"; }
        }




        [HttpGet]
        [Route("GetWebImagess")]
        public string GetWebImagess(string pixelScale, string ClassName, string ObjectRef)
        {
            string query = $"SELECT * FROM KsrImage WHERE Classname = '{ClassName}' AND ObjectRef = {ObjectRef} order by 1 desc";
            DataTable dataTable = db.Support_ImageExecQuery(query);
            return jsonClass.ConvertAndScaleImageToBase64(Convert.ToInt32(pixelScale), dataTable);

        }

        [HttpPost]
        [Route("KowsarAttachFile")]
        public string KowsarAttachFile([FromBody] SearchTargetDto searchTarget)
        {
            string query = $"spWeb_SearchAttachFile {searchTarget.SearchTarget}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }

        [HttpPost]
        [Route("KowsarAttachUrl")]
        public string KowsarAttachUrl([FromBody] SearchTargetDto searchTarget)
        {
            string query = $"spWeb_SearchAttachFile '{searchTarget.SearchTarget}' ,'URL'";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpPost]
        [Route("SetAttachFile")]
        public string SetAttachFile([FromBody] AttachFile attachFile)
        {

            if (attachFile.Type == "URL")
            {


                string query = $"exec spWeb_AttachFile '{attachFile.Title}','{attachFile.FileName}','{attachFile.ClassName}','{attachFile.Type}','{attachFile.FilePath}',''";

                DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

                return jsonClass.JsonResultWithout_Str(dataTable);





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

                // Create a zip archive and add the image file to it
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

                    DataTable dataTable1 = db.Support_ExecQuery(HttpContext, query1);
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

            DataTable dataTable2 = db.Support_ExecQuery(HttpContext, query11);

            return jsonClass.JsonResult_Str(dataTable2, "Text", "TodeyFromServer");

        }




        [HttpPost]
        [Route("GetAttachFileList")]
        public string GetAttachFileList([FromBody] AttachFile attachFile)
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



            DataTable dataTable1 = db.Support_ExecQuery(HttpContext, query1);
            dbname = dataTable1.Rows[0]["dbname"] + "";

            string query = $"select * from {dbname}..AttachedFiles where ClassName = '{attachFile.ClassName}' And ObjectRef = {attachFile.ObjectRef} ";
            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "AttachedFiles", "");


        }



        [HttpGet]
        [Route("GetAttachFile")]
        public IActionResult GetAttachFile(string AttachedFileCode, string ClassName, string ObjectRef)
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



            DataTable dataTable4 = db.Support_ExecQuery(HttpContext, query11);
            dbname = dataTable4.Rows[0]["dbname"] + "";


            string query1 = $"spWeb_GetAttachFile '{AttachedFileCode}' , '{dbname}'";
            DataTable dataTable1 = db.Support_ExecQuery(HttpContext, query1);
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
        [Route("GetNotification")]
        public string GetNotification(string PersonInfoCode)
        {
            string query = $"spWeb_GetNotification {PersonInfoCode}";
            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "users", "");


        }




        [HttpPost]
        [Route("EditFactorProperty")]
        public string EditFactorProperty([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"spWeb_EditFactorProperty '{factorwebDto.starttime}','{factorwebDto.Endtime}','{factorwebDto.worktime}','{factorwebDto.Barbary}',{factorwebDto.ObjectRef} ";



            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");



        }












        [HttpPost]
        [Route("EditCustomerProperty")]
        public string EditCustomerProperty([FromBody] CustomerWebDto customerWebDto)
        {

            string query = $"spWeb_EditCustomerProperty '{customerWebDto.AppNumber}','{customerWebDto.DatabaseNumber}','{customerWebDto.Delegacy}',{customerWebDto.ObjectRef}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Customers", "");


        }


        [HttpPost]
        [Route("EditCustomerExplain")]
        public string EditCustomerExplain([FromBody] CustomerWebDto customerWebDto)
        {

            string query = $"Update Customer set Explain ='{customerWebDto.Explain}' where CustomerCode={customerWebDto.ObjectRef}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Customers", "");


        }


        [HttpGet]
        [Route("GetWebFactorSupport")]
        public string GetWebFactorSupport(string FactorCode)
        {

            string query = $" select FactorCode, FactorDate, CustName, CustomerCode, Explain, BrokerRef, BrokerName ,starttime,Endtime,worktime,Barbary from vwFactor where FactorCode = {FactorCode}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");


        }


        [HttpGet]
        [Route("GetWebFactorRowsSupport")]
        public string GetWebFactorRowsSupport(string FactorCode)
        {

            string query = $" select FactorRowCode,GoodName from vwFactorRows where Factorref= {FactorCode}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");


        }




        [HttpGet]
        [Route("DeleteWebFactorRowsSupport")]
        public string DeleteWebFactorRowsSupport(string FactorRowCode)
        {

            string query = $" delete from  FactorRows where FactorRowCode= {FactorRowCode}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");


        }



        [HttpGet]
        [Route("DeleteWebFactorSupport")]
        public string DeleteWebFactorSupport(string FactorCode)
        {

            string query = $" delete from  Factor where FactorCode= {FactorCode}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");


        }




        [HttpPost]
        [Route("GetGoodListSupport")]
        public string GetGoodListSupport([FromBody] SearchTargetDto searchTargetDto)
        {



            string query = $"spWeb_GetGoodListSupport '{SanitizeInput(searchTargetDto.SearchTarget)}'";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");


        }








        [HttpPost]
        [Route("WebSupportFactorInsert")]
        public string WebSupportFactorInsert([FromBody] FactorwebDto factorwebDto)
        {

            string UserId = _configuration.GetConnectionString("Support_UserId");

            string query = $"spWeb_Factor_Insert  @ClassName ='Factor',@StackRef =1,@UserId ={UserId},@Date ='{factorwebDto.FactorDate}',@Customer ={factorwebDto.CustomerCode},@Explain ='{factorwebDto.Explain}',@BrokerRef  = {factorwebDto.BrokerRef},@IsShopFactor  = 0";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");


        }



        [HttpPost]
        [Route("WebSupportFactorInsertRow")]
        public string WebSupportFactorInsertRow([FromBody] FactorRow factorRow)
        {

            string query = $"spWeb_Factor_InsertRow  @ClassName ='Factor', @FactorCode={factorRow.FactorRef}, @GoodRef ={factorRow.GoodRef},@Amount =1,@Price =0,@UserId =29,@MustHasAmount =0, @MergeFlag =1 ";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");
        }











        [HttpPost]
        [Route("Support_StartFactorTime")]
        public string StartFactorTime([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"Update PropertyValue Set Nvarchar15 = '{factorwebDto.starttime}'  where ClassName = 'TFactor' And ObjectRef = {factorwebDto.ObjectRef} ";



            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");



        }

        [HttpPost]
        [Route("Support_EndFactorTime")]
        public string EndFactorTime([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"Update PropertyValue Set Nvarchar9 = '{factorwebDto.Endtime}', int1 = {factorwebDto.worktime} where ClassName = 'TFactor' And ObjectRef = {factorwebDto.ObjectRef} ";



            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");



        }


        [HttpPost]
        [Route("Support_ExplainFactor")]
        public string Support_ExplainFactor([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"Update PropertyValue Set Nvarchar14 = '{SanitizeInput(factorwebDto.Barbary)}' where ClassName = 'TFactor' And ObjectRef = {factorwebDto.ObjectRef} ";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }




        [HttpPost]
        [Route("Support_Count")]
        public string Support_Count([FromBody] FactorwebDto factorwebDto)
        {

            string query = $" Declare @S nvarchar(20)=dbo.fnDate_AddDays(dbo.fnDate_Today(),-365) select BrokerCode, BrokerName, sum(worktime)/60 worktime,cast(sum(SumAmount) as int) SumAmount,Count(*) FactorCount from vwFactor where FactorDate>@S group by BrokerName, BrokerCode ";



            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");



        }

        [HttpGet]
        [Route("GetGridSchema")]
        public string GetGridSchema(string Where)
        {


            string query = $"Select * From [dbo].[fnGetGridSchema]('{Where}')  where Visible = 1";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "GridSchemas", "");

        }

        [HttpPost]
        [Route("GetFactors")]
        public string GetFactors([FromBody] SearchTargetDto searchTargetDto)
        {

            string query = $" Exec spWeb_GetFactor";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }

        /*
        [HttpPost]
        [Route("GetFactor")]
        public string GetFactor([FromBody] FactorwebDto factorwebDto)
        {

            string query = $" spWeb_GetFactor '{factorwebDto.StartDateTarget}','{factorwebDto.EndDateTarget}','{factorwebDto.SearchTarget}','{factorwebDto.BrokerRef}','{factorwebDto.isShopFactor}'";



            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");



        }

        */

        [HttpPost]
        [Route("GetSupportFactors")]
        public string GetSupportFactors([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"Exec spWeb_GetSupportFactor '{factorwebDto.StartDateTarget}','{factorwebDto.EndDateTarget}','{factorwebDto.SearchTarget}','{factorwebDto.BrokerRef}','{factorwebDto.isShopFactor}'";



            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");



        }



        [HttpPost]
        [Route("WebFactorInsert")]
        public string WebFactorInsert([FromBody] FactorwebDto factorwebDto)
        {


            string UserId = _configuration.GetConnectionString("Support_UserId");

            string query = $"spWeb_Factor_Insert  @ClassName ='{factorwebDto.ClassName}',@StackRef ={factorwebDto.StackRef},@UserId ={UserId},@Date ='{factorwebDto.FactorDate}',@Customer ={factorwebDto.CustomerCode},@Explain ='{factorwebDto.Explain}',@BrokerRef  = {factorwebDto.BrokerRef},@IsShopFactor  = {factorwebDto.isShopFactor}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");


        }



        [HttpPost]
        [Route("WebFactorInsertRow")]
        public string WebFactorInsertRow([FromBody] FactorRow factorRow)
        {

            string UserId = _configuration.GetConnectionString("Support_UserId");

            string query = $"spWeb_Factor_InsertRow  @ClassName ='{factorRow.ClassName}', @FactorCode={factorRow.FactorRef}, @GoodRef ={factorRow.GoodRef},@Amount ={factorRow.Amount},@Price ={factorRow.Price},@UserId ={UserId},@MustHasAmount ={factorRow.MustHasAmount}, @MergeFlag ={factorRow.MergeFlag} ";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");
        }




        [HttpGet]
        [Route("GetCustomerFactor")]
        public string GetCustomerFactor(string Where)
        {


            string query = $"spWeb_GetCustomerFactor {Where}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }



        [HttpPost]
        [Route("GetSupportData")]
        public string GetSupportData([FromBody] SupportDto supportDto)

        {
            // 1 support panel
            // 2 EmptyEndTimeCount


            string query = $"   spWeb_SupportData @DateTarget = '{supportDto.DateTarget}', @BrokerCode = {supportDto.BrokerCode}, @Flag = {supportDto.Flag}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "SupportDatas", "");
        }






        [HttpPost]
        [Route("ManualAttendance")]
        public string ManualAttendance([FromBody] ManualAttendance manualAttendance)
            
        {
            // 0 ghayeb 
            // 1 hozor
            // 2 mashghol

            string userId = _configuration.GetConnectionString("Support_UserId");

            string query = $"spWeb_Attendance_ManualInsert @CentralRef = {manualAttendance.CentralRef}, @Status = {manualAttendance.Status}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Attendances", "");
        }



        [HttpGet]
        [Route("AttendanceDashboard")]
        public string AttendanceDashboard()
        {
            string query = "spWeb_Attendance_Dashboard";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Attendances", "");
        }


        [HttpGet]
        [Route("AttendanceHistory")]
        public string AttendanceHistory(string CentralRef)
        {
            string query = $"spWeb_Attendance_History @CentralRef = {CentralRef}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Attendances", "");
        }


        [HttpGet]
        [Route("DeleteAutLetterRows")]
        public string DeleteAutLetterRows(string LetterRowCode)
        {

            string query = $" Delete From  AutLetterRow where LetterRowCode= {LetterRowCode}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "AutLetters", "");


        }



        [HttpGet]
        [Route("DeleteAutLetter")]
        public string DeleteAutLetter(string LetterCode)
        {

            string query = $" Delete From  AutLetter where LetterCode= {LetterCode}";

            DataTable dataTable = db.Support_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "AutLetters", "");


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
