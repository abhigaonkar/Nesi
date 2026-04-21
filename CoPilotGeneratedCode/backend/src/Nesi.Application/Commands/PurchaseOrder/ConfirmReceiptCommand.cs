using MediatR;

namespace Nesi.Application.Commands.PurchaseOrder;

public record ConfirmReceiptCommand(int ReceiptId) : IRequest<bool>;
