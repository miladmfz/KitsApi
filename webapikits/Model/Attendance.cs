namespace webapikits.Model
{
    public class Attendance
    {
    }



    public class ManualAttendance
    {

        public string? ObjectCode { get; set; }
        public string? Status { get; set; }
        public string? CentralRef { get; set; }
        public string? UseTodayInstead { get; set; }
        public string? TargetDate { get; set; }

    }





    public class AttendanceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public AttendanceStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime? LastActivityAt { get; set; }
    }

    public enum AttendanceStatus
    {
        Active,
        OnBreak,
        SuspiciousInactive,
        Inactive
    }

    public class ActivityLogRequest
    {
        public int EmployeeId { get; set; }
        public string ActivityType { get; set; } // مثل: InvoiceIssued, TicketAnswered
    }



}
