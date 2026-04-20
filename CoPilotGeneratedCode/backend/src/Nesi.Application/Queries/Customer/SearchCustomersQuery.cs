using MediatR;
using Nesi.Application.DTOs.Customer;

namespace Nesi.Application.Queries.Customer;

public record SearchCustomersQuery(string SearchTerm) : IRequest<List<CustomerDto>>;
