using edumis.DataAccess.IRepositories.IGlobal;
using edumis.Models.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edumis.DataAccess.Repositories.Global
{
    internal class VisitorCounterRepo(ApplicationDBContext context) : Repository<VisitorCounterModel>(context), IVisitorCounterRepo
    {
        
    }
}
