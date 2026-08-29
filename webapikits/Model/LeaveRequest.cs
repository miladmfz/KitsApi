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
        public string? IgnoreLeaveBalance { get; set; }
        public string? LimitDay { get; set; }
        public string? LimitMinute { get; set; }


    }
    public class LeaveRequestUserPolicyDto
    {
        public string? PolicyCode { get; set; }
        public string? CentralRef { get; set; }

        public string? EmploymentType { get; set; }

        public string? WorkStartTime { get; set; }
        public string? WorkEndTime { get; set; }

        public string? BreakMinute { get; set; }
        public string? DailyWorkMinute { get; set; }

        public string? AnnualLeaveLimitDay { get; set; }
        public string? MonthlyHourlyLeaveLimitMinute { get; set; }
        public string? PartTimeRatio { get; set; }

        public string? RequestSubmitStartTime { get; set; }
        public string? RequestSubmitEndTime { get; set; }

        public string? AllowDailyLeave { get; set; }
        public string? AllowHourlyLeave { get; set; }
        public string? AllowSickLeave { get; set; }

        public string? IsActive { get; set; }

        public string? EffectiveFromJDate { get; set; }
        public string? EffectiveToJDate { get; set; }

        public string? Explain { get; set; }

        public string? OnlyActive { get; set; }
    }
}
