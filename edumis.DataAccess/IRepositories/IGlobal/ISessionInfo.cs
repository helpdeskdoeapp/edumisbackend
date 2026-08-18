using edumis.Models.Global;
using edumis.Models.Global.DTO;

namespace edumis.DataAccess.IRepositories.IGlobal;

public interface ISessionInfo : IRepository<SessionInfoModel>
{
    Task<bool> Update(SessionInfoDTO sessiondetails);
}
