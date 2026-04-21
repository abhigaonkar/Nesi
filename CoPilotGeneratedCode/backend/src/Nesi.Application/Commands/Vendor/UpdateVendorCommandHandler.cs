using MediatR;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Vendor;

public class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommand, bool>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVendorCommandHandler(
        IVendorRepository vendorRepository,
        IUnitOfWork unitOfWork)
    {
        _vendorRepository = vendorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _vendorRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (vendor == null)
            return false;

        // Update contact information
        vendor.UpdateContactInfo(
            request.ContactName,
            request.Email,
            request.Phone,
            request.Fax,
            request.Website);

        // Update address
        vendor.UpdateAddress(
            request.Address,
            request.City,
            request.State,
            request.ZipCode,
            request.Country ?? "USA");

        // Update business information
        vendor.UpdateBusinessInfo(
            request.TaxId,
            request.AccountNumber,
            request.PaymentTermsDays,
            request.CreditLimit);

        // Update notes
        vendor.UpdateNotes(request.Notes);

        await _vendorRepository.UpdateAsync(vendor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
