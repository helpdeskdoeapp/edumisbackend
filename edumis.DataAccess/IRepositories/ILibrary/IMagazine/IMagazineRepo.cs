using edumis.Models.Library.Magazine;
using edumis.Models.Library.Magazine.DTO;

namespace edumis.DataAccess.IRepositories.ILibrary.IMagazine;

public interface IMagazineRepo : IRepository<MagazineModel>
{
    Task<MagazineDetailsReponseDTO?> GetDetails(Guid magazineId);
    Task<IEnumerable<MagazineDetailsReponseDTO>?> GetMagazines(string branchId);
}
