using System.Drawing.Printing;
using System.Drawing;
using System.Media;

using Spire.Pdf;
using System.IO;
using System;

using System.Drawing.Printing;
using System;
using Spire.Pdf;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Media;

public class PdfPrinter
{

    public void PrintPdf(String FilePath,String PrinterName)
    {


        Console.WriteLine(FilePath);


        PdfDocument doc = new PdfDocument();
        doc.LoadFromFile(FilePath);
        doc.PrintSettings.PrinterName = PrinterName; 
        doc.PrintSettings.SelectPageRange(1, 5);

        doc.PrintSettings.PrintController = new StandardPrintController();
        doc.Print();


        FileManager fileManager = new ();        
        fileManager.DeletePdfFromStorage(FilePath);
        
    }





























}


