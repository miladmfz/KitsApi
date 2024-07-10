using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using webapikits.Model;
using System.IO.Compression;
using System.Data.SqlClient;
using static webapikits.Controllers.OcrController;
using System.Data.Entity.Core.Objects;


namespace webapikits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactorWebController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        DataBaseClass db;
        DataTable DataTable = new DataTable();
        string Query = "";
        Response response = new();
        JsonClass jsonClass = new JsonClass();
        Dictionary<string, string> jsonDict = new Dictionary<string, string>();

        public FactorWebController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DataBaseClass(_configuration);

        }












    }
}
