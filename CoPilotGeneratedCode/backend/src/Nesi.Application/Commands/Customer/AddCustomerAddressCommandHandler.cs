using MediatR;
using Nesi.Domain.Entities;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Customer;

public class AddCustomerAddressCommandHandler : IRequestHandler<AddCustomerAddressCommand, int>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCustomerAddressCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetWithAddressesAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {request.CustomerId} not found");
        }

        var address = new CustomerAddress(
            request.CustomerId,
            request.AddressType,
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.State,
            request.ZipCode,
            request.Country,
            request.IsDefault);

        // If this is marked as default, unmark other default addresses of the same type
        if (request.IsDefault)
        {
            foreach (var existingAddress in customer.Addresses.Where(a => a.AddressType == request.AddressType && a.IsDefault))
            {
                existingAddress.SetAsDefault(false);
            }
        }

        customer.Addresses.Add(address);
        
        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return address.Id;
    }
}
