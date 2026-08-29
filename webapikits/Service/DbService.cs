using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Reflection.PortableExecutable;

public class DbService : IDbService
{
    private readonly IConfiguration _configuration;

    public DbService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<DataTable> Web_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Web_Connection", query, parameters);

    public async Task<DataTable> Kowsar_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Kowsar_Connection", query, parameters);


    public async Task<DataTable> SupportApp_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "SupportApp_Connection", query, parameters);

    public async Task<DataTable> Broker_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Broker_Connection", query, parameters);

    public async Task<DataTable> Ocr_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Ocr_Connection", query, parameters);

    public async Task<DataTable> Order_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Order_Connection", query, parameters);

    public async Task<DataTable> Company_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Company_Connection", query, parameters);

    public async Task<DataTable> Kits_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Kits_Connection", query, parameters);

    public async Task<DataTable> Auth_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "KowsarIdentityDb", query, parameters);

    public async Task<DataTable> Image_ExecQuery(HttpContext? context, string query, Dictionary<string, object>? parameters = null)
=> await ExecuteQueryAsync(context, "ImageConnection", query, parameters);

    public async Task<DataTable> Report_ExecQuery(HttpContext? context, string query, Dictionary<string, object>? parameters = null)
=> await ExecuteQueryAsync(context, "ReportConnection", query, parameters);
    public async Task<DataTable> Event_ExecQuery(HttpContext? context, string query, Dictionary<string, object>? parameters = null)
=> await ExecuteQueryAsync(context, "EventConnection", query, parameters);

    


    public async Task<DataTable> Wedding_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Wedding_Connection", query, parameters);

    public async Task<DataTable> ExecSearchQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Search_Connection", query, parameters);

    public async Task<DataTable> ExecWebQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Web_Connection", query, parameters);

    public async Task<DataTable> ExecKowsarQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Kowsar_Connection", query, parameters);

    public async Task<DataTable> ExecSupportQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Support_Connection", query, parameters);

    public async Task<DataTable> ExecSupportAppQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "SupportApp_Connection", query, parameters);

    public async Task<DataTable> ExecBrokerQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Broker_Connection", query, parameters);

    public async Task<DataTable> ExecOcrQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Ocr_Connection", query, parameters);

    public async Task<DataTable> ExecOrderQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Order_Connection", query, parameters);

    public async Task<DataTable> ExecCompanyQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Company_Connection", query, parameters);

    public async Task<DataTable> ExecKitsQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null)
        => await ExecuteQueryAsync(context, "Kits_Connection", query, parameters);

    public async Task<byte[]?> GetImageDataAsync(string query)
    {
        var dt = await RunQueryAsync(_configuration.GetConnectionString("ImageConnection"), query);
        if (dt.Rows.Count > 0 && !Convert.IsDBNull(dt.Rows[0]["IMG"]))
        {
            return (byte[])dt.Rows[0]["IMG"];
        }

        return null;
    }

    public async Task<byte[]?> Web_GetImageData(string query)
    {
        var dt = await RunQueryAsync(_configuration.GetConnectionString("ImageConnection"), query);
        if (dt.Rows.Count > 0 && !Convert.IsDBNull(dt.Rows[0]["IMG"]))
        {
            return (byte[])dt.Rows[0]["IMG"];
        }

        return null;
    }

    

    private async Task<DataTable> ExecuteQueryAsync(HttpContext? context, string connKey, string query, Dictionary<string, object>? parameters)
    {
        if (context.Request.Path != "/api/Web/GetWebLog")
        {
            await LogQueryAsync(context, query, parameters);
        }

        return await RunQueryAsync(_configuration.GetConnectionString(connKey), query, parameters);
    }

    private async Task<DataTable> RunQueryAsync(string connectionString, string query, Dictionary<string, object>? parameters = null)
    {
        var dt = new DataTable();

        using var con = new SqlConnection(connectionString);
        using var cmd = new SqlCommand(query, con);

        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }
        }

        using var adapter = new SqlDataAdapter(cmd);
        await Task.Run(() => adapter.Fill(dt));

        return dt;
    }

    private async Task LogQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null)
    {
        var agent = context.Request.Headers["User-Agent"].FirstOrDefault() ?? string.Empty;
        var PersonInfoRef = context.Request.Headers["PIC"].FirstOrDefault() ?? string.Empty;

        var UserName = WebUtility.UrlDecode(context.Request.Headers["UN"].FirstOrDefault()) ?? string.Empty;



        var SessionId = context.Request.Headers["SI"].FirstOrDefault() ?? string.Empty;

        if (SessionId.Length > 0)
        {
            await XUserSession_Insert(context, query, parameters);
        }
        else
        {
            await XUserSession_Update(context, query, parameters);
        }


        var referer = context.Request.Headers["Referer"].FirstOrDefault() ?? string.Empty;

        query = query.Replace("'", "''");





        // تبدیل پارامترها به string
        string tagName = "";
        if (parameters != null && parameters.Any())
        {
            var paramList = parameters.Select(kv => $"{kv.Key}={kv.Value?.ToString()?.Replace("'", "''")}");
            tagName = string.Join("; ", paramList);
        }

        // escape کردن tagName
        tagName = tagName.Replace("'", "''");

        var logQuery = $@"
        exec spWeb_LogInsert 
            @ClassName = '{context.Request.Path}',
            @TagName = '{tagName}',
            @LogValue = '{query}',
            @IpAddress = '{referer}',
            @UserAgent = '{agent}',
            @SessionId = '{SessionId}',
            @UserName = '{UserName}',
            @PersonInfoRef = '{PersonInfoRef}'
            "




            ;

        var logConn = _configuration.GetConnectionString("Web_Connection");

        using var con = new SqlConnection(logConn);
        using var cmd = new SqlCommand(logQuery, con);
        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }



    private async Task XUserSession_Insert(HttpContext context, string query, Dictionary<string, object>? parameters = null)
    {

        var UserName = WebUtility.UrlDecode(context.Request.Headers["UN"].FirstOrDefault()) ?? string.Empty;
        var IpAddress_referer = context.Request.Headers["Referer"].FirstOrDefault() ?? string.Empty;
        var agent = context.Request.Headers["User-Agent"].FirstOrDefault() ?? string.Empty;
        var SessionId = context.Request.Headers["SI"].FirstOrDefault() ?? string.Empty;

        query = query.Replace("'", "''");

        // تبدیل پارامترها به string
        string tagName = "";
        if (parameters != null && parameters.Any())
        {
            var paramList = parameters.Select(kv => $"{kv.Key}={kv.Value?.ToString()?.Replace("'", "''")}");
            tagName = string.Join("; ", paramList);
        }

        // escape کردن tagName
        tagName = tagName.Replace("'", "''");

        var logQuery = $@"
        exec  spWeb_XUserSession_Insert 
            @UserName = '{UserName}',
            @IpAddress = '{IpAddress_referer}',
            @UserAgent = '{agent}',
            @SessionId = '{SessionId}'
            "

            ;

        var logConn = _configuration.GetConnectionString("Web_Connection");

        using var con = new SqlConnection(logConn);
        using var cmd = new SqlCommand(logQuery, con);
        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private async Task XUserSession_Update(HttpContext context, string query, Dictionary<string, object>? parameters = null)
    {

        var SessionId = context.Request.Headers["SI"].FirstOrDefault() ?? string.Empty;


        var logQuery = $@" exec spWeb_XUserSession_UpdateActivity '{SessionId}'";


        var logConn = _configuration.GetConnectionString("Web_Connection");

        using var con = new SqlConnection(logConn);
        using var cmd = new SqlCommand(logQuery, con);
        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }





    public class RowLevelSecurityDto
    {
        public bool Active { get; set; } = false;
        public string WhereCondition { get; set; } = "";
    }

    public async Task<string> GetRowLevelSecurityStringAsync(
    HttpContext context,
    string className
)
    {
        int userIdRef = Convert.ToInt32(WebUtility.UrlDecode(context.Request.Headers["UI"].FirstOrDefault()) ?? "0");

        string query = $@"
    SELECT TOP 1
        Active,
        SecurityCondition
    FROM RowLevelSecurity
    WHERE UserIdRef = {userIdRef}
      AND ClassName = CONVERT(VARBINARY(MAX), N'{className}')
      AND ISNULL(Active, 0) = 1
    ORDER BY CreationDate DESC
";

        DataTable dt = await Kowsar_ExecQuery(context, query);

        if (dt.Rows.Count == 0)
            return "";

        string xml = dt.Rows[0]["SecurityCondition"]?.ToString() ?? "";

        if (string.IsNullOrWhiteSpace(xml))
            return "";

        string condition = BuildConditionFromRowLevelXml(xml);

        return string.IsNullOrWhiteSpace(condition) ? "" : condition;
    }

    public async Task<RowLevelSecurityDto> GetRowLevelSecurityConditionAsync(
        HttpContext context,
        string className
    )
    {
        int userIdRef = Convert.ToInt32(WebUtility.UrlDecode(context.Request.Headers["UI"].FirstOrDefault()) ?? "0");

        string query = $@"
        SELECT TOP 1
            Active,
            SecurityCondition
        FROM RowLevelSecurity
        WHERE UserIdRef = {userIdRef}
          AND ClassName = CONVERT(VARBINARY(MAX), N'{className}')
          AND ISNULL(Active, 0) = 1
        ORDER BY CreationDate DESC
    ";

        DataTable dt = await Kowsar_ExecQuery(context, query);

        if (dt.Rows.Count == 0)
            return new RowLevelSecurityDto();

        string xml =
            dt.Rows[0]["SecurityCondition"]?.ToString() ?? "";

        if (string.IsNullOrWhiteSpace(xml))
            return new RowLevelSecurityDto();

        string condition =
            BuildConditionFromRowLevelXml(xml);

        if (string.IsNullOrWhiteSpace(condition))
            return new RowLevelSecurityDto();



        return new RowLevelSecurityDto
        {
            Active = true,
            WhereCondition = condition
        };
    }

    private string BuildConditionFromRowLevelXml(string xml)
    {
        var doc = System.Xml.Linq.XDocument.Parse(xml);

        List<string> conditions = new();

        foreach (var node in doc.Root.Elements())
        {
            string fieldName =
                node.Element("FieldName")?.Value ?? "";

            string fieldValue =
                node.Element("FieldValue")?.Value ?? "";

            string fieldType =
                node.Element("FieldType")?.Value ?? "";

            if (string.IsNullOrWhiteSpace(fieldName))
                continue;

            if (string.IsNullOrWhiteSpace(fieldValue))
                continue;

            string cleanValue = string.Join(",",
                fieldValue
                    .Replace("@", "")
                    .Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
            );

            if (string.IsNullOrWhiteSpace(cleanValue))
                continue;

            if (fieldType.Equals("Integer", StringComparison.OrdinalIgnoreCase))
            {
                conditions.Add($"And ({fieldName} in ({cleanValue}))");
            }
            else
            {
                string quotedValues = string.Join(",",
                    cleanValue
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => $"'{x.Trim().Replace("'", "''")}'")
                );

                conditions.Add($"And ({fieldName} in ({quotedValues}))");
            }
        }

        return string.Join(" ", conditions);
    }


}
