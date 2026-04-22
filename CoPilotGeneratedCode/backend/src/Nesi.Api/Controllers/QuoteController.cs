using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nesi.Application.Commands.Quote;
using Nesi.Application.Common;
using Nesi.Application.DTOs.Quote;
using Nesi.Application.Queries.Quote;
using Nesi.Application.Services;

namespace Nesi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuoteController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<QuoteController> _logger;
    private readonly IExportService _exportService;

    public QuoteController(IMediator mediator, ILogger<QuoteController> logger, IExportService exportService)
    {
        _mediator = mediator;
        _logger = logger;
        _exportService = exportService;
    }

    /// <summary>
    /// Get all quotes
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<QuoteDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<QuoteDto>>>> GetQuotes()
    {
        try
        {
            var query = new GetQuotesQuery();
            var result = await _mediator.Send(query);
            
            return Ok(ApiResponse<IEnumerable<QuoteDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving quotes");
            return StatusCode(500, ApiResponse<IEnumerable<QuoteDto>>.ErrorResponse("An error occurred retrieving quotes"));
        }
    }

    /// <summary>
    /// Get a specific quote by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<QuoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<QuoteDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<QuoteDto>>> GetQuoteById(int id)
    {
        try
        {
            var query = new GetQuoteByIdQuery(id);
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound(ApiResponse<QuoteDto>.ErrorResponse("Quote not found"));
            }
            
            return Ok(ApiResponse<QuoteDto>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving quote {QuoteId}", id);
            return StatusCode(500, ApiResponse<QuoteDto>.ErrorResponse("An error occurred retrieving the quote"));
        }
    }

    /// <summary>
    /// Create a new quote
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<int>>> CreateQuote([FromBody] CreateQuoteCommand command)
    {
        try
        {
            var quoteId = await _mediator.Send(command);
            
            return CreatedAtAction(
                nameof(GetQuoteById),
                new { id = quoteId },
                ApiResponse<int>.SuccessResponse(quoteId, "Quote created successfully"));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Failed to create quote - validation error");
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to create quote");
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating quote");
            return StatusCode(500, ApiResponse<int>.ErrorResponse("An error occurred creating the quote"));
        }
    }

    /// <summary>
    /// Submit a quote for approval
    /// </summary>
    [HttpPost("{id}/submit")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> SubmitQuote(int id)
    {
        try
        {
            var command = new SubmitQuoteCommand(id);
            await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Quote submitted successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to submit quote {QuoteId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting quote {QuoteId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred submitting the quote"));
        }
    }

    /// <summary>
    /// Approve a quote (Manager only)
    /// </summary>
    [HttpPost("{id}/approve")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> ApproveQuote(int id)
    {
        try
        {
            // TODO: Get userId from authenticated user claims
            var userId = int.Parse(Request.Headers["X-User-Id"].FirstOrDefault() ?? "1");
            
            var command = new ApproveQuoteCommand(id, userId);
            await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Quote approved successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to approve quote {QuoteId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving quote {QuoteId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred approving the quote"));
        }
    }

    /// <summary>
    /// Reject a quote (Manager only)
    /// </summary>
    [HttpPost("{id}/reject")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> RejectQuote(int id, [FromBody] RejectQuoteRequest request)
    {
        try
        {
            // TODO: Get userId from authenticated user claims
            var userId = int.Parse(Request.Headers["X-User-Id"].FirstOrDefault() ?? "1");
            
            var command = new RejectQuoteCommand(id, userId, request.Reason);
            await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Quote rejected successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to reject quote {QuoteId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting quote {QuoteId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred rejecting the quote"));
        }
    }

    /// <summary>
    /// Customer approve a quote
    /// </summary>
    [HttpPost("{id}/customer-approve")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> CustomerApproveQuote(int id)
    {
        try
        {
            // TODO: Get customer contact from authenticated user claims
            var approvedBy = Request.Headers["X-User-Name"].FirstOrDefault() ?? "Customer";
            
            var command = new CustomerApproveQuoteCommand(id, approvedBy);
            await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Quote approved by customer successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to customer approve quote {QuoteId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error customer approving quote {QuoteId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred approving the quote"));
        }
    }

    /// <summary>
    /// Convert a quote to a work order
    /// </summary>
    [HttpPost("{id}/convert-to-workorder")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<int>>> ConvertToWorkOrder(int id)
    {
        try
        {
            var command = new ConvertQuoteToWorkOrderCommand(id);
            var workOrderId = await _mediator.Send(command);
            
            return Ok(ApiResponse<int>.SuccessResponse(workOrderId, "Quote converted to work order successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to convert quote {QuoteId} to work order", id);
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error converting quote {QuoteId} to work order", id);
            return StatusCode(500, ApiResponse<int>.ErrorResponse("An error occurred converting the quote to a work order"));
        }
    }

    /// <summary>
    /// Export quotes to PDF
    /// </summary>
    [HttpGet("export/pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportQuotesToPdf()
    {
        try
        {
            var query = new GetQuotesQuery();
            var result = await _mediator.Send(query);
            
            var columns = new Dictionary<string, string>
            {
                { "QuoteNumber", "Quote #" },
                { "CustomerName", "Customer" },
                { "Description", "Description" },
                { "Status", "Status" },
                { "TotalAmount", "Total" },
                { "ValidUntil", "Valid Until" },
                { "CreatedDate", "Created" }
            };

            var pdfBytes = _exportService.ExportToPdf(result, "Quotes List", columns);
            return File(pdfBytes, "application/pdf", $"Quotes_{DateTime.Now:yyyyMMdd}.pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting quotes to PDF");
            return StatusCode(500, "An error occurred exporting quotes");
        }
    }

    /// <summary>
    /// Export quotes to Excel
    /// </summary>
    [HttpGet("export/excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportQuotesToExcel()
    {
        try
        {
            var query = new GetQuotesQuery();
            var result = await _mediator.Send(query);
            
            var columns = new Dictionary<string, string>
            {
                { "QuoteNumber", "Quote #" },
                { "CustomerName", "Customer" },
                { "Description", "Description" },
                { "Status", "Status" },
                { "TotalAmount", "Total" },
                { "ValidUntil", "Valid Until" },
                { "CreatedDate", "Created" },
                { "ApprovedDate", "Approved" }
            };

            var excelBytes = _exportService.ExportToExcel(result, "Quotes", columns);
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"Quotes_{DateTime.Now:yyyyMMdd}.xlsx");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting quotes to Excel");
            return StatusCode(500, "An error occurred exporting quotes");
        }
    }
}

public record RejectQuoteRequest(string Reason);
