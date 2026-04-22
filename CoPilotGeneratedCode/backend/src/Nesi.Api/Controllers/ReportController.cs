using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nesi.Application.Queries.Report;
using Nesi.Application.DTOs.Report;
using Nesi.Application.Services;

namespace Nesi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IExportService _exportService;

    public ReportController(IMediator mediator, IExportService exportService)
    {
        _mediator = mediator;
        _exportService = exportService;
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

    [HttpGet("job-cost/export/pdf")]
    public async Task<IActionResult> ExportJobCostReportToPdf(
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
        
        var columns = new Dictionary<string, string>
        {
            { "JobNumber", "Job #" },
            { "CustomerName", "Customer" },
            { "Description", "Description" },
            { "EstimatedCost", "Est. Cost" },
            { "ActualCost", "Actual Cost" },
            { "Variance", "Variance" },
            { "Status", "Status" }
        };

        var pdfBytes = _exportService.ExportToPdf(result.JobCosts, "Job Cost Report", columns);
        return File(pdfBytes, "application/pdf", $"JobCostReport_{DateTime.Now:yyyyMMdd}.pdf");
    }

    [HttpGet("job-cost/export/excel")]
    public async Task<IActionResult> ExportJobCostReportToExcel(
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
        
        var columns = new Dictionary<string, string>
        {
            { "JobNumber", "Job #" },
            { "CustomerName", "Customer" },
            { "Description", "Description" },
            { "EstimatedCost", "Est. Cost" },
            { "ActualCost", "Actual Cost" },
            { "Variance", "Variance" },
            { "Status", "Status" }
        };

        var excelBytes = _exportService.ExportToExcel(result.JobCosts, "Job Cost Report", columns);
        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            $"JobCostReport_{DateTime.Now:yyyyMMdd}.xlsx");
    }

    [HttpGet("ar-aging/export/pdf")]
    public async Task<IActionResult> ExportArAgingReportToPdf([FromQuery] DateTime? asOfDate)
    {
        var query = new GetArAgingReportQuery { AsOfDate = asOfDate };
        var result = await _mediator.Send(query);
        
        var columns = new Dictionary<string, string>
        {
            { "CustomerName", "Customer" },
            { "Current", "Current" },
            { "Days30", "1-30 Days" },
            { "Days60", "31-60 Days" },
            { "Days90", "61-90 Days" },
            { "Over90", "Over 90 Days" },
            { "TotalDue", "Total Due" }
        };

        var pdfBytes = _exportService.ExportToPdf(result.Customers, "AR Aging Report", columns);
        return File(pdfBytes, "application/pdf", $"ARAgingReport_{DateTime.Now:yyyyMMdd}.pdf");
    }

    [HttpGet("ar-aging/export/excel")]
    public async Task<IActionResult> ExportArAgingReportToExcel([FromQuery] DateTime? asOfDate)
    {
        var query = new GetArAgingReportQuery { AsOfDate = asOfDate };
        var result = await _mediator.Send(query);
        
        var columns = new Dictionary<string, string>
        {
            { "CustomerName", "Customer" },
            { "Current", "Current" },
            { "Days30", "1-30 Days" },
            { "Days60", "31-60 Days" },
            { "Days90", "61-90 Days" },
            { "Over90", "Over 90 Days" },
            { "TotalDue", "Total Due" }
        };

        var excelBytes = _exportService.ExportToExcel(result.Customers, "AR Aging Report", columns);
        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            $"ARAgingReport_{DateTime.Now:yyyyMMdd}.xlsx");
    }

    [HttpGet("customer-analysis/export/pdf")]
    public async Task<IActionResult> ExportCustomerAnalysisToPdf(
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
        
        var columns = new Dictionary<string, string>
        {
            { "CustomerName", "Customer" },
            { "TotalRevenue", "Revenue" },
            { "TotalCost", "Cost" },
            { "GrossProfit", "Gross Profit" },
            { "ProfitMargin", "Profit Margin %" },
            { "JobCount", "Job Count" }
        };

        var pdfBytes = _exportService.ExportToPdf(result.Customers, "Customer Analysis Report", columns);
        return File(pdfBytes, "application/pdf", $"CustomerAnalysis_{DateTime.Now:yyyyMMdd}.pdf");
    }

    [HttpGet("customer-analysis/export/excel")]
    public async Task<IActionResult> ExportCustomerAnalysisToExcel(
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
        
        var columns = new Dictionary<string, string>
        {
            { "CustomerName", "Customer" },
            { "TotalRevenue", "Revenue" },
            { "TotalCost", "Cost" },
            { "GrossProfit", "Gross Profit" },
            { "ProfitMargin", "Profit Margin %" },
            { "JobCount", "Job Count" }
        };

        var excelBytes = _exportService.ExportToExcel(result.Customers, "Customer Analysis Report", columns);
        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            $"CustomerAnalysis_{DateTime.Now:yyyyMMdd}.xlsx");
    }
}
