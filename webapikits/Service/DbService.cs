using System.Data;
using System.Data.SqlClient;

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


    public async Task<DataTable> Support_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null)
    => await ExecuteQueryAsync(context, "Support_Connection", query, parameters);


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

    public async Task<DataTable> Support_ImageExecQuery(string query, Dictionary<string, object>? parameters = null, HttpContext? context = null)
    => await ExecuteQueryAsync(context, "Support_ImageConnection", query, parameters);

    





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
        var agent = context.Request.Headers["User-Agent"].ToString();
        var personInfoRef = context.Request.Headers["PersonInfoRef"].ToString();
        var referer = context.Request.Headers["Referer"].ToString();
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
        exec sp_WebLogInsert 
            @ClassName = '{context.Request.Path}',
            @TagName = '{tagName}',
            @LogValue = '{query}',
            @IpAddress = '{referer}',
            @UserAgent = '{agent}',
            @SessionId = '{personInfoRef}'";

        var logConn = _configuration.GetConnectionString("Web_Connection");

        using var con = new SqlConnection(logConn);
        using var cmd = new SqlCommand(logQuery, con);
        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public Task<DataTable> Support_ImageExecQuery(string query)
    {
        throw new NotImplementedException();
    }
}
