namespace Nesi.Domain.Entities;

/// <summary>
/// Employee (Member) entity - represents an employee/member of the organization
/// </summary>
public class Employee : BaseEntity
{
    // Basic Information
    public string EmployeeNumber { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? MiddleInitial { get; private set; }
    public string? Nickname { get; private set; }
    public string FullName => $"{FirstName} {LastName}";
    public string? Title { get; private set; }
    public string? Email { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Contact Information
    public string? PhoneAreaCode { get; private set; }
    public string? PhoneFirst { get; private set; }
    public string? PhoneLast { get; private set; }
    public string? PhoneExtension { get; private set; }
    public string? CellPhoneAreaCode { get; private set; }
    public string? CellPhoneFirst { get; private set; }
    public string? CellPhoneLast { get; private set; }

    // Address Information
    public string? Address { get; private set; }
    public string? City { get; private set; }
    public string? Province { get; private set; }
    public string? PostalCode { get; private set; }
    public string? Country { get; private set; }

    // Employment Information
    public int? MemberTypeId { get; private set; }
    public int? BusinessUnitId { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? TerminationDate { get; private set; }
    public string Status { get; private set; } = "Active"; // Active, Terminated, On Leave, etc.
    public int? ReportsToEmployeeId { get; private set; }
    public bool IsPartTime { get; private set; }

    // Payroll Information
    public int? PayTypeId { get; private set; }
    public decimal CurrentWage { get; private set; }
    public string? PayrollId { get; private set; }
    public string? PayrollHandler { get; private set; }
    public string? SIN { get; private set; } // Social Insurance Number
    public bool ReceivesStatPay { get; private set; } = true;

    // Personal Information
    public DateTime? BirthDate { get; private set; }
    public string? DriversLicense { get; private set; }
    public string? ElectricalLicense { get; private set; }
    
    // Emergency Contact Information
    public string? EmergencyContactFirstName1 { get; private set; }
    public string? EmergencyContactLastName1 { get; private set; }
    public string? EmergencyContactPhone1 { get; private set; }
    public string? EmergencyContactFirstName2 { get; private set; }
    public string? EmergencyContactLastName2 { get; private set; }
    public string? EmergencyContactPhone2 { get; private set; }

    // Benefits and Compensation
    public string? BenefitsId { get; private set; }
    public string? LifeInsuranceId { get; private set; }
    public DateTime? BenefitsStartDate { get; private set; }
    public bool HasCompensation { get; private set; }
    public string? CompensationDetails { get; private set; }

    // Equipment and Resources
    public bool GetsVehicle { get; private set; }
    public bool GetsPhone { get; private set; }
    public bool GetsLaptop { get; private set; }
    public bool GetsBarcodeScanner { get; private set; }
    public bool GetsBusinessCards { get; private set; }
    public bool GetsDirectDeposit { get; private set; }
    public bool GetsCompanyEmail { get; private set; }
    public bool GetsPhoneExtension { get; private set; }

    // Vacation and Leave
    public int VacationInterval1 { get; private set; }
    public decimal VacationAmount1 { get; private set; }
    public int VacationInterval2 { get; private set; }
    public decimal VacationAmount2 { get; private set; }
    public int VacationInterval3 { get; private set; }
    public decimal VacationAmount3 { get; private set; }
    public decimal CurrentVacationAmount { get; private set; }
    public bool ReceivesAutoVacationPayout { get; private set; }

    // Training and Development
    public int? ApprenticeLevel { get; private set; }
    public string? ApprenticeContract { get; private set; }

    // System and Administrative
    public string? LoginUsername { get; private set; }
    public int? DefaultLocationId { get; private set; }
    public int? DefaultPageId { get; private set; }
    public bool IncludeInMobileContactList { get; private set; } = true;
    public bool IsBoardMemberUS { get; private set; }
    public bool IsBoardMemberCAN { get; private set; }
    public string? Color { get; private set; } // For calendar/scheduling
    public string? Notes { get; private set; }
    public string? BonusNotes { get; private set; }
    public string? OfferNotes { get; private set; }
    public bool ProbationEmailSent { get; private set; }
    public bool WelcomeEmailSent { get; private set; }

    // Navigation properties
    public virtual ICollection<EmployeeDaysOff> DaysOff { get; private set; } = new List<EmployeeDaysOff>();
    public virtual ICollection<EmployeeWageHistory> WageHistory { get; private set; } = new List<EmployeeWageHistory>();
    public virtual ICollection<EmployeeNote> Notes_Collection { get; private set; } = new List<EmployeeNote>();

    // Private constructor for EF Core
    private Employee() { }

    // Public constructor
    public Employee(
        string employeeNumber,
        string firstName,
        string lastName,
        string? email = null,
        int? businessUnitId = null,
        DateTime? startDate = null)
    {
        if (string.IsNullOrWhiteSpace(employeeNumber))
            throw new ArgumentException("Employee number cannot be empty", nameof(employeeNumber));
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        EmployeeNumber = employeeNumber;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        BusinessUnitId = businessUnitId;
        StartDate = startDate ?? DateTime.Now;
        IsActive = true;
        Status = "Active";
    }

    // Update Methods
    public void UpdateBasicInfo(string firstName, string lastName, string? middleInitial, string? nickname, string? title, string? email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
        MiddleInitial = middleInitial;
        Nickname = nickname;
        Title = title;
        Email = email;
    }

    public void UpdateContactInfo(
        string? phoneAreaCode, string? phoneFirst, string? phoneLast, string? phoneExtension,
        string? cellPhoneAreaCode, string? cellPhoneFirst, string? cellPhoneLast)
    {
        PhoneAreaCode = phoneAreaCode;
        PhoneFirst = phoneFirst;
        PhoneLast = phoneLast;
        PhoneExtension = phoneExtension;
        CellPhoneAreaCode = cellPhoneAreaCode;
        CellPhoneFirst = cellPhoneFirst;
        CellPhoneLast = cellPhoneLast;
    }

    public void UpdateAddress(string? address, string? city, string? province, string? postalCode, string? country)
    {
        Address = address;
        City = city;
        Province = province;
        PostalCode = postalCode;
        Country = country;
    }

    public void UpdateEmploymentInfo(int? memberTypeId, int? businessUnitId, int? reportsToEmployeeId, bool isPartTime)
    {
        MemberTypeId = memberTypeId;
        BusinessUnitId = businessUnitId;
        ReportsToEmployeeId = reportsToEmployeeId;
        IsPartTime = isPartTime;
    }

    public void UpdatePayrollInfo(int? payTypeId, decimal wage, string? payrollId, string? payrollHandler, string? sin)
    {
        PayTypeId = payTypeId;
        CurrentWage = wage;
        PayrollId = payrollId;
        PayrollHandler = payrollHandler;
        SIN = sin;
    }

    public void SetBirthDate(DateTime? birthDate)
    {
        BirthDate = birthDate;
    }

    public void UpdateEmergencyContact(
        string? firstName1, string? lastName1, string? phone1,
        string? firstName2, string? lastName2, string? phone2)
    {
        EmergencyContactFirstName1 = firstName1;
        EmergencyContactLastName1 = lastName1;
        EmergencyContactPhone1 = phone1;
        EmergencyContactFirstName2 = firstName2;
        EmergencyContactLastName2 = lastName2;
        EmergencyContactPhone2 = phone2;
    }

    public void UpdateBenefits(string? benefitsId, string? lifeInsuranceId, DateTime? benefitsStartDate)
    {
        BenefitsId = benefitsId;
        LifeInsuranceId = lifeInsuranceId;
        BenefitsStartDate = benefitsStartDate;
    }

    public void SetEquipment(bool vehicle, bool phone, bool laptop, bool scanner, bool businessCards, bool directDeposit, bool email, bool phoneExt)
    {
        GetsVehicle = vehicle;
        GetsPhone = phone;
        GetsLaptop = laptop;
        GetsBarcodeScanner = scanner;
        GetsBusinessCards = businessCards;
        GetsDirectDeposit = directDeposit;
        GetsCompanyEmail = email;
        GetsPhoneExtension = phoneExt;
    }

    public void SetVacationPolicy(
        int interval1, decimal amount1,
        int interval2, decimal amount2,
        int interval3, decimal amount3)
    {
        VacationInterval1 = interval1;
        VacationAmount1 = amount1;
        VacationInterval2 = interval2;
        VacationAmount2 = amount2;
        VacationInterval3 = interval3;
        VacationAmount3 = amount3;
    }

    public void UpdateVacationBalance(decimal amount)
    {
        CurrentVacationAmount = amount;
    }

    public void Terminate(DateTime terminationDate, string reason)
    {
        TerminationDate = terminationDate;
        Status = "Terminated";
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
        Status = "Active";
        TerminationDate = null;
    }

    public void SetLogin(string username)
    {
        LoginUsername = username;
    }
}
