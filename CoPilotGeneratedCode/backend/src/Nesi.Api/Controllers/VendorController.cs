using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nesi.Application.Commands.Vendor;
using Nesi.Application.Common;
using Nesi.Application.DTOs.Vendor;
using Nesi.Application.Queries.Vendor;

namespace Nesi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendorController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<VendorController> _logger;

    public VendorController(IMediator mediator, ILogger<VendorController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all vendors with optional filtering and pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetVendorsQueryResult>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<GetVendorsQueryResult>>> GetVendors(
        [FromQuery] bool activeOnly = true,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var query = new GetVendorsQuery(activeOnly, pageNumber, pageSize);
            var result = await _mediator.Send(query);
            
            return Ok(ApiResponse<GetVendorsQueryResult>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vendors");
            return StatusCode(500, ApiResponse<GetVendorsQueryResult>.ErrorResponse("An error occurred retrieving vendors"));
        }
    }

    /// <summary>
    /// Get a specific vendor by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<VendorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<VendorDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<VendorDto>>> GetVendorById(int id)
    {
        try
        {
            var query = new GetVendorByIdQuery(id);
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound(ApiResponse<VendorDto>.ErrorResponse("Vendor not found"));
            }
            
            return Ok(ApiResponse<VendorDto>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving vendor {VendorId}", id);
            return StatusCode(500, ApiResponse<VendorDto>.ErrorResponse("An error occurred retrieving the vendor"));
        }
    }

    /// <summary>
    /// Create a new vendor
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<int>>> CreateVendor([FromBody] CreateVendorCommand command)
    {
        try
        {
            var vendorId = await _mediator.Send(command);
            return CreatedAtAction(
                nameof(GetVendorById),
                new { id = vendorId },
                ApiResponse<int>.SuccessResponse(vendorId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating vendor");
            return StatusCode(500, ApiResponse<int>.ErrorResponse("An error occurred creating the vendor"));
        }
    }

    /// <summary>
    /// Update an existing vendor
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateVendor(
        int id,
        [FromBody] UpdateVendorCommand command)
    {
        try
        {
            if (id != command.Id)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("ID mismatch"));
            }

            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse("Vendor not found"));
            }
            
            return Ok(ApiResponse<bool>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating vendor {VendorId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred updating the vendor"));
        }
    }
}
