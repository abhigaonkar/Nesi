namespace Nesi.Application.DTOs.Customer;

public class CustomerContactDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public bool IsPrimaryContact { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
