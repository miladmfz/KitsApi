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


        [HttpGet]
        [Route("GetObjectTypeFromDbSetup")]
        public string GetObjectTypeFromDbSetup(string ObjectType)
        {

            string query = $"select * from dbo.fnObjectType('{ObjectType}') ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "ObjectTypes", "");

        }


        [HttpPost]
        [Route("OrderMizList")]
        public string OrderMizList([FromBody] OrderModel orderModel)
        {

            string query = $" exec spApp_OrderMizList  {orderModel.InfoState}, N'{orderModel.MizType}' ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }





        [HttpGet]
        [Route("OrderPrintFactor")]
        public string OrderPrintFactor(String AppBasketInfoRef)
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


        }











        [HttpGet]
        [Route("DbSetupvalue")]
        public string DbSetupvalue(string Where)
        {

            string query = $"select top 1 DataValue from dbsetup where KeyValue = '{Where}'";



            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");


        }



        [HttpGet]
        [Route("GetDistinctValues")]
        public string GetDistinctValues(
            string TableName,
            string FieldNames,
            string WhereClause
            )
        {

            string query = $"Exec spAppGetDistinctValues '{TableName}','{FieldNames} Value','{WhereClause}' ";





            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Values", "");

        }







        [HttpGet]
        [Route("GetSellBroker")]
        public string GetSellBroker()
        {

            string query = "Select brokerCode,BrokerNameWithoutType,CentralRef,Active From vwSellBroker";


            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "SellBrokers", "");

        }












        [HttpGet]
        [Route("OrderChangeTable")]
        public string OrderChangeTable(String AppBasketInfoRef)
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


            DataTable dataTable = db.Order_ExecQuery(HttpContext, sq);

            return jsonClass.JsonResult_Str(dataTable, "Groups", "");

        }
        

        [HttpPost]
        [Route("GetOrderGoodList")]
        public string GetOrderGoodList([FromBody] OrderModel orderModel)
        {

            //string query = "Exec spApp_GetGoods2 @RowCount = $RowCount,@Where = N'$Where',@AppBasketInfoRef=$AppBasketInfoRef, @GroupCode = $GroupCode ,@AppType=3 , @OrderBy = ' order by PrivateCodeForSort ' ";
            string query = $"Exec spApp_GetGoods2 @RowCount = {orderModel.RowCount}, @Where = N'{orderModel.Where}', @AppBasketInfoRef = {orderModel.AppBasketInfoRef}, @GroupCode = {orderModel.GroupCode}, @AppType = 3, @OrderBy = ' order by PrivateCodeForSort '";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }



        [HttpGet]
        [Route("DeleteGoodFromBasket")]
        public string DeleteGoodFromBasket(
            string RowCode,
            string AppBasketInfoRef
            )
        {

            string query = $"Delete From AppBasket Where AppBasketInfoRef = {AppBasketInfoRef} and AppBasketCode = {RowCode}";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

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

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }


        [HttpGet]
        [Route("OrderReserveList")]
        public string OrderReserveList(string MizRef)
        {

            string query = $"exec spApp_OrderReserveList {MizRef}"  ;

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }

        [HttpGet]
        [Route("GetTodeyFromServer")]
        public string GetTodeyFromServer()
        {

            string query = "select dbo.fnDate_Today() TodeyFromServer ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

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

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetOrderSum")]
        public string GetOrderSum(string AppBasketInfoRef)
        {

            string query = $"Exec spApp_OrderGetSummmary {AppBasketInfoRef}" ;

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }

        [HttpGet]
        [Route("OrderGet")]
        public string OrderGet(string AppBasketInfoRef, string AppType)
        {

            string query = $"Exec [dbo].[spApp_OrderGet] {AppBasketInfoRef} , {AppType} ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("OrderGetFactor")]
        public string OrderGetFactor(string AppBasketInfoRef)
        {

            string query = $"Exec [dbo].[spApp_OrderGetFactor] {AppBasketInfoRef}  ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }



        [HttpGet]
        [Route("OrderToFactor")]
        public string OrderToFactor(string AppBasketInfoRef)
        {

            string query = $"Exec [dbo].[spApp_OrderToFactor] {AppBasketInfoRef} , -2000 ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }

        [HttpGet]
        [Route("OrderGetAppPrinter")]
        public string OrderGetAppPrinter()
        {

            string query = $"select * from AppPrinter ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

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

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");

        }

        [HttpGet]
        [Route("OrderDeleteAll")]
        public string OrderDeleteAll(string AppBasketInfoRef)
        {

            string query = $"Delete From AppBasket Where  PreFactorCode is null and  AppBasketInfoRef= {AppBasketInfoRef} ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");

        }

        [HttpGet]
        [Route("OrderInfoReserveDelete")]
        public string OrderInfoReserveDelete(string AppBasketInfoRef)
        {

            string query = $" spApp_OrderInfoReserveDelete  {AppBasketInfoRef} ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Text", "Done");

        }

        [HttpPost]
        [Route("OrderEditInfoExplain")]
        public string OrderEditInfoExplain([FromBody] OrderModel orderModel)
        {

            string query = $" spApp_OrderInfoUpdateExplain  '{orderModel.Explain}', {orderModel.AppBasketInfoCode}   ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "BasketInfos", "");

        }






        [HttpGet]
        [Route("GetGoodFromGroup")]
        public string GetGoodFromGroup(string GroupCode)
        {

            string query = $"select GoodCode,GoodName,MaxSellPrice,'' ImageName from vwGood where  GoodCode in(Select GoodRef From GoodGroup p Join GoodsGrp s on p.GoodGroupRef = s.GroupCode Where s.GroupCode = {GroupCode} or s.L1 = {GroupCode} or s.L2 = {GroupCode} or s.L3 = {GroupCode} )";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

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


            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Factors", "");

        }








    }



}
