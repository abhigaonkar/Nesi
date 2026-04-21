namespace Nesi.Application.DTOs.PurchaseOrder;

public class PurchaseOrderDto
{
    public int Id { get; set; }
    public string PurchaseOrderNumber { get; set; } = string.Empty;
    public int VendorId { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public int? WorkOrderId { get; set; }
    public string? WorkOrderNumber { get; set; }
    public int RequestedBy { get; set; }
    public string RequesterName { get; set; } = string.Empty;
    public int? ApprovedBy { get; set; }
    public string? ApproverName { get; set; }
    
    public DateTime OrderDate { get; set; }
    public DateTime? RequiredByDate { get; set; }
    public DateTime? ApprovedAt { get; set; }
    
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    
    // Financial
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    
    // Shipping
    public string? ShippingAddress { get; set; }
    public string? ShippingCity { get; set; }
    public string? ShippingState { get; set; }
    public string? ShippingZipCode { get; set; }
    
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Related entities (optional, loaded when needed)
    public List<PurchaseOrderLineItemDto>? LineItems { get; set; }
    public List<PurchaseOrderReceiptDto>? Receipts { get; set; }
}
