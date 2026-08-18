using edumis.DataAccess.IRepositories.ILeave;
using edumis.Models.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edumis.DataAccess.Repositories.Leave;

public class LeaveApplicationTrackRepo(ApplicationDBContext dbCOntext): Repository<LeaveApplicationTrackModel>(dbCOntext), ILeaveApplicationTrackRepo
{
}
