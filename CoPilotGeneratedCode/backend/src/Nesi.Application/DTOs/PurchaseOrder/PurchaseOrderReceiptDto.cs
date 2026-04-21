namespace Nesi.Application.DTOs.PurchaseOrder;

public class PurchaseOrderReceiptDto
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public DateTime ReceivedDate { get; set; }
    public int ReceivedBy { get; set; }
    public string ReceiverName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? PackingSlipNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Related entities
    public List<PurchaseOrderReceiptItemDto>? ReceiptItems { get; set; }
}

public class PurchaseOrderReceiptItemDto
{
    public int Id { get; set; }
    public int PurchaseOrderReceiptId { get; set; }
    public int PurchaseOrderLineItemId { get; set; }
    public string ItemDescription { get; set; } = string.Empty;
    public decimal QuantityReceived { get; set; }
    public string? Condition { get; set; }
    public string? Notes { get; set; }
    public bool HasDiscrepancy { get; set; }
    public string? DiscrepancyReason { get; set; }
}
