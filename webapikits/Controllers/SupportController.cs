using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;

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






    }
}
