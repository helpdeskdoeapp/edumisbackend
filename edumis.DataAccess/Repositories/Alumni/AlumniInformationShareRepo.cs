using edumis.DataAccess.IRepositories.IAlumni;
using edumis.Models.Alumni.Members;

namespace edumis.DataAccess.Repositories.Alumni
{
    internal class AlumniInformationShareRepo : Repository<AlumniInformationShareModel>, IAlumniInformationShareRepo
    {
        private readonly ApplicationDBContext dBContext;
        public AlumniInformationShareRepo(ApplicationDBContext dBContext) : base(dBContext)
        {
            this.dBContext = dBContext;
        }
    }
}
