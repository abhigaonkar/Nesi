namespace Nesi.Domain.Entities;

/// <summary>
/// Work Order entity
/// </summary>
public class WorkOrder : BaseEntity
{
    public string WorkOrderNumber { get; private set; } = string.Empty;
    public int CustomerId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public virtual Customer Customer { get; private set; } = null!;
    public virtual ICollection<TimesheetEntry> TimesheetEntries { get; private set; } = new List<TimesheetEntry>();

    // Private constructor for EF Core
    private WorkOrder() { }

    // Public constructor
    public WorkOrder(string workOrderNumber, int customerId, string description, DateTime startDate)
    {
        if (string.IsNullOrWhiteSpace(workOrderNumber))
            throw new ArgumentException("Work order number cannot be empty", nameof(workOrderNumber));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        WorkOrderNumber = workOrderNumber;
        CustomerId = customerId;
        Description = description;
        StartDate = startDate;
        IsActive = true;
    }

    public void Complete(DateTime endDate)
    {
        if (endDate < StartDate)
            throw new ArgumentException("End date cannot be before start date");

        EndDate = endDate;
        IsActive = false;
    }

    public void Reopen()
    {
        EndDate = null;
        IsActive = true;
    }

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        Description = description;
    }
}
