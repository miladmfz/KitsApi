using System.Data;
using Microsoft.AspNetCore.Mvc;
using webapikits.Model;

using FastReport;
using FastReport.Export.PdfSimple;
using System.Reflection;
using static Stimulsoft.Report.StiOptions;


namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        public readonly IConfiguration _configuration;
        DataBaseClass db;
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();
        FileManager fileManager = new();


        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }




        public class OrderModel
        {
            public string MizType { get; set; } = "";
            public string InfoState { get; set; } = "0";



            public string GroupCode { get; set; } = "0";
            public string RowCount { get; set; } = "100";
            public string Where { get; set; } = "";
            public string AppBasketInfoRef { get; set; } = "0";




            public string Broker { get; set; } = "0";
            public string Miz { get; set; } = "0";
            public string PersonName { get; set; } = "";
            public string Mobile { get; set; } = "";
            public string InfoExplain { get; set; } = "";
            public string Prepayed { get; set; } = "0";
            public string ReserveStartTime { get; set; } = "";
            public string ReserveEndTime { get; set; } = "";
            public string Date { get; set; } = "";
            public string State { get; set; } = "0";
            public string InfoCode { get; set; } = "0";



            public string GoodRef { get; set; } = "0";
            public string FacAmount { get; set; } = "0";
            public string Price { get; set; } = "0";
            public string bUnitRef { get; set; } = "0";
            public string bRatio { get; set; } = "0";
            public string Explain { get; set; } = "";
            public string UserId { get; set; } = "-3000";
            public string InfoRef { get; set; } = "0";
            public string RowCode { get; set; } = "0";



            public string AppBasketInfoCode { get; set; } = "0";


        }


        [HttpGet]
        [Route("GetObjectTypeFromDbSetup")]
        public string GetObjectTypeFromDbSetup(string ObjectType)
        {

            string query = "select * from dbo.fnObjectType('" + ObjectType + "') ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "ObjectTypes", "");

        }


        [HttpPost]
        [Route("OrderMizList")]
        public string OrderMizList([FromBody] OrderModel orderModel)
        {

            string query = $" exec spApp_OrderMizList  {orderModel.InfoState}, N'{orderModel.MizType}' ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }





        [HttpGet]
        [Route("OrderPrintFactor")]
        public string OrderPrintFactor(String AppBasketInfoRef)
        {




            string query = " select * from AppPrinter ";

            DataTable Table_print = db.Order_ExecQuery(Request.Path,  query);

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


                string s1 = printer.AppPrinterCode;
                string s2 = printer.PrinterName;
                string s3 = printer.PrinterExplain;
                string s4 = printer.GoodGroups;
                string s5 = printer.WhereClause;
                string s6 = printer.PrintCount;
                string s7 = printer.PrinterActive;
                string s8 = printer.FilePath;
                string s9 = printer.AppType;


                // نام فایل به عنوان هدر درخواست
                Report report = new Report();
                report.Load(printer.FilePath);


                query = $"Exec [dbo].[spApp_OrderGetFactor ] {AppBasketInfoRef} ";
                DataTable dataTable_factor = db.Order_ExecQuery(Request.Path, query);

                List<Factor> factorHeader = new List<Factor>();






                // Check if the DataTable has rows before accessing the first row
                if (dataTable_factor.Rows.Count > 0)
                {
                    Factor factor = new Factor(); // Create a new Factor object
                    factor.AppBasketInfoCode = Convert.ToString(dataTable_factor.Rows[0]["AppBasketInfoCode"]);

                    // Add the factor object to the list
                    factorHeader.Add(factor);
                }
                else
                {
                    // Handle the case when there are no rows in the DataTable
                    // For example, you might want to log a message or handle it differently based on your requirements
                }









                if (dataTable_factor.Rows.Count > 0)
                {


                factorHeader[0].AppBasketInfoCode = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["AppBasketInfoCode"]));
                factorHeader[0].AppBasketInfoDate = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["AppBasketInfoDate"]));
                factorHeader[0].DailyCode = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["DailyCode"]));
                factorHeader[0].MizType = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["MizType"]));
                factorHeader[0].RstMizName = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["RstMizName"]));
                factorHeader[0].InfoExplain = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["InfoExplain"]));
                factorHeader[0].FactorExplain = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["FactorExplain"]));
                factorHeader[0].TimeStart = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["TimeStart"]));
                factorHeader[0].InfoPrintCount = Convert.ToString(dataTable_factor.Rows[0]["InfoPrintCount"]);
                factorHeader[0].InfoState = Convert.ToString(dataTable_factor.Rows[0]["InfoState"]);
                factorHeader[0].ReserveStart = Convert.ToString(dataTable_factor.Rows[0]["ReserveStart"]);
                if (Convert.ToInt64(dataTable_factor.Rows[0]["InfoPrintCount"]) > 0)
                {
                        factorHeader[0].CustName = "(چاپ مجدد)";
                }
                
                }
                {
                    Console.WriteLine("No factor");

                    // Handle the case when there are no rows in the DataTable
                    // For example, you might want to log a message or handle it differently based on your requirements
                }
                string convertedString = printer.WhereClause.Replace("=''", "=N''");

                query = $"Exec [dbo].[spApp_OrderGetFactorRow ] {AppBasketInfoRef} , {printer.GoodGroups} , N'{convertedString}' ";
                DataTable dataTable_Row = db.Order_ExecQuery(Request.Path, query);




                List<FactorRow> FactorRows = new List<FactorRow>();


                if (dataTable_Row.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable_Row.Rows.Count; i++)
                    {
                        FactorRow factorRow = new FactorRow();


                        if (Convert.ToString(dataTable_Row.Rows[i]["IsExtra"]) == "1")
                        {
                            factorRow.GoodName = Convert.ToString(dataTable_Row.Rows[i]["GoodName"])+ " (سفارش مجدد) ";
                            factorRow.GoodName = db.ConvertToPersianNumber(factorRow.GoodName);

                        }
                        else {
                            factorRow.GoodName = Convert.ToString(dataTable_Row.Rows[i]["GoodName"]);
                            factorRow.GoodName = db.ConvertToPersianNumber(factorRow.GoodName);

                        }

                        factorRow.FactorRowCode = Convert.ToString(dataTable_Row.Rows[i]["FactorRowCode"]);
                        factorRow.GoodRef = Convert.ToString(dataTable_Row.Rows[i]["GoodRef"]);
                        factorRow.FacAmount = Convert.ToString(dataTable_Row.Rows[i]["FacAmount"]);
                        factorRow.FacAmount = factorRow.FacAmount.Substring(0,factorRow.FacAmount.IndexOf("."));
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


                if (Convert.ToInt64(printer.PrintCount) > 0) {

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

            string quer1y = $"spApp_Order_CanPrint  {AppBasketInfoRef} , 0  ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, quer1y);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");


        }

        [HttpGet]
        [Route("OrderChangeTable")]
        public string OrderChangeTable(String AppBasketInfoRef)
        {

            string quer1y = $"spApp_Order_CanPrint  {AppBasketInfoRef} , 1  ";

            DataTable dataTable1 = db.Order_ExecQuery(Request.Path, quer1y);


            string query = " select * from AppPrinter ";

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


                string s1 = printer.AppPrinterCode;
                string s2 = printer.PrinterName;
                string s3 = printer.PrinterExplain;
                string s4 = printer.GoodGroups;
                string s5 = printer.WhereClause;
                string s6 = printer.PrintCount;
                string s7 = printer.PrinterActive;
                string s8 = printer.FilePath;
                string s9 = printer.AppType;


                // نام فایل به عنوان هدر درخواست
                Report report = new Report();
                report.Load(printer.FilePath);


                query = $"Exec [dbo].[spApp_OrderGetFactor ] {AppBasketInfoRef} ";
                DataTable dataTable_factor = db.Order_ExecQuery(Request.Path, query);

                List<Factor> factorHeader = new List<Factor>();






                // Check if the DataTable has rows before accessing the first row
                if (dataTable_factor.Rows.Count > 0)
                {
                    Factor factor = new Factor(); // Create a new Factor object
                    factor.AppBasketInfoCode = Convert.ToString(dataTable_factor.Rows[0]["AppBasketInfoCode"]);

                    // Add the factor object to the list
                    factorHeader.Add(factor);
                }
                else
                {
                    // Handle the case when there are no rows in the DataTable
                    // For example, you might want to log a message or handle it differently based on your requirements
                }









                if (dataTable_factor.Rows.Count > 0)
                {


                    factorHeader[0].AppBasketInfoCode = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["AppBasketInfoCode"]));
                    factorHeader[0].AppBasketInfoDate = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["AppBasketInfoDate"]));
                    factorHeader[0].DailyCode = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["DailyCode"]));
                    factorHeader[0].MizType = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["MizType"]));
                    factorHeader[0].RstMizName = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["RstMizName"]));
                    factorHeader[0].InfoExplain = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["InfoExplain"]));
                    factorHeader[0].FactorExplain = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["FactorExplain"]));
                    factorHeader[0].TimeStart = db.ConvertToPersianNumber(Convert.ToString(dataTable_factor.Rows[0]["TimeStart"]));
                    factorHeader[0].InfoPrintCount = Convert.ToString(dataTable_factor.Rows[0]["InfoPrintCount"]);
                    factorHeader[0].InfoState = Convert.ToString(dataTable_factor.Rows[0]["InfoState"]);
                    factorHeader[0].ReserveStart = Convert.ToString(dataTable_factor.Rows[0]["ReserveStart"]);
                    if (Convert.ToInt64(dataTable_factor.Rows[0]["InfoPrintCount"]) > 0)
                    {
                        factorHeader[0].CustName = "(چاپ مجدد)";
                    }

                }
                {
                    Console.WriteLine("No factor");

                    // Handle the case when there are no rows in the DataTable
                    // For example, you might want to log a message or handle it differently based on your requirements
                }
                string convertedString = printer.WhereClause.Replace("=''", "=N''");

                query = $"Exec [dbo].[spApp_OrderGetFactorRow ] {AppBasketInfoRef} , {printer.GoodGroups} , N'{convertedString}' ";
                DataTable dataTable_Row = db.Order_ExecQuery(Request.Path, query);




                List<FactorRow> FactorRows = new List<FactorRow>();


                if (dataTable_Row.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable_Row.Rows.Count; i++)
                    {
                        FactorRow factorRow = new FactorRow();


                        if (Convert.ToString(dataTable_Row.Rows[i]["IsExtra"]) == "1")
                        {
                            factorRow.GoodName = Convert.ToString(dataTable_Row.Rows[i]["GoodName"]) + " (سفارش مجدد) ";
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

                    bool justtable = printer.WhereClause.Contains("%");
                    if (!justtable)
                    {
                        if (Convert.ToInt64(printer.PrintCount) > 0)
                        {

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



            }

            string query2 = $"spApp_Order_CanPrint  {AppBasketInfoRef} , 0  ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query2);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");
        }


        [HttpGet]
        [Route("GetOrdergroupList")]
        public string GetOrdergroupList( string GroupCode   )
        {

            string sq = "Exec [dbo].[spApp_GetGoodGroups]  @GroupName = N''  ";


            if (!string.IsNullOrEmpty(GroupCode))
            {
                sq += $" , @GroupCode = {GroupCode} ";
            }


            DataTable dataTable = db.Order_ExecQuery(Request.Path, sq);

            return jsonClass.JsonResult_Str(dataTable, "Groups", "");

        }
        

        [HttpPost]
        [Route("GetOrderGoodList")]
        public string GetOrderGoodList([FromBody] OrderModel orderModel)
        {

            //string query = "Exec spApp_GetGoods2 @RowCount = $RowCount,@Where = N'$Where',@AppBasketInfoRef=$AppBasketInfoRef, @GroupCode = $GroupCode ,@AppType=3 , @OrderBy = ' order by PrivateCodeForSort ' ";
            string query = $"Exec spApp_GetGoods2 @RowCount = {orderModel.RowCount}, @Where = N'{orderModel.Where}', @AppBasketInfoRef = {orderModel.AppBasketInfoRef}, @GroupCode = {orderModel.GroupCode}, @AppType = 3, @OrderBy = ' order by PrivateCodeForSort '";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }



        [HttpGet]
        [Route("DeleteGoodFromBasket")]
        public string DeleteGoodFromBasket(
            string RowCode,
            string AppBasketInfoRef
            )
        {

            //string query = "Exec spApp_GetGoods2 @RowCount = $RowCount,@Where = N'$Where',@AppBasketInfoRef=$AppBasketInfoRef, @GroupCode = $GroupCode ,@AppType=3 , @OrderBy = ' order by PrivateCodeForSort ' ";
            string query = $"Delete From AppBasket Where AppBasketInfoRef = {AppBasketInfoRef} and AppBasketCode = {RowCode}";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");

        }


        [HttpPost]
        [Route("OrderInfoInsert")]
        public string OrderInfoInsert([FromBody] OrderModel orderModel)
        {

            string query = $"exec spApp_OrderInfoInsert  {orderModel.Broker} , {orderModel.Miz} " +
                        $" ,'{orderModel.PersonName}','{orderModel.Mobile}','{orderModel.InfoExplain}'" +
                        $" ,{orderModel.Prepayed},'{orderModel.ReserveStartTime}','{orderModel.ReserveEndTime}'" +
                        $" ,'{orderModel.Date}',{orderModel.State},{orderModel.InfoCode}";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }


        [HttpGet]
        [Route("OrderReserveList")]
        public string OrderReserveList(string MizRef)
        {

            string query = "exec spApp_OrderReserveList " + MizRef;

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }

        [HttpGet]
        [Route("GetTodeyFromServer")]
        public string GetTodeyFromServer()
        {

            string query = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");

        }


        [HttpPost]
        [Route("OrderRowInsert")]
        public string OrderRowInsert([FromBody] OrderModel orderModel)
        {

            string query = $"[dbo].[spApp_OrderRowInsert] {orderModel.GoodRef}," +
                        $" {orderModel.FacAmount}, {orderModel.Price}, {orderModel.bUnitRef}," +
                        $" {orderModel.bRatio}, '{orderModel.Explain}', {orderModel.UserId}," +
                        $" {orderModel.InfoRef}, {orderModel.RowCode}";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetOrderSum")]
        public string GetOrderSum(string AppBasketInfoRef)
        {

            string query = "Exec spApp_OrderGetSummmary " + AppBasketInfoRef;

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }

        [HttpGet]
        [Route("OrderGet")]
        public string OrderGet(string AppBasketInfoRef, string AppType)
        {

            string query = $"Exec [dbo].[spApp_OrderGet] {AppBasketInfoRef} , {AppType} ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("OrderGetFactor")]
        public string OrderGetFactor(string AppBasketInfoRef)
        {

            string query = $"Exec [dbo].[spApp_OrderGetFactor] {AppBasketInfoRef}  ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }



        [HttpGet]
        [Route("OrderToFactor")]
        public string OrderToFactor(string AppBasketInfoRef)
        {

            string query = $"Exec [dbo].[spApp_OrderToFactor] {AppBasketInfoRef} , -2000 ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }

        [HttpGet]
        [Route("OrderGetAppPrinter")]
        public string OrderGetAppPrinter()
        {

            string query = $"select * from AppPrinter ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "AppPrinters", "");

        }

        [HttpGet]
        [Route("Order_CanPrint")]
        public string Order_CanPrint(
        string AppBasketInfoRef,
        string CanPrint
        )
        {

            string query = $"spApp_Order_CanPrint  {AppBasketInfoRef} ,{CanPrint}  ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");

        }

        [HttpGet]
        [Route("OrderDeleteAll")]
        public string OrderDeleteAll(string AppBasketInfoRef)
        {

            string query = $"Delete From AppBasket Where  PreFactorCode is null and  AppBasketInfoRef= {AppBasketInfoRef} ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");

        }

        [HttpGet]
        [Route("OrderInfoReserveDelete")]
        public string OrderInfoReserveDelete(string AppBasketInfoRef)
        {

            string query = $" spApp_OrderInfoReserveDelete  {AppBasketInfoRef} ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");

        }

        [HttpPost]
        [Route("OrderEditInfoExplain")]
        public string OrderEditInfoExplain([FromBody] OrderModel orderModel)
        {

            string query = $" spApp_OrderInfoUpdateExplain  '{orderModel.Explain}', {orderModel.AppBasketInfoCode}   ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }






        /*




       [HttpGet]
       [Route("OrderSendImage")]
       public string OrderSendImage(
           string Image,
           string Code,
           string PrinterName,
           string PrintCount
           )
       {



                 $decodedImage = base64_decode($Image);
         file_put_contents("FactorImage/$Code.jpg", $decodedImage);


         try {

             $tux = EscposImage::load(__DIR__ . "/../FactorImage/$Code.jpg", false);
             //$connector = new WindowsPrintConnector($PrinterName);
             //$connector = new FilePrintConnector("//192.168.1.33/asd");
             $connector = new FilePrintConnector("$PrinterName");
             $printer = new Printer($connector);
             $printer -> setJustification( Printer::JUSTIFY_CENTER );
             echo "{\"Text\":\"Done\"}";
             for ($x = 0; $x < $PrintCount; $x++) {
                 $printer -> getPrintConnector() -> write(PRINTER::RS);

                 $printer -> graphics($tux);
                 $printer -> cut();
             }


             $printer -> close();
              $filename=__DIR__ . "/../FactorImage/$Code.jpg";
              unlink($filename);
             DataTable dataTable = db.Order_ExecQuery(sq);

           return jsonClass.JsonResult_Str(dataTable, "Groups", "");
           



    }


         */





        [HttpGet]
        [Route("WebOrderMizData")]
        public string WebOrderMizData(string RstMizCode)
        {
            string query = $"exec spApp_OrderMizData {RstMizCode}";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.ConvertDataTableToJson(dataTable);

        }




        [HttpGet]
        [Route("WebOrderInfoInsert")]
        public string WebOrderInfoInsert(string Miz, string Date)
        {

            string query = $"exec spApp_OrderInfoInsert 0,{Miz},'','','',0,'','','{Date}',1,0 ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);
            return jsonClass.ConvertDataTableToJson(dataTable);
        }




        [HttpGet]
        [Route("GetMenuOnlinegroups")]
        public string GetMenuOnlinegroups(
            string GroupName,
            string GroupCode
            )
        {

            string sq = "Exec [dbo].[spApp_GetGoodGroups] @where=' And GroupCode <100', ";

            if (!string.IsNullOrEmpty(GroupName))
            {
                sq += $" @GroupName = N'{GroupName}' ";
            }

            if (!string.IsNullOrEmpty(GroupCode))
            {
                sq += $" @GroupCode = N'{GroupCode}' ";
            }


            DataTable dataTable = db.Order_ExecQuery(Request.Path, sq);

            return jsonClass.JsonResult_Str(dataTable, "Groups", "");

        }


        [HttpGet]
        [Route("GetGoodFromGroup")]
        public string GetGoodFromGroup(string GroupCode)
        {

            string query = "select GoodCode,GoodName,MaxSellPrice,'' ImageName from vwGood where  GoodCode in(Select GoodRef From GoodGroup p Join GoodsGrp s on p.GoodGroupRef = s.GroupCode Where s.GroupCode = " + GroupCode + " or s.L1 = " + GroupCode + " or s.L2 = " + GroupCode + " or s.L3 = " + GroupCode + " )";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }




        [HttpGet]
        [Route("OrderGetFactorRow")]
        public string OrderGetFactorRow(
            string AppBasketInfoRef,
            string GoodGroups,
            string Where
            )
        {

            string query = $"Exec [dbo].[spApp_OrderGetFactorRow] {AppBasketInfoRef}, {GoodGroups}, '{Where}'";


            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }








    }



}
