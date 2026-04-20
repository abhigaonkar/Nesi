namespace Nesi.Domain.Entities;

/// <summary>
/// Customer Address entity - represents shipping/billing addresses for a customer
/// </summary>
public class CustomerAddress : BaseEntity
{
    public int CustomerId { get; private set; }
    public string AddressLine1 { get; private set; } = string.Empty;
    public string? AddressLine2 { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string? StateProvince { get; private set; }
    public string? PostalCode { get; private set; }
    public string Country { get; private set; } = string.Empty;
    public string AddressType { get; private set; } = "Shipping"; // Shipping, Billing, Both
    public bool IsPrimary { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation property
    public virtual Customer Customer { get; private set; } = null!;

    // Private constructor for EF Core
    private CustomerAddress() { }

    // Public constructor
    public CustomerAddress(
        int customerId,
        string addressLine1,
        string city,
        string country,
        string? addressLine2 = null,
        string? stateProvince = null,
        string? postalCode = null,
        string addressType = "Shipping",
        bool isPrimary = false)
    {
        if (string.IsNullOrWhiteSpace(addressLine1))
            throw new ArgumentException("Address line 1 cannot be empty", nameof(addressLine1));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be empty", nameof(country));

        CustomerId = customerId;
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        City = city;
        StateProvince = stateProvince;
        PostalCode = postalCode;
        Country = country;
        AddressType = addressType;
        IsPrimary = isPrimary;
        IsActive = true;
    }

    public void UpdateAddress(
        string addressLine1,
        string? addressLine2,
        string city,
        string? stateProvince,
        string? postalCode,
        string country)
    {
        if (string.IsNullOrWhiteSpace(addressLine1))
            throw new ArgumentException("Address line 1 cannot be empty", nameof(addressLine1));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be empty", nameof(country));

        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        City = city;
        StateProvince = stateProvince;
        PostalCode = postalCode;
        Country = country;
    }

    public void SetAddressType(string addressType)
    {
        if (string.IsNullOrWhiteSpace(addressType))
            throw new ArgumentException("Address type cannot be empty", nameof(addressType));

        AddressType = addressType;
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
    }

    public void UnsetAsPrimary()
    {
        IsPrimary = false;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
