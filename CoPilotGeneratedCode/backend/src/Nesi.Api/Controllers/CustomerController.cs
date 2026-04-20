using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nesi.Application.Commands.Customer;
using Nesi.Application.Common;
using Nesi.Application.DTOs.Customer;
using Nesi.Application.Queries.Customer;

namespace Nesi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(IMediator mediator, ILogger<CustomerController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all customers with optional filtering and pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetCustomersQueryResult>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<GetCustomersQueryResult>>> GetCustomers(
        [FromQuery] bool activeOnly = true,
        [FromQuery] int? businessUnitId = null,
        [FromQuery] int? accountManagerId = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var query = new GetCustomersQuery(activeOnly, businessUnitId, accountManagerId, pageNumber, pageSize);
            var result = await _mediator.Send(query);
            
            return Ok(ApiResponse<GetCustomersQueryResult>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customers");
            return StatusCode(500, ApiResponse<GetCustomersQueryResult>.ErrorResponse("An error occurred retrieving customers"));
        }
    }

    /// <summary>
    /// Get a specific customer by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomerById(int id, [FromQuery] bool includeDetails = false)
    {
        try
        {
            var query = new GetCustomerByIdQuery(id, includeDetails);
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound(ApiResponse<CustomerDto>.ErrorResponse("Customer not found"));
            }
            
            return Ok(ApiResponse<CustomerDto>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer {CustomerId}", id);
            return StatusCode(500, ApiResponse<CustomerDto>.ErrorResponse("An error occurred retrieving the customer"));
        }
    }

    /// <summary>
    /// Search customers by search term
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<List<CustomerDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<CustomerDto>>>> SearchCustomers([FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest(ApiResponse<List<CustomerDto>>.ErrorResponse("Search term is required"));
            }

            var query = new SearchCustomersQuery(searchTerm);
            var result = await _mediator.Send(query);
            
            return Ok(ApiResponse<List<CustomerDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching customers with term: {SearchTerm}", searchTerm);
            return StatusCode(500, ApiResponse<List<CustomerDto>>.ErrorResponse("An error occurred searching customers"));
        }
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<int>>> CreateCustomer([FromBody] CreateCustomerCommand command)
    {
        try
        {
            var customerId = await _mediator.Send(command);
            
            return CreatedAtAction(
                nameof(GetCustomerById),
                new { id = customerId },
                ApiResponse<int>.SuccessResponse(customerId, "Customer created successfully"));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Failed to create customer - validation error");
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to create customer");
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            return StatusCode(500, ApiResponse<int>.ErrorResponse("An error occurred creating the customer"));
        }
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateCustomer(int id, [FromBody] UpdateCustomerCommand command)
    {
        try
        {
            if (id != command.Id)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Customer ID mismatch"));
            }

            await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Customer updated successfully"));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Failed to update customer {CustomerId} - validation error", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to update customer {CustomerId}", id);
            return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating customer {CustomerId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred updating the customer"));
        }
    }

    /// <summary>
    /// Delete a customer (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCustomer(int id)
    {
        try
        {
            var command = new DeleteCustomerCommand(id);
            await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Customer deleted successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to delete customer {CustomerId}", id);
            return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting customer {CustomerId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred deleting the customer"));
        }
    }

    /// <summary>
    /// Activate a customer
    /// </summary>
    [HttpPost("{id}/activate")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> ActivateCustomer(int id)
    {
        try
        {
            var command = new ActivateCustomerCommand(id);
            await _mediator.Send(command);
            
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Customer activated successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to activate customer {CustomerId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating customer {CustomerId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred activating the customer"));
        }
    }

    /// <summary>
    /// Add an address to a customer
    /// </summary>
    [HttpPost("{id}/addresses")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<int>>> AddCustomerAddress(int id, [FromBody] AddCustomerAddressCommand command)
    {
        try
        {
            if (id != command.CustomerId)
            {
                return BadRequest(ApiResponse<int>.ErrorResponse("Customer ID mismatch"));
            }

            var addressId = await _mediator.Send(command);
            
            return CreatedAtAction(
                nameof(GetCustomerById),
                new { id, includeDetails = true },
                ApiResponse<int>.SuccessResponse(addressId, "Customer address added successfully"));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Failed to add address to customer {CustomerId} - validation error", id);
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to add address to customer {CustomerId}", id);
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding address to customer {CustomerId}", id);
            return StatusCode(500, ApiResponse<int>.ErrorResponse("An error occurred adding the customer address"));
        }
    }

    /// <summary>
    /// Add a contact to a customer
    /// </summary>
    [HttpPost("{id}/contacts")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<int>>> AddCustomerContact(int id, [FromBody] AddCustomerContactCommand command)
    {
        try
        {
            if (id != command.CustomerId)
            {
                return BadRequest(ApiResponse<int>.ErrorResponse("Customer ID mismatch"));
            }

            var contactId = await _mediator.Send(command);
            
            return CreatedAtAction(
                nameof(GetCustomerById),
                new { id, includeDetails = true },
                ApiResponse<int>.SuccessResponse(contactId, "Customer contact added successfully"));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Failed to add contact to customer {CustomerId} - validation error", id);
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to add contact to customer {CustomerId}", id);
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding contact to customer {CustomerId}", id);
            return StatusCode(500, ApiResponse<int>.ErrorResponse("An error occurred adding the customer contact"));
        }
    }

    /// <summary>
    /// Add a note to a customer
    /// </summary>
    [HttpPost("{id}/notes")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<int>>> AddCustomerNote(int id, [FromBody] AddCustomerNoteCommand command)
    {
        try
        {
            if (id != command.CustomerId)
            {
                return BadRequest(ApiResponse<int>.ErrorResponse("Customer ID mismatch"));
            }

            var noteId = await _mediator.Send(command);
            
            return CreatedAtAction(
                nameof(GetCustomerById),
                new { id, includeDetails = true },
                ApiResponse<int>.SuccessResponse(noteId, "Customer note added successfully"));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Failed to add note to customer {CustomerId} - validation error", id);
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Failed to add note to customer {CustomerId}", id);
            return BadRequest(ApiResponse<int>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding note to customer {CustomerId}", id);
            return StatusCode(500, ApiResponse<int>.ErrorResponse("An error occurred adding the customer note"));
        }
    }
}
