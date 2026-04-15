namespace Nesi.Domain.Entities;

using Nesi.Domain.Enums;

/// <summary>
/// Quote entity representing a customer quote
/// </summary>
public class Quote : BaseEntity
{
    public string QuoteNumber { get; private set; } = string.Empty;
    public int CustomerId { get; private set; }
    public QuoteType QuoteType { get; private set; }
    public QuoteStatus Status { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string Scope { get; private set; } = string.Empty;
    public DateTime EstimatedStartDate { get; private set; }
    public DateTime EstimatedCompletionDate { get; private set; }
    public int? ProjectManagerId { get; private set; }
    public string TermsAndConditions { get; private set; } = string.Empty;
    
    // Financial fields
    public decimal Subtotal { get; private set; }
    public decimal TaxRate { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal DiscountPercent { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal Total { get; private set; }
    
    // Approval tracking
    public int? ApprovedBy { get; private set; }
    public DateTime? ApprovedAt { get; private set; }
    public string? RejectionReason { get; private set; }
    public int RevisionNumber { get; private set; }
    public int? ParentQuoteId { get; private set; }
    
    // Customer acceptance
    public DateTime? CustomerApprovedAt { get; private set; }
    public string? CustomerApprovedBy { get; private set; }
    
    // Conversion tracking
    public int? WorkOrderId { get; private set; }
    public DateTime? ConvertedToWorkOrderAt { get; private set; }
    
    // Navigation properties
    public virtual Customer Customer { get; private set; } = null!;
    public virtual User? ProjectManager { get; private set; }
    public virtual User? Approver { get; private set; }
    public virtual ICollection<QuoteLineItem> LineItems { get; private set; } = new List<QuoteLineItem>();
    public virtual WorkOrder? WorkOrder { get; private set; }
    public virtual Quote? ParentQuote { get; private set; }
    
    // Private constructor for EF Core
    private Quote() { }
    
    // Public constructor
    public Quote(
        string quoteNumber,
        int customerId,
        QuoteType quoteType,
        string description,
        string scope,
        DateTime estimatedStartDate,
        DateTime estimatedCompletionDate,
        int? projectManagerId = null,
        string termsAndConditions = "")
    {
        if (string.IsNullOrWhiteSpace(quoteNumber))
            throw new ArgumentException("Quote number cannot be empty", nameof(quoteNumber));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        if (estimatedCompletionDate < estimatedStartDate)
            throw new ArgumentException("Completion date cannot be before start date");
        
        QuoteNumber = quoteNumber;
        CustomerId = customerId;
        QuoteType = quoteType;
        Description = description;
        Scope = scope;
        EstimatedStartDate = estimatedStartDate;
        EstimatedCompletionDate = estimatedCompletionDate;
        ProjectManagerId = projectManagerId;
        TermsAndConditions = termsAndConditions;
        Status = QuoteStatus.Draft;
        RevisionNumber = 1;
    }
    
    public void UpdateHeaderInfo(
        QuoteType quoteType,
        string description,
        string scope,
        DateTime estimatedStartDate,
        DateTime estimatedCompletionDate,
        int? projectManagerId,
        string termsAndConditions)
    {
        if (Status != QuoteStatus.Draft && Status != QuoteStatus.Rejected)
            throw new InvalidOperationException("Can only update draft or rejected quotes");
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        if (estimatedCompletionDate < estimatedStartDate)
            throw new ArgumentException("Completion date cannot be before start date");
        
        QuoteType = quoteType;
        Description = description;
        Scope = scope;
        EstimatedStartDate = estimatedStartDate;
        EstimatedCompletionDate = estimatedCompletionDate;
        ProjectManagerId = projectManagerId;
        TermsAndConditions = termsAndConditions;
    }
    
    public void CalculateTotals(decimal taxRate)
    {
        Subtotal = LineItems.Sum(li => li.Total);
        TaxRate = taxRate;
        DiscountAmount = Subtotal * (DiscountPercent / 100);
        TaxAmount = (Subtotal - DiscountAmount) * (TaxRate / 100);
        Total = Subtotal - DiscountAmount + TaxAmount;
    }
    
    public void ApplyDiscount(decimal discountPercent)
    {
        if (discountPercent < 0 || discountPercent > 100)
            throw new ArgumentException("Discount percent must be between 0 and 100");
        
        DiscountPercent = discountPercent;
        CalculateTotals(TaxRate);
    }
    
    public void Submit()
    {
        if (Status != QuoteStatus.Draft && Status != QuoteStatus.Rejected)
            throw new InvalidOperationException("Can only submit draft or rejected quotes");
        
        if (!LineItems.Any())
            throw new InvalidOperationException("Quote must have at least one line item");
        
        Status = QuoteStatus.Submitted;
    }
    
    public void Approve(int approvedBy)
    {
        if (Status != QuoteStatus.Submitted)
            throw new InvalidOperationException("Can only approve submitted quotes");
        
        Status = QuoteStatus.Approved;
        ApprovedBy = approvedBy;
        ApprovedAt = DateTime.UtcNow;
        RejectionReason = null;
    }
    
    public void Reject(int rejectedBy, string reason)
    {
        if (Status != QuoteStatus.Submitted)
            throw new InvalidOperationException("Can only reject submitted quotes");
        
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Rejection reason is required", nameof(reason));
        
        Status = QuoteStatus.Rejected;
        ApprovedBy = rejectedBy;
        ApprovedAt = DateTime.UtcNow;
        RejectionReason = reason;
    }
    
    public void CustomerApprove(string approvedBy)
    {
        if (Status != QuoteStatus.Approved)
            throw new InvalidOperationException("Quote must be internally approved before customer approval");
        
        Status = QuoteStatus.CustomerApproved;
        CustomerApprovedAt = DateTime.UtcNow;
        CustomerApprovedBy = approvedBy;
    }
    
    public WorkOrder ConvertToWorkOrder()
    {
        if (Status != QuoteStatus.CustomerApproved)
            throw new InvalidOperationException("Can only convert customer-approved quotes to work orders");
        
        if (WorkOrderId.HasValue)
            throw new InvalidOperationException("Quote has already been converted to a work order");
        
        // Generate work order number based on quote number
        var workOrderNumber = $"WO-{QuoteNumber}";
        
        // Create work order from quote
        var workOrder = new WorkOrder(
            workOrderNumber,
            CustomerId,
            Description,
            EstimatedStartDate,
            this.Id);
        
        // Assign project manager if available
        if (ProjectManagerId.HasValue)
        {
            workOrder.AssignProjectManager(ProjectManagerId.Value, EstimatedStartDate, EstimatedCompletionDate);
        }
        
        return workOrder;
    }

    public void MarkAsConvertedToWorkOrder(int workOrderId)
    {
        WorkOrderId = workOrderId;
        ConvertedToWorkOrderAt = DateTime.UtcNow;
    }
    
    public Quote CreateRevision(string newQuoteNumber)
    {
        var revision = new Quote(
            newQuoteNumber,
            CustomerId,
            QuoteType,
            Description,
            Scope,
            EstimatedStartDate,
            EstimatedCompletionDate,
            ProjectManagerId,
            TermsAndConditions)
        {
            ParentQuoteId = this.Id,
            RevisionNumber = this.RevisionNumber + 1,
            TaxRate = this.TaxRate,
            DiscountPercent = this.DiscountPercent
        };
        
        return revision;
    }
}
