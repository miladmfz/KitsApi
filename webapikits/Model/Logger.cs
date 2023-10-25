using System;
using System.Diagnostics;
using System.IO;

public class Logger
{
    public void LogFile(IConfiguration configuration, string functionName, string query)
    {


        string dirfileName = configuration.GetConnectionString("log_SaveStorage");

        string fileName = $"{dirfileName}/{functionName}.txt";
        string strTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string headerTag = strTime + " : ";

        if (new FileInfo(fileName).Length > 100000)
        {
            File.Delete(fileName);
        }

        using (StreamWriter writer = File.AppendText(fileName))
        {

            writer.Write(headerTag);
            writer.Write("\t");
            writer.Write(query);
            writer.Write("\n");
        }
    }







}
