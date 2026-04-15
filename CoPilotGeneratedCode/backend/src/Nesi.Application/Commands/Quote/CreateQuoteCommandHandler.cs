using MediatR;
using Nesi.Domain.Entities;
using Nesi.Domain.Enums;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Quote;

public class CreateQuoteCommandHandler : IRequestHandler<CreateQuoteCommand, int>
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateQuoteCommandHandler(IQuoteRepository quoteRepository, IUnitOfWork unitOfWork)
    {
        _quoteRepository = quoteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateQuoteCommand request, CancellationToken cancellationToken)
    {
        // Generate quote number
        var quoteNumber = await _quoteRepository.GenerateQuoteNumberAsync(cancellationToken);

        // Create quote entity
        var quote = new Domain.Entities.Quote(
            quoteNumber,
            request.CustomerId,
            request.QuoteType,
            request.Description,
            request.Scope,
            request.EstimatedStartDate,
            request.EstimatedCompletionDate,
            request.ProjectManagerId,
            request.TermsAndConditions);

        // Add line items
        int lineNumber = 1;
        foreach (var item in request.LineItems)
        {
            QuoteLineItem lineItem = item.ItemType switch
            {
                QuoteLineItemType.Labor => QuoteLineItem.CreateLabor(
                    0, // Will be set when quote is saved
                    lineNumber,
                    item.Description,
                    item.JobTypeId ?? 0,
                    item.EstimatedHours,
                    item.UnitPrice,
                    item.Notes),
                QuoteLineItemType.Material => QuoteLineItem.CreateMaterial(
                    0,
                    lineNumber,
                    item.Description,
                    item.PartNumber ?? "",
                    item.Quantity,
                    item.UnitPrice,
                    item.Notes),
                QuoteLineItemType.Equipment => QuoteLineItem.CreateEquipment(
                    0,
                    lineNumber,
                    item.Description,
                    item.Quantity,
                    item.UnitPrice,
                    item.Notes),
                QuoteLineItemType.Miscellaneous => QuoteLineItem.CreateMiscellaneous(
                    0,
                    lineNumber,
                    item.Description,
                    item.Quantity,
                    item.UnitPrice,
                    item.Notes),
                _ => throw new InvalidOperationException($"Unknown line item type: {item.ItemType}")
            };

            quote.LineItems.Add(lineItem);
            lineNumber++;
        }

        // Calculate totals
        quote.CalculateTotals(request.TaxRate);

        // Save quote
        await _quoteRepository.AddAsync(quote, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return quote.Id;
    }
}
