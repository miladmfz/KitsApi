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
        public string UNewPass { get; set; } = "";
        public int DepartmentCode { get; set; }
    }

        public class IsUserDto
        {

            public string UName { get; set; } = "";
            public string UPass { get; set; } = "";
            public string UNewPass { get; set; } = "";

        }

        public class LoginResultDto
    {
        public int UserId { get; set; }

        public int OldUserId { get; set; }

        public int CentralRef { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }
        public string Manager { get; set; }
        public string Delegacy { get; set; }


        public int DepartmentCode { get; set; }

        public string DepartmentName { get; set; }

        public decimal UserMaxDiscount { get; set; }

        public int UserIdRef { get; set; }
        public string ActiveDate { get; set; }
        public string Name { get; set; }

        public string CentralName { get; set; }
        public string SessionId { get; set; }
    }



}

