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
    string? TaxId,
    decimal? CreditLimit,
    int? PaymentTermsDays,
    int? AccountManagerId,
    string? Website,
    string? Notes) : IRequest<Unit>;
