using MediatR;

namespace Nesi.Application.Commands.PurchaseOrder;

public record SubmitPurchaseOrderCommand(int PurchaseOrderId) : IRequest<bool>;
