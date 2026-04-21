using MediatR;
using Nesi.Application.DTOs.Report;

namespace Nesi.Application.Queries.Report;

public class GetJobCostReportQuery : IRequest<JobCostSummaryDto>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? CustomerId { get; set; }
    public string? Status { get; set; }
}
