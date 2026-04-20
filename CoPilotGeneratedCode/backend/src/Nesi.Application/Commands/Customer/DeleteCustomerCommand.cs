using MediatR;

namespace Nesi.Application.Commands.Customer;

public record DeleteCustomerCommand(int Id) : IRequest<Unit>;
