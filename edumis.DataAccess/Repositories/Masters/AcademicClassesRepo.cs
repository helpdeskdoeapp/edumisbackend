using edumis.DataAccess.IRepositories.IMasters;
using edumis.Models.Masters;

namespace edumis.DataAccess.Repositories.Masters;

internal class AcademicClassesRepo(ApplicationDBContext dBContext) : 
    Repository<AcademicClassesModel>(dBContext), IAcademicClassesRepo
{
}
