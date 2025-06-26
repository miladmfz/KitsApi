using System.Data;

public interface IJsonFormatter
{
    string JsonResult_Str(DataTable dataTable, string keyResponse, string? textValue = null);

    string ConvertDataTableToJson(DataTable dataTable);

    DataTable ConvertJsonToDataTable(string json);

    List<Dictionary<string, object>> ConvertJsonToDictionaryList(string json);

    DataTable ConvertDictionaryListToDataTable(List<Dictionary<string, object>> dictionaryList);

    string ConvertImageToBase64(DataTable dataTable);

    string ConvertAndScaleImageToBase64(int targetSize, DataTable dataTable);
    string JsonResultWithout_Str(DataTable dataTable);
}