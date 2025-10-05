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





}
