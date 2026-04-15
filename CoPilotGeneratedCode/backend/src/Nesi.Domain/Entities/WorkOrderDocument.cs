namespace Nesi.Domain.Entities;

/// <summary>
/// Work order document representing uploaded files/photos for work orders
/// </summary>
public class WorkOrderDocument : BaseEntity
{
    public int WorkOrderId { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string FileUrl { get; private set; } = string.Empty;
    public string FileType { get; private set; } = string.Empty;
    public long FileSize { get; private set; }
    public string? Description { get; private set; }
    public string DocumentType { get; private set; } = string.Empty;
    public int UploadedBy { get; private set; }
    public DateTime UploadedAt { get; private set; }
    
    // Navigation properties
    public virtual WorkOrder WorkOrder { get; private set; } = null!;
    public virtual User Uploader { get; private set; } = null!;
    
    // Private constructor for EF Core
    private WorkOrderDocument() { UploadedAt = DateTime.UtcNow; }
    
    // Public constructor
    public WorkOrderDocument(
        int workOrderId,
        string fileName,
        string fileUrl,
        string fileType,
        long fileSize,
        string documentType,
        int uploadedBy,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be empty", nameof(fileName));
        
        if (string.IsNullOrWhiteSpace(fileUrl))
            throw new ArgumentException("File URL cannot be empty", nameof(fileUrl));
        
        if (fileSize <= 0)
            throw new ArgumentException("File size must be greater than zero", nameof(fileSize));
        
        WorkOrderId = workOrderId;
        FileName = fileName;
        FileUrl = fileUrl;
        FileType = fileType;
        FileSize = fileSize;
        DocumentType = documentType;
        UploadedBy = uploadedBy;
        UploadedAt = DateTime.UtcNow;
        Description = description;
    }
    
    public void UpdateDescription(string description)
    {
        Description = description;
    }
}
