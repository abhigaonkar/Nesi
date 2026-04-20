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

        // Set optional properties
        if (!string.IsNullOrWhiteSpace(request.TaxId))
            customer.UpdateTaxId(request.TaxId);

        if (request.CreditLimit.HasValue)
            customer.UpdateCreditLimit(request.CreditLimit.Value);

        if (request.PaymentTermsDays.HasValue)
            customer.UpdatePaymentTerms(request.PaymentTermsDays.Value);

        if (request.AccountManagerId.HasValue)
            customer.AssignAccountManager(request.AccountManagerId.Value);

        if (!string.IsNullOrWhiteSpace(request.Website))
            customer.UpdateWebsite(request.Website);

        if (!string.IsNullOrWhiteSpace(request.Notes))
            customer.UpdateNotes(request.Notes);

        // Save customer
        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
