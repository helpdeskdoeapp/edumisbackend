using System.Globalization;
using edumis.DataAccess.IRepositories.IMasters;
using edumis.Models.Masters;
using edumis.Models.Masters.DTO;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.Masters;

internal class BranchRepo(ApplicationDBContext dBContext) : Repository<Models.Masters.BranchesModel>(dBContext), IBranchRepo
{  
    public async Task<List<BranchDetailsDTO>?> GetBranches()
    {
        var returnData = await (from a in dBContext.Branches
                                join b in dBContext.CodeValues on a.BranchType equals b.CodeValue
                                join d in dBContext.Employees on a.InchargeId equals d.EmployeeId into inchargeGroup
                                from incharge in inchargeGroup.DefaultIfEmpty()
                                join e in dBContext.Districts on a.DistrictId equals e.RowId into districtGroup
                                from district in districtGroup.DefaultIfEmpty()
                                join f in dBContext.Zones on a.ZoneId equals f.RowId into zoneGroup
                                from zone in zoneGroup.DefaultIfEmpty()                             
                                select new BranchDetailsDTO
                                {
                                    BranchId = a.BranchId,
                                    BranchName = a.BranchName,
                                    Address = a.Address,
                                    BranchType = a.BranchType,
                                    BranchTypeDesc = b.CodeValDescription,
                                    BuildingId = a.BuildingId,
                                    ContactNo = a.ContactNo,
                                    DistrictId = a.DistrictId,
                                    DistrictTitle = district != null ? district.Title : null,
                                    EmailId = a.EmailId,
                                    InchargeId = a.InchargeId,
                                    InchargeName = incharge != null ? $"{incharge.FirstName} {incharge.MiddleName} {incharge.LastName}"
                                        : string.Empty,
                                    ZoneId = a.ZoneId,
                                    ZoneTitle = zone != null ? zone.Title : null,
                                    ParentBranchId = a.ParentBranchId,
                                    IsActive = a.IsActive
                                }).ToListAsync();
        return returnData;
    }

    public async Task<BranchDetailsDTO?> GetDetails(string BranchId)
    {
        var returnData = await (from a in dBContext.Branches
                                join b in dBContext.CodeValues on a.BranchType equals b.CodeValue
                                join d in dBContext.Employees on a.InchargeId equals d.EmployeeId into inchargeGroup
                                from incharge in inchargeGroup.DefaultIfEmpty()
                                join e in dBContext.Districts on a.DistrictId equals e.RowId into districtGroup
                                from district in districtGroup.DefaultIfEmpty()
                                join f in dBContext.Zones on a.ZoneId equals f.RowId into zoneGroup
                                from zone in zoneGroup.DefaultIfEmpty()
                                where a.BranchId == BranchId
                                select new BranchDetailsDTO
                                {
                                    BranchId = a.BranchId,
                                    BranchName = a.BranchName,
                                    Address = a.Address,
                                    BranchType = a.BranchType,
                                    BranchTypeDesc = b.CodeValDescription,
                                    BuildingId = a.BuildingId,
                                    ContactNo = a.ContactNo,
                                    DistrictId = a.DistrictId,
                                    DistrictTitle = district != null ? district.Title : null,
                                    EmailId = a.EmailId,
                                    InchargeId = a.InchargeId,
                                    InchargeName = incharge != null ? $"{incharge.FirstName} {incharge.MiddleName} {incharge.LastName}"
                                        : string.Empty,
                                    ZoneId = a.ZoneId,
                                    ZoneTitle = zone != null ? zone.Title : null,
                                    ParentBranchId = a.ParentBranchId,
                                    IsActive = a.IsActive
                                }).FirstOrDefaultAsync();
        return returnData;
    }

    public async Task<BranchesNamesDTO?> GetParentBranch(string branchId) {
        return await (from branch in dBContext.Branches
            join parent in dBContext.Branches on branch.ParentBranchId equals parent.BranchId 
            where branch.BranchId == branchId
            select new BranchesNamesDTO{
                BranchId = parent.BranchId,
                BranchName = parent.BranchName,
                BranchType = parent.BranchType,
            }).FirstOrDefaultAsync();
    }
}
