using System.Text.Json.Serialization;

namespace EmployeesManagement.Models.Entities;

public enum UserRole
{
    Admin,
    Standard
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmployeeType
{
    Manager,
    General
}
