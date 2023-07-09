using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using webapikits.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        public readonly IConfiguration _configuration;
        DataBaseClass db = new DataBaseClass();
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();







        [HttpGet]
        [Route("VersionInfo")]
        public string VersionInfo()
        {
            response.StatusCode = "200";
            response.Errormessage = "";

            jsonDict.Add("response", JsonConvert.SerializeObject(response));
            jsonDict.Add("Text", "2.0");
            return JsonConvert.SerializeObject(jsonDict);
        }


        [HttpGet]
        [Route("check_server")]
        public string check_server()
        {
            response.StatusCode = "200";
            response.Errormessage = "";

            jsonDict.Add("response", JsonConvert.SerializeObject(response));
            jsonDict.Add("Text", "false");
            return JsonConvert.SerializeObject(jsonDict);
        }



        [HttpGet]
        [Route("BasketGet")]
        public string BasketGet(string Mobile)
        {

            string query = "Exec [dbo].[spApp_BasketGet] '"+ Mobile + "'";


            DataTable dataTable = db.ExecQuery(query, _configuration);


            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }



        [HttpGet]
        [Route("BasketHistory")]
        public string BasketHistory(string Mobile,string Code, string ReservedRows)
        {

            string query = "Exec [dbo].[spApp_BasketPreFactors] '"+ Mobile + "',"+ Code + ","+ ReservedRows ;


            DataTable dataTable = db.ExecQuery(query, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "PreFactors", "");


        }


        [HttpGet]
        [Route("BasketToPreFactor")]
        public string BasketToPreFactor(string Mobile, string Explain)
        {

            string query = "Exec [dbo].[spApp_BasketToPreFactor] '"+ Mobile + "', -2000 , '"+Explain+"'";


            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");


        }


        [HttpGet]
        [Route("BasketSum")]
        public string BasketSum(string Mobile)
        {

            string query = "Exec dbo.spApp_BasketSummary '"+ Mobile + "'";


            DataTable dataTable = db.ExecQuery(query, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");


        }


        [HttpGet]
        [Route("Basketdeleteall")]
        public string Basketdeleteall(string Mobile)
        {

            string query = "  set nocount on Update AppBasket set ProcessStatus = 10 where MobileNo = '"+ Mobile + "' and ProcessStatus = 0 select 1";



            DataTable dataTable = db.ExecQuery(query, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "Text", "done");


        }








        [HttpGet]
        [Route("deletebasket")]
        public string deletebasket(
            string DeviceCode,
            string GoodRef,
            string UserId,
            string Mobile
            )
        {

            string query = "Exec [dbo].[spApp_BasketDelete] '"+ DeviceCode + "', "+GoodRef + ", "+UserId + " , '"+ Mobile + "'";



            DataTable dataTable = db.ExecQuery(query, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "Text", "done");


        }





        [HttpGet]
        [Route("Favorite_action")]
        public string Favorite_action(
            string Mobile,
            string GoodRef,
            string DeleteFlag
            )
        {

            string query = "Exec [dbo].[spApp_FavoriteInsert] '"+ Mobile + "',"+ GoodRef + ","+DeleteFlag;



            DataTable dataTable = db.ExecQuery(query, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "Text", "Result");


        }


        [HttpGet]
        [Route("GoodGroupInfo_Default")]
        public string GoodGroupInfo_Default()
        {

            string query = "Exec [dbo].[spApp_GetGoodGroups_Default] ";


            DataTable dataTable = db.ExecQuery(query, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "Groups", "");


        }



        [HttpGet]
        [Route("GoodGroupInfo_DefaultImage")]
        public string GoodGroupInfo_DefaultImage()
        {

            string query = "Exec [dbo].[spApp_GetGoodGroups_DefaultImage]  ";


            DataTable dataTable = db.ExecQuery(query, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "Groups", "");


        }






        [HttpGet]
        [Route("goodinfo")]
        public string goodinfo(
            string RowCount,
            string SearchTarget,
            string OrderBy,
            string OnlyActive,
            string OnlyAvailable,
            string GroupCode,
            string Where,
            string LikeGood,
            string PageNo,
            string MobileNo,
            string OnlyFavorite,
            string GoodCode
            )
        {

            string query = "exec spApp_GetGoods2 ";

            if (!string.IsNullOrEmpty(RowCount))
            {
                query += "@RowCount = " + RowCount + " ";
            }else { 
                query += "@RowCount = 10"; 
            }

            if (!string.IsNullOrEmpty(SearchTarget))
            {
                query += "@SearchTarget =  N'" + SearchTarget + "' ";
            }

            if (!string.IsNullOrEmpty(OrderBy))
            {
                query += "@OrderBy = N'" + OrderBy + "' ";
            }

            if (!string.IsNullOrEmpty(OnlyActive))
            {
                query += "@OnlyActive = " + OnlyActive + " ";
            }

            if (!string.IsNullOrEmpty(OnlyAvailable))
            {
                query += "@OnlyAvailable = " + OnlyAvailable + " ";
            }

            if (!string.IsNullOrEmpty(GroupCode))
            {
                query += "@GroupCode = " + GroupCode + " ";
            }

            if (!string.IsNullOrEmpty(Where))
            {
                query += "@Where = N'" + Where + "' ";
            }

            if (!string.IsNullOrEmpty(LikeGood))
            {
                query += "@LikeGoodRef = " + LikeGood + " ";
            }

            if (!string.IsNullOrEmpty(PageNo))
            {
                query += "@PageNo = " + PageNo + " ";
            }

            if (!string.IsNullOrEmpty(MobileNo))
            {
                query += "@MobileNo = '" + MobileNo + "' ";
            }

            if (!string.IsNullOrEmpty(OnlyFavorite))
            {
                query += "@OnlyFavorite = " + OnlyFavorite + " ";
            }

            if (!string.IsNullOrEmpty(GoodCode))
            {
                query += "@GoodCode = " + GoodCode + " ";
            }


            DataTable dataTable = db.ExecQuery(query, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");


        }





        [HttpGet]
        [Route("GoodGroupInfo")]
        public string GoodGroupInfo(string GroupName,
            string GroupCode
            )
        {

            string query = "Exec [dbo].[spApp_GetGoodGroups] ";

            if (!string.IsNullOrEmpty(GroupName))
            {
                query += "@GroupName = N'" + GroupName + "' ";
            }

            if (!string.IsNullOrEmpty(GroupCode))
            {
                query += "@GroupCode = " + GroupCode + " ";
            }


            DataTable dataTable = db.ExecQuery(query, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "Groups", "");


        }





        [HttpGet]
        [Route("XUserCreate")]
        public string XUserCreate(
            string FName,
            string LName,
            string UName,
            string UPass,
            string address,
            string mobile,
            string company,
            string email,
            string Flag,
            string NewPass,
            string PostalCode
            )
        {



            string query = "Exec [dbo].[spApp_XUserCreate] '"+UName + "','"+ UPass + "','"+ NewPass+"','"+ FName + "','"+ LName + "','"+ mobile + "','"+ company + "','"+address + "','"+PostalCode + "','"+ email + "',-2000,"+ Flag  ;


            DataTable dataTable = db.ExecQuery(query, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "users", "");


        }




        [HttpPost]
        [Route("InsertBasket")]
        public string InsertBasket([FromBody] JObject request)
        {
            int XUser = 0;
            string DeviceCode = "";
            int GoodRef = 0;
            int FacAmount = 0;
            int Price = 0;
            string Explain = "";
            string Ratio = "1";
            string UnitRef = "1";
            string Source = "";
            int UserId = -2000;
            string Mobile = "";

            if (request.TryGetValue("DeviceCode", out JToken deviceCodeToken))
            {
                DeviceCode = deviceCodeToken.Value<string>();
            }

            if (request.TryGetValue("GoodRef", out JToken goodRefToken))
            {
                GoodRef = goodRefToken.Value<int>();
            }

            if (request.TryGetValue("FacAmount", out JToken facAmountToken))
            {
                FacAmount = facAmountToken.Value<int>();
            }

            if (request.TryGetValue("Price", out JToken priceToken))
            {
                Price = priceToken.Value<int>();
            }

            if (request.TryGetValue("UnitRef", out JToken unitRefToken))
            {
                UnitRef = unitRefToken.Value<string>();
            }

            if (request.TryGetValue("Ratio", out JToken ratioToken))
            {
                Ratio = ratioToken.Value<string>();
            }

            if (request.TryGetValue("Explain", out JToken explainToken))
            {
                Explain = explainToken.Value<string>();
            }

            if (request.TryGetValue("Source", out JToken sourceToken))
            {
                Source = sourceToken.Value<string>();
            }

            if (request.TryGetValue("UserId", out JToken userIdToken))
            {
                UserId = userIdToken.Value<int>();
            }

            if (request.TryGetValue("Mobile", out JToken mobileToken))
            {
                Mobile = mobileToken.Value<string>();
            }

            string sq = $"Exec [dbo].[spApp_BasketInsert] '{DeviceCode}', {GoodRef}, {FacAmount}, {Price}, '{UnitRef}', '{Ratio}', '{Explain}', '{Source}', {UserId}, '{Mobile}'";

            DataTable dataTable = db.ExecQuery(sq, _configuration);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");


        }

        /*

        public function PFRCDEWS()
        { 		//spApp_InsertReceive
		$UserId = -2000;
            if (isset($_REQUEST['Mobile'])) { $Mobile = $_REQUEST['Mobile']; } else {$Mobile = ""; };
            if (isset($_REQUEST['Price'])) { $Price = $_REQUEST['Price']; } else {$Price = "0"; };
            if (isset($_REQUEST['Rahgiri'])) { $Rahgiri = $_REQUEST['Rahgiri']; } else {$Rahgiri = ""; };
            if (isset($_REQUEST['BankKart'])) { $BankKart = $_REQUEST['BankKart']; } else {$BankKart = ""; };
            if (isset($_REQUEST['KartOwner'])) { $KartOwner = $_REQUEST['KartOwner']; } else {$KartOwner = ""; };
            if (isset($_REQUEST['PreFactorCode'])) { $PreFactorCode = $_REQUEST['PreFactorCode']; } else {$PreFactorCode = "0"; };
		
		$sq = "Exec [dbo].[spApp_InsertReceive] '$Mobile',$Price,'$Rahgiri','$BankKart','$KartOwner',$PreFactorCode,$UserId";

            MainClass::LogFile("dargah_InsertReceive",$sq);
		$this->response = database::custom_sqlSRV($sq, true);
		$Last = json_encode($this->response, JSON_UNESCAPED_UNICODE);
            echo "{\"users\":".$Last."}";

        }
        */

        [HttpPost]
        [Route("PFRCDEWS")]
        public IActionResult PFRCDEWS([FromBody] JObject request)
        {
            int UserId = -2000;
            string Mobile = "";
            int Price = 0;
            string Rahgiri = "";
            string BankKart = "";
            string KartOwner = "";
            int PreFactorCode = 0;

            if (request.TryGetValue("Mobile", out JToken mobileToken))
            {
                Mobile = mobileToken.Value<string>();
            }

            if (request.TryGetValue("Price", out JToken priceToken))
            {
                Price = priceToken.Value<int>();
            }

            if (request.TryGetValue("Rahgiri", out JToken rahgiriToken))
            {
                Rahgiri = rahgiriToken.Value<string>();
            }

            if (request.TryGetValue("BankKart", out JToken bankKartToken))
            {
                BankKart = bankKartToken.Value<string>();
            }

            if (request.TryGetValue("KartOwner", out JToken kartOwnerToken))
            {
                KartOwner = kartOwnerToken.Value<string>();
            }

            if (request.TryGetValue("PreFactorCode", out JToken preFactorCodeToken))
            {
                PreFactorCode = preFactorCodeToken.Value<int>();
            }

            string sq = $"Exec [dbo].[spApp_InsertReceive] '{Mobile}', {Price}, '{Rahgiri}', '{BankKart}', '{KartOwner}', {PreFactorCode}, {UserId}";

           // var response = database.custom_sqlSRV(sq, true);
            string last = JsonConvert.SerializeObject(response, Formatting.None);
            return Content("{\"users\":" + last + "}", "application/json");
        }












        /*
         
         	public function Banner(){
		$directoryToScan = "\xampp\htdocs\login\SlideImage";

		define('WEBSITE', "/login");
	
		$array = array();
		$num_files = count(glob("/xampp/htdocs/login/SlideImage/*.jpg"));
		$totalFiles = (string)$num_files;
		foreach (glob("/xampp/htdocs/login/SlideImage/*.jpg") as $filename) {
			$object_url = str_replace("/xampp/htdocs/login", '', $filename) ;
			$object_name = str_replace("/xampp/htdocs/login/SlideImage/", '', $filename) ;
			$object_name = str_replace(".jpg", '', $object_name) ;			
			$turl = WEBSITE.$object_url;
			$url = str_replace("\/", "\\", $turl);
			if(substr($object_name,0,5)!="image"){
					array_push($array, array('GoodName'=>$object_name,'GoodImageUrl'=>$url));
			}
		}
		$Last =  json_encode($array, JSON_UNESCAPED_UNICODE);
		echo "{\"Goods\":".$Last."}";
	}			
	

         
         
         
         */



    }
}
