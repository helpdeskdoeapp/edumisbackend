using edumis.Models.Alumni.Members;
using edumis.Models.Alumni.Members.DTO;

namespace edumis.DataAccess.IRepositories.IAlumni;

public interface IAlumniDetailsRepo : IRepository<AlumniDetailsModel>
{
    Task RegisterBranchAlumni(SchoolAlumniRegistrationRequestDTO requestDTO, string registeredBy);
    Task<AlumniDetailsDTO?> GetDetails(Guid alumniId);
    Task<List<AlumniSearchResponseDTO>?> Search(AlumniSearchRequestDTO requestDTO);
    Task<List<AlumniSearchResponseDTO>?> Search(string searchText);
    Task<List<SelectedAlumniCollageDTO>?> GetCollageAlumni(int fetchRecords);
}
