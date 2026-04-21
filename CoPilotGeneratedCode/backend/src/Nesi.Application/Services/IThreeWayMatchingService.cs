namespace Nesi.Application.Services;

/// <summary>
/// Result of 3-way matching validation
/// </summary>
public class ThreeWayMatchResult
{
    public bool IsMatched { get; set; }
    public List<string> Discrepancies { get; set; } = new();
    public decimal PurchaseOrderTotal { get; set; }
    public decimal ReceiptTotal { get; set; }
    public decimal InvoiceTotal { get; set; }
    public bool QuantitiesMatch { get; set; }
    public bool PricesMatch { get; set; }
    public bool TotalsMatch { get; set; }
    public decimal TolerancePercentage { get; set; }
    
    public void AddDiscrepancy(string discrepancy)
    {
        Discrepancies.Add(discrepancy);
        IsMatched = false;
    }
}

/// <summary>
/// Service for performing 3-way matching between PO, Receipt, and Invoice
/// </summary>
public interface IThreeWayMatchingService
{
    /// <summary>
    /// Performs 3-way match validation
    /// </summary>
    /// <param name="purchaseOrderId">Purchase order ID</param>
    /// <param name="invoiceTotal">Invoice total amount</param>
    /// <param name="invoiceLineItems">Invoice line items with quantities and prices</param>
    /// <param name="tolerancePercentage">Acceptable variance percentage (default 5%)</param>
    Task<ThreeWayMatchResult> PerformThreeWayMatchAsync(
        int purchaseOrderId,
        decimal invoiceTotal,
        List<InvoiceLineItem> invoiceLineItems,
        decimal tolerancePercentage = 5.0m,
        CancellationToken cancellationToken = default);
}

public class InvoiceLineItem
{
    public int PurchaseOrderLineItemId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
}
