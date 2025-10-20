
using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcrWebController : ControllerBase
    {
        //public readonly IConfiguration _configuration;
        //DataBaseClass db;
        //DataTable DataTable = new DataTable();
        //string Query = "";
        //Response response = new();
        //JsonClass jsonClass = new JsonClass();
        //Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        //public OcrWebController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    db = new DataBaseClass(_configuration);

        //}



        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public OcrWebController(
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


        /// ////////////////////////////////////////////////////////////////////////



        [HttpPost]
        [Route("OcrFactorList")]
        public async Task<IActionResult> OcrFactorList([FromBody] ConditionDto conditionDto)
        {
            string query = $"Exec dbo.spApp_ocrFactorList 4 , '{conditionDto.SearchTarget}' ,' ',50 ,0 , ' order by o.AppTcPrintRef desc' ,0,'{_configuration.GetConnectionString("OcrSecond_Db")}',{conditionDto.SourceFlag} ";

            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);

            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(OcrFactorList));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("ocrGetFactorDetail")]
        public async Task<IActionResult> ocrGetFactorDetail(string AppOCRFactorCode)
        {

            string query = $"Exec spApp_ocrGetFactorDetail {AppOCRFactorCode}  ";
            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);

            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ocrGetFactorDetail));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("ExitDelivery")]
        public async Task<IActionResult> ExitDelivery(string AppOCRFactorCode)
        {

            string query = $"update AppOCRFactor set HasSignature = 0, AppIsDelivered = 0 where AppOCRFactorCode = {AppOCRFactorCode}  ";
            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);

            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ExitDelivery));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetOcrPanel")]
        public async Task<IActionResult> GetOcrPanel(string StartDate, string EndDate, string State)
        {

            string query = $" spWeb_GetOcrPanel '{StartDate}' ,'{EndDate}' ,{State} ";

            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetOcrPanel));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetTodeyFromServer")]
        public async Task<IActionResult> GetTodeyFromServer(string day)
        {

            string query = $"select dbo.fnDate_AddDays(dbo.fnDate_Today(),{day}) TodeyFromServer  ";

            //DataTable dataTable = db.Order_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetTodeyFromServer));
                return StatusCode(500, "Internal server error.");
            }

        }



        /// ////////////////////////////////////////////////////////////////////////


        [HttpGet]
        [Route("BasketColumnCard")]
        public async Task<IActionResult> BasketColumnCard(string Where, string AppType)
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
            



            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResultWithout_Str(dataTable);

            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BasketColumnCard));
                return StatusCode(500, "Internal server error.");
            }

        }



        [HttpGet]
        [Route("Web_GetDbsetupObject")]
        public async Task<IActionResult> Web_GetDbsetupObject(string Where)
        {
            string query = "";


             if (Where == "Ocrkowsar")
            {
                query = " select KeyId,KeyValue,DataValue,Description,SubSystem from DbSetup where KeyValue like '%appocr%'";
            }
           


            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(Web_GetDbsetupObject));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpGet]
        [Route("CreateBasketColumn")]
        public async Task<IActionResult> CreateBasketColumn(string AppType)
        {
            string query = "";


            if (AppType == "2") // Ocrkowsar
            {
                query = "  select '' test ";
            }




            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CreateBasketColumn));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("GetBasketColumnList")]
        public async Task<IActionResult> GetBasketColumnList(string AppType)
        {


            string query = $" select * from AppBasketColumn Where AppType ={AppType} ";

            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetBasketColumnList));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("GetGoodType")]
        public async Task<IActionResult> GetGoodType()
        {

            string query = "Exec [spApp_GetGoodType]";



            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGoodType));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("GetProperty")]
        public async Task<IActionResult> GetProperty(string Where)
        {

            string query = $" Select  PropertySchema,PropertyValueMap,PropertyName  from PropertySchema Where ClassName = 'TGOOD' And  ObjectType = '{Where}'";



            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);

            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetProperty));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("InsertSingleColumn")]
        public async Task<IActionResult> InsertSingleColumn(
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


            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(InsertSingleColumn));
                return StatusCode(500, "Internal server error.");
            }


        }





        [HttpGet]
        [Route("UpdateDbSetup")]
        public async Task<IActionResult> UpdateDbSetup(
            string DataValue,
            string KeyId)

        {

            string query = $" update dbsetup set DataValue = '{DataValue}'  where keyid = {KeyId}";


            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);

            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UpdateDbSetup));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("GetAppPrinter")]
        public async Task<IActionResult> GetAppprinter(string AppType)

        {

            string query = $"select * from AppPrinter Where AppType={AppType}";


            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAppprinter));
                return StatusCode(500, "Internal server error.");
            }


        }






        [HttpPost]
        [Route("UpdatePrinter")]
        public async Task<IActionResult> UpdatePrinter([FromBody] AppPrinterDto printerDto)
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


            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(UpdatePrinter));
                return StatusCode(500, "Internal server error.");
            }

        }
        [HttpPost]
        [Route("GetAttachFileList")]
        public async Task<IActionResult> GetAttachFileList([FromBody] AttachFile attachFile)
        {


            string dbname = "";
            string query1 = "";
            if (attachFile.ClassName == "AutLetter")
            {
                query1 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join AutLetter aut on PeriodId=PeriodRef Where LetterCode= {attachFile.ObjectRef}  Select @db dbname";

            }
            else if (attachFile.ClassName == "Factor")
            {

                query1 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join Factor f on PeriodId=PeriodRef Where FactorCode= {attachFile.ObjectRef}  Select @db dbname";

            }
            else
            {
                query1 = $"Declare @dbname nvarchar(200)=db_name()+'Ocr' select  @dbname dbname";


            }



            DataTable dataTable1 =await  db.Ocr_ExecQuery(HttpContext, query1);
            dbname = dataTable1.Rows[0]["dbname"] + "";

            string query = $"select * from {dbname}..AttachedFiles where ClassName = '{attachFile.ClassName}' And ObjectRef = {attachFile.ObjectRef} ";
            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);

            //return jsonClass.JsonResult_Str(dataTable, "AttachedFiles", "");
            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable,"AttachedFiles", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetAttachFileList));
                return StatusCode(500, "Internal server error.");
            }


        }



        [HttpGet]
        [Route("GetAttachFile")]
        public async Task<IActionResult> GetAttachFileAsync(string AttachedFileCode, string ClassName, string ObjectRef)
        {

            string dbname = "";
            string query11 = "";
            if (ClassName == "AutLetter")
            {
                query11 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join AutLetter aut on PeriodId=PeriodRef Where LetterCode= {ObjectRef}  Select @db dbname";

            }
            else if (ClassName == "Factor")
            {

                query11 = $"  Declare @db nvarchar(100)=''  Select @db = db_name()+'Ocr'+REPLACE(FromDate, '/', '')   From FiscalPeriod p Join Factor f on PeriodId=PeriodRef Where FactorCode= {ObjectRef}  Select @db dbname";

            }
            else
            {
                query11 = $"Declare @dbname nvarchar(200)=db_name()+'Ocr' select  @dbname dbname";


            }



            DataTable dataTable4 = await db.Ocr_ExecQuery(HttpContext, query11);
            dbname = dataTable4.Rows[0]["dbname"] + "";


            string query1 = $"spWeb_GetAttachFile '{AttachedFileCode}' , '{dbname}'";
            DataTable dataTable1 = await db.Ocr_ExecQuery(HttpContext, query1);
            string base64File = dataTable1.Rows[0]["SourceFile"] + "";
            byte[] fileBytes = Convert.FromBase64String(base64File);


            string FileName = dataTable1.Rows[0]["FileName"] + "";




            string dataName_zip = $"{FileName}.zip"; // Constructing the image name
            string data_zipPath = _configuration.GetConnectionString("Ocr_imagePath") + $"{dataName_zip}";
            string contentType = $"application/{dataTable1.Rows[0]["Type"]}";


            System.IO.File.WriteAllBytes(data_zipPath, fileBytes);
            return File(fileBytes, contentType, Path.GetFileName(data_zipPath));





        }


        [HttpPost]
        [Route("EditPackDetail")]
        public async Task<IActionResult> EditPackDetail([FromBody] OcrModel ocrModel)
        {


            string query = $"Update AppOcrFactor Set AppPackCount ={ocrModel.AppPackCount} and AppPackDeliverDate='{ocrModel.AppPackDeliverDate}' where AppOCRFactorCode = {ocrModel.AppOCRFactorCode}";


            //DataTable dataTable = db.Ocr_ExecQuery(HttpContext, query);
            //return jsonClass.JsonResultWithout_Str(dataTable);
            try
            {
                DataTable dataTable = await db.Ocr_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResultWithout_Str(dataTable);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(EditPackDetail));
                return StatusCode(500, "Internal server error.");
            }

        }


    }
}







