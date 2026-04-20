using MediatR;
using Nesi.Domain.Entities;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Customer;

public class AddCustomerNoteCommandHandler : IRequestHandler<AddCustomerNoteCommand, int>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCustomerNoteCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AddCustomerNoteCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetWithNotesAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {request.CustomerId} not found");
        }

        var note = new CustomerNote(
            request.CustomerId,
            request.Note,
            request.CreatedByUserId);

        customer.CustomerNotes.Add(note);
        
        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return note.Id;
    }
}
