using edumis.Models.Employees.DTO;
using edumis.Models.Employees;
using edumis.Models;

namespace edumis.DataAccess.IRepositories.IEmployees;

public interface IEmployeeExperienceRepo : IRepository<EmployeeExperienceModel>
{
    Task<List<EmployeeExperienceDTO>?> GetAllExperiences(string employeeid);

    Task<bool> Update(EmployeeExperienceUpdateDTO requestData, UploadedFileDetailsModel? FileDetails, string UpdatedBy);
}
