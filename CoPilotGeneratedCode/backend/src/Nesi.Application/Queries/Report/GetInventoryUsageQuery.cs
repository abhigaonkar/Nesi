using MediatR;
using Nesi.Application.DTOs.Report;

namespace Nesi.Application.Queries.Report;

public class GetInventoryUsageQuery : IRequest<InventoryUsageSummaryDto>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Category { get; set; }
}
