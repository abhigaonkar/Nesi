namespace Nesi.Domain.Entities;

/// <summary>
/// Purchase Order Receipt Item entity for individual line item receipts
/// </summary>
public class PurchaseOrderReceiptItem : BaseEntity
{
    public int PurchaseOrderReceiptId { get; private set; }
    public int PurchaseOrderLineItemId { get; private set; }
    public decimal QuantityReceived { get; private set; }
    public string? Condition { get; private set; }
    public string? Notes { get; private set; }
    public bool HasDiscrepancy { get; private set; }
    public string? DiscrepancyReason { get; private set; }
    
    // Navigation properties
    public virtual PurchaseOrderReceipt Receipt { get; private set; } = null!;
    public virtual PurchaseOrderLineItem LineItem { get; private set; } = null!;
    
    // Private constructor for EF Core
    private PurchaseOrderReceiptItem() { }
    
    // Public constructor
    public PurchaseOrderReceiptItem(
        int purchaseOrderReceiptId,
        int purchaseOrderLineItemId,
        decimal quantityReceived,
        string? condition = null,
        string? notes = null,
        bool hasDiscrepancy = false,
        string? discrepancyReason = null)
    {
        if (quantityReceived <= 0)
            throw new ArgumentException("Quantity received must be greater than zero", nameof(quantityReceived));
        
        PurchaseOrderReceiptId = purchaseOrderReceiptId;
        PurchaseOrderLineItemId = purchaseOrderLineItemId;
        QuantityReceived = quantityReceived;
        Condition = condition;
        Notes = notes;
        HasDiscrepancy = hasDiscrepancy;
        DiscrepancyReason = discrepancyReason;
    }
    
    public void ReportDiscrepancy(string reason)
    {
        HasDiscrepancy = true;
        DiscrepancyReason = reason;
    }
    
    public void ResolveDiscrepancy()
    {
        HasDiscrepancy = false;
        DiscrepancyReason = null;
    }
    
    public void UpdateQuantity(decimal quantityReceived)
    {
        if (quantityReceived <= 0)
            throw new ArgumentException("Quantity received must be greater than zero", nameof(quantityReceived));
        
        QuantityReceived = quantityReceived;
    }
    
    public void UpdateCondition(string condition)
    {
        Condition = condition;
    }
}
