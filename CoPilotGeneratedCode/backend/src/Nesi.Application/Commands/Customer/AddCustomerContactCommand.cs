using MediatR;

namespace Nesi.Application.Commands.Customer;

public record AddCustomerContactCommand(
    int CustomerId,
    string ContactName,
    string? Title,
    string? Email,
    string? Phone,
    string? Mobile,
    bool IsPrimaryContact,
    string? Notes) : IRequest<int>;
