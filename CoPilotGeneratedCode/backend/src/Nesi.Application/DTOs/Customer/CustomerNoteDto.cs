namespace Nesi.Application.DTOs.Customer;

public class CustomerNoteDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Note { get; set; } = string.Empty;
    public int CreatedByUserId { get; set; }
    public string? CreatedByUserName { get; set; }
    public DateTime CreatedAt { get; set; }
}
