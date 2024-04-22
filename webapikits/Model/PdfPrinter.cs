using Spire.Pdf;
using System.Drawing.Printing;

public class PdfPrinter
{

    public void PrintPdf(String FilePath, String PrinterName)
    {
        
        
        Console.WriteLine(FilePath);


        PdfDocument doc = new PdfDocument();
        doc.LoadFromFile(FilePath);
        doc.PrintSettings.PrinterName = PrinterName;
        doc.PrintSettings.SelectPageRange(1, 1);
        
        doc.Print();








        FileManager fileManager = new();
        fileManager.DeletePdfFromStorage(FilePath);

        
        


    }



    public void PrintPdfs(MemoryStream ms, String PrinterName)
    {


        PdfDocument doc = new PdfDocument();
        doc.LoadFromStream(ms);
        doc.PrintSettings.PrinterName = PrinterName;
        doc.PrintSettings.SelectPageRange(1, 1);

        doc.Print();




        /*


        FileManager fileManager = new();
        fileManager.DeletePdfFromStorage(FilePath);
        */




    }
















}