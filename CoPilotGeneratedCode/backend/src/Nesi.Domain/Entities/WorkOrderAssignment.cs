namespace Nesi.Domain.Entities;

/// <summary>
/// Work order assignment representing technician assignments to work orders
/// </summary>
public class WorkOrderAssignment : BaseEntity
{
    public int WorkOrderId { get; private set; }
    public int TechnicianId { get; private set; }
    public DateTime AssignedAt { get; private set; }
    public int AssignedBy { get; private set; }
    public string? Role { get; private set; }
    public string? Notes { get; private set; }
    public bool IsActive { get; private set; } = true;
    
    // Navigation properties
    public virtual WorkOrder WorkOrder { get; private set; } = null!;
    public virtual User Technician { get; private set; } = null!;
    public virtual User Assigner { get; private set; } = null!;
    
    // Private constructor for EF Core
    private WorkOrderAssignment() { AssignedAt = DateTime.UtcNow; }
    
    // Public constructor
    public WorkOrderAssignment(
        int workOrderId,
        int technicianId,
        int assignedBy,
        string? role = null,
        string? notes = null)
    {
        WorkOrderId = workOrderId;
        TechnicianId = technicianId;
        AssignedBy = assignedBy;
        AssignedAt = DateTime.UtcNow;
        Role = role;
        Notes = notes;
        IsActive = true;
    }
    
    public void Unassign()
    {
        IsActive = false;
    }
    
    public void Reassign()
    {
        IsActive = true;
    }
    
    public void UpdateRole(string role)
    {
        Role = role;
    }
}
