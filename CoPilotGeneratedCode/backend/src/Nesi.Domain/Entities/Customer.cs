namespace Nesi.Domain.Entities;

/// <summary>
/// Customer entity
/// </summary>
public class Customer : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? ContactName { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Address { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public virtual ICollection<WorkOrder> WorkOrders { get; private set; } = new List<WorkOrder>();

    // Private constructor for EF Core
    private Customer() { }

    // Public constructor
    public Customer(string name, string? contactName = null, string? email = null, string? phone = null, string? address = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty", nameof(name));

        Name = name;
        ContactName = contactName;
        Email = email;
        Phone = phone;
        Address = address;
        IsActive = true;
    }

    public void UpdateInfo(string name, string? contactName, string? email, string? phone, string? address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty", nameof(name));

        Name = name;
        ContactName = contactName;
        Email = email;
        Phone = phone;
        Address = address;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
