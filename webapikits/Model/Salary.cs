namespace webapikits.Model
{


    public class MonthSummaryDto
    {
        public string? MonthSummaryCode { get; set; }
        public string? MonthSummaryRef { get; set; }
        public string? EmployeCode { get; set; }

        public string? Sal { get; set; } = "0";
        public string? Mah{ get; set; } = "0";
        public string? TotalDays { get; set; }
        public string? HolidayDays { get; set; }
        public string? SearchTarget { get; set; }



    }


    public class SalarySummaryDto
    {
        public string? SalarySummaryCode { get; set; }
        public string? MonthSummaryRef { get; set; }
        public string? EmployeRef { get; set; }
        public string? EmployeCode { get; set; }

        public string? WorkingHours { get; set; }
        public string? LeaveHours { get; set; }
        public string? OvertimeHours { get; set; }
        public string? Bonus { get; set; }
        public string? Deduction1 { get; set; }
        public string? Deduction2 { get; set; }
        public string? SearchTarget { get; set; }
        public string? Sal { get; set; }
        public string? Mah { get; set; }


    }


    public class EmployeDto
    {
        public string? EmployeCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CodeMeli { get; set; }
        public string? JobTitle { get; set; }
        public string? Rozkarkard { get; set; }
        public string? HoghoghRozane { get; set; }
        public string? SanavatRozane { get; set; }
        public string? HaghMaskanRozane { get; set; }
        public string? HaghKharobarRozane { get; set; }
        public string? EzafekarSaati { get; set; }
        public string? SaatNaharNamaz { get; set; }
        public string? VaziyatTaahol { get; set; }
        public string? TedadOlad { get; set; }
        public string? HaghOlad { get; set; }
        public string? HaghTaahol { get; set; }
        public string? WorkingHoursMinistry { get; set; }
        public string? SearchTarget { get; set; }



    }


}


