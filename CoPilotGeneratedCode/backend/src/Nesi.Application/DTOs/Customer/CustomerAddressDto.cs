using Nesi.Domain.Enums;

namespace Nesi.Application.DTOs.Customer;

public class CustomerAddressDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public AddressType AddressType { get; set; }
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
