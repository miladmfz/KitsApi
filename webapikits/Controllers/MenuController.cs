using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using webapikits.Model;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {




        public readonly IConfiguration _configuration;
        DataBaseClass db = new DataBaseClass();
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        public MenuController(IConfiguration configuration)
        {
            _configuration = configuration;

        }






        [HttpGet]
        [Route("GetOrderGoodList")]
        public string GetOrderGoodList(
            string GroupCode,
            string RowCount,
            string Where,
            string AppBasketInfoRef
            )
        {

            //string query = "Exec spApp_GetGoods2 @RowCount = $RowCount,@Where = N'$Where',@AppBasketInfoRef=$AppBasketInfoRef, @GroupCode = $GroupCode ,@AppType=3 , @OrderBy = ' order by PrivateCodeForSort ' ";
            string query = $"Exec spApp_GetGoods2 @RowCount = {RowCount}, @Where = N'{Where}', @AppBasketInfoRef = {AppBasketInfoRef}, @GroupCode = {GroupCode}, @AppType = 3, @OrderBy = ' order by PrivateCodeForSort '";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetOrderSum")]
        public string GetOrderSum(string AppBasketInfoRef)
        {

            string query = "Exec spApp_OrderGetSummmary " + AppBasketInfoRef;

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }




        [HttpGet]
        [Route("OrderRowInsert")]
        public string OrderRowInsert(
            string GoodRef,
            string FacAmount,
            string Price,
            string bUnitRef,
            string bRatio,
            string Explain,
            string UserId,
            string InfoRef,
            string RowCode
            )
        {

            string query = $"[dbo].[spApp_OrderRowInsert] {GoodRef}, {FacAmount}, {Price}, {bUnitRef}, {bRatio}, '{Explain}', {UserId}, {InfoRef}, {RowCode}";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }





        [HttpGet]
        [Route("OrderGet")]
        public string OrderGet(string AppBasketInfoRef,string AppType)
        {

            string query = $"Exec [dbo].[spApp_OrderGet] {AppBasketInfoRef} , {AppType} ";

            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("kowsar_info")]
        public string kowsar_info(string Where)
        {

            string query = "select top 1 DataValue from dbsetup where KeyValue = '" + Where + "'";



            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");


        }



        [HttpGet]
        [Route("GetOrdergroupList")]
        public string GetOrdergroupList(string Where)
        {
            /*
             	
		$sq = "Exec [dbo].[spApp_GetGoodGroups] ";
		if (isset($_REQUEST['GroupName'])){ $sq = $sq."@GroupName = N'".$_REQUEST['GroupName']."'";} else {$sq = $sq."@GroupName = N''";}
		if (isset($_REQUEST['GroupCode'])){ $sq = $sq.", @GroupCode = ".$_REQUEST['GroupCode'];}


		MainClass::LogFile("GoodGroupInfo",$sq);
		$this->response = database::custom_sqlSRV($sq,true);		
		$Last =  json_encode($this->response, JSON_UNESCAPED_UNICODE);	
		MainClass::handlePreflightRequest();
		echo "{\"Groups\":".$Last."}";
	   
             
             */

            string query = "select top 1 DataValue from dbsetup where KeyValue = '" + Where + "'";



            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");


        }




        [HttpGet]
        [Route("WebOrderMizData")]
        public string WebOrderMizData(string RstMizCode)
        {
            /*
             	
		if (isset($_REQUEST['RstMizCode']))	{ $RstMizCode = $_REQUEST['RstMizCode'];}else {$RstMizCode = "0";};

		
		$Res = array();
	
		$sq = "exec spApp_OrderMizData  $RstMizCode ";

		MainClass::LogFile("OrderMizData",$sq);
		$this->response = database::custom_sqlSRV($sq,true);		
		$Last =  json_encode($this->response, JSON_UNESCAPED_UNICODE);	
		MainClass::handlePreflightRequest();	
		echo $Last;
	   
             
             */

            string query = "select top 1 DataValue from dbsetup where KeyValue = '" + Where + "'";



            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");


        }
        
        [HttpGet]
        [Route("WebOrderInfoInsert")]
        public string WebOrderInfoInsert(string Miz, string Date)
        {
            /*
             	
		if (isset($_REQUEST['Miz']))	{ $Miz = $_REQUEST['Miz'];}else {$Miz = "0";};
		if (isset($_REQUEST['Date']))	{ $Date = $_REQUEST['Date'];}else {$Date = "1401/01/01";};
		
		$Res = array();
	
		$sq = "exec spApp_OrderInfoInsert 0,$Miz,'','','',0,'','','$Date',1,0";
		MainClass::LogFile("Order_OrderInfoInsert",$sq);
	
		$this->response = database::custom_sqlSRV($sq,true);		
		$Last =  json_encode($this->response, JSON_UNESCAPED_UNICODE);		
		MainClass::handlePreflightRequest();
		echo $Last;
	   
             
             */

            string query = "select top 1 DataValue from dbsetup where KeyValue = '" + Where + "'";



            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");


        }

        [HttpGet]
        [Route("DeleteGoodFromBasket")]
        public string DeleteGoodFromBasket(string RowCode, string AppBasketInfoRef)
        {
            /*
             	
		if (isset($_REQUEST['RowCode']))	{ $RowCode = $_REQUEST['RowCode'];}			else {$RowCode = 0;};
		if (isset($_REQUEST['AppBasketInfoRef']))	{ $AppBasketInfoRef = $_REQUEST['AppBasketInfoRef'];} 			else {$AppBasketInfoRef = 0;};

		$sq ="Delete From AppBasket Where AppBasketInfoRef=$AppBasketInfoRef and AppBasketCode=$RowCode ";

		MainClass::LogFile("Order_DeleteGoodFromBasket",$sq);
		$this->response = database::custom_sqlSRV($sq,true);
		$Last =  json_encode($this->response, JSON_UNESCAPED_UNICODE);
		MainClass::handlePreflightRequest();
		echo "{\"Text\":\"Done\"}";
	
	   
             
             */

            string query = "select top 1 DataValue from dbsetup where KeyValue = '" + Where + "'";



            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");


        }



        [HttpGet]
        [Route("getImage")]
        public string getImage(string Miz, string Date)
        {
            /*
             	
	public function getImage(){
		$ObjectRef = $_REQUEST['ObjectRef'];
		if (isset($_REQUEST['IX'])) {$IX = $_REQUEST['IX']+1;} else {$IX = 1;}
		if (isset($_REQUEST['Scale'])) {$Scale = $_REQUEST['Scale'];} else {$Scale = 500;}
		if (isset($_REQUEST['ClassName'])) {$ClassName = $_REQUEST['ClassName'];} else {$ClassName = "TGood";}

		$sq = "Exec dbo.spApp_GetImage $ObjectRef, $IX , '$ClassName'";
		$res = database::custom_imgSRV($sq,false);
		MainClass::LogFile("getImage",$sq);
		MainClass::LogFile("getImage",$Scale);
		MainClass::handlePreflightRequest();
		if ($res) {
			$im = new Imagick();
			$im->readimageblob($res["IMG"]);
			$cropWidth = $im->getImageWidth();
			$cropHeight = $im->getImageHeight();
			
			if ($cropWidth>$cropHeight){
				$R=$cropWidth/$cropHeight;
				$cropWidth=$Scale;
				$cropHeight=$cropWidth/$R;
			}
			else{
				$R=$cropHeight/$cropWidth;
				$cropHeight=$Scale;
				$cropWidth=$cropHeight/$R;
			}
			
			$im->adaptiveResizeImage($cropWidth,$cropHeight);

			echo "{\"Text\":\"".base64_encode($im->getimageblob())."\"}";

		}
		else{
			echo "{\"Text\":\"no_photo\"}";
		}		
	}
			
	   
             
             */

            string query = "select top 1 DataValue from dbsetup where KeyValue = '" + Where + "'";



            DataTable dataTable = db.ExecQuery(query, _configuration);

            return jsonClass.JsonResult_Str(dataTable, "Text", "DataValue");


        }






    }
}
