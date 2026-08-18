using edumis.Models.Tenders;
using edumis.Models.Tenders.DTO;

namespace edumis.DataAccess.IRepositories.ITenders
{
    public interface ITendersRepo : IRepository<TendersModel>
    {
        Task<bool> UpdateTender(long recordId, TenderUpdateRequestDTO tendersModel, string fileName, string userID);

    }
}
