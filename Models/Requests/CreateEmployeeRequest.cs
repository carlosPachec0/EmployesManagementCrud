using System.ComponentModel.DataAnnotations;
using EmployeesManagement.Models.Entities;

namespace EmployeesManagement.Models.Requests;

public class CreateEmployeeRequest
{
    [Required(ErrorMessage = "First name is required"), MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required"), MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Employee type is required")]
    public EmployeeType Type { get; set; }
}
