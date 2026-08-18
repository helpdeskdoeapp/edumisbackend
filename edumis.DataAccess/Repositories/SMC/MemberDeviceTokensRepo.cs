using edumis.DataAccess.IRepositories.ISMC;
using edumis.Models.SMC;

namespace edumis.DataAccess.Repositories.SMC;

internal class MemberDeviceTokensRepo : Repository<MemberDeviceTokensModel> , IMemberDeviceTokensRepo
{
    private readonly ApplicationDBContext dBContext;
    public MemberDeviceTokensRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
