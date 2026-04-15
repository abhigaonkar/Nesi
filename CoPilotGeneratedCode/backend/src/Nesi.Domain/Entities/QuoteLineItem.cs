namespace Nesi.Domain.Entities;

using Nesi.Domain.Enums;

/// <summary>
/// Quote line item representing labor, materials, equipment, or miscellaneous costs
/// </summary>
public class QuoteLineItem : BaseEntity
{
    public int QuoteId { get; private set; }
    public QuoteLineItemType ItemType { get; private set; }
    public int LineNumber { get; private set; }
    public string Description { get; private set; } = string.Empty;
    
    // For Labor
    public int? JobTypeId { get; private set; }
    public decimal EstimatedHours { get; private set; }
    
    // For Materials/Equipment
    public string? PartNumber { get; private set; }
    public decimal Quantity { get; private set; }
    
    // Common
    public decimal UnitPrice { get; private set; }
    public decimal Total { get; private set; }
    public string? Notes { get; private set; }
    
    // Navigation properties
    public virtual Quote Quote { get; private set; } = null!;
    public virtual JobType? JobType { get; private set; }
    
    // Private constructor for EF Core
    private QuoteLineItem() { }
    
    // Constructor for Labor
    public static QuoteLineItem CreateLabor(
        int quoteId,
        int lineNumber,
        string description,
        int jobTypeId,
        decimal estimatedHours,
        decimal hourlyRate,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        if (estimatedHours <= 0)
            throw new ArgumentException("Estimated hours must be greater than zero", nameof(estimatedHours));
        
        if (hourlyRate < 0)
            throw new ArgumentException("Hourly rate cannot be negative", nameof(hourlyRate));
        
        var lineItem = new QuoteLineItem
        {
            QuoteId = quoteId,
            ItemType = QuoteLineItemType.Labor,
            LineNumber = lineNumber,
            Description = description,
            JobTypeId = jobTypeId,
            EstimatedHours = estimatedHours,
            UnitPrice = hourlyRate,
            Notes = notes
        };
        
        lineItem.CalculateTotal();
        return lineItem;
    }
    
    // Constructor for Material
    public static QuoteLineItem CreateMaterial(
        int quoteId,
        int lineNumber,
        string description,
        string partNumber,
        decimal quantity,
        decimal unitPrice,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));
        
        var lineItem = new QuoteLineItem
        {
            QuoteId = quoteId,
            ItemType = QuoteLineItemType.Material,
            LineNumber = lineNumber,
            Description = description,
            PartNumber = partNumber,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Notes = notes
        };
        
        lineItem.CalculateTotal();
        return lineItem;
    }
    
    // Constructor for Equipment
    public static QuoteLineItem CreateEquipment(
        int quoteId,
        int lineNumber,
        string description,
        decimal rentalPeriod,
        decimal rate,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        if (rentalPeriod <= 0)
            throw new ArgumentException("Rental period must be greater than zero", nameof(rentalPeriod));
        
        if (rate < 0)
            throw new ArgumentException("Rate cannot be negative", nameof(rate));
        
        var lineItem = new QuoteLineItem
        {
            QuoteId = quoteId,
            ItemType = QuoteLineItemType.Equipment,
            LineNumber = lineNumber,
            Description = description,
            Quantity = rentalPeriod,
            UnitPrice = rate,
            Notes = notes
        };
        
        lineItem.CalculateTotal();
        return lineItem;
    }
    
    // Constructor for Miscellaneous
    public static QuoteLineItem CreateMiscellaneous(
        int quoteId,
        int lineNumber,
        string description,
        decimal quantity,
        decimal unitPrice,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));
        
        var lineItem = new QuoteLineItem
        {
            QuoteId = quoteId,
            ItemType = QuoteLineItemType.Miscellaneous,
            LineNumber = lineNumber,
            Description = description,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Notes = notes
        };
        
        lineItem.CalculateTotal();
        return lineItem;
    }
    
    public void UpdateQuantity(decimal quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        if (ItemType == QuoteLineItemType.Labor)
            EstimatedHours = quantity;
        else
            Quantity = quantity;
        
        CalculateTotal();
    }
    
    public void UpdateUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));
        
        UnitPrice = unitPrice;
        CalculateTotal();
    }
    
    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        Description = description;
    }
    
    private void CalculateTotal()
    {
        if (ItemType == QuoteLineItemType.Labor)
            Total = EstimatedHours * UnitPrice;
        else
            Total = Quantity * UnitPrice;
    }
}
