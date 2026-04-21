namespace Nesi.Domain.Entities;

/// <summary>
/// Purchase Order Line Item entity
/// </summary>
public class PurchaseOrderLineItem : BaseEntity
{
    public int PurchaseOrderId { get; private set; }
    public int LineNumber { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string? PartNumber { get; private set; }
    public decimal Quantity { get; private set; }
    public string UnitOfMeasure { get; private set; } = "EA";
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice { get; private set; }
    public decimal QuantityReceived { get; private set; }
    public string? Notes { get; private set; }
    
    // Navigation properties
    public virtual PurchaseOrder PurchaseOrder { get; private set; } = null!;
    
    // Private constructor for EF Core
    private PurchaseOrderLineItem() { }
    
    // Public constructor
    public PurchaseOrderLineItem(
        int purchaseOrderId,
        int lineNumber,
        string description,
        decimal quantity,
        decimal unitPrice,
        string? partNumber = null,
        string unitOfMeasure = "EA")
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));
        
        PurchaseOrderId = purchaseOrderId;
        LineNumber = lineNumber;
        Description = description;
        PartNumber = partNumber;
        Quantity = quantity;
        UnitOfMeasure = unitOfMeasure;
        UnitPrice = unitPrice;
        TotalPrice = quantity * unitPrice;
        QuantityReceived = 0;
    }
    
    public void UpdateQuantity(decimal quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        Quantity = quantity;
        TotalPrice = quantity * UnitPrice;
    }
    
    public void UpdateUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));
        
        UnitPrice = unitPrice;
        TotalPrice = Quantity * unitPrice;
    }
    
    public void ReceiveQuantity(decimal quantityReceived)
    {
        if (quantityReceived <= 0)
            throw new ArgumentException("Quantity received must be greater than zero", nameof(quantityReceived));
        
        if (QuantityReceived + quantityReceived > Quantity)
            throw new InvalidOperationException("Cannot receive more than ordered quantity");
        
        QuantityReceived += quantityReceived;
    }
    
    public void RecordReceivedQuantity(decimal quantityReceived)
    {
        ReceiveQuantity(quantityReceived);
    }
    
    public bool IsFullyReceived() => QuantityReceived >= Quantity;
    
    public decimal RemainingQuantity() => Quantity - QuantityReceived;
    
    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        Description = description;
    }
}
