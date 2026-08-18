using System.Linq.Expressions;
using edumis.DataAccess.IRepositories.ILeave;
using edumis.Models.Leave;
using edumis.Models.Masters;
using edumis.Models.Masters.DTO;
using Microsoft.EntityFrameworkCore;
using Condition = System.Linq.Expressions.Expression<System.Func<edumis.Models.Leave.LeaveApplicationModel, bool>>;

namespace edumis.DataAccess.Repositories.Leave;

public static class ConditionHelper {
    public static Condition And(this Condition left, Condition right) {
        var param = Expression.Parameter(typeof(LeaveApplicationModel));
        var body = Expression.AndAlso(
            Expression.Invoke(left, param),
            Expression.Invoke(right, param)
        );
        return Expression.Lambda<Func<LeaveApplicationModel, bool>>(body, param);
    }
}
internal class LeaveApplicationRepo(ApplicationDBContext dbContext)
    : Repository<LeaveApplicationModel>(dbContext), ILeaveApplicationRepo
{
    private readonly ApplicationDBContext dbContext = dbContext;

    public async Task<List<LeaveApplicationsAtBranchDto>> GetApplicationsAtBranch(string status, string branchId,
        BranchType branchType, string? applicationId = null) {
        if (applicationId != null) status = "all";
        
        Condition baseCondition = status switch {
            "pending" => a => a.LeaveStatus == LeaveStatus.Pending, 
            "approved" => a => a.LeaveStatus == LeaveStatus.Approved,
            "rejected" =>  a => a.LeaveStatus == LeaveStatus.Rejected,
            _ => a => applicationId==null || a.ApplicationId==applicationId
        };
        
        var diverted = CompareLevel(status, LeaveLevel.DivertedBranch).And(a => a.DivertedBranchId == branchId);
        var service = CompareLevel(status, LeaveLevel.ServiceBranch).And(a => a.ServiceBranchId == branchId);
        var super = branchType switch {
            BranchType.Zone => CompareLevel(status, LeaveLevel.Zone).And(a => a.ZoneId == branchId),
            BranchType.District => CompareLevel(status, LeaveLevel.District).And(a => a.DistrictId == branchId),
            BranchType.Region => CompareLevel(status, LeaveLevel.Region).And(a => a.RegionId == branchId),
            BranchType.Branch => CompareLevel(status, LeaveLevel.Goc).And(a => a.GocId == branchId),
            BranchType.HQ => CompareLevel(status, LeaveLevel.HqBranch).And(a => a.HqBranchId == branchId),
            _ => a => false
        };
        return await FilterAndTransform(baseCondition, diverted, service, super);
    }

    private static Condition CompareLevel(string status, LeaveLevel level) => status switch {
        "all"       => l => l.CurrentLevel >= level,
        "forwarded" => l => l.CurrentLevel > level,
        _           => l => l.CurrentLevel == level
    };

    public async Task<List<LeaveApplicationsAtEmplDto>> GetApplicationsAtEmpl(string emplId,
        string? applicationId = null)
    {
        var applications = dbContext.LeaveApplications.Where(e => e.EmployeeId == emplId);
        if (applicationId != null)
        {
            applications = applications.Where(e => e.ApplicationId == applicationId);
        }


        return await (
                from a in applications
                join e in dbContext.Employees on a.EmployeeId equals e.EmployeeId
                join p in dbContext.EmployeeAppointmentDetails on e.EmployeeId equals p.EmployeeId
                join d in dbContext.Designations on p.Designation equals d.RowId
                join b in dbContext.Branches on 
                    a.CurrentLevel == LeaveLevel.DivertedBranch ? a.DivertedBranchId :
                    a.CurrentLevel == LeaveLevel.ServiceBranch ? a.ServiceBranchId :
                    a.CurrentLevel == LeaveLevel.Zone ? a.ZoneId :
                    a.CurrentLevel == LeaveLevel.District ? a.DistrictId :
                    a.CurrentLevel == LeaveLevel.Region ? a.RegionId :
                    a.CurrentLevel == LeaveLevel.Goc ? a.GocId :
                    a.CurrentLevel == LeaveLevel.HqBranch ? a.HqBranchId :
                    null 
                    equals b.BranchId
                join t in dbContext.LeaveApplicationTrack on a.ApplicationId equals t.ApplicationId into ts
                let latestTrack = ts
                    .OrderByDescending(x => x.ActionAt) 
                    .FirstOrDefault()
                select new LeaveApplicationsAtEmplDto
                {
                    ApplicationId = a.ApplicationId,
                    EmployeeId = a.EmployeeId,
                    AppliedAt = a.AppliedAt,
                    LeaveType = (LeaveType)a.LeaveType,
                    Days = a.Days,
                    FromDate = a.FromDate,
                    ToDate = a.ToDate,
                    LeaveStation = a.LeaveStation,
                    LeaveStatus = a.LeaveStatus,
                    EmployeeName = e.Name,
                    Designation = d.Title,
                    Reason = a.Reason,
                    AddressDuringLeave = a.AddressDuringLeave,
                    LeaveWithNoc = a.LeaveWithNoc,
                    ChildDob = a.ChildDob,
                    ActionBranch = new BranchesNamesDTO
                    {
                        BranchId = b.BranchId,
                        BranchName = b.BranchName,
                        BranchType = b.BranchType,
                    },
                    LastActionDate = latestTrack != null? latestTrack.ActionAt : a.UpdatedAt
                }
            )
            .OrderByDescending(x => x.FromDate)
            .ToListAsync();
    }

    private async Task<List<LeaveApplicationsAtBranchDto>> FilterAndTransform(Condition baseCondition,
        Condition diverted, Condition service, Condition super)
    {
        var applications = dbContext.LeaveApplications.Where(baseCondition);
        var baseApps = applications.Where(diverted).Select(a => new { App = a, Source = "Diverted" })
            .Concat(applications.Where(service).Select(a => new { App = a, Source = "Service" }))
            .Concat(applications.Where(super).Select(a => new { App = a, Source = "Super" }));

        return await (
                from a in baseApps
                join e in dbContext.Employees on a.App.EmployeeId equals e.EmployeeId
                join p in dbContext.EmployeeAppointmentDetails on e.EmployeeId equals p.EmployeeId
                join d in dbContext.Designations on p.Designation equals d.RowId
                join b in dbContext.Branches on 
                    a.App.CurrentLevel == LeaveLevel.DivertedBranch ? a.App.DivertedBranchId :
                    a.App.CurrentLevel == LeaveLevel.ServiceBranch ? a.App.ServiceBranchId :
                    a.App.CurrentLevel == LeaveLevel.Zone ? a.App.ZoneId :
                    a.App.CurrentLevel == LeaveLevel.District ? a.App.DistrictId :
                    a.App.CurrentLevel == LeaveLevel.Region ? a.App.RegionId :
                    a.App.CurrentLevel == LeaveLevel.Goc ? a.App.GocId :
                    a.App.CurrentLevel == LeaveLevel.HqBranch ? a.App.HqBranchId :
                    null 
                    equals b.BranchId
                join t in dbContext.LeaveApplicationTrack on a.App.ApplicationId equals t.ApplicationId into ts
                let latestTrack = ts
                    .OrderByDescending(x => x.ActionAt) 
                    .FirstOrDefault()

                select new LeaveApplicationsAtBranchDto
                {
                    ApplicationId = a.App.ApplicationId,
                    EmployeeId = a.App.EmployeeId,
                    AppliedAt = a.App.AppliedAt,
                    LeaveType = (LeaveType)a.App.LeaveType,
                    Days = a.App.Days,
                    FromDate = a.App.FromDate,
                    ToDate = a.App.ToDate,
                    LeaveStation = a.App.LeaveStation,
                    LeaveStatus = a.App.LeaveStatus,
                    IsLastLevel = true,
                    BranchRole = a.Source,
                    EmployeeName = e.Name,
                    Designation = d.Title,
                    Reason = a.App.Reason,
                    AddressDuringLeave = a.App.AddressDuringLeave,
                    LeaveWithNoc = a.App.LeaveWithNoc,
                    ChildDob = a.App.ChildDob,
                    ActionBranch = new BranchesNamesDTO
                    {
                        BranchId = b.BranchId,
                        BranchName = b.BranchName,
                        BranchType = b.BranchType,
                    },
                    LastActionDate = latestTrack != null? latestTrack.ActionAt : a.App.UpdatedAt
                    
                }
            )
            .OrderByDescending(x => x.FromDate)
            .ToListAsync();
    }

    public string? BranchIdAtLevel(LeaveApplicationModel model, LeaveLevel? level) => level switch {
        LeaveLevel.DivertedBranch => model.DivertedBranchId,
        LeaveLevel.ServiceBranch => model.ServiceBranchId,
        LeaveLevel.Zone => model.ZoneId,
        LeaveLevel.District => model.DistrictId,
        LeaveLevel.Goc => model.GocId,
        LeaveLevel.Region => model.RegionId,
        LeaveLevel.HqBranch => model.HqBranchId,
        _ => null
    };

    public string? GetActionBranch(LeaveApplicationModel model) => BranchIdAtLevel(model, model.CurrentLevel);

    public async Task<List<LeaveApplicationTrackDto>?> TrackApplication(LeaveApplicationModel app) {
        var emp = await dbContext.Employees.Where(a => a.EmployeeId == app.EmployeeId).FirstOrDefaultAsync();
        if (emp == null) return null;
        
        List<LeaveApplicationTrackDto> nodes = [];
        
        var createNode = new LeaveApplicationTrackDto {
            Action = "Apply",
            ActorId = emp.EmployeeId,
            ActorName = emp.Name,
            Remark = app.Reason,
            Date = app.AppliedAt
        };
        nodes.Add(createNode);
        
        string?[] stakeholders = [app.DivertedBranchId, app.ServiceBranchId, app.ZoneId, app.DistrictId, 
            app.RegionId, app.GocId, app.HqBranchId];
        var nameDict = (from id in stakeholders where id is not null
            join b in dbContext.Branches on id equals b.BranchId
            select new { Id = id, Name = b.BranchName })
            .ToDictionary(bn => bn.Id, bn => bn.Name);
        nameDict.Add(emp.EmployeeId, emp.Name);

        var trackingNodes = dbContext.LeaveApplicationTrack
            .Where(t => t.ApplicationId == app.ApplicationId)
            .Select(t =>
                new LeaveApplicationTrackDto {
                    ActorId = t.ActionBy,
                    ActorName = nameDict[t.ActionBy],
                    Action = t.ActionType,
                    Remark = t.Comment,
                    Date = t.ActionAt
                })
            .OrderBy(dto => dto.Date).ToList();
        nodes.AddRange(trackingNodes);
        
        if (app.LeaveStatus == LeaveStatus.Pending) {
            var pendingAt = GetActionBranch(app);
            if (pendingAt != null) {
                var pendingNode = new LeaveApplicationTrackDto {
                    ActorId = pendingAt,
                    ActorName = nameDict[pendingAt],
                    Action = "Pending"
                };
                nodes.Add(pendingNode);
            }
        }

        var skipLevels = (int)app.CurrentLevel;
        var futureNodes = stakeholders.Skip(skipLevels).Where(p=> p!= null).Select(p =>
            new LeaveApplicationTrackDto {
                ActorId = p!,
                ActorName = nameDict[p!],
                Action = "Awaited"
            }).ToList();
        nodes.AddRange(futureNodes);
        
        return nodes;
    }
    
}