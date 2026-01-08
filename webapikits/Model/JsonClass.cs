using Newtonsoft.Json;
using System.Data;
using System.Drawing;
using System.Net.Mime;
using Image = System.Drawing.Image;

namespace webapikits.Model
{
    public class JsonClass
     {
        public string ConvertDataTableToJson(DataTable dataTable)
        {
            DataTable newDt = new DataTable();

            foreach (DataColumn col in dataTable.Columns)
            {
                newDt.Columns.Add(col.ColumnName, typeof(string));
            }

            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = newDt.NewRow();
                foreach (DataColumn col in dataTable.Columns)
                {
                    newRow[col.ColumnName] = row[col.ColumnName].ToString();
                }
                newDt.Rows.Add(newRow);
            }

            string json = JsonConvert.SerializeObject(newDt);

            return json;
        }
        public DataTable ConvertJsonToDataTable(string json)
        {
            DataTable dataTable = new DataTable();
            // Add columns to the DataTable based on the JSON data structure
            // For simplicity, let's assume all rows have the same structure
            if (!string.IsNullOrEmpty(json))
            {
                List<Dictionary<string, object>> rows = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                if (rows.Count > 0)
                {
                    foreach (string key in rows[0].Keys)
                    {
                        dataTable.Columns.Add(key, typeof(object)); // You might need to change 'typeof(object)' to match the actual data types
                    }
                    foreach (Dictionary<string, object> row in rows)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string key in row.Keys)
                        {
                            dataRow[key] = row[key];
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }
            }
            return dataTable;
        }

        public List<Dictionary<string, object>> ConvertJsonToDictionaryList(string json)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            DataTable dataTable = ConvertJsonToDataTable(json);

            foreach (DataRow row in dataTable.Rows)
            {
                Dictionary<string, object> currentRow = new Dictionary<string, object>();

                foreach (DataColumn col in dataTable.Columns)
                {
                    currentRow.Add(col.ColumnName, row[col]);
                }

                rows.Add(currentRow);
            }

            return rows;
        }

        public DataTable ConvertDictionaryListToDataTable(List<Dictionary<string, object>> dictionaryList)
        {
            DataTable dataTable = new DataTable();

            // Add columns to the DataTable based on the keys of the dictionaries
            if (dictionaryList.Count > 0)
            {
                foreach (string key in dictionaryList[0].Keys)
                {
                    dataTable.Columns.Add(key, typeof(object)); // You might need to change 'typeof(object)' to match the actual data types
                }

                // Add rows to the DataTable based on the values of the dictionaries
                foreach (Dictionary<string, object> dictionary in dictionaryList)
                {
                    DataRow dataRow = dataTable.NewRow();
                    foreach (string key in dictionary.Keys)
                    {
                        dataRow[key] = dictionary[key];
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }


        public string ConvertImageToBase64(DataTable dataTable)
        {
            if (dataTable.Rows.Count > 0)
            {
                byte[] imageData = (byte[])dataTable.Rows[0]["IMG"];
                string base64String = Convert.ToBase64String(imageData);

                Console.WriteLine(base64String); // Dar inja Console Log ra ejra kon

                // Daryaft-e ContentType
                string contentType = "image/jpeg"; // Moshakhas konande noe tasvir ra inja

                var responseObj = new
                {
                    Text = base64String,
                    ContentType = contentType
                };

                return JsonConvert.SerializeObject(responseObj);
            }
            else
            {
                string contentType = "image/jpeg";
                var responseObj = new
                {
                    Text = "Nophoto",
                    ContentType = contentType
                };
                return JsonConvert.SerializeObject(responseObj);
            }
        }



        /// <summary>
        /// تبدیل تصویر به Base64 با قابلیت Resize
        /// </summary>
        public static string ConvertAndScaleImageAttachedToBase64(int? targetSize, DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return BuildResponse("Nophoto", "image/jpeg","NoName");
            }

            byte[] imageData = (byte[])dataTable.Rows[0]["SourceFile"];
           string? fileName = (string?) dataTable.Rows[0]["FileName"];


            string base64String;

            using (MemoryStream ms = new MemoryStream(imageData))
            {
                using (Image image = Image.FromStream(ms))
                {
                    // اگر تصویر بزرگتر از targetSize بود Resize می‌کنیم
                    if (targetSize.HasValue && Math.Max(image.Width, image.Height) > targetSize.Value)
                    {
                        double scaleFactor = (double)targetSize.Value / Math.Max(image.Width, image.Height);

                        int newWidth = (int)(image.Width * scaleFactor);
                        int newHeight = (int)(image.Height * scaleFactor);

                        using (Image scaledImage = new Bitmap(newWidth, newHeight))
                        {
                            using (Graphics g = Graphics.FromImage(scaledImage))
                            {
                                g.DrawImage(image, 0, 0, newWidth, newHeight);
                            }

                            using (MemoryStream scaledMs = new MemoryStream())
                            {
                                scaledImage.Save(scaledMs, System.Drawing.Imaging.ImageFormat.Jpeg);
                                base64String = Convert.ToBase64String(scaledMs.ToArray());
                            }
                        }
                    }
                    else
                    {
                        // بدون Resize
                        base64String = Convert.ToBase64String(imageData);
                    }
                }
            }

            return BuildResponse(base64String, "image/jpeg",fileName);
        }


        /// <summary>
        /// خروجی استاندارد JSON
        /// </summary>
        private static string BuildResponse(string base64, string contentType, string fileName)
        {
            var responseObj = new
            {
                Text = base64,
                ContentType = contentType,
                FileName = fileName
            };

            return JsonConvert.SerializeObject(responseObj);
        }





        public string ConvertVoiceToBase64(DataTable dataTable)
        {
            if (dataTable.Rows.Count > 0)
            {
                byte[] voiceData = (byte[])dataTable.Rows[0]["SourceFile"]; // ستون صدا رو اینجا بذار

                string base64String = Convert.ToBase64String(voiceData);

                // ContentType رو بر اساس فرمت ذخیره‌شده تنظیم کن (اینجا مثلا mp3)
                string contentType = "audio/mpeg";

                var responseObj = new
                {
                    Text = base64String,
                    ContentType = contentType
                };

                return JsonConvert.SerializeObject(responseObj);
            }
            else
            {
                string contentType = "audio/mpeg";
                var responseObj = new
                {
                    Text = "Novoice",
                    ContentType = contentType
                };

                return JsonConvert.SerializeObject(responseObj);
            }
        }






        public string ConvertAndScaleImageAttachedToBase64(int targetSize, DataTable dataTable)
        {

            if (dataTable.Rows.Count > 0)
            {
                byte[] imageData = (byte[])dataTable.Rows[0]["SourceFile"];
                string base64String;

                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image image = Image.FromStream(ms);

                    // Check image size and scale if necessary
                    if (Math.Max(image.Width, image.Height) > targetSize)
                    {
                        double scaleFactor = (double)targetSize / Math.Max(image.Width, image.Height);

                        int newWidth = (int)(image.Width * scaleFactor);
                        int newHeight = (int)(image.Height * scaleFactor);

                        Image scaledImage = new Bitmap(newWidth, newHeight);
                        using (Graphics g = Graphics.FromImage(scaledImage))
                        {
                            g.DrawImage(image, 0, 0, newWidth, newHeight);
                        }

                        using (MemoryStream scaledMs = new MemoryStream())
                        {
                            scaledImage.Save(scaledMs, System.Drawing.Imaging.ImageFormat.Jpeg);
                            base64String = Convert.ToBase64String(scaledMs.ToArray());
                        }
                    }
                    else
                    {
                        base64String = Convert.ToBase64String(imageData);
                    }
                }

                // Return the Base64 string and ContentType
                string contentType = "image/jpeg";
                var responseObj = new
                {
                    Text = base64String,
                    ContentType = contentType
                };

                return JsonConvert.SerializeObject(responseObj);
            }
            else
            {
                // Return placeholder for no photo
                string contentType = "image/jpeg";
                var responseObj = new
                {
                    Text = "Nophoto",
                    ContentType = contentType
                };
                return JsonConvert.SerializeObject(responseObj);
            }
        }





        public string ConvertAndScaleImageToBase64(int targetSize, DataTable dataTable)
    {

        if (dataTable.Rows.Count > 0)
        {
            byte[] imageData = (byte[])dataTable.Rows[0]["IMG"];
            string base64String;

            using (MemoryStream ms = new MemoryStream(imageData))
            {
                    Image image = Image.FromStream(ms);

                // Check image size and scale if necessary
                if (Math.Max(image.Width, image.Height) > targetSize)
                {
                    double scaleFactor = (double)targetSize / Math.Max(image.Width, image.Height);

                    int newWidth = (int)(image.Width * scaleFactor);
                    int newHeight = (int)(image.Height * scaleFactor);

                    Image scaledImage = new Bitmap(newWidth, newHeight);
                    using (Graphics g = Graphics.FromImage(scaledImage))
                    {
                        g.DrawImage(image, 0, 0, newWidth, newHeight);
                    }

                    using (MemoryStream scaledMs = new MemoryStream())
                    {
                        scaledImage.Save(scaledMs, System.Drawing.Imaging.ImageFormat.Jpeg);
                        base64String = Convert.ToBase64String(scaledMs.ToArray());
                    }
                }
                else
                {
                    base64String = Convert.ToBase64String(imageData);
                }
            }

            // Return the Base64 string and ContentType
            string contentType = "image/jpeg";
            var responseObj = new
            {
                Text = base64String,
                ContentType = contentType
            };

            return JsonConvert.SerializeObject(responseObj);
        }
        else
        {
            // Return placeholder for no photo
            string contentType = "image/jpeg";
            var responseObj = new
            {
                Text = "Nophoto",
                ContentType = contentType
            };
            return JsonConvert.SerializeObject(responseObj);
        }
    }





    public string JsonResult_Str1(DataTable dataTable, String keyresponse, string textValue)
        {
            Response response = new();
            JsonClass jsonClass = new JsonClass();
            Dictionary<string, string> jsonDict = new Dictionary<string, string>();

            if (dataTable.Rows.Count > 0)
            {

                if (textValue.Length > 0)
                {
                    jsonDict.Add(keyresponse, Convert.ToString(dataTable.Rows[0][textValue]));
                }
                else
                {
                    jsonDict.Add(keyresponse, jsonClass.ConvertDataTableToJson(dataTable));
                }

                return JsonConvert.SerializeObject(jsonDict);
            }
            else
            {

                response.StatusCode = "1000";
                response.Errormessage = "No Data Found";
                jsonDict.Add("response", JsonConvert.SerializeObject(response));
                return JsonConvert.SerializeObject(jsonDict);


            }



        }



        public string JsonResult_Str(DataTable dataTable, string keyResponse, string textValue)
        {
            Response response = new Response();
            JsonClass jsonClass = new JsonClass();

            if (dataTable.Rows.Count > 0)
            {
                response.StatusCode = "200";
                response.Errormessage = "";

                string json = "{";
                json += "\"response\":{";
                json += "\"StatusCode\":\"" + response.StatusCode + "\",";
                json += "\"Errormessage\":\"" + response.Errormessage + "\"";
                json += "},";

                json += "\"" + keyResponse + "\":";

                if (!string.IsNullOrEmpty(textValue))
                {
                    json += "\"" + Convert.ToString(dataTable.Rows[0][textValue]) + "\"";
                }
                else
                {
                    json += jsonClass.ConvertDataTableToJson(dataTable);
                }

                json += "}";

                return json;
            }
            else
            {
                response.StatusCode = "1000";
                response.Errormessage = "No Data Found";

                // Construct the JSON string for the error case
                string json = "{\"response\":{\"StatusCode\":\"" + response.StatusCode + "\",\"Errormessage\":\"" + response.Errormessage + "\"},";

                // Check if "Done" condition is met
                if (textValue == "Done" && keyResponse == "Text")
                {
                    json += "\"Text\":\"Done\"";
                }
                else
                {
                    // Return empty array if no data found
                    json += "\"" + keyResponse + "\":[]";
                }

                json += "}";
                return json;

            }
        }

        public string JsonResult_StrRepInfo(DataTable dataTable, string keyResponse, string textValue)
        {
            Response response = new Response();
            JsonClass jsonClass = new JsonClass();

            if (dataTable.Rows.Count > 0)
            {
                response.StatusCode = "2000";
                response.Errormessage = "";
                /*
                // Construct the custom JSON string
                string json = "{" +
                    "\"response\":{\"StatusCode\":\"" + response.StatusCode + "\",\"Errormessage\":\"" + response.Errormessage + "\"}," +
                    "\"" + keyResponse + "\":";
                */
                string json = "{\"" + keyResponse + "\":";
                if (textValue.Length > 0)
                {
                    json +=  Convert.ToString(dataTable.Rows[0][textValue]);
                }
                else
                {
                    json +=  jsonClass.ConvertDataTableToJson(dataTable) ;
                }

                json += "}";

                return json;
            }
            else
            {
                response.StatusCode = "1000";
                response.Errormessage = "No Data Found";

                // Construct the custom JSON string for the error case
                string json = "{\"response\":{\"StatusCode\":\"" + response.StatusCode + "\",\"Errormessage\":\"" + response.Errormessage + "\"}}";

                return json;
            }
        }






        public string JsonResultWithout_Str(DataTable dataTable)
        {
            Response response = new();
            JsonClass jsonClass = new JsonClass();
            Dictionary<string, string> jsonDict = new Dictionary<string, string>();


                return jsonClass.ConvertDataTableToJson(dataTable);
            
           



        }

        /// <summary>
        /// تبدیل صدا به Base64 (MP3/WAV و …)
        /// </summary>
        public string ConvertVoiceToBase641(DataTable dataTable)
        {
            return ConvertFileToBase64(dataTable, "audio/mpeg", "Novoice");
        }



        public string ConvertFileToBase64(DataTable dataTable, string contentType, string emptyText)
        {
            string? fileName = (string?)dataTable.Rows[0]["FileName"];
            byte[] fileData = (byte[])dataTable.Rows[0]["SourceFile"];


            if (dataTable.Rows.Count > 0)
            {

                string base64String = Convert.ToBase64String(fileData);
                return BuildResponse(base64String, contentType, fileName);
            }
            else
            {
                return BuildResponse(emptyText, contentType, fileName);
            }
        }

    }



}
