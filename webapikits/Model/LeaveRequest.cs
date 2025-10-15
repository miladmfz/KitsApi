namespace webapikits.Model
{
    public class LeaveRequestDto
    {
        public string? LeaveRequestCode { get; set; }
        public string? UserRef { get; set; }
        public string? LeaveRequestType { get; set; }
        public string? LeaveRequestExplain { get; set; }


        public string? TotalDay { get; set; }
        public string? WorkDay { get; set; }
        public string? OffDay { get; set; }

        public string? LeaveStartDate { get; set; }
        public string? LeaveEndDate { get; set; }
        public string? LeaveStartTime { get; set; }
        public string? LeaveEndTime { get; set; }
        public string? ManagerRef { get; set; } 
        public string? WorkFlowStatus { get; set; }
        public string? WorkFlowExplain { get; set; }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }


    }

}
