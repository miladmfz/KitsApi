using System.Data;

public interface IDbService
{
    Task<DataTable> ExecSearchQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> ExecWebQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> ExecKowsarQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> ExecSupportQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> ExecSupportAppQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> ExecBrokerQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> ExecOcrQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> ExecOrderQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> ExecCompanyQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> ExecKitsQueryAsync(HttpContext context, string query, Dictionary<string, object>? parameters = null);

    Task<byte[]?> GetImageDataAsync(string query);




    Task<DataTable> Web_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> Kowsar_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> Support_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> SupportApp_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> Broker_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> Ocr_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> Order_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> Company_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> Kits_ExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null);
    Task<DataTable> Support_ImageExecQuery(HttpContext context, string query, Dictionary<string, object>? parameters = null);

    Task<byte[]?> Web_GetImageData(string query);
}
