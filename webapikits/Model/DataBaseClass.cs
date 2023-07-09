using System.Data;
using System.Data.SqlClient;


namespace webapikits.Model
{
    public class DataBaseClass
    {





        public DataTable ExecQuery(String query, IConfiguration _configuration)
        {

            SqlConnection con = new SqlConnection("Data Source=.\\SQL2019;Database=Kowsardb;User Id=sa;Password=1; Integrated Security=true");
            SqlDataAdapter ad = new SqlDataAdapter(query, con);
            DataTable dataTable = new DataTable();
            DataTable dt = dataTable;
            ad.Fill(dt);


            return dt;
        }

        public DataTable ImageExecQuery(String query, IConfiguration _configuration)
        {

            SqlConnection con = new SqlConnection("Data Source=.\\SQL2019;Database=kowsarImage;User Id=sa;Password=1; Integrated Security=true");
            SqlDataAdapter ad = new SqlDataAdapter(query, con);
            DataTable dataTable = new DataTable();
            DataTable dt = dataTable;
            ad.Fill(dt);


            return dt;
        }




    }
}
