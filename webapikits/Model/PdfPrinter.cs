public class PdfPrinter
{

    public void PrintPdf(String FilePath,String PrinterName)
    {
        Spire.Pdf.PdfDocument doc = new ();
        Console.WriteLine(FilePath);
        doc.LoadFromFile(FilePath);
        doc.PrintSettings.PrinterName = PrinterName; 
        doc.PrintSettings.SelectPageRange(1, 5);
        doc.Print();
        FileManager fileManager = new ();        
        fileManager.DeletePdfFromStorage(FilePath);

    }
}


