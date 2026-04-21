using MediatR;
using Nesi.Application.DTOs.Vendor;

namespace Nesi.Application.Queries.Vendor;

public record GetVendorByIdQuery(int Id) : IRequest<VendorDto?>;
