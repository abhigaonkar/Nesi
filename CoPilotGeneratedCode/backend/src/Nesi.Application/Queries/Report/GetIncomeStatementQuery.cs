using MediatR;
using Nesi.Application.DTOs.Report;

namespace Nesi.Application.Queries.Report;

public class GetIncomeStatementQuery : IRequest<IncomeStatementDto>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
