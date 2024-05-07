namespace webapikits.Model
{
    public class Printer
    {

        public string AppPrinterCode { get; set; }
        public string PrinterName { get; set; }
        public string PrinterExplain { get; set; }
        public string GoodGroups { get; set; }
        public string WhereClause { get; set; }
        public string PrintCount { get; set; }
        public string PrinterActive { get; set; }
        public string FilePath { get; set; }
        public string AppType { get; set; }




    }
    public class AppPrinterDto
    {
        public string? AppPrinterCode { get; set; }
        public string? PrinterName { get; set; }
        public string? PrinterExplain { get; set; }
        public string? GoodGroups { get; set; }
        public string? WhereClause { get; set; }
        public string? PrintCount { get; set; }
        public string? PrinterActive { get; set; }
        public string? FilePath { get; set; }
        public string? AppType { get; set; }
    }



}

