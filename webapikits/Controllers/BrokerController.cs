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


        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        public BrokerController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }




        public class BrokerOrderClass
        {
            public List<HeaderDetail> HeaderDetails { get; set; }
            public List<RowDetail> RowDetails { get; set; }
        }

        public class HeaderDetail
        {
            public string PreFactorCode { get; set; }
            public string PreFactorDate { get; set; }
            public string PreFactorExplain { get; set; }
            public string CustomerRef { get; set; }
            public string BrokerRef { get; set; }
            public string RwCount { get; set; }
        }

        public class RowDetail
        {
            public string GoodRef { get; set; }
            public string FactorAmount { get; set; }
            public string Price { get; set; }
        }

        public class GpsLocation
        {
            public String Longitude { get; set; }
            public String Latitude { get; set; }
            public String BrokerRef { get; set; }
            public String GpsDate { get; set; }
        }

        public class PrintRequest
        {
            public string Image { get; set; }
            public string Code { get; set; }
            public string PrinterName { get; set; }
            public int PrintCount { get; set; }
        }



        [HttpGet]
        [Route("GetColumnList")]
        public string GetColumnList(
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

            string query = "Exec [spApp_GetColumn]  0  ,'', " + Type + "," + AppType + "," + IncludeZero;


            DataTable dataTable = db.Broker_ExecQuery(Request.Path, query);
            return jsonClass.JsonResult_Str(dataTable, "Columns", "");


        }

        [HttpGet]
        [Route("BrokerStack")]
        public string BrokerStack(string BrokerRef)
        {
            if (string.IsNullOrEmpty(BrokerRef))
            {
                BrokerRef = "0";
            }

            string query = "exec spApp_GetBrokerStack " + BrokerRef;
            DataTable dataTable = db.Broker_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "BrokerStack");

        }

        [HttpGet]
        [Route("getImageInfo")]
        public string getImageInfo(string code)
        {


            string query = "Exec spApp_GetInfo 1, 'KsrImage', " + code + " , @RowCount=200, @CountFlag=1 ";

            DataTable dataTable = db.ImageExecQuery(query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "");

        }

        [HttpGet]
        [Route("GetMenuBroker")]
        public string GetMenuBroker()
        {


            string query = "select DataValue from DbSetup where KeyValue='AppBroker_MenuGroupCode'";

            DataTable dataTable = db.Broker_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");


        }

        [HttpGet]
        [Route("GetMaxRepLog")]
        public string GetMaxRepLog()
        {


            string query = "Select top 1 * from RepLogData order by 1 desc";

            DataTable dataTable = db.Broker_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "RepLogDataCode");


        }

        [HttpGet]
        [Route("RetrofitReplicate")]
        public string RetrofitReplicate(
            string code,
            string table,
            string reptype,
            string Reprow
            )
        {

            string query = "Exec spApp_GetInfo " + reptype + ", " + table + ", " + code + ", @RowCount=" + Reprow + ", @CountFlag=1";


            DataTable dataTable;
            if (table.Equals("KsrImage"))
            {
                dataTable = db.ImageExecQuery(query);
            }
            else
            {
                dataTable = db.Broker_ExecQuery(Request.Path, query);
            }

            return jsonClass.JsonResult_Str1(dataTable, "Text", "");


        }



    [HttpPost]
    [Route("BrokerOrder")]
    public string BrokerOrder([FromBody] BrokerOrderClass brokerOrderrequest)
    {

            List<HeaderDetail> headerDetails = brokerOrderrequest.HeaderDetails;
            List<RowDetail> rowDetails = brokerOrderrequest.RowDetails;
            

            string Explain = _configuration.GetConnectionString("BrokerOrder_Explain");
            string Stk = _configuration.GetConnectionString("BrokerOrder_Stack");
            string HasMustAmount = _configuration.GetConnectionString("BrokerOrder_HasMustAmount");

            var NotAmount = new List<Dictionary<string, object>>();

            string MobFDate = "";
            string factordate = "";
            string ClassName = "";

            int UserId = -1000;
            int factorcode = 0;
            int Customer = 0;
            int Broker = 0;
            int MobFCode = 0;
            int ExistFlag = 0;
            int CountRows = 0;
            int z = 0;


            MobFCode = Convert.ToInt32(headerDetails[0].PreFactorCode);
            MobFDate=headerDetails[0].PreFactorDate;
            Customer = Convert.ToInt32(headerDetails[0].CustomerRef);
            Broker = Convert.ToInt32(headerDetails[0].BrokerRef);
            Explain = headerDetails[0].PreFactorExplain;
            CountRows = Convert.ToInt32(headerDetails[0].RwCount);


                string sq = "IF Exists(Select 1 From DbSetup Where KeyValue = 'App_FactorTypeInKowsar' And DataValue='1')" +
                            " Select ClassName = 'Factor' Else Select ClassName = 'PreFactor' ";
                var ClassResult = db.Broker_ExecQuery(Request.Path, sq);
                if (ClassResult != null)
                {
                    ClassName = ClassResult.Rows[0]["ClassName"].ToString();
                }

                 sq = $"Exec [dbo].[spPreFactor_Insert] '{ClassName}', {Stk}, {UserId}, 0, '', {Customer}, '{Explain}', {Broker}, {MobFCode}, '{MobFDate}'";

            var result = db.Broker_ExecQuery(Request.Path, sq);
            if (result != null)
            {
                factorcode = Convert.ToInt32(result.Rows[0]["PreFactorCode"]);
                factordate = result.Rows[0]["PreFactorDate"].ToString();
                ExistFlag = Convert.ToInt32(result.Rows[0]["ExistFlag"]);


            }

            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            DataTable dataTable3 = new DataTable();





         if (ExistFlag == 0)
        {


                // Create a DataTable with two columns: GoodCode and Flag
                DataTable notAmountTable = new DataTable();
                notAmountTable.Columns.Add("GoodCode", typeof(int));
                notAmountTable.Columns.Add("Flag", typeof(int));

                foreach (RowDetail rowDetail_single in rowDetails)
                {
                    z++;
                    int Code = Convert.ToInt32(rowDetail_single.GoodRef);
                    int Amount = Convert.ToInt32(rowDetail_single.FactorAmount);
                    int Price = Convert.ToInt32(rowDetail_single.Price);

                    string sqrow = $"Exec [dbo].[spPreFactor_InsertRow] '{ClassName}', {factorcode}, {Code}, {Amount}, 0, {UserId}, '', {HasMustAmount}, 0, {Price}";
                    var res = db.Broker_ExecQuery(Request.Path, sqrow);
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



            

            if (notAmountTable.Rows.Count < 1)
            {
                string sq1 = $"Select Sum(FacAmount) rcount From {ClassName}Rows Where {ClassName}Ref = {factorcode}";


                var res1 = db.Broker_ExecQuery(Request.Path, sq1);
                int rcount = Convert.ToInt32(res1.Rows[0]["rcount"]);

                if (CountRows == rcount)
                {
                        string query = $"select  0 as GoodCode, {factorcode} as PreFactorCode ,{factordate} as PreFactorDate ,{ExistFlag} as ExistFlag ";
                        return jsonClass.JsonResult_Str1(db.Broker_ExecQuery(Request.Path, query), "Text", "");

                    }
                    else
                {
                    string temp1 = $"Delete {ClassName}Rows Where {ClassName}Ref = {factorcode}";
                    string temp2 = $"Delete {ClassName} Where {ClassName}Code = {factorcode}";


                    db.Broker_ExecQuery(Request.Path, temp1);
                    db.Broker_ExecQuery(Request.Path, temp2);
                }
            }
            else
            {
                string temp1 = $"Delete {ClassName}Rows Where {ClassName}Ref = {factorcode}";
                string temp2 = $"Delete {ClassName} Where {ClassName}Code = {factorcode}";
                db.Broker_ExecQuery(Request.Path, temp1);
                db.Broker_ExecQuery(Request.Path, temp2);
            }
                return jsonClass.JsonResult_Str1(notAmountTable, "Text", "");


        }
        else
        {
                string query = $"select  0 as GoodCode, {factorcode} as PreFactorCode ,{factordate} as PreFactorDate ,{ExistFlag} as ExistFlag ";
                return jsonClass.JsonResult_Str1(db.Broker_ExecQuery(Request.Path, query), "Text", "");



        }


        }










        [HttpGet]
        [Route("UpdateLocation")]
        public string UpdateLocation([FromBody] string GpsLocations)
        {


            var locations = JsonConvert.DeserializeObject<List<GpsLocation>>(GpsLocations);

            foreach (var lc in locations)
            {
                string longitude = lc.Longitude;
                string latitude = lc.Latitude;
                string brokerRef = lc.BrokerRef;
                string gpsDate = lc.GpsDate.Replace("\\", "");

                DateTime dateObj = DateTime.ParseExact(gpsDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                string formattedDate = dateObj.ToString("yyyy/MM/dd HH:mm:ss");

                Query = "INSERT INTO GpsLocation (Longitude, Latitude, BrokerRef, GpsDate) VALUES ('" + longitude + "', '" + latitude + "', " + brokerRef + ", '" + formattedDate + "'); SELECT SCOPE_IDENTITY();";
                DataTable = db.Broker_ExecQuery(Request.Path, Query);

            }


            Query = "select top 1 * from GpsLocation order by 1 desc  ";
            DataTable dataTable = db.Broker_ExecQuery(Request.Path, Query);


            return jsonClass.JsonResult_Str(dataTable, "Locations", "");


        }



        [HttpGet]
        [Route("CustomerInsert")]
        public string CustomerInsert(string BrokerRef,
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

            string query = "EXEC spApp_CustomerIns ";

            if (!string.IsNullOrEmpty(BrokerRef))
            {
                query += "@BrokerRef = " + BrokerRef + " ";
            }

            if (!string.IsNullOrEmpty(CityCode))
            {
                query += "@CityCode = " + CityCode + " ";
            }

            if (!string.IsNullOrEmpty(KodeMelli))
            {
                query += "@KodeMelli = '" + KodeMelli + "' ";
            }

            if (!string.IsNullOrEmpty(FName))
            {
                query += "@FName = '" + FName + "' ";
            }

            if (!string.IsNullOrEmpty(LName))
            {
                query += "@LName = '" + LName + "' ";
            }

            if (!string.IsNullOrEmpty(Address))
            {
                query += "@Address = '" + Address + "' ";
            }

            if (!string.IsNullOrEmpty(Phone))
            {
                query += "@Phone = '" + Phone + "' ";
            }

            if (!string.IsNullOrEmpty(Mobile))
            {
                query += "@Mobile = '" + Mobile + "' ";
            }

            if (!string.IsNullOrEmpty(Fax))
            {
                query += "@Fax = '" + Fax + "' ";
            }

            if (!string.IsNullOrEmpty(EMail))
            {
                query += "@EMail = '" + EMail + "' ";
            }

            if (!string.IsNullOrEmpty(PostCode))
            {
                query += "@PostCode = '" + PostCode + "' ";
            }

            if (!string.IsNullOrEmpty(ZipCode))
            {
                query += "@ZipCode = '" + ZipCode + "' ";
            }

            if (!string.IsNullOrEmpty(UserId))
            {
                query += "@UserId = '" + UserId + "' ";
            }


            DataTable dataTable = db.Broker_ExecQuery(Request.Path, query);


            return jsonClass.JsonResult_Str(dataTable, "Customers", "");

        }







    }







}
