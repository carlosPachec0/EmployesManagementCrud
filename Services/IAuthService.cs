using System.Security.Claims;
using EmployeesManagement.Models.Entities;
using EmployeesManagement.Models.Requests;

namespace EmployeesManagement.Services;

public interface IAuthService
{
    Task<ClaimsPrincipal?> AuthenticateAsync(LoginRequest request);
}
