using MediatR;
using Nesi.Application.DTOs.Customer;

namespace Nesi.Application.Queries.Customer;

public record GetCustomersQuery(
    bool ActiveOnly = true,
    int? BusinessUnitId = null,
    int? AccountManagerId = null,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<GetCustomersQueryResult>;

public class GetCustomersQueryResult
{
    public List<CustomerDto> Customers { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
