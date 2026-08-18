using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using edumis.Models.Leave;

namespace edumis.DataAccess.IRepositories.ILeave;

public interface ILeaveRegisterRepo: IRepository<LeaveRegisterModel>
{
    public Task<(bool, string)> AddLeaves(string employeeId, List<SingleAddLeaveDto> leaves, string actorId, string? ip = null);
    public Task<(bool, string)> DeductLeave(string employeeId, LeaveType leaveType, int days, string actorId,  string applicationId, string? comment, string? ip=null);
}