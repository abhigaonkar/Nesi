using MediatR;
using Nesi.Application.DTOs.Customer;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Queries.Customer;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = request.IncludeDetails 
            ? await _customerRepository.GetWithAllDetailsAsync(request.Id, cancellationToken)
            : await _customerRepository.GetByIdAsync(request.Id, cancellationToken);

        if (customer == null)
            return null;

        return MapToDto(customer, request.IncludeDetails);
    }

    private static CustomerDto MapToDto(Domain.Entities.Customer customer, bool includeDetails)
    {
        var dto = new CustomerDto
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
            CreditLimit = customer.CreditLimit,
            TermId = customer.TermId,
            Notes = customer.Notes,
            IsActive = customer.IsActive,
            CreatedAt = customer.CreatedAt,
            CreatedBy = customer.CreatedBy
        };

        if (includeDetails)
        {
            dto.Contacts = customer.Contacts.Select(c => new CustomerContactDto
            {
                Id = c.Id,
                CustomerId = c.CustomerId,
                Name = c.Name,
                Title = c.Title,
                Email = c.Email,
                Phone = c.Phone,
                CellPhone = c.CellPhone,
                IsPrimary = c.IsPrimary,
                Status = c.Status,
                CreatedAt = c.CreatedAt
            }).ToList();

            dto.Addresses = customer.Addresses.Select(a => new CustomerAddressDto
            {
                Id = a.Id,
                CustomerId = a.CustomerId,
                AddressType = a.AddressType,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                City = a.City,
                State = a.State,
                ZipCode = a.ZipCode,
                Country = a.Country,
                IsDefault = a.IsDefault,
                IsActive = a.IsActive,
                CreatedAt = a.CreatedAt
            }).ToList();

            dto.CustomerNotesList = customer.CustomerNotes.Select(n => new CustomerNoteDto
            {
                Id = n.Id,
                CustomerId = n.CustomerId,
                Note = n.Note,
                CreatedByUserId = n.CreatedByUserId,
                CreatedAt = n.CreatedAt
            }).ToList();
        }

        return dto;
    }
}
