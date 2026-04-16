using MediatR;
using Nesi.Application.DTOs.Quote;
using Nesi.Domain.Enums;
using System.Text.Json.Serialization;

namespace Nesi.Application.Commands.Quote;

public record CreateQuoteCommand(
    int CustomerId,
    QuoteType QuoteType,
    string Description,
    string Scope,
    DateTime EstimatedStartDate,
    DateTime EstimatedCompletionDate,
    int? ProjectManagerId,
    string TermsAndConditions,
    decimal TaxRate,
    [property: JsonPropertyName("lineItems")] List<CreateQuoteLineItemDto> LineItems) : IRequest<int>;

public class CreateQuoteLineItemDto
{
    public QuoteLineItemType ItemType { get; set; }
    public string Description { get; set; } = string.Empty;
    public int? JobTypeId { get; set; }
    public decimal EstimatedHours { get; set; }
    public string? PartNumber { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Notes { get; set; }
}
