using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Customer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {request.Id} not found");
        }

        // Update basic information
        customer.UpdateBasicInfo(
            request.Name,
            request.BusinessUnitId,
            request.ContactName,
            request.Email,
            request.Phone,
            request.Address);

        // Update optional properties
        if (request.CreditLimit.HasValue || request.PaymentTermsDays.HasValue)
        {
            customer.UpdateFinancialInfo(
                null,
                request.CreditLimit ?? customer.CreditLimit,
                customer.Discount,
                customer.BudgetThreshold,
                request.PaymentTermsDays);
        }

        if (request.AccountManagerId.HasValue)
            customer.AssignAccountManager(request.AccountManagerId.Value);

        if (!string.IsNullOrWhiteSpace(request.Notes))
            customer.AddNote(request.Notes);

        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
