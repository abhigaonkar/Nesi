using Nesi.Domain.Enums;

namespace Nesi.Application.DTOs.Timesheet;

public class TimesheetDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Hours { get; set; }
    public int PayTypeId { get; set; }
    public string PayTypeName { get; set; } = string.Empty;
    public int? WorkOrderId { get; set; }
    public string? WorkOrderNumber { get; set; }
    public string? WorkOrderDescription { get; set; }
    public int? JobTypeId { get; set; }
    public string? JobTypeName { get; set; }
    public string? Notes { get; set; }
    public TimesheetStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
