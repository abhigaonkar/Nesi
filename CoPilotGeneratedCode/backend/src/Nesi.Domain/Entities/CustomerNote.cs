namespace Nesi.Domain.Entities;

/// <summary>
/// Customer Note entity - represents notes/history for a customer
/// </summary>
public class CustomerNote : BaseEntity
{
    public int CustomerId { get; private set; }
    public string Note { get; private set; } = string.Empty;
    public int CreatedByUserId { get; private set; }
    public string? Category { get; private set; }
    public bool IsImportant { get; private set; }

    // Navigation property
    public virtual Customer Customer { get; private set; } = null!;

    // Private constructor for EF Core
    private CustomerNote() { }

    // Public constructor
    public CustomerNote(int customerId, string note, int createdByUserId, string? category = null, bool isImportant = false)
    {
        if (string.IsNullOrWhiteSpace(note))
            throw new ArgumentException("Note cannot be empty", nameof(note));

        CustomerId = customerId;
        Note = note;
        CreatedByUserId = createdByUserId;
        Category = category;
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

    public void SetCategory(string? category)
    {
        Category = category;
    }
}
