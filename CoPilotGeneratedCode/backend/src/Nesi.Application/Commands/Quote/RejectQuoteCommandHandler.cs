using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Quote;

public class RejectQuoteCommandHandler : IRequestHandler<RejectQuoteCommand, Unit>
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectQuoteCommandHandler(IQuoteRepository quoteRepository, IUnitOfWork unitOfWork)
    {
        _quoteRepository = quoteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RejectQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = await _quoteRepository.GetByIdAsync(request.QuoteId, cancellationToken);
        if (quote == null)
            throw new InvalidOperationException($"Quote with ID {request.QuoteId} not found");

        quote.Reject(request.RejectedBy, request.Reason);

        _quoteRepository.Update(quote);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
