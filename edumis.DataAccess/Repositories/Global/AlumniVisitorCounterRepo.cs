using edumis.DataAccess.IRepositories.IGlobal;
using edumis.Models.Global;

namespace edumis.DataAccess.Repositories.Global;

internal class AlumniVisitorCounterRepo(ApplicationDBContext context) : 
    Repository<AlumniVisitorCounterModel>(context), IAlumniVisitorCounterRepo
{
}
