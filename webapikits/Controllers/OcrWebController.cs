
using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcrWebController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        public OcrWebController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }



        /// ////////////////////////////////////////////////////////////////////////



        [HttpPost]
        [Route("OcrFactorList")]
        public string OcrFactorList([FromBody] ConditionDto conditionDto)
        {
            string query = $"Exec dbo.spApp_ocrFactorList 4 , '{conditionDto.SearchTarget}' ,' ',50 ,0 , ' order by o.AppTcPrintRef desc' ,0,'{_configuration.GetConnectionString("OcrSecond_Db")}',{conditionDto.SourceFlag} ";

            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("ocrGetFactorDetail")]
        public string ocrGetFactorDetail(string AppOCRFactorCode)
        {

            string query = $"Exec spApp_ocrGetFactorDetail {AppOCRFactorCode}  ";
            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }




        [HttpGet]
        [Route("ExitDelivery")]
        public string ExitDelivery(string AppOCRFactorCode)
        {

            string query = $"update AppOCRFactor set HasSignature = 0, AppIsDelivered = 0 where AppOCRFactorCode = {AppOCRFactorCode}  ";
            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }





        /// ////////////////////////////////////////////////////////////////////////


        [HttpGet]
        [Route("BasketColumnCard")]
        public string BasketColumnCard(string Where, string AppType)
        {
            string query = "";



            if (Where == "ListVisible")
            {
                query = $" select AppBasketColumnCode,ColumnName,ColumnDesc,ColumnDefinition,ObjectType, " +
                        $" ListVisible,ColumnType,OrderIndex,Condition,AppType from AppBasketColumn " +
                        $" where apptype ={AppType} and ListVisible > 0 And ObjectType='' order by ListVisible  ";
            }
            else if (Where == "DetailVisible")
            {
                query = $" select AppBasketColumnCode,ColumnName,ColumnDesc,ColumnDefinition,ObjectType, " +
                        $" DetailVisible,ColumnType,OrderIndex,Condition,AppType from AppBasketColumn  " +
                        $" where apptype ={AppType} and DetailVisible > 0 And ObjectType='' order by DetailVisible  ";
            }
            else if (Where == "SearchVisible")
            {
                query = $" select AppBasketColumnCode,ColumnName,ColumnDesc,ColumnDefinition,ObjectType, " +
                        $"SearchVisible,ColumnType,OrderIndex,Condition,AppType from AppBasketColumn " +
                        $" where apptype ={AppType} and SearchVisible > 0 And ObjectType='' order by SearchVisible  ";
            }
            



            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("Web_GetDbsetupObject")]
        public string Web_GetDbsetupObject(string Where)
        {
            string query = "";


             if (Where == "Ocrkowsar")
            {
                query = " select KeyId,KeyValue,DataValue,Description,SubSystem from DbSetup where KeyValue like '%appocr%'";
            }
           


            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }





        [HttpGet]
        [Route("CreateBasketColumn")]
        public string CreateBasketColumn(string AppType)
        {
            string query = "";


            if (AppType == "2") // Ocrkowsar
            {
                query = "  select '' test ";
            }




            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }

        [HttpGet]
        [Route("GetBasketColumnList")]
        public string GetBasketColumnList(string AppType)
        {


            string query = $" select * from AppBasketColumn Where AppType ={AppType} ";

            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }




        [HttpGet]
        [Route("GetGoodType")]
        public string GetGoodType()
        {

            string query = "Exec [spApp_GetGoodType]";



            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("GetProperty")]
        public string GetProperty(string Where)
        {

            string query = $" Select  PropertySchema,PropertyValueMap,PropertyName  from PropertySchema Where ClassName = 'TGOOD' And  ObjectType = '{Where}'";



            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }


        [HttpGet]
        [Route("InsertSingleColumn")]
        public string InsertSingleColumn(
            string ColumnName,
            string ColumnDesc,
            string ObjectType,
            string DetailVisible,
            string ListVisible,
            string SearchVisible,
            string ColumnType,
            string AppType
)
        {

            string query =

                $" Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)" +
                $" Select '{ColumnName}','{ColumnDesc}','','{ObjectType}','{DetailVisible}','{ListVisible}','-1','{SearchVisible}','{ColumnType}','0','','{AppType}' ";


            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }





        [HttpGet]
        [Route("UpdateDbSetup")]
        public string UpdateDbSetup(
            string DataValue,
            string KeyId)

        {

            string query = $" update dbsetup set DataValue = '{DataValue}'  where keyid = {KeyId}";


            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }




        [HttpGet]
        [Route("GetAppPrinter")]
        public string GetAppprinter(string AppType)

        {

            string query = $"select * from AppPrinter Where AppType={AppType}";


            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }






        [HttpPost]
        [Route("UpdatePrinter")]
        public string UpdatePrinter([FromBody] AppPrinterDto printerDto)
        {


            string query = "";

            if (printerDto.AppPrinterCode == "0")
            {
                query = $" Insert Into AppPrinter ( [PrinterName], [PrinterExplain], [GoodGroups], [WhereClause], [PrintCount], [PrinterActive], [FilePath], [AppType] ) values ('{printerDto.PrinterName}', '{printerDto.PrinterExplain}', '{printerDto.GoodGroups}', '{printerDto.WhereClause}', '{printerDto.PrintCount}', '{printerDto.PrinterActive}', '{printerDto.FilePath}', '{printerDto.AppType}') ";

            }
            else
            {
                query = $" Update AppPrinter set [PrinterName] = '{printerDto.PrinterName}', [PrinterExplain]= '{printerDto.PrinterExplain}', [GoodGroups]= '{printerDto.GoodGroups}', [WhereClause]= '{printerDto.WhereClause}', [PrintCount]= '{printerDto.PrintCount}', [PrinterActive]= '{printerDto.PrinterActive}', [FilePath]= '{printerDto.FilePath}', [AppType] = '{printerDto.AppType}' Where AppPrinterCode = {printerDto.AppPrinterCode}";
            }


            DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



    }
}







