using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edumis.Models.Leave;

public enum LeaveType {
    CasualLeave = 101,
    SpecialLeave = 102,
    SpecialCasualLeave = 103,
    ChildCareLeave = 104,
    HalfPayLeave = 105,
    MaternityLeave = 106,
    PaternityLeave = 107,
    EarnedLeave = 108,
    CommutedHalfPayLeave = 109,
    ExtraordinaryLeave = 110,
    HalfCasualLeave = 111
}

public enum LeaveApplicationStatus {
    Pending=1, Approved=2, Rejected=3
}

public enum LeaveAction {
    Approve,
    Forward,
    Reject,
    Withdraw,   
}

public enum LeaveStatus{
    Approved = 501,
    Rejected = 502,
    Pending = 503,
    Withdrawn = 504,
}

public enum LeaveLevel{
    DivertedBranch = 1,
    ServiceBranch = 2,
    Zone = 3,
    District = 4,
    Region = 5,
    Goc = 6,
    HqBranch = 7
}
