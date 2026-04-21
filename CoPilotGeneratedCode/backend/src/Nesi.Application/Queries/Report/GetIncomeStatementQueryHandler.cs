using MediatR;
using Microsoft.EntityFrameworkCore;
using Nesi.Application.DTOs.Report;
using Nesi.Infrastructure.Data;

namespace Nesi.Application.Queries.Report;

public class GetIncomeStatementQueryHandler : IRequestHandler<GetIncomeStatementQuery, IncomeStatementDto>
{
    private readonly NesiDbContext _context;

    public GetIncomeStatementQueryHandler(NesiDbContext context)
    {
        _context = context;
    }

    public async Task<IncomeStatementDto> Handle(GetIncomeStatementQuery request, CancellationToken cancellationToken)
    {
        // Get completed work orders in the date range
        var workOrders = await _context.WorkOrders
            .Include(wo => wo.Quote)
            .Include(wo => wo.TimesheetEntries)
            .Include(wo => wo.Materials)
            .Where(wo => wo.StartDate >= request.StartDate && wo.StartDate <= request.EndDate)
            .ToListAsync(cancellationToken);

        var totalRevenue = workOrders.Sum(wo => wo.Quote?.TotalAmount ?? 0);
        var laborCost = workOrders.SelectMany(wo => wo.TimesheetEntries).Sum(t => t.Hours * 50); // $50/hr avg
        var materialCost = workOrders.SelectMany(wo => wo.Materials).Sum(m => m.Quantity * m.UnitCost);
        var totalCOGS = laborCost + materialCost;
        var grossProfit = totalRevenue - totalCOGS;
        var grossProfitMargin = totalRevenue > 0 ? (grossProfit / totalRevenue) * 100 : 0;

        // Simplified operating expenses - would come from accounting system
        var operatingExpenses = totalRevenue * 0.15m; // Assume 15% of revenue
        var netIncome = grossProfit - operatingExpenses;
        var netProfitMargin = totalRevenue > 0 ? (netIncome / totalRevenue) * 100 : 0;

        var quotes = await _context.Quotes
            .Where(q => q.CreatedAt >= request.StartDate && q.CreatedAt <= request.EndDate)
            .CountAsync(cancellationToken);

        return new IncomeStatementDto
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalRevenue = totalRevenue,
            ServiceRevenue = totalRevenue - materialCost, // Simplified
            MaterialRevenue = materialCost,
            TotalCOGS = totalCOGS,
            LaborCost = laborCost,
            MaterialCost = materialCost,
            GrossProfit = grossProfit,
            GrossProfitMargin = grossProfitMargin,
            OperatingExpenses = operatingExpenses,
            AdminExpenses = operatingExpenses * 0.6m,
            SellingExpenses = operatingExpenses * 0.4m,
            NetIncome = netIncome,
            NetProfitMargin = netProfitMargin,
            TotalWorkOrdersCompleted = workOrders.Count,
            TotalQuotesGenerated = quotes,
            AverageWorkOrderValue = workOrders.Count > 0 ? totalRevenue / workOrders.Count : 0
        };
    }
}
