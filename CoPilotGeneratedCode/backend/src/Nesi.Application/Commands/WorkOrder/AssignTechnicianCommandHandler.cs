using MediatR;
using Nesi.Domain.Entities;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.WorkOrder;

public class AssignTechnicianCommandHandler : IRequestHandler<AssignTechnicianCommand, int>
{
    private readonly IRepository<WorkOrderAssignment> _assignmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignTechnicianCommandHandler(
        IRepository<WorkOrderAssignment> assignmentRepository,
        IUnitOfWork unitOfWork)
    {
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AssignTechnicianCommand request, CancellationToken cancellationToken)
    {
        var assignment = new WorkOrderAssignment(
            request.WorkOrderId,
            request.TechnicianId,
            request.AssignedBy,
            request.Role,
            request.Notes);

        await _assignmentRepository.AddAsync(assignment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return assignment.Id;
    }
}
