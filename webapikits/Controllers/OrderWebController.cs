
using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderWebController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        public OrderWebController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }




        /// ////////////////////////////////////////////////////////////////






        [HttpGet]
        [Route("OrderMizList")]
        public string OrderMizList(string InfoState, string MizType)
        {

            string query = $" exec spApp_OrderMizList  {InfoState}, N'{MizType}' ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }

        [HttpGet]
        [Route("GetAmountItem")]
        public string GetAmountItem(string Date, string State)
        {

            string query = $" spWeb_Getchartpanel '{Date}' ,{State} ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("GetTodeyFromServer")]
        public string GetTodeyFromServer(string day)
        {

            string query = $"select dbo.fnDate_AddDays(dbo.fnDate_Today(),{day}) TodeyFromServer  ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }


        [HttpGet]
        [Route("minMaxGood")]
        public string minMaxGood(string StartDate, string EndDate, string State)
        {

            string query = $" spweb_Getorderpanel '{StartDate}' ,'{EndDate}' ,{State} ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("GetCustomerMandeh")]
        public string GetCustomerMandeh()
        {

            string query = $" spWeb_GetCustomerMandeh ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("GetCustomerlastGood")]
        public string GetCustomerlastGood(string CustomerCode)
        {

            string query = $" spWeb_GetCustomerlastGood {CustomerCode} ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }





        /// ////////////////////////////////////////////////////////////////


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




            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("Web_GetDbsetupObject")]
        public string Web_GetDbsetupObject(string Where)
        {
            string query = "";


             if (Where == "OrderKowsar")
            {
                query = " select KeyId,KeyValue,DataValue,Description,SubSystem from DbSetup where  KeyValue like '%apporder%' or KeyValue like'%rstfactor%' ";
            }
           


            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }




        [HttpGet]
        [Route("CreateBasketColumn")]
        public string CreateBasketColumn(string AppType)
        {
            string query = "";



             if (AppType == "3") // OrderKowsar
            {
                query = " select '' test";
            }




            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }

        [HttpGet]
        [Route("GetBasketColumnList")]
        public string GetBasketColumnList(string AppType)
        {


            string query = $" select * from AppBasketColumn Where AppType ={AppType} ";

            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }




        [HttpGet]
        [Route("GetGoodType")]
        public string GetGoodType()
        {

            string query = "Exec [spApp_GetGoodType]";



            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("GetProperty")]
        public string GetProperty(string Where)
        {

            string query = $" Select  PropertySchema,PropertyValueMap,PropertyName  from PropertySchema Where ClassName = 'TGOOD' And  ObjectType = '{Where}'";



            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);
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


            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }





        [HttpGet]
        [Route("UpdateDbSetup")]
        public string UpdateDbSetup(
            string DataValue,
            string KeyId)

        {

            string query = $" update dbsetup set DataValue = '{DataValue}'  where keyid = {KeyId}";


            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }




        [HttpGet]
        [Route("GetAppPrinter")]
        public string GetAppprinter(string AppType)

        {

            string query = $"select * from AppPrinter Where AppType={AppType}";


            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);
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


            DataTable dataTable = db.Order_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



    }
}







