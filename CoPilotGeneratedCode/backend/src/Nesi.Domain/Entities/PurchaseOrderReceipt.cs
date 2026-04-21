namespace Nesi.Domain.Entities;

using Nesi.Domain.Enums;

/// <summary>
/// Purchase Order Receipt entity for tracking received items
/// </summary>
public class PurchaseOrderReceipt : BaseEntity
{
    public int PurchaseOrderId { get; private set; }
    public string ReceiptNumber { get; private set; } = string.Empty;
    public DateTime ReceivedDate { get; private set; }
    public int ReceivedBy { get; private set; }
    public ReceiptStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public string? PackingSlipNumber { get; private set; }
    
    // Navigation properties
    public virtual PurchaseOrder PurchaseOrder { get; private set; } = null!;
    public virtual User Receiver { get; private set; } = null!;
    public virtual ICollection<PurchaseOrderReceiptItem> ReceiptItems { get; private set; } = new List<PurchaseOrderReceiptItem>();
    
    // Private constructor for EF Core
    private PurchaseOrderReceipt() { }
    
    // Public constructor
    public PurchaseOrderReceipt(
        int purchaseOrderId,
        string receiptNumber,
        int receivedBy,
        DateTime? receivedDate = null,
        string? packingSlipNumber = null)
    {
        if (string.IsNullOrWhiteSpace(receiptNumber))
            throw new ArgumentException("Receipt number cannot be empty", nameof(receiptNumber));
        
        PurchaseOrderId = purchaseOrderId;
        ReceiptNumber = receiptNumber;
        ReceivedBy = receivedBy;
        ReceivedDate = receivedDate ?? DateTime.UtcNow;
        PackingSlipNumber = packingSlipNumber;
        Status = ReceiptStatus.Pending;
    }
    
    public void MarkAsReceived()
    {
        if (!ReceiptItems.Any())
            throw new InvalidOperationException("Cannot mark receipt as received without items");
        
        Status = ReceiptStatus.Received;
    }
    
    public void MarkAsPartiallyReceived()
    {
        Status = ReceiptStatus.PartiallyReceived;
    }
    
    public void MarkAsDiscrepancy(string reason)
    {
        Status = ReceiptStatus.Discrepancy;
        Notes = $"Discrepancy: {reason}\n{Notes}";
    }
    
    public void UpdateNotes(string notes)
    {
        Notes = notes;
    }
}
