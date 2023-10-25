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
        [Route("SetAlarmOff")]
        public string SetAlarmOff(
           string LetterRowCode
            )
        {

            string query = $" Update AutLetterRow Set AlarmActive=0 Where LetterRowCode={LetterRowCode} ";

            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }





        [HttpGet]
        [Route("AutLetterConversation_Insert")]
        public string AutLetterConversation_Insert(
           string LetterRef,
           string CentralRef,
           string ConversationText
            )
        {

            string query = $"Exec spWeb_AutLetterConversation_Insert @LetterRef={LetterRef}, @CentralRef={CentralRef}, @ConversationText='{ConversationText}'";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetAutConversation")]
        public string GetAutConversation(
           string LetterRef
            )
        {

            string query = $"Exec spWeb_GetAutConversation  {LetterRef}";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetCentralUser")]
        public string GetCentralUser()
        {

            string query = $"select CentralCode,CentralName from vwCentralUser ";


            DataTable dataTable = db.ExecQuery(query);

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


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetDbSetupValue")]
        public string GetDbSetupValue(
           string Where
            )
        {

            string query = $"select top 1 * from dbsetup where KeyValue = '{Where}'";


            DataTable dataTable = db.ExecQuery(query);

            return Convert.ToString(dataTable.Rows[0]["DataValue"]);

        }





        [HttpGet]
        [Route("GetAppBrokerCustomer")]
        public string GetAppBrokerCustomer(    )
        {

            string query = $"select * from AppBrokerCustomer";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }





        [HttpGet]
        [Route("GetAllBrokers")]
        public string GetAllBrokers(
       string Filter
        )
        {

            string query = $"Select Server_Name, STRING_AGG([Broker],',') within group (order by case when isnumeric([Broker])=1 then cast([Broker] as decimal) else 0 end, [Broker] ) as BrokerStr From (select Server_Name, Device_Id, [Broker] from app_info where DATEDIFF(m,Updatedate,GETDATE())<{Filter} group by Server_Name, Device_Id, [Broker]) ds group by Server_Name";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpGet]
        [Route("GetGoodsSum")]
        public string GetGoodsSum()
        {

            string query = $"Exec spGoodsSum_Web";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetCustomerForosh")]
        public string GetCustomerForosh()
        {

            string query = $"Exec spCustomerForosh_web";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }







        [HttpGet]
        [Route("GetBrokers")]
        public string GetBrokers()
        {

            string query = $"select BrokerCode , CentralRef,BrokerNameWithoutType from vwSellBroker";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }
        


        [HttpGet]
        [Route("GetBrokerDetail")]
        public string GetBrokerDetail(string BrokerCode)
        {

            string query = $"spWeb_BrokerDetail {BrokerCode} ";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }


        
        [HttpGet]
        [Route("GetBrokerReportDetail")]
        public string GetBrokerReportDetail(string BrokerCode)
        {

            string query = $"spWeb_BrokerReportDays {BrokerCode} ";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }
        
        
        [HttpGet]
        [Route("GetPrefactorBroker")]
        public string GetPrefactorBroker(string BrokerCode,string Days)
        {

            string query = $"Declare @D varchar(10)= dbo.fnDate_ConvertToShamsi(dateadd(d, -{Days}, getdate())) Select CustName,sum(RowsCount) RowsCount, Sum(SumAmount) SumAmount, Sum(SumPrice) SumPrice From vwPreFactor Where BrokerRef={BrokerCode} And PreFactorDate >= @D Group By CustName";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("Gettracker")]
        public string Gettracker(string BrokerCode, string StartDate, string EndDate)
        {

            string query = $"Select * From(select * , rwn=row_Number() over (partition by gpsdate order by gpsdate)From GpsLocation Where Brokerref = {BrokerCode} And GpsDate between '{StartDate}' And '{EndDate}' ) ds where rwn=1 order by GpsDate ";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetCDCustName")]
        public string GetCDCustName(string BrokerCode, string Days)
        {

            string query = $"spWeb_GetBrokerChartData {BrokerCode} ,  {Days} , 'CustName ' ,'CustName, '''' PreFactorDate' ";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }





        [HttpGet]
        [Route("GetCDPreFactorDate")]
        public string GetCDPreFactorDate(string BrokerCode, string Days)
        {

            string query = $"spWeb_GetBrokerChartData {BrokerCode} ,  {Days} , 'PreFactorDate ' ,'PreFactorDate, '''' CustName' ";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("ExistUser")]
        public string ExistUser(string UName, string UPass)
        {

            string query = $"Exec spapp_IsXUser  '{UName}','{UPass}'";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }


        


        [HttpGet]
        [Route("ChangeXUserPassword")]
        public string ChangeXUserPassword(string UName, string UPass, string NewPass)
        {

            string query = $"Exec spApp_ChangeXUserPassword  '{UName}','{UPass}','{NewPass}'";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpGet]
        [Route("LetterInsert")]
        public string LetterInsert(string LetterDate, string title, string Description, string CentralRef)
        {

            string query = $"exec dbo.spAutLetter_Insert @LetterDate='{LetterDate}', @InOutFlag=0,@Title ='{title}', @Description='{Description}',@State ='درحال انجام',@Priority ='عادي', @ReceiveType ='دستي', @CreatorCentral =28, @OwnerCentral ={CentralRef} ";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpGet]
        [Route("GetLetterList")]
        public string GetLetterList(string SearchTarget, string CentralRef, string CreationDate)
        {

            string Where = "";

            if (!string.IsNullOrEmpty(SearchTarget))
            {
                Where = $"(LetterTitle like '%{SearchTarget}%' or LetterDescription like '%{SearchTarget}%' or ds.RowExecutorName like '%{SearchTarget}%')";
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
                    Where += $" And LetterDate>'{CreationDate}'";
                }
                else
                {
                    Where = $"LetterDate>'{CreationDate}'";
                }
            }

            string query = $"Exec spWeb_AutLetterList '{Where}'";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("GetLetterRowList")]
        public string GetLetterRowList(string LetterRef)
        {

            string query = $"select  LetterRowCode,Name RowExecutorName,LetterRef ,LetterDate RowLetterDate,LetterDescription LetterRowDescription, LetterState LetterRowState, ExecutorCentralRef RowExecutorCentralRef from vwautletterrow join central on CentralCode=ExecutorCentralRef where LetterRef = {LetterRef} order by LetterRowCode desc";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }






        [HttpGet]
        [Route("GetLetterFromPersoninfo")]
        public string GetLetterFromPersoninfo(string PersonInfoCode)
        {

            string query = $"spWeb_AutLetterListByPerson {PersonInfoCode}";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("Web_ActivationCode")]
        public string Web_ActivationCode(string ActivationCode)
        {

            string query = $"select * from AppBrokerCustomer Where ActivationCode = '{ActivationCode}'";


            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }


        [HttpGet]
        [Route("Web_GetDbsetupObject")]
        public string Web_GetDbsetupObject(string Where)
        {
            string order = "";

            if (Where == "AppBroker_ActivationCode")
            {
                order = " where KeyValue in ('AppBroker_MenuGroupCode', 'App_FactorTypeInKowsar', 'AppBroker_DefaultGroupCode', 'PreFactor_UsePriceTip', 'PreFactor_IsReserved')";
            }
            else if (Where == "AppOcr_ActivationCode")
            {
                order = " where KeyValue like '%appocr%'";
            }
            else if (Where == "AppOrder_ActivationCode")
            {
                order = " where KeyValue like '%apporder%' or KeyValue like '%rstfactor%'";
            }

            string query = "select KeyValue, DataValue, Description from DbSetup " + order;

            DataTable dataTable = db.ExecQuery(query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }








        public byte[] GetImageData(String query)
        {
            byte[] imageData = null;

            DataTable dataTable = db.ImageExecQuery(query);
            if (DataTable.Rows.Count > 0)
            {
                if (!Convert.IsDBNull(DataTable.Rows[0][0]))
                {
                    imageData = (byte[])DataTable.Rows[0]["IMG"];
                }
            }

            return imageData;
        }





        /*
        [HttpGet]
        [Route("WebImageConversation")]
        public void WebImageConversation()
        {
            
            string ObjectId = "0";
            string LetterRef = HttpContext.Current.Request["LetterRef"] ?? "";
            string CentralRef = HttpContext.Current.Request["CentralRef"] ?? "";
            string ConversationText = HttpContext.Current.Request["ConversationText"] ?? "";

            string sq = $"Exec spWeb_AutLetterConversation_Insert @LetterRef='{LetterRef}', @CentralRef='{CentralRef}', @ConversationText='Image'";

            List<Dictionary<string, object>> ClassResult = database.custom_sqlSRV(sq, true);

            if (ClassResult.Count > 0)
            {
                ObjectId = ClassResult[0]["ConversationCode"].ToString();
            }

            string Image = HttpContext.Current.Request["Image"];
            byte[] decodedImage = Convert.FromBase64String(Image);
            string imagePath = HttpContext.Current.Server.MapPath("~/LetterImage/" + ObjectId + ".jpg");
            File.WriteAllBytes(imagePath, decodedImage);

            sq = $"Exec spImageImport 'Aut', {ObjectId}, '{imagePath}'; select @@IDENTITY KsrImageCode";
            MainClass.LogFile("WebImageConversation", sq);
            List<Dictionary<string, object>> response = database.custom_imgSRV(sq, true);

            string Last = JsonConvert.SerializeObject(response, Formatting.None);
            HttpContext.Current.Response.Write(Last);
            string filename = HttpContext.Current.Server.MapPath("~/LetterImage/" + ObjectId + ".jpg");
            File.Delete(filename);
            
        }






        [HttpGet]
        [Route("getWebImage")]
        public string getWebImage(
            string ObjectRef,
            string IX,
            string Scale,
            string ClassName
            )
        {

            
            int sScale = Convert.ToInt32(Scale);

            string sq = $"Exec dbo.spApp_GetImage {ObjectRef}, {IX}, '{ClassName}'";

            byte[] imageBytes = GetImageData(sq, _configuration);


            if (imageBytes != null)
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (Image image = Image.FromStream(ms))
                    {
                        int cropWidth = image.Width;
                        int cropHeight = image.Height;

                        if (cropWidth > cropHeight)
                        {
                            float ratio = (float)cropWidth / cropHeight;
                            cropWidth = sScale;
                            cropHeight = (int)(cropWidth / ratio);
                        }
                        else
                        {
                            float ratio = (float)cropHeight / cropWidth;
                            cropHeight = sScale;
                            cropWidth = (int)(cropHeight / ratio);
                        }

                        using (Image resizedImage = new Bitmap(cropWidth, cropHeight))
                        {
                            using (Graphics graphics = Graphics.FromImage(resizedImage))
                            {
                                graphics.DrawImage(image, 0, 0, cropWidth, cropHeight);
                            }

                            using (MemoryStream outputMs = new MemoryStream())
                            {
                                resizedImage.Save(outputMs, ImageFormat.Jpeg);
                                byte[] resizedImageBytes = outputMs.ToArray();

                                string encodedImage = Convert.ToBase64String(resizedImageBytes);
                                response.StatusCode = "200";
                                response.Errormessage = "";

                                jsonDict.Add("response", JsonConvert.SerializeObject(response));
                                jsonDict.Add("Text", encodedImage);
                                //jsonDict.Add("SellBrokers", jsonClass.ConvertDataTableToJson(dataTable));

                                return JsonConvert.SerializeObject(jsonDict);
                            }
                        }
                    }
                }
            }
            else
            {
                response.StatusCode = "200";
                response.Errormessage = "";

                jsonDict.Add("response", JsonConvert.SerializeObject(response));
                jsonDict.Add("Text", "no_photo");
                //jsonDict.Add("SellBrokers", jsonClass.ConvertDataTableToJson(dataTable));

                return JsonConvert.SerializeObject(jsonDict);
            }
            
        }



        */






    }
}
