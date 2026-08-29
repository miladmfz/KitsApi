namespace webapikits.Model
{

    public class PersonInfoDto1
    {
        public string? LetterRef { get; set; }
        public string? CentralRef { get; set; }
        public string? ConversationText { get; set; }
    }



    public class PersonInfoDto
    {

        public string PersonInfoCode { get; set; } = "";

        public string PhFirstName { get; set; } = "";
        public string PhLastName { get; set; } = "";
        public string PhCompanyName { get; set; } = "";
        public string PhAddress1 { get; set; } = "";
        public string PhTel1 { get; set; } = "";
        public string PhMobile1 { get; set; } = "";
        public string PhEmail { get; set; } = "";
        public string CONTACTS { get; set; } = "";
        public string NumberPhone { get; set; } = "";


        public string PersonInfoRef { get; set; } = "";
        public string XUserName { get; set; } = "";
        public string XUserPass { get; set; } = "";
        public string OldPass { get; set; } = "";
        public string Active { get; set; } = "";
        public string AuthSMS { get; set; } = "";



    }

}
