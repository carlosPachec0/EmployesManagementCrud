namespace EmployeesManagement.Models.Responses;

public class EmployeeResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // enum serialized as string
    public DateTime CreatedAt { get; set; }
}
