namespace webapikits.Model
{
    public class BrokerApi
    {
    }



        public class BrokerOrderClass
        {
            public List<HeaderDetail> HeaderDetails { get; set; }
            public List<RowDetail> RowDetails { get; set; }
        }

        public class HeaderDetail
        {
            public string PreFactorCode { get; set; }
            public string PreFactorDate { get; set; }
            public string PreFactorExplain { get; set; }
            public string CustomerRef { get; set; }
            public string BrokerRef { get; set; }
            public string RwCount { get; set; }
        }

        public class RowDetail
        {
            public string GoodRef { get; set; }
            public string FactorAmount { get; set; }
            public string Price { get; set; }
        }

        public class GpsLocation
        {
            public String Longitude { get; set; }
            public String Latitude { get; set; }
            public String BrokerRef { get; set; }
            public String GpsDate { get; set; }
        }

        public class PrintRequest
        {
            public string Image { get; set; }
            public string Code { get; set; }
            public string PrinterName { get; set; }
            public int PrintCount { get; set; }
        }


    
}
