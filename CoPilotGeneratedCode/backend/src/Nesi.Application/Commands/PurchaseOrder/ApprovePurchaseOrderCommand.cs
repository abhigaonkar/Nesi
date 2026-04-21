using MediatR;

namespace Nesi.Application.Commands.PurchaseOrder;

public record ApprovePurchaseOrderCommand(int PurchaseOrderId, int ApprovedBy) : IRequest<bool>;
