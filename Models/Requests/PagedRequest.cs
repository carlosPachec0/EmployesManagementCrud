namespace EmployeesManagement.Models.Requests;

public class PagedRequest
{
    private const int MaxPageSize = 10;
    private int _pageSize = 5;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}
