using MediatR;
using Nesi.Application.DTOs.Vendor;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Queries.Vendor;

public class GetVendorsQueryHandler : IRequestHandler<GetVendorsQuery, GetVendorsQueryResult>
{
    private readonly IVendorRepository _vendorRepository;

    public GetVendorsQueryHandler(IVendorRepository vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    public async Task<GetVendorsQueryResult> Handle(GetVendorsQuery request, CancellationToken cancellationToken)
    {
        var vendors = request.ActiveOnly
            ? await _vendorRepository.GetActiveVendorsAsync(cancellationToken)
            : await _vendorRepository.GetAllAsync(cancellationToken);

        var vendorList = vendors.ToList();
        
        // Apply pagination
        var paginatedVendors = vendorList
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(v => new VendorDto
            {
                Id = v.Id,
                VendorNumber = v.VendorNumber,
                CompanyName = v.CompanyName,
                ContactName = v.ContactName,
                Email = v.Email,
                Phone = v.Phone,
                Fax = v.Fax,
                Website = v.Website,
                Address = v.Address,
                City = v.City,
                State = v.State,
                ZipCode = v.ZipCode,
                Country = v.Country,
                TaxId = v.TaxId,
                AccountNumber = v.AccountNumber,
                Status = v.Status.ToString(),
                PaymentTermsDays = v.PaymentTermsDays,
                CreditLimit = v.CreditLimit,
                Rating = v.Rating,
                Notes = v.Notes,
                IsActive = v.IsActive,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt
            })
            .ToList();

        return new GetVendorsQueryResult
        {
            Vendors = paginatedVendors,
            TotalCount = vendorList.Count,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
