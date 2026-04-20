namespace Nesi.Domain.Entities;

/// <summary>
/// Employee Wage History entity - tracks wage changes over time
/// </summary>
public class EmployeeWageHistory : BaseEntity
{
    public int EmployeeId { get; private set; }
    public decimal WageAmount { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public string? ChangeReason { get; private set; }
    public int ChangedByUserId { get; private set; }
    public decimal? PreviousWageAmount { get; private set; }
    public bool IsCurrent { get; private set; } = true;

    // Navigation property
    public virtual Employee Employee { get; private set; } = null!;

    // Private constructor for EF Core
    private EmployeeWageHistory() { }

    // Public constructor
    public EmployeeWageHistory(
        int employeeId,
        decimal wageAmount,
        DateTime effectiveDate,
        int changedByUserId,
        string? changeReason = null,
        decimal? previousWageAmount = null)
    {
        if (wageAmount < 0)
            throw new ArgumentException("Wage amount cannot be negative", nameof(wageAmount));

        EmployeeId = employeeId;
        WageAmount = wageAmount;
        EffectiveDate = effectiveDate;
        ChangedByUserId = changedByUserId;
        ChangeReason = changeReason;
        PreviousWageAmount = previousWageAmount;
        IsCurrent = true;
    }

    public void MarkAsHistorical()
    {
        IsCurrent = false;
    }

    public void UpdateReason(string reason)
    {
        ChangeReason = reason;
    }
}
