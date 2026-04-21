using MediatR;
using Nesi.Domain.Entities;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Vendor;

public class CreateVendorCommandHandler : IRequestHandler<CreateVendorCommand, int>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVendorCommandHandler(
        IVendorRepository vendorRepository,
        IUnitOfWork unitOfWork)
    {
        _vendorRepository = vendorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
    {
        // Generate vendor number
        var vendorNumber = await _vendorRepository.GenerateVendorNumberAsync(cancellationToken);

        // Create vendor entity
        var vendor = new Domain.Entities.Vendor(
            vendorNumber,
            request.CompanyName,
            request.Email,
            request.Phone);

        // Update optional contact information
        vendor.UpdateContactInfo(
            request.ContactName,
            request.Email,
            request.Phone,
            request.Fax,
            request.Website);

        // Update address if provided
        if (!string.IsNullOrWhiteSpace(request.Address))
        {
            vendor.UpdateAddress(
                request.Address,
                request.City,
                request.State,
                request.ZipCode,
                request.Country ?? "USA");
        }

        // Update business information if provided
        if (request.TaxId != null || request.AccountNumber != null || 
            request.PaymentTermsDays != null || request.CreditLimit != null)
        {
            vendor.UpdateBusinessInfo(
                request.TaxId,
                request.AccountNumber,
                request.PaymentTermsDays,
                request.CreditLimit);
        }

        // Update notes if provided
        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            vendor.UpdateNotes(request.Notes);
        }

        // Add to repository and save
        await _vendorRepository.AddAsync(vendor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return vendor.Id;
    }
}
