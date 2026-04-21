using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nesi.Application.Commands.PurchaseOrder;
using Nesi.Application.Common;
using Nesi.Application.DTOs.PurchaseOrder;
using Nesi.Application.Queries.PurchaseOrder;
using Nesi.Application.Services;

namespace Nesi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PurchaseOrderController> _logger;
    private readonly IThreeWayMatchingService _threeWayMatchingService;

    public PurchaseOrderController(
        IMediator mediator, 
        ILogger<PurchaseOrderController> logger,
        IThreeWayMatchingService threeWayMatchingService)
    {
        _mediator = mediator;
        _logger = logger;
        _threeWayMatchingService = threeWayMatchingService;
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

    /// <summary>
    /// Create a receipt for a purchase order
    /// </summary>
    [HttpPost("{id}/receipts")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<int>>> CreateReceipt(int id, [FromBody] CreateReceiptCommand command)
    {
        try
        {
            if (id != command.PurchaseOrderId)
            {
                return BadRequest(ApiResponse<int>.ErrorResponse("Purchase order ID mismatch"));
            }

            var receiptId = await _mediator.Send(command);
            return CreatedAtAction(
                nameof(GetReceiptsByPurchaseOrder),
                new { id = command.PurchaseOrderId },
                ApiResponse<int>.SuccessResponse(receiptId));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot create receipt for purchase order {PurchaseOrderId}", id);
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating receipt for purchase order {PurchaseOrderId}", id);
            return StatusCode(500, ApiResponse<int>.ErrorResponse("An error occurred creating the receipt"));
        }
    }

    /// <summary>
    /// Get all receipts for a purchase order
    /// </summary>
    [HttpGet("{id}/receipts")]
    [ProducesResponseType(typeof(ApiResponse<List<PurchaseOrderReceiptDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<PurchaseOrderReceiptDto>>>> GetReceiptsByPurchaseOrder(int id)
    {
        try
        {
            var query = new GetReceiptsByPurchaseOrderQuery(id);
            var result = await _mediator.Send(query);
            
            return Ok(ApiResponse<List<PurchaseOrderReceiptDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving receipts for purchase order {PurchaseOrderId}", id);
            return StatusCode(500, ApiResponse<List<PurchaseOrderReceiptDto>>.ErrorResponse("An error occurred retrieving receipts"));
        }
    }

    /// <summary>
    /// Confirm a receipt
    /// </summary>
    [HttpPost("receipts/{receiptId}/confirm")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> ConfirmReceipt(int receiptId)
    {
        try
        {
            var command = new ConfirmReceiptCommand(receiptId);
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse("Receipt not found"));
            }
            
            return Ok(ApiResponse<bool>.SuccessResponse(result));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot confirm receipt {ReceiptId}", receiptId);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming receipt {ReceiptId}", receiptId);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred confirming the receipt"));
        }
    }

    /// <summary>
    /// Perform 3-way matching validation (PO vs Receipt vs Invoice)
    /// </summary>
    [HttpPost("{id}/validate-invoice")]
    [ProducesResponseType(typeof(ApiResponse<ThreeWayMatchResult>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ThreeWayMatchResult>>> ValidateInvoice(
        int id,
        [FromBody] InvoiceValidationRequest request)
    {
        try
        {
            var result = await _threeWayMatchingService.PerformThreeWayMatchAsync(
                id,
                request.InvoiceTotal,
                request.LineItems,
                request.TolerancePercentage);
            
            return Ok(ApiResponse<ThreeWayMatchResult>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing 3-way match for purchase order {PurchaseOrderId}", id);
            return StatusCode(500, ApiResponse<ThreeWayMatchResult>.ErrorResponse("An error occurred performing 3-way match"));
        }
    }
}

public class RejectPurchaseOrderRequest
{
    public string Reason { get; set; } = string.Empty;
}

public class InvoiceValidationRequest
{
    public decimal InvoiceTotal { get; set; }
    public List<InvoiceLineItem> LineItems { get; set; } = new();
    public decimal TolerancePercentage { get; set; } = 5.0m;
}
