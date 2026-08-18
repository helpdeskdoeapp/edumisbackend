using edumis.Models;
using edumis.Models.Employees;
using edumis.Models.Employees.DTO;

namespace edumis.DataAccess.IRepositories.IEmployees;

public interface IEmployeeAchievementRepo : IRepository<EmployeeAchievementModel>
{
    Task<List<EmployeeAchievementDTO>?> GetAllAchievements(string EmployeeId);

    Task<bool> Update(EmployeeAchievementUpdateDTO requestData, UploadedFileDetailsModel? FileDetails, string UpdatedBy);
}
