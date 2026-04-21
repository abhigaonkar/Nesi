using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nesi.Application.Commands.PurchaseOrder;
using Nesi.Application.Common;
using Nesi.Application.DTOs.PurchaseOrder;
using Nesi.Application.Queries.PurchaseOrder;

namespace Nesi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PurchaseOrderController> _logger;

    public PurchaseOrderController(IMediator mediator, ILogger<PurchaseOrderController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all purchase orders with optional filtering and pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetPurchaseOrdersQueryResult>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<GetPurchaseOrdersQueryResult>>> GetPurchaseOrders(
        [FromQuery] int? vendorId = null,
        [FromQuery] int? workOrderId = null,
        [FromQuery] string? status = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var query = new GetPurchaseOrdersQuery(vendorId, workOrderId, status, pageNumber, pageSize);
            var result = await _mediator.Send(query);
            
            return Ok(ApiResponse<GetPurchaseOrdersQueryResult>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving purchase orders");
            return StatusCode(500, ApiResponse<GetPurchaseOrdersQueryResult>.ErrorResponse("An error occurred retrieving purchase orders"));
        }
    }

    /// <summary>
    /// Get a specific purchase order by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PurchaseOrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PurchaseOrderDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PurchaseOrderDto>>> GetPurchaseOrderById(
        int id,
        [FromQuery] bool includeLineItems = true,
        [FromQuery] bool includeReceipts = false)
    {
        try
        {
            var query = new GetPurchaseOrderByIdQuery(id, includeLineItems, includeReceipts);
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound(ApiResponse<PurchaseOrderDto>.ErrorResponse("Purchase order not found"));
            }
            
            return Ok(ApiResponse<PurchaseOrderDto>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving purchase order {PurchaseOrderId}", id);
            return StatusCode(500, ApiResponse<PurchaseOrderDto>.ErrorResponse("An error occurred retrieving the purchase order"));
        }
    }

    /// <summary>
    /// Create a new purchase order
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<int>>> CreatePurchaseOrder([FromBody] CreatePurchaseOrderCommand command)
    {
        try
        {
            var purchaseOrderId = await _mediator.Send(command);
            return CreatedAtAction(
                nameof(GetPurchaseOrderById),
                new { id = purchaseOrderId },
                ApiResponse<int>.SuccessResponse(purchaseOrderId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating purchase order");
            return StatusCode(500, ApiResponse<int>.ErrorResponse("An error occurred creating the purchase order"));
        }
    }

    /// <summary>
    /// Submit a purchase order for approval
    /// </summary>
    [HttpPost("{id}/submit")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> SubmitPurchaseOrder(int id)
    {
        try
        {
            var command = new SubmitPurchaseOrderCommand(id);
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse("Purchase order not found"));
            }
            
            return Ok(ApiResponse<bool>.SuccessResponse(result));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot submit purchase order {PurchaseOrderId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting purchase order {PurchaseOrderId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred submitting the purchase order"));
        }
    }

    /// <summary>
    /// Approve a purchase order
    /// </summary>
    [HttpPost("{id}/approve")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> ApprovePurchaseOrder(int id)
    {
        try
        {
            // TODO: Get current user ID from authentication context
            int currentUserId = 1;
            
            var command = new ApprovePurchaseOrderCommand(id, currentUserId);
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse("Purchase order not found"));
            }
            
            return Ok(ApiResponse<bool>.SuccessResponse(result));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot approve purchase order {PurchaseOrderId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving purchase order {PurchaseOrderId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred approving the purchase order"));
        }
    }

    /// <summary>
    /// Reject a purchase order
    /// </summary>
    [HttpPost("{id}/reject")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> RejectPurchaseOrder(int id, [FromBody] RejectPurchaseOrderRequest request)
    {
        try
        {
            // TODO: Get current user ID from authentication context
            int currentUserId = 1;
            
            var command = new RejectPurchaseOrderCommand(id, currentUserId, request.Reason);
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse("Purchase order not found"));
            }
            
            return Ok(ApiResponse<bool>.SuccessResponse(result));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot reject purchase order {PurchaseOrderId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting purchase order {PurchaseOrderId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred rejecting the purchase order"));
        }
    }
}

public class RejectPurchaseOrderRequest
{
    public string Reason { get; set; } = string.Empty;
}
