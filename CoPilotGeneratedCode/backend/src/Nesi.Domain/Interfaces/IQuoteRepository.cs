using Nesi.Domain.Entities;
using Nesi.Domain.Enums;

namespace Nesi.Domain.Interfaces;

/// <summary>
/// Repository interface for Quote entity with specific query methods
/// </summary>
public interface IQuoteRepository : IRepository<Quote>
{
    Task<IEnumerable<Quote>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Quote>> GetByStatusAsync(QuoteStatus status, CancellationToken cancellationToken = default);
    Task<Quote?> GetByQuoteNumberAsync(string quoteNumber, CancellationToken cancellationToken = default);
    Task<Quote?> GetWithLineItemsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Quote>> GetPendingApprovalAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Quote>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<string> GenerateQuoteNumberAsync(CancellationToken cancellationToken = default);
}
