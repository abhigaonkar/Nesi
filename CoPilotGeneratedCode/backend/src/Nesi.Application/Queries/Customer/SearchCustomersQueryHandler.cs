using MediatR;
using Nesi.Application.DTOs.Customer;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Queries.Customer;

public class SearchCustomersQueryHandler : IRequestHandler<SearchCustomersQuery, List<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;

    public SearchCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<CustomerDto>> Handle(SearchCustomersQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.SearchTerm))
            return new List<CustomerDto>();

        var customers = await _customerRepository.SearchAsync(request.SearchTerm, cancellationToken);

        return customers.Select(MapToDto).ToList();
    }

    private static CustomerDto MapToDto(Domain.Entities.Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            CustomerNumber = customer.CustomerNumber,
            Name = customer.Name,
            ContactName = customer.ContactName,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
            BusinessUnitId = customer.BusinessUnitId,
            AccountManagerId = customer.AccountManagerId,
            TaxId = customer.TaxId,
            CreditLimit = customer.CreditLimit,
            CurrentBalance = customer.CurrentBalance,
            PaymentTermsDays = customer.PaymentTermsDays,
            Website = customer.Website,
            Notes = customer.Notes,
            IsActive = customer.IsActive,
            CreatedAt = customer.CreatedAt,
            CreatedBy = customer.CreatedBy
        };
    }
}
