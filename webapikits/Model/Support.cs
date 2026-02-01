using System.Net;
using System.Reflection;
using System.Reflection.Emit;

namespace webapikits.Model
{
    public class Support
    {
    }
    public class WorkItemDto
    {
        public string? WorkItemCode { get; set; }
        public string? Title { get; set; }
        public string? Explain { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public string? OriginalDate { get; set; }
        public string? TargetDate { get; set; }
        public string? ChangeStateDate { get; set; }
        public string? ModuleName { get; set; }
        public string? ClassName { get; set; }
        public string? ObjectRef { get; set; }
        public string? OwnerRef { get; set; }
        public string? CreatorRef { get; set; }
        public string? CentralRef { get; set; }
        public string? SearchTarget { get; set; }

    }
    public class KowsarTaskDto
    {
        public string? TaskCode { get; set; }
        public string? TaskRef { get; set; }
        public string? Title { get; set; }
        public string? Explain { get; set; }
        public string? Flag { get; set; }
    }
    public class KowsarReportDto
    {
        public string? SearchTarget { get; set; }
        public string? CentralRef { get; set; }
        public string? LetterRowCode { get; set; }
        public string? Flag { get; set; }
        public string? DateTarget { get; set; }
        public string? StartDateTarget { get; set; }
        public string? EndDateTarget { get; set; }
        public string? CustomerRef { get; set; }


    }

    public class SupportDto
    {
        public string? DateTarget { get; set; }
        public string? BrokerCode { get; set; }
        public string? Flag { get; set; }

    }

    public class AddressDto
    {
        public string? AddressCode { get; set; }
        public string? CentralRef { get; set; }
        public string? AddressTitle { get; set; }
        public string? CityCode { get; set; }
        public string? Address { get; set; }
        public string? ZipCode { get; set; }
        public string? PostCode { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? MobileName { get; set; }

    }

}

