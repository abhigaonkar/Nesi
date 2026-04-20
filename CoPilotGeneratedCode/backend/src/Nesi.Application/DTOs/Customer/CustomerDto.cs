namespace Nesi.Application.DTOs.Customer;

public class CustomerDto
{
    public int Id { get; set; }
    public string CustomerNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public int? BusinessUnitId { get; set; }
    public string? BusinessUnitName { get; set; }
    public int? AccountManagerId { get; set; }
    public string? AccountManagerName { get; set; }
    public decimal CreditLimit { get; set; }
    public int? TermId { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public List<CustomerContactDto> Contacts { get; set; } = new();
    public List<CustomerAddressDto> Addresses { get; set; } = new();
    public List<CustomerNoteDto> CustomerNotesList { get; set; } = new();
}
