using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesManagement.Controllers;

[Authorize]
public class EmployeesController : Controller
{
    public IActionResult Create() => View();
}
