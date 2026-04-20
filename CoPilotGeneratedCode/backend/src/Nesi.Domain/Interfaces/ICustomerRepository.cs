using Nesi.Domain.Entities;

namespace Nesi.Domain.Interfaces;

/// <summary>
/// Repository interface for Customer entity with specific query methods
/// </summary>
public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByCustomerNumberAsync(string customerNumber, CancellationToken cancellationToken = default);
    Task<Customer?> GetWithContactsAsync(int id, CancellationToken cancellationToken = default);
    Task<Customer?> GetWithAddressesAsync(int id, CancellationToken cancellationToken = default);
    Task<Customer?> GetWithNotesAsync(int id, CancellationToken cancellationToken = default);
    Task<Customer?> GetWithAllDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetByBusinessUnitAsync(int businessUnitId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetByAccountManagerAsync(int accountManagerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<string> GenerateCustomerNumberAsync(CancellationToken cancellationToken = default);
    Task<bool> CustomerNumberExistsAsync(string customerNumber, CancellationToken cancellationToken = default);
}
