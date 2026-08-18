using edumis.DataAccess.IRepositories.IMasters;
using edumis.Models.Masters;

namespace edumis.DataAccess.Repositories.Masters;

internal class DistrictRepo : Repository<DistrictsModel>, IDistrictRepo
{
    private readonly ApplicationDBContext dBContext;
    public DistrictRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
