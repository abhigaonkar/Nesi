using MediatR;
using Nesi.Application.DTOs.WorkOrder;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Queries.WorkOrder;

public class GetWorkOrderDocumentsQueryHandler : IRequestHandler<GetWorkOrderDocumentsQuery, List<WorkOrderDocumentDto>>
{
    private readonly IWorkOrderRepository _workOrderRepository;

    public GetWorkOrderDocumentsQueryHandler(IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }

    public async Task<List<WorkOrderDocumentDto>> Handle(GetWorkOrderDocumentsQuery request, CancellationToken cancellationToken)
    {
        var workOrder = await _workOrderRepository.GetByIdWithDetailsAsync(request.WorkOrderId, cancellationToken);
        
        if (workOrder?.Documents == null)
            return new List<WorkOrderDocumentDto>();

        return workOrder.Documents.Select(d => new WorkOrderDocumentDto
        {
            Id = d.Id,
            WorkOrderId = d.WorkOrderId,
            FileName = d.FileName,
            FileUrl = d.FileUrl,
            FileType = d.FileType,
            FileSize = d.FileSize,
            Description = d.Description,
            DocumentType = d.DocumentType,
            UploadedBy = d.UploadedBy,
            UploaderName = "", // TODO: Load from user repository
            UploadedAt = d.UploadedAt
        }).ToList();
    }
}
