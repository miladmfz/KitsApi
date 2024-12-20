using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using FastReport;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapikits.Model;
using Image = System.Drawing.Image;
using FastReport.Export.PdfSimple;
using System.Diagnostics; // Add this namespace


namespace webapikits.Controllers
{




    [Route("api/[controller]")]
    [ApiController]
    public class KowsarController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        Dictionary<string, string> jsonDict = new();

        DataBaseClass db;
        DataTable DataTable = new ();
        Response response = new();
        FileManager fileManager = new();
        JsonClass jsonClass = new ();



        public KowsarController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new (_configuration);
        }




        [HttpGet]
        [Route("DbSetupvalue")]
        public string DbSetupvalue(string Where)
        {

            string query = $"select top 1 DataValue from dbsetup where KeyValue = '{Where}'";



            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");


        }



        [HttpGet]
        [Route("ToDoPrint")]
        public IActionResult ToDoPrint()
        {




            /*

            string base64Image = printRequest.Image;
            string code = printRequest.Code;
            string printerName = printRequest.PrinterName;
            int printCount = printRequest.PrintCount;

            byte[] imageBytes = Convert.FromBase64String(base64Image);
            string imagePath = $"FactorImage/{code}.jpg";

            // Save the image to a file
            System.IO.File.WriteAllBytes(imagePath, imageBytes);

            */



            string query = " select * from AppPrinter ";

            DataTable Table_print = db.Kowsar_ExecQuery(HttpContext, query);

            List<Printer> Printers = new();


            if (Table_print.Rows.Count > 0)
                {
                    for (int i = 0; i < Table_print.Rows.Count; i++)
                    {
                        Printer printer = new();

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



                Report report = new ();

                report.Load(printer.FilePath);


                query = "select top 2 GoodCode, GoodName, GoodExplain1 from good ";
                DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
                List<Good> goods = new ();


                if (dataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            Good g = new ();



                            g.GoodCode = Convert.ToString(dataTable.Rows[i]["GoodCode"]);
                            g.GoodName = Convert.ToString(dataTable.Rows[i]["GoodName"]);
                            g.GoodExplain1 = Convert.ToString(dataTable.Rows[i]["GoodExplain1"]);
                            goods.Add(g);


                        }
                    }


                report.RegisterData(goods, "GoodRef");



                if (report.Prepare())
                {
                    // Export the report to PDF
                    PDFSimpleExport pdfExport = new ();
                    PdfPrinter pdfPrinter = new();
                    MemoryStream ms = new();


                    pdfExport.ShowProgress = false;
                    pdfExport.Subject = "Subject Report";
                    pdfExport.Title = "Report Title";

                    report.Export(pdfExport, ms);
                    report.Dispose();
                    pdfExport.Dispose();
                    ms.Position = 0;


                    fileManager.SavePdfToStorage(ms, _configuration.GetConnectionString("Pdf_SaveStorage"));
                    pdfPrinter.PrintPdf(_configuration.GetConnectionString("Pdf_SaveStorage"), printer.PrinterName);




                }


            }

            return Ok("Ok");
            
        }



        [HttpGet]
        [Route("Check")]
        public string Check()
        {
            
            string query = "select top 2 GoodCode,GoodName,GoodExplain1 from good ";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            // Log the result to the console
            Debug.WriteLine("Check action result: " );

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");


        }








    [HttpGet]
        [Route("kowsarVersion")]
        public string kowsarVersion()
        {

            string query = "Exec spversioninfo";



            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            
            return jsonClass.JsonResult_Str(dataTable, "Text", "VerNo");


        }







        [HttpGet]
        [Route("GetGoodType")]
        public string GetGoodType()
        {

            string query = "Exec [spApp_GetGoodType]";



            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "Columns", "");

        }
        
        [HttpGet]
        [Route("GetColumnList")]
        public string GetColumnList(
            string GoodCode,
            string GoodType,
            string Type,
            string AppType,
            string IncludeZero
            )
        {
            if (string.IsNullOrEmpty(GoodCode))
            {
                GoodCode = "0";
            }
            if (string.IsNullOrEmpty(GoodType))
            {
                GoodType = "0";
            }
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
            

            string query;
            if (AppType == "1") {
                query = $"Exec [spApp_GetColumn] {GoodCode} ,'', {Type}, {AppType},{IncludeZero}";
            } else
            {
                query = $"Exec [spApp_GetColumn] {GoodCode} ,'{GoodType}', {Type},{AppType}, {IncludeZero}" ;
            }



            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            
            return jsonClass.JsonResult_Str(dataTable, "Columns", "");


        }

                
        [HttpGet]
        [Route("GetDistinctValues")]
        public string GetDistinctValues(
            string TableName,
            string FieldNames,
            string WhereClause
            )
        {

            string query= $"Exec spAppGetDistinctValues '{TableName}','{FieldNames} Value','{WhereClause}' ";





            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Values", "");

        }





        [HttpGet]
        [Route("GetSellBroker")]
        public string GetSellBroker()
        {

            string query= "Select brokerCode,BrokerNameWithoutType,CentralRef,Active From vwSellBroker";


            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "SellBrokers", "");

        }




        [HttpGet]
        [Route("GetImage")]
        public string GetImage(
            string ObjectRef,
            string IX,
            string Scale,
            string ClassName
            )
        {


            int sScale = Convert.ToInt32(Scale);

            string sq = $"Exec dbo.spApp_GetImage {ObjectRef}, {IX}, '{ClassName}'";

            byte[] imageBytes = db.Web_GetImageData(sq);


            if (imageBytes != null)
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (Image image = Image.FromStream(ms))
                    {
                        int cropWidth = image.Width;
                        int cropHeight = image.Height;

                        if (cropWidth > cropHeight)
                        {
                            float ratio = (float)cropWidth / cropHeight;
                            cropWidth = sScale;
                            cropHeight = (int)(cropWidth / ratio);
                        }
                        else
                        {
                            float ratio = (float)cropHeight / cropWidth;
                            cropHeight = sScale;
                            cropWidth = (int)(cropHeight / ratio);
                        }

                        using (Image resizedImage = new Bitmap(cropWidth, cropHeight))
                        {
                            using (Graphics graphics = Graphics.FromImage(resizedImage))
                            {
                                graphics.DrawImage(image, 0, 0, cropWidth, cropHeight);
                            }

                            using (MemoryStream outputMs = new MemoryStream())
                            {
                                resizedImage.Save(outputMs, ImageFormat.Jpeg);
                                byte[] resizedImageBytes = outputMs.ToArray();

                                string encodedImage = Convert.ToBase64String(resizedImageBytes);
                                response.StatusCode = "200";
                                response.Errormessage = "";

                                jsonDict.Add("response", JsonConvert.SerializeObject(response));
                                jsonDict.Add("Text", encodedImage);

                                return JsonConvert.SerializeObject(jsonDict);
                            }
                        }
                    }
                }
            }
            else
            {
                response.StatusCode = "200";
                response.Errormessage = "";

                jsonDict.Add("response", JsonConvert.SerializeObject(response));
                jsonDict.Add("Text", "no_photo");

                return JsonConvert.SerializeObject(jsonDict);
            }
        }

        

        [HttpGet]
        [Route("GetImageFromKsr")]
        public string GetImageFromKsr(string KsrImageCode)
        {


            int sScale = 500;

            string sq = $"Exec dbo.spApp_GetKsrImage {KsrImageCode}" ;

            byte[] imageBytes = db.Web_GetImageData(sq);


            if (imageBytes != null)
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (Image image = Image.FromStream(ms))
                    {
                        int cropWidth = image.Width;
                        int cropHeight = image.Height;

                        if (cropWidth > cropHeight)
                        {
                            float ratio = (float)cropWidth / cropHeight;
                            cropWidth = sScale;
                            cropHeight = (int)(cropWidth / ratio);
                        }
                        else
                        {
                            float ratio = (float)cropHeight / cropWidth;
                            cropHeight = sScale;
                            cropWidth = (int)(cropHeight / ratio);
                        }

                        using (Image resizedImage = new Bitmap(cropWidth, cropHeight))
                        {
                            using (Graphics graphics = Graphics.FromImage(resizedImage))
                            {
                                graphics.DrawImage(image, 0, 0, cropWidth, cropHeight);
                            }

                            using (MemoryStream outputMs = new MemoryStream())
                            {
                                resizedImage.Save(outputMs, ImageFormat.Jpeg);
                                byte[] resizedImageBytes = outputMs.ToArray();

                                string encodedImage = Convert.ToBase64String(resizedImageBytes);
                                response.StatusCode = "200";
                                response.Errormessage = "";

                                jsonDict.Add("response", JsonConvert.SerializeObject(response));
                                jsonDict.Add("Text", encodedImage);

                                return JsonConvert.SerializeObject(jsonDict);
                            }
                        }
                    }
                }
            }
            else
            {
                response.StatusCode = "200";
                response.Errormessage = "";

                jsonDict.Add("response", JsonConvert.SerializeObject(response));
                jsonDict.Add("Text", "no_photo");

                return JsonConvert.SerializeObject(jsonDict);
            }
        }






        [HttpGet]
        [Route("GetImageCustom")]
        public string GetImageCustom(string ClassName, string ObjectRef, string Scale)
        {


            int sScale = 500;

            string sq = $"set nocount on  select IMG from ksrimage where ClassName ='{ClassName}' and ObjectRef={ObjectRef} ";

            byte[] imageBytes = db.Web_GetImageData(sq);


            if (imageBytes != null)
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (Image image = Image.FromStream(ms))
                    {
                        int cropWidth = image.Width;
                        int cropHeight = image.Height;

                        if (cropWidth > cropHeight)
                        {
                            float ratio = (float)cropWidth / cropHeight;
                            cropWidth = sScale;
                            cropHeight = (int)(cropWidth / ratio);
                        }
                        else
                        {
                            float ratio = (float)cropHeight / cropWidth;
                            cropHeight = sScale;
                            cropWidth = (int)(cropHeight / ratio);
                        }

                        using (Image resizedImage = new Bitmap(cropWidth, cropHeight))
                        {
                            using (Graphics graphics = Graphics.FromImage(resizedImage))
                            {
                                graphics.DrawImage(image, 0, 0, cropWidth, cropHeight);
                            }

                            using (MemoryStream outputMs = new MemoryStream())
                            {
                                resizedImage.Save(outputMs, ImageFormat.Jpeg);
                                byte[] resizedImageBytes = outputMs.ToArray();

                                string encodedImage = Convert.ToBase64String(resizedImageBytes);

                                jsonDict.Add("Text", encodedImage);

                                return JsonConvert.SerializeObject(jsonDict);
                            }
                        }
                    }
                }
            }
            else
            {
                response.StatusCode = "200";
                response.Errormessage = "";

                jsonDict.Add("response", JsonConvert.SerializeObject(response));
                jsonDict.Add("Text", "no_photo");

                return JsonConvert.SerializeObject(jsonDict);
            }
        }































    }
}
