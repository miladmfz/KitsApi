using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapikits.Model;
using Image = System.Drawing.Image;
using QuickReport;


namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KowsarController : ControllerBase
    {

        public readonly IConfiguration _configuration;
        DataBaseClass db = new DataBaseClass();
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();




        [HttpGet]
        [Route("Check")]
        public string Check()
        {


            PrintDocument pd = new PrintDocument();
            pd.PrintPage += (sender, e) =>
            {
                // Configure print settings
                PrinterSettings settings = new PrinterSettings();
                settings.PrinterName = "Name of the printer";
                e.Graphics.DrawString("1231231\n1231231\n1231231\n", new Font("Arial", 12), Brushes.Black, 10, 10);
            };


            pd.Print();

            string query = "select top 2 GoodCode,GoodName,GoodExplain1 from good ";



            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");


        }





        [HttpGet]
        [Route("kowsarVersion")]
        public string kowsarVersion()
        {

            string query = "Exec spversioninfo";



            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "VerNo");


        }







        [HttpGet]
        [Route("GetGoodType")]
        public string GetGoodType()
        {

            string query = "Exec [spApp_GetGoodType]";



            DataTable dataTable = db.ExecQuery(query, _configuration);

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

            string query;
            if (AppType == "1") {
                query = "Exec [spApp_GetColumn] "+GoodCode + " ,'', "+ Type + ","+ AppType + ","+ IncludeZero;
            } else
            {
                query = "Exec [spApp_GetColumn] "+GoodCode + " ,'"+ GoodType + "', "+ Type + ","+ AppType + "," + IncludeZero;
            }



            DataTable dataTable = db.ExecQuery(query, _configuration);
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

            string query= "Exec spAppGetDistinctValues '"+ TableName + "','"+ FieldNames + " Value','"+ WhereClause + "' ";





            DataTable dataTable = db.ExecQuery(query, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "Values", "");

        }




        
                
        [HttpGet]
        [Route("GetSellBroker")]
        public string GetSellBroker()
        {

            string query= "select brokerCode,BrokerNameWithoutType from vwSellBroker";


            DataTable dataTable = db.ExecQuery(query, _configuration);

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

            byte[] imageBytes = GetImageData(sq,_configuration);


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
                                //jsonDict.Add("SellBrokers", jsonClass.ConvertDataTableToJson(dataTable));

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
                //jsonDict.Add("SellBrokers", jsonClass.ConvertDataTableToJson(dataTable));

                return JsonConvert.SerializeObject(jsonDict);
            }
        }

        

        [HttpGet]
        [Route("GetImageFromKsr")]
        public string GetImageFromKsr(string KsrImageCode)
        {


            int sScale = 500;

            string sq = "Exec dbo.spApp_GetKsrImage "+ KsrImageCode;

            byte[] imageBytes = GetImageData(sq,_configuration);


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
                                //jsonDict.Add("SellBrokers", jsonClass.ConvertDataTableToJson(dataTable));

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
                //jsonDict.Add("SellBrokers", jsonClass.ConvertDataTableToJson(dataTable));

                return JsonConvert.SerializeObject(jsonDict);
            }
        }









        public byte[] GetImageData(String query, IConfiguration _configuration)
        {
            byte[] imageData = null;

            DataTable dataTable = db.ImageExecQuery(query, _configuration);
            if (DataTable.Rows.Count > 0)
            {
                if (!Convert.IsDBNull(DataTable.Rows[0][0]))
                {
                    imageData = (byte[])DataTable.Rows[0]["IMG"];
                }
            }

            return imageData;
        }
























    }
}
