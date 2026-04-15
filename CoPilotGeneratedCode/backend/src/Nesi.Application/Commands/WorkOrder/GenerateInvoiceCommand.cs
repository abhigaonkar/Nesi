using MediatR;

namespace Nesi.Application.Commands.WorkOrder;

public record GenerateInvoiceCommand(
    int WorkOrderId,
    decimal InvoiceAmount) : IRequest<Unit>;
