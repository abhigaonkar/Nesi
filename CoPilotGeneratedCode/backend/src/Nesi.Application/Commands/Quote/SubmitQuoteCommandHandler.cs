using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Quote;

public class SubmitQuoteCommandHandler : IRequestHandler<SubmitQuoteCommand, Unit>
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitQuoteCommandHandler(IQuoteRepository quoteRepository, IUnitOfWork unitOfWork)
    {
        _quoteRepository = quoteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(SubmitQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = await _quoteRepository.GetWithLineItemsAsync(request.QuoteId, cancellationToken);
        if (quote == null)
            throw new InvalidOperationException($"Quote with ID {request.QuoteId} not found");

        quote.Submit();

        _quoteRepository.Update(quote);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
