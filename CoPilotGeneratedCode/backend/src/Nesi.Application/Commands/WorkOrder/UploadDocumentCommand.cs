using MediatR;
using Microsoft.AspNetCore.Http;

namespace Nesi.Application.Commands.WorkOrder;

public record UploadDocumentCommand(
    int WorkOrderId,
    IFormFile File,
    string DocumentType,
    string? Description) : IRequest<int>;
