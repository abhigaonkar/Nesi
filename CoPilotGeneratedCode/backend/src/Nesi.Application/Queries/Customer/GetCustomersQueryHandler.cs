using MediatR;
using Nesi.Application.DTOs.Customer;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Queries.Customer;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, GetCustomersQueryResult>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<GetCustomersQueryResult> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Customer> customers;

        // Apply filters
        if (request.BusinessUnitId.HasValue)
        {
            customers = await _customerRepository.GetByBusinessUnitAsync(request.BusinessUnitId.Value, cancellationToken);
        }
        else if (request.AccountManagerId.HasValue)
        {
            customers = await _customerRepository.GetByAccountManagerAsync(request.AccountManagerId.Value, cancellationToken);
        }
        else if (request.ActiveOnly)
        {
            customers = await _customerRepository.GetActiveCustomersAsync(cancellationToken);
        }
        else
        {
            customers = await _customerRepository.GetAllAsync(cancellationToken);
        }

        var customersList = customers.ToList();
        var totalCount = customersList.Count;

        // Apply pagination
        var paginatedCustomers = customersList
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(MapToDto)
            .ToList();

        return new GetCustomersQueryResult
        {
            Customers = paginatedCustomers,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
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
