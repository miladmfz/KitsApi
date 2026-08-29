using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using webapikits.Model;

namespace webapikits.Controllers.InternalWebApi
{

    [Route("api/[controller]")]
    [ApiController]

    public class SupportController : ControllerBase
    {


        JsonClass jsonClass = new JsonClass();

        private readonly IDbService db;
        private readonly IJsonFormatter _jsonFormatter1;
        private readonly ILogger<SupportController> _logger;
        private readonly IConfiguration _configuration;
        public SupportController(
            IDbService dbService,
            IJsonFormatter jsonFormatter,
            ILogger<SupportController> logger,
            IConfiguration configuration
            )
        {
            db = dbService;
            _jsonFormatter1 = jsonFormatter;
            _logger = logger;
            _configuration = configuration;
        }





    }
}
