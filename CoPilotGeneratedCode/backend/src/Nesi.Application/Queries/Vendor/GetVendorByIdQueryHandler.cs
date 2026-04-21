using MediatR;
using Nesi.Application.DTOs.Vendor;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Queries.Vendor;

public class GetVendorByIdQueryHandler : IRequestHandler<GetVendorByIdQuery, VendorDto?>
{
    private readonly IVendorRepository _vendorRepository;

    public GetVendorByIdQueryHandler(IVendorRepository vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    public async Task<VendorDto?> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
    {
        var vendor = await _vendorRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (vendor == null)
            return null;

        return new VendorDto
        {
            Id = vendor.Id,
            VendorNumber = vendor.VendorNumber,
            CompanyName = vendor.CompanyName,
            ContactName = vendor.ContactName,
            Email = vendor.Email,
            Phone = vendor.Phone,
            Fax = vendor.Fax,
            Website = vendor.Website,
            Address = vendor.Address,
            City = vendor.City,
            State = vendor.State,
            ZipCode = vendor.ZipCode,
            Country = vendor.Country,
            TaxId = vendor.TaxId,
            AccountNumber = vendor.AccountNumber,
            Status = vendor.Status.ToString(),
            PaymentTermsDays = vendor.PaymentTermsDays,
            CreditLimit = vendor.CreditLimit,
            Rating = vendor.Rating,
            Notes = vendor.Notes,
            IsActive = vendor.IsActive,
            CreatedAt = vendor.CreatedAt,
            UpdatedAt = vendor.UpdatedAt
        };
    }
}
