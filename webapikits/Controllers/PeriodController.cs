using System.Data;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapikits.Model;

namespace webapikits.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class PeriodController : ControllerBase
    {




        public readonly IConfiguration _configuration;

        DataBaseClass db;

        JsonClass jsonClass = new JsonClass();




        public PeriodController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }


        [HttpGet]
        [Route("EvaluationperiodGet")]
        public string EvaluationperiodGet(string PeriodId)
        {

            string query = $"Exec sp_EvaluationperiodGet {PeriodId} ";

            DataTable dataTable = db.ExecQuery(query);
            return jsonClass.JsonResult_Str(dataTable, "Evaluationperiods", "");

        }


        [HttpGet]
        [Route("GetAllPeriod")]
        public string GetAllPeriod()
        {

            string query = $"select * from Evaluationperiod ";

            DataTable dataTable = db.ExecQuery(query);
            return jsonClass.JsonResult_Str(dataTable, "Evaluationperiods", "");

        }



        [HttpGet]
        [Route("EvaluationperiodDelete")]
        public string EvaluationperiodDelete(string PeriodId)
        {

            string query = $"Exec sp_EvaluationperiodDelete {PeriodId} ";

            DataTable dataTable = db.ExecQuery(query);
            return jsonClass.JsonResult_Str(dataTable, "Evaluationperiods", "");

        }




        [HttpGet]
        [Route("EvaluationperiodInsert")]
        public string EvaluationperiodInsert(
            string PeriodTitle,
            string StartDate,
            string EndDate,
            string FromDate,
            string ToDate,
            string Explain,
            string CreatorId
            )
        {

            string query = $"Exec sp_EvaluationperiodInsert  '{PeriodTitle}', '{StartDate}' , '{EndDate}' , '{FromDate}' , '{ToDate}' , '{Explain}' , {CreatorId}   ";

            DataTable dataTable = db.ExecQuery(query);
            return jsonClass.JsonResult_Str(dataTable, "Evaluationperiods", "");

        }



        [HttpGet]
        [Route("EvaluationperiodUpdate")]
        public string EvaluationperiodUpdate(
            string PeriodId,
            string PeriodTitle,
            string StartDate,
            string EndDate,
            string FromDate,
            string ToDate,
            string Explain,
            string CreatorId
            )
        {

            string query = $"Exec sp_EvaluationperiodUpdate  {PeriodId} ,'{PeriodTitle}', '{StartDate}' , '{EndDate}' , '{FromDate}' , '{ToDate}' , '{Explain}' , {CreatorId}   ";

            DataTable dataTable = db.ExecQuery(query);
            return jsonClass.JsonResult_Str(dataTable, "Evaluationperiods", "");

        }



    }
}
