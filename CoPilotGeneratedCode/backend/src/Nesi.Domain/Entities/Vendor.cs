namespace Nesi.Domain.Entities;

using Nesi.Domain.Enums;

/// <summary>
/// Vendor entity representing suppliers and service providers
/// </summary>
public class Vendor : BaseEntity
{
    public string VendorNumber { get; private set; } = string.Empty;
    public string CompanyName { get; private set; } = string.Empty;
    public string? ContactName { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Fax { get; private set; }
    public string? Website { get; private set; }
    
    // Address information
    public string? Address { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? ZipCode { get; private set; }
    public string? Country { get; private set; }
    
    // Business information
    public string? TaxId { get; private set; }
    public string? AccountNumber { get; private set; }
    public VendorStatus Status { get; private set; }
    public int? PaymentTermsDays { get; private set; }
    public decimal? CreditLimit { get; private set; }
    
    // Ratings and performance
    public decimal? Rating { get; private set; }
    public string? Notes { get; private set; }
    
    public bool IsActive { get; private set; } = true;
    
    // Navigation properties
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; private set; } = new List<PurchaseOrder>();
    
    // Private constructor for EF Core
    private Vendor() { }
    
    // Public constructor
    public Vendor(
        string vendorNumber,
        string companyName,
        string? email = null,
        string? phone = null)
    {
        if (string.IsNullOrWhiteSpace(vendorNumber))
            throw new ArgumentException("Vendor number cannot be empty", nameof(vendorNumber));
        
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name cannot be empty", nameof(companyName));
        
        VendorNumber = vendorNumber;
        CompanyName = companyName;
        Email = email;
        Phone = phone;
        Status = VendorStatus.Active;
        IsActive = true;
    }
    
    public void UpdateContactInfo(string? contactName, string? email, string? phone, string? fax = null, string? website = null)
    {
        ContactName = contactName;
        Email = email;
        Phone = phone;
        Fax = fax;
        Website = website;
    }
    
    public void UpdateAddress(string? address, string? city, string? state, string? zipCode, string? country = "USA")
    {
        Address = address;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
    }
    
    public void UpdateBusinessInfo(string? taxId, string? accountNumber, int? paymentTermsDays, decimal? creditLimit)
    {
        TaxId = taxId;
        AccountNumber = accountNumber;
        PaymentTermsDays = paymentTermsDays;
        CreditLimit = creditLimit;
    }
    
    public void UpdateStatus(VendorStatus status)
    {
        Status = status;
        IsActive = status == VendorStatus.Active;
    }
    
    public void UpdateRating(decimal rating)
    {
        if (rating < 0 || rating > 5)
            throw new ArgumentException("Rating must be between 0 and 5", nameof(rating));
        
        Rating = rating;
    }
    
    public void UpdateNotes(string? notes)
    {
        Notes = notes;
    }
    
    public void Activate()
    {
        Status = VendorStatus.Active;
        IsActive = true;
    }
    
    public void Deactivate()
    {
        Status = VendorStatus.Inactive;
        IsActive = false;
    }
    
    public void Suspend(string reason)
    {
        Status = VendorStatus.Suspended;
        IsActive = false;
        Notes = $"Suspended: {reason}\n{Notes}";
    }
}
