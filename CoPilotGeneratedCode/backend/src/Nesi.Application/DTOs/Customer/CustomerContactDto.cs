namespace Nesi.Application.DTOs.Customer;

public class CustomerContactDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? CellPhone { get; set; }
    public bool IsPrimary { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
