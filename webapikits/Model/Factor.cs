using System;

namespace webapikits.Model
{
	public class Factor
	{

		public string AppBasketInfoCode { get; set; }
		public string AppBasketInfoDate { get; set; }
		public string DailyCode { get; set; }
		public string MizType { get; set; }
		public string RstMizName { get; set; }
		public string InfoExplain { get; set; }
		public string FactorExplain { get; set; }
		public string TimeStart { get; set; }
		public string InfoPrintCount { get; set; }
		public string InfoState { get; set; }
		public string ReserveStart { get; set; }
		public string CustName { get; set; }
        public string FactorCode { get; set; } = "";
        public string FactorPrivateCode { get; set; } = "";
        public string AppPackCount { get; set; } = "";
        public string AppDeliverer { get; set; } = "";
        public string AppPackCounter { get; set; } = "";

    }
    public class FactorwebDto
    {
        public string? FactorRef { get; set; } = "";
        public string? FactorCode { get; set; } = "";
        public string? BrokerRef { get; set; } = "";
        public string? FactorDate { get; set; } = "";
        public string? CustomerRef { get; set; } = "";
        public string? CustomerCode { get; set; } = "";
        public string? ClassName { get; set; } = "Factor";
        public string? StackRef { get; set; } = "1";
        public string? IsShopFactor { get; set; } = "0";


        public string? StartDateTarget { get; set; } = "";
        public string? EndDateTarget { get; set; } = "";
        public string? SearchTarget { get; set; } = "";
        public string? isShopFactor { get; set; } = "";

        public string? Nvarchar15 { get; set; } = "";
        public string?   Nvarchar14 { get; set; } = "";
        public string? Nvarchar9 { get; set; } = "";
        public string? int1 { get; set; } = "";

        public string? starttime { get; set; } = "";
        public string? Endtime { get; set; } = "";
        public string? worktime { get; set; } = "";
        public string? Barbary { get; set; } = "";



        public string? AppNumber { get; set; } = "";
        public string? DatabaseNumber { get; set; } = "";
        public string?   LockNumber { get; set; } = "";


        public string?        Explain { get; set; } = "";

        public string? ObjectRef { get; set; } = "";



    }


    public class CustomerWebDto
    {


        public string AppNumber { get; set; } = "";
        public string DatabaseNumber { get; set; } = "";
        public string Delegacy { get; set; } = "";


        public string Explain { get; set; } = "";
        public string CustomerCode { get; set; } = "";

        public string ObjectRef { get; set; } = "";



    }


}
