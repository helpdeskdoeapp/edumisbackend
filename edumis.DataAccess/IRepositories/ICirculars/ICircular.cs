using edumis.Models.Circulars;
using edumis.Models.Circulars.DTO;

namespace edumis.DataAccess.IRepositories.ICirculars
{
    public interface ICircular : IRepository<CircularModel>
    {
        Task<bool> Update(CircularUpdateRequestDTO circular, string fileName, string userId);
    }
}
