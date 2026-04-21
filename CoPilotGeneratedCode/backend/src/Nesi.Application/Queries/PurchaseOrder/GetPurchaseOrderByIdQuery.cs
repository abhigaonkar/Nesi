using MediatR;
using Nesi.Application.DTOs.PurchaseOrder;

namespace Nesi.Application.Queries.PurchaseOrder;

public record GetPurchaseOrderByIdQuery(int Id, bool IncludeLineItems = true, bool IncludeReceipts = false) : IRequest<PurchaseOrderDto?>;
