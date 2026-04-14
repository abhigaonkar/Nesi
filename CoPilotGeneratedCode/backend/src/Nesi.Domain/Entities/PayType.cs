namespace Nesi.Domain.Entities;

/// <summary>
/// Pay Type lookup entity (Regular, Overtime, Double Time, etc.)
/// </summary>
public class PayType : BaseEntity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public decimal Multiplier { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public virtual ICollection<TimesheetEntry> TimesheetEntries { get; private set; } = new List<TimesheetEntry>();

    // Private constructor for EF Core
    private PayType() { }

    // Public constructor
    public PayType(string code, string name, decimal multiplier)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty", nameof(code));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (multiplier <= 0)
            throw new ArgumentException("Multiplier must be greater than 0", nameof(multiplier));

        Code = code;
        Name = name;
        Multiplier = multiplier;
        IsActive = true;
    }

    public void UpdateMultiplier(decimal multiplier)
    {
        if (multiplier <= 0)
            throw new ArgumentException("Multiplier must be greater than 0", nameof(multiplier));

        Multiplier = multiplier;
    }
}
