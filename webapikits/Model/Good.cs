namespace webapikits.Model
{
    public class Good
    {
        public string GoodCode { get; set; } 
        public string GoodName { get; set; }    
        public string GoodExplain1 { get; set; }

    }
    public class GoodDto
    {
        public string? GoodCode { get; set; }
        public string? GoodName { get; set; }
        public string? MaxSellPrice { get; set; }
        public string? GoodType { get; set; }
        public string? GoodExplain1 { get; set; }
        public string? GoodExplain2 { get; set; }
        public string? GoodExplain3 { get; set; }
        public string? GoodExplain4 { get; set; }
        public string? GoodExplain5 { get; set; }
        public string? GoodExplain6 { get; set; }


    }

    public class IsbnToBarcodeDto
    {
        public string? Isbn { get; set; }
        public string? GoodCode { get; set; }

    }

}
