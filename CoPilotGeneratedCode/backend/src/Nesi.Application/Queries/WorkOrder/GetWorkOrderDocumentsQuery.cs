using MediatR;
using Nesi.Application.DTOs.WorkOrder;

namespace Nesi.Application.Queries.WorkOrder;

public record GetWorkOrderDocumentsQuery(int WorkOrderId) : IRequest<List<WorkOrderDocumentDto>>;
