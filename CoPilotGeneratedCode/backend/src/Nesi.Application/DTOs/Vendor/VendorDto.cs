namespace Nesi.Application.DTOs.Vendor;

public class VendorDto
{
    public int Id { get; set; }
    public string VendorNumber { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? Website { get; set; }
    
    // Address
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    
    // Business Info
    public string? TaxId { get; set; }
    public string? AccountNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? PaymentTermsDays { get; set; }
    public decimal? CreditLimit { get; set; }
    
    // Performance
    public decimal? Rating { get; set; }
    public string? Notes { get; set; }
    
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
