namespace Nesi.Application.DTOs.PurchaseOrder;

public class PurchaseOrderLineItemDto
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public int LineNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? PartNumber { get; set; }
    public decimal Quantity { get; set; }
    public string UnitOfMeasure { get; set; } = "EA";
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal QuantityReceived { get; set; }
    public decimal RemainingQuantity { get; set; }
    public bool IsFullyReceived { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
