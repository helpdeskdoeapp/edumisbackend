using edumis.DataAccess.IRepositories.ISMC;
using edumis.Models.SMC;

namespace edumis.DataAccess.Repositories.SMC;

internal class SMCTransactionAttachmentsRepo : Repository<SMCTransactionAttachmentsModel>, ISMCTransactionAttachmentsRepo
{
    private readonly ApplicationDBContext dBContext;
    public SMCTransactionAttachmentsRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
