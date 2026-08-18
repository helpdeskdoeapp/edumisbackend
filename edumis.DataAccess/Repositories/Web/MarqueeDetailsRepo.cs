using edumis.DataAccess.IRepositories.IWeb;
using edumis.Models.Web;

namespace edumis.DataAccess.Repositories.Web;

internal class MarqueeDetailsRepo(ApplicationDBContext dBContext) : Repository<MarqueeDetailsModels>(dBContext), IMarqueeDetailsRepo
{
    
}
