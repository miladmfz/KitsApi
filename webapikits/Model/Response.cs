namespace webapikits.Model
{
    public class Response
    {
        public string StatusCode { get; set; }
        
        public string Errormessage { get; set; }
    }


    public class LogReportDto
    {
        public string Device_Id { get; set; }

        public string Address_Ip { get; set; }
        public string Server_Name { get; set; }
        public string Factor_Code { get; set; }
        public string StrDate { get; set; }
        public string Broker { get; set; }
        public string Explain { get; set; }
    }


}
