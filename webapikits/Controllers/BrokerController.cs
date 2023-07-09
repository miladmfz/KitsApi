using System.Data;
using System.Globalization;
using Microsoft.AspNetCore.Http;
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
        DataBaseClass db = new DataBaseClass();
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();









        public BrokerController(IConfiguration configuration)
        {
            _configuration = configuration;

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
                DataTable = db.ExecQuery(Query, _configuration);

            }


            Query = "select top 1 * from GpsLocation order by 1 desc  ";
            DataTable dataTable = db.ExecQuery(Query, _configuration);


            return jsonClass.JsonResult_Str(dataTable, "Locations", "");


        }



        [HttpGet]
        [Route("BrokerStack")]
        public string BrokerStack(string BrokerRef)
        {
            if (string.IsNullOrEmpty(BrokerRef))
            {
                BrokerRef = "0";
            }

            string query = "exec spApp_GetBrokerStack"+ BrokerRef;
            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "BrokerStack");

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


            DataTable dataTable = db.ExecQuery(query, _configuration);


            return jsonClass.JsonResult_Str(dataTable, "Customers", "");
          
        }



        [HttpGet]
        [Route("getImageInfo")]
        public string getImageInfo(string code)
        {

            
            string query = "Exec spApp_GetInfo 1, 'KsrImage', "+ code + " , @RowCount=200, @CountFlag=1 ";

            DataTable dataTable = db.ImageExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "");

        }

        [HttpGet]
        [Route("GetMenuBroker")]
        public string GetMenuBroker()
        {


            string query = "select DataValue from DbSetup where KeyValue='AppBroker_MenuGroupCode'";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");


        }


        [HttpGet]
        [Route("MaxRepLogCode")]
        public string MaxRepLogCode(string code)
        {


            string query = "Select top 1 * from RepLogData order by 1 desc";

            DataTable dataTable = db.ExecQuery(query, _configuration);


            return jsonClass.JsonResult_Str(dataTable, "Text", "RepLogDataCode");

        }





        [HttpGet]
        [Route("repinfo")]
        public string repinfo(string table,
            string code,
            string reptype,
            string Reprow,
            string Where
            )
        {


            if (string.IsNullOrEmpty(table))
            {
                table ="" ;
            }

            if (!string.IsNullOrEmpty(code))
            {
                code="0";
            }

            if (!string.IsNullOrEmpty(reptype))
            {
                reptype = "0";
            }

            if (!string.IsNullOrEmpty(Reprow))
            {
                Reprow = "100";
            }

            if (!string.IsNullOrEmpty(Where))
            {
                Where = "''";
            }





            string query = "Exec spApp_GetInfo "+ reptype + ", "+ table + ", "+ code + ", @RowCount="+ Reprow + ", @CountFlag=1 , @WhereCluase= '"+ Where + "'";
            
            
            DataTable dataTable;
            if (table.Equals("KsrImage"))
            {
                dataTable = db.ImageExecQuery(query, _configuration);
            }
            else {
                dataTable = db.ExecQuery(query, _configuration);
            }

            return jsonClass.JsonResult_Str(dataTable, "Text", "");


        }


        [HttpGet]
        [Route("PFQASWED")]
        public string PFQASWED(string PFHDQASW, string PFDTQASW)
        {
            int UserId = -1000;
            int Stk = 1;
            string HJson = PFHDQASW;
            var hobj = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(HJson);
            int factorcode = 0;
            string factordate = "";
            string Explain = "BazaryabApp";
            int Customer = 0;
            int Broker = 0;
            int MobFCode = 0;
            string MobFDate = "";
            int ExistFlag = 0;
            string ClassName = "PreFactor";
            int CountRows = 0;

            var NotAmount = new List<Dictionary<string, object>>();
            int z = 0;

            if (hobj != null)
            {
                if (hobj[0].ContainsKey("PreFactorCode")) { MobFCode = Convert.ToInt32(hobj[0]["PreFactorCode"]); }
                if (hobj[0].ContainsKey("PreFactorDate")) { MobFDate = hobj[0]["PreFactorDate"].ToString(); }
                if (hobj[0].ContainsKey("CustomerRef")) { Customer = Convert.ToInt32(hobj[0]["CustomerRef"]); }
                if (hobj[0].ContainsKey("BrokerRef")) { Broker = Convert.ToInt32(hobj[0]["BrokerRef"]); }
                if (hobj[0].ContainsKey("PreFactorExplain")) { Explain = hobj[0]["PreFactorExplain"].ToString(); }
                if (hobj[0].ContainsKey("rwCount")) { CountRows = Convert.ToInt32(hobj[0]["rwCount"]); }

                string sq = "IF Exists(Select 1 From DbSetup Where KeyValue = 'App_FactorTypeInKowsar' And DataValue='1') Select ClassName = 'Factor' Else Select ClassName = 'PreFactor' ";
                var ClassResult = db.ExecQuery(sq, _configuration);
                if (ClassResult != null)
                {
                    ClassName = ClassResult.Rows[0]["ClassName"].ToString();
                }

                sq = $"Exec [dbo].[spPreFactor_Insert] '{ClassName}', {Stk}, {UserId}, 0, '', {Customer}, '{Explain}', {Broker}, {MobFCode}, '{MobFDate}'";
                

                var result = db.ExecQuery(sq, _configuration);
                if (result != null)
                {
                    factorcode = Convert.ToInt32(result.Rows[0]["PreFactorCode"]);
                    factordate = result.Rows[0]["PreFactorDate"].ToString();
                    ExistFlag = Convert.ToInt32(result.Rows[0]["ExistFlag"]);
                }
            }

            if (ExistFlag == 0)
            {
                string DJson = PFDTQASW;
                var dobj = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(DJson);
                if (dobj != null)
                {
                    foreach (var item in dobj)
                    {
                        z++;
                        int Code = Convert.ToInt32(item["GoodRef"]);
                        int Amount = Convert.ToInt32(item["FactorAmount"]);
                        int Price = Convert.ToInt32(item["Price"]);

                        string sq = $"Exec [dbo].[spPreFactor_InsertRow] '{ClassName}', {factorcode}, {Code}, {Amount}, 0, {UserId}, '', 1, 0, {Price}";

                        var res = db.ExecQuery(sq, _configuration);
                        if (res != null)
                        {
                            if (Convert.ToInt32(res.Rows[0]["RowCode"]) == -1)
                            {
                                NotAmount.Add(new Dictionary<string, object> { { "GoodCode", Code }, { "Flag", 1 } });
                            }
                            else if (Convert.ToInt32(res.Rows[0]["RowCode"]) == -2)
                            {
                                NotAmount.Add(new Dictionary<string, object> { { "GoodCode", Code }, { "Flag", 2 } });
                            }
                        }
                    }
                }

                if (NotAmount.Count < 1)
                {
                    string sq1 = $"Select Sum(FacAmount) rcount From {ClassName}Rows Where {ClassName}Ref = {factorcode}";


                    var res1 = db.ExecQuery(sq1, _configuration);
                    int rcount = Convert.ToInt32(res1.Rows[0]["rcount"]);

                    if (CountRows == rcount)
                    {
                        NotAmount.Add(new Dictionary<string, object>
                {
                    { "GoodCode", "0" },
                    { "PreFactorCode", factorcode },
                    { "PreFactorDate", factordate },
                    { "ExistFlag", ExistFlag }
                });
                    }
                    else
                    {
                        string temp1 = $"Delete {ClassName}Rows Where {ClassName}Ref = {factorcode}";
                        string temp2 = $"Delete {ClassName} Where {ClassName}Code = {factorcode}";


                        db.ExecQuery(temp1, _configuration);
                        db.ExecQuery(temp2, _configuration);
                    }
                }
                else
                {
                    string temp1 = $"Delete {ClassName}Rows Where {ClassName}Ref = {factorcode}";
                    string temp2 = $"Delete {ClassName} Where {ClassName}Code = {factorcode}";
                    db.ExecQuery(temp1, _configuration);
                    db.ExecQuery(temp2, _configuration);
                }
            }
            else
            {
                NotAmount.Add(new Dictionary<string, object>
        {
            { "GoodCode", "0" },
            { "PreFactorCode", factorcode },
            { "PreFactorDate", factordate },
            { "ExistFlag", ExistFlag }
        });
            }

            return JsonConvert.SerializeObject(NotAmount, Formatting.None, new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.Default });
        }











    }


    public class GpsLocation
    {
        public String Longitude { get; set; }
        public String Latitude { get; set; }
        public String BrokerRef { get; set; }
        public String GpsDate { get; set; }
    }

}
