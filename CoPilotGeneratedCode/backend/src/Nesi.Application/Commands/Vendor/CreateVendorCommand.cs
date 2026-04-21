using MediatR;

namespace Nesi.Application.Commands.Vendor;

public record CreateVendorCommand(
    string CompanyName,
    string? ContactName,
    string? Email,
    string? Phone,
    string? Fax,
    string? Website,
    string? Address,
    string? City,
    string? State,
    string? ZipCode,
    string? Country,
    string? TaxId,
    string? AccountNumber,
    int? PaymentTermsDays,
    decimal? CreditLimit,
    string? Notes) : IRequest<int>;
