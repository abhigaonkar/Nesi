using MediatR;
using Nesi.Application.DTOs.Quote;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Queries.Quote;

public class GetQuotesQueryHandler : IRequestHandler<GetQuotesQuery, IEnumerable<QuoteDto>>
{
    private readonly IQuoteRepository _quoteRepository;

    public GetQuotesQueryHandler(IQuoteRepository quoteRepository)
    {
        _quoteRepository = quoteRepository;
    }

    public async Task<IEnumerable<QuoteDto>> Handle(GetQuotesQuery request, CancellationToken cancellationToken)
    {
        var quotes = await _quoteRepository.GetAllAsync(cancellationToken);
        
        return quotes.Where(q => !q.IsDeleted).Select(quote => new QuoteDto
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
            ApprovedAt = quote.ApprovedAt,
            RejectionReason = quote.RejectionReason,
            RevisionNumber = quote.RevisionNumber,
            CustomerApprovedAt = quote.CustomerApprovedAt,
            CustomerApprovedBy = quote.CustomerApprovedBy,
            WorkOrderId = quote.WorkOrderId,
            ConvertedToWorkOrderAt = quote.ConvertedToWorkOrderAt,
            CreatedAt = quote.CreatedAt,
            CreatedBy = quote.CreatedBy
        }).ToList();
    }
}
