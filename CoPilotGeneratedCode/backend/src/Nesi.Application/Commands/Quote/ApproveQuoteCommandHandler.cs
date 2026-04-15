using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Quote;

public class ApproveQuoteCommandHandler : IRequestHandler<ApproveQuoteCommand, Unit>
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveQuoteCommandHandler(IQuoteRepository quoteRepository, IUnitOfWork unitOfWork)
    {
        _quoteRepository = quoteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ApproveQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = await _quoteRepository.GetByIdAsync(request.QuoteId, cancellationToken);
        if (quote == null)
            throw new InvalidOperationException($"Quote with ID {request.QuoteId} not found");

        // Business Rule BR-001: Quotes over $50,000 require manager approval
        // This should be enforced at the controller/authorization level

        quote.Approve(request.ApprovedBy);

        _quoteRepository.Update(quote);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
