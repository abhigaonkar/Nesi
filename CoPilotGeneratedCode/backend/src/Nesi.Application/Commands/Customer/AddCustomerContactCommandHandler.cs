using MediatR;
using Nesi.Domain.Entities;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Customer;

public class AddCustomerContactCommandHandler : IRequestHandler<AddCustomerContactCommand, int>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCustomerContactCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AddCustomerContactCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetWithContactsAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {request.CustomerId} not found");
        }

        var contact = new CustomerContact(
            request.CustomerId,
            request.ContactName,
            request.Title,
            request.Email,
            request.Phone,
            request.Mobile,
            request.IsPrimaryContact,
            request.Notes);

        // If this is marked as primary, unmark other primary contacts
        if (request.IsPrimaryContact)
        {
            foreach (var existingContact in customer.Contacts.Where(c => c.IsPrimaryContact))
            {
                existingContact.SetAsPrimary(false);
            }
        }

        customer.Contacts.Add(contact);
        
        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return contact.Id;
    }
}
