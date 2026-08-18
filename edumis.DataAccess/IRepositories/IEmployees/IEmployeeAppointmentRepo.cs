using edumis.Models.Employees;
using edumis.Models.Employees.DTO;

namespace edumis.DataAccess.IRepositories.IEmployees;

public interface IEmployeeAppointmentRepo : IRepository<AppointmentModel>
{
    Task<AppointmentDetailsDTO?> GetAppointmentDetails(string EmployeeID);
    Task<bool> UpdateAppointmentDetails(AppointmentDTO AppointmentModel, string UpdatedBy);
}
