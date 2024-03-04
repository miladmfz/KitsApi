using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapikits.Model;
using System.Data.SqlClient;
using static webapikits.Controllers.OrderController;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcrController : ControllerBase
    {


        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        public OcrController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }



        public class OcrModel
        {
            public string barcode { get; set; } = "";
            public string Step { get; set; } = "0";
            public string orderby { get; set; } = "Goodname";



            public string State { get; set; } = "";
            public string SearchTarget { get; set; } = "";
            public string Stack { get; set; } = "";
            public string path { get; set; } = "";
            public string Row { get; set; } = "";
            public string PageNo { get; set; } = "";
            public string HasShortage { get; set; } = "";
            public string IsEdited { get; set; } = "";


            public string OcrFactorCode { get; set; } = "";
            public string Reader { get; set; } = "";
            public string Controler { get; set; } = "";
            public string Packer { get; set; } = "";
            public string PackDeliverDate { get; set; } = "";
            public string PackCount { get; set; } = "";
            public string AppDeliverDate { get; set; } = "";



        }


        



        [HttpGet]
        [Route("GetOcrFactor")]
        public string GetOcrFactor([FromBody] OcrModel ocrModel)
        {


            string query = $"Exec dbo.spApp_ocrGetFactor '{ocrModel.barcode}',1,{ocrModel.Step},'{ocrModel.orderby}'";


            DataTable dataTable = db.ExecQuery(Request.Path, query);




            response.StatusCode = "200";
            response.Errormessage = "";
            jsonDict.Add("response", JsonConvert.SerializeObject(response));
            jsonDict.Add("Factor", jsonClass.ConvertDataTableToJson(dataTable));

            jsonDict.Add("Goods", jsonClass.ConvertDataTableToJson(dataTable));
            return JsonConvert.SerializeObject(jsonDict);


        }

        [HttpGet]
        [Route("OcrDeliverd")]
        public string OcrDeliverd(
            string AppOCRCode,
            string State,
            string Deliverer
            )
        {

            string query = "Exec dbo.spApp_ocrSetDelivery " + AppOCRCode + ", " + State + ",'" + Deliverer + "'";


            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }

        [HttpGet]
        [Route("OcrControlled")]
        public string OcrControlled(
    string AppOCRCode,
    string State,
    string JobPersonRef
    )
        {

            string query = "Exec dbo.spApp_ocrSetControlled " + AppOCRCode + " ," + State + " ," + JobPersonRef;



            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }

        [HttpGet]
        [Route("SetGoodShortage")]
        public string SetGoodShortage(
    string OCRFactorRowCode,
    string Shortage
    )
        {

            string query = "Exec dbo.spApp_ocrSetShortage " + OCRFactorRowCode + ", " + Shortage;



            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }

        [HttpPost]
        [Route("GetOcrFactorList")]
        public string GetOcrFactorList([FromBody] OcrModel ocrModel)
        {


            if (ocrModel.Row.Length < 1)
            {
                ocrModel.Row = "10";
            }
            string order = "";
            string where = "";

            if (ocrModel.State == "0")
            {
                if (ocrModel.Stack == "همه")
                {
                    where = "";
                }
                else
                {
                    where = $" And Exists(Select 1 From FactorRows r Join Good g on GoodRef = GoodCode Join AppOCRFactorRow cr on cr.AppFactorRowRef = r.FactorRowCode And cr.AppOCRFactorRef = o.AppOCRFactorCode Where r.FactorRef = FactorCode And IsNull({_configuration.GetConnectionString("Ocr_StackCategory")}, '''') = ''{ocrModel.Stack}'' And IsNull(cr.AppRowIsControled, 0) = 0) ";
                }
            }
            if (ocrModel.State == "4")
            {
                order += ", ' order by o.AppTcPrintRef desc' ";
            }
            else
            {
                order += ", ' order by o.AppTcPrintRef' ";
            }

            if (ocrModel.path == "همه")
            {
                where = where + " ";
            }
            else


            {
                where = where + $" And IsNull({_configuration.GetConnectionString("Ocr_Ersall")}, '''') = N''{ocrModel.path}'' ";
            }

            if (ocrModel.HasShortage == "1")
            {
                where = where + " And o.HasShortage = 1 ";
            }
            if (ocrModel.IsEdited == "1")
            {
                where = where + " And o.IsEdited = 1 ";
            }

            string sq = $"Exec dbo.spApp_ocrFactorList {ocrModel.State}, '{ocrModel.SearchTarget}', '{where}', {ocrModel.Row}, {ocrModel.PageNo} {order}";


            DataTable dataTable = db.ExecQuery(Request.Path, sq);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");
        }



        [HttpPost]
        [Route("SetPackDetail")]
        public string SetPackDetail([FromBody] OcrModel ocrModel)
        {


            string query = $"Exec dbo.spApp_ocrSetPackDetail {ocrModel.OcrFactorCode},'{ocrModel.Reader}','{ocrModel.Controler}','{ocrModel.Packer} - {ocrModel.AppDeliverDate}','{ocrModel.PackDeliverDate}',{ocrModel.PackCount}";


            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetOcrGoodDetail")]
        public string GetOcrGoodDetail(string GoodCode)
        {
            string Stackref = _configuration.GetConnectionString("Ocr_StackRef");

            string query = $"select cast(s.Amount as Int) TotalAvailable ,size,CoverType,cast(PageNo as Int) PageNo from vwGood with(nolock) Join GoodStack s with(nolock) on GoodCode = GoodRef where StackRef = {Stackref} And Goodcode={GoodCode}";


            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }




        [HttpGet]
        [Route("ExitDelivery")]
        public string ExitDelivery(string Where)
        {

            string query = " update AppOCRFactor set HasSignature=0,AppIsDelivered=0 where AppOCRFactorCode= " + Where;

            DataTable dataTable = db.ExecQuery(Request.Path, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }



        [HttpGet]
        [Route("GetJob")]
        public string GetJob(string Where)
        {

            string query = "select JobCode,Title,Explain from job where Explain='" + Where + "'";



            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Jobs", "");

        }




        [HttpGet]
        [Route("GetJobPerson")]
        public string GetJobPerson(string Where)
        {

            string query = "select j.JobCode,jp.JobPersonCode,j.Title,c.Name,c.FName from  JobPerson jp  join job j on j.JobCode=jp.JobRef  join Central c on c.CentralCode=jp.CentralRef  where j.Title='" + Where + "'";



            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "JobPersons", "");

        }






        [HttpGet]
        [Route("GetOcrFactorDetail")]
        public string GetOcrFactorDetail(string OCRFactorCode)
        {

            string query = "[dbo].[spApp_ocrGetFactorDetail] " + OCRFactorCode;



            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "AppOcrFactors", "");

        }



        [HttpGet]
        [Route("GetCustomerPath")]
        public string GetCustomerPath()
        {

            string query = "Select Distinct IsNull(" + _configuration.GetConnectionString("Ocr_CustomerPath") + " , '') " + _configuration.GetConnectionString("Ocr_CustomerPath_Lible") + " From PropertyValue Where ClassName= 'TCustomer'";


            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }


        [HttpGet]
        [Route("GetStackCategory")]
        public string GetStackCategory()
        {

            string query = "Select Distinct IsNull(" + _configuration.GetConnectionString("Ocr_StackCategory") + " , '') " + _configuration.GetConnectionString("Ocr_StackCategory") + " From good";


            DataTable dataTable = db.ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }
































































































































        /*
        
        
        
        [HttpGet]
        [Route("GetFactorListCount")]
        public string GetFactorListCount(
            string State,
            string SearchTarget,
            string Stack,
            string path,
            string Row,
            string PageNo,
            string HasShortage,
            string IsEdited
            )
        {


            string where = "";

            if (State == "0")
            {
                if (Stack == "همه")
                {
                    where = "";
                }
                else
                {
                    where = " And Exists(Select 1 From FactorRows r Join Good g on GoodRef = GoodCode Join AppOCRFactorRow cr on cr.AppFactorRowRef = r.FactorRowCode And cr.AppOCRFactorRef = o.AppOCRFactorCode Where r.FactorRef = FactorCode And IsNull(" + _configuration.GetConnectionString("Ocr_StackCategory") + ", '''') = ''" + Stack + "'' And IsNull(cr.AppRowIsControled, 0) = 0) ";
                }
            }

            if (path == "همه")
            {
                where = where + " ";
            }
            else
            {
                where = where + " And IsNull(" + _configuration.GetConnectionString("Ocr_Ersall") + ", '''') = N''" + path + "'' ";
            }

            if (HasShortage == "1")
            {
                where = where + " And o.HasShortage = 1 ";
            }
            if (IsEdited == "1")
            {
                where = where + " And o.IsEdited = 1 ";
            }

            string sq = $"Exec dbo.spApp_ocrFactorListTotal {State}, '{SearchTarget}', '{where}', {Row}, {PageNo}";


            DataTable dataTable = db.ExecQuery(Request.Path, sq);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");



        }




        
        public void SaveOcrImage(string barcode, string zipPath)
        {
            string query = $"Exec dbo.spApp_ocrGetFactor '{barcode}', 0 ";

            DataTable dataTable = ExecQuery(query);

            string dbname = Convert.ToString(dataTable.Rows[0]["dbname"]);
            string FactorRef = Convert.ToString(dataTable.Rows[0]["FactorRef"]);
            string TcPrintRef = Convert.ToString(dataTable.Rows[0]["TcPrintRef"]);

            byte[] fileContent = fileManager.GetFile(zipPath);

            string query1 = $"Insert Into {dbname}.dbo.AttachedFiles(Title, ClassName, ObjectRef, FileName, SourceFile, Type, Owner, CreationDate, Reformer, ReformDate, TcPrintRef) " +
                $"Select 'App_ocr', 'Factor', {FactorRef}, '{barcode}.jpg', @FContent, 'zip', -1000, GetDate(), -1000, GetDate(), {TcPrintRef} ";

            ExecNonQueryWithBinaryParam(query1, fileContent);

            string query3 = $"set nocount on Update AppOCRFactor Set HasSignature = 1 Where AppTcPrintRef = {TcPrintRef} ";

            ExecNonQuery(query3);

            Console.WriteLine("\"done\"");
        }

        private DataTable ExecQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        private void ExecNonQueryWithBinaryParam(string query, byte[] binaryData)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@FContent", SqlDbType.VarBinary, -1).Value = binaryData;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ExecNonQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }


        */







    }




}

