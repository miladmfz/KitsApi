using System.Data;
using Microsoft.AspNetCore.Mvc;
using webapikits.Model;
using FastReport.Export.PdfSimple;
using FastReport;
using System.Drawing.Printing;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        //public readonly IConfiguration _configuration;
        //DataBaseClass db;
        //JsonClass jsonClass = new JsonClass();
        //Dictionary<string, string> jsonDict = new Dictionary<string, string>();
        //FileManager fileManager = new();


        //public OrderController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    db = new DataBaseClass(_configuration);

        //}

        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public OrderController(
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
        [Route("GetObjectTypeFromDbSetup")]
        public async Task<IActionResult> GetObjectTypeFromDbSetup(string ObjectType)
        {

            string query = $"select * from dbo.fnObjectType('{ObjectType}') ";

             

            
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "ObjectTypes", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetObjectTypeFromDbSetup));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpPost]
        [Route("OrderMizList")]
        public async Task<IActionResult> OrderMizList([FromBody] OrderModel orderModel)
        {

            string query = $" exec spApp_OrderMizList  {orderModel.InfoState}, N'{orderModel.MizType}' ";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderMizList));
                return StatusCode(500, "Internal server error.");
            }

        }



        /*

        [HttpGet]
        [Route("OrderPrintFactor")]
        public async Task<IActionResult> OrderPrintFactor(String AppBasketInfoRef)
        {




            string query = " select * from AppPrinter where Apptype = 3 ";

            DataTable Table_print = db.Order_ExecQuery(HttpContext,  query);

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
                if (printer.PrinterActive== "True") {
                    if (Convert.ToInt64(printer.PrintCount) > 0)
                    {


                        Report report = new Report();
                        report.Load(printer.FilePath);


                        query = $"Exec [dbo].[spApp_OrderGetFactor ] {AppBasketInfoRef} ";
                        DataTable dataTable_factor = db.Order_ExecQuery(HttpContext, query);

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
                            factor.CustName = _configuration.GetConnectionString("Order_chapmojadad");
                        }
                        factorHeader.Add(factor);



                        string convertedString = printer.WhereClause.Replace("=''", "=N''");

                        query = $"Exec [dbo].[spApp_OrderGetFactorRow ] {AppBasketInfoRef} , {printer.GoodGroups} , N'{convertedString}' ";
                        DataTable dataTable_Row = db.Order_ExecQuery(HttpContext, query);




                        List<FactorRow> FactorRows = new List<FactorRow>();


                        if (dataTable_Row.Rows.Count > 0)
                        {

                            for (int i = 0; i < dataTable_Row.Rows.Count; i++)
                            {
                                FactorRow factorRow = new FactorRow();
                                Console.WriteLine((dataTable_Row.Rows[i]["IsExtra"]));

                                if (Convert.ToString(dataTable_Row.Rows[i]["IsExtra"]) == "True")
                                {
                                    factorRow.GoodName = Convert.ToString(dataTable_Row.Rows[i]["GoodName"]) + _configuration.GetConnectionString("Order_sefareshmojadad");
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
                                pdfPrinter.PrintPdfs(ms, printer.PrinterName);






                            }

                        }

                    }

                }


            }

            string query11 = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query11);

            return jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(IsUser));
                return StatusCode(500, "Internal server error.");
            }


        }





        */





        [HttpGet]
        [Route("DbSetupvalue")]
        public async Task<IActionResult> DbSetupvalue(string Where)
        {

            string query = $"select top 1 DataValue from dbsetup where KeyValue = '{Where}'";



             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DbSetupvalue));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("GetDistinctValues")]
        public async Task<IActionResult> GetDistinctValues(
            string TableName,
            string FieldNames,
            string WhereClause
            )
        {

            string query = $"Exec spAppGetDistinctValues '{TableName}','{FieldNames} Value','{WhereClause}' ";

             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Values", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetDistinctValues));
                return StatusCode(500, "Internal server error.");
            }


        }







        [HttpGet]
        [Route("GetSellBroker")]
        public async Task<IActionResult> GetSellBroker()
        {

            string query = "Select brokerCode,BrokerNameWithoutType,CentralRef,Active From vwSellBroker";


             
            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "SellBrokers", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetSellBroker));
                return StatusCode(500, "Internal server error.");
            }

        }








        /*



        [HttpGet]
        [Route("OrderChangeTable")]
        public async Task<IActionResult> OrderChangeTable(String AppBasketInfoRef)
        {


            string query = " select * from AppPrinter where Apptype = 3 ";

            DataTable Table_print = db.Order_ExecQuery(HttpContext, query);

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
                    DataTable dataTable_factor = db.Order_ExecQuery(HttpContext, query);

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
                        factor.CustName = _configuration.GetConnectionString("Order_chapmojadad");
                    }
                    factorHeader.Add(factor);


                    string convertedString = printer.WhereClause.Replace("=''", "=N''");

                    query = $"Exec [dbo].[spApp_OrderGetFactorRow ] {AppBasketInfoRef} , {printer.GoodGroups} , N'{convertedString}' ";
                    DataTable dataTable_Row = db.Order_ExecQuery(HttpContext, query);




                    List<FactorRow> FactorRows = new List<FactorRow>();


                    if (dataTable_Row.Rows.Count > 0)
                    {
                        for (int i = 0; i < dataTable_Row.Rows.Count; i++)
                        {
                            FactorRow factorRow = new FactorRow();


                            if (Convert.ToString(dataTable_Row.Rows[i]["IsExtra"]) == "True")
                            {
                                factorRow.GoodName = Convert.ToString(dataTable_Row.Rows[i]["GoodName"]) + _configuration.GetConnectionString("Order_sefareshmojadad");
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


                            if (report.Prepare())
                            {

                                MemoryStream ms = new MemoryStream();


                                // Export the report to PDF
                                PDFSimpleExport pdfExport = new PDFSimpleExport();
                                pdfExport.ShowProgress = false;
                                pdfExport.Subject = "Subject Report";
                                pdfExport.Title = "Report Title";
                                

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

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query2);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(IsUser));
                return StatusCode(500, "Internal server error.");
            }

        }


        */





        [HttpGet]
        [Route("GetOrdergroupList")]
        public async Task<IActionResult> GetOrdergroupList( string GroupCode   )
        {

            string query = "Exec [dbo].[spApp_GetGoodGroups]  @GroupName = N''  ";


            if (!string.IsNullOrEmpty(GroupCode))
            {
                query += $" , @GroupCode = {GroupCode} ";
            }


           

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Groups", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetOrdergroupList));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpPost]
        [Route("GetOrderGoodList")]
        public async Task<IActionResult> GetOrderGoodList([FromBody] OrderModel orderModel)
        {

            //string query = "Exec spApp_GetGoods2 @RowCount = $RowCount,@Where = N'$Where',@AppBasketInfoRef=$AppBasketInfoRef, @GroupCode = $GroupCode ,@AppType=3 , @OrderBy = ' order by PrivateCodeForSort ' ";
            string query = $"Exec spApp_GetGoods2 @RowCount = {orderModel.RowCount}, @Where = N'{orderModel.Where}', @AppBasketInfoRef = {orderModel.AppBasketInfoRef}, @GroupCode = {orderModel.GroupCode}, @AppType = 3, @OrderBy = ' order by PrivateCodeForSort '";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetOrderGoodList));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("DeleteGoodFromBasket")]
        public async Task<IActionResult> DeleteGoodFromBasket(
            string RowCode,
            string AppBasketInfoRef
            )
        {

            string query = $"Delete From AppBasket Where AppBasketInfoRef = {AppBasketInfoRef} and AppBasketCode = {RowCode}";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "Done");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DeleteGoodFromBasket));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpPost]
        [Route("OrderInfoInsert")]
        public async Task<IActionResult> OrderInfoInsert([FromBody] OrderModel orderModel)
        {

            string query = $"exec spApp_OrderInfoInsert  {orderModel.Broker} , {orderModel.Miz} " +
                        $" ,'{orderModel.PersonName}','{orderModel.Mobile}','{orderModel.InfoExplain}'" +
                        $" ,{orderModel.Prepayed},'{orderModel.ReserveStartTime}','{orderModel.ReserveEndTime}'" +
                        $" ,'{orderModel.Date}',{orderModel.State},{orderModel.InfoCode}";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderInfoInsert));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpGet]
        [Route("OrderReserveList")]
        public async Task<IActionResult> OrderReserveList(string MizRef)
        {

            string query = $"exec spApp_OrderReserveList {MizRef}"  ;

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderReserveList));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("GetTodeyFromServer")]
        public async Task<IActionResult> GetTodeyFromServer()
        {

            string query = "select dbo.fnDate_Today() TodeyFromServer ";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "TodeyFromServer");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetTodeyFromServer));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpPost]
        [Route("OrderRowInsert")]
        public async Task<IActionResult> OrderRowInsert([FromBody] OrderModel orderModel)
        {

            string query = $"[dbo].[spApp_OrderRowInsert] {orderModel.GoodRef}," +
                        $" {orderModel.FacAmount}, {orderModel.Price}, {orderModel.bUnitRef}," +
                        $" {orderModel.bRatio}, '{orderModel.Explain}', {orderModel.UserId}," +
                        $" {orderModel.InfoRef}, {orderModel.RowCode}";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderRowInsert));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetOrderSum")]
        public async Task<IActionResult> GetOrderSum(string AppBasketInfoRef)
        {

            string query = $"Exec spApp_OrderGetSummmary {AppBasketInfoRef}" ;

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetOrderSum));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("OrderGet")]
        public async Task<IActionResult> OrderGet(string AppBasketInfoRef, string AppType)
        {

            string query = $"Exec [dbo].[spApp_OrderGet] {AppBasketInfoRef} , {AppType} ";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderGet));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("OrderGetFactor")]
        public async Task<IActionResult> OrderGetFactor(string AppBasketInfoRef)
        {

            string query = $"Exec [dbo].[spApp_OrderGetFactor] {AppBasketInfoRef}  ";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderGetFactor));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("OrderToFactor")]
        public async Task<IActionResult> OrderToFactor(string AppBasketInfoRef)
        {

            string query = $"Exec [dbo].[spApp_OrderToFactor] {AppBasketInfoRef} , -2000 ";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderToFactor));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("OrderGetAppPrinter")]
        public async Task<IActionResult> OrderGetAppPrinter()
        {

            string query = $"select * from AppPrinter ";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "AppPrinters", ""); ;
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderGetAppPrinter));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("Order_CanPrint")]
        public async Task<IActionResult> Order_CanPrint(
        string AppBasketInfoRef,
        string CanPrint
        )
        {

            string query = $"spApp_Order_CanPrint  {AppBasketInfoRef} ,{CanPrint}  ";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "Done");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Order_CanPrint));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("OrderDeleteAll")]
        public async Task<IActionResult> OrderDeleteAll(string AppBasketInfoRef)
        {

            string query = $"Delete From AppBasket Where  PreFactorCode is null and  AppBasketInfoRef= {AppBasketInfoRef} ";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "Done");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderDeleteAll));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("OrderInfoReserveDelete")]
        public async Task<IActionResult> OrderInfoReserveDelete(string AppBasketInfoRef)
        {

            string query = $" spApp_OrderInfoReserveDelete  {AppBasketInfoRef} ";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "Done");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderInfoReserveDelete));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpPost]
        [Route("OrderEditInfoExplain")]
        public async Task<IActionResult> OrderEditInfoExplain([FromBody] OrderModel orderModel)
        {

            string query = $" spApp_OrderInfoUpdateExplain  '{orderModel.Explain}', {orderModel.AppBasketInfoCode}   ";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderEditInfoExplain));
                return StatusCode(500, "Internal server error.");
            }

        }






        [HttpGet]
        [Route("GetGoodFromGroup")]
        public async Task<IActionResult> GetGoodFromGroup(string GroupCode)
        {

            string query = $"select GoodCode,GoodName,MaxSellPrice,'' ImageName from vwGood where  GoodCode in(Select GoodRef From GoodGroup p Join GoodsGrp s on p.GoodGroupRef = s.GroupCode Where s.GroupCode = {GroupCode} or s.L1 = {GroupCode} or s.L2 = {GroupCode} or s.L3 = {GroupCode} )";

             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodFromGroup));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("OrderGetFactorRow")]
        public async Task<IActionResult> OrderGetFactorRow(
            string AppBasketInfoRef,
            string GoodGroups,
            string Where
            )
        {

            string query = $"Exec [dbo].[spApp_OrderGetFactorRow] {AppBasketInfoRef}, {GoodGroups}, '{Where}'";


             

            try
            {
                DataTable dataTable = await db.Order_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Factors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OrderGetFactorRow));
                return StatusCode(500, "Internal server error.");
            }

        }








    }



}
