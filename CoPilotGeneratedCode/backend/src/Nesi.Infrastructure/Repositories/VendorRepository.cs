using Microsoft.EntityFrameworkCore;
using Nesi.Domain.Entities;
using Nesi.Domain.Enums;
using Nesi.Domain.Interfaces;
using Nesi.Infrastructure.Data;

namespace Nesi.Infrastructure.Repositories;

public class VendorRepository : Repository<Vendor>, IVendorRepository
{
    public VendorRepository(NesiDbContext context) : base(context)
    {
    }

    public async Task<Vendor?> GetByVendorNumberAsync(string vendorNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(v => v.VendorNumber == vendorNumber && !v.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Vendor>> GetByStatusAsync(VendorStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(v => v.Status == status && !v.IsDeleted)
            .OrderBy(v => v.CompanyName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vendor>> GetActiveVendorsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(v => v.IsActive && !v.IsDeleted)
            .OrderBy(v => v.CompanyName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vendor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        return await _dbSet
            .Where(v => !v.IsDeleted && (
                v.VendorNumber.ToLower().Contains(lowerSearchTerm) ||
                v.CompanyName.ToLower().Contains(lowerSearchTerm) ||
                (v.Email != null && v.Email.ToLower().Contains(lowerSearchTerm)) ||
                (v.Phone != null && v.Phone.Contains(searchTerm))
            ))
            .OrderBy(v => v.CompanyName)
            .ToListAsync(cancellationToken);
    }

    public async Task<string> GenerateVendorNumberAsync(CancellationToken cancellationToken = default)
    {
        var currentYear = DateTime.UtcNow.Year;
        var prefix = $"VEN-{currentYear}-";

        var lastVendor = await _dbSet
            .Where(v => v.VendorNumber.StartsWith(prefix))
            .OrderByDescending(v => v.VendorNumber)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastVendor == null)
        {
            return $"{prefix}00001";
        }

        var lastNumber = int.Parse(lastVendor.VendorNumber.Substring(prefix.Length));
        var nextNumber = lastNumber + 1;
        return $"{prefix}{nextNumber:D5}";
    }
}
