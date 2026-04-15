using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Quote;

public class ConvertQuoteToWorkOrderCommandHandler : IRequestHandler<ConvertQuoteToWorkOrderCommand, int>
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IWorkOrderRepository _workOrderRepository;

    public ConvertQuoteToWorkOrderCommandHandler(
        IQuoteRepository quoteRepository,
        IWorkOrderRepository workOrderRepository)
    {
        _quoteRepository = quoteRepository;
        _workOrderRepository = workOrderRepository;
    }

    public async Task<int> Handle(ConvertQuoteToWorkOrderCommand request, CancellationToken cancellationToken)
    {
        var quote = await _quoteRepository.GetByIdAsync(request.QuoteId);
        if (quote == null)
        {
            throw new InvalidOperationException($"Quote with ID {request.QuoteId} not found");
        }

        var workOrder = quote.ConvertToWorkOrder();
        
        await _workOrderRepository.AddAsync(workOrder);
        
        // Mark quote as converted
        quote.MarkAsConvertedToWorkOrder(workOrder.Id);
        await _quoteRepository.UpdateAsync(quote);
        
        return workOrder.Id;
    }
}
