using MediatR;
using Nesi.Domain.Entities;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.WorkOrder;

public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, int>
{
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;

    public UploadDocumentCommandHandler(
        IWorkOrderRepository workOrderRepository,
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService)
    {
        _workOrderRepository = workOrderRepository;
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
    }

    public async Task<int> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        // Verify work order exists
        var workOrder = await _workOrderRepository.GetByIdAsync(request.WorkOrderId, cancellationToken);
        if (workOrder == null)
            throw new InvalidOperationException($"Work order {request.WorkOrderId} not found");

        // Upload file to storage
        var fileUrl = await _fileStorageService.UploadFileAsync(
            request.File,
            $"work-orders/{request.WorkOrderId}",
            cancellationToken);

        // TODO: Get current user ID from authentication context
        int currentUserId = 1;

        // Create document entity
        var document = new WorkOrderDocument(
            request.WorkOrderId,
            request.File.FileName,
            fileUrl,
            request.File.ContentType,
            request.File.Length,
            request.DocumentType,
            currentUserId,
            request.Description);

        // Note: Document will be added through navigation property
        // In a real implementation, you might have a DocumentRepository
        // For now, we assume EF Core tracks this through the relationship
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return document.Id;
    }
}
