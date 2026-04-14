using Nesi.Domain.Enums;

namespace Nesi.Domain.Entities;

/// <summary>
/// Timesheet entry entity representing employee work hours
/// </summary>
public class TimesheetEntry : BaseEntity
{
    public int UserId { get; private set; }
    public DateTime Date { get; private set; }
    public decimal Hours { get; private set; }
    public int? WorkOrderId { get; private set; }
    public int PayTypeId { get; private set; }
    public int? JobTypeId { get; private set; }
    public string Notes { get; private set; } = string.Empty;
    public TimesheetStatus Status { get; private set; }

    // Navigation properties
    public virtual User User { get; private set; } = null!;
    public virtual WorkOrder? WorkOrder { get; private set; }
    public virtual PayType PayType { get; private set; } = null!;
    public virtual JobType? JobType { get; private set; }

    // Private constructor for EF Core
    private TimesheetEntry() { }

    // Public constructor
    public TimesheetEntry(int userId, DateTime date, decimal hours, int payTypeId, int? workOrderId = null, int? jobTypeId = null, string notes = "")
    {
        if (hours <= 0 || hours > 24)
            throw new ArgumentException("Hours must be between 0 and 24", nameof(hours));

        if (date > DateTime.Today)
            throw new ArgumentException("Cannot create timesheet for future dates", nameof(date));

        UserId = userId;
        Date = date.Date; // Normalize to date only
        Hours = hours;
        PayTypeId = payTypeId;
        WorkOrderId = workOrderId;
        JobTypeId = jobTypeId;
        Notes = notes ?? string.Empty;
        Status = TimesheetStatus.Draft;
    }

    public void UpdateHours(decimal newHours)
    {
        if (Status != TimesheetStatus.Draft)
            throw new InvalidOperationException("Can only update draft timesheets");

        if (newHours <= 0 || newHours > 24)
            throw new ArgumentException("Hours must be between 0 and 24");

        Hours = newHours;
    }

    public void UpdateNotes(string notes)
    {
        if (Status != TimesheetStatus.Draft)
            throw new InvalidOperationException("Can only update draft timesheets");

        Notes = notes ?? string.Empty;
    }

    public void UpdateDetails(DateTime date, decimal hours, int payTypeId, int? workOrderId, int? jobTypeId, string? notes)
    {
        if (Status != TimesheetStatus.Draft)
            throw new InvalidOperationException("Can only update draft timesheets");

        if (hours <= 0 || hours > 24)
            throw new ArgumentException("Hours must be between 0 and 24");

        if (date > DateTime.Today)
            throw new ArgumentException("Cannot set timesheet to future dates", nameof(date));

        Date = date.Date;
        Hours = hours;
        PayTypeId = payTypeId;
        WorkOrderId = workOrderId;
        JobTypeId = jobTypeId;
        Notes = notes ?? string.Empty;
    }

    public void Submit()
    {
        if (Status != TimesheetStatus.Draft)
            throw new InvalidOperationException("Only draft timesheets can be submitted");

        Status = TimesheetStatus.Submitted;
    }

    public void Approve()
    {
        if (Status != TimesheetStatus.Submitted)
            throw new InvalidOperationException("Only submitted timesheets can be approved");

        Status = TimesheetStatus.Approved;
    }

    public void Reject()
    {
        if (Status != TimesheetStatus.Submitted)
            throw new InvalidOperationException("Only submitted timesheets can be rejected");

        Status = TimesheetStatus.Rejected;
    }

    public void ReturnToDraft()
    {
        if (Status != TimesheetStatus.Rejected)
            throw new InvalidOperationException("Only rejected timesheets can be returned to draft");

        Status = TimesheetStatus.Draft;
    }
}
