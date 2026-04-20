using MediatR;

namespace Nesi.Application.Commands.Customer;

public record AddCustomerContactCommand(
    int CustomerId,
    string Name,
    string? Title,
    string? Email,
    string? Phone,
    string? CellPhone,
    bool IsPrimary) : IRequest<int>;
