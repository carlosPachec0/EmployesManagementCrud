using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using EmployeesManagement.Models.Requests;
using EmployeesManagement.Models.Responses;
using EmployeesManagement.Services;

namespace EmployeesManagement.Controllers.Api.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class EmployeesController(IEmployeeService employeeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponse<EmployeeResponse>>> Get([FromQuery] PagedRequest request)
    {
        var result = await employeeService.GetPagedAsync(request);
        return Ok(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<EmployeeResponse>> Create([FromBody] CreateEmployeeRequest request)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await employeeService.CreateAsync(request, userId);
        return CreatedAtAction(nameof(Get), new { }, result);
    }
}
