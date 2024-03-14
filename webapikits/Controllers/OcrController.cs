using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapikits.Model;
using System.Data.SqlClient;
using static webapikits.Controllers.OrderController;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Identity;
using FastReport;

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
        FileManager fileManager = new();

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

            public string CountFlag { get; set; } = "";
            public string DbName { get; set; } = "";


            public string OcrFactorCode { get; set; } = "";
            public string Reader { get; set; } = "";
            public string Controler { get; set; } = "";
            public string Packer { get; set; } = "";
            public string PackDeliverDate { get; set; } = "";
            public string PackCount { get; set; } = "";
            public string AppDeliverDate { get; set; } = "";



        }




    [HttpPost]
        [Route("GetOcrFactor")]
        public string GetOcrFactor([FromBody] OcrModel ocrModel)
        {


            string query = $"Exec dbo.spApp_ocrGetFactor '{ocrModel.barcode}',1,{ocrModel.Step},'{ocrModel.orderby}'";


            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, query);




            response.StatusCode = "200";
            response.Errormessage = "";

            


            List<Dictionary<string, object>> factor_rows = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> goods_rows = new List<Dictionary<string, object>>();


            Dictionary<string, object> f_currentRow = new Dictionary<string, object>();

            Dictionary<string, object> result = new Dictionary<string, object>();






            if (dataTable.Rows.Count > 0)
            {

                f_currentRow.Add("FactorCode", Convert.ToString(dataTable.Rows[0]["FactorCode"]));
                f_currentRow.Add("FactorPrivateCode", Convert.ToString(dataTable.Rows[0]["FactorPrivateCode"]));
                f_currentRow.Add("FactorDate", Convert.ToString(dataTable.Rows[0]["FactorDate"]));
                f_currentRow.Add("SumAmount", Convert.ToString(dataTable.Rows[0]["SumAmount"]));
                f_currentRow.Add("SumPrice", Convert.ToString(dataTable.Rows[0]["SumPrice"]));
                f_currentRow.Add("NewSumPrice", Convert.ToString(dataTable.Rows[0]["NewSumPrice"]));
                f_currentRow.Add("CustName", Convert.ToString(dataTable.Rows[0]["CustName"]));
                f_currentRow.Add("CustomerRef", Convert.ToString(dataTable.Rows[0]["CustomerRef"]));
                f_currentRow.Add("Address", Convert.ToString(dataTable.Rows[0]["Address"]));
                f_currentRow.Add("Phone", Convert.ToString(dataTable.Rows[0]["Phone"]));
                f_currentRow.Add("ErrCode", Convert.ToString(dataTable.Rows[0]["ErrCode"]));
                f_currentRow.Add("ErrMessage", Convert.ToString(dataTable.Rows[0]["ErrMessage"]));
                f_currentRow.Add("AppIsControled", Convert.ToString(dataTable.Rows[0]["AppIsControled"]));
                f_currentRow.Add("AppIsPacked", Convert.ToString(dataTable.Rows[0]["AppIsPacked"]));
                f_currentRow.Add("AppOCRFactorCode", Convert.ToString(dataTable.Rows[0]["AppOCRFactorCode"]));
                // Add more columns as needed
                factor_rows.Add(f_currentRow);

                foreach (DataRow row in dataTable.Rows)
                {
                    Dictionary<string, object> goods_currentRow = new Dictionary<string, object>();

                    // Add specific columns to the JSON
                    goods_currentRow.Add("GoodCode", Convert.ToString(row["GoodRef"]));
                    goods_currentRow.Add("GoodMaxSellPrice", Convert.ToString(row["GoodMaxSellPrice"]));
                    goods_currentRow.Add("FactorRowCode", Convert.ToString(row["FactorRowCode"]));
                    goods_currentRow.Add("GoodName", Convert.ToString(row["GoodName"]));
                    goods_currentRow.Add("Price", Convert.ToString(row["Price"]));
                    goods_currentRow.Add("FacAmount", Convert.ToString(row["FacAmount"]));
                    goods_currentRow.Add("GoodExplain4", Convert.ToString(row["GoodExplain4"]));
                    goods_currentRow.Add("AppRowIsControled", Convert.ToString(row["AppRowIsControled"]));
                    goods_currentRow.Add("AppRowIsPacked", Convert.ToString(row["AppRowIsPacked"]));
                    goods_currentRow.Add("AppOCRFactorRowCode", Convert.ToString(row["AppOCRFactorRowCode"]));
                    goods_currentRow.Add("ShortageAmount", Convert.ToString(row["ShortageAmount"]));
                    goods_currentRow.Add("CachedBarCode", Convert.ToString(row["CachedBarCode"]));
                    // Add more columns as needed

                    goods_rows.Add(goods_currentRow);
                }

            }
            else {
                f_currentRow.Add("FactorCode", "0");
                f_currentRow.Add("FactorPrivateCode", "0");
                f_currentRow.Add("FactorDate", "");
                f_currentRow.Add("SumAmount", "0");
                f_currentRow.Add("SumPrice", "0");
                f_currentRow.Add("NewSumPrice", "0");
                f_currentRow.Add("CustName", "");
                f_currentRow.Add("CustomerRef", "");
                f_currentRow.Add("Address", "");
                f_currentRow.Add("Phone", "");
                f_currentRow.Add("ErrCode", "");
                f_currentRow.Add("ErrMessage", "");
                f_currentRow.Add("AppIsControled", "");
                f_currentRow.Add("AppIsPacked", "");
                f_currentRow.Add("AppOCRFactorCode", "");
                // Add more columns as needed
                factor_rows.Add(f_currentRow);

                Dictionary<string, object> goods_currentRow = new Dictionary<string, object>();


                // Add specific columns to the JSON
                goods_currentRow.Add("GoodCode","0");
                goods_currentRow.Add("GoodMaxSellPrice","0");
                goods_currentRow.Add("FactorRowCode","0");
                goods_currentRow.Add("GoodName","");
                goods_currentRow.Add("Price","0");
                goods_currentRow.Add("FacAmount","0");
                goods_currentRow.Add("GoodExplain4","");
                goods_currentRow.Add("AppRowIsControled","");
                goods_currentRow.Add("AppRowIsPacked","");
                goods_currentRow.Add("AppOCRFactorRowCode","");
                goods_currentRow.Add("ShortageAmount","");
                goods_currentRow.Add("CachedBarCode","");
                // Add more columns as needed

                goods_rows.Add(goods_currentRow);
                


            }







            result.Add("Factors", factor_rows);
            result.Add("OcrGoods", goods_rows);

            //jsonDict.Add("Factor", jsonClass.ConvertDataTableToJson(jsonClass.ConvertDictionaryListToDataTable(factor_rows)));
            ///jsonDict.Add("Goods", jsonClass.ConvertDataTableToJson(jsonClass.ConvertDictionaryListToDataTable(goods_rows)));
            return JsonConvert.SerializeObject(result);

            //return jsonClass.JsonResult_Str(dataTable, "OcrGoods", "");



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


            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, query);

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



            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, query);

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



            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, query);

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
            string countflag = "";
            string dbname = "";

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


            if (ocrModel.CountFlag == "1")
            {
                countflag = countflag + ", @CountFlag=1 ";
            }
            else if (ocrModel.CountFlag == "0")
            {
                countflag = countflag + ", @CountFlag=0 ";
            }

            if (ocrModel.DbName == "")
            {
                dbname = dbname + $" , @Db='{ocrModel.DbName}'";
            }
            if (ocrModel.IsEdited == "1")
            {
                dbname = dbname + " ,  @Db='' ";
            }

            string sq = $"Exec dbo.spApp_ocrFactorList {ocrModel.State}, '{ocrModel.SearchTarget}', '{where}', {ocrModel.Row}, {ocrModel.PageNo} {order} {countflag} {dbname}";


            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, sq);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");
        }



        [HttpPost]
        [Route("SetPackDetail")]
        public string SetPackDetail([FromBody] OcrModel ocrModel)
        {


            string query = $"Exec dbo.spApp_ocrSetPackDetail {ocrModel.OcrFactorCode},'{ocrModel.Reader}','{ocrModel.Controler}','{ocrModel.Packer} - {ocrModel.AppDeliverDate}','{ocrModel.PackDeliverDate}',{ocrModel.PackCount}";


            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetOcrGoodDetail")]
        public string GetOcrGoodDetail(string GoodCode)
        {
            string Stackref = _configuration.GetConnectionString("Ocr_StackRef");

            string query = $"select cast(s.Amount as Int) TotalAvailable ,size,CoverType,cast(PageNo as Int) PageNo from vwGood with(nolock) Join GoodStack s with(nolock) on GoodCode = GoodRef where StackRef = {Stackref} And Goodcode={GoodCode}";


            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }




        [HttpGet]
        [Route("ExitDelivery")]
        public string ExitDelivery(string Where)
        {

            string query = " update AppOCRFactor set HasSignature=0,AppIsDelivered=0 where AppOCRFactorCode= " + Where;

            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }



        [HttpGet]
        [Route("GetJob")]
        public string GetJob(string Where)
        {

            string query = "select JobCode,Title,Explain from job where Explain='" + Where + "'";



            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Jobs", "");

        }




        [HttpGet]
        [Route("GetJobPerson")]
        public string GetJobPerson(string Where)
        {

            string query = "select j.JobCode,jp.JobPersonCode,j.Title,c.Name,c.FName from  JobPerson jp  join job j on j.JobCode=jp.JobRef  join Central c on c.CentralCode=jp.CentralRef  where j.Title='" + Where + "'";



            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "JobPersons", "");

        }






        [HttpGet]
        [Route("GetOcrFactorDetail")]
        public string GetOcrFactorDetail(string OCRFactorCode)
        {

            string query = "[dbo].[spApp_ocrGetFactorDetail] " + OCRFactorCode;



            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "AppOcrFactors", "");

        }



        [HttpGet]
        [Route("GetCustomerPath")]
        public string GetCustomerPath()
        {

            string query = "Select Distinct IsNull(" + _configuration.GetConnectionString("Ocr_CustomerPath") + " , '') " + _configuration.GetConnectionString("Ocr_CustomerPath_Lible") + " From PropertyValue Where ClassName= 'TCustomer'";


            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }


        [HttpGet]
        [Route("GetStackCategory")]
        public string GetStackCategory()
        {

            string query = "Select Distinct IsNull(" + _configuration.GetConnectionString("Ocr_StackCategory") + " , '') " + _configuration.GetConnectionString("Ocr_StackCategory") + " From good";


            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }





        [HttpGet]
        [Route("OrderPrintFactor")]
        public string OrderPrintFactor(String AppBasketInfoRef)
        {




            string query = " select * from AppPrinter where Apptype = 2 ";

            DataTable Table_print = db.Order_ExecQuery(Request.Path, query);

            List<Printer> Printers = new List<Printer>();


            if (Table_print.Rows.Count > 0)
            {
                for (int i = 0; i < Table_print.Rows.Count; i++)
                {
                    Printer printer = new Printer();

                    printer.AppPrinterCode = Convert.ToString(Table_print.Rows[i]["AppPrinterCode"]);
                    printer.PrinterName = Convert.ToString(Table_print.Rows[i]["PrinterName"]);
                    printer.PrinterExplain = Convert.ToString(Table_print.Rows[i]["PrinterExplain"]);
                    printer.GoodGroups = Convert.ToString(Table_print.Rows[i]["GoodGroups"]);
                    printer.WhereClause = Convert.ToString(Table_print.Rows[i]["WhereClause"]);
                    printer.PrintCount = Convert.ToString(Table_print.Rows[i]["PrintCount"]);
                    printer.PrinterActive = Convert.ToString(Table_print.Rows[i]["PrinterActive"]);
                    printer.FilePath = Convert.ToString(Table_print.Rows[i]["FilePath"]);
                    printer.AppType = Convert.ToString(Table_print.Rows[i]["AppType"]);


                    Printers.Add(printer);

                }
            }



            foreach (Printer printer in Printers)
            {

                if (Convert.ToInt64(printer.PrintCount) > 0)
                {


                    Report report = new Report();
                    report.Load(printer.FilePath);


                    query = $"Exec [dbo].[spApp_OrderGetFactor ] {AppBasketInfoRef} ";
                    DataTable dataTable_factor = db.Order_ExecQuery(Request.Path, query);

                    List<Factor> factorHeader = new List<Factor>();
                    Factor factor = new Factor();


                    factor.AppBasketInfoCode = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["AppBasketInfoCode"]));
                    factor.AppBasketInfoDate = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["AppBasketInfoDate"]));
                    factor.DailyCode = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["DailyCode"]));
                    factor.MizType = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["MizType"]));
                    factor.RstMizName = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["RstMizName"]));
                    factor.InfoExplain = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["InfoExplain"]));
                    factor.FactorExplain = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["FactorExplain"]));
                    factor.TimeStart = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["TimeStart"]));
                    factor.InfoPrintCount = Convert.ToString(dataTable_factor.Rows[0]["InfoPrintCount"]);
                    factor.InfoState = Convert.ToString(dataTable_factor.Rows[0]["InfoState"]);
                    factor.ReserveStart = Convert.ToString(dataTable_factor.Rows[0]["ReserveStart"]);
                    if (Convert.ToInt64(dataTable_factor.Rows[0]["InfoPrintCount"]) > 0)
                    {
                        factor.CustName = "(چاپ مجدد)";
                    }
                    factorHeader.Add(factor);



                    string convertedString = printer.WhereClause.Replace("=''", "=N''");

                    query = $"Exec [dbo].[spApp_OrderGetFactorRow ] {AppBasketInfoRef} , {printer.GoodGroups} , N'{convertedString}' ";
                    DataTable dataTable_Row = db.Order_ExecQuery(Request.Path, query);




                    List<FactorRow> FactorRows = new List<FactorRow>();


                    if (dataTable_Row.Rows.Count > 0)
                    {
                        for (int i = 0; i < dataTable_Row.Rows.Count; i++)
                        {
                            FactorRow factorRow = new FactorRow();
                            Console.WriteLine((dataTable_Row.Rows[i]["IsExtra"]));

                            if (Convert.ToString(dataTable_Row.Rows[i]["IsExtra"]) == "True")
                            {
                                factorRow.GoodName = Convert.ToString(dataTable_Row.Rows[i]["GoodName"]) + " (سفارش مجدد) .";
                                factorRow.GoodName = db.ConvertToPersianNumber(factorRow.GoodName);

                            }
                            else
                            {
                                factorRow.GoodName = Convert.ToString(dataTable_Row.Rows[i]["GoodName"]);
                                factorRow.GoodName = db.ConvertToPersianNumber(factorRow.GoodName);

                            }

                            factorRow.FactorRowCode = Convert.ToString(dataTable_Row.Rows[i]["FactorRowCode"]);
                            factorRow.GoodRef = Convert.ToString(dataTable_Row.Rows[i]["GoodRef"]);
                            factorRow.FacAmount = Convert.ToString(dataTable_Row.Rows[i]["FacAmount"]);
                            factorRow.FacAmount = factorRow.FacAmount.Substring(0, factorRow.FacAmount.IndexOf("."));
                            factorRow.CanPrint = Convert.ToString(dataTable_Row.Rows[i]["CanPrint"]);
                            factorRow.RowExplain = db.ConvertToPersianNumber(Convert.ToString(dataTable_Row.Rows[i]["RowExplain"]));
                            factorRow.IsExtra = Convert.ToString(dataTable_Row.Rows[i]["IsExtra"]);

                            FactorRows.Add(factorRow);


                        }

                        List<Printer> printerss = new List<Printer>();
                        printerss.Add(printer);
                        string time = db.ConvertToPersianNumber(DateTime.Now.ToString("HH:mm"));

                        report.RegisterData(factorHeader, "Factor");
                        report.RegisterData(FactorRows, "FactorRow");
                        report.RegisterData(printerss, "Printer");
                        report.SetParameterValue("CurrentTime", time);



                        if (report.Prepare())
                        {
                            // Export the report to PDF
                            PDFSimpleExport pdfExport = new PDFSimpleExport();
                            pdfExport.ShowProgress = false;
                            pdfExport.Subject = "Subject Report";
                            pdfExport.Title = "Report Title";
                            MemoryStream ms = new MemoryStream();

                            report.Export(pdfExport, ms);
                            report.Dispose();
                            pdfExport.Dispose();
                            ms.Position = 0;


                            fileManager.SavePdfToStorage(ms, _configuration.GetConnectionString("Pdf_SaveStorage"));

                            PdfPrinter pdfPrinter = new PdfPrinter();

                            pdfPrinter.PrintPdf(_configuration.GetConnectionString("Pdf_SaveStorage"), printer.PrinterName);





                        }

                    }

                }

            }

            string query11 = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query11);

            return jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");


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


            DataTable dataTable = db.Ocr_ExecQuery(Request.Path, sq);

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

