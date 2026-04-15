namespace Nesi.Domain.Entities;

/// <summary>
/// Material entity representing materials used in work orders
/// </summary>
public class Material : BaseEntity
{
    public int WorkOrderId { get; private set; }
    public string PartNumber { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Quantity { get; private set; }
    public decimal UnitCost { get; private set; }
    public decimal TotalCost { get; private set; }
    public string? PurchaseOrderNumber { get; private set; }
    public DateTime? ReceivedDate { get; private set; }
    public string? Supplier { get; private set; }
    public string? Notes { get; private set; }
    
    // Navigation properties
    public virtual WorkOrder WorkOrder { get; private set; } = null!;
    
    // Private constructor for EF Core
    private Material() { }
    
    // Public constructor
    public Material(
        int workOrderId,
        string partNumber,
        string description,
        decimal quantity,
        decimal unitCost,
        string? purchaseOrderNumber = null,
        string? supplier = null,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(partNumber))
            throw new ArgumentException("Part number cannot be empty", nameof(partNumber));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        if (unitCost < 0)
            throw new ArgumentException("Unit cost cannot be negative", nameof(unitCost));
        
        WorkOrderId = workOrderId;
        PartNumber = partNumber;
        Description = description;
        Quantity = quantity;
        UnitCost = unitCost;
        TotalCost = quantity * unitCost;
        PurchaseOrderNumber = purchaseOrderNumber;
        Supplier = supplier;
        Notes = notes;
    }
    
    public void MarkAsReceived(DateTime receivedDate)
    {
        if (ReceivedDate.HasValue)
            throw new InvalidOperationException("Material already marked as received");
        
        ReceivedDate = receivedDate;
    }
    
    public void UpdateQuantityAndCost(decimal quantity, decimal unitCost)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        if (unitCost < 0)
            throw new ArgumentException("Unit cost cannot be negative", nameof(unitCost));
        
        Quantity = quantity;
        UnitCost = unitCost;
        TotalCost = quantity * unitCost;
    }
}
