using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Customer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        // Generate customer number
        var customerNumber = await _customerRepository.GenerateCustomerNumberAsync(cancellationToken);

        // Create customer entity
        var customer = new Domain.Entities.Customer(
            customerNumber,
            request.Name,
            request.BusinessUnitId,
            request.ContactName,
            request.Email,
            request.Phone,
            request.Address);

        // Set optional properties using available methods
        if (request.CreditLimit.HasValue || request.PaymentTermsDays.HasValue)
        {
            customer.UpdateFinancialInfo(
                null,
                request.CreditLimit ?? 0,
                0,
                0,
                request.PaymentTermsDays);
        }

        if (request.AccountManagerId.HasValue)
            customer.AssignAccountManager(request.AccountManagerId.Value);

        if (!string.IsNullOrWhiteSpace(request.Notes))
            customer.AddNote(request.Notes);

        // Save customer
        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
