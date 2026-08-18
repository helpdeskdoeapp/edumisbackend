using edumis.DataAccess.IRepositories.ICommunication;
using edumis.Models.Communication;

namespace edumis.DataAccess.Repositories.Communication;

internal class OTPSentRepo : Repository<OTPSentModel>, IOTPSentRepo
{
    private readonly ApplicationDBContext dBContext;
    public OTPSentRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
