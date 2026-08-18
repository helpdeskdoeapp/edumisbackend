using edumis.Models.Employees;
using edumis.Models.Employees.DTO;

namespace edumis.DataAccess.IRepositories.IEmployees;

public interface IEmployeeEducationRepo : IRepository<EducationModel>
{
    Task<List<EducationDetailsDTO>?> GetEducationDetails(string EmployeeID);
    Task<bool> AddEducationalDetails(EducationDTO EducationModel, string CreatedBy, long? recordid);
}
