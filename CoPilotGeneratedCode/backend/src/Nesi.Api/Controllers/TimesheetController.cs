using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nesi.Application.Commands.Timesheet;
using Nesi.Application.Common;
using Nesi.Application.DTOs.Timesheet;
using Nesi.Application.Queries.Timesheet;

namespace Nesi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimesheetController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TimesheetController> _logger;

    public TimesheetController(IMediator mediator, ILogger<TimesheetController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all timesheets with optional filtering and pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<TimesheetDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<TimesheetDto>>>> GetTimesheets(
        [FromQuery] int? userId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] string? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = new GetTimesheetsQuery(userId, startDate, endDate, status, pageNumber, pageSize);
            var result = await _mediator.Send(query);
            
            return Ok(ApiResponse<PagedResult<TimesheetDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving timesheets");
            return StatusCode(500, ApiResponse<PagedResult<TimesheetDto>>.ErrorResponse("An error occurred retrieving timesheets"));
        }
    }

    /// <summary>
    /// Get a specific timesheet by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<TimesheetDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TimesheetDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TimesheetDto>>> GetTimesheetById(int id)
    {
        try
        {
            var query = new GetTimesheetByIdQuery(id);
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound(ApiResponse<TimesheetDto>.ErrorResponse("Timesheet not found"));
            }
            
            return Ok(ApiResponse<TimesheetDto>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving timesheet {TimesheetId}", id);
            return StatusCode(500, ApiResponse<TimesheetDto>.ErrorResponse("An error occurred retrieving the timesheet"));
        }
    }

    /// <summary>
    /// Create a new timesheet entry
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<int>>> CreateTimesheet([FromBody] CreateTimesheetRequest request)
    {
        try
        {
            // TODO: Get userId from authenticated user claims
            // For now, using a default userId from the request or header
            var userId = int.Parse(Request.Headers["X-User-Id"].FirstOrDefault() ?? "1");
            
            var command = new CreateTimesheetCommand(
                userId,
                request.Date,
                request.Hours,
                request.PayTypeId,
                request.WorkOrderId,
                request.JobTypeId,
                request.Notes);
            
            var timesheetId = await _mediator.Send(command);
            
            return CreatedAtAction(
                nameof(GetTimesheetById),
                new { id = timesheetId },
                ApiResponse<int>.SuccessResponse(timesheetId, "Timesheet created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to create timesheet");
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating timesheet");
            return StatusCode(500, ApiResponse<int>.ErrorResponse("An error occurred creating the timesheet"));
        }
    }

    /// <summary>
    /// Update an existing timesheet entry
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateTimesheet(int id, [FromBody] UpdateTimesheetRequest request)
    {
        try
        {
            if (id != request.Id)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Timesheet ID mismatch"));
            }
            
            // TODO: Get userId from authenticated user claims
            var userId = int.Parse(Request.Headers["X-User-Id"].FirstOrDefault() ?? "1");
            
            var command = new UpdateTimesheetCommand(
                request.Id,
                userId,
                request.Date,
                request.Hours,
                request.PayTypeId,
                request.WorkOrderId,
                request.JobTypeId,
                request.Notes);
            
            var result = await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Timesheet updated successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to update timesheet {TimesheetId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized attempt to update timesheet {TimesheetId}", id);
            return Unauthorized(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating timesheet {TimesheetId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred updating the timesheet"));
        }
    }

    /// <summary>
    /// Submit a timesheet for approval
    /// </summary>
    [HttpPost("{id}/submit")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> SubmitTimesheet(int id)
    {
        try
        {
            // TODO: Get userId from authenticated user claims
            var userId = int.Parse(Request.Headers["X-User-Id"].FirstOrDefault() ?? "1");
            
            var command = new SubmitTimesheetCommand(id, userId);
            var result = await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Timesheet submitted successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to submit timesheet {TimesheetId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized attempt to submit timesheet {TimesheetId}", id);
            return Unauthorized(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting timesheet {TimesheetId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred submitting the timesheet"));
        }
    }

    /// <summary>
    /// Approve a timesheet (Manager/Admin only)
    /// </summary>
    [HttpPost("{id}/approve")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> ApproveTimesheet(int id)
    {
        try
        {
            var command = new ApproveTimesheetCommand(id);
            var result = await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Timesheet approved successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to approve timesheet {TimesheetId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving timesheet {TimesheetId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred approving the timesheet"));
        }
    }

    /// <summary>
    /// Reject a timesheet (Manager/Admin only)
    /// </summary>
    [HttpPost("{id}/reject")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> RejectTimesheet(int id)
    {
        try
        {
            var command = new RejectTimesheetCommand(id);
            var result = await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Timesheet rejected successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to reject timesheet {TimesheetId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting timesheet {TimesheetId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred rejecting the timesheet"));
        }
    }
}
