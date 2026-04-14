using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nesi.Application.Common;
using Nesi.Application.DTOs.WorkOrder;
using Nesi.Application.Queries.WorkOrder;

namespace Nesi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkOrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<WorkOrderController> _logger;

    public WorkOrderController(IMediator mediator, ILogger<WorkOrderController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all active work orders
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<WorkOrderDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<WorkOrderDto>>>> GetWorkOrders()
    {
        try
        {
            var query = new GetWorkOrdersQuery();
            var result = await _mediator.Send(query);
            
            return Ok(ApiResponse<List<WorkOrderDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving work orders");
            return StatusCode(500, ApiResponse<List<WorkOrderDto>>.ErrorResponse("An error occurred retrieving work orders"));
        }
    }
}
