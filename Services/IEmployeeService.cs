using EmployeesManagement.Models.Requests;
using EmployeesManagement.Models.Responses;

namespace EmployeesManagement.Services;

public interface IEmployeeService
{
    Task<PagedResponse<EmployeeResponse>> GetPagedAsync(PagedRequest request);
    Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, int createdByUserId);
}
