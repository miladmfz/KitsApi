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
        public string? Explain { get; set; }



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
        public string? LeaveDays { get; set; }


    }


    public class EmployeeDto
    {
        public string? SearchTarget { get; set; }

        public string? EmployeeCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CodeMeli { get; set; }
        public string? JobTitle { get; set; }
        public string? Rozkarkard { get; set; }
        public string? NerkhHoghogh { get; set; }
        public string? NerkhSanavat { get; set; }
        public string? NerkhMaskan { get; set; }
        public string? NerkhKharobar { get; set; }
        public string? NerkhEzafekar { get; set; }
        public string? NerkhPadash { get; set; }
        public string? TedadPadash { get; set; }
        public string? NerkhExtra1 { get; set; }
        public string? TedadExtra1 { get; set; }
        public string? NerkhExtra2 { get; set; }
        public string? TedadExtra2 { get; set; }
        public string? BimePaye { get; set; }
        public string? BimeTakmili { get; set; }
        public string? Extra3 { get; set; }
        public string? Extra4 { get; set; }
        public string? SaatNaharNamaz { get; set; }
        public string? VaziyatTaahol { get; set; }
        public string? TedadOlad { get; set; }
        public string? HaghOlad { get; set; }
        public string? HaghTaahol { get; set; }
        public string? Explain { get; set; }



    }


}


