namespace webapikits.Model
{
    public class FactorRow
    {
        public string? FactorRef { get; set; }

        public string?  GoodCode { get; set; } 
        public string? GoodName { get; set; }
        public string? FactorRowCode { get; set; }
        public string? GoodRef { get; set; } 
        public string? FacAmount { get; set; } 
        public string? CanPrint { get; set; } 
        public string? RowExplain { get; set; } 
        public string? IsExtra { get; set; }
        public string? ClassName { get; set; } = "Factor";
        public string? StackRef { get; set; } = "1";
        public string? IsShopFactor { get; set; } = "0";
 
        public string? Amount { get; set; } = "0";
        public string? Price { get; set; } = "0";
        public string? MustHasAmount { get; set; } = "0";
        public string? MergeFlag { get; set; } = "1";


    }
}
