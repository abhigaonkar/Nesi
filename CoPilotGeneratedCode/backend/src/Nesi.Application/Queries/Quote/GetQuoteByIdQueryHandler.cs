using MediatR;
using Nesi.Application.DTOs.Quote;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Queries.Quote;

public class GetQuoteByIdQueryHandler : IRequestHandler<GetQuoteByIdQuery, QuoteDto?>
{
    private readonly IQuoteRepository _quoteRepository;

    public GetQuoteByIdQueryHandler(IQuoteRepository quoteRepository)
    {
        _quoteRepository = quoteRepository;
    }

    public async Task<QuoteDto?> Handle(GetQuoteByIdQuery request, CancellationToken cancellationToken)
    {
        var quote = await _quoteRepository.GetWithLineItemsAsync(request.Id, cancellationToken);
        if (quote == null)
            return null;

        return MapToDto(quote);
    }

    private static QuoteDto MapToDto(Domain.Entities.Quote quote)
    {
        return new QuoteDto
        {
            Id = quote.Id,
            QuoteNumber = quote.QuoteNumber,
            CustomerId = quote.CustomerId,
            CustomerName = quote.Customer?.Name ?? "",
            QuoteType = quote.QuoteType,
            Status = quote.Status,
            Description = quote.Description,
            Scope = quote.Scope,
            EstimatedStartDate = quote.EstimatedStartDate,
            EstimatedCompletionDate = quote.EstimatedCompletionDate,
            ProjectManagerId = quote.ProjectManagerId,
            ProjectManagerName = quote.ProjectManager?.FirstName + " " + quote.ProjectManager?.LastName,
            TermsAndConditions = quote.TermsAndConditions,
            Subtotal = quote.Subtotal,
            TaxRate = quote.TaxRate,
            TaxAmount = quote.TaxAmount,
            DiscountPercent = quote.DiscountPercent,
            DiscountAmount = quote.DiscountAmount,
            Total = quote.Total,
            ApprovedBy = quote.ApprovedBy,
            ApprovedByName = quote.Approver?.FirstName + " " + quote.Approver?.LastName,
            ApprovedAt = quote.ApprovedAt,
            RejectionReason = quote.RejectionReason,
            RevisionNumber = quote.RevisionNumber,
            CustomerApprovedAt = quote.CustomerApprovedAt,
            CustomerApprovedBy = quote.CustomerApprovedBy,
            WorkOrderId = quote.WorkOrderId,
            ConvertedToWorkOrderAt = quote.ConvertedToWorkOrderAt,
            LineItems = quote.LineItems.Select(li => new QuoteLineItemDto
            {
                Id = li.Id,
                QuoteId = li.QuoteId,
                ItemType = li.ItemType,
                LineNumber = li.LineNumber,
                Description = li.Description,
                JobTypeId = li.JobTypeId,
                JobTypeName = li.JobType?.Name,
                EstimatedHours = li.EstimatedHours,
                PartNumber = li.PartNumber,
                Quantity = li.Quantity,
                UnitPrice = li.UnitPrice,
                Total = li.Total,
                Notes = li.Notes
            }).ToList(),
            CreatedAt = quote.CreatedAt,
            CreatedBy = quote.CreatedBy
        };
    }
}
