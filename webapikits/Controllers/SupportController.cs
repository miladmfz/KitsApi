using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;
using System.IO.Compression;
using System.Data.SqlClient;
using static webapikits.Controllers.OcrController;
using System.Data.Entity.Core.Objects;

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

        public SupportController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }




        [HttpGet]
        [Route("GetTodeyFromServer")]
        public string GetTodeyFromServer()
        {

            string query = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);


        }

        [HttpGet]
        [Route("GetApplicationForMenu")]
        public string GetApplicationForMenu()
        {

            string query = $"select KeyValue,Description,DataValue,KeyId from dbsetup where KeyValue in ('AppBroker_ActivationCode','AppOcr_ActivationCode','AppOrder_ActivationCode') and DataValue <> '0'";

            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);
            return jsonClass.JsonResult_Str(dataTable, "applications", "");


        }







        [HttpPost]
        [Route("UpdatePersonInfo")]
        public string UpdatePersonInfo([FromBody] PersonInfoDto personInfoDto)
        {


            string query = $"Exec [dbo].[spWeb_UpdatePersonInfo] {personInfoDto.PersonInfoCode} ,'{personInfoDto.PhFirstName}','{personInfoDto.PhLastName}','{personInfoDto.PhCompanyName}','{personInfoDto.PhAddress1}','{personInfoDto.PhTel1}','{personInfoDto.PhMobile1}','{personInfoDto.PhEmail}'";
            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "users", "");
        }



        [HttpGet]
        [Route("GetKowsarPersonInfo")]
        public string GetKowsarPersonInfo(String PersonInfoCode)
        {


            string query = $"Exec [dbo].[spWeb_GetKowsarPersonInfo] {PersonInfoCode}";
            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "users", "");
        }


        [HttpPost]
        [Route("IsUser")]
        public string IsUser([FromBody] LoginUserDto loginUserDto)
        {


            string query = $"Exec [dbo].[spWeb_IsXUser] '{loginUserDto.UName}','{loginUserDto.UPass}'";
            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "users", "");
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
            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Centrals", "");
        }


        [HttpPost]
        [Route("GetKowsarCentral")]
        public string GetKowsarCentral([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Exec [dbo].[spWeb_GetKowsarCentral] '{searchTargetDto.SearchTarget}'";
            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Centrals", "");
        }







        [HttpPost]
        [Route("GetKowsarCustomer")]
        public string GetKowsarCustomer([FromBody] SearchTargetDto searchTargetDto)
        {


            string query = $"Exec [dbo].[spWeb_GetKowsarCustomer] '{searchTargetDto.SearchTarget}'";
            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Customers", "");
        }





        [HttpGet]
        [Route("GetLetterList")]
        public string GetLetterList(string SearchTarget = null, string CentralRef = null, string CreationDate = null)
        {


            string Where = "";

            if (!string.IsNullOrEmpty(SearchTarget))
            {
                Where = $"(LetterTitle like ''%{SearchTarget}%'' or LetterDescription like ''%{SearchTarget}%'' or ds.RowExecutorName like ''%{SearchTarget}%'')";
            }

            if (!string.IsNullOrEmpty(CentralRef))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += $" And (CreatorCentralRef={CentralRef} or OwnerCentralRef={CentralRef} or RowExecutorCentralRef={CentralRef})";
                }
                else
                {
                    Where = $"(CreatorCentralRef={CentralRef} or OwnerCentralRef={CentralRef} or RowExecutorCentralRef={CentralRef})";
                }
            }

            if (!string.IsNullOrEmpty(CreationDate))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += $" And LetterDate>''{CreationDate}''";
                }
                else
                {
                    Where = $"LetterDate>''{CreationDate}''";
                }
            }


            string query = $"Exec spWeb_AutLetterList '{Where}'";


            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);


        }





        [HttpPost]
        [Route("LetterInsert")]
        public string LetterInsert([FromBody] LetterInsert letterInsert)
        {
            string CreatorCentral = _configuration.GetConnectionString("Support_CreatorCentral");


            string query = $"exec dbo.spAutLetter_Insert @LetterDate='{letterInsert.LetterDate}', @InOutFlag=2,@Title ='{letterInsert.title}', @Description='{letterInsert.Description}',@State ='درحال انجام',@Priority ='عادي', @ReceiveType ='دستي', @CreatorCentral ={CreatorCentral}, @OwnerCentral ={letterInsert.CentralRef} ";


            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpGet]
        [Route("GetLetterRowList")]
        public string GetLetterRowList(string LetterRef)
        {

            string query = $"select  LetterRowCode,Name RowExecutorName,LetterRef ,LetterDate RowLetterDate,LetterDescription LetterRowDescription, LetterState LetterRowState, ExecutorCentralRef RowExecutorCentralRef from vwautletterrow join central on CentralCode=ExecutorCentralRef where LetterRef = {LetterRef} order by LetterRowCode desc";


            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetCentralUser")]
        public string GetCentralUser()
        {

            string query = $"select CentralCode,CentralName from vwCentralUser ";


            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpPost]
        [Route("AutLetterRowInsert")]
        public string AutLetterRowInsert([FromBody] AutLetterRowInsert autLetterRowInsert)
        {

            string query = $"spAutLetterRow_Insert @LetterRef = {autLetterRowInsert.LetterRef}, @LetterDate = '{autLetterRowInsert.LetterDate}'" +
                $", @Description = '{autLetterRowInsert.Description}', @State = 'درحال انجام', @Priority = 'عادي'" +
                $", @CreatorCentral = {autLetterRowInsert.CreatorCentral}, @ExecuterCentral = {autLetterRowInsert.ExecuterCentral}";

            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }








        [HttpGet]
        [Route("SetAlarmOff")]
        public string SetAlarmOff(
             string LetterRowCode
            )
        {

            string query = $" Update AutLetterRow Set AlarmActive=0 Where LetterRowCode={LetterRowCode} ";

            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetAutConversation")]
        public string GetAutConversation(
           string LetterRef
            )
        {

            string query = $"Exec spWeb_GetAutConversation  {LetterRef}";

            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }






        [HttpPost]
        [Route("Conversation_Insert")]
        public string Conversation_Insert([FromBody] LetterDto letterdto)
        {

            string query = $"Exec spWeb_AutLetterConversation_Insert @LetterRef={letterdto.LetterRef}, @CentralRef={letterdto.CentralRef}, @ConversationText='{letterdto.ConversationText}'";

            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }







        [HttpGet]
        [Route("GetLetterFromPersoninfo")]
        public string GetLetterFromPersoninfo(string PersonInfoCode)
        {

            string query = $"spWeb_AutLetterListByPerson {PersonInfoCode}";


            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }


        [HttpPost]
        [Route("Conversation_UploadImage")]
        public string Conversation_UploadImage([FromBody] ksrImageModel data)
        {
            try
            {
                string query1 = $"Exec spWeb_AutLetterConversation_Insert @LetterRef={data.LetterRef}, @CentralRef={data.CentralRef}, @ConversationText='Image'";

                DataTable dataTable1 = db.Support_ExecQuery(Request.Path, query1);
                string Conversationref = dataTable1.Rows[0]["ConversationCode"]+"";
                byte[] decodedImage = Convert.FromBase64String(data.image);

                string filePath = _configuration.GetConnectionString("web_imagePath") + $"{Conversationref}.jpg"; // Provide the path where you want to save the image
                System.IO.File.WriteAllBytes(filePath, decodedImage);
                string query = $"Exec spImageImport  '{data.ClassName}',{Conversationref},'{filePath}' ;select @@IDENTITY KsrImageCode";
                DataTable dataTable = db.Support_ImageExecQuery(query);
                return jsonClass.JsonResultWithout_Str(dataTable);
            }
            catch (Exception ex) 
            { return $"{ex.Message}";  }
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

            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }

        [HttpPost]
        [Route("KowsarAttachUrl")]
        public string KowsarAttachUrl([FromBody] SearchTargetDto searchTarget)
        {
            string query = $"spWeb_SearchAttachFile '{searchTarget.SearchTarget}' ,'URL'";

            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpPost]
        [Route("SetAttachFile")]
        public string SetAttachFile([FromBody] AttachFile attachFile)
        {

            if (attachFile.Type == "URL")
            {


                string query = $"exec spWeb_AttachFile '{attachFile.Title}','{attachFile.FileName}','{attachFile.ClassName}','{attachFile.Type}','{attachFile.FilePath}',''";

                DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

                return jsonClass.JsonResultWithout_Str(dataTable);





            }
            else {



                string data_base64 = attachFile.Data;
                byte[] data_Bytes = Convert.FromBase64String(data_base64);



                string dataName = $"{attachFile.FileName}.{attachFile.FileType}"; // Constructing the image name
                string dataName_zip = $"{attachFile.FileName}.zip"; // Constructing the image name
                string dataPath = _configuration.GetConnectionString("Ocr_imagePath") + $"{dataName}"; // Provide the path where you want to save the image
                string data_zipPath = _configuration.GetConnectionString("Ocr_imagePath") + $"{dataName_zip}"; // Provide the path where you want to save the zip file
                Console.WriteLine(dataPath);
                Console.WriteLine(data_zipPath);

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
                    if (attachFile.ClassName == "AutLetter") {
                         query1 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join AutLetter aut on PeriodId=PeriodRef Where LetterCode= {attachFile.ObjectRef}  Select @db dbname";

                    }else if (attachFile.ClassName == "Factor")
                    {

                        query1 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join Factor f on PeriodId=PeriodRef Where FactorCode= {attachFile.ObjectRef}  Select @db dbname";

                    }
                    else
                    {
                        query1 = $"Declare @dbname nvarchar(200)=db_name()+'Ocr' select  @dbname dbname";


                    }

                    DataTable dataTable1 = db.Support_ExecQuery(Request.Path, query1);
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

            DataTable dataTable2 = db.Support_ExecQuery(Request.Path, query11);

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



            DataTable dataTable1 = db.Support_ExecQuery(Request.Path, query1);
            dbname = dataTable1.Rows[0]["dbname"] + "";

            string query = $"select * from {dbname}..AttachedFiles where ClassName = '{attachFile.ClassName}' And ObjectRef = {attachFile.ObjectRef} ";
            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);

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



            DataTable dataTable4 = db.Support_ExecQuery(Request.Path, query11);
            dbname = dataTable4.Rows[0]["dbname"] + "";







            string query1 = $"spWeb_GetAttachFile '{AttachedFileCode}' , '{dbname}'";
            DataTable dataTable1 = db.Support_ExecQuery(Request.Path, query1);
            string base64File = dataTable1.Rows[0]["SourceFile"] + "";
            byte[] fileBytes = Convert.FromBase64String(base64File);


            string FileName = dataTable1.Rows[0]["FileName"] + "";




            string dataName_zip = $"{FileName}.zip"; // Constructing the image name
            string data_zipPath = _configuration.GetConnectionString("Ocr_imagePath") + $"{dataName_zip}";
            string contentType = $"application/{dataTable1.Rows[0]["Type"]}";


            System.IO.File.WriteAllBytes(data_zipPath, fileBytes);
            return File(fileBytes, contentType, Path.GetFileName(data_zipPath));





        }


        [HttpPost]
        [Route("GetFactor")]
        public string GetFactor([FromBody] FactorwebDto factorwebDto)
        {

            string query = $" spWeb_GetFactor '{factorwebDto.StartDateTarget}','{factorwebDto.EndDateTarget}','{factorwebDto.SearchTarget}','{factorwebDto.isShopFactor}'";



            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");



        }



        [HttpPost]
        [Route("EditFactorProperty")]
        public string EditFactorProperty([FromBody] FactorwebDto factorwebDto)
        {

            string query = $"spWeb_EditFactorProperty '{factorwebDto.starttime}','{factorwebDto.Endtime}','{factorwebDto.worktime}','{factorwebDto.Barbary}',{factorwebDto.ObjectRef} ";



            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");



        }





        [HttpPost]
        [Route("EditCustomerProperty")]
        public string EditCustomerProperty([FromBody] CustomerWebDto customerWebDto)
        {

            string query = $"spWeb_EditCustomerProperty '{customerWebDto.AppNumber}','{customerWebDto.DatabaseNumber}','{customerWebDto.LockNumber}',{customerWebDto.ObjectRef}";

            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);
            return jsonClass.JsonResult_Str(dataTable, "Customers", "");


        }


        [HttpPost]
        [Route("EditCustomerExplain")]
        public string EditCustomerExplain([FromBody] CustomerWebDto customerWebDto)
        {

            string query = $"Update Customer set Explain ='{customerWebDto.Explain}' where CustomerCode={customerWebDto.ObjectRef}";

            DataTable dataTable = db.Support_ExecQuery(Request.Path, query);
            return jsonClass.JsonResult_Str(dataTable, "Customers", "");


        }




    }
}
