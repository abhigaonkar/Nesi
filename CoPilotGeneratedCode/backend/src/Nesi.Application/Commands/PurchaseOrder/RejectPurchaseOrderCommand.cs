using MediatR;

namespace Nesi.Application.Commands.PurchaseOrder;

public record RejectPurchaseOrderCommand(
    int PurchaseOrderId, 
    int RejectedBy, 
    string Reason) : IRequest<bool>;
