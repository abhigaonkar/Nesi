using Nesi.Domain.Entities;
using Nesi.Domain.Enums;

namespace Nesi.Domain.Interfaces;

/// <summary>
/// Repository interface for Vendor entity with specific query methods
/// </summary>
public interface IVendorRepository : IRepository<Vendor>
{
    Task<Vendor?> GetByVendorNumberAsync(string vendorNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vendor>> GetByStatusAsync(VendorStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vendor>> GetActiveVendorsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Vendor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<string> GenerateVendorNumberAsync(CancellationToken cancellationToken = default);
}
