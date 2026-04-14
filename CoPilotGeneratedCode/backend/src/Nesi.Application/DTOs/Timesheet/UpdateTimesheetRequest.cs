namespace Nesi.Application.DTOs.Timesheet;

public class UpdateTimesheetRequest
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Hours { get; set; }
    public int PayTypeId { get; set; }
    public int? WorkOrderId { get; set; }
    public int? JobTypeId { get; set; }
    public string? Notes { get; set; }
}
