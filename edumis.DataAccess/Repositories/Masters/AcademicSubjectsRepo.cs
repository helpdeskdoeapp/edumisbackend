using edumis.DataAccess.IRepositories.IMasters;
using edumis.Models.Masters;

namespace edumis.DataAccess.Repositories.Masters;

internal class AcademicSubjectsRepo(ApplicationDBContext dBContext) :
    Repository<AcademicSubjectsModel>(dBContext), IAcademicSubjectsRepo
{
}
