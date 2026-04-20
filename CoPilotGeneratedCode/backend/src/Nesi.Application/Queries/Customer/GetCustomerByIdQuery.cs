using MediatR;
using Nesi.Application.DTOs.Customer;

namespace Nesi.Application.Queries.Customer;

public record GetCustomerByIdQuery(int Id, bool IncludeDetails = false) : IRequest<CustomerDto?>;
