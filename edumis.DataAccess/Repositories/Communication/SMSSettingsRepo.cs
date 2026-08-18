using edumis.DataAccess.IRepositories.ICommunication;
using edumis.Models.Communication;

namespace edumis.DataAccess.Repositories.Communication;

internal class SMSSettingsRepo : Repository<SMSSettingsModel>, ISMSSettingsRepo
{
    private readonly ApplicationDBContext dBContext;
    public SMSSettingsRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
