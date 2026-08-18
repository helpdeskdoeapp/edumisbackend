using System.ComponentModel.DataAnnotations;

namespace edumis.Models.SMC.DTO
{
    public record SMCMemberRequestDTO(  
        string? UniqueId,
        [Required] string Name,
        [Required] int Gender,
        [Required] int DesignationId,
        //[Required] string BranchId,
        [Required] int MemberType,        
        [Required] string MobileNo
    );

    public record SMCMemberUpdateRequestDTO(
        [Required] string MemberId,
        [Required] string UniqueId,
        [Required] string Name,
        [Required] int Gender,
        [Required] int DesignationId,
        //[Required] string BranchId,
        [Required] int MemberType,
        [Required] string MobileNo,
        [Required] bool IsActive
    );

    public record SMCMemberDetailsDTO(
        string? MemberId,
        string UniqueId,
        string Name,
        int DesignationId,
        string DesignationTitle,
        int MemberType,
        string MemberTypeDesc,
        int Gender,
        string GenderTitle,
        string MobileNo,
        string BranchId,
        string BranchName,
        bool IsActive,
        string ForSession
    );

    public class SearchSMCTeamMembers {
        public string? ForSession { get; set; }
        public int? DesignationId {  get; set; }
        public int? MemberType { get; set; }
        public string? BranchId { get; set; }
        public string? District { get; set; }
        public string? Zone { get; set; }
        public int? Gender { get; set; }
        public string? MobileNo { get; set; }
        public bool? Status { get; set; }
    }

    //public record MemberDisableRequestDTO(
    //    [Required] string MemberId        
    //);
}
