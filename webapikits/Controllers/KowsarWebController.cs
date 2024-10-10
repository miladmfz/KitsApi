using FastReport;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using webapikits.Model;
using Microsoft.AspNetCore.Http;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KowsarWebController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        public KowsarWebController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }


        [HttpGet]
        [Route("GetProperty")]
        public string GetProperty(string Where)
        {

            string query = $" Select  PropertySchema,PropertyValueMap,PropertyName  from PropertySchema Where ClassName = 'TGOOD' And  ObjectType = '{Where}'";



            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }


        [HttpGet]
        [Route("GetObjectTypeFromDbSetup")]
        public string GetObjectTypeFromDbSetup(string ObjectType)
        {

            string query = "select * from dbo.fnObjectType('" + ObjectType + "') ";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);

            return jsonClass.JsonResult_Str(dataTable, "ObjectTypes", "");

        }
        





        [HttpGet]
        [Route("GetLastGoodData")]
        public string GetLastGoodData()
        {

            string query = $"  declare @ss int  select  @ss=max(GoodCode) from good exec spWeb_GetGoodById @ss,0";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetGoodBase")]
        public string GetGoodBase(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},0";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }

        [HttpGet]
        [Route("GetGoodExplain")]
        public string GetGoodExplain(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},1";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetGoodComplete")]
        public string GetGoodComplete(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},2";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }




        [HttpGet]
        [Route("GetGoodProperty")]
        public string GetGoodProperty(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},3";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }






        [HttpGet]
        [Route("GetGoodRelations")]
        public string GetGoodRelations(string GoodCode)
        {

            string query = $"  spWeb_GetGoodById {GoodCode},4";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }



        [HttpGet]
        [Route("GetGoodImages")]
        public string GetGoodImages(string GoodCode)
        {

            string query = $"  select top 10 KsrImageCode,ClassName,ObjectRef,IsDefaultImage,FileName from KsrImage where objectref={GoodCode}";

            DataTable dataTable = db.Web_ImageExecQuery(query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }


        [HttpGet]
        [Route("GetGoodGroups")]
        public string GetGoodGroups(string GoodCode)
        {

            string query = $"select GoodGroupCode,GroupCode, Name, GoodRef from GoodGroup join Goodsgrp  on GoodGroupRef = GroupCode\r\n where Goodref = {GoodCode}  ";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");

        }



        [HttpGet]
        [Route("GetGoodStacks")]
        public string GetGoodStacks(string GoodCode)
        {

            string query = $"select GoodStackCode,GoodRef,StackRef,Amount,ReservedAmount,Name,ActiveStack  from vwGoodStack where goodref= {GoodCode}  ";

            DataTable dataTable = db.Kowsar_ExecQuery(HttpContext, query);
            return jsonClass.JsonResult_Str(dataTable, "Goods", "");
        }








    }
}







