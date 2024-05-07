using System.Data;
using Microsoft.AspNetCore.Mvc;
using webapikits.Model;


namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebController : ControllerBase
    {



        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        public WebController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }







        [HttpGet]
        [Route("GetAppBrokerCustomer")]
        public string GetAppBrokerCustomer()
        {

            string query = $"select * from AppBrokerCustomer";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("GetAppBrokerCustomerByCode")]
        public string GetAppBrokerCustomerByCode(string AppBrokerCustomerCode)
        {

            string query = $"select * from AppBrokerCustomer Where AppBrokerCustomerCode = '{AppBrokerCustomerCode}'";

            DataTable dataTable = db.Web_ExecQuery( Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);





        }









        [HttpPost]
        [Route("InsertAppBrokerCustomer")]
        public string InsertAppBrokerCustomer([FromBody] BrokerCustomerDto brokercustomerdto )
        {

            string query = $"exec [spApp_InsertAppBrokerCustomer] '{brokercustomerdto.ActivationCode}', '{brokercustomerdto.EnglishCompanyName}', '{brokercustomerdto.PersianCompanyName}', '{brokercustomerdto.ServerURL}'," +
                $" '{brokercustomerdto.SQLiteURL}', {brokercustomerdto.MaxDevice}, '{brokercustomerdto.SecendServerURL}' , '{brokercustomerdto.DbName}', {brokercustomerdto.AppType} ";


            DataTable dataTable = db.Web_ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }






        [HttpPost]
        [Route("UpdateAppBrokerCustomer")]
        public string UpdateAppBrokerCustomer([FromBody] BrokerCustomerDto brokercustomerdto)
        {

            string query = $"exec [spApp_UpdateAppBrokerCustomer] '{brokercustomerdto.ActivationCode}', '{brokercustomerdto.EnglishCompanyName}', '{brokercustomerdto.PersianCompanyName}', " +
                $"'{brokercustomerdto.ServerURL}', '{brokercustomerdto.SQLiteURL}', {brokercustomerdto.MaxDevice}, '{brokercustomerdto.SecendServerURL}' , '{brokercustomerdto.DbName}', {brokercustomerdto.AppType} ";


            DataTable dataTable = db.Web_ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }





        [HttpGet]
        [Route("GetActiveApplication")]
        public string GetActiveApplication(string Filter)
        {

            string query = $"Select Server_Name, STRING_AGG([Broker],',') within group (order by case when isnumeric([Broker])=1 then cast([Broker] as decimal) else 0 end, [Broker] ) as BrokerStr From (select Server_Name, Device_Id, [Broker] from app_info where DATEDIFF(m,Updatedate,GETDATE())<{Filter} group by Server_Name, Device_Id, [Broker]) ds group by Server_Name";


            DataTable dataTable = db.Web_ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }






        [HttpGet]
        [Route("GetWebLog")]
        public string GetWebLog()
        {

            string query = $"select top 50 * from WebLog order by 1 desc";


            DataTable dataTable = db.Web_ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("InsertwebLog")]
        public string InsertwebLog(string ClassName, string TagName, string LogValue)
        {

            string query = $"exec sp_WebLogInsert @ClassName='{ClassName}',@TagName='{TagName}',@LogValue='{LogValue}'";


            DataTable dataTable = db.Web_ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        //------------------------------------------------------------




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


            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);


        }





        [HttpGet]
        [Route("LetterInsert")]
        public string LetterInsert(string LetterDate, string title, string Description, string CentralRef)
        {

            string query = $"exec dbo.spAutLetter_Insert @LetterDate='{LetterDate}', @InOutFlag=0,@Title ='{title}', @Description='{Description}',@State ='درحال انجام',@Priority ='عادي', @ReceiveType ='دستي', @CreatorCentral =28, @OwnerCentral ={CentralRef} ";


            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpGet]
        [Route("GetLetterRowList")]
        public string GetLetterRowList(string LetterRef)
        {

            string query = $"select  LetterRowCode,Name RowExecutorName,LetterRef ,LetterDate RowLetterDate,LetterDescription LetterRowDescription, LetterState LetterRowState, ExecutorCentralRef RowExecutorCentralRef from vwautletterrow join central on CentralCode=ExecutorCentralRef where LetterRef = {LetterRef} order by LetterRowCode desc";


            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetCentralUser")]
        public string GetCentralUser()
        {

            string query = $"select CentralCode,CentralName from vwCentralUser ";


            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpGet]
        [Route("AutLetterRowInsert")]
        public string AutLetterRowInsert(
           string LetterRef,
           string LetterDate,
           string Description,
           string CreatorCentral,
           string ExecuterCentral
            )
        {

            string query = $"spAutLetterRow_Insert @LetterRef = {LetterRef}, @LetterDate = '{LetterDate}', @Description = '{Description}', @State = 'درحال انجام', @Priority = 'عادي', @CreatorCentral = {CreatorCentral}, @ExecuterCentral = {ExecuterCentral}";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpGet]
        [Route("SetAlarmOff")]
        public string SetAlarmOff(
           string LetterRowCode
            )
        {

            string query = $" Update AutLetterRow Set AlarmActive=0 Where LetterRowCode={LetterRowCode} ";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetAutConversation")]
        public string GetAutConversation(
           string LetterRef
            )
        {

            string query = $"Exec spWeb_GetAutConversation  {LetterRef}";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }


        public class LetterDto
        {
            public string? LetterRef { get; set; }
            public string? CentralRef { get; set; }
            public string? ConversationText { get; set; }
        }



        [HttpPost]
        [Route("Conversation_Insert")]
        public string Conversation_Insert([FromBody] LetterDto letterdto)
        {

            string query = $"Exec spWeb_AutLetterConversation_Insert @LetterRef={letterdto.LetterRef}, @CentralRef={letterdto.CentralRef}, @ConversationText='{letterdto.ConversationText}'";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }







        [HttpGet]
        [Route("GetLetterFromPersoninfo")]
        public string GetLetterFromPersoninfo(string PersonInfoCode)
        {

            string query = $"spWeb_AutLetterListByPerson {PersonInfoCode}";


            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        /// <summary>
        /// ///////////////////////////////////////////////////////////
        /// </summary>



        [HttpPost]
        [Route("Conversation_UploadImage")]
        public string Conversation_UploadImage([FromBody] ksrImageModel data)
        {


            try
            {


                string query1 = $"Exec spWeb_AutLetterConversation_Insert @LetterRef={data.LetterRef}, @CentralRef={data.CentralRef}, @ConversationText='Image'";

                DataTable dataTable1 = db.Web_ExecQuery(Request.Path, query1);
                string Conversationref = dataTable1.Rows[0]["ConversationCode"]+"";




                // Decode the base64 string to bytes
                byte[] decodedImage = Convert.FromBase64String(data.image);


                // Save the image bytes to a file

                string filePath = _configuration.GetConnectionString("web_imagePath") + $"{Conversationref}.jpg"; // Provide the path where you want to save the image





                System.IO.File.WriteAllBytes(filePath, decodedImage);


                string query = $"Exec spImageImport  '{data.ClassName}',{Conversationref},'{filePath}' ;select @@IDENTITY KsrImageCode";


                DataTable dataTable = db.ImageExecQuery(query);

                return "Ok";
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";

            }
        }




        [HttpGet]
        [Route("GetWebImagess")]
        public string GetWebImagess(string pixelScale, string ClassName, string ObjectRef)
        {
            string query = $"SELECT * FROM KsrImage WHERE Classname = '{ClassName}' AND ObjectRef = {ObjectRef} order by 1 desc";


            DataTable dataTable = db.Image_ExecQuery(query);


            return jsonClass.ConvertAndScaleImageToBase64(Convert.ToInt32(pixelScale), dataTable);

        }



        [HttpGet]
        [Route("GetOrdergroupList")]
        public string GetOrdergroupList(string GroupCode)
        {

            string query = $"Exec  spApp_GetGoodGroups  @GroupCode= {GroupCode} ";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }






        [HttpGet]
        [Route("kowsar_info")]
        public string kowsar_info(string Where)
        {

            string query = "select top 1 DataValue from dbsetup where KeyValue = '" + Where + "'";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);



        }





        [HttpPost]
        [Route("GetOrderGoodList")]
        public string GetOrderGoodList([FromBody] OrderGoodListSearchDto searchDto)
        {
            string searchtarget = searchDto.Where.Replace(" ", "%");

            string query = $"Exec spApp_GetGoods2 @RowCount = {searchDto.RowCount},@Where = N' GoodName like ''%{searchtarget}%''' ,@AppBasketInfoRef=0, @GroupCode = {searchDto.GroupCode} ,@AppType=3 ";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("ChangeGoodActive")]
        public string ChangeGoodActive(string GoodCode, string ActiveFlag)
        {

            string query = $"spWeb_ChangeGoodActive {GoodCode},{ActiveFlag} ";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }





        [HttpGet]
        [Route("GetGoodEdit")]
        public string GetGoodEdit(string Where)
        {

            string query = $"Select GoodCode,GoodName, CAST(MaxSellprice AS INT) MaxSellprice,GoodExplain1,GoodExplain2,GoodExplain3,GoodExplain4,GoodExplain5,GoodExplain6 from Good Where GoodCode = {Where} ";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }

        [HttpGet]
        [Route("GetActiveGood")]
        public string GetActiveGood(string GoodCode)
        {

            string query = $"select ActiveStack,GoodRef from GoodStack where goodref = {GoodCode}  order by 1 desc ";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }





        [HttpPost]
        [Route("Web_InsertGood")]
        public string Web_InsertGood([FromBody] GoodDto gooddto)
        {

            string query = $"Exec spWeb_InsertGood '{gooddto.GoodName}' , {gooddto.MaxSellPrice},'{gooddto.GoodExplain1}','{gooddto.GoodExplain2}','{gooddto.GoodExplain3}','{gooddto.GoodExplain4}','{gooddto.GoodExplain5}','{gooddto.GoodExplain6}' ";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpPost]
        [Route("Web_UpdateGoodDetail")]
        public string Web_UpdateGoodDetail([FromBody] GoodDto gooddto)
        {

            string query = $"Exec spWeb_UpdateGoodDetail {gooddto.GoodCode},'{gooddto.GoodName}' , {gooddto.MaxSellPrice},'{gooddto.GoodExplain1}','{gooddto.GoodExplain2}','{gooddto.GoodExplain3}','{gooddto.GoodExplain4}','{gooddto.GoodExplain5}','{gooddto.GoodExplain6}' ";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }






        [HttpGet]
        [Route("GetGroupFromGood")]
        public string GetGroupFromGood(string Where)
        {

            string query = $"select GoodGroupCode,GoodGroupRef, Name, GoodRef from GoodGroup join Goodsgrp  on GoodGroupRef = GroupCode where Goodref = {Where}  ";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]


        [Route("GetGoodFromGroup")]
        public string GetGoodFromGroup(string Where)
        {

            string query = $"select GoodGroupCode, GoodName, GoodCode from Good join GoodGroup on goodref = GoodCode  where GoodGroupRef = {Where}  ";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }


        [HttpGet]
        [Route("DeleteGoodGroupCode")]
        public string DeleteGoodGroupCode(string Where)
        {

            string query = $" delete from GoodGroup Where GoodGroupCode = {Where}  ";

            DataTable dataTable = db.Web_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }




        [HttpGet]
        [Route("Gettracker")]
        public string Gettracker(string BrokerCode, string StartDate, string EndDate)
        {

            string query = $"Select * From(select * , rwn=row_Number() over (partition by gpsdate order by gpsdate)From GpsLocation Where Brokerref = {BrokerCode} And GpsDate between '{StartDate}' And '{EndDate}' ) ds where rwn=1 order by GpsDate ";


            DataTable dataTable = db.Web_ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }








        [HttpGet]
        [Route("ExistUser")]
        public string ExistUser(string UName, string UPass)
        {

            string query = $"Exec spapp_IsXUser  '{UName}','{UPass}'";


            DataTable dataTable = db.Web_ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }


        


        [HttpGet]
        [Route("ChangeXUserPassword")]
        public string ChangeXUserPassword(string UName, string UPass, string NewPass)
        {

            string query = $"Exec spApp_ChangeXUserPassword  '{UName}','{UPass}','{NewPass}'";


            DataTable dataTable = db.Web_ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }















    }
}
