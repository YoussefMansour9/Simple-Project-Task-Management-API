namespace TaskManagement.Application.Common;

public interface IPaginatedQuery
{
    int PageIndex { get; init; }
    int PageSize { get; init; }
}