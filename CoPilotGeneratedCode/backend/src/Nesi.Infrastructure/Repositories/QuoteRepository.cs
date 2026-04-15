using Microsoft.EntityFrameworkCore;
using Nesi.Domain.Entities;
using Nesi.Domain.Enums;
using Nesi.Domain.Interfaces;
using Nesi.Infrastructure.Data;

namespace Nesi.Infrastructure.Repositories;

public class QuoteRepository : Repository<Quote>, IQuoteRepository
{
    public QuoteRepository(NesiDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Quote>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(q => q.CustomerId == customerId && !q.IsDeleted)
            .Include(q => q.Customer)
            .Include(q => q.ProjectManager)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Quote>> GetByStatusAsync(QuoteStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(q => q.Status == status && !q.IsDeleted)
            .Include(q => q.Customer)
            .Include(q => q.ProjectManager)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Quote?> GetByQuoteNumberAsync(string quoteNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(q => q.Customer)
            .Include(q => q.ProjectManager)
            .Include(q => q.LineItems).ThenInclude(li => li.JobType)
            .FirstOrDefaultAsync(q => q.QuoteNumber == quoteNumber && !q.IsDeleted, cancellationToken);
    }

    public async Task<Quote?> GetWithLineItemsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(q => q.Customer)
            .Include(q => q.ProjectManager)
            .Include(q => q.Approver)
            .Include(q => q.LineItems).ThenInclude(li => li.JobType)
            .Include(q => q.WorkOrder)
            .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Quote>> GetPendingApprovalAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(q => q.Status == QuoteStatus.Submitted && !q.IsDeleted)
            .Include(q => q.Customer)
            .Include(q => q.ProjectManager)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Quote>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(q => q.CreatedAt >= startDate && q.CreatedAt <= endDate && !q.IsDeleted)
            .Include(q => q.Customer)
            .Include(q => q.ProjectManager)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<string> GenerateQuoteNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"Q-{year}-";
        
        var lastQuote = await _dbSet
            .Where(q => q.QuoteNumber.StartsWith(prefix))
            .OrderByDescending(q => q.QuoteNumber)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (lastQuote == null)
        {
            return $"{prefix}0001";
        }
        
        var lastNumber = lastQuote.QuoteNumber.Split('-').Last();
        if (int.TryParse(lastNumber, out var number))
        {
            return $"{prefix}{(number + 1):D4}";
        }
        
        return $"{prefix}0001";
    }
}
