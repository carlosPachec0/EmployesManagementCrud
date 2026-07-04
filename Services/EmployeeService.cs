using Microsoft.EntityFrameworkCore;
using EmployeesManagement.Data;
using EmployeesManagement.Models.Entities;
using EmployeesManagement.Models.Requests;
using EmployeesManagement.Models.Responses;

namespace EmployeesManagement.Services;

public class EmployeeService(ApplicationDbContext context) : IEmployeeService
{
    public async Task<PagedResponse<EmployeeResponse>> GetPagedAsync(PagedRequest request)
    {
        var query = context.Employees
            .OrderByDescending(e => e.CreatedAt);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new EmployeeResponse
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Type = e.Type.ToString(),
                CreatedAt = e.CreatedAt
            })
            .ToListAsync();

        return new PagedResponse<EmployeeResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, int createdByUserId)
    {
        var employee = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Type = request.Type,
            CreatedByUserId = createdByUserId,
            IsActive = true
        };

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        return new EmployeeResponse
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Type = employee.Type.ToString(),
            CreatedAt = employee.CreatedAt
        };
    }
}
