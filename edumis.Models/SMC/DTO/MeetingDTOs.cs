using System.ComponentModel.DataAnnotations;

namespace edumis.Models.SMC.DTO;

public record MeetingRequestDTO(
  // [Required] string BranchId,
   [Required] DateOnly MeetingDate,
   [Required] TimeOnly MeetingTime,
   [Required] string Title,
   //[Required] string[] Invitees,
   [Required] List<AgendaRequestDTO> Agenda,
   string? AttachmentTitle
);

public class AgendaRequestDTO
{   
    public int AgendaCode { get; set; }   
    public string? OtherDetails { get; set; } = default!;
}

public record MeetingUpdateRequestDTO(
   [Required] string MeetingId,  
   [Required] DateOnly MeetingDate,
   [Required] TimeOnly MeetingTime,
   [Required] string Title
);

public record AddMeetingAttachmentRequestDTO(
   [Required] string MeetingId,
   [Required] string AttachmentTitle
);

public class ConcludeMeetingRequestDTO {
    [Required]
    public string MeetingId { get; set; } = default!;

    [Required] 
    public string MoM_Brief { get; set; } = default!;

    [Required]
    public List<string> Attendees { get; set; } = default!;
        
    public string? AttachmentTitle { get; set; } = default!;
        
    public List<MeetingResolutionsListDTO>? MeetingResolutions { get; set; } = default!;
}

public class MeetingResolutionsListDTO {   
    public int[]? AgendaSrNo { get; set; }        
    public string Resolution { get; set; } = default!;
    public decimal? EstimatedCost { get; set; }
}

public record CloseMeetingResolutionRequestDTO(
    [Required] string ResolutionId,
    [Required] DateOnly ClosingDate,
    string? AttachmentTitle,
    string? Comments
);


public class MeetingsListDTO
{
    public Guid MeetingId { get; set; } = default!;
    public string ForSession { get; set; } = default!;
    public string BranchId { get; set; } = default!;
    public string BranchName { get; set; } = default!;
    public DateOnly MeetingDate {  get; set; }
    public TimeOnly MeetingTime { get; set; }
    public string Title { get; set; } = default!;    
    public string? Mom_Brief { get; set; }    
    public int Status { get; set; }
    public string StatusDesc { get; set; } = default!;
    public int? TotalInvitees { get; set; }
    public int? TotalAttendees {  get; set; }
    public int? TotalAgendas { get; set; }
    public int? TotalResolutions { get; set; }
}

public class MeetingDetailsDTO
{
    public Guid MeetingId { get; set; } = default!;
    public string ForSession { get; set; } = default!;
    public string BranchId { get; set; } = default!;
    public string BranchName { get; set; } = default!;
    public DateOnly MeetingDate { get; set; }
    public TimeOnly MeetingTime { get; set; }
    public string Title { get; set; } = default!;  
    public string? Mom_Brief { get; set; }
    public int Status { get; set; }
    public string StatusDesc { get; set; } = default!; 
    public List<InviteesDetailsDTO>? InviteesDetails { get; set; }
    public List<MeetingAgendaDetailDTO>? AgendaDetails { get; set; }
    public List<MeetingAttachmentsDTO>? MeetingAttachments { get; set; }
    public List<AttendeesDetailsDTO>? AttendeesDetails { get; set; }
    public List<MeetingResolutionsDTO>? MeetingResolutions { get; set; }  
}

public class InviteesDetailsDTO
{
    public Guid MemberId { get; set; } = default!;
    public string MemberName { get; set; } = default!;
    public int MemberType { get; set; }
    public string MemberTypeDesc { get; set; } = default!;
    public string UniqueId { get; set; } = default!;
    public string MobileNo { get; set; } = default!;
    public int? DesignationId { get; set; }
    public string? DesignationTitle { get; set; }
}

public class AttendeesDetailsDTO
{
    public Guid MemberId { get; set; } = default!;
    public string MemberName { get; set; } = default!;
    public int MemberType { get; set; }
    public string MemberTypeDesc { get; set; } = default!;
    public string UniqueId { get; set; } = default!;
    public string MobileNo { get; set; } = default!;
    public int? DesignationId { get; set; }
    public string? DesignationTitle { get; set; }
}

public class MeetingResolutionsDTO
{
    public Guid ResolutionId { get; set; }
    public int[]? AgendaSrNo { get; set; }
    public string Resolution { get; set; } = default!;
    public bool? IsClosed { get; set; }
    public DateOnly? ClosingDate { get; set; }
    public string? Comments { get; set; } = default!;
    public decimal? EstimatedCost { get; set; }
}

public class MeetingAgendaDetailDTO
{
    public Guid MeetingId { get; set; } 
    public int SerialNo { get; set; }
    public int AgendaCode { get; set; }
    public string AgendaTitle { get; set; } = default!;
    public string? OtherDetails { get; set; } = default!;
}

public class MeetingAttachmentsDTO
{
    public Guid MeetingId { get; set; }
    public int SerialNo { get; set; }
    public string? Title { get; set; } = default!;
    public string? FileName { get; set; } = default!;
    public string? FileURL { get; set; } = default!;
    public string? FileExtension { get; set; }
}