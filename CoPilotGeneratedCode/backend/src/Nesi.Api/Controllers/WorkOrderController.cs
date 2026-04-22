using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nesi.Application.Commands.WorkOrder;
using Nesi.Application.Common;
using Nesi.Application.DTOs.WorkOrder;
using Nesi.Application.Queries.WorkOrder;
using Nesi.Application.Services;

namespace Nesi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkOrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<WorkOrderController> _logger;
    private readonly IExportService _exportService;

    public WorkOrderController(IMediator mediator, ILogger<WorkOrderController> logger, IExportService exportService)
    {
        _mediator = mediator;
        _logger = logger;
        _exportService = exportService;
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

    /// <summary>
    /// Get a specific work order by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<WorkOrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<WorkOrderDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<WorkOrderDto>>> GetWorkOrderById(int id)
    {
        try
        {
            var query = new GetWorkOrderByIdQuery(id);
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound(ApiResponse<WorkOrderDto>.ErrorResponse("Work order not found"));
            }
            
            return Ok(ApiResponse<WorkOrderDto>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving work order {WorkOrderId}", id);
            return StatusCode(500, ApiResponse<WorkOrderDto>.ErrorResponse("An error occurred retrieving the work order"));
        }
    }

    /// <summary>
    /// Assign a project manager to a work order
    /// </summary>
    [HttpPost("{id}/assign-manager")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> AssignProjectManager(
        int id,
        [FromBody] AssignProjectManagerRequest request)
    {
        try
        {
            var command = new AssignProjectManagerCommand(
                id,
                request.ProjectManagerId,
                request.ScheduledStartDate,
                request.ScheduledEndDate);
            
            await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Project manager assigned successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to assign project manager to work order {WorkOrderId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning project manager to work order {WorkOrderId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred assigning the project manager"));
        }
    }

    /// <summary>
    /// Assign a technician to a work order
    /// </summary>
    [HttpPost("{id}/assign-technician")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<int>>> AssignTechnician(
        int id,
        [FromBody] AssignTechnicianRequest request)
    {
        try
        {
            // TODO: Get userId from authenticated user claims
            var userId = int.Parse(Request.Headers["X-User-Id"].FirstOrDefault() ?? "1");
            
            var command = new AssignTechnicianCommand(
                id,
                request.TechnicianId,
                userId,
                request.Role,
                request.Notes);
            
            var assignmentId = await _mediator.Send(command);
            
            return CreatedAtAction(
                nameof(GetWorkOrders),
                new { id = assignmentId },
                ApiResponse<int>.SuccessResponse(assignmentId, "Technician assigned successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning technician to work order {WorkOrderId}", id);
            return StatusCode(500, ApiResponse<int>.ErrorResponse("An error occurred assigning the technician"));
        }
    }

    /// <summary>
    /// Add material to a work order
    /// </summary>
    [HttpPost("{id}/materials")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<int>>> AddMaterial(
        int id,
        [FromBody] AddMaterialRequest request)
    {
        try
        {
            var command = new AddMaterialCommand(
                id,
                request.PartNumber,
                request.Description,
                request.Quantity,
                request.UnitCost,
                request.PurchaseOrderNumber,
                request.Supplier,
                request.Notes);
            
            var materialId = await _mediator.Send(command);
            
            return CreatedAtAction(
                nameof(GetWorkOrders),
                new { id = materialId },
                ApiResponse<int>.SuccessResponse(materialId, "Material added successfully"));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Failed to add material to work order {WorkOrderId}", id);
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding material to work order {WorkOrderId}", id);
            return StatusCode(500, ApiResponse<int>.ErrorResponse("An error occurred adding the material"));
        }
    }

    /// <summary>
    /// Complete a work order
    /// </summary>
    [HttpPost("{id}/complete")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> CompleteWorkOrder(
        int id,
        [FromBody] CompleteWorkOrderRequest request)
    {
        try
        {
            // TODO: Get userId from authenticated user claims
            var userId = int.Parse(Request.Headers["X-User-Id"].FirstOrDefault() ?? "1");
            
            var command = new CompleteWorkOrderCommand(id, userId, request.CompletionNotes);
            await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Work order completed successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to complete work order {WorkOrderId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing work order {WorkOrderId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred completing the work order"));
        }
    }

    /// <summary>
    /// Generate invoice for a work order
    /// </summary>
    [HttpPost("{id}/invoice")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> GenerateInvoice(
        int id,
        [FromBody] GenerateInvoiceRequest request)
    {
        try
        {
            var command = new GenerateInvoiceCommand(id, request.InvoiceAmount);
            await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Invoice generated successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to generate invoice for work order {WorkOrderId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid invoice amount for work order {WorkOrderId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating invoice for work order {WorkOrderId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred generating the invoice"));
        }
    }

    /// <summary>
    /// Upload a document to a work order
    /// </summary>
    [HttpPost("{id}/documents")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<int>>> UploadDocument(
        int id,
        [FromForm] IFormFile file,
        [FromForm] string documentType,
        [FromForm] string? description)
    {
        try
        {
            var command = new UploadDocumentCommand(id, file, documentType, description);
            var documentId = await _mediator.Send(command);
            
            return CreatedAtAction(
                nameof(GetWorkOrderDocuments),
                new { id },
                ApiResponse<int>.SuccessResponse(documentId, "Document uploaded successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading document to work order {WorkOrderId}", id);
            return StatusCode(500, ApiResponse<int>.ErrorResponse("An error occurred uploading the document"));
        }
    }

    /// <summary>
    /// Get all documents for a work order
    /// </summary>
    [HttpGet("{id}/documents")]
    [ProducesResponseType(typeof(ApiResponse<List<WorkOrderDocumentDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<WorkOrderDocumentDto>>>> GetWorkOrderDocuments(int id)
    {
        try
        {
            var query = new GetWorkOrderDocumentsQuery(id);
            var result = await _mediator.Send(query);
            
            return Ok(ApiResponse<List<WorkOrderDocumentDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving documents for work order {WorkOrderId}", id);
            return StatusCode(500, ApiResponse<List<WorkOrderDocumentDto>>.ErrorResponse("An error occurred retrieving documents"));
        }
    }

    /// <summary>
    /// Update work order progress and milestones
    /// </summary>
    [HttpPut("{id}/progress")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateProgress(
        int id,
        [FromBody] UpdateProgressRequest request)
    {
        try
        {
            var command = new UpdateProgressCommand(id, request.Milestones, request.PercentComplete);
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse("Work order not found"));
            }
            
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Progress updated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating progress for work order {WorkOrderId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred updating progress"));
        }
    }

    /// <summary>
    /// Export work orders to PDF
    /// </summary>
    [HttpGet("export/pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportWorkOrdersToPdf()
    {
        try
        {
            var query = new GetWorkOrdersQuery();
            var result = await _mediator.Send(query);
            
            var columns = new Dictionary<string, string>
            {
                { "WorkOrderNumber", "WO #" },
                { "CustomerName", "Customer" },
                { "Description", "Description" },
                { "Status", "Status" },
                { "ScheduledStartDate", "Start Date" },
                { "ScheduledEndDate", "End Date" },
                { "EstimatedCost", "Est. Cost" },
                { "ActualCost", "Actual Cost" }
            };

            var pdfBytes = _exportService.ExportToPdf(result, "Work Orders List", columns);
            return File(pdfBytes, "application/pdf", $"WorkOrders_{DateTime.Now:yyyyMMdd}.pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting work orders to PDF");
            return StatusCode(500, "An error occurred exporting work orders");
        }
    }

    /// <summary>
    /// Export work orders to Excel
    /// </summary>
    [HttpGet("export/excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportWorkOrdersToExcel()
    {
        try
        {
            var query = new GetWorkOrdersQuery();
            var result = await _mediator.Send(query);
            
            var columns = new Dictionary<string, string>
            {
                { "WorkOrderNumber", "WO #" },
                { "CustomerName", "Customer" },
                { "Description", "Description" },
                { "Status", "Status" },
                { "Priority", "Priority" },
                { "ScheduledStartDate", "Start Date" },
                { "ScheduledEndDate", "End Date" },
                { "EstimatedCost", "Est. Cost" },
                { "ActualCost", "Actual Cost" },
                { "PercentComplete", "% Complete" }
            };

            var excelBytes = _exportService.ExportToExcel(result, "Work Orders", columns);
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"WorkOrders_{DateTime.Now:yyyyMMdd}.xlsx");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting work orders to Excel");
            return StatusCode(500, "An error occurred exporting work orders");
        }
    }
}

// Request DTOs
public record AssignProjectManagerRequest(
    int ProjectManagerId,
    DateTime? ScheduledStartDate,
    DateTime? ScheduledEndDate);

public record AssignTechnicianRequest(
    int TechnicianId,
    string? Role,
    string? Notes);

public record AddMaterialRequest(
    string PartNumber,
    string Description,
    decimal Quantity,
    decimal UnitCost,
    string? PurchaseOrderNumber,
    string? Supplier,
    string? Notes);

public record CompleteWorkOrderRequest(string? CompletionNotes);

public record GenerateInvoiceRequest(decimal InvoiceAmount);

public record UpdateProgressRequest(string Milestones, int? PercentComplete);
