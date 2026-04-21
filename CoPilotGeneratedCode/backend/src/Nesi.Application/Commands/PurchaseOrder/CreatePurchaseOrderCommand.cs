using MediatR;

namespace Nesi.Application.Commands.PurchaseOrder;

public record CreatePurchaseOrderCommand(
    int VendorId,
    int? WorkOrderId,
    DateTime OrderDate,
    DateTime? RequiredByDate,
    string? Description,
    string? ShippingAddress,
    string? ShippingCity,
    string? ShippingState,
    string? ShippingZipCode,
    List<PurchaseOrderLineItemInput> LineItems) : IRequest<int>;

public record PurchaseOrderLineItemInput(
    int LineNumber,
    string Description,
    string? PartNumber,
    decimal Quantity,
    string UnitOfMeasure,
    decimal UnitPrice);
