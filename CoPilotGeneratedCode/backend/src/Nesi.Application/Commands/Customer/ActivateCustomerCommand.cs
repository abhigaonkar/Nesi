using MediatR;

namespace Nesi.Application.Commands.Customer;

public record ActivateCustomerCommand(int Id) : IRequest<Unit>;
