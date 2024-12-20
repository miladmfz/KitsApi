using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace webapikits.Model
{
    public class HelpFunction
    {
        /*
        string sanitizedInput = HelpFunction.SanitizeInput("some input");
        SqlParameter param = HelpFunction.CreateSqlParameter("@param", 123);
        */




        public static string SanitizeInput(string input)
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

        public static SqlParameter CreateSqlParameter(string parameterName, object value)
        {
            if (value == null)
            {
                return new SqlParameter(parameterName, DBNull.Value);
            }

            SqlDbType dbType;

            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.String:
                    dbType = SqlDbType.NVarChar;
                    break;
                case TypeCode.Int32:
                    dbType = SqlDbType.Int;
                    break;
                case TypeCode.Int64:
                    dbType = SqlDbType.BigInt;
                    break;
                case TypeCode.Decimal:
                    dbType = SqlDbType.Decimal;
                    break;
                case TypeCode.Double:
                    dbType = SqlDbType.Float;
                    break;
                case TypeCode.Boolean:
                    dbType = SqlDbType.Bit;
                    break;
                case TypeCode.DateTime:
                    dbType = SqlDbType.DateTime;
                    break;
                default:
                    dbType = SqlDbType.Variant; // For unhandled types
                    break;
            }

            return new SqlParameter(parameterName, dbType) { Value = value };
        }

        /*

        [HttpGet]
        [Route("GetBarcodeList1")]
        public string GetBarcodeList1(string Where, int SomeInt, double SomeDouble)
        {
            string query = "SELECT BarCodeId, GoodRef, BarCode FROM Barcode WHERE GoodRef = @GoodRef AND SomeInt = @SomeInt AND SomeDouble = @SomeDouble";

            var parameters = new SqlParameter[]
            {
        CreateSqlParameter("@GoodRef", Where),
        CreateSqlParameter("@SomeInt", SomeInt),
        CreateSqlParameter("@SomeDouble", SomeDouble)
            };

            DataTable dataTable = db.Kowsar_ExecQuery1(HttpContext, query, parameters);
            return jsonClass.JsonResult_Str(dataTable, "Barcodes", "");
        }
        */

    }
}
