using MediatR;
using Microsoft.EntityFrameworkCore;
using Nesi.Application.Common;
using Nesi.Application.DTOs.Report;

namespace Nesi.Application.Queries.Report;

public class GetArAgingReportQueryHandler : IRequestHandler<GetArAgingReportQuery, ArAgingSummaryDto>
{
    private readonly IApplicationDbContext _context;

    public GetArAgingReportQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ArAgingSummaryDto> Handle(GetArAgingReportQuery request, CancellationToken cancellationToken)
    {
        var asOfDate = request.AsOfDate ?? DateTime.UtcNow;

        // Simplified - in real system would track invoices
        // For now, use completed work orders as proxy for invoices
        var completedWorkOrders = await _context.WorkOrders
            .Include(wo => wo.Quote)
                .ThenInclude(q => q!.Customer)
            .Where(wo => wo.Status == Nesi.Domain.Enums.WorkOrderStatus.Complete && wo.CompletedAt.HasValue)
            .ToListAsync(cancellationToken);

        var customerDetails = completedWorkOrders
            .GroupBy(wo => wo.Quote!.Customer)
            .Select(g =>
            {
                var customer = g.Key!;
                var workOrders = g.ToList();
                var totalOutstanding = workOrders.Sum(wo => wo.Quote?.Total ?? 0);

                // Calculate aging buckets based on completion date
                var current = workOrders
                    .Where(wo => (asOfDate - wo.CompletedAt!.Value).Days <= 30)
                    .Sum(wo => wo.Quote?.Total ?? 0);
                
                var days31To60 = workOrders
                    .Where(wo => (asOfDate - wo.CompletedAt!.Value).Days > 30 && 
                                 (asOfDate - wo.CompletedAt!.Value).Days <= 60)
                    .Sum(wo => wo.Quote?.Total ?? 0);

                var days61To90 = workOrders
                    .Where(wo => (asOfDate - wo.CompletedAt!.Value).Days > 60 && 
                                 (asOfDate - wo.CompletedAt!.Value).Days <= 90)
                    .Sum(wo => wo.Quote?.Total ?? 0);

                var days91To120 = workOrders
                    .Where(wo => (asOfDate - wo.CompletedAt!.Value).Days > 90 && 
                                 (asOfDate - wo.CompletedAt!.Value).Days <= 120)
                    .Sum(wo => wo.Quote?.Total ?? 0);

                var over120 = workOrders
                    .Where(wo => (asOfDate - wo.CompletedAt!.Value).Days > 120)
                    .Sum(wo => wo.Quote?.Total ?? 0);

                return new ArAgingReportDto
                {
                    CustomerId = customer.Id,
                    CustomerName = customer.Name,
                    CustomerNumber = customer.CustomerNumber,
                    ContactEmail = customer.Email,
                    ContactPhone = customer.Phone,
                    Current = current,
                    Days31To60 = days31To60,
                    Days61To90 = days61To90,
                    Days91To120 = days91To120,
                    Over120Days = over120,
                    TotalOutstanding = totalOutstanding,
                    TotalInvoices = workOrders.Count,
                    OldestInvoiceDate = workOrders.Min(wo => wo.CompletedAt)
                };
            })
            .Where(c => c.TotalOutstanding > 0)
            .OrderByDescending(c => c.TotalOutstanding)
            .ToList();

        return new ArAgingSummaryDto
        {
            AsOfDate = asOfDate,
            TotalCurrent = customerDetails.Sum(c => c.Current),
            TotalDays31To60 = customerDetails.Sum(c => c.Days31To60),
            TotalDays61To90 = customerDetails.Sum(c => c.Days61To90),
            TotalDays91To120 = customerDetails.Sum(c => c.Days91To120),
            TotalOver120Days = customerDetails.Sum(c => c.Over120Days),
            GrandTotal = customerDetails.Sum(c => c.TotalOutstanding),
            TotalCustomersWithBalance = customerDetails.Count,
            TotalOutstandingInvoices = customerDetails.Sum(c => c.TotalInvoices),
            AverageDaysOutstanding = 45, // Simplified - would calculate properly
            CustomerDetails = customerDetails
        };
    }
}
