namespace Nesi.Domain.Enums;

public enum TimesheetStatus
{
    Draft = 0,
    Submitted = 1,
    Approved = 2,
    Rejected = 3
}

public enum UserRole
{
    Employee = 2,
    Manager = 1,
    Admin = 0
}

public enum QuoteStatus
{
    Draft = 0,
    Submitted = 1,
    Approved = 2,
    Rejected = 3,
    CustomerApproved = 4
}

public enum QuoteType
{
    TimeAndMaterial = 0,
    FixedPrice = 1,
    CostPlus = 2
}

public enum QuoteLineItemType
{
    Labor = 0,
    Material = 1,
    Equipment = 2,
    Miscellaneous = 3
}

public enum WorkOrderStatus
{
    Created = 0,
    Assigned = 1,
    InProgress = 2,
    Complete = 3,
    Cancelled = 4,
    Closed = 5
}

public enum AddressType
{
    Shipping = 0,
    Billing = 1,
    Both = 2
}
