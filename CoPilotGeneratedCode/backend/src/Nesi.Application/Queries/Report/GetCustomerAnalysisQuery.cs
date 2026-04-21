using MediatR;
using Nesi.Application.DTOs.Report;

namespace Nesi.Application.Queries.Report;

public class GetCustomerAnalysisQuery : IRequest<CustomerAnalysisSummaryDto>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? CustomerId { get; set; }
}
