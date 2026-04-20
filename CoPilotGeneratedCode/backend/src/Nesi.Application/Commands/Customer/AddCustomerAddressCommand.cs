using MediatR;
using Nesi.Domain.Enums;

namespace Nesi.Application.Commands.Customer;

public record AddCustomerAddressCommand(
    int CustomerId,
    AddressType AddressType,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string ZipCode,
    string Country,
    bool IsDefault) : IRequest<int>;
