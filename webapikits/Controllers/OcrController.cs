using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapikits.Model;
using System.Data.SqlClient;
using FastReport.Export.PdfSimple;
using FastReport;
using System.IO.Compression;



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
            public string FactorCode { get; set; } = "";
            public string StackCategory { get; set; } = "";
            public string Sender { get; set; } = "";
            public string ImageStr { get; set; } = "";



        }
        


        [HttpPost]
        [Route("OcrPrintControler")]
        public string OcrPrintControler([FromBody] OcrModel ocrModel)
        {




            string query = " select * from AppPrinter where Apptype = 2 ";
            string where = ocrModel.StackCategory;
            string sender = ocrModel.Sender;

            DataTable Table_print = db.Ocr_ExecQuery(HttpContext, query);

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
                if (printer.WhereClause.Equals(where)) { 
                


                    Report report = new Report();
                    report.Load(printer.FilePath);


                    query = $"select CustName,FactorCode,FactorPrivateCode,AppPackCount,AppDeliverer from vwfactor where factorcode = {ocrModel.FactorCode} ";
                    DataTable dataTable_factor = db.Ocr_ExecQuery(HttpContext, query);

                    List<Factor> factorHeader = new List<Factor>();
                    Factor factor = new Factor();


                    factor.CustName = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["CustName"]));
                    factor.FactorCode = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["FactorCode"]));
                    factor.FactorPrivateCode = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["FactorPrivateCode"]));
                    factor.AppPackCount = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["AppPackCount"]));
                    factor.AppDeliverer = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["AppDeliverer"]));

                    factorHeader.Add(factor);


                    List<Printer> printerss = new List<Printer>();
                    printerss.Add(printer);
                    string time = db.ConvertToPersianNumber(DateTime.Now.ToString("HH:mm"));


                    report.RegisterData(factorHeader, "Factor");
                    report.RegisterData(printerss, "Printer");
                    report.SetParameterValue("CurrentTime", time);
                    report.SetParameterValue("CurrentSender", sender);
 


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

            string query11 = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query11);

            return jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");


        }



        [HttpPost]
        [Route("OcrPrintPacker")]
        public string OcrPrintPacker([FromBody] OcrModel ocrModel)
        {




            string query = " select * from AppPrinter where Apptype = 2 ";
            string where = ocrModel.StackCategory;
            string sender = ocrModel.Sender;

            DataTable Table_print = db.Ocr_ExecQuery(HttpContext, query);

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
                

                if (Convert.ToInt64(printer.PrintCount) > 0 && printer.WhereClause.Equals("Pack"))
                {




                    query = $"select CustName,FactorCode,FactorPrivateCode,AppPackCount,AppDeliverer from vwfactor where factorcode = {ocrModel.FactorCode} ";
                    DataTable dataTable_factor = db.Ocr_ExecQuery(HttpContext, query);

                    List<Factor> factorHeader = new List<Factor>();
                    Factor factor = new Factor();


                    factor.CustName = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["CustName"]));
                    factor.FactorCode = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["FactorCode"]));
                    factor.FactorPrivateCode = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["FactorPrivateCode"]));
                    factor.AppPackCount = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["AppPackCount"]));
                    factor.AppDeliverer = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["AppDeliverer"]));
                    factorHeader.Add(factor);

                    int counter = Convert.ToInt32(dataTable_factor.Rows[0]["AppPackCount"]);

                   

                    for (int i = 0; i < counter; i++)
                    {
                        Console.WriteLine("" + (i + 1));
                        List<Printer> printerss = new List<Printer>();
                        printerss.Add(printer);
                        string time = db.ConvertToPersianNumber(DateTime.Now.ToString("HH:mm"));

                        string pack = db.ConvertToPersianNumber(Convert.ToString((i + 1)));

                        Report report = new Report();
                        report.Load(printer.FilePath);

                        report.RegisterData(factorHeader, "Factor");
                        report.RegisterData(printerss, "Printer");
                        report.SetParameterValue("CurrentTime", time);
                        report.SetParameterValue("CurrentPack", pack);
                        report.SetParameterValue("CurrentSender", sender);



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
                        report.Clear();
                    }





                }

            }

            string query11 = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query11);

            return jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");


        }




        [HttpPost]
        [Route("GetOcrFactor")]
        public string GetOcrFactor([FromBody] OcrModel ocrModel)
        {


            string query = $"Exec dbo.spApp_ocrGetFactor '{ocrModel.barcode}',1,{ocrModel.Step},'{ocrModel.orderby}'";


            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);




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

            return JsonConvert.SerializeObject(result);


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


            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

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



            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

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



            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

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


            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, sq);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");
        }



        [HttpPost]
        [Route("SetPackDetail")]
        public string SetPackDetail([FromBody] OcrModel ocrModel)
        {


            string query = $"Exec dbo.spApp_ocrSetPackDetail {ocrModel.OcrFactorCode},'{ocrModel.Reader}','{ocrModel.Controler}','{ocrModel.Packer} - {ocrModel.AppDeliverDate}','{ocrModel.PackDeliverDate}',{ocrModel.PackCount}";


            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetOcrGoodDetail")]
        public string GetOcrGoodDetail(string GoodCode)
        {
            string Stackref = _configuration.GetConnectionString("Ocr_StackRef");

            string query = $"select cast(s.Amount as Int) TotalAvailable ,size,CoverType,cast(PageNo as Int) PageNo from vwGood with(nolock) Join GoodStack s with(nolock) on GoodCode = GoodRef where StackRef = {Stackref} And Goodcode={GoodCode}";


            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }




        [HttpGet]
        [Route("ExitDelivery")]
        public string ExitDelivery(string Where)
        {

            string query = " update AppOCRFactor set HasSignature=0,AppIsDelivered=0 where AppOCRFactorCode= " + Where;

            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }



        [HttpGet]
        [Route("GetJob")]
        public string GetJob(string Where)
        {

            string query = "select JobCode,Title,Explain from job where Explain='" + Where + "'";



            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Jobs", "");

        }




        [HttpGet]
        [Route("GetJobPerson")]
        public string GetJobPerson(string Where)
        {

            string query = "select j.JobCode,jp.JobPersonCode,j.Title,c.Name,c.FName from JobPerson jp  join job j on j.JobCode=jp.JobRef  join Central c on c.CentralCode=jp.CentralRef  where j.Title='" + Where + "'";



            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "JobPersons", "");

        }






        [HttpGet]
        [Route("GetOcrFactorDetail")]
        public string GetOcrFactorDetail(string OCRFactorCode)
        {

            string query = "[dbo].[spApp_ocrGetFactorDetail] " + OCRFactorCode;
            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "AppOcrFactors", "");

        }



        [HttpGet]
        [Route("GetCustomerPath")]
        public string GetCustomerPath()
        {

            string query = "Select Distinct IsNull(" + _configuration.GetConnectionString("Ocr_CustomerPath") + " , '') " + _configuration.GetConnectionString("Ocr_CustomerPath_Lible") + " From PropertyValue Where ClassName= 'TCustomer'";
            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }


        [HttpGet]
        [Route("GetStackCategory")]
        public string GetStackCategory()
        {

            string query = "Select Distinct IsNull(" + _configuration.GetConnectionString("Ocr_StackCategory") + " , '') " + _configuration.GetConnectionString("Ocr_StackCategory") + " From good";
            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }





        [HttpPost]
        [Route("SaveOcrImage")]
        public string SaveOcrImage([FromBody] OcrModel ocrModel)
        {

            string image_base64 = ocrModel.ImageStr;
            string query = $"Exec dbo.spApp_ocrGetFactor '{ocrModel.barcode}', 0 ";

            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            string dbname = Convert.ToString(dataTable.Rows[0]["dbname"]);
            string FactorRef = Convert.ToString(dataTable.Rows[0]["FactorRef"]);
            string TcPrintRef = Convert.ToString(dataTable.Rows[0]["TcPrintRef"]);



            string base64Image = ocrModel.ImageStr;
            byte[] imageBytes = Convert.FromBase64String(base64Image);



            string imageName = $"{ocrModel.barcode}.jpg"; // Constructing the image name
            string imageName_zip = $"{ocrModel.barcode}.zip"; // Constructing the image name
            string imagePath = _configuration.GetConnectionString("Ocr_imagePath") + $"{imageName}"; // Provide the path where you want to save the image
            string image_zipPath = _configuration.GetConnectionString("Ocr_imagePath") + $"{imageName_zip}"; // Provide the path where you want to save the zip file
            Console.WriteLine(imagePath);
            Console.WriteLine(image_zipPath);

            System.IO.File.WriteAllBytes(imagePath, imageBytes);
            
            // Create a zip archive and add the image file to it
            using (FileStream zipStream = new FileStream(image_zipPath, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(imagePath, imageName);
                }
            }

            
            string connectionString = _configuration.GetConnectionString("Ocr_Connection"); // Provide your SQL Server connection string



            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                // Construct the SQL query with parameters
                string sqlCommandText = @" INSERT INTO " + dbname + @".dbo.AttachedFiles
                                            (Title, ClassName, ObjectRef, FileName, SourceFile, Type, Owner, CreationDate, Reformer, ReformDate, TcPrintRef)
                                             VALUES
                                             ('App_ocr', 'Factor', @FactorRef, @ImageName, @SourceFile, 'Zip', -1000, GETDATE(), -1000, GETDATE(), @TcPrintRef)  ";

                // Create a SqlCommand object
                using (SqlCommand sqlCommand = new SqlCommand(sqlCommandText, dbConnection))
                {
                    // Bind parameters
                    sqlCommand.Parameters.AddWithValue("@FactorRef", FactorRef);
                    sqlCommand.Parameters.AddWithValue("@ImageName", ocrModel.barcode + ".zip");
                    sqlCommand.Parameters.AddWithValue("@SourceFile", System.IO.File.ReadAllBytes(image_zipPath));
                    sqlCommand.Parameters.AddWithValue("@TcPrintRef", TcPrintRef);

                    // Execute the command
                    sqlCommand.ExecuteNonQuery();
                }
            }




            System.IO.File.Delete(imagePath);
            System.IO.File.Delete(image_zipPath);





            string query4 = $"UPDATE AppOCRFactor SET HasSignature = 1 WHERE AppTcPrintRef = {TcPrintRef} ";
            DataTable dataTable4 = db.Order_ExecQuery(HttpContext, query4);

            string query11 = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable2 = db.Order_ExecQuery(HttpContext, query11);

            return jsonClass.JsonResult_Str(dataTable2, "Text", "TodeyFromServer");
        }





    }




}

