namespace webapikits.Model
{
    public class WorkforceAbsenceTypeDto
    {
        public string? AbsenceTypeCode { get; set; }
        public string? TypeKey { get; set; }
        public string? TypeTitle { get; set; }
        public string? CalculationMode { get; set; }
        public string? BalanceMode { get; set; }
        public string? MinimumAdvanceWorkDay { get; set; }
        public string? AllowFriday { get; set; }
        public string? AllowHoliday { get; set; }
        public string? RequireAttachment { get; set; }
        public string? AttachmentRequiredAfterDay { get; set; }
        public string? RequireDescription { get; set; }
        public string? DeductFromBalance { get; set; }
        public string? AllowFullTime { get; set; }
        public string? AllowPartTime { get; set; }
        public string? AllowShift { get; set; }
        public string? AllowCustom { get; set; }
        public string? HelpText { get; set; }
        public string? DisplayOrder { get; set; }
        public string? IsActive { get; set; }
        public string? OnlyActive { get; set; }
    }

    public class WorkforceAbsencePolicyDto
    {
        public string? PolicyCode { get; set; }
        public string? CentralRef { get; set; }
        public string? EmploymentType { get; set; }
        public string? WorkStartTime { get; set; }
        public string? WorkEndTime { get; set; }
        public string? BreakMinute { get; set; }
        public string? DailyWorkMinute { get; set; }
        public string? AnnualDailyLimit { get; set; }
        public string? MonthlyHourlyLimitMinute { get; set; }
        public string? PartTimeRatio { get; set; }
        public string? RequestSubmitStartTime { get; set; }
        public string? RequestSubmitEndTime { get; set; }
        public string? MinimumDailyAdvanceWorkDay { get; set; }
        public string? MaximumHourlyMinutePerRequest { get; set; }
        public string? ConcurrentLeaveWarningCount { get; set; }
        public string? SickAttachmentRequiredAfterDay { get; set; }
        public string? AllowDaily { get; set; }
        public string? AllowHourly { get; set; }
        public string? AllowSick { get; set; }
        public string? AllowEmergency { get; set; }
        public string? AllowHolidayRequest { get; set; }
        public string? EffectiveFromJDate { get; set; }
        public string? EffectiveToJDate { get; set; }
        public string? Explain { get; set; }
        public string? IsActive { get; set; }
        public string? OnlyActive { get; set; }
        public string? TargetJDate { get; set; }
    }

    public class WorkforceAbsenceRequestDto
    {
        public string? AbsenceRequestCode { get; set; }
        public string? CentralRef { get; set; }
        public string? AbsenceTypeKey { get; set; }
        public string? RequestMode { get; set; }
        public string? RequestJDate { get; set; }
        public string? StartJDate { get; set; }
        public string? EndJDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? TotalCalendarDay { get; set; }
        public string? TotalWorkDay { get; set; }
        public string? TotalOffDay { get; set; }
        public string? TotalMinute { get; set; }
        public string? Description { get; set; }
        public string? CurrentUserRef { get; set; }
        public string? IsManager { get; set; }
        public string? ManagerOverrideReason { get; set; }
        public string? HasAttachment { get; set; }
        public string? WorkflowStatus { get; set; }
        public string? ManagerRef { get; set; }
        public string? ManagerExplain { get; set; }
    }

    public class WorkforceAbsenceSearchDto
    {
        public string? StartJDate { get; set; }
        public string? EndJDate { get; set; }
        public string? CentralRef { get; set; }
        public string? WorkflowStatus { get; set; }
        public string? AbsenceTypeKey { get; set; }
        public string? TargetJDate { get; set; }
    }
}
