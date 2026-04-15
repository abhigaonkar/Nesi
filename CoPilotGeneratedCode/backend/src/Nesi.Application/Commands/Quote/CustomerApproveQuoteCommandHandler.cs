using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Quote;

public class CustomerApproveQuoteCommandHandler : IRequestHandler<CustomerApproveQuoteCommand, bool>
{
    private readonly IQuoteRepository _quoteRepository;

    public CustomerApproveQuoteCommandHandler(IQuoteRepository quoteRepository)
    {
        _quoteRepository = quoteRepository;
    }

    public async Task<bool> Handle(CustomerApproveQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = await _quoteRepository.GetByIdAsync(request.QuoteId);
        if (quote == null)
        {
            throw new InvalidOperationException($"Quote with ID {request.QuoteId} not found");
        }

        quote.CustomerApprove(request.ApprovedBy);
        
        await _quoteRepository.UpdateAsync(quote);
        
        return true;
    }
}
