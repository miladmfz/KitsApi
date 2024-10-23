
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

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }

        [HttpGet]
        [Route("GetAmountItem")]
        public string GetAmountItem(string Date, string State)
        {

            string query = $" spWeb_Getchartpanel '{Date}' ,{State} ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("GetTodeyFromServer")]
        public string GetTodeyFromServer(string day)
        {

            string query = $"select dbo.fnDate_AddDays(dbo.fnDate_Today(),{day}) TodeyFromServer  ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }


        [HttpGet]
        [Route("minMaxGood")]
        public string minMaxGood(string StartDate, string EndDate, string State)
        {

            string query = $" spweb_Getorderpanel '{StartDate}' ,'{EndDate}' ,{State} ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("GetCustomerMandeh")]
        public string GetCustomerMandeh()
        {

            string query = $" spWeb_GetCustomerMandeh ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("GetCustomerlastGood")]
        public string GetCustomerlastGood(string CustomerCode)
        {

            string query = $" spWeb_GetCustomerlastGood {CustomerCode} ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
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




            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

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
           


            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

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




            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }

        [HttpGet]
        [Route("GetBasketColumnList")]
        public string GetBasketColumnList(string AppType)
        {


            string query = $" select * from AppBasketColumn Where AppType ={AppType} ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);

            return jsonClass.JsonResultWithout_Str(dataTable);
        }




        [HttpGet]
        [Route("GetGoodType")]
        public string GetGoodType()
        {

            string query = "Exec [spApp_GetGoodType]";



            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }



        [HttpGet]
        [Route("GetProperty")]
        public string GetProperty(string Where)
        {

            string query = $" Select  PropertySchema,PropertyValueMap,PropertyName  from PropertySchema Where ClassName = 'TGOOD' And  ObjectType = '{Where}'";



            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
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


            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }





        [HttpGet]
        [Route("UpdateDbSetup")]
        public string UpdateDbSetup(
            string DataValue,
            string KeyId)

        {

            string query = $" update dbsetup set DataValue = '{DataValue}'  where keyid = {KeyId}";


            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);


        }




        [HttpGet]
        [Route("GetAppPrinter")]
        public string GetAppprinter(string AppType)

        {

            string query = $"select * from AppPrinter Where AppType={AppType}";


            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
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


            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }


        [HttpPost]
        [Route("GetOrderGoodList")]
        public string GetOrderGoodList([FromBody] OrderGoodListSearchDto searchDto)
        {
            string searchtarget = searchDto.Where.Replace(" ", "%");

            string query = $"Exec spApp_GetGoods2 @RowCount = {searchDto.RowCount},@Where = N' GoodName like ''%{searchtarget}%''' ,@AppBasketInfoRef=0, @GroupCode = {searchDto.GroupCode} ,@AppType=3 ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("ChangeGoodActive")]
        public string ChangeGoodActive(string GoodCode, string ActiveFlag)
        {

            string query = $"spWeb_ChangeGoodActive {GoodCode},{ActiveFlag} ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }





        [HttpGet]
        [Route("GetGoodEdit")]
        public string GetGoodEdit(string Where)
        {

            string query = $"Select GoodCode,GoodName, CAST(MaxSellprice AS INT) MaxSellprice,GoodExplain1,GoodExplain2,GoodExplain3,GoodExplain4,GoodExplain5,GoodExplain6 from Good Where GoodCode = {Where} ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }

        [HttpGet]
        [Route("GetActiveGood")]
        public string GetActiveGood(string GoodCode)
        {

            string query = $"select ActiveStack,GoodRef from GoodStack where goodref = {GoodCode}  order by 1 desc ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }





        [HttpPost]
        [Route("Web_InsertGood")]
        public string Web_InsertGood([FromBody] GoodDto gooddto)
        {

            string query = $"Exec spWeb_InsertGood '{gooddto.GoodName}' , {gooddto.MaxSellPrice},'{gooddto.GoodExplain1}','{gooddto.GoodExplain2}','{gooddto.GoodExplain3}','{gooddto.GoodExplain4}','{gooddto.GoodExplain5}','{gooddto.GoodExplain6}' ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpPost]
        [Route("Web_UpdateGoodDetail")]
        public string Web_UpdateGoodDetail([FromBody] GoodDto gooddto)
        {

            string query = $"Exec spWeb_UpdateGoodDetail {gooddto.GoodCode},'{gooddto.GoodName}' , {gooddto.MaxSellPrice},'{gooddto.GoodExplain1}','{gooddto.GoodExplain2}','{gooddto.GoodExplain3}','{gooddto.GoodExplain4}','{gooddto.GoodExplain5}','{gooddto.GoodExplain6}' ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }






        [HttpGet]
        [Route("GetGroupFromGood")]
        public string GetGroupFromGood(string Where)
        {

            string query = $"select GoodGroupCode,GoodGroupRef, Name, GoodRef from GoodGroup join Goodsgrp  on GoodGroupRef = GroupCode where Goodref = {Where}  ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]


        [Route("GetGoodFromGroup")]
        public string GetGoodFromGroup(string Where)
        {

            string query = $"select GoodGroupCode, GoodName, GoodCode from Good join GoodGroup on goodref = GoodCode  where GoodGroupRef = {Where}  ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }


        [HttpGet]
        [Route("DeleteGoodGroupCode")]
        public string DeleteGoodGroupCode(string Where)
        {

            string query = $" delete from GoodGroup Where GoodGroupCode = {Where}  ";

            DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            return jsonClass.JsonResultWithout_Str(dataTable);
        }



        [HttpGet]
        [Route("GetWebImagess")]
        public string GetWebImagess(string pixelScale, string ClassName, string ObjectRef)
        {
            string query = $"SELECT * FROM KsrImage WHERE Classname = '{ClassName}' AND ObjectRef = {ObjectRef} order by 1 desc";


            DataTable dataTable = db.Web_ImageExecQuery( query);


            return jsonClass.ConvertAndScaleImageToBase64(Convert.ToInt32(pixelScale), dataTable);

        }

        [HttpPost]
        [Route("UploadImage")]
        public string UploadImage([FromBody] ksrImageModeldto data)
        {


            try
            {


                // Decode the base64 string to bytes
                byte[] decodedImage = Convert.FromBase64String(data.image);

                // Save the image bytes to a file

                string filePath = _configuration.GetConnectionString("web_imagePath") + $"{data.ObjectCode}.jpg"; // Provide the path where you want to save the image

                System.IO.File.WriteAllBytes(filePath, decodedImage);


                string query = $"Exec spImageImport  '{data.ClassName}',{data.ObjectCode},'{filePath}' ;select @@IDENTITY KsrImageCode";


                DataTable dataTable = db.Web_ImageExecQuery( query);

                return "\"Ok\"";
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";

            }
        }

    }
}







