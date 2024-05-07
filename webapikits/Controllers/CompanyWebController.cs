using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyWebController : Controller
    {
        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        public CompanyWebController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }



        /// ////////////////////////////////////////////////////////////////////////





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




            DataTable dataTable = db.Company_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("Web_GetDbsetupObject")]
        public string Web_GetDbsetupObject(string Where)
        {
            string query = "";


            if (Where == "Company")
            {
                query = " select KeyId,KeyValue,DataValue,Description,SubSystem from DbSetup where KeyValue like '%appbasket%'";
            }


            DataTable dataTable = db.Company_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }





        [HttpGet]
        [Route("CreateBasketColumn")]
        public string CreateBasketColumn(string AppType)
        {
            string query = "";

            if (AppType == "0")  //Company
            {
                query =
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodCode'          ,'كد سيستمي',''  ,'','0'  ,'1','1'  ,'0'  ,'','0'  ,'','0' " +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodName'          ,'نام كالا','GoodExplain5'  ,'','0'  ,'1','2'  ,'1'  ,'','0'  ,'','0' " +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodType'          ,'نوع كالا',''  ,'','0'  ,'1','3'  ,'0'  ,'','0'  ,'','0' " +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'MaxSellPrice'      ,'في ناخالص',''  ,'','0'  ,'1','4'  ,'0'  ,'','0'  ,'','0' " +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'SellPrice'         ,'في','Cast(Case @C When 1 Then SellPrice1Str When 2 Then SellPrice2Str When 3 Then SellPrice3Str When 4 Then SellPrice4Str When 5 Then SellPrice5Str When 6 Then SellPrice6Str When 7 Then MaxSellPrice*(100-@Takhf)/100 Else MaxSellPrice End as bigInt)'  ,'','0'  ,'1','5'  ,'0'  ,'','0'  ,'','0' " +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodMainCode'      ,'كد محصول',''  ,'','1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodSubCode'       ,'كدفرعي',''  ,'','1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodExplain2'      ,'مشخصه2',''  ,'','0'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodExplain3'      ,'مشخصه3',''  ,'','0'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodExplain4'      ,'مشخصه4',''  ,'','0'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodExplain5'      ,'مشخصه5',''  ,'','0'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodExplain6'      ,'مشخصه6',''  ,'','0'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'Barcode'           ,'باركد',''  ,'','1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'ISBN'              ,'شابك',''  ,'','1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'ImageCount'        ,'تعداد عكس',''  ,'','0'  ,'1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'BulletinGroupName' ,'گروه كالا',''  ,'','1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GroupsWhitoutCode' ,'موضوع',''  ,'','1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'HasStackAmount'    ,'موجودي','Case When @IsReserved in(0,2) Then 1 When @IsReserved=1 And IsNull( b.FacAmount ,0)<Amount-ReservedAmount-@MinAmount Then 1 Else 0 End'  ,'','0'  ,'1','1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'IsFavorite'        ,'علاقه مندي','Case When Exists(Select Top 1 1 From AppFavorite f Where f.GoodRef = GoodCode And f.MobileNo=@MobileNo) Then 1 Else 0 End'  ,'','0'  ,'1','1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'GoodImageName'     ,'نام عكس',''''  ,'','0'  ,'-1','0'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'IsReserved'        ,'رزرو','@IsReserved'  ,'','-1'  ,'-1','-1'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'BasketAmount'      ,'تعداد سفارش','Cast( IsNull( b.FacAmount ,0) as bigInt)'  ,'','1'  ,'1','0'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'IsReserved'        ,'رزرو','isnull(Cast (R.IsReserved as int ), @IsReserved)'  ,'','-1'  ,'-1','0'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'FacAmount'         ,'تعداد','ISNULL (R.facamount  ,b.facamount)'  ,'','-1'  ,'-1','8'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'Price'             ,'في','ISNULL (R.Price,b.Price)'  ,'','-1'  ,'-1','9'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'TotalAmount'       ,'جمع كل','IsNull((Select Sum(sks.Amount) From GoodStack sks Where sks.StackRef in(10110) And sks.GoodRef= GoodCode ),0)'  ,'','0'  ,'0','0'  ,'0'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'MinPrice'          ,'از قيمت','Cast(Case @C When 1 Then SellPrice1Str When 2 Then SellPrice2Str When 3 Then SellPrice3Str When 4 Then SellPrice4Str When 5 Then SellPrice5Str When 6 Then SellPrice6Str When 7 Then MaxSellPrice*(100-@Takhf)/100 Else MaxSellPrice End as bigInt)'  ,'','-1'  ,'-1','-1'  ,'20'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'MaxPrice'          ,'تا قيمت','Cast(Case @C When 1 Then SellPrice1Str When 2 Then SellPrice2Str When 3 Then SellPrice3Str When 4 Then SellPrice4Str When 5 Then SellPrice5Str When 6 Then SellPrice6Str When 7 Then MaxSellPrice*(100-@Takhf)/100 Else MaxSellPrice End as bigInt)'  ,'','-1'  ,'-1','-1'  ,'21'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'Active'            ,'فعال','g.active'  ,'','-1'  ,'0','-1'  ,'-1'  ,'','0'  ,'','0' Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'bUnitRef','واحد خريد',''  ,'','-1'  ,'-1','0'  ,'-1'  ,'','0'  ,'','0'" +
                    " Insert Into AppBasketColumn(ColumnName, ColumnDesc, ColumnDefinition, ObjectType, DetailVisible, ListVisible, BasketVisible, SearchVisible,ColumnType,OrderIndex,Condition,AppType)  Select   'bRatio'            ,'واحدتعداد',''  ,'','-1'  ,'-1','0'  ,'-1'  ,'','0'  ,'','0'";
            }




            DataTable dataTable = db.Company_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }

        [HttpGet]
        [Route("GetBasketColumnList")]
        public string GetBasketColumnList(string AppType)
        {


            string query = $" select * from AppBasketColumn Where AppType ={AppType} ";

            DataTable dataTable = db.Company_ExecQuery(Request.Path, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }




        [HttpGet]
        [Route("GetGoodType")]
        public string GetGoodType()
        {

            string query = "Exec [spApp_GetGoodType]";



            DataTable dataTable = db.Company_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("GetProperty")]
        public string GetProperty(string Where)
        {

            string query = $" Select  PropertySchema,PropertyValueMap,PropertyName  from PropertySchema Where ClassName = 'TGOOD' And  ObjectType = '{Where}'";



            DataTable dataTable = db.Company_ExecQuery(Request.Path, query);
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


            DataTable dataTable = db.Company_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }





        [HttpGet]
        [Route("UpdateDbSetup")]
        public string UpdateDbSetup(
            string DataValue,
            string KeyId)

        {

            string query = $" update dbsetup set DataValue = '{DataValue}'  where keyid = {KeyId}";


            DataTable dataTable = db.Company_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }




        [HttpGet]
        [Route("GetAppPrinter")]
        public string GetAppprinter(string AppType)

        {

            string query = $"select * from AppPrinter Where AppType={AppType}";


            DataTable dataTable = db.Company_ExecQuery(Request.Path, query);
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


            DataTable dataTable = db.Company_ExecQuery(Request.Path, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



    }
}







