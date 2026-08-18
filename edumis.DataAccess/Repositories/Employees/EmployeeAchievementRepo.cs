using edumis.DataAccess.IRepositories.IEmployees;
using edumis.Models.Employees.DTO;
using edumis.Models.Employees;
using Microsoft.EntityFrameworkCore;
using edumis.Models;

namespace edumis.DataAccess.Repositories.Employees;

internal class EmployeeAchievementRepo : Repository<EmployeeAchievementModel>, IEmployeeAchievementRepo
{
    private readonly ApplicationDBContext dBContext;

    public EmployeeAchievementRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<List<EmployeeAchievementDTO>?> GetAllAchievements(string EmployeeId)
    {
        var res = await Task.Run(() => from a in dBContext.EmployeeAchievements
                                       join b in dBContext.Employees on a.EmployeeId equals b.EmployeeId
                                       join c in dBContext.EmployeeAppointmentDetails on a.EmployeeId equals c.EmployeeId
                                       join d in dBContext.Designations on c.Designation equals d.RowId
                                       where a.EmployeeId == EmployeeId
                                       orderby a.RowId
                                       select new EmployeeAchievementDTO()
                                       {
                                           RecordID = a.RowId,
                                           EmployeeId = EmployeeId,
                                           EmployeeName = b.FirstName +
                                                  (!string.IsNullOrEmpty(b.MiddleName) ? (" " + b.MiddleName) : "") +
                                                  (!string.IsNullOrEmpty(b.LastName) ? (" " + b.LastName) : ""),
                                           Designation = d.Title,
                                           Achievement = a.Achievement,
                                           FileUploaded = a.FileUploaded,
                                           FileContentType = a.FileContentType,
                                           FileExtension = a.FileExtension,
                                           FilePath = a.FilePath,
                                           IsActive = a.IsActive
                                       });
        return await res.ToListAsync();
    }

    public async Task<bool> Update(EmployeeAchievementUpdateDTO requestData, UploadedFileDetailsModel? FileDetails, string UpdatedBy)
    {
        await dBContext.EmployeeAchievements.Where(x => x.RowId == requestData.RecordId && x.EmployeeId == requestData.EmployeeId).ExecuteUpdateAsync(b => b
            .SetProperty(prop => prop.Achievement, requestData.Achievement)
            .SetProperty(prop => prop.FileUploaded, FileDetails != null ?  FileDetails.FileName : string.Empty)
            .SetProperty(prop => prop.FilePath, FileDetails != null ? FileDetails.FilePath : string.Empty)
            .SetProperty(prop => prop.FileExtension, FileDetails != null ? FileDetails.FileExtension : string.Empty)
            .SetProperty(prop => prop.FileContentType, FileDetails != null ? FileDetails.FileMimeType : string.Empty)
            .SetProperty(prop => prop.ModifiedBy, UpdatedBy)
            .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
            .SetProperty(prop => prop.IsActive, requestData.IsActive)
        );

        return true;
    }
}