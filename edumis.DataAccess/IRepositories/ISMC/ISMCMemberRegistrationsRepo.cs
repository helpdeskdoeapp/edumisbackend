using edumis.Models.SMC;
using edumis.Models.SMC.DTO;

namespace edumis.DataAccess.IRepositories.ISMC;

public interface ISMCMemberRegistrationsRepo : IRepository<MemberRegistrationsModel>
{
    Task<string> CreateMember(SMCMemberRequestDTO memberDetails, string BranchId);
    Task<bool> UpdateMember(SMCMemberUpdateRequestDTO memberDetails, string BranchId, string UserId);
    Task<SMCMemberDetailsDTO?> GetMemberDetails(string MemberID);    
    Task<List<SMCMemberDetailsDTO>> SearchMembers(SearchSMCTeamMembers searchEmployee);
    Task<bool> UpdateStatus(string MemberId, bool Status, string UserId);
    Task<List<SMCMemberDetailsDTO>> GetAllMembers(string MobileNo);
}
