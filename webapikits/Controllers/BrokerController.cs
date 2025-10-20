using System.Data;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapikits.Model;



namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrokerController : ControllerBase
    {


        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public BrokerController(
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
        [Route("GetColumnList")]
        public async Task<IActionResult> GetColumnList(
            string Type,
            string AppType,
            string IncludeZero
            )
        {

            if (string.IsNullOrEmpty(Type))
            {
                Type = "0";
            }
            if (string.IsNullOrEmpty(AppType))
            {
                AppType = "0";
            }
            if (string.IsNullOrEmpty(IncludeZero))
            {
                IncludeZero = "0";
            }

            string query = $"Exec [spApp_GetColumn]  0  ,'', {Type},{AppType}, {IncludeZero}";


             


            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Columns", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetColumnList));
                return StatusCode(500, "Internal server error.");
            }




        }

        [HttpGet]
        [Route("BrokerStack")]
        public async Task<IActionResult> BrokerStack(string BrokerRef)
        {
            if (string.IsNullOrEmpty(BrokerRef))
            {
                BrokerRef = "0";
            }

            string query = $"exec spApp_GetBrokerStack {BrokerRef}" ;

             

            

            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "BrokerStack");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BrokerStack));
                return StatusCode(500, "Internal server error.");
            }

        }

        //[HttpGet]
        //[Route("getImageInfo")]
        //public string getImageInfo(string code)
        //{


        //    string query = $"Exec spApp_GetInfo 1, 'KsrImage', {code} , @RowCount=200, @CountFlag=1 ";

        //    DataTable dataTable = db.Web_ImageExecQuery(HttpContext,query);

        //    return jsonClass.JsonResult_Str(dataTable, "Text", "");


        //}

        [HttpGet]
        [Route("GetMenuBroker")]
        public async Task<IActionResult>GetMenuBroker()
        {


            string query = $"select DataValue from DbSetup where KeyValue='AppBroker_MenuGroupCode'";

             

            //return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetMenuBroker));
                return StatusCode(500, "Internal server error.");
            }


        }

        [HttpGet]
        [Route("GetMaxRepLog")]
        public async Task<IActionResult>GetMaxRepLog()
        {


            string query = $"Select top 1 * from RepLogData order by 1 desc";

             

            //return jsonClass.JsonResult_Str(dataTable, "Text", "RepLogDataCode");
            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "RepLogDataCode");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetMaxRepLog));
                return StatusCode(500, "Internal server error.");
            }


        }

        [HttpGet]
        [Route("RetrofitReplicate")]
        public async Task<IActionResult>RetrofitReplicate(
            string code,
            string table,
            string reptype,
            string Reprow
            )
        {


            string query = $"Exec spApp_GetInfo {reptype}, {table}, {code}, @RowCount={Reprow} , @CountFlag=1";
             


            try
            {
                DataTable dataTable;
                if (table.Equals("KsrImage"))
                {
                    dataTable = await db.Image_ExecQuery(HttpContext, query);
                }
                else
                {
                    dataTable = await db.Broker_ExecQuery(HttpContext, query);
                }

                string json = jsonClass.JsonResult_Str1(dataTable, "Text", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(RetrofitReplicate));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpPost]
        [Route("BrokerOrder")]
        public async Task<IActionResult>BrokerOrder([FromBody] BrokerOrderClass brokerOrderrequest)
        {

            List<HeaderDetail> headerDetails = brokerOrderrequest.HeaderDetails;
            List<RowDetail> rowDetails = brokerOrderrequest.RowDetails;
            

            string Explain = "BazaryabApp";

            var NotAmount = new List<Dictionary<string, object>>();

            string MobFDate = "";
            string factordate = "";

            int UserId = -1000;
            int factorcode = 0;
            int Customer = 0;
            int Broker = 0;
            int MobFCode = 0;
            int ExistFlag = 0;
            int CountRows = 0;
           
            string appBrokerFactorType = "";
            string appBrokerIsShopFactor = "";
            string appBrokerDefaultStackCode = "";
            string appBrokerMustHasAmount = "";

            MobFDate = headerDetails[0].PreFactorDate;
            Explain = headerDetails[0].PreFactorExplain;

            MobFCode = Convert.ToInt32(headerDetails[0].PreFactorCode);
            Customer = Convert.ToInt32(headerDetails[0].CustomerRef);
            Broker = Convert.ToInt32(headerDetails[0].BrokerRef);
            CountRows = Convert.ToInt32(headerDetails[0].RwCount);


            string query_dbsetup = "SELECT KeyValue, DataValue FROM DbSetup WHERE KeyValue IN ('AppBroker_FactorType', 'AppBroker_IsShopFactor', 'AppBroker_DefaultStackCode', 'AppBroker_MustHasAmount')";

            // اجرای کوئری و دریافت نتیجه به صورت DataTable
            DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query_dbsetup);

            // بررسی نتیجه
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    string keyValue = row["KeyValue"].ToString();
                    string dataValue = row["DataValue"].ToString();

                    // مقداردهی بر اساس KeyValue
                    switch (keyValue)
                    {
                        case "AppBroker_FactorType":
                            appBrokerFactorType = dataValue;
                            break;
                        case "AppBroker_IsShopFactor":
                            appBrokerIsShopFactor = dataValue;
                            break;
                        case "AppBroker_DefaultStackCode":
                            appBrokerDefaultStackCode = dataValue;
                            break;
                        case "AppBroker_MustHasAmount":
                            appBrokerMustHasAmount = dataValue;
                            break;
                    }
                }
            }



            string sq = $"Exec [dbo].[spPreFactor_Insert] '{appBrokerFactorType}', {appBrokerDefaultStackCode}, {UserId}, 0, '', {Customer}, '{Explain}', {Broker}, {MobFCode}, '{MobFDate}'";

            var result = await db.Broker_ExecQuery(HttpContext, sq);
            if (result != null)
            {
                factorcode = Convert.ToInt32(result.Rows[0]["PreFactorCode"]);
                factordate = result.Rows[0]["PreFactorDate"].ToString();
                ExistFlag = Convert.ToInt32(result.Rows[0]["ExistFlag"]);
            }

            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            DataTable dataTable3 = new DataTable();


            if (ExistFlag == 0){

                   DataTable notAmountTable = new DataTable();
                   notAmountTable.Columns.Add("GoodCode", typeof(int));
                   notAmountTable.Columns.Add("Flag", typeof(int));

                   foreach (RowDetail rowDetail_single in rowDetails)
                   {
              
                       int Code = Convert.ToInt32(rowDetail_single.GoodRef);
                       int Amount = Convert.ToInt32(rowDetail_single.FactorAmount);
                       int Price = Convert.ToInt32(rowDetail_single.Price);

                       string sqrow = $"Exec [dbo].[spPreFactor_InsertRow] '{appBrokerFactorType}', {factorcode}, {Code}, {Amount}, 0, {UserId}, '', {appBrokerMustHasAmount}, 0, {Price}";
                       var res = await db.Broker_ExecQuery(HttpContext, sqrow);
                       if (res != null)
                       {
                           int rowCode = Convert.ToInt32(res.Rows[0]["RowCode"]);
                           // Add rows to the DataTable based on the result
                           if (rowCode == -1 || rowCode == -2)
                           {
                               notAmountTable.Rows.Add(Code, rowCode);
                           }
                       }
                   }



            

                   if (notAmountTable.Rows.Count < 1){
                           string sq1 = $"Select Sum(FacAmount) rcount From {appBrokerFactorType}Rows Where {appBrokerFactorType}Ref = {factorcode}";


                           var res1 = await db.Broker_ExecQuery(HttpContext, sq1);
                           int rcount = Convert.ToInt32(res1.Rows[0]["rcount"]);

                           if (CountRows == rcount)    {

                                   string query = $"select  0 as GoodCode, {factorcode} as PreFactorCode ,{factordate} as PreFactorDate ,{ExistFlag} as ExistFlag ";



                                DataTable dataTable22 = await db.Broker_ExecQuery(HttpContext, query);


                                string json = jsonClass.JsonResult_Str1(dataTable, "Text", "");
                                return Content(json, "application/json");



                           }else{

                               string temp1 = $"Delete {appBrokerFactorType}Rows Where {appBrokerFactorType}Ref = {factorcode}";
                               string temp2 = $"Delete {appBrokerFactorType} Where {appBrokerFactorType}Code = {factorcode}";


                               db.Broker_ExecQuery(HttpContext, temp1);
                               db.Broker_ExecQuery(HttpContext, temp2);
                           }
                   }else{
                           string temp1 = $"Delete {appBrokerFactorType}Rows Where {appBrokerFactorType}Ref = {factorcode}";
                           string temp2 = $"Delete {appBrokerFactorType} Where {appBrokerFactorType}Code = {factorcode}";
                           db.Broker_ExecQuery(HttpContext, temp1);
                           db.Broker_ExecQuery(HttpContext, temp2);
                   }




                try
                {
                    string json = jsonClass.JsonResult_Str1(notAmountTable, "Text", "");

                    return Content(json, "application/json");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in {Function}", nameof(BrokerOrder));
                    return StatusCode(500, "Internal server error.");
                }


            }
            else
            {
                   string query = $"select  0 as GoodCode, {factorcode} as PreFactorCode ,{factordate} as PreFactorDate ,{ExistFlag} as ExistFlag ";

                   


                try
                {
                    
                    string json = jsonClass.JsonResult_Str1(await db.Broker_ExecQuery(HttpContext, query), "Text", "");
                    return Content(json, "application/json");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in {Function}", nameof(BrokerOrder));
                    return StatusCode(500, "Internal server error.");
                }



            }


        }










        [HttpGet]
        [Route("UpdateLocation")]
        public async Task<IActionResult>UpdateLocation([FromBody] string GpsLocations)
        {

            var locations = JsonConvert.DeserializeObject<List<GpsLocation>>(GpsLocations);
            string query = "";
            foreach (var lc in locations)
            {
                string longitude = lc.Longitude;
                string latitude = lc.Latitude;
                string brokerRef = lc.BrokerRef;
                string gpsDate = lc.GpsDate.Replace("\\", "");

                DateTime dateObj = DateTime.ParseExact(gpsDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                string formattedDate = dateObj.ToString("yyyy/MM/dd HH:mm:ss");

                query = $"INSERT INTO GpsLocation (Longitude, Latitude, BrokerRef, GpsDate) VALUES ('{longitude}', '{latitude}', {brokerRef}, '{formattedDate}'); SELECT SCOPE_IDENTITY();";
                await db.Broker_ExecQuery(HttpContext, query);

            }


             query = $"select top 1 * from GpsLocation order by 1 desc  ";

            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Locations", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UpdateLocation));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("CustomerInsert")]
        public async Task<IActionResult>CustomerInsert(string BrokerRef,
            string CityCode,
            string KodeMelli,
            string FName,
            string LName,
            string Address,
            string Phone,
            string Mobile,
            string Fax,
            string EMail,
            string PostCode,
            string ZipCode,
            string UserId
            )
        {

            string query = $"EXEC spApp_CustomerIns ";

            if (!string.IsNullOrEmpty(BrokerRef))
            {
                query += $" , @BrokerRef = {BrokerRef} ";
            }

            if (!string.IsNullOrEmpty(CityCode))
            {
                query += $" , @CityCode = {CityCode} ";
            }

            if (!string.IsNullOrEmpty(KodeMelli))
            {
                query += $" , @KodeMelli = '{KodeMelli}' ";
            }

            if (!string.IsNullOrEmpty(FName))
            {
                query += $" , @FName = '{FName}' ";
            }

            if (!string.IsNullOrEmpty(LName))
            {
                query += $" , @LName = '{LName}' ";
            }

            if (!string.IsNullOrEmpty(Address))
            {
                query += $" , @Address = '{Address}' ";
            }

            if (!string.IsNullOrEmpty(Phone))
            {
                query += $" , @Phone = '{Phone}' ";
            }

            if (!string.IsNullOrEmpty(Mobile))
            {
                query += $" , @Mobile = '{Mobile}' ";
            }

            if (!string.IsNullOrEmpty(Fax))
            {
                query += $" , @Fax = '{Fax}' ";
            }

            if (!string.IsNullOrEmpty(EMail))
            {
                query += $" , @EMail = '{EMail}' ";
            }

            if (!string.IsNullOrEmpty(PostCode))
            {
                query += $" , @PostCode = '{PostCode}' ";
            }

            if (!string.IsNullOrEmpty(ZipCode))
            {
                query += $" , @ZipCode = '{ZipCode}' ";
            }

            if (!string.IsNullOrEmpty(UserId))
            {
                query += $" , @UserId = '{UserId}' ";
            }


            try
            {
                DataTable dataTable = await db.Broker_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Customers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerInsert));
                return StatusCode(500, "Internal server error.");
            }

        }


    }



}
