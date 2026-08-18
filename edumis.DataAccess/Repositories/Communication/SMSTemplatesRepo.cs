using edumis.DataAccess.IRepositories.ICommunication;
using edumis.Models.Communication;

namespace edumis.DataAccess.Repositories.Communication;

internal class SMSTemplatesRepo : Repository<SMSTemplatesModel>, ISMSTemplatesRepo
{
    private readonly ApplicationDBContext dBContext;
    public SMSTemplatesRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
