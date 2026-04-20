using MediatR;

namespace Nesi.Application.Commands.Customer;

public record CreateCustomerCommand(
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
    string? Notes) : IRequest<int>;
