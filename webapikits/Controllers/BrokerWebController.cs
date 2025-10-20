using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrokerWebController : ControllerBase
    {
        //public readonly IConfiguration _configuration;
        //DataBaseClass db;
        //DataTable DataTable = new DataTable();
        //string Query = "";
        //Response response = new();
        //JsonClass jsonClass = new JsonClass();
        //Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        //public BrokerWebController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    db = new DataBaseClass(_configuration);

        //}


        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public BrokerWebController(
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





        [HttpGet]
        [Route("GetWebImagess")]
        public async Task<IActionResult> GetWebImagess(string pixelScale, string ClassName, string ObjectRef)
        {
            string query = $"SELECT * FROM KsrImage WHERE Classname = '{ClassName}' AND ObjectRef = {ObjectRef} order by 1 desc";


            try
            {
                DataTable dataTable = await db.Image_ExecQuery(HttpContext, query);
                string json = jsonClass.ConvertAndScaleImageToBase64(Convert.ToInt32(pixelScale), dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetWebImagess));
                return StatusCode(500, "Internal server error.");
            }

        }

        /// <summary>

        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> UploadImage([FromBody] ksrImageModeldto data)
        {




                // Decode the base64 string to bytes
                byte[] decodedImage = Convert.FromBase64String(data.image);

                // Save the image bytes to a file

                string filePath = _configuration.GetConnectionString("web_imagePath") + $"{data.ObjectCode}.jpg"; // Provide the path where you want to save the image

                System.IO.File.WriteAllBytes(filePath, decodedImage);


                string query = $"Exec spImageImport  '{data.ClassName}',{data.ObjectCode},'{filePath}' ;select @@IDENTITY KsrImageCode";


     
                try
                {
                    DataTable dataTable = await db.Image_ExecQuery(HttpContext, query);
                    string json = jsonClass.JsonResult_Str(dataTable, "Ok", "Ok");
                    return Content(json, "application/json");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in {Function}", nameof(UploadImage));
                    return StatusCode(500, "Internal server error.");
                }

        }


        [HttpGet]
        [Route("GetBrokers")]
        public async Task<IActionResult> GetBrokers()
        {

            string query = $"select Explain,RelationType,BrokerCode , CentralRef,BrokerNameWithoutType from vwSellBroker where active=0";


            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetBrokers));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetBrokerDetail")]
        public async Task<IActionResult> GetBrokerDetail(string BrokerCode)
        {

            string query = $"spWeb_BrokerDetail {BrokerCode} ";


            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetBrokerDetail));
                return StatusCode(500, "Internal server error.");
            }

        }







        [HttpPost]
        [Route("GetAppBrokerReport")]
        public async Task<IActionResult> GetAppBrokerReport([FromBody] BrokerWebDto brokerWebDto)
        {

            // flag report
            //   
            //  1- GetCDPreFactorDate
            //  2- GetCDCustName
            //  3- GetPrefactorBroker
            //  4-

            string query1 = $"select DataValue From DBSetup with(nolock) Where KeyValue = 'AppBroker_FactorType'";
            DataTable dataTable1 = await db.Broker_ExecQuery(HttpContext, query1);
            string FactorType = dataTable1.Rows[0]["DataValue"] + "";



            if (brokerWebDto.Flag == "1")
            {

                brokerWebDto.GroupField = FactorType+"Date";
                brokerWebDto.Columns = FactorType+ "Date as ReportDate, '''' CustName";
            }
            else if (brokerWebDto.Flag == "2")
            {
                brokerWebDto.GroupField = "CustName";
                brokerWebDto.Columns = "CustName, '''' ReportDate";
            }
            else if (brokerWebDto.Flag == "3")
            {
                brokerWebDto.GroupField = "";
                brokerWebDto.Columns = "";
            }
            else if (brokerWebDto.Flag == "4")
            {
                brokerWebDto.GroupField = "";
                brokerWebDto.Columns = "";
            }
            else {
                brokerWebDto.GroupField = "";
                brokerWebDto.Columns = "";
            }


                string query = $"spWeb_GetAppBrokerReport @BrokerRef={brokerWebDto.BrokerRef}, @GroupField='{brokerWebDto.GroupField}', " +
                        $"@Columns='{brokerWebDto.Columns}', @startDate='{brokerWebDto.StartDate}', @endDate='{brokerWebDto.EndDate}', @Flag ={brokerWebDto.Flag}";


            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAppBrokerReport));
                return StatusCode(500, "Internal server error.");
            }

        }
















        /*
        [HttpGet]
        [Route("CreateAppBroker")]
        public async Task<IActionResult> CreateAppBroker(string KowsarDb, string KowsarImage)
        {

            db.Broker_ExecQuery(HttpContext, "DROP DATABASE IF EXISTS Appbroker");

            string query = $"{KowsarDb}..spApp_BrokerRep 'Appbroker','{KowsarImage}'";


            DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);

            return TestImport();

        }


        */


        //[HttpGet]
        //[Route("TestImport")]
        //public async Task<IActionResult> TestImport()
        //{

        //    db.TestImportedit();

        //    DataTable dataTable = db.Broker_ExecQuery(HttpContext, "select  count (*) count from Good");
        //    return jsonClass.JsonResultWithout_Str(dataTable);
        //    try
        //    {
        //        DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
        //        string json = jsonClass.JsonResultWithout_Str(dataTable);
        //        return Content(json, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred in {Function}", nameof(TestImport));
        //        return StatusCode(500, "Internal server error.");
        //    }

        //}

        [HttpGet]
        [Route("BrokerCustomerRefresh")]
        public async Task<IActionResult> BrokerCustomerRefresh()
        {
            string query = "Insert Into BrokerCustomer(BrokerRef, CustomerRef, Owner, CreationDate, Reformer, ReformDate)" +
                " Select BrokerCode, CustomerCode,1, GetDate(),1, GetDate() From Customer c Join SellBroker b on 1=1" +
                " Where c.Active<2 And b.Active<2 And Not Exists(Select 1 from BrokerCustomer s where s.BrokerRef=b.BrokerCode and s.CustomerRef=c.CustomerCode)";

            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BrokerCustomerRefresh));
                return StatusCode(500, "Internal server error.");
            }

        }


        /// <returns></returns>




        [HttpGet]
        [Route("BasketColumnCard")]
        public async Task<IActionResult> BasketColumnCard(string Where, string AppType)
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




            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BasketColumnCard));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("Web_GetDbsetupObject")]
        public async Task<IActionResult> Web_GetDbsetupObject(string Where)
        {
            string query = "";


            if (Where == "BrokerKowsar")
            {
                query = " select KeyId,KeyValue,DataValue,Description,SubSystem from DbSetup where KeyValue ='PreFactor_IsReserved' or KeyValue='PreFactor_UsePriceTip' or KeyValue='App_FactorTypeInKowsar' or KeyValue like '%appbroker%' ";
            }
            


            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Web_GetDbsetupObject));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("CreateBasketColumn")]
        public async Task<IActionResult> CreateBasketColumn(string AppType)
        {
            string query = "";





             if (AppType == "1")  // BrokerKowsar
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





            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CreateBasketColumn));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("GetBasketColumnList")]
        public async Task<IActionResult> GetBasketColumnList(string AppType)
        {


            string query = $" select * from AppBasketColumn Where AppType ={AppType} ";

            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetBasketColumnList));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetGoodType")]
        public async Task<IActionResult> GetGoodType()
        {

            string query = "Exec [spApp_GetGoodType]";



            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodType));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpGet]
        [Route("GetProperty")]
        public async Task<IActionResult> GetProperty(string Where)
        {

            string query = $" Select  PropertySchema,PropertyValueMap,PropertyName  from PropertySchema Where ClassName = 'TGOOD' And  ObjectType = '{Where}'";



            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetProperty));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpGet]
        [Route("InsertSingleColumn")]
        public async Task<IActionResult> InsertSingleColumn(
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


            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);

            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(InsertSingleColumn));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpGet]
        [Route("UpdateDbSetup")]
        public async Task<IActionResult> UpdateDbSetup(
            string DataValue,
            string KeyId)

        {

            string query = $" update dbsetup set DataValue = '{DataValue}'  where keyid = {KeyId}";


            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UpdateDbSetup));
                return StatusCode(500, "Internal server error.");
            }


        }




        [HttpGet]
        [Route("GetAppPrinter")]
        public async Task<IActionResult> GetAppprinter(string AppType)

        {

            string query = $"select * from AppPrinter Where AppType={AppType}";


            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);

            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAppprinter));
                return StatusCode(500, "Internal server error.");
            }

        }






        [HttpPost]
        [Route("UpdatePrinter")]
        public async Task<IActionResult> UpdatePrinter([FromBody] AppPrinterDto printerDto)
        {


            string query = "";

            if (printerDto.AppPrinterCode == "0")
            {
                query = $" Insert Into AppPrinter ( [PrinterName], [PrinterExplain], [GoodGroups], [WhereClause], [PrintCount], [PrinterActive], [FilePath], [AppType] ) values ('{printerDto.PrinterName}', '{printerDto.PrinterExplain}', '{printerDto.GoodGroups}', '{printerDto.WhereClause}', '{printerDto.PrintCount}', '{printerDto.PrinterActive}', '{printerDto.FilePath}', '{printerDto.AppType}') ";

            }
            else
            {
                query = $" Update AppPrinter set [PrinterName] = '{printerDto.PrinterName}', [PrinterExplain]= '{printerDto.PrinterExplain}', [GoodGroups]= '{printerDto.GoodGroups}', [WhereClause]= '{printerDto.WhereClause}', [PrintCount]= '{printerDto.PrintCount}', [PrinterActive]= '{printerDto.PrinterActive}', [FilePath]= '{printerDto.FilePath}', [AppType] = '{printerDto.AppType}' Where AppPrinterCode = {printerDto.AppPrinterCode}";
            }


            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UpdatePrinter));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("GetGpstracker")]
        public async Task<IActionResult> GetGpstracker(string BrokerCode, string StartDate, string EndDate)
        {

            string query = $" Exec spWeb_GetGpstracker  '{StartDate}' , '{EndDate}',{BrokerCode}  ";


            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResult_Str(dataTable, "Gpstrackers", "");

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Gpstrackers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGpstracker));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetBrokerCustomer")]
        public async Task<IActionResult> GetBrokerCustomer(string BrokerCode, string FactorDate)
        {

            string query = $" Exec spWeb_GetBrokerCustomer  '{FactorDate}',{BrokerCode}  ";


            //DataTable dataTable = db.Broker_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResult_Str(dataTable, "BrokerCustomer", "");

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "BrokerCustomer", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetBrokerCustomer));
                return StatusCode(500, "Internal server error.");
            }

        }





    }
}






