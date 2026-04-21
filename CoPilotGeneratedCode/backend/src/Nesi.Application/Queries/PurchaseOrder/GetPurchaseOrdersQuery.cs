using MediatR;
using Nesi.Application.DTOs.PurchaseOrder;

namespace Nesi.Application.Queries.PurchaseOrder;

public record GetPurchaseOrdersQuery(
    int? VendorId = null,
    int? WorkOrderId = null,
    string? Status = null,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<GetPurchaseOrdersQueryResult>;

public class GetPurchaseOrdersQueryResult
{
    public List<PurchaseOrderDto> PurchaseOrders { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
