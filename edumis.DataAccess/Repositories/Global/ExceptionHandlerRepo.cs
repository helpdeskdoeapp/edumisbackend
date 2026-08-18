using edumis.DataAccess.IRepositories.IGlobal;
using edumis.Models.Global;

namespace edumis.DataAccess.Repositories.Global;

internal class ExceptionHandlerRepo : Repository<ExceptionLogs>, IExceptionHandlerRepo
{
    private readonly ApplicationDBContext dBContext;
    public ExceptionHandlerRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
