using edumis.DataAccess.IRepositories.ISMC;
using edumis.Models.SMC;

namespace edumis.DataAccess.Repositories.SMC;

internal class MeetingAgendaRepo : Repository<MeetingAgendaModel>, IMeetingAgendaRepo
{
    private readonly ApplicationDBContext dBContext;
    public MeetingAgendaRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
