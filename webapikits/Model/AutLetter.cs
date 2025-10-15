namespace webapikits.Model
{
    public class AutLetterRowInsert
    {
        public string LetterRef { get; set; } = "";

        public string ObjectRef { get; set; } = "";
        public string LetterDate { get; set; } = "";
        public string Description { get; set; } = "";
        public string CreatorCentral { get; set; } = "";
        public string ExecuterCentral { get; set; } = "";
        public string LetterPriority { get; set; } = "";
        public string LetterState { get; set; } = "";
        public string? LetterRowDescription { get; set; } = "";
        public string? LetterRowState { get; set; } = "";
        public string? AutLetterRow_PropDescription1 { get; set; } = "";
        


    }
    public class LetterInsert
    {
        public string LetterDate { get; set; } = "";
        public string title { get; set; } = "";
        public string InOutFlag { get; set; } = "";
        public string Description { get; set; } = "";
        public string CentralRef { get; set; } = "";
        public string LetterPriority { get; set; } = "";
        public string LetterState { get; set; } = "";
        public string OwnerCentral { get; set; } = "";
        public string CreatorCentral { get; set; } = "";
        public string OwnerPersonInfoRef { get; set; } = "";

    }

    public class LetterDto
    {
        public string? LetterRef { get; set; }
        public string? CentralRef { get; set; }
        public string? ConversationText { get; set; }
    }


    public class AlarmOffDto
    {
        public string? LetterRef { get; set; }
        public string? LetterRowCode { get; set; }
        public string? CentralRef { get; set; }
    }


}
