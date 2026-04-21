using Nesi.Domain.Interfaces;

namespace Nesi.Application.Services;

public class ThreeWayMatchingService : IThreeWayMatchingService
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;

    public ThreeWayMatchingService(IPurchaseOrderRepository purchaseOrderRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
    }

    public async Task<ThreeWayMatchResult> PerformThreeWayMatchAsync(
        int purchaseOrderId,
        decimal invoiceTotal,
        List<InvoiceLineItem> invoiceLineItems,
        decimal tolerancePercentage = 5.0m,
        CancellationToken cancellationToken = default)
    {
        var result = new ThreeWayMatchResult
        {
            IsMatched = true,
            TolerancePercentage = tolerancePercentage
        };

        // 1. Get Purchase Order with line items and receipts
        var purchaseOrder = await _purchaseOrderRepository.GetWithDetailsAsync(purchaseOrderId, cancellationToken);
        
        if (purchaseOrder == null)
        {
            result.AddDiscrepancy("Purchase order not found");
            return result;
        }

        // Check PO is approved
        if (purchaseOrder.Status != Domain.Enums.PurchaseOrderStatus.Approved &&
            purchaseOrder.Status != Domain.Enums.PurchaseOrderStatus.Received &&
            purchaseOrder.Status != Domain.Enums.PurchaseOrderStatus.Closed)
        {
            result.AddDiscrepancy($"Purchase order status is {purchaseOrder.Status}, must be Approved or Received");
            return result;
        }

        result.PurchaseOrderTotal = purchaseOrder.TotalAmount;
        result.InvoiceTotal = invoiceTotal;

        // 2. Validate receipts exist
        if (purchaseOrder.Receipts == null || !purchaseOrder.Receipts.Any())
        {
            result.AddDiscrepancy("No receipts found for this purchase order");
            return result;
        }

        // Calculate total received quantity per line item
        var receivedQuantities = new Dictionary<int, decimal>();
        foreach (var receipt in purchaseOrder.Receipts)
        {
            if (receipt.ReceiptItems != null)
            {
                foreach (var receiptItem in receipt.ReceiptItems)
                {
                    if (!receivedQuantities.ContainsKey(receiptItem.PurchaseOrderLineItemId))
                        receivedQuantities[receiptItem.PurchaseOrderLineItemId] = 0;
                    
                    receivedQuantities[receiptItem.PurchaseOrderLineItemId] += receiptItem.QuantityReceived;
                }
            }
        }

        result.ReceiptTotal = receivedQuantities.Sum(r =>
        {
            var lineItem = purchaseOrder.LineItems?.FirstOrDefault(li => li.Id == r.Key);
            return lineItem != null ? r.Value * lineItem.UnitPrice : 0;
        });

        // 3. Match quantities between PO, Receipts, and Invoice
        result.QuantitiesMatch = true;
        foreach (var invoiceItem in invoiceLineItems)
        {
            var poLineItem = purchaseOrder.LineItems?.FirstOrDefault(li => li.Id == invoiceItem.PurchaseOrderLineItemId);
            
            if (poLineItem == null)
            {
                result.AddDiscrepancy($"Invoice line item references non-existent PO line item {invoiceItem.PurchaseOrderLineItemId}");
                continue;
            }

            // Check invoice quantity matches received quantity
            if (!receivedQuantities.TryGetValue(invoiceItem.PurchaseOrderLineItemId, out var receivedQty))
            {
                result.AddDiscrepancy($"Line item '{poLineItem.Description}': No receipt found");
                result.QuantitiesMatch = false;
                continue;
            }

            // Allow tolerance on quantities
            var qtyVariance = Math.Abs(invoiceItem.Quantity - receivedQty) / receivedQty * 100;
            if (qtyVariance > tolerancePercentage)
            {
                result.AddDiscrepancy(
                    $"Line item '{poLineItem.Description}': Quantity mismatch - " +
                    $"Received: {receivedQty}, Invoiced: {invoiceItem.Quantity} " +
                    $"(Variance: {qtyVariance:F2}%)");
                result.QuantitiesMatch = false;
            }
        }

        // 4. Match prices
        result.PricesMatch = true;
        foreach (var invoiceItem in invoiceLineItems)
        {
            var poLineItem = purchaseOrder.LineItems?.FirstOrDefault(li => li.Id == invoiceItem.PurchaseOrderLineItemId);
            
            if (poLineItem == null)
                continue;

            // Check unit prices match within tolerance
            var priceVariance = Math.Abs(invoiceItem.UnitPrice - poLineItem.UnitPrice) / poLineItem.UnitPrice * 100;
            if (priceVariance > tolerancePercentage)
            {
                result.AddDiscrepancy(
                    $"Line item '{poLineItem.Description}': Price mismatch - " +
                    $"PO Price: {poLineItem.UnitPrice:C}, Invoice Price: {invoiceItem.UnitPrice:C} " +
                    $"(Variance: {priceVariance:F2}%)");
                result.PricesMatch = false;
            }
        }

        // 5. Match totals (PO vs Receipt vs Invoice)
        result.TotalsMatch = true;
        
        // Compare PO total with invoice total
        var poInvoiceVariance = Math.Abs(result.InvoiceTotal - result.PurchaseOrderTotal) / result.PurchaseOrderTotal * 100;
        if (poInvoiceVariance > tolerancePercentage)
        {
            result.AddDiscrepancy(
                $"Total amount mismatch: PO Total: {result.PurchaseOrderTotal:C}, " +
                $"Invoice Total: {result.InvoiceTotal:C} " +
                $"(Variance: {poInvoiceVariance:F2}%)");
            result.TotalsMatch = false;
        }

        // Compare receipt total with invoice total
        var receiptInvoiceVariance = Math.Abs(result.InvoiceTotal - result.ReceiptTotal) / result.ReceiptTotal * 100;
        if (receiptInvoiceVariance > tolerancePercentage)
        {
            result.AddDiscrepancy(
                $"Receipt/Invoice mismatch: Receipt Total: {result.ReceiptTotal:C}, " +
                $"Invoice Total: {result.InvoiceTotal:C} " +
                $"(Variance: {receiptInvoiceVariance:F2}%)");
            result.TotalsMatch = false;
        }

        // Final match status
        result.IsMatched = result.QuantitiesMatch && result.PricesMatch && result.TotalsMatch;

        return result;
    }
}
