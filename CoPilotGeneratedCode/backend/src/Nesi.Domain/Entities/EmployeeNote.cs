namespace Nesi.Domain.Entities;

/// <summary>
/// Employee Note entity - represents notes/history for an employee
/// </summary>
public class EmployeeNote : BaseEntity
{
    public int EmployeeId { get; private set; }
    public string Note { get; private set; } = string.Empty;
    public int CreatedByUserId { get; private set; }
    public string? Category { get; private set; } // Performance, Disciplinary, General, etc.
    public bool IsConfidential { get; private set; }
    public bool IsImportant { get; private set; }

    // Navigation property
    public virtual Employee Employee { get; private set; } = null!;

    // Private constructor for EF Core
    private EmployeeNote() { }

    // Public constructor
    public EmployeeNote(
        int employeeId,
        string note,
        int createdByUserId,
        string? category = null,
        bool isConfidential = false,
        bool isImportant = false)
    {
        if (string.IsNullOrWhiteSpace(note))
            throw new ArgumentException("Note cannot be empty", nameof(note));

        EmployeeId = employeeId;
        Note = note;
        CreatedByUserId = createdByUserId;
        Category = category;
        IsConfidential = isConfidential;
        IsImportant = isImportant;
    }

    public void UpdateNote(string note)
    {
        if (string.IsNullOrWhiteSpace(note))
            throw new ArgumentException("Note cannot be empty", nameof(note));

        Note = note;
    }

    public void MarkAsImportant()
    {
        IsImportant = true;
    }

    public void UnmarkAsImportant()
    {
        IsImportant = false;
    }

    public void MarkAsConfidential()
    {
        IsConfidential = true;
    }

    public void SetCategory(string? category)
    {
        Category = category;
    }
}
