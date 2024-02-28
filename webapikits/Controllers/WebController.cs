using System.Data;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spire.Pdf.OPC;
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

            DataTable dataTable = db.ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("GetAppBrokerCustomerByCode")]
        public string GetAppBrokerCustomerByCode(string AppBrokerCustomerCode)
        {

            string query = $"select * from AppBrokerCustomer Where AppBrokerCustomerCode = '{AppBrokerCustomerCode}'";

            DataTable dataTable = db.ExecQuery( Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);





        }


        public class BrokerCustomerDto
        {

            
            
            
            public string? ActivationCode { get; set; }
            public string? EnglishCompanyName { get; set; }
            public string? PersianCompanyName { get; set; }
            public string? ServerURL { get; set; }
            public string? SQLiteURL { get; set; }
            public string? MaxDevice { get; set; }
            public string? SecendServerURL { get; set; }
            public string? DbName { get; set; }
            public string? AppType { get; set; }
        }







        [HttpPost]
        [Route("InsertAppBrokerCustomer")]
        public string InsertAppBrokerCustomer([FromBody] BrokerCustomerDto brokercustomerdto )
        {

            string query = $"exec [spApp_InsertAppBrokerCustomer] '{brokercustomerdto.ActivationCode}', '{brokercustomerdto.EnglishCompanyName}', '{brokercustomerdto.PersianCompanyName}', '{brokercustomerdto.ServerURL}'," +
                $" '{brokercustomerdto.SQLiteURL}', {brokercustomerdto.MaxDevice}, '{brokercustomerdto.SecendServerURL}' , '{brokercustomerdto.DbName}', {brokercustomerdto.AppType} ";


            DataTable dataTable = db.ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }






        [HttpPost]
        [Route("UpdateAppBrokerCustomer")]
        public string UpdateAppBrokerCustomer([FromBody] BrokerCustomerDto brokercustomerdto)
        {

            string query = $"exec [spApp_UpdateAppBrokerCustomer] '{brokercustomerdto.ActivationCode}', '{brokercustomerdto.EnglishCompanyName}', '{brokercustomerdto.PersianCompanyName}', " +
                $"'{brokercustomerdto.ServerURL}', '{brokercustomerdto.SQLiteURL}', {brokercustomerdto.MaxDevice}, '{brokercustomerdto.SecendServerURL}' , '{brokercustomerdto.DbName}', {brokercustomerdto.AppType} ";


            DataTable dataTable = db.ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }





        [HttpGet]
        [Route("GetActiveApplication")]
        public string GetActiveApplication(string Filter)
        {

            string query = $"Select Server_Name, STRING_AGG([Broker],',') within group (order by case when isnumeric([Broker])=1 then cast([Broker] as decimal) else 0 end, [Broker] ) as BrokerStr From (select Server_Name, Device_Id, [Broker] from app_info where DATEDIFF(m,Updatedate,GETDATE())<{Filter} group by Server_Name, Device_Id, [Broker]) ds group by Server_Name";


            DataTable dataTable = db.ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }






        [HttpGet]
        [Route("GetWebLog")]
        public string GetWebLog()
        {

            string query = $"select top 50 * from WebLog order by 1 desc";


            DataTable dataTable = db.ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("InsertwebLog")]
        public string InsertwebLog(string ClassName, string TagName, string LogValue)
        {

            string query = $"exec sp_WebLogInsert @ClassName='{ClassName}',@TagName='{TagName}',@LogValue='{LogValue}'";


            DataTable dataTable = db.ExecQuery( Request.Path, query);

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


            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);


        }





        [HttpGet]
        [Route("LetterInsert")]
        public string LetterInsert(string LetterDate, string title, string Description, string CentralRef)
        {

            string query = $"exec dbo.spAutLetter_Insert @LetterDate='{LetterDate}', @InOutFlag=0,@Title ='{title}', @Description='{Description}',@State ='درحال انجام',@Priority ='عادي', @ReceiveType ='دستي', @CreatorCentral =28, @OwnerCentral ={CentralRef} ";


            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpGet]
        [Route("GetLetterRowList")]
        public string GetLetterRowList(string LetterRef)
        {

            string query = $"select  LetterRowCode,Name RowExecutorName,LetterRef ,LetterDate RowLetterDate,LetterDescription LetterRowDescription, LetterState LetterRowState, ExecutorCentralRef RowExecutorCentralRef from vwautletterrow join central on CentralCode=ExecutorCentralRef where LetterRef = {LetterRef} order by LetterRowCode desc";


            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetCentralUser")]
        public string GetCentralUser()
        {

            string query = $"select CentralCode,CentralName from vwCentralUser ";


            DataTable dataTable = db.ExecQuery(Request.Path, query);

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

            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpGet]
        [Route("SetAlarmOff")]
        public string SetAlarmOff(
           string LetterRowCode
            )
        {

            string query = $" Update AutLetterRow Set AlarmActive=0 Where LetterRowCode={LetterRowCode} ";

            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetAutConversation")]
        public string GetAutConversation(
           string LetterRef
            )
        {

            string query = $"Exec spWeb_GetAutConversation  {LetterRef}";

            DataTable dataTable = db.ExecQuery(Request.Path, query);

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

            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }







        [HttpGet]
        [Route("GetLetterFromPersoninfo")]
        public string GetLetterFromPersoninfo(string PersonInfoCode)
        {

            string query = $"spWeb_AutLetterListByPerson {PersonInfoCode}";

            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }





        /// <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>


        [HttpGet]
        [Route("GetBrokers")]
        public string GetBrokers()
        {

            string query = $"select Explain,RelationType,BrokerCode , CentralRef,BrokerNameWithoutType from vwSellBroker where active=0";


            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }




        [HttpGet]
        [Route("GetBrokerDetail")]
        public string GetBrokerDetail(string BrokerCode)
        {

            string query = $"spWeb_BrokerDetail {BrokerCode} ";


            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }








        [HttpGet]
        [Route("GetPrefactorBroker")]
        public string GetPrefactorBroker(string BrokerCode, string Days)
        {

            string query = $"Declare @D varchar(10)= dbo.fnDate_ConvertToShamsi(dateadd(d, -{Days}, getdate())) Select CustName,sum(RowsCount) RowsCount, Sum(SumAmount) SumAmount, Sum(SumPrice) SumPrice From vwPreFactor Where BrokerRef={BrokerCode} And PreFactorDate >= @D Group By CustName order by RowsCount desc";


            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }






        [HttpGet]
        [Route("GetCDCustName")]
        public string GetCDCustName(string BrokerCode, string Days)
        {

            string query = $"spWeb_GetBrokerChartData {BrokerCode} ,  {Days} , 'CustName ' ,'CustName, '''' PreFactorDate' ";


            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }









        [HttpGet]
        [Route("GetCDPreFactorDate")]
        public string GetCDPreFactorDate(string BrokerCode, string Days)
        {

            string query = $"spWeb_GetBrokerChartData {BrokerCode} ,  {Days} , 'PreFactorDate ' ,'PreFactorDate, '''' CustName' ";


            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }



        [HttpGet]
        [Route("CreateAppBroker")]
        public string CreateAppBroker(string KowsarDb, string KowsarImage)
        {

            db.ExecQuerytestDb(Request.Path, "DROP DATABASE IF EXISTS Appbroker");

            string query = $"{KowsarDb}..spApp_BrokerRep 'Appbroker','{KowsarImage}'";


            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);

            return TestImport();

        }


        [HttpGet]
        [Route("TestImport")]
        public string TestImport()
        {

            db.TestImportedit();

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, "select  count (*) count from Good");
            return jsonClass.JsonResultWithout_Str(dataTable);

        }












        [HttpGet]
        [Route("Web_GetDbsetupObject")]
        public string Web_GetDbsetupObject(string Where)
        {
            string query = "";
            

            if (Where == "BrokerKowsar")
            {
                query = " select KeyId,KeyValue,DataValue,Description,SubSystem from DbSetup where KeyValue ='PreFactor_IsReserved' or KeyValue='PreFactor_UsePriceTip' or KeyValue='App_FactorTypeInKowsar' or KeyValue like '%appbroker%' ";
            }
            else if (Where == "Ocrkowsar")
            {
                query = " select KeyId,KeyValue,DataValue,Description,SubSystem from DbSetup where KeyValue like '%appocr%'";
            }
            else if (Where == "OrderKowsar")
            {
                query = " select KeyId,KeyValue,DataValue,Description,SubSystem from DbSetup where  KeyValue like '%apporder%' or KeyValue like'%rstfactor%' ";
            }
            else if (Where == "Company")
            {
                query = " select KeyId,KeyValue,DataValue,Description,SubSystem from DbSetup where KeyValue like '%appbasket%'";
            }


            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }

        [HttpGet]
        [Route("CreateBasketColumn")]
        public string CreateBasketColumn(string AppType)
        {
            string query = "";
            
            
            
            
            if (AppType == "0")  //Company
            {
                query = 
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodCode'          ,'كد سيستمي',''  ,'','0'  ,'1','1'  ,'0'  ,'','0'  ,'','0' " +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodName'          ,'نام كالا','GoodExplain5'  ,'','0'  ,'1','2'  ,'1'  ,'','0'  ,'','0' " +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodType'          ,'نوع كالا',''  ,'','0'  ,'1','3'  ,'0'  ,'','0'  ,'','0' " +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'MaxSellPrice'      ,'في ناخالص',''  ,'','0'  ,'1','4'  ,'0'  ,'','0'  ,'','0' " +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'SellPrice'         ,'في','Cast(Case @C When 1 Then SellPrice1Str When 2 Then SellPrice2Str When 3 Then SellPrice3Str When 4 Then SellPrice4Str When 5 Then SellPrice5Str When 6 Then SellPrice6Str When 7 Then MaxSellPrice*(100-@Takhf)/100 Else MaxSellPrice End as bigInt)'  ,'','0'  ,'1','5'  ,'0'  ,'','0'  ,'','0' " +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodMainCode'      ,'كد محصول',''  ,'','1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodSubCode'       ,'كدفرعي',''  ,'','1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodExplain2'      ,'مشخصه2',''  ,'','0'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodExplain3'      ,'مشخصه3',''  ,'','0'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodExplain4'      ,'مشخصه4',''  ,'','0'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodExplain5'      ,'مشخصه5',''  ,'','0'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodExplain6'      ,'مشخصه6',''  ,'','0'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'Barcode'           ,'باركد',''  ,'','1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'ISBN'              ,'شابك',''  ,'','1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'ImageCount'        ,'تعداد عكس',''  ,'','0'  ,'1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'BulletinGroupName' ,'گروه كالا',''  ,'','1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GroupsWhitoutCode' ,'موضوع',''  ,'','1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'HasStackAmount'    ,'موجودي','Case When @IsReserved in(0,2) Then 1 When @IsReserved=1 And IsNull( b.FacAmount ,0)<Amount-ReservedAmount-@MinAmount Then 1 Else 0 End'  ,'','0'  ,'1','1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'IsFavorite'        ,'علاقه مندي','Case When Exists(Select Top 1 1 From AppFavorite f Where f.GoodRef = GoodCode And f.MobileNo=@MobileNo) Then 1 Else 0 End'  ,'','0'  ,'1','1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodImageName'     ,'نام عكس',''''  ,'','0'  ,'-1','0'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'IsReserved'        ,'رزرو','@IsReserved'  ,'','-1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'BasketAmount'      ,'تعداد سفارش','Cast( IsNull( b.FacAmount ,0) as bigInt)'  ,'','1'  ,'1','0'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'IsReserved'        ,'رزرو','isnull(Cast (R.IsReserved as int ), @IsReserved)'  ,'','-1'  ,'-1','0'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'FacAmount'         ,'تعداد','ISNULL (R.facamount  ,b.facamount)'  ,'','-1'  ,'-1','8'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'Price'             ,'في','ISNULL (R.Price,b.Price)'  ,'','-1'  ,'-1','9'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'TotalAmount'       ,'جمع كل','IsNull((Select Sum(sks.Amount) From GoodStack sks Where sks.StackRef in(10110) And sks.GoodRef= GoodCode ),0)'  ,'','0'  ,'0','0'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'MinPrice'          ,'از قيمت','Cast(Case @C When 1 Then SellPrice1Str When 2 Then SellPrice2Str When 3 Then SellPrice3Str When 4 Then SellPrice4Str When 5 Then SellPrice5Str When 6 Then SellPrice6Str When 7 Then MaxSellPrice*(100-@Takhf)/100 Else MaxSellPrice End as bigInt)'  ,'','-1'  ,'-1','-1'  ,'20'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'MaxPrice'          ,'تا قيمت','Cast(Case @C When 1 Then SellPrice1Str When 2 Then SellPrice2Str When 3 Then SellPrice3Str When 4 Then SellPrice4Str When 5 Then SellPrice5Str When 6 Then SellPrice6Str When 7 Then MaxSellPrice*(100-@Takhf)/100 Else MaxSellPrice End as bigInt)'  ,'','-1'  ,'-1','-1'  ,'21'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'Active'            ,'فعال','g.active'  ,'','-1'  ,'0','-1'  ,'-1'  ,'','0'  ,'','0' Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'bUnitRef','واحد خريد',''  ,'','-1'  ,'-1','0'  ,'-1'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'bRatio'            ,'واحدتعداد',''  ,'','-1'  ,'-1','0'  ,'-1'  ,'','0'  ,'','0'";
            }
            else if(AppType == "1")  // BrokerKowsar
            {
                query =
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodCode','کد کالا',''  ,'','18'  ,'1','0'  ,'0'  ,'1','-2'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodName','نام کالا',''  ,'','2'  ,'2','2'  ,'3'  ,'0','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodExplain1','ناشر',''  ,'','3'  ,'3','-1'  ,'3'  ,'0','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'MaxSellPrice','قيمت',''  ,'','3'  ,'4','3'  ,'-1'  ,'1','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'DefaultUnitValue','DefaultUnitValue',''  ,'','-1'  ,'-1','0'  ,'-1'  ,'1','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'Isbn','Isbn',''  ,'','17'  ,'-1','-1'  ,'-1'  ,'0','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'UnitName','واحد اندازه گيري','u.UnitName'  ,'','-1'  ,'-1','0'  ,'-1'  ,'0','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'Price','قيمت','IfNull(pf.Price,0)'  ,'','-1'  ,'-1','0'  ,'-1'  ,'0','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GroupsWhitoutCode','گروه کالايي','IfNull(GroupsWhitoutCode,'''''''')'  ,'','1'  ,'-1','-1'  ,'-1'  ,'0','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'Shortage','','IfNull(pf.Shortage,0)'  ,'','-1'  ,'-1','0'  ,'-1'  ,'0','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'PrefactorRowCode','','pf.PrefactorRowCode'  ,'','-1'  ,'-1','0'  ,'-1'  ,'0','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'DefaultUnitValue','',''  ,'','-1'  ,'-1','-1'  ,'-1'  ,'0','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'SellPrice','قيمت خريد','Case c.PriceTip When 1 Then SellPrice1 When 2 Then SellPrice2  When 3 Then SellPrice3 When 4 Then SellPrice4 When 5 Then SellPrice5 When 6 Then SellPrice6   Else Case When g.SellPriceType = 0 Then MaxSellPrice Else 100 End End *  Case When g.SellPriceType = 0 Then 1 Else MaxSellPrice/100 End '  ,'','-1'  ,'-1','-1'  ,'-1'  ,'1','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'StackAmount','تعداد واقعي','(select Sum(Amount-ReservedAmount) from goodstack stackCondition and GoodRef=GoodCode)'  ,'','4'  ,'3','-1'  ,'-1'  ,'1','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'ActiveStack','فعال','Case When Exists(Select 1 From goodstack stackCondition and GoodRef=GoodCode) Then 1 Else 0 End'  ,'','0'  ,'0','-1'  ,'0'  ,'1','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'Date','تاريخ تحويل','Case When SecondField=1 Then g.Date2 Else g.Date2 End'  ,'','0'  ,'0','-1'  ,'0'  ,'0','-1'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'ksrImageCode','کد عکس','(Select ki.KsrImageCode From KsrImage ki Where ki.ObjectRef=g.GoodCode Order By ki.IsDefaultImage DESC, ki.KsrImageCode LIMIT 1)'  ,'','0'  ,'0','0'  ,'-1'  ,'1','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'FactorAmount','موجودي فاكتور','IfNull(pf.FactorAmount,0)'  ,'','-1'  ,'-1','0'  ,'-1'  ,'1','0'  ,'','1'" +
                    "  Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'Nvarchar2','مترجم','','کتاب','6','-1','-1','5'  ,'0','0'  ,'','1'";
            }
            else if (AppType == "2") // Ocrkowsar
            {
                query = "  select '' test ";
            }
            else if (AppType == "3") // OrderKowsar
            {
                query = " select '' test";
            }




            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }

        [HttpGet]
        [Route("GetBasketColumnList")]
        public string GetBasketColumnList(string AppType)
        {


            string query = $" select * from AppBasketColumn Where AppType ={AppType} ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }


        [HttpGet]
        [Route("BrokerCustomerRefresh")]
        public string BrokerCustomerRefresh()
        {
            string query = "Insert Into BrokerCustomer(BrokerRef, CustomerRef, Owner, CreationDate, Reformer, ReformDate)" +
                " Select BrokerCode, CustomerCode,1, GetDate(),1, GetDate() From Customer c Join SellBroker b on 1=1" +
                " Where c.Active<2 And b.Active<2 And Not Exists(Select 1 from BrokerCustomer s where s.BrokerRef=b.BrokerCode and s.CustomerRef=c.CustomerCode)";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }




        [HttpGet]
        [Route("BasketColumnCard")]
        public string BasketColumnCard(string Where,string AppType)
        {
            string query = "";



            if (Where == "ListVisible")  
            {
                query = $" select AppBasketColumnCode,ColumnName,ColumnDesc,ColumnDefinition,ObjectType, " +
                        $" ListVisible,ColumnType,OrderIndex,Condition,AppType from AppBasketColumn " +
                        $" where apptype ={AppType} and ListVisible > 0 And ObjectType='' order by ListVisible  ";
            }
            else if (Where == "DetailVisible")  
            {
                query = $" select AppBasketColumnCode,ColumnName,ColumnDesc,ColumnDefinition,ObjectType, " +
                        $" DetailVisible,ColumnType,OrderIndex,Condition,AppType from AppBasketColumn  " +
                        $" where apptype ={AppType} and DetailVisible > 0 And ObjectType='' order by DetailVisible  ";
            }
            else if (Where == "SearchVisible") 
            {
                query = $" select AppBasketColumnCode,ColumnName,ColumnDesc,ColumnDefinition,ObjectType, " +
                        $"SearchVisible,ColumnType,OrderIndex,Condition,AppType from AppBasketColumn " +
                        $" where apptype ={AppType} and SearchVisible > 0 And ObjectType='' order by SearchVisible  ";
            }
            



            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }










        [HttpGet]
        [Route("GetGoodType")]
        public string GetGoodType()
        {

            string query = "Exec [spApp_GetGoodType]";



            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("GetProperty")]
        public string GetProperty(string Where)
        {

            string query = $" Select  PropertySchema,PropertyValueMap,PropertyName  from PropertySchema Where ClassName = 'TGOOD' And  ObjectType = '{Where}'";



            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("InsertSingleColumn")]
        public string InsertSingleColumn(
            string ColumnName,
            string ColumnDesc,
            string ObjectType,
            string DetailVisible,
            string ListVisible,
            string SearchVisible,
            string ColumnType,
            string AppType
)
        {

            string query = 

                $" Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)" +
                $" Select '{ColumnName}','{ColumnDesc}','','{ObjectType}','{DetailVisible}','{ListVisible}','-1','{SearchVisible}','{ColumnType}','0','','{AppType}' ";


            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }





        [HttpGet]
        [Route("UpdateDbSetup")]
        public string UpdateDbSetup(
            string DataValue,
            string KeyId)

        {

            string query =  $" update dbsetup set DataValue = '{DataValue}'  where keyid = {KeyId}";


            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }




        [HttpGet]
        [Route("GetAppPrinter")]
        public string GetAppprinter(string AppType)

        {

            string query = $"select * from AppPrinter Where AppType={AppType}";


            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }









        public class AppPrinterDto
        {
            public string? AppPrinterCode { get; set; }
            public string? PrinterName { get; set; }
            public string? PrinterExplain { get; set; }
            public string? GoodGroups { get; set; }
            public string? WhereClause { get; set; }
            public string? PrintCount { get; set; }
            public string? PrinterActive { get; set; }
            public string? FilePath { get; set; }
            public string? AppType { get; set; }
        }




        

        [HttpPost]
        [Route("UpdatePrinter")]
        public string UpdatePrinter([FromBody] AppPrinterDto printerDto)
        {
        

            string query = "";

            if (printerDto.AppPrinterCode == "0")
            {
                query = $" Insert Into AppPrinter ( [PrinterName], [PrinterExplain], [GoodGroups], [WhereClause], [PrintCount], [PrinterActive], [FilePath], [AppType] ) values ('{printerDto.PrinterName}', '{printerDto.PrinterExplain}', '{printerDto.GoodGroups}', '{printerDto.WhereClause}', '{printerDto.PrintCount}', '{printerDto.PrinterActive}', '{printerDto.FilePath}', '{printerDto.AppType}') ";

            }
            else {
                query = $" Update AppPrinter set [PrinterName] = '{printerDto.PrinterName}', [PrinterExplain]= '{printerDto.PrinterExplain}', [GoodGroups]= '{printerDto.GoodGroups}', [WhereClause]= '{printerDto.WhereClause}', [PrintCount]= '{printerDto.PrintCount}', [PrinterActive]= '{printerDto.PrinterActive}', [FilePath]= '{printerDto.FilePath}', [AppType] = '{printerDto.AppType}' Where AppPrinterCode = {printerDto.AppPrinterCode }";
            }


            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }








        






        [HttpGet]
        [Route("OcrFactorList")]
        public string OcrFactorList(string SearchTarget)
        {


            string query = $"Exec dbo.spApp_ocrFactorList 4 , '{SearchTarget}' ,' ',50 ,0 , ' order by o.AppTcPrintRef desc'  ";




            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }







        [HttpGet]
        [Route("ocrGetFactorDetail")]
        public string ocrGetFactorDetail(string AppOCRFactorCode)
        {


            string query = $"Exec spApp_ocrGetFactorDetail {AppOCRFactorCode}  ";




            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }







        [HttpGet]
        [Route("ExitDelivery")]
        public string ExitDelivery(string AppOCRFactorCode)
        {


            string query = $"update AppOCRFactor set HasSignature = 0, AppIsDelivered = 0 where AppOCRFactorCode = {AppOCRFactorCode}  ";




            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }






        [HttpPost]
        [Route("Conversation_UploadImage")]
        public string Conversation_UploadImage([FromBody] ksrImageModel data)
        {


            try
            {


                string query1 = $"Exec spWeb_AutLetterConversation_Insert @LetterRef={data.LetterRef}, @CentralRef={data.CentralRef}, @ConversationText='Image'";

                DataTable dataTable1 = db.ExecQuery(Request.Path, query1);
                string Conversationref = dataTable1.Rows[0]["ConversationCode"]+"";




                // Decode the base64 string to bytes
                byte[] decodedImage = Convert.FromBase64String(data.image);


                // Save the image bytes to a file
                string filePath = $"E:\\KowsarAcc\\WebApiLocation\\Log_SaveStorage\\{Conversationref}.jpg"; // Adjust the file path as needed
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












        public class ksrImageModel
        {
            public string ObjectCode { get; set; }
            public string image { get; set; }
            public string ClassName { get; set; }
            public string LetterRef { get; set; }
            public string CentralRef { get; set; }
        }


        [HttpPost]
        [Route("UploadImage")]
        public string UploadImage([FromBody] ksrImageModel data)
        {


            try
            {


                // Decode the base64 string to bytes
                byte[] decodedImage = Convert.FromBase64String(data.image);

                // Save the image bytes to a file
                string filePath = $"E:\\KowsarAcc\\WebApiLocation\\Log_SaveStorage\\{data.ObjectCode}.jpg"; // Adjust the file path as needed
                System.IO.File.WriteAllBytes(filePath, decodedImage);


                string query = $"Exec spImageImport  '{data.ClassName}',{data.ObjectCode},'{filePath}' ;select @@IDENTITY KsrImageCode";


                DataTable dataTable = db.ImageExecQuery( query);

                return "Ok";
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";

            }
        }





        [HttpGet]
        [Route("GetWebImagess")]
        public string GetWebImagess(string pixelScale,string ClassName, string ObjectRef)
        {
            string query = $"SELECT * FROM KsrImage WHERE Classname = '{ClassName}' AND ObjectRef = {ObjectRef} order by 1 desc";


            DataTable dataTable = db.ImageExecQuery( query);

            
            return jsonClass.ConvertAndScaleImageToBase64(Convert.ToInt32(pixelScale), dataTable);

        }











        [HttpGet]
        [Route("OrderMizList")]
        public string OrderMizList(string InfoState, string MizType)
        {

            string query = $" exec spApp_OrderMizList  {InfoState}, N'{MizType}' ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }

        [HttpGet]
        [Route("GetAmountItem")]
        public string GetAmountItem(string Date, string State)
        {

            string query = $" spWeb_Getchartpanel '{Date}' ,{State} ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("GetTodeyFromServer")]
        public string GetTodeyFromServer(string day)
        {

            string query = $"select dbo.fnDate_AddDays(dbo.fnDate_Today(),{day}) TodeyFromServer  ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }


        [HttpGet]
        [Route("minMaxGood")]
        public string minMaxGood(string Date, string State)
        {

            string query = $" spweb_Getorderpanel '{Date}' ,{State} ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("GetCustomerMandeh")]
        public string GetCustomerMandeh()
        {

            string query = $" spWeb_GetCustomerMandeh ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("GetCustomerlastGood")]
        public string GetCustomerlastGood(string CustomerCode)
        {

            string query = $" spWeb_GetCustomerlastGood {CustomerCode} ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }




        [HttpGet]
        [Route("GetOrdergroupList")]
        public string GetOrdergroupList(string GroupCode)
        {

            string query = $"Exec  spApp_GetGoodGroups  @GroupCode= {GroupCode} ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }






        [HttpGet]
        [Route("kowsar_info")]
        public string kowsar_info(string Where)
        {

            string query = "select top 1 DataValue from dbsetup where KeyValue = '" + Where + "'";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);



        }





        public class SearchDto
        {
            public string? RowCount { get; set; }
            public string? Where { get; set; }
            public string? GroupCode { get; set; }
        }


        [HttpPost]
        [Route("GetOrderGoodList")]
        public string GetOrderGoodList([FromBody] SearchDto searchDto)
        {
            string searchtarget = searchDto.Where.Replace(" ", "%");

            string query = $"Exec spApp_GetGoods2 @RowCount = {searchDto.RowCount},@Where = N' GoodName like ''%{searchtarget}%''' ,@AppBasketInfoRef=0, @GroupCode = {searchDto.GroupCode} ,@AppType=3 ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("ChangeGoodActive")]
        public string ChangeGoodActive(string GoodCode, string ActiveFlag)
        {

            string query = $"spWeb_ChangeGoodActive {GoodCode},{ActiveFlag} ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }





        [HttpGet]
        [Route("GetGoodEdit")]
        public string GetGoodEdit(string Where)
        {

            string query = $"Select GoodCode,GoodName, CAST(MaxSellprice AS INT) MaxSellprice,GoodExplain1,GoodExplain2,GoodExplain3,GoodExplain4,GoodExplain5,GoodExplain6 from Good Where GoodCode = {Where} ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }

        [HttpGet]
        [Route("GetActiveGood")]
        public string GetActiveGood(string GoodCode)
        {

            string query = $"select ActiveStack,GoodRef from GoodStack where goodref = {GoodCode}  order by 1 desc ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }





        public class GoodDto
        {
            public string? GoodCode { get; set; }
            public string? GoodName { get; set; }
            public string? MaxSellPrice { get; set; }
            public string? GoodType { get; set; }
            public string? GoodExplain1 { get; set; }
            public string? GoodExplain2 { get; set; }
            public string? GoodExplain3 { get; set; }
            public string? GoodExplain4 { get; set; }
            public string? GoodExplain5 { get; set; }
            public string? GoodExplain6 { get; set; }


        }























        [HttpPost]
        [Route("Web_InsertGood")]
        public string Web_InsertGood([FromBody] GoodDto gooddto)
        {

            string query = $"Exec spWeb_InsertGood '{gooddto.GoodName}' , {gooddto.MaxSellPrice},'{gooddto.GoodExplain1}','{gooddto.GoodExplain2}','{gooddto.GoodExplain3}','{gooddto.GoodExplain4}','{gooddto.GoodExplain5}','{gooddto.GoodExplain6}' ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpPost]
        [Route("Web_UpdateGoodDetail")]
        public string Web_UpdateGoodDetail([FromBody] GoodDto gooddto)
        {

            string query = $"Exec spWeb_UpdateGoodDetail {gooddto.GoodCode},'{gooddto.GoodName}' , {gooddto.MaxSellPrice},'{gooddto.GoodExplain1}','{gooddto.GoodExplain2}','{gooddto.GoodExplain3}','{gooddto.GoodExplain4}','{gooddto.GoodExplain5}','{gooddto.GoodExplain6}' ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }






        [HttpGet]
        [Route("GetGroupFromGood")]
        public string GetGroupFromGood(string Where)
        {

            string query = $"select GoodGroupCode,GoodGroupRef, Name, GoodRef from GoodGroup join Goodsgrp  on GoodGroupRef = GroupCode where Goodref = {Where}  ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("GetGoodFromGroup")]
        public string GetGoodFromGroup(string Where)
        {

            string query = $"select GoodGroupCode, GoodName, GoodCode from Good join GoodGroup on goodref = GoodCode  where GoodGroupRef = {Where}  ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }


        [HttpGet]
        [Route("DeleteGoodGroupCode")]
        public string DeleteGoodGroupCode(string Where)
        {

            string query = $" delete from GoodGroup Where GoodGroupCode = {Where}  ";

            DataTable dataTable = db.ExecQuerytestDb(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }




































        [HttpGet]
        [Route("Gettracker")]
        public string Gettracker(string BrokerCode, string StartDate, string EndDate)
        {

            string query = $"Select * From(select * , rwn=row_Number() over (partition by gpsdate order by gpsdate)From GpsLocation Where Brokerref = {BrokerCode} And GpsDate between '{StartDate}' And '{EndDate}' ) ds where rwn=1 order by GpsDate ";


            DataTable dataTable = db.ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }








        [HttpGet]
        [Route("ExistUser")]
        public string ExistUser(string UName, string UPass)
        {

            string query = $"Exec spapp_IsXUser  '{UName}','{UPass}'";


            DataTable dataTable = db.ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

        }


        


        [HttpGet]
        [Route("ChangeXUserPassword")]
        public string ChangeXUserPassword(string UName, string UPass, string NewPass)
        {

            string query = $"Exec spApp_ChangeXUserPassword  '{UName}','{UPass}','{NewPass}'";


            DataTable dataTable = db.ExecQuery( Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);

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


        */






    }
}
