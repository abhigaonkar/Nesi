namespace Nesi.Domain.Entities;

using Nesi.Domain.Enums;

/// <summary>
/// Work Order entity
/// </summary>
public class WorkOrder : BaseEntity
{
    public string WorkOrderNumber { get; private set; } = string.Empty;
    public int CustomerId { get; private set; }
    public int? QuoteId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public WorkOrderStatus Status { get; private set; }
    
    // Assignment and scheduling
    public int? ProjectManagerId { get; private set; }
    public DateTime? ScheduledStartDate { get; private set; }
    public DateTime? ScheduledEndDate { get; private set; }
    public string? Milestones { get; private set; }
    
    // Completion tracking
    public DateTime? CompletedAt { get; private set; }
    public int? CompletedBy { get; private set; }
    public string? CompletionNotes { get; private set; }
    
    // Invoicing
    public decimal? InvoiceAmount { get; private set; }
    public DateTime? InvoicedAt { get; private set; }
    public DateTime? PaidAt { get; private set; }
    
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public virtual Customer Customer { get; private set; } = null!;
    public virtual Quote? Quote { get; private set; }
    public virtual User? ProjectManager { get; private set; }
    public virtual ICollection<TimesheetEntry> TimesheetEntries { get; private set; } = new List<TimesheetEntry>();
    public virtual ICollection<Material> Materials { get; private set; } = new List<Material>();
    public virtual ICollection<WorkOrderAssignment> Assignments { get; private set; } = new List<WorkOrderAssignment>();
    public virtual ICollection<WorkOrderDocument> Documents { get; private set; } = new List<WorkOrderDocument>();

    // Private constructor for EF Core
    private WorkOrder() { }

    // Public constructor
    public WorkOrder(string workOrderNumber, int customerId, string description, DateTime startDate, int? quoteId = null)
    {
        if (string.IsNullOrWhiteSpace(workOrderNumber))
            throw new ArgumentException("Work order number cannot be empty", nameof(workOrderNumber));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        WorkOrderNumber = workOrderNumber;
        CustomerId = customerId;
        QuoteId = quoteId;
        Description = description;
        StartDate = startDate;
        Status = WorkOrderStatus.Created;
        IsActive = true;
    }

    public void AssignProjectManager(int projectManagerId, DateTime? scheduledStartDate = null, DateTime? scheduledEndDate = null)
    {
        ProjectManagerId = projectManagerId;
        ScheduledStartDate = scheduledStartDate;
        ScheduledEndDate = scheduledEndDate;
        
        if (Status == WorkOrderStatus.Created)
            Status = WorkOrderStatus.Assigned;
    }

    public void StartWork()
    {
        if (Status != WorkOrderStatus.Assigned && Status != WorkOrderStatus.Created)
            throw new InvalidOperationException("Work order must be assigned or created to start work");
        
        Status = WorkOrderStatus.InProgress;
    }

    public void Complete(int completedBy, string? completionNotes = null)
    {
        if (Status != WorkOrderStatus.InProgress)
            throw new InvalidOperationException("Can only complete work orders that are in progress");

        Status = WorkOrderStatus.Complete;
        CompletedAt = DateTime.UtcNow;
        CompletedBy = completedBy;
        CompletionNotes = completionNotes;
        EndDate = DateTime.UtcNow;
        IsActive = false;
    }

    public void Cancel(string reason)
    {
        if (Status == WorkOrderStatus.Closed)
            throw new InvalidOperationException("Cannot cancel a closed work order");
        
        Status = WorkOrderStatus.Cancelled;
        CompletionNotes = reason;
        IsActive = false;
    }

    public void Reopen()
    {
        if (Status != WorkOrderStatus.Complete && Status != WorkOrderStatus.Cancelled)
            throw new InvalidOperationException("Can only reopen complete or cancelled work orders");
        
        EndDate = null;
        CompletedAt = null;
        CompletedBy = null;
        Status = WorkOrderStatus.InProgress;
        IsActive = true;
    }

    public void GenerateInvoice(decimal invoiceAmount)
    {
        if (Status != WorkOrderStatus.Complete)
            throw new InvalidOperationException("Can only invoice completed work orders");
        
        if (invoiceAmount < 0)
            throw new ArgumentException("Invoice amount cannot be negative");
        
        InvoiceAmount = invoiceAmount;
        InvoicedAt = DateTime.UtcNow;
    }

    public void MarkAsPaid()
    {
        if (!InvoicedAt.HasValue)
            throw new InvalidOperationException("Work order must be invoiced before marking as paid");
        
        PaidAt = DateTime.UtcNow;
        Status = WorkOrderStatus.Closed;
    }

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        Description = description;
    }

    public void UpdateSchedule(DateTime? scheduledStartDate, DateTime? scheduledEndDate)
    {
        if (scheduledStartDate.HasValue && scheduledEndDate.HasValue && scheduledEndDate < scheduledStartDate)
            throw new ArgumentException("Scheduled end date cannot be before start date");
        
        ScheduledStartDate = scheduledStartDate;
        ScheduledEndDate = scheduledEndDate;
    }

    public void UpdateMilestones(string milestones)
    {
        Milestones = milestones;
    }
}

