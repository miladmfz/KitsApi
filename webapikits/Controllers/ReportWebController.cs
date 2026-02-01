using FastReport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using webapikits.Model;

namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportWebController : ControllerBase
    {


        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportNewController> _logger;
        private readonly IConfiguration _configuration;
        JsonClass jsonClass = new JsonClass();


        public ReportWebController(
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








        [HttpPost]
        [Route("GetReports")]
        public async Task<IActionResult> GetReports([FromBody] SearchTargetDto searchTargetDto)
        {

            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query = $" spWeb_GetReports  0, N'%{searchTargetDto.SearchTarget}%' , '{UserName}' ";


            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetReports));
                return StatusCode(500, "Internal server error.");
            }

        }




        [HttpGet]
        [Route("GetReportsByCode")]
        public async Task<IActionResult> GetReportsByCode(string ReportCode)
        {
            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;



            string query = $"spWeb_GetReports {ReportCode},'','{UserName}'";


            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetReportsByCode));
                return StatusCode(500, "Internal server error.");
            }

        }


        [HttpGet]
        [Route("GetStacks")]
        public async Task<IActionResult> GetStacks()
        {

            string query = $"spWeb_GetStacks ";



            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Stacks", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetStacks));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("GetGridSchema")]
        public async Task<IActionResult> GetGridSchema(string ClassName)
        {


            string query = $"Select * From dbo.fnGetGridSchema('{ClassName}') Where Visible=1 ";


            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GridSchemas", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGridSchema));
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        [Route("GetGridSchemaAll")]
        public async Task<IActionResult> GetGridSchemaAll(string ClassName)
        {


            string query = $"Select * From dbo.fnGetGridSchema('{ClassName}')  ";


            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "GridSchemas", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GetGridSchemaAll));
                return StatusCode(500, "Internal server error.");
            }

        }





        [HttpPost]
        [Route("BazaryabKarkardRpt")]
        public async Task<IActionResult> BazaryabKarkardRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {



            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   BazaryabKarkardRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BazaryabKarkardRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerCityRpt")]
        public async Task<IActionResult> CustomerCityRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {



            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerCityRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerCityRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerGroupRpt")]
        public async Task<IActionResult> CustomerGroupRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerGroupRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerGroupRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerMandehRpt")]
        public async Task<IActionResult> CustomerMandehRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerMandehRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerMandehRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("AccSanadBrowseRpt")]
        public async Task<IActionResult> AccSanadBrowseRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   AccSanadBrowseRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(AccSanadBrowseRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerReceiveRpt")]
        public async Task<IActionResult> CustomerReceiveRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;


            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerReceiveRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerReceiveRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerPaymentRpt")]
        public async Task<IActionResult> CustomerPaymentRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerPaymentRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerPaymentRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerForoshRpt")]
        public async Task<IActionResult> CustomerForoshRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            



            string query = $"   spCustomerForosh  @FromDate ='{searchTargetReportDto.FromDate}',  @ToDate ='{searchTargetReportDto.ToDate}',   @Department ='{searchTargetReportDto.Department}',  @WhereCluase =N'{XCondition}',  @OrderBy='{searchTargetReportDto.OrderBy}',  @Column = '{ColumnsName}'   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerForoshRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerActionTypeRpt")]
        public async Task<IActionResult> CustomerActionTypeRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerActionTypeRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerActionTypeRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerIdentificationEtebarRpt")]
        public async Task<IActionResult> CustomerIdentificationEtebarRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerIdentificationEtebarRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerIdentificationEtebarRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerFactorEtebarRpt")]
        public async Task<IActionResult> CustomerFactorEtebarRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerFactorEtebarRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerFactorEtebarRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerForoshCityRpt")]
        public async Task<IActionResult> CustomerForoshCityRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerForoshCityRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerForoshCityRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("PeriodicCustomerPurchaseRpt")]
        public async Task<IActionResult> PeriodicCustomerPurchaseRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   PeriodicCustomerPurchaseRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PeriodicCustomerPurchaseRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("SellReportRpt")]
        public async Task<IActionResult> SellReportRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   SellReportRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SellReportRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("PeriodicSellRpt")]
        public async Task<IActionResult> PeriodicSellRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   PeriodicSellRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PeriodicSellRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerReceiveGroupRpt")]
        public async Task<IActionResult> CustomerReceiveGroupRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerReceiveGroupRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerReceiveGroupRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerReceiveCityRpt")]
        public async Task<IActionResult> CustomerReceiveCityRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerReceiveCityRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerReceiveCityRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerPaymentGroupRpt")]
        public async Task<IActionResult> CustomerPaymentGroupRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerPaymentGroupRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerPaymentGroupRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerPaymentCityRpt")]
        public async Task<IActionResult> CustomerPaymentCityRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerPaymentCityRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerPaymentCityRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("BulletinCitySellRpt")]
        public async Task<IActionResult> BulletinCitySellRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   BulletinCitySellRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BulletinCitySellRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerFactorRpt")]
        public async Task<IActionResult> CustomerFactorRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerFactorRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerFactorRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("FactorRowsRpt")]
        public async Task<IActionResult> FactorRowsRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   FactorRowsRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(FactorRowsRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerPreFactorRpt")]
        public async Task<IActionResult> CustomerPreFactorRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerPreFactorRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerPreFactorRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("PreFactorRowsRpt")]
        public async Task<IActionResult> PreFactorRowsRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   PreFactorRowsRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PreFactorRowsRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerReturnFactorRpt")]
        public async Task<IActionResult> CustomerReturnFactorRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerReturnFactorRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerReturnFactorRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("ReturnFactorRowsRpt")]
        public async Task<IActionResult> ReturnFactorRowsRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   ReturnFactorRowsRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ReturnFactorRowsRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("SellReceivedFactorRpt")]
        public async Task<IActionResult> SellReceivedFactorRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   SellReceivedFactorRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SellReceivedFactorRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("ShopfactorByPriceRpt")]
        public async Task<IActionResult> ShopfactorByPriceRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   ShopfactorByPriceRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ShopfactorByPriceRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerGoodRpt")]
        public async Task<IActionResult> CustomerGoodRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerGoodRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerGoodRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodForoshRpt")]
        public async Task<IActionResult> GoodForoshRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $" Exec  spGoodForosh  @FromDate = '{searchTargetReportDto.FromDate}',  @ToDate = '{searchTargetReportDto.ToDate}',  @FromTime = '{searchTargetReportDto.FromTime}'," +
                            $"  @ToTime = '{searchTargetReportDto.ToTime}',  @StackRef = {searchTargetReportDto.StackRef},  @GoodTableName = '{searchTargetReportDto.GoodTableName}'," +
                            $"  @GoodFieldName = '{searchTargetReportDto.GoodFieldName}',  @DamagedGoodsStackCode = {searchTargetReportDto.DamagedGoodsStackCode},  @GroupByField = '{searchTargetReportDto.GroupByField}', " +
                            $"  @Department = '{searchTargetReportDto.Department}',  @WhereCluase = '{searchTargetReportDto.WhereCluase}',  @OrderBy = '{searchTargetReportDto.OrderBy}',  @Column ='{ColumnsName}'   ";

            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodForoshRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodCustomerRpt")]
        public async Task<IActionResult> GoodCustomerRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodCustomerRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodCustomerRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodFactorRpt")]
        public async Task<IActionResult> GoodFactorRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodFactorRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodFactorRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodFactorRowsRpt")]
        public async Task<IActionResult> GoodFactorRowsRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodFactorRowsRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodFactorRowsRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("FactorTypeMonthlyGoodSellStateRpt")]
        public async Task<IActionResult> FactorTypeMonthlyGoodSellStateRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   FactorTypeMonthlyGoodSellStateRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(FactorTypeMonthlyGoodSellStateRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("BuyStyleMonthlyGoodSellStateRpt")]
        public async Task<IActionResult> BuyStyleMonthlyGoodSellStateRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   BuyStyleMonthlyGoodSellStateRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BuyStyleMonthlyGoodSellStateRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("MonthlyGoodSellStateRpt")]
        public async Task<IActionResult> MonthlyGoodSellStateRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   MonthlyGoodSellStateRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(MonthlyGoodSellStateRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("SumofPeriodicGoodSellRpt")]
        public async Task<IActionResult> SumofPeriodicGoodSellRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   SumofPeriodicGoodSellRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SumofPeriodicGoodSellRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("PeriodicGoodSellRpt")]
        public async Task<IActionResult> PeriodicGoodSellRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   PeriodicGoodSellRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PeriodicGoodSellRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("PeriodicCustomerPurchaseSeparateRpt")]
        public async Task<IActionResult> PeriodicCustomerPurchaseSeparateRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   PeriodicCustomerPurchaseSeparateRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PeriodicCustomerPurchaseSeparateRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("BulletinGroupNameSellRpt")]
        public async Task<IActionResult> BulletinGroupNameSellRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   BulletinGroupNameSellRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BulletinGroupNameSellRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodBulletinGroupSellRpt")]
        public async Task<IActionResult> GoodBulletinGroupSellRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodBulletinGroupSellRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodBulletinGroupSellRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerIdentificationRpt")]
        public async Task<IActionResult> CustomerIdentificationRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerIdentificationRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerIdentificationRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("AllGoodsRpt")]
        public async Task<IActionResult> AllGoodsRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   AllGoodsRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(AllGoodsRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodInStackRpt")]
        public async Task<IActionResult> GoodInStackRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodInStackRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodInStackRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodHistoryRpt")]
        public async Task<IActionResult> GoodHistoryRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodHistoryRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodHistoryRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodGroupRpt")]
        public async Task<IActionResult> GoodGroupRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodGroupRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodGroupRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodSefareshPointRpt")]
        public async Task<IActionResult> GoodSefareshPointRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodSefareshPointRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodSefareshPointRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("PeriodicInOutGoodStateRpt")]
        public async Task<IActionResult> PeriodicInOutGoodStateRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   PeriodicInOutGoodStateRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PeriodicInOutGoodStateRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("StackTransferRpt")]
        public async Task<IActionResult> StackTransferRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   StackTransferRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(StackTransferRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("StackTransferRowRpt")]
        public async Task<IActionResult> StackTransferRowRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   StackTransferRowRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(StackTransferRowRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("LastGoodSubCodeHistoryRpt")]
        public async Task<IActionResult> LastGoodSubCodeHistoryRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   LastGoodSubCodeHistoryRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LastGoodSubCodeHistoryRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CashReceiveRpt")]
        public async Task<IActionResult> CashReceiveRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CashReceiveRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CashReceiveRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CashPaymentRpt")]
        public async Task<IActionResult> CashPaymentRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CashPaymentRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CashPaymentRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CashReceiveCheckRpt")]
        public async Task<IActionResult> CashReceiveCheckRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CashReceiveCheckRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CashReceiveCheckRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("BankPaymentCheckRpt")]
        public async Task<IActionResult> BankPaymentCheckRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   BankPaymentCheckRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BankPaymentCheckRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CheckHistoryRpt")]
        public async Task<IActionResult> CheckHistoryRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CheckHistoryRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CheckHistoryRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("BrokerKarkardRpt")]
        public async Task<IActionResult> BrokerKarkardRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   BrokerKarkardRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BrokerKarkardRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerWithoutGoodRpt")]
        public async Task<IActionResult> CustomerWithoutGoodRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerWithoutGoodRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerWithoutGoodRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodInStackVerticalRpt")]
        public async Task<IActionResult> GoodInStackVerticalRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodInStackVerticalRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodInStackVerticalRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodSerialHistoryRpt")]
        public async Task<IActionResult> GoodSerialHistoryRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodSerialHistoryRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodSerialHistoryRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodSerialAmountControlRpt")]
        public async Task<IActionResult> GoodSerialAmountControlRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodSerialAmountControlRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodSerialAmountControlRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("HesabPeriodicForCustomerAndVendorRpt")]
        public async Task<IActionResult> HesabPeriodicForCustomerAndVendorRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   HesabPeriodicForCustomerAndVendorRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(HesabPeriodicForCustomerAndVendorRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("StackSellRpt")]
        public async Task<IActionResult> StackSellRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   StackSellRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(StackSellRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("StackSellWithGoodGroupingRpt")]
        public async Task<IActionResult> StackSellWithGoodGroupingRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   StackSellWithGoodGroupingRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(StackSellWithGoodGroupingRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("TotalSellSanadTypeRpt")]
        public async Task<IActionResult> TotalSellSanadTypeRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   TotalSellSanadTypeRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(TotalSellSanadTypeRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodSerialsRpt")]
        public async Task<IActionResult> GoodSerialsRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodSerialsRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodSerialsRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodSellSerialsRpt")]
        public async Task<IActionResult> GoodSellSerialsRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodSellSerialsRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodSellSerialsRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("SellStackGroupingRpt")]
        public async Task<IActionResult> SellStackGroupingRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   SellStackGroupingRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SellStackGroupingRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerReceiveMablaghRpt")]
        public async Task<IActionResult> CustomerReceiveMablaghRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerReceiveMablaghRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerReceiveMablaghRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("LogHistoryByFilterRpt")]
        public async Task<IActionResult> LogHistoryByFilterRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   LogHistoryByFilterRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LogHistoryByFilterRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodCycleWithStackRpt")]
        public async Task<IActionResult> GoodCycleWithStackRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodCycleWithStackRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodCycleWithStackRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("SellUnPriceTipRpt")]
        public async Task<IActionResult> SellUnPriceTipRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   SellUnPriceTipRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SellUnPriceTipRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("PrefactorShortageRpt")]
        public async Task<IActionResult> PrefactorShortageRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   PrefactorShortageRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PrefactorShortageRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodCycleRpt")]
        public async Task<IActionResult> GoodCycleRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodCycleRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodCycleRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("FactorSellDariaftRpt")]
        public async Task<IActionResult> FactorSellDariaftRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   FactorSellDariaftRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(FactorSellDariaftRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("PeriodicCustomerDailyPurchaseCashRpt")]
        public async Task<IActionResult> PeriodicCustomerDailyPurchaseCashRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   PeriodicCustomerDailyPurchaseCashRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PeriodicCustomerDailyPurchaseCashRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("PeriodicCustomerDailyPurchaseRpt")]
        public async Task<IActionResult> PeriodicCustomerDailyPurchaseRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   PeriodicCustomerDailyPurchaseRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PeriodicCustomerDailyPurchaseRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodReceiptRpt")]
        public async Task<IActionResult> GoodReceiptRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodReceiptRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodReceiptRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodReceiptRowsRpt")]
        public async Task<IActionResult> GoodReceiptRowsRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodReceiptRowsRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodReceiptRowsRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodIssueRpt")]
        public async Task<IActionResult> GoodIssueRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodIssueRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodIssueRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodIssueRowsRpt")]
        public async Task<IActionResult> GoodIssueRowsRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodIssueRowsRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodIssueRowsRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodRevisionRpt")]
        public async Task<IActionResult> GoodRevisionRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodRevisionRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodRevisionRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("AllGoodAccountingGrpRpt")]
        public async Task<IActionResult> AllGoodAccountingGrpRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   AllGoodAccountingGrpRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(AllGoodAccountingGrpRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("GoodKardexRpt")]
        public async Task<IActionResult> GoodKardexRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   GoodKardexRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(GoodKardexRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("KardexRialyRpt")]
        public async Task<IActionResult> KardexRialyRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   KardexRialyRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(KardexRialyRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("FactorsSanadStackRpt")]
        public async Task<IActionResult> FactorsSanadStackRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   FactorsSanadStackRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(FactorsSanadStackRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerSponsorRpt")]
        public async Task<IActionResult> CustomerSponsorRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerSponsorRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerSponsorRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerDebitStateRpt")]
        public async Task<IActionResult> CustomerDebitStateRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerDebitStateRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerDebitStateRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("LastGoodSubCodeHistoryInStackRpt")]
        public async Task<IActionResult> LastGoodSubCodeHistoryInStackRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   LastGoodSubCodeHistoryInStackRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LastGoodSubCodeHistoryInStackRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("BrokerKarkardDetailRpt")]
        public async Task<IActionResult> BrokerKarkardDetailRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   BrokerKarkardDetailRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(BrokerKarkardDetailRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("TcPrintFactorRpt")]
        public async Task<IActionResult> TcPrintFactorRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   TcPrintFactorRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(TcPrintFactorRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("TcPrintFactorAttachedRpt")]
        public async Task<IActionResult> TcPrintFactorAttachedRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   TcPrintFactorAttachedRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(TcPrintFactorAttachedRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("TcPrintReturnFactorRpt")]
        public async Task<IActionResult> TcPrintReturnFactorRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   TcPrintReturnFactorRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(TcPrintReturnFactorRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("TcPrintReturnFactorAttachedRpt")]
        public async Task<IActionResult> TcPrintReturnFactorAttachedRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   TcPrintReturnFactorAttachedRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(TcPrintReturnFactorAttachedRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("TcPrintReturnPurchaseRpt")]
        public async Task<IActionResult> TcPrintReturnPurchaseRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   TcPrintReturnPurchaseRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(TcPrintReturnPurchaseRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("TcPrintReturnPurchaseAttachedRpt")]
        public async Task<IActionResult> TcPrintReturnPurchaseAttachedRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   TcPrintReturnPurchaseAttachedRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(TcPrintReturnPurchaseAttachedRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("LogHistoryByFieldRpt")]
        public async Task<IActionResult> LogHistoryByFieldRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   LogHistoryByFieldRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LogHistoryByFieldRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerVasigheRpt")]
        public async Task<IActionResult> CustomerVasigheRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerVasigheRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerVasigheRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("CustomerFactor_SanadTypeRpt")]
        public async Task<IActionResult> CustomerFactor_SanadTypeRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   CustomerFactor_SanadTypeRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(CustomerFactor_SanadTypeRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("DailyWorkRpt")]
        public async Task<IActionResult> DailyWorkRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   DailyWorkRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(DailyWorkRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("LastGoodWeightedAutomaticRpt")]
        public async Task<IActionResult> LastGoodWeightedAutomaticRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   LastGoodWeightedAutomaticRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LastGoodWeightedAutomaticRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("LastGoodWeightedRpt")]
        public async Task<IActionResult> LastGoodWeightedRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   LastGoodWeightedRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(LastGoodWeightedRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("ReminderRpt")]
        public async Task<IActionResult> ReminderRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   ReminderRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(ReminderRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("SamaneGoodInputOutputRpt")]
        public async Task<IActionResult> SamaneGoodInputOutputRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   SamaneGoodInputOutputRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SamaneGoodInputOutputRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("SamaneGoodInputOutputDetailRpt")]
        public async Task<IActionResult> SamaneGoodInputOutputDetailRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   SamaneGoodInputOutputDetailRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(SamaneGoodInputOutputDetailRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("PersonInfoSummaryRpt")]
        public async Task<IActionResult> PersonInfoSummaryRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   PersonInfoSummaryRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PersonInfoSummaryRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("PersonEtebarReceiveRpt")]
        public async Task<IActionResult> PersonEtebarReceiveRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   PersonEtebarReceiveRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PersonEtebarReceiveRpt));
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpPost]
        [Route("PersonGoodRpt")]
        public async Task<IActionResult> PersonGoodRpt([FromBody] SearchTargetReportDto searchTargetReportDto)
        {


            string ColumnsName = "";
            string XCondition = "";

            string query1 = $"Select STUFF((Select ', '+FieldName From dbo.fnGetGridSchema('T{searchTargetReportDto.ClassName}') where FieldName <> 'ksrRowNumber' And visible=1 FOR XML PATH('')), 1, 2, '') ColumnsName";


            DataTable dataTable1 = await db.Report_ExecQuery(HttpContext, query1);

            ColumnsName = dataTable1.Rows[0]["ColumnsName"] + "";




            var UserName = WebUtility.UrlDecode(HttpContext.Request.Headers["UserName"].FirstOrDefault()) ?? string.Empty;

            string query2 = $" spWeb_GetXUserReportCondition '{searchTargetReportDto.ClassName}','{UserName}'";


            DataTable dataTable2 = await db.Report_ExecQuery(HttpContext, query2);

            XCondition = " Where 1 = 1 " + dataTable2.Rows[0]["XCondition"] + "";





            string query = $"   PersonGoodRpt   ";
            try
            {
                DataTable dataTable = await db.Report_ExecQuery(HttpContext, query);
                string json = jsonClass.JsonResult_Str(dataTable, "Reports", "");
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in {Function}", nameof(PersonGoodRpt));
                return StatusCode(500, "Internal server error.");
            }
        }








        private string SanitizeInput(string input)
        {
            if (input == null)
                return string.Empty;

            // Prevent SQL Injection by replacing dangerous characters
            input = input.Replace("'", "''");  // Escape single quotes for SQL
            input = input.Replace(";", "");    // Remove semicolons
            input = input.Replace("--", "");   // Remove SQL comments
            input = input.Replace("/*", "");   // Remove SQL block comments
            input = input.Replace("*/", "");   // Remove SQL block comments

            // Prevent XSS by replacing HTML-sensitive characters with their HTML-encoded equivalents
            input = input.Replace("<", "&lt;"); // < becomes &lt;
            input = input.Replace(">", "&gt;"); // > becomes &gt;
            input = input.Replace("&", "&amp;"); // & becomes &amp;
            input = input.Replace("\"", "&quot;"); // " becomes &quot;
            input = input.Replace("'", "&#x27;"); // ' becomes &#x27;
            input = input.Replace("/", "&#x2F;"); // / becomes &#x2F;
            input = input.Replace("\\", "&#x5C;"); // \ becomes &#x5C;

            // Remove leading/trailing whitespace
            input = input.Trim();

            return input;
        }





    }
}





