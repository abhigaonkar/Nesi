using MediatR;
using Nesi.Domain.Entities;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.WorkOrder;

public class AddMaterialCommandHandler : IRequestHandler<AddMaterialCommand, int>
{
    private readonly IRepository<Material> _materialRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddMaterialCommandHandler(
        IRepository<Material> materialRepository,
        IUnitOfWork unitOfWork)
    {
        _materialRepository = materialRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AddMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = new Material(
            request.WorkOrderId,
            request.PartNumber,
            request.Description,
            request.Quantity,
            request.UnitCost,
            request.PurchaseOrderNumber,
            request.Supplier,
            request.Notes);

        await _materialRepository.AddAsync(material, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return material.Id;
    }
}
