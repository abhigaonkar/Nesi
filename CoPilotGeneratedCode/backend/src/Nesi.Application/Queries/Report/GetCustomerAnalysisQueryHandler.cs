using MediatR;
using Microsoft.EntityFrameworkCore;
using Nesi.Application.DTOs.Report;
using Nesi.Infrastructure.Data;

namespace Nesi.Application.Queries.Report;

public class GetCustomerAnalysisQueryHandler : IRequestHandler<GetCustomerAnalysisQuery, CustomerAnalysisSummaryDto>
{
    private readonly NesiDbContext _context;

    public GetCustomerAnalysisQueryHandler(NesiDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerAnalysisSummaryDto> Handle(GetCustomerAnalysisQuery request, CancellationToken cancellationToken)
    {
        var startDate = request.StartDate ?? DateTime.UtcNow.AddYears(-1);
        var endDate = request.EndDate ?? DateTime.UtcNow;

        var customers = await _context.Customers
            .Include(c => c.Quotes)
                .ThenInclude(q => q.WorkOrder)
                    .ThenInclude(wo => wo!.TimesheetEntries)
            .Include(c => c.Quotes)
                .ThenInclude(q => q.WorkOrder)
                    .ThenInclude(wo => wo!.Materials)
            .Where(c => request.CustomerId == null || c.Id == request.CustomerId.Value)
            .ToListAsync(cancellationToken);

        var customerAnalysisList = customers.Select(customer =>
        {
            var quotes = customer.Quotes
                .Where(q => q.CreatedAt >= startDate && q.CreatedAt <= endDate)
                .ToList();

            var workOrders = quotes
                .Where(q => q.WorkOrder != null)
                .Select(q => q.WorkOrder!)
                .ToList();

            var totalRevenue = workOrders.Sum(wo => wo.Quote?.TotalAmount ?? 0);
            var laborCost = workOrders.SelectMany(wo => wo.TimesheetEntries).Sum(t => t.Hours * 50);
            var materialCost = workOrders.SelectMany(wo => wo.Materials).Sum(m => m.Quantity * m.UnitCost);
            var totalCost = laborCost + materialCost;
            var profit = totalRevenue - totalCost;
            var profitMargin = totalRevenue > 0 ? (profit / totalRevenue) * 100 : 0;

            return new CustomerAnalysisDto
            {
                CustomerId = customer.Id,
                CustomerName = customer.Name,
                CustomerNumber = customer.CustomerNumber,
                TotalQuotes = quotes.Count,
                TotalWorkOrders = workOrders.Count,
                QuoteWinRate = quotes.Count > 0 ? ((decimal)workOrders.Count / quotes.Count) * 100 : 0,
                TotalRevenue = totalRevenue,
                AverageWorkOrderValue = workOrders.Count > 0 ? totalRevenue / workOrders.Count : 0,
                TotalProfit = profit,
                ProfitMargin = profitMargin,
                AverageLaborRate = 50, // Simplified
                AverageMaterialMarkup = 20, // Simplified
                FirstQuoteDate = quotes.Min(q => (DateTime?)q.CreatedAt),
                LastWorkOrderDate = workOrders.Max(wo => (DateTime?)wo.StartDate),
                DaysSinceLastOrder = workOrders.Any() 
                    ? (int)(DateTime.UtcNow - workOrders.Max(wo => wo.StartDate)).TotalDays 
                    : 0,
                OutstandingBalance = 0, // Simplified
                AverageDaysToPayment = 30 // Simplified
            };
        }).ToList();

        var totalRevenue = customerAnalysisList.Sum(c => c.TotalRevenue);

        return new CustomerAnalysisSummaryDto
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalActiveCustomers = customerAnalysisList.Count(c => c.TotalWorkOrders > 0),
            TotalRevenue = totalRevenue,
            AverageRevenuePerCustomer = customerAnalysisList.Count > 0 
                ? totalRevenue / customerAnalysisList.Count 
                : 0,
            TopCustomersByRevenue = customerAnalysisList
                .OrderByDescending(c => c.TotalRevenue)
                .Take(10)
                .ToList(),
            TopCustomersByProfitMargin = customerAnalysisList
                .OrderByDescending(c => c.ProfitMargin)
                .Take(10)
                .ToList(),
            AllCustomers = customerAnalysisList
        };
    }
}
