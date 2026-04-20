namespace Nesi.Domain.Entities;

/// <summary>
/// Employee Days Off entity - tracks vacation, sick days, and other time off
/// </summary>
public class EmployeeDaysOff : BaseEntity
{
    public int EmployeeId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public decimal Hours { get; private set; }
    public string DayOffType { get; private set; } = string.Empty; // Vacation, Sick, Personal, Unpaid, Holiday
    public string Status { get; private set; } = "Pending"; // Pending, Approved, Rejected, Cancelled
    public string? Reason { get; private set; }
    public string? Notes { get; private set; }
    public int? ApprovedByUserId { get; private set; }
    public DateTime? ApprovedDate { get; private set; }
    public int RequestedByUserId { get; private set; }
    public DateTime RequestedDate { get; private set; }

    // Navigation property
    public virtual Employee Employee { get; private set; } = null!;

    // Private constructor for EF Core
    private EmployeeDaysOff() { }

    // Public constructor
    public EmployeeDaysOff(
        int employeeId,
        DateTime startDate,
        DateTime endDate,
        decimal hours,
        string dayOffType,
        int requestedByUserId,
        string? reason = null)
    {
        if (string.IsNullOrWhiteSpace(dayOffType))
            throw new ArgumentException("Day off type cannot be empty", nameof(dayOffType));
        if (endDate < startDate)
            throw new ArgumentException("End date cannot be before start date");

        EmployeeId = employeeId;
        StartDate = startDate;
        EndDate = endDate;
        Hours = hours;
        DayOffType = dayOffType;
        RequestedByUserId = requestedByUserId;
        RequestedDate = DateTime.Now;
        Reason = reason;
        Status = "Pending";
    }

    public void Approve(int approvedByUserId)
    {
        Status = "Approved";
        ApprovedByUserId = approvedByUserId;
        ApprovedDate = DateTime.Now;
    }

    public void Reject(int rejectedByUserId, string? notes = null)
    {
        Status = "Rejected";
        ApprovedByUserId = rejectedByUserId;
        ApprovedDate = DateTime.Now;
        Notes = notes;
    }

    public void Cancel()
    {
        Status = "Cancelled";
    }

    public void UpdateDates(DateTime startDate, DateTime endDate, decimal hours)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date cannot be before start date");

        StartDate = startDate;
        EndDate = endDate;
        Hours = hours;
    }

    public void AddNotes(string notes)
    {
        Notes = notes;
    }
}
