using MediatR;
using Microsoft.EntityFrameworkCore;
using Nesi.Application.Common;
using Nesi.Application.DTOs.Report;

namespace Nesi.Application.Queries.Report;

public class GetJobCostReportQueryHandler : IRequestHandler<GetJobCostReportQuery, JobCostSummaryDto>
{
    private readonly IApplicationDbContext _context;

    public GetJobCostReportQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobCostSummaryDto> Handle(GetJobCostReportQuery request, CancellationToken cancellationToken)
    {
        var query = _context.WorkOrders
            .Include(wo => wo.Quote)
                .ThenInclude(q => q!.Customer)
            .Include(wo => wo.TimesheetEntries)
            .Include(wo => wo.Materials)
            .AsQueryable();

        // Apply filters
        if (request.StartDate.HasValue)
        {
            query = query.Where(wo => wo.StartDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            query = query.Where(wo => wo.StartDate <= request.EndDate.Value);
        }

        if (request.CustomerId.HasValue)
        {
            query = query.Where(wo => wo.Quote != null && wo.Quote.CustomerId == request.CustomerId.Value);
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            if (Enum.TryParse<Nesi.Domain.Enums.WorkOrderStatus>(request.Status, out var statusEnum))
            {
                query = query.Where(wo => wo.Status == statusEnum);
            }
        }

        var workOrders = await query.ToListAsync(cancellationToken);

        var reportData = workOrders.Select(wo =>
        {
            var laborCost = wo.TimesheetEntries.Sum(t => t.Hours * 50); // Assuming $50/hr avg rate
            var materialCost = wo.Materials.Sum(m => m.Quantity * m.UnitCost);
            var totalCost = laborCost + materialCost;
            var quotedAmount = wo.Quote?.Total ?? 0;
            var profit = quotedAmount - totalCost;
            var profitMargin = quotedAmount > 0 ? (profit / quotedAmount) * 100 : 0;
            var totalHours = wo.TimesheetEntries.Sum(t => t.Hours);
            var costPerHour = totalHours > 0 ? totalCost / totalHours : 0;

            return new JobCostReportDto
            {
                WorkOrderId = wo.Id,
                WorkOrderNumber = wo.WorkOrderNumber,
                CustomerName = wo.Quote?.Customer?.Name ?? "N/A",
                Description = wo.Description,
                Status = wo.Status.ToString(),
                StartDate = wo.StartDate,
                CompletionDate = wo.CompletedAt,
                ActualLaborCost = laborCost,
                ActualMaterialCost = materialCost,
                TotalActualCost = totalCost,
                QuotedAmount = quotedAmount,
                InvoicedAmount = quotedAmount, // Simplified - same as quoted
                GrossProfit = profit,
                GrossProfitMargin = profitMargin,
                TotalHours = totalHours,
                CostPerHour = costPerHour
            };
        }).ToList();

        var summary = new JobCostSummaryDto
        {
            TotalWorkOrders = reportData.Count,
            TotalRevenue = reportData.Sum(r => r.InvoicedAmount),
            TotalCost = reportData.Sum(r => r.TotalActualCost),
            TotalProfit = reportData.Sum(r => r.GrossProfit),
            AverageProfitMargin = reportData.Any() ? reportData.Average(r => r.GrossProfitMargin) : 0,
            TotalHours = reportData.Sum(r => r.TotalHours),
            WorkOrders = reportData
        };

        return summary;
    }
}
