using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Quote;

public class CustomerApproveQuoteCommandHandler : IRequestHandler<CustomerApproveQuoteCommand, bool>
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerApproveQuoteCommandHandler(IQuoteRepository quoteRepository, IUnitOfWork unitOfWork)
    {
        _quoteRepository = quoteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CustomerApproveQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = await _quoteRepository.GetByIdAsync(request.QuoteId, cancellationToken);
        if (quote == null)
        {
            throw new InvalidOperationException($"Quote with ID {request.QuoteId} not found");
        }

        quote.CustomerApprove(request.ApprovedBy);
        
        _quoteRepository.Update(quote);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
