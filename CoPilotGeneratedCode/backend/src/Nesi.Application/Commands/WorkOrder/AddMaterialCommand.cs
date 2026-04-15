using MediatR;

namespace Nesi.Application.Commands.WorkOrder;

public record AddMaterialCommand(
    int WorkOrderId,
    string PartNumber,
    string Description,
    decimal Quantity,
    decimal UnitCost,
    string? PurchaseOrderNumber,
    string? Supplier,
    string? Notes) : IRequest<int>;
