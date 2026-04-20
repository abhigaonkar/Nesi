using MediatR;

namespace Nesi.Application.Commands.Customer;

public record CreateCustomerCommand(
    string Name,
    int? BusinessUnitId,
    string? ContactName,
    string? Email,
    string? Phone,
    string? Address,
    decimal? CreditLimit,
    int? PaymentTermsDays,
    int? AccountManagerId,
    string? Notes) : IRequest<int>;
