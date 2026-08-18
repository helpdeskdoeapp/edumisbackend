using edumis.DataAccess.IRepositories.IEmployees;
using edumis.Models;
using edumis.Models.Employees;
using edumis.Models.Employees.DTO;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.Employees;

internal class EmployeeExperienceRepo : Repository<EmployeeExperienceModel>, IEmployeeExperienceRepo
{
    private readonly ApplicationDBContext dBContext;

    public EmployeeExperienceRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<List<EmployeeExperienceDTO>?> GetAllExperiences(string employeeid)
    {
        var res = await Task.Run(() => from a in dBContext.EmployeeExperiences
                                       join b in dBContext.Employees on a.EmployeeId equals b.EmployeeId
                                       join c in dBContext.EmployeeAppointmentDetails on a.EmployeeId equals c.EmployeeId
                                       join d in dBContext.Designations on c.Designation equals d.RowId
                                       where a.EmployeeId == employeeid
                                       orderby a.RowId
                                       select new EmployeeExperienceDTO()
                                       {
                                           RecordId = a.RowId,
                                           EmployeeId = employeeid,
                                           EmployeeName = b.FirstName +
                                                  (!string.IsNullOrEmpty(b.MiddleName) ? (" " + b.MiddleName) : "") +
                                                  (!string.IsNullOrEmpty(b.LastName) ? (" " + b.LastName) : ""),
                                           Designation = d.Title,
                                           Experience = a.Experience,
                                           FileUploaded = a.FileUploaded,
                                           FileContentType = a.FileContentType,
                                           FileExtension = a.FileExtension,
                                           FilePath = a.FilePath,
                                           IsActive = a.IsActive
                                       });
        return await res.ToListAsync();
    }

    public async Task<bool> Update(EmployeeExperienceUpdateDTO requestData, UploadedFileDetailsModel? FileDetails, string UpdatedBy)
    {
        await dBContext.EmployeeExperiences.Where(x => x.RowId == requestData.RecordId && x.EmployeeId == requestData.EmployeeId).ExecuteUpdateAsync(b => b
            .SetProperty(prop => prop.Experience, requestData.Experience)
            .SetProperty(prop => prop.FileUploaded, FileDetails != null ? FileDetails.FileName : string.Empty)
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
