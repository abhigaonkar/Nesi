using MediatR;

namespace Nesi.Application.Commands.Customer;

public record UpdateCustomerCommand(
    int Id,
    string Name,
    int? BusinessUnitId,
    string? ContactName,
    string? Email,
    string? Phone,
    string? Address,
    decimal? CreditLimit,
    int? PaymentTermsDays,
    int? AccountManagerId,
    string? Notes) : IRequest<Unit>;
