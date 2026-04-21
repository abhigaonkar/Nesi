using MediatR;
using Nesi.Application.DTOs.Report;

namespace Nesi.Application.Queries.Report;

public class GetArAgingReportQuery : IRequest<ArAgingSummaryDto>
{
    public DateTime? AsOfDate { get; set; }
}
