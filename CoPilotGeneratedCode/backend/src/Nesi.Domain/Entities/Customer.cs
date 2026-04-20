namespace Nesi.Domain.Entities;

/// <summary>
/// Customer entity - enhanced with comprehensive business properties
/// </summary>
public class Customer : BaseEntity
{
    // Basic Information
    public string CustomerNumber { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public int? BusinessUnitId { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsQualityChecked { get; private set; }
    public bool IsPartner { get; private set; }
    public bool IsKeyAccount { get; private set; }

    // Contact Information (Primary - for backward compatibility)
    public string? ContactName { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Address { get; private set; }

    // Financial Information
    public int? CreditType { get; private set; }
    public decimal CreditLimit { get; private set; }
    public decimal Discount { get; private set; }
    public decimal BudgetThreshold { get; private set; }
    public int? TermId { get; private set; }
    public bool ApplyFinanceCharges { get; private set; }
    
    // Account Management
    public int? AccountManagerId { get; private set; }
    public int? InsideSalesRepId { get; private set; }
    public int? OutsideSalesRepId { get; private set; }
    public int? RegionalAccountManagerId { get; private set; }
    public int? MajorAccountManagerId { get; private set; }
    public int? DecisionMakerId { get; private set; }

    // Status and Hold Information
    public string? HoldStatus { get; private set; }
    public string? HoldReason { get; private set; }
    public int? HoldByUserId { get; private set; }
    
    // Invoicing Preferences
    public int? DefaultInvoiceType { get; private set; }
    public int? StatementCode { get; private set; }
    public string? ServiceChargeCode { get; private set; }
    public string? TaxPrompt { get; private set; }
    public string? PriceCode { get; private set; }
    public string? PORequired { get; private set; }
    public int AutoInvoice { get; private set; }

    // Follow-up and Notes
    public string? Notes { get; private set; }
    public string? FollowUpNotes { get; private set; }
    public DateTime? NextFollowUpDate { get; private set; }
    public int FollowUpFrequencyDays { get; private set; }
    public int? YearEnd { get; private set; }

    // Navigation properties
    public virtual ICollection<CustomerContact> Contacts { get; private set; } = new List<CustomerContact>();
    public virtual ICollection<CustomerAddress> Addresses { get; private set; } = new List<CustomerAddress>();
    public virtual ICollection<CustomerNote> CustomerNotes { get; private set; } = new List<CustomerNote>();
    public virtual ICollection<WorkOrder> WorkOrders { get; private set; } = new List<WorkOrder>();
    public virtual ICollection<Quote> Quotes { get; private set; } = new List<Quote>();

    // Private constructor for EF Core
    private Customer() { }

    // Public constructor
    public Customer(
        string customerNumber,
        string name, 
        int? businessUnitId = null,
        string? contactName = null, 
        string? email = null, 
        string? phone = null, 
        string? address = null)
    {
        if (string.IsNullOrWhiteSpace(customerNumber))
            throw new ArgumentException("Customer number cannot be empty", nameof(customerNumber));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty", nameof(name));

        CustomerNumber = customerNumber;
        Name = name;
        BusinessUnitId = businessUnitId;
        ContactName = contactName;
        Email = email;
        Phone = phone;
        Address = address;
        IsActive = true;
    }

    // Update Methods
    public void UpdateBasicInfo(string name, int? businessUnitId, string? contactName, string? email, string? phone, string? address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty", nameof(name));

        Name = name;
        BusinessUnitId = businessUnitId;
        ContactName = contactName;
        Email = email;
        Phone = phone;
        Address = address;
    }

    public void UpdateFinancialInfo(int? creditType, decimal creditLimit, decimal discount, decimal budgetThreshold, int? termId)
    {
        CreditType = creditType;
        CreditLimit = creditLimit;
        Discount = discount;
        BudgetThreshold = budgetThreshold;
        TermId = termId;
    }

    public void AssignAccountManager(int? accountManagerId)
    {
        AccountManagerId = accountManagerId;
    }

    public void AssignSalesReps(int? insideSalesRepId, int? outsideSalesRepId, int? regionalAccountManagerId, int? majorAccountManagerId)
    {
        InsideSalesRepId = insideSalesRepId;
        OutsideSalesRepId = outsideSalesRepId;
        RegionalAccountManagerId = regionalAccountManagerId;
        MajorAccountManagerId = majorAccountManagerId;
    }

    public void SetHoldStatus(string? holdReason, int? holdByUserId)
    {
        HoldStatus = "Yes";
        HoldReason = holdReason;
        HoldByUserId = holdByUserId;
    }

    public void ReleaseHold()
    {
        HoldStatus = "No";
        HoldReason = null;
        HoldByUserId = null;
    }

    public void UpdateInvoicingPreferences(int? defaultInvoiceType, int? statementCode, string? serviceChargeCode, string? poRequired)
    {
        DefaultInvoiceType = defaultInvoiceType;
        StatementCode = statementCode;
        ServiceChargeCode = serviceChargeCode;
        PORequired = poRequired;
    }

    public void AddNote(string notes)
    {
        Notes = notes;
    }

    public void SetFollowUp(DateTime? nextDate, string? notes, int frequencyDays)
    {
        NextFollowUpDate = nextDate;
        FollowUpNotes = notes;
        FollowUpFrequencyDays = frequencyDays;
    }

    public void MarkAsKeyAccount()
    {
        IsKeyAccount = true;
    }

    public void MarkAsPartner()
    {
        IsPartner = true;
    }

    public void MarkAsQualityChecked()
    {
        IsQualityChecked = true;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
