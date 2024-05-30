namespace webapikits.Model
{
    public class AutLetterRowInsert
    {
        public string LetterRef { get; set; } = "";
        public string LetterDate { get; set; } = "";
        public string Description { get; set; } = "";
        public string CreatorCentral { get; set; } = "";
        public string ExecuterCentral { get; set; } = "";

    }
    public class LetterInsert
    {
        public string LetterDate { get; set; } = "";
        public string title { get; set; } = "";
        public string Description { get; set; } = "";
        public string CentralRef { get; set; } = "";

    }


}
