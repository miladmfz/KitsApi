using Newtonsoft.Json;
using System.Data;

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

                // Construct the custom JSON string for the error case
                string json = "{\"response\":{\"StatusCode\":\"" + response.StatusCode + "\",\"Errormessage\":\"" + response.Errormessage + "\"}}";

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





    }


   
}
