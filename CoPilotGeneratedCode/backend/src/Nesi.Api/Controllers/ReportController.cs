using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nesi.Application.Queries.Report;
using Nesi.Application.DTOs.Report;

namespace Nesi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("job-cost")]
    public async Task<ActionResult<JobCostSummaryDto>> GetJobCostReport(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? customerId,
        [FromQuery] string? status)
    {
        var query = new GetJobCostReportQuery
        {
            StartDate = startDate,
            EndDate = endDate,
            CustomerId = customerId,
            Status = status
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("income-statement")]
    public async Task<ActionResult<IncomeStatementDto>> GetIncomeStatement(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
        var end = endDate ?? DateTime.UtcNow;

        var query = new GetIncomeStatementQuery
        {
            StartDate = start,
            EndDate = end
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("ar-aging")]
    public async Task<ActionResult<ArAgingSummaryDto>> GetArAgingReport(
        [FromQuery] DateTime? asOfDate)
    {
        var query = new GetArAgingReportQuery
        {
            AsOfDate = asOfDate
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("customer-analysis")]
    public async Task<ActionResult<CustomerAnalysisSummaryDto>> GetCustomerAnalysis(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? customerId)
    {
        var query = new GetCustomerAnalysisQuery
        {
            StartDate = startDate,
            EndDate = endDate,
            CustomerId = customerId
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("inventory-usage")]
    public async Task<ActionResult<InventoryUsageSummaryDto>> GetInventoryUsage(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] string? category)
    {
        var query = new GetInventoryUsageQuery
        {
            StartDate = startDate,
            EndDate = endDate,
            Category = category
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
