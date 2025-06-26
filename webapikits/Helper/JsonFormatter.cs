using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Drawing;

public class JsonFormatter : IJsonFormatter
{

    public string JsonResultWithout_Str(DataTable dataTable)
    {
        if (dataTable == null || dataTable.Rows.Count == 0)
        {
            return "{\"response\":{\"StatusCode\":\"1000\",\"Errormessage\":\"No Data Found\"}}";
        }

        var rows = new List<Dictionary<string, object>>();

        foreach (DataRow row in dataTable.Rows)
        {
            var dict = new Dictionary<string, object>();
            foreach (DataColumn col in dataTable.Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            rows.Add(dict);
        }

        return JsonSerializer.Serialize(rows);
    }

    public string JsonResult_Str(DataTable dataTable, string keyResponse, string? textValue = null)
    {
        if (dataTable.Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(textValue))
            {
                return $"{{\"{keyResponse}\":\"{dataTable.Rows[0][textValue]}\"}}";
            }

            var rows = new List<Dictionary<string, object>>();

            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in dataTable.Columns)
                {
                    dict[col.ColumnName] = row[col] is DBNull ? null : row[col];
                }
                rows.Add(dict);
            }

            var response = new Dictionary<string, object>
            {
                { keyResponse, rows }
            };

            return JsonSerializer.Serialize(response);
        }

        if (textValue == "Done" && keyResponse == "Text")
        {
            return "{\"Text\":\"Done\"}";
        }

        return "{\"response\":{\"StatusCode\":\"1000\",\"Errormessage\":\"No Data Found\"}}";
    }

    public string ConvertDataTableToJson(DataTable dataTable)
    {
        var rows = new List<Dictionary<string, object>>();
        foreach (DataRow row in dataTable.Rows)
        {
            var dict = new Dictionary<string, object>();
            foreach (DataColumn col in dataTable.Columns)
            {
                dict[col.ColumnName] = row[col] is DBNull ? null : row[col];
            }
            rows.Add(dict);
        }
        return JsonSerializer.Serialize(rows);
    }

    public DataTable ConvertJsonToDataTable(string json)
    {
        var dataTable = new DataTable();

        if (string.IsNullOrWhiteSpace(json))
            return dataTable;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var rows = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(json, options);
        if (rows == null || rows.Count == 0)
            return dataTable;

        // Add columns
        foreach (var key in rows[0].Keys)
        {
            dataTable.Columns.Add(key, typeof(object));
        }

        // Add rows
        foreach (var row in rows)
        {
            var dataRow = dataTable.NewRow();
            foreach (var kvp in row)
            {
                dataRow[kvp.Key] = GetValueFromJsonElement(kvp.Value);
            }
            dataTable.Rows.Add(dataRow);
        }

        return dataTable;
    }

    public List<Dictionary<string, object>> ConvertJsonToDictionaryList(string json)
    {
        var dataTable = ConvertJsonToDataTable(json);
        var list = new List<Dictionary<string, object>>();

        foreach (DataRow row in dataTable.Rows)
        {
            var dict = new Dictionary<string, object>();
            foreach (DataColumn col in dataTable.Columns)
            {
                dict[col.ColumnName] = row[col] is DBNull ? null : row[col];
            }
            list.Add(dict);
        }

        return list;
    }

    public DataTable ConvertDictionaryListToDataTable(List<Dictionary<string, object>> dictionaryList)
    {
        var dataTable = new DataTable();

        if (dictionaryList == null || dictionaryList.Count == 0)
            return dataTable;

        // Add columns
        foreach (var key in dictionaryList[0].Keys)
        {
            dataTable.Columns.Add(key, typeof(object));
        }

        // Add rows
        foreach (var dict in dictionaryList)
        {
            var dataRow = dataTable.NewRow();
            foreach (var kvp in dict)
            {
                dataRow[kvp.Key] = kvp.Value ?? DBNull.Value;
            }
            dataTable.Rows.Add(dataRow);
        }

        return dataTable;
    }

    public string ConvertImageToBase64(DataTable dataTable)
    {
        if (dataTable.Rows.Count == 0)
        {
            return JsonSerializer.Serialize(new { Text = "Nophoto", ContentType = "image/jpeg" });
        }

        if (!dataTable.Columns.Contains("IMG") || dataTable.Rows[0]["IMG"] == DBNull.Value)
        {
            return JsonSerializer.Serialize(new { Text = "Nophoto", ContentType = "image/jpeg" });
        }

        byte[] imageData = (byte[])dataTable.Rows[0]["IMG"];
        string base64String = Convert.ToBase64String(imageData);
        return JsonSerializer.Serialize(new { Text = base64String, ContentType = "image/jpeg" });
    }

    public string ConvertAndScaleImageToBase64(int targetSize, DataTable dataTable)
    {
        if (dataTable.Rows.Count == 0 || !dataTable.Columns.Contains("IMG") || dataTable.Rows[0]["IMG"] == DBNull.Value)
        {
            return JsonSerializer.Serialize(new { Text = "Nophoto", ContentType = "image/jpeg" });
        }

        byte[] imageData = (byte[])dataTable.Rows[0]["IMG"];
        string base64String;

        using (var ms = new MemoryStream(imageData))
        using (var image = System.Drawing.Image.FromStream(ms))
        {
            if (Math.Max(image.Width, image.Height) > targetSize)
            {
                double scaleFactor = (double)targetSize / Math.Max(image.Width, image.Height);
                int newWidth = (int)(image.Width * scaleFactor);
                int newHeight = (int)(image.Height * scaleFactor);

                using (var scaledImage = new Bitmap(newWidth, newHeight))
                using (var g = Graphics.FromImage(scaledImage))
                {
                    g.DrawImage(image, 0, 0, newWidth, newHeight);
                    using (var scaledMs = new MemoryStream())
                    {
                        scaledImage.Save(scaledMs, System.Drawing.Imaging.ImageFormat.Jpeg);
                        base64String = Convert.ToBase64String(scaledMs.ToArray());
                    }
                }
            }
            else
            {
                base64String = Convert.ToBase64String(imageData);
            }
        }

        return JsonSerializer.Serialize(new { Text = base64String, ContentType = "image/jpeg" });
    }

    // Helper method to extract values from JsonElement
    private object? GetValueFromJsonElement(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Undefined:
            case JsonValueKind.Null:
                return null;
            case JsonValueKind.String:
                return element.GetString();
            case JsonValueKind.Number:
                if (element.TryGetInt64(out long l))
                    return l;
                if (element.TryGetDouble(out double d))
                    return d;
                return element.GetDecimal();
            case JsonValueKind.True:
            case JsonValueKind.False:
                return element.GetBoolean();
            case JsonValueKind.Object:
            case JsonValueKind.Array:
                return element.GetRawText();
            default:
                return element.GetRawText();
        }
    }
}
