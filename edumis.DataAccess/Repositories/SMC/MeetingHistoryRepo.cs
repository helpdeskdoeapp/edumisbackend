using edumis.DataAccess.IRepositories.ISMC;
using edumis.Models.SMC;

namespace edumis.DataAccess.Repositories.SMC;

internal class MeetingHistoryRepo : Repository<MeetingHistoryModel>, IMeetingHistoryRepo
{
    private readonly ApplicationDBContext dBContext;
    public MeetingHistoryRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
