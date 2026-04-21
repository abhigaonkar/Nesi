using MediatR;

namespace Nesi.Application.Commands.PurchaseOrder;

public record CreateReceiptCommand(
    int PurchaseOrderId,
    DateTime ReceivedDate,
    string? PackingSlipNumber,
    string? Notes,
    List<ReceiptItemInput> Items) : IRequest<int>;

public record ReceiptItemInput(
    int PurchaseOrderLineItemId,
    decimal QuantityReceived,
    string? Condition,
    string? Notes,
    bool HasDiscrepancy = false,
    string? DiscrepancyReason = null);
