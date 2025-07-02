namespace webapikits.Model
{
    public class UserDto
    {

        public string FName { get; set; } = "";
        public string LName { get; set; } = "";
        public string UName { get; set; } = "";
        public string UPass { get; set; } = "";
        public string address { get; set; } = "";
        public string mobile { get; set; } = "";
        public string company { get; set; } = "";
        public string email { get; set; } = "";
        public string Flag { get; set; } = "";
        public string NewPass { get; set; } = "";
        public string PostalCode { get; set; } = "";
    }
    public class LoginUserDto
    {

        public string UName { get; set; } = "";
        public string UPass { get; set; } = "";

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


    }
}

