using Microsoft.EntityFrameworkCore;
using Nesi.Domain.Entities;
using Nesi.Domain.Interfaces;
using Nesi.Infrastructure.Data;

namespace Nesi.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(NesiDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByCustomerNumberAsync(string customerNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.CustomerNumber == customerNumber && !c.IsDeleted, cancellationToken);
    }

    public async Task<Customer?> GetWithContactsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Contacts)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
    }

    public async Task<Customer?> GetWithAddressesAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
    }

    public async Task<Customer?> GetWithNotesAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Notes)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
    }

    public async Task<Customer?> GetWithAllDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Contacts)
            .Include(c => c.Addresses)
            .Include(c => c.Notes)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetActiveCustomersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.IsActive && !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetByBusinessUnitAsync(int businessUnitId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.BusinessUnitId == businessUnitId && !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetByAccountManagerAsync(int accountManagerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.AccountManagerId == accountManagerId && !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        
        return await _dbSet
            .Where(c => !c.IsDeleted && (
                c.Name.ToLower().Contains(lowerSearchTerm) ||
                c.CustomerNumber.ToLower().Contains(lowerSearchTerm) ||
                (c.Email != null && c.Email.ToLower().Contains(lowerSearchTerm)) ||
                (c.Phone != null && c.Phone.ToLower().Contains(lowerSearchTerm))
            ))
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<string> GenerateCustomerNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"CUST-{year}-";
        
        var lastCustomer = await _dbSet
            .Where(c => c.CustomerNumber.StartsWith(prefix))
            .OrderByDescending(c => c.CustomerNumber)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (lastCustomer == null)
        {
            return $"{prefix}0001";
        }
        
        var lastNumber = lastCustomer.CustomerNumber.Split('-').Last();
        if (int.TryParse(lastNumber, out var number))
        {
            return $"{prefix}{(number + 1):D4}";
        }
        
        return $"{prefix}0001";
    }

    public async Task<bool> CustomerNumberExistsAsync(string customerNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(c => c.CustomerNumber == customerNumber, cancellationToken);
    }
}
