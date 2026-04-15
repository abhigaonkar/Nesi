using Nesi.Domain.Enums;

namespace Nesi.Application.DTOs.Quote;

public class QuoteLineItemDto
{
    public int Id { get; set; }
    public int QuoteId { get; set; }
    public QuoteLineItemType ItemType { get; set; }
    public int LineNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    
    public int? JobTypeId { get; set; }
    public string? JobTypeName { get; set; }
    public decimal EstimatedHours { get; set; }
    
    public string? PartNumber { get; set; }
    public decimal Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
    public string? Notes { get; set; }
}
