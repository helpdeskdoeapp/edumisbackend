using edumis.DataAccess.IRepositories.IMasters;
using edumis.Models.Masters;

namespace edumis.DataAccess.Repositories.Masters;

internal class ZoneRepo: Repository<ZonesModel>, IZoneRepo
{
    private readonly ApplicationDBContext dBContext;
    public ZoneRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
