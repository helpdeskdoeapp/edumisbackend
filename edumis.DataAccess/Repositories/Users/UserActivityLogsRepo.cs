using edumis.DataAccess.IRepositories.IUsers;
using edumis.Models.Users;

namespace edumis.DataAccess.Repositories.Users;

internal class UserActivityLogsRepo : Repository<UserActivityLogsModel>, IUserActivityLogsRepo
{
    private readonly ApplicationDBContext dBContext;
    public UserActivityLogsRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
