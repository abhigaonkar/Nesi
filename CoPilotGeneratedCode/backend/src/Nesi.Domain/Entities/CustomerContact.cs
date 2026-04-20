namespace Nesi.Domain.Entities;

/// <summary>
/// Customer Contact entity - represents a contact person for a customer
/// </summary>
public class CustomerContact : BaseEntity
{
    public int CustomerId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Title { get; private set; }
    public string? Phone { get; private set; }
    public string? CellPhone { get; private set; }
    public string? DirectLine { get; private set; }
    public string? Extension { get; private set; }
    public string? Type { get; private set; }
    public DateTime? Birthday { get; private set; }
    public bool LoginEnabled { get; private set; }
    public string? Login { get; private set; }
    public bool IsPrimary { get; private set; }
    public string? Status { get; private set; }

    // Navigation property
    public virtual Customer Customer { get; private set; } = null!;

    // Private constructor for EF Core
    private CustomerContact() { }

    // Public constructor
    public CustomerContact(
        int customerId, 
        string name, 
        string? email = null, 
        string? title = null,
        string? phone = null,
        string? cellPhone = null,
        bool isPrimary = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Contact name cannot be empty", nameof(name));

        CustomerId = customerId;
        Name = name;
        Email = email;
        Title = title;
        Phone = phone;
        CellPhone = cellPhone;
        IsPrimary = isPrimary;
        Status = "Active";
    }

    public void UpdateInfo(string name, string? email, string? title, string? phone, string? cellPhone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Contact name cannot be empty", nameof(name));

        Name = name;
        Email = email;
        Title = title;
        Phone = phone;
        CellPhone = cellPhone;
    }

    public void UpdatePhoneDetails(string? directLine, string? extension)
    {
        DirectLine = directLine;
        Extension = extension;
    }

    public void SetBirthday(DateTime? birthday)
    {
        Birthday = birthday;
    }

    public void EnableLogin(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException("Login cannot be empty when enabling login", nameof(login));

        LoginEnabled = true;
        Login = login;
    }

    public void DisableLogin()
    {
        LoginEnabled = false;
        Login = null;
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
    }

    public void UnsetAsPrimary()
    {
        IsPrimary = false;
    }

    public void Deactivate()
    {
        Status = "Inactive";
    }

    public void Activate()
    {
        Status = "Active";
    }
}
