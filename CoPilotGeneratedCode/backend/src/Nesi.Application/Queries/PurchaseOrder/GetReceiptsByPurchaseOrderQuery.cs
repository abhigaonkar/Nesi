using MediatR;
using Nesi.Application.DTOs.PurchaseOrder;

namespace Nesi.Application.Queries.PurchaseOrder;

public record GetReceiptsByPurchaseOrderQuery(int PurchaseOrderId) : IRequest<List<PurchaseOrderReceiptDto>>;
