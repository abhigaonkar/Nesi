using MediatR;
using Microsoft.EntityFrameworkCore;
using Nesi.Application.Common;
using Nesi.Application.DTOs.Report;

namespace Nesi.Application.Queries.Report;

public class GetInventoryUsageQueryHandler : IRequestHandler<GetInventoryUsageQuery, InventoryUsageSummaryDto>
{
    private readonly IApplicationDbContext _context;

    public GetInventoryUsageQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryUsageSummaryDto> Handle(GetInventoryUsageQuery request, CancellationToken cancellationToken)
    {
        var startDate = request.StartDate ?? DateTime.UtcNow.AddMonths(-1);
        var endDate = request.EndDate ?? DateTime.UtcNow;

        // Get all work order materials in the date range
        var materials = await _context.WorkOrders
            .Include(wo => wo.Materials)
            .Where(wo => wo.StartDate >= startDate && wo.StartDate <= endDate)
            .SelectMany(wo => wo.Materials.Select(m => new
            {
                MaterialName = m.Description,
                PartNumber = "PART-" + m.Id, // Simplified
                Category = "General", // Simplified
                UnitOfMeasure = "EA",
                Quantity = m.Quantity,
                TotalCost = m.Quantity * m.UnitCost,
                UnitCost = m.UnitCost,
                WorkOrderNumber = wo.WorkOrderNumber,
                UsedDate = wo.StartDate
            }))
            .ToListAsync(cancellationToken);

        var materialUsage = materials
            .GroupBy(m => m.MaterialName)
            .Select(g => new InventoryUsageDto
            {
                MaterialId = g.GetHashCode(), // Simplified
                MaterialName = g.Key,
                PartNumber = g.First().PartNumber,
                Category = g.First().Category,
                UnitOfMeasure = g.First().UnitOfMeasure,
                QuantityUsed = g.Sum(m => m.Quantity),
                TotalCost = g.Sum(m => m.TotalCost),
                AverageCostPerUnit = g.Average(m => m.UnitCost),
                TimesUsed = g.Count(),
                WorkOrderNumbers = g.Select(m => m.WorkOrderNumber).Distinct().ToList(),
                FirstUsedDate = g.Min(m => m.UsedDate),
                LastUsedDate = g.Max(m => m.UsedDate)
            })
            .ToList();

        var totalCost = materialUsage.Sum(m => m.TotalCost);
        var workOrderCount = materials.Select(m => m.WorkOrderNumber).Distinct().Count();

        return new InventoryUsageSummaryDto
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalMaterialsUsed = materialUsage.Count,
            TotalMaterialCost = totalCost,
            AverageCostPerWorkOrder = workOrderCount > 0 ? totalCost / workOrderCount : 0,
            TopMaterialsByQuantity = materialUsage
                .OrderByDescending(m => m.QuantityUsed)
                .Take(10)
                .ToList(),
            TopMaterialsByCost = materialUsage
                .OrderByDescending(m => m.TotalCost)
                .Take(10)
                .ToList(),
            AllMaterials = materialUsage
        };
    }
}
