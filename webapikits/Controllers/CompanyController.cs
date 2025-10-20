using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using webapikits.Model;
namespace webapikits.Controllers
{




    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        //public readonly IConfiguration _configuration;
        //DataBaseClass db;
        //DataTable DataTable = new DataTable();
        //string Query = "";
        //Response response = new();
        //JsonClass jsonClass = new JsonClass();
        //Dictionary<string, string> jsonDict = new Dictionary<string, string>();



        //public CompanyController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    db = new DataBaseClass(_configuration);

        //}

        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public CompanyController(
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






        //[HttpGet]
        //[Route("VersionInfo")]
        //public string VersionInfo()
        //{
        //    response.StatusCode = "2000";
        //    response.Errormessage = "";

        //    jsonDict.Add("response", JsonConvert.SerializeObject(response));
        //    jsonDict.Add("Text", "2.0");
        //    return JsonConvert.SerializeObject(jsonDict);
            
        //}


        //[HttpGet]
        //[Route("check_server")]
        //public async Task<IActionResult> check_server()
        //{
        //    response.StatusCode = "2000";
        //    response.Errormessage = "";

        //    jsonDict.Add("response", JsonConvert.SerializeObject(response));
        //    jsonDict.Add("Text", "false");
        //    return JsonConvert.SerializeObject(jsonDict);

        //    try
        //    {
        //        DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
        //        string json = jsonClass.JsonResult_Str(dataTable, "users", "");
        //        return Content(json, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred in {Function}", nameof(IsUser));
        //        return StatusCode(500, "Internal server error.");
        //    }

        //}



        [HttpGet]
        [Route("BasketGet")]
        public async Task<IActionResult> BasketGet(string Mobile)
        {

            string query = $"Exec [dbo].[spApp_BasketGet] '{Mobile}'";


            


             

            try
            {
                DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BasketGet));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("BasketHistory")]
        public async Task<IActionResult> BasketHistory(string Mobile,string Code, string ReservedRows)
        {

            string query = $"Exec [dbo].[spApp_BasketPreFactors] '{Mobile}',{Code},{ReservedRows}" ;


            

            try
            {
                DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "PreFactors", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BasketHistory));
                return StatusCode(500, "Internal server error.");
            }



        }


        [HttpGet]
        [Route("BasketToPreFactor")]
        public async Task<IActionResult> BasketToPreFactor(string Mobile, string Explain)
        {

            string query = $"Exec [dbo].[spApp_BasketToPreFactor] '{Mobile}', -2000 , '{Explain}'";


            

             

            try
            {
                DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BasketToPreFactor));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("BasketSum")]
        public async Task<IActionResult> BasketSum(string Mobile)
        {

            string query = $"Exec dbo.spApp_BasketSummary '{Mobile}'";


            
             
            try
            {
                DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BasketSum));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpGet]
        [Route("Basketdeleteall")]
        public async Task<IActionResult> Basketdeleteall(string Mobile)
        {

            string query = $"  set nocount on Update AppBasket set ProcessStatus = 10 where MobileNo = '{Mobile}' and ProcessStatus = 0 select 1";



            
            try
            {
                DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "done");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Basketdeleteall));
                return StatusCode(500, "Internal server error.");
            }


        }








        [HttpGet]
        [Route("deletebasket")]
        public async Task<IActionResult> deletebasket(
            string DeviceCode,
            string GoodRef,
            string UserId,
            string Mobile
            )
        {

            string query = $"Exec [dbo].[spApp_BasketDelete] '{DeviceCode}' , {GoodRef} , {UserId} , '{Mobile}'";



            
            try
            {
                DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "done");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(deletebasket));
                return StatusCode(500, "Internal server error.");
            }


        }





        [HttpGet]
        [Route("Favorite_action")]
        public async Task<IActionResult> Favorite_action(
            string Mobile,
            string GoodRef,
            string DeleteFlag
            )
        {

            string query = $"Exec [dbo].[spApp_FavoriteInsert] '{Mobile}',{GoodRef},{DeleteFlag} ";



            
            try
            {
                DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Text", "Result");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Favorite_action));
                return StatusCode(500, "Internal server error.");
            }


        }


        [HttpGet]
        [Route("GoodGroupInfo_Default")]
        public async Task<IActionResult> GoodGroupInfo_Default()
        {

            string query = $"Exec [dbo].[spApp_GetGoodGroups_Default] ";


            
            try
            {
                DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Groups", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodGroupInfo_Default));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("GoodGroupInfo_DefaultImage")]
        public async Task<IActionResult> GoodGroupInfo_DefaultImage()
        {

            string query = $"Exec [dbo].[spApp_GetGoodGroups_DefaultImage]  ";


            
            try
            {
                DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Groups", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodGroupInfo_DefaultImage));
                return StatusCode(500, "Internal server error.");
            }


        }






        [HttpGet]
        [Route("goodinfo")]
        public async Task<IActionResult> goodinfo(
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

            string query = $"exec spApp_GetGoods2 ";

            if (!string.IsNullOrEmpty(RowCount))
            {
                query += $"@RowCount = {RowCount} ";
            }else { 
                query += $"@RowCount = 10"; 
            }

            if (!string.IsNullOrEmpty(SearchTarget))
            {
                query += $"@SearchTarget =  N'{SearchTarget}' ";
            }

            if (!string.IsNullOrEmpty(OrderBy))
            {
                query += $"@OrderBy = N'{OrderBy}' ";
            }

            if (!string.IsNullOrEmpty(OnlyActive))
            {
                query += $"@OnlyActive = {OnlyActive} ";
            }

            if (!string.IsNullOrEmpty(OnlyAvailable))
            {
                query += $"@OnlyAvailable = {OnlyAvailable} ";
            }

            if (!string.IsNullOrEmpty(GroupCode))
            {
                query += $"@GroupCode = {GroupCode} ";
            }

            if (!string.IsNullOrEmpty(Where))
            {
                query += $"@Where = N'{Where}' ";
            }

            if (!string.IsNullOrEmpty(LikeGood))
            {
                query += $"@LikeGoodRef = {LikeGood} ";
            }

            if (!string.IsNullOrEmpty(PageNo))
            {
                query += $"@PageNo = {PageNo} ";
            }

            if (!string.IsNullOrEmpty(MobileNo))
            {
                query += $"@MobileNo = '{MobileNo}' ";
            }

            if (!string.IsNullOrEmpty(OnlyFavorite))
            {
                query += $"@OnlyFavorite = {OnlyFavorite} ";
            }

            if (!string.IsNullOrEmpty(GoodCode))
            {
                query += $"@GoodCode = {GoodCode} ";
            }


            
             
            try
            {
                DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(goodinfo));
                return StatusCode(500, "Internal server error.");
            }


        }





        [HttpGet]
        [Route("GoodGroupInfo")]
        public async Task<IActionResult> GoodGroupInfo(string GroupName,
            string GroupCode
            )
        {

            string query = $"Exec [dbo].[spApp_GetGoodGroups] ";

            if (!string.IsNullOrEmpty(GroupName))
            {
                query += $"@GroupName = N'{GroupName}' ";
            }

            if (!string.IsNullOrEmpty(GroupCode))
            {
                query += $"@GroupCode = {GroupCode} ";
            }


            


            try
            {
                DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Groups", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodGroupInfo));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpGet]
        [Route("XUserCreate")]
        public async Task<IActionResult> XUserCreate(
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



            string query = $"Exec [dbo].[spApp_XUserCreate] '{UName}','{UPass}','{NewPass}','{FName}','{LName}','{mobile}','{company}','{address}','{PostalCode}','{email}',-2000,{Flag}"  ;


            
            try
            {
                DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "users", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(XUserCreate));
                return StatusCode(500, "Internal server error.");
            }


        }







        [HttpPost]
        [Route("InsertBasket")]
        public async Task<IActionResult> InsertBasket([FromBody] JObject request)
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

            string query = $"Exec [dbo].[spApp_BasketInsert] '{DeviceCode}', {GoodRef}, {FacAmount}, {Price}, '{UnitRef}', '{Ratio}', '{Explain}', '{Source}', {UserId}, '{Mobile}'";

             
            try
            {
                DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Goods", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(InsertBasket));
                return StatusCode(500, "Internal server error.");
            }


        }


        //[HttpPost]
        //[Route("PFRCDEWS")]
        //public IActionResult PFRCDEWS([FromBody] JObject request)
        //{
        //    int UserId = -2000;
        //    string Mobile = "";
        //    int Price = 0;
        //    string Rahgiri = "";
        //    string BankKart = "";
        //    string KartOwner = "";
        //    int PreFactorCode = 0;

        //    if (request.TryGetValue("Mobile", out JToken mobileToken))
        //    {
        //        Mobile = mobileToken.Value<string>();
        //    }

        //    if (request.TryGetValue("Price", out JToken priceToken))
        //    {
        //        Price = priceToken.Value<int>();
        //    }

        //    if (request.TryGetValue("Rahgiri", out JToken rahgiriToken))
        //    {
        //        Rahgiri = rahgiriToken.Value<string>();
        //    }

        //    if (request.TryGetValue("BankKart", out JToken bankKartToken))
        //    {
        //        BankKart = bankKartToken.Value<string>();
        //    }

        //    if (request.TryGetValue("KartOwner", out JToken kartOwnerToken))
        //    {
        //        KartOwner = kartOwnerToken.Value<string>();
        //    }

        //    if (request.TryGetValue("PreFactorCode", out JToken preFactorCodeToken))
        //    {
        //        PreFactorCode = preFactorCodeToken.Value<int>();
        //    }

        //    string sq = $"Exec [dbo].[spApp_InsertReceive] '{Mobile}', {Price}, '{Rahgiri}', '{BankKart}', '{KartOwner}', {PreFactorCode}, {UserId}";

        //    string last = JsonConvert.SerializeObject(response, Formatting.None);
        //    return Content("{\"users\":" + last + "}", "application/json");
        //    try
        //    {
        //        DataTable dataTable = await db.Company_ExecQuery(HttpContext, query);
        //        string json = jsonClass.JsonResult_Str(dataTable, "users", "");
        //        return Content(json, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred in {Function}", nameof(PFRCDEWS));
        //        return StatusCode(500, "Internal server error.");
        //    }

        //}






        [HttpGet("Banner")]
    public IActionResult GetBanner()
    {
            //TODO banner list 
        return Ok(GetBanner);
    }



}
}
