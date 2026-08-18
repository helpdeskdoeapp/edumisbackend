using edumis.Models.Masters;
using edumis.Models.Masters.DTO;

namespace edumis.DataAccess.IRepositories.IMasters
{
    public interface IInfrastructureRepo : IRepository<InfrastructureModel>
    {
        Task<string> CreateOrUpdate(InfrastructureDTO infrastructureDetails, bool CreateNew = false);
        Task<List<InfrastructureDetailsDTO>> GetAllInfra();
    }
}
