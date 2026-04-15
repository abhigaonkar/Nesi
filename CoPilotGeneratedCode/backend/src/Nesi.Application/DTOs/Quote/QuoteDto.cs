using Nesi.Domain.Enums;

namespace Nesi.Application.DTOs.Quote;

public class QuoteDto
{
    public int Id { get; set; }
    public string QuoteNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public QuoteType QuoteType { get; set; }
    public QuoteStatus Status { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public DateTime EstimatedStartDate { get; set; }
    public DateTime EstimatedCompletionDate { get; set; }
    public int? ProjectManagerId { get; set; }
    public string? ProjectManagerName { get; set; }
    public string TermsAndConditions { get; set; } = string.Empty;
    
    public decimal Subtotal { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
    
    public int? ApprovedBy { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? RejectionReason { get; set; }
    public int RevisionNumber { get; set; }
    
    public DateTime? CustomerApprovedAt { get; set; }
    public string? CustomerApprovedBy { get; set; }
    
    public int? WorkOrderId { get; set; }
    public DateTime? ConvertedToWorkOrderAt { get; set; }
    
    public List<QuoteLineItemDto> LineItems { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}
