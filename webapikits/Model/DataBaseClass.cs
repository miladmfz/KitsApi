using System.Data;
using System.Data.SqlClient; 

namespace webapikits.Model
{
    public class DataBaseClass
    {
        private readonly IConfiguration _configuration;

        public DataBaseClass(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DataTable ExecQuery(String query)
        {

            Logger logger = new Logger();
            logger.LogFile(_configuration,"LOG", query); ;

            string connectionString = _configuration.GetConnectionString("DefaultConnection"); // استفاده از IConfiguration برای خواندن connection string
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                return dataTable;
            }
        }


        public DataTable ImageExecQuery(String query)
        {
            Logger logger = new Logger();
            logger.LogFile(_configuration, "LOG", query); ;
            string connectionString = _configuration.GetConnectionString("ImageConnection"); // استفاده از IConfiguration برای خواندن connection string
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                return dataTable;
            }
        }
    }
}
