using edumis.Models.Global;
using edumis.Models.Global.DTO;

namespace edumis.DataAccess.IRepositories.IGlobal;

public interface ICodesRepo : IRepository<CodesModel>
{
    Task<List<CodesModel>?> GetMasterCodeDetails();
    Task<CodesModel?> GetMasterCodeDetails(int code);
    Task<List<CodesModel>?> GetMasterCodeDetails(List<int> codesList);
}
