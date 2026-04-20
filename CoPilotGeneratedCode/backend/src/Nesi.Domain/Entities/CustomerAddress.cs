using Nesi.Domain.Enums;

namespace Nesi.Domain.Entities;

/// <summary>
/// Customer Address entity - represents shipping/billing addresses for a customer
/// </summary>
public class CustomerAddress : BaseEntity
{
    public int CustomerId { get; private set; }
    public AddressType AddressType { get; private set; }
    public string AddressLine1 { get; private set; } = string.Empty;
    public string? AddressLine2 { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public bool IsDefault { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation property
    public virtual Customer Customer { get; private set; } = null!;

    // Private constructor for EF Core
    private CustomerAddress() { }

    // Public constructor
    public CustomerAddress(
        int customerId,
        AddressType addressType,
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string zipCode,
        string country,
        bool isDefault = false)
    {
        if (string.IsNullOrWhiteSpace(addressLine1))
            throw new ArgumentException("Address line 1 cannot be empty", nameof(addressLine1));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be empty", nameof(country));

        CustomerId = customerId;
        AddressType = addressType;
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
        IsDefault = isDefault;
        IsActive = true;
    }

    public void UpdateAddress(
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string zipCode,
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
        State = state;
        ZipCode = zipCode;
        Country = country;
    }

    public void SetAddressType(AddressType addressType)
    {
        AddressType = addressType;
    }

    public void SetAsDefault(bool isDefault = true)
    {
        IsDefault = isDefault;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
