namespace webapikits.Model
{
    public class WeddingEventCrudDto
    {
        public int WeddingEventId { get; set; } = 0;

        public string EventCode { get; set; } = "";

        public string GroomName { get; set; } = "";
        public string BrideName { get; set; } = "";

        public string? EventTitle { get; set; }
        public string? EventDateJalali { get; set; }
        public DateTime? EventDateTime { get; set; }

        public string? MainVenueTitle { get; set; }
        public string? MainVenueAddress { get; set; }
        public string? MainVenueMapUrl { get; set; }

        public string? InviteDefaultText { get; set; }
        public string? MixedCeremonyText { get; set; }
        public string? ChildPolicyText { get; set; }

        public bool IsActive { get; set; } = true;

        public int? Owner { get; set; }
        public int? Reformer { get; set; }
    }

    public class WeddingGuestCrudDto
    {
        public int WeddingGuestId { get; set; } = 0;

        public int WeddingEventId { get; set; }

        public string GuestCode { get; set; } = "";
        public string? GuestToken { get; set; }

        public string FullName { get; set; } = "";
        public string? DisplayName { get; set; }

        public string? Mobile { get; set; }

        public string? GuestGroup { get; set; }
        public string? RelationTitle { get; set; }

        public string? CustomInviteText { get; set; }

        public bool IsActive { get; set; } = true;

        public int? Owner { get; set; }
        public int? Reformer { get; set; }
    }

    public class WeddingGuestGetDto
    {
        public string GuestCode { get; set; } = "";
        public string? GuestToken { get; set; }
    }

    public class WeddingGuestLogDto
    {
        public string GuestCode { get; set; } = "";

        public string LogType { get; set; } = "";
        public string? PageName { get; set; }

        public string? DeviceInfo { get; set; }
        public string? ReferrerUrl { get; set; }

        public string? GuestToken { get; set; }
    }

    public class WeddingGuestResponseDto
    {
        public string GuestCode { get; set; } = "";

        /// <summary>
        /// YES / NO
        /// </summary>
        public string AttendStatus { get; set; } = "";

        public string? GuestMessage { get; set; }

        public string? GuestToken { get; set; }
    }

    public class WeddingGuestListDto
    {
        public int WeddingEventId { get; set; }

        public string? SearchText { get; set; }

        /// <summary>
        /// ALL / YES / NO / PENDING
        /// </summary>
        public string? AttendStatus { get; set; } = "ALL";

        /// <summary>
        /// ALL / SEEN / NOT_SEEN
        /// </summary>
        public string? SeenStatus { get; set; } = "ALL";
    }

    public class WeddingEventGetByIdDto
    {
        public int WeddingEventId { get; set; }
    }

    public class WeddingGuestGetByIdDto
    {
        public int WeddingGuestId { get; set; }
    }

    public class WeddingGuestLogListDto
    {
        public int WeddingGuestId { get; set; }
    }
}
