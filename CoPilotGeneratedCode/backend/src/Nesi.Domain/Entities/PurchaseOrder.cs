namespace Nesi.Domain.Entities;

using Nesi.Domain.Enums;

/// <summary>
/// Purchase Order entity
/// </summary>
public class PurchaseOrder : BaseEntity
{
    public string PurchaseOrderNumber { get; private set; } = string.Empty;
    public int VendorId { get; private set; }
    public int? WorkOrderId { get; private set; }
    public int RequestedBy { get; private set; }
    public int? ApprovedBy { get; private set; }
    
    public DateTime OrderDate { get; private set; }
    public DateTime? RequiredByDate { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    
    public PurchaseOrderStatus Status { get; private set; }
    public string? Description { get; private set; }
    public string? Notes { get; private set; }
    
    // Financial information
    public decimal SubTotal { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal ShippingCost { get; private set; }
    public decimal TotalAmount { get; private set; }
    
    // Shipping information
    public string? ShippingAddress { get; private set; }
    public string? ShippingCity { get; private set; }
    public string? ShippingState { get; private set; }
    public string? ShippingZipCode { get; private set; }
    
    public bool IsActive { get; private set; } = true;
    
    // Navigation properties
    public virtual Vendor Vendor { get; private set; } = null!;
    public virtual WorkOrder? WorkOrder { get; private set; }
    public virtual User Requester { get; private set; } = null!;
    public virtual User? Approver { get; private set; }
    public virtual ICollection<PurchaseOrderLineItem> LineItems { get; private set; } = new List<PurchaseOrderLineItem>();
    public virtual ICollection<PurchaseOrderReceipt> Receipts { get; private set; } = new List<PurchaseOrderReceipt>();
    
    // Private constructor for EF Core
    private PurchaseOrder() { }
    
    // Public constructor
    public PurchaseOrder(
        string purchaseOrderNumber,
        int vendorId,
        int requestedBy,
        DateTime orderDate,
        string? description = null,
        int? workOrderId = null)
    {
        if (string.IsNullOrWhiteSpace(purchaseOrderNumber))
            throw new ArgumentException("PO number cannot be empty", nameof(purchaseOrderNumber));
        
        PurchaseOrderNumber = purchaseOrderNumber;
        VendorId = vendorId;
        WorkOrderId = workOrderId;
        RequestedBy = requestedBy;
        OrderDate = orderDate;
        Description = description;
        Status = PurchaseOrderStatus.Draft;
        IsActive = true;
        SubTotal = 0;
        TaxAmount = 0;
        ShippingCost = 0;
        TotalAmount = 0;
    }
    
    public void Submit()
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Only draft purchase orders can be submitted");
        
        if (!LineItems.Any())
            throw new InvalidOperationException("Cannot submit purchase order without line items");
        
        Status = PurchaseOrderStatus.Submitted;
    }
    
    public void Approve(int approvedBy)
    {
        if (Status != PurchaseOrderStatus.Submitted)
            throw new InvalidOperationException("Only submitted purchase orders can be approved");
        
        Status = PurchaseOrderStatus.Approved;
        ApprovedBy = approvedBy;
        ApprovedAt = DateTime.UtcNow;
    }
    
    public void Reject(int rejectedBy, string reason)
    {
        if (Status != PurchaseOrderStatus.Submitted)
            throw new InvalidOperationException("Only submitted purchase orders can be rejected");
        
        Status = PurchaseOrderStatus.Rejected;
        Notes = $"Rejected by User {rejectedBy}: {reason}\n{Notes}";
    }
    
    public void MarkAsOrdered()
    {
        if (Status != PurchaseOrderStatus.Approved)
            throw new InvalidOperationException("Only approved purchase orders can be marked as ordered");
        
        Status = PurchaseOrderStatus.Ordered;
    }
    
    public void ReceivePartial()
    {
        if (Status != PurchaseOrderStatus.Ordered && Status != PurchaseOrderStatus.PartiallyReceived)
            throw new InvalidOperationException("Invalid status for receiving items");
        
        Status = PurchaseOrderStatus.PartiallyReceived;
    }
    
    public void ReceiveComplete()
    {
        if (Status != PurchaseOrderStatus.Ordered && Status != PurchaseOrderStatus.PartiallyReceived)
            throw new InvalidOperationException("Invalid status for receiving items");
        
        Status = PurchaseOrderStatus.Received;
    }
    
    public void Close()
    {
        if (Status != PurchaseOrderStatus.Received)
            throw new InvalidOperationException("Only received purchase orders can be closed");
        
        Status = PurchaseOrderStatus.Closed;
        IsActive = false;
    }
    
    public void Cancel(string reason)
    {
        if (Status == PurchaseOrderStatus.Closed)
            throw new InvalidOperationException("Cannot cancel a closed purchase order");
        
        if (Status == PurchaseOrderStatus.Received || Status == PurchaseOrderStatus.PartiallyReceived)
            throw new InvalidOperationException("Cannot cancel a purchase order that has been received");
        
        Status = PurchaseOrderStatus.Cancelled;
        Notes = $"Cancelled: {reason}\n{Notes}";
        IsActive = false;
    }
    
    public void UpdateFinancials(decimal subTotal, decimal taxAmount, decimal shippingCost)
    {
        if (subTotal < 0 || taxAmount < 0 || shippingCost < 0)
            throw new ArgumentException("Financial amounts cannot be negative");
        
        SubTotal = subTotal;
        TaxAmount = taxAmount;
        ShippingCost = shippingCost;
        TotalAmount = subTotal + taxAmount + shippingCost;
    }
    
    public void UpdateShippingAddress(string address, string city, string state, string zipCode)
    {
        ShippingAddress = address;
        ShippingCity = city;
        ShippingState = state;
        ShippingZipCode = zipCode;
    }
    
    public void UpdateDescription(string description)
    {
        Description = description;
    }
    
    public void UpdateRequiredByDate(DateTime requiredByDate)
    {
        if (requiredByDate < OrderDate)
            throw new ArgumentException("Required by date cannot be before order date");
        
        RequiredByDate = requiredByDate;
    }
    
    public void MarkAsReceived()
    {
        ReceiveComplete();
    }
}
