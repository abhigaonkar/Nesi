using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Quote;

public class ConvertQuoteToWorkOrderCommandHandler : IRequestHandler<ConvertQuoteToWorkOrderCommand, int>
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConvertQuoteToWorkOrderCommandHandler(
        IQuoteRepository quoteRepository,
        IWorkOrderRepository workOrderRepository,
        IUnitOfWork unitOfWork)
    {
        _quoteRepository = quoteRepository;
        _workOrderRepository = workOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(ConvertQuoteToWorkOrderCommand request, CancellationToken cancellationToken)
    {
        var quote = await _quoteRepository.GetByIdAsync(request.QuoteId, cancellationToken);
        if (quote == null)
        {
            throw new InvalidOperationException($"Quote with ID {request.QuoteId} not found");
        }

        var workOrder = quote.ConvertToWorkOrder();
        
        await _workOrderRepository.AddAsync(workOrder, cancellationToken);
        
        // Save the work order first to generate its ID
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // Now mark quote as converted with the generated work order ID
        quote.MarkAsConvertedToWorkOrder(workOrder.Id);
        _quoteRepository.Update(quote);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return workOrder.Id;
    }
}
