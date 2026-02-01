using System.Windows.Forms;

namespace webapikits.Model
{

    public class ConditionDto
    {

        public string? SearchTarget { get; set; }
        public string? SourceFlag { get; set; }
    }
    public class SearchTargetDto
    {

        public string? SearchTarget { get; set; } = "";
        public string? ObjectRef { get; set; } = "0";
        public string? ClassName { get; set; } = "";
        public string? BrokerRef { get; set; } = "";


    }


    public class SearchTargetLetterDto
    {

        public string? SearchTarget { get; set; }
        public string? PersonInfoCode { get; set; }
        public string? CentralRef { get; set; }
        public string? CreationDate { get; set; }
        public string? OwnCentralRef { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? Flag { get; set; }
    }



           
    public class DbSetupDto
    {

        public string? KeyId { get; set; }
        public string? KeyValue { get; set; }
        public string? DataValue { get; set; }
        public string? Description { get; set; }
        public string? SubSystem { get; set; }
    }

    public class SearchTargetReportDto
    {

        public string? SearchTarget { get; set; } = "";
        public string? ObjectRef { get; set; } = "0";
        public string? ClassName { get; set; } = "";
        public string? BrokerRef { get; set; } = "";

        /// <summary>
        ///             All
        /// </summary>
        public string? FromDate { get; set; } = "";
        public string? ToDate { get; set; } = "";
        public string? Department { get; set; } = "";
        public string? WhereCluase { get; set; } = "";
        public string? OrderBy { get; set; } = "";
        public string? Column { get; set; } = "";




        /// <summary>
        ///             GoodForosh
        /// </summary>
        public string? FromTime { get; set; } = "";
        public string? ToTime { get; set; } = "";
        public string? StackRef { get; set; } = "";
        public string? GoodTableName { get; set; } = "";
        public string? GoodFieldName { get; set; } = "";
        public string? DamagedGoodsStackCode { get; set; } = "";
        public string? GroupByField { get; set; } = "";









    }



}
