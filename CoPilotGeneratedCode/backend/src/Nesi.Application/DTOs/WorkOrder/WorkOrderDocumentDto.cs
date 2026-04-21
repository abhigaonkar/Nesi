namespace Nesi.Application.DTOs.WorkOrder;

public class WorkOrderDocumentDto
{
    public int Id { get; set; }
    public int WorkOrderId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? Description { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public int UploadedBy { get; set; }
    public string UploaderName { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}
