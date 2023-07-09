using System.Data;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using webapikits.Model;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitsController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        DataBaseClass db = new DataBaseClass();
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();






        [HttpGet]
        [Route("kowsar_info")]
        public string kowsar_info(string Where)
        {

            string query = "select top 1 DataValue from dbsetup where KeyValue = '"+ Where+"'";



            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");


        }
        


        [HttpGet]
        [Route("ActivationCode")]
        public string ActivationCode(string ActivationCode)
        {

            string query = "select * from AppBrokerCustomer Where ActivationCode = '"+ ActivationCode + "'";



            DataTable dataTable = db.ExecQuery(query, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "Activations", "");

        }



        
        [HttpGet]
        [Route("ErrorLog")]
        public string ErrorLog(
            string ErrorLog,
            string Broker,
            string DeviceId,
            string ServerName,
            string VersionName,
            string StrDate

            )
        {

            string query = " Insert into ErrorLogReport([ErrorLogText], [Broker], [DeviceId], [ServerName], [VersionName], [StrDate])values ('"+ ErrorLog + "','"+Broker + "','"+ DeviceId + "','"+ ServerName + "','"+ VersionName + "','"+ StrDate + "')";



            DataTable dataTable = db.ExecQuery(query, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "Text", "done");

        }



        
        
        [HttpGet]
        [Route("Log_report")]
        public string Log_report(
            string Device_Id,
            string Address_Ip,
            string Server_Name,
            string Factor_Code,
            string StrDate,
            string Broker,
            string Explain
            )
        {

            string query = "exec spApp_appinfo '"+ Device_Id + "','"+ Address_Ip + "','"+ Server_Name + "','"+ Factor_Code + "','"+ StrDate + "','"+ Broker + "','"+ Explain + "'";



            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "done");


        }
        /*
        //TODO

        public string Verification(string Code, string MobileNumber)
        {


            VerificationCode1(Code, MobileNumber);
            jsonDict.Add("response", JsonConvert.SerializeObject(response));
            jsonDict.Add("Text", "done");
            // jsonDict.Add("Activations", jsonClass.ConvertDataTableToJson(dataTable));

            return JsonConvert.SerializeObject(jsonDict);
        }

        private string VerificationCode1(string Code, string MobileNumber)
        {
            string token = GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                var postData = new
                {
                    Code = Code,
                    MobileNumber = MobileNumber
                };

                string url = GetAPIVerificationCodeUrl();
                string verificationCode = Execute(postData, url, token);
                dynamic obj = JsonConvert.DeserializeObject(verificationCode);

                if (obj != null && obj.GetType() == typeof(JObject))
                {
                    Dictionary<string, object> dict = obj.ToObject<Dictionary<string, object>>();

                    if (dict != null && dict.ContainsKey("Message"))
                    {
                        return dict["Message"].ToString();
                    }
                }
            }

            return null;
        }

        private string GetToken()
        {
            var postData = new
            {
                UserApiKey = "1e605332b72c1590d5c57b3",
                SecretKey = "Kowsar321@!"
            };

            string postString = JsonConvert.SerializeObject(postData);

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string result = client.UploadString(GetApiTokenUrl(), postString);

                dynamic response = JsonConvert.DeserializeObject(result);

                if (response != null && response.GetType() == typeof(JObject))
                {
                    Dictionary<string, object> dict = response.ToObject<Dictionary<string, object>>();

                    if (dict != null && dict.ContainsKey("IsSuccessful") && (bool)dict["IsSuccessful"])
                    {
                        return dict.ContainsKey("TokenKey") ? dict["TokenKey"].ToString() : null;
                    }
                }
            }

            return null;
        }

        protected string GetAPIVerificationCodeUrl()
        {
            return "http://RestfulSms.com/api/VerificationCode";
        }

        protected string GetApiTokenUrl()
        {
            return "http://RestfulSms.com/api/Token";
        }

        private string Execute(object postData, string url, string token)
        {
            string postString = JsonConvert.SerializeObject(postData);

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers["x-sms-ir-secure-token"] = token;
                string result = client.UploadString(url, postString);
                return result;
            }
        }


        */











    }
}
