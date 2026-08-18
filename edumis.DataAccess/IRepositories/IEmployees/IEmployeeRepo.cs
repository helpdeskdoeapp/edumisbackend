using edumis.DataAccess.Repositories.Employees;
using edumis.Models.Employees;
using edumis.Models.Employees.DTO;

namespace edumis.DataAccess.IRepositories.IEmployees;

public interface IEmployeeRepo : IRepository<EmployeeModel>
{
    Task<EmployeeDetailsDTO?> GetEmployeeDetails(string EmployeeID);
    Task<List<SearchResultResponseDTO>?> SearchEmployees(SearchEmployeeRequestDTO searchEmployee);
    Task<List<EmployeeBasicDto>> GetEmployeesByBranch(string branch);
    Task<string?> CreateEmployee(EmployeeDTO empModel, string CreatedBy);
    Task<bool> UpdateEmployee(EmployeeDTO empModel, string UpdatedBy);
    Task<bool> EditPhoto(ProfilePhoto photo, string UpdatedBy);  
    Task<bool> DeActivateEmployee(string EmployeeId, string UpdatedBy);
    Task<bool> ActivateEmployee(string EmployeeId, string UpdatedBy);
}
