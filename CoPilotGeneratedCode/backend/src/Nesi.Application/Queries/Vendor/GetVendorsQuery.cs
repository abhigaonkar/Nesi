using MediatR;
using Nesi.Application.DTOs.Vendor;

namespace Nesi.Application.Queries.Vendor;

public record GetVendorsQuery(
    bool ActiveOnly = true,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<GetVendorsQueryResult>;

public class GetVendorsQueryResult
{
    public List<VendorDto> Vendors { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
