using edumis.DataAccess.IRepositories.ISMC;
using edumis.Models.SMC;

namespace edumis.DataAccess.Repositories.SMC;

internal class MeetingAttachmentsRepo : Repository<MeetingAttachmentsModel>, IMeetingAttachmentsRepo
{
    private readonly ApplicationDBContext dBContext;
    public MeetingAttachmentsRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
